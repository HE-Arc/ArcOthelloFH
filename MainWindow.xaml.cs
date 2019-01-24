using Microsoft.Win32;
using Othello.Data;
using Othello.UI;
using System.Windows;
using System.Windows.Resources;
using System.Media;
using System;

namespace Othello
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainMenuUI menuUi = new MainMenuUI();
        private GameUI gameUi = null;
        private SoundPlayer soundPlayer;

        public MainWindow()
        {
            InitializeComponent();

            grid_main.Children.Add(menuUi);
            soundPlayer = new SoundPlayer();
        }

        public void LaunchMainMenu()
        {
            grid_main.Children.Remove(gameUi);
            grid_main.Children.Add(menuUi);
        }

        public void LaunchShowGame(OthelloLogic logic = null)
        {
            gameUi = new GameUI(logic);

            grid_main.Children.Remove(menuUi);
            grid_main.Children.Add(gameUi);
    
        }

        public void LoadSaveGame()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "othello save files (*.oth)|*.oth";
            OthelloLogic dataSave = null;

            if (dialog.ShowDialog() == true)
            {
                dataSave = (OthelloLogic)Tools.DeserializeFromFile(dialog.FileName);
                dataSave.InitTimer();
                this.LaunchShowGame(dataSave);
            }
        }

        public void SaveGame(OthelloLogic logic)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "othello save files (*.oth)|*.oth";

            if (dialog.ShowDialog() == true)
            {
                Tools.SerializeToFile(dialog.FileName, logic);
            }
        }

        public void Quit()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OnLoadClicked(object sender, RoutedEventArgs e)
        {
            LoadSaveGame();
        }

        public void TurnMusicOff()
        {
            soundPlayer.Stop();
        }

        public void TurnMusicOn()
        {
            StreamResourceInfo sri = Application.GetResourceStream(new Uri(@"pack://application:,,,/Othello;component/Sound/lelelele.wav", UriKind.Absolute));
            soundPlayer.Stream = sri.Stream;
            soundPlayer.LoadAsync();
            soundPlayer.PlayLooping();
        }
    }
}

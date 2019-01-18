using Microsoft.Win32;
using Othello.Data;
using Othello.UI;
using System.Windows;
using System.Windows.Controls;

namespace Othello
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainMenuUI menuUi = new MainMenuUI();
        private GameUI gameUi = null;

        public MainWindow()
        {
            InitializeComponent();

            grid_main.Children.Add(menuUi);
        }

        public void LaunchMainMenu()
        {
            grid_main.Children.Remove(gameUi);
            grid_main.Children.Add(menuUi);
        }

        public void LaunchShowGame(OthelloBoardLogic logic = null)
        {
            gameUi = new GameUI(logic);

            grid_main.Children.Remove(menuUi);
            grid_main.Children.Add(gameUi);
        }

        public void LoadSaveGame()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "othello save files (*.oth)|*.oth";
            OthelloBoardLogic dataSave = null;

            if (dialog.ShowDialog() == true)
            {
                dataSave = (OthelloBoardLogic)Tools.DeserializeFromFile(dialog.FileName);
                dataSave.InitTimer();
                this.LaunchShowGame(dataSave);
            }
        }

        public void SaveGame(OthelloBoardLogic logic)
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
    }
}

using Microsoft.Win32;
using Othello;
using Othello.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Othello.UI
{
    /// <summary>
    /// Logique d'interaction pour MainMenu.xaml
    /// </summary>
    public partial class MainMenuUI : UserControl
    {
        public MainMenuUI()
        {
            InitializeComponent();

            PlayerData data = new PlayerData();
            data.NumberOfPawns = 55;

            Tools.SerializeToFile("testfile", data);
            PlayerData data2 = (PlayerData)Tools.DeserializeFromFile("testfile");

            Console.WriteLine(data2.NumberOfPawns);
        }

        private void OnLoadClicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.LoadSaveGame();
        }

        private void OnQuitClicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Quit();
        }

        private void btn_pvp_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.LaunchShowGame(new OthelloLogic());
        }

        private void btn_pvia_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.LaunchShowGame(new OthelloLogic());
        }

        private void btn_quit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Quit();
        }

        private void btn_load_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            OpenFileDialog dialog = new OpenFileDialog();
            OthelloLogic dataSave = null;

            if (dialog.ShowDialog() == true)
            {
                dataSave = (OthelloLogic)Tools.DeserializeFromFile(dialog.FileName);
                dataSave.InitTimer();
                mainWindow.LaunchShowGame(dataSave);
            }
        }
    }
}

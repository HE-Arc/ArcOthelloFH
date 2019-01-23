/// ----------------------------------------------------------------------------------
/// Othello
/// 
/// Course : C#
/// Authors: Malik Fleury, Thibault Haldenwang
/// ----------------------------------------------------------------------------------

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
        }

        /// <summary>
        /// Load game from file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadClicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.LoadSaveGame();
        }

        /// <summary>
        /// Quit the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnQuitClicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Quit();
        }

        /// <summary>
        /// Launch game : player vs player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPvpClicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.LaunchShowGame(new OthelloLogic());
        }

        /// <summary>
        /// Launch game : player vs ia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPviaClicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.LaunchShowGame(new OthelloLogic());
        }
    }
}

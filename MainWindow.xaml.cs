using Othello.Data;
using Othello.UI;
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

namespace Orthello
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainMenuUI menuUi = new MainMenuUI();

        public MainWindow()
        {
            InitializeComponent();

            LaunchMainMenu();
        }

        public void LaunchMainMenu()
        {
            grid_main.Children.Clear();
            grid_main.Children.Add(menuUi);
        }

        public void LaunchShowGame(OthelloBoardLogic logic = null)
        {
            GameUI gameUi = new GameUI(logic);
            grid_main.Children.Clear();
            grid_main.Children.Add(gameUi);
        }
    }
}

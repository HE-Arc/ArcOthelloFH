using System;
using System.Windows.Controls;

namespace Othello.UI
{
    /// <summary>
    /// Logique d'interaction pour GameUI.xaml
    /// </summary>
    public partial class GameUI : UserControl
    {
        private OthelloGrid grid;

        public GameUI()
        {
            InitializeComponent();

            grid = new OthelloGrid(6, 7);
            MainDockPanel.Children.Add(grid);
        }
    }
}

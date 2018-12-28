using Othello.Data;
using System;
using System.Windows.Controls;

namespace Othello.UI
{
    /// <summary>
    /// Logique d'interaction pour GameUI.xaml
    /// </summary>
    public partial class GameUI : UserControl
    {
        private OthelloLogic logic;
        private OthelloGrid grid;

        public GameUI()
        {
            InitializeComponent();

            logic = new OthelloLogic();
            grid = new OthelloGrid(logic);

            MainDockPanel.Children.Add(grid);
        }
    }
}

using Othello.Data;
using System;
using System.Collections.Generic;
using System.Windows;
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

            // Prepare data
            logic = new OthelloLogic();

            // Prepare the grid
            grid = new OthelloGrid(logic);
            MainDockPanel.Children.Add(grid);

            // Add the events
            game_controls_ui.EventEndTurnPressed += endOfTurnPressed;

            executeBefore();
        }

        public void executeBefore()
        {
            IntPosition currentPosition = new IntPosition(3, 3);
            List<IntPosition> directionsList = logic.GetNeighborsDirections(currentPosition);
            List<IntPosition> movements = new List<IntPosition>();

            foreach (IntPosition direction in directionsList)
            {
                if (logic.IsPossibleMove(currentPosition, direction, movements))
                {
                    IntPosition possibleMove = movements[movements.Count - 1];
                    grid.MarkSlot(possibleMove);
                }
                movements.Clear();
            }
        }

        public void endOfTurnPressed(object sender, EventArgs e)
        {

        }
    }
}

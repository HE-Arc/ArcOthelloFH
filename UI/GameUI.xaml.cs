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
            grid = new OthelloGrid(new IntPosition(logic.Rows, logic.Columns));
            MainDockPanel.Children.Add(grid);

            // Add the events
            game_controls_ui.EventEndTurnClicked += OnEndOfTurnClicked;

            //Init the game
            InitGame();

            // Execute 
            ExecuteBefore();
        }

        public void InitGame()
        {
            // Update the visual of the grid with data
            for(int row = 0;row < logic.Rows; row++)
            {
                for(int column = 0;column < logic.Columns; column++)
                {
                    grid.SlotsArray[row, column].SetContent((SlotContent)logic.GameBoard[row, column]);
                }
            }

            // Prepare the first turn for black pawns
        }

        public void PrepareNextTurn()
        {

        }

        public void ExecuteBefore()
        {
            IntPosition currentPosition = new IntPosition(3, 3);
            List<IntPosition> directionsList = logic.GetNeighborsDirections(currentPosition);
            List<IntPosition> movements = new List<IntPosition>();

            foreach (IntPosition direction in directionsList)
            {
                if (logic.IsPossibleMove(currentPosition, direction, movements))
                {
                    IntPosition possibleMove = movements[movements.Count - 1];
                    grid.SlotsArray[possibleMove.Row, possibleMove.Column].Mark();
                }
                movements.Clear();
            }
        }

        public void OnEndOfTurnClicked(object sender, EventArgs e)
        {

        }
    }
}

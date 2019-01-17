using Microsoft.Win32;
using Othello;
using Othello.UI;
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
        private OthelloBoardLogic logic;
        private OthelloGrid grid;

        public GameUI(OthelloBoardLogic logic = null)
        {
            InitializeComponent();

            // Prepare data
            if(logic == null)
            {
                this.logic = new OthelloBoardLogic();
            }
            else
            {
                this.logic = logic;
            }

            // Define data context for each panel of the players
            this.player_ui_left.DataContext = this.logic.GetWhitePlayerData();
            this.player_ui_right.DataContext = this.logic.GetBlackPlayerData();

            // Prepare the grid
            grid = new OthelloGrid(new IntPosition(this.logic.Rows, this.logic.Columns));
            MainDockPanel.Children.Add(grid);

            // Events
            grid.EventSlotClicked += OnSlotClicked;

            // Init the game
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

        public void ClearBoardMarks()
        {
            for (int row = 0; row < logic.Rows; row++)
            {
                for (int column = 0; column < logic.Columns; column++)
                {
                    grid.SlotsArray[row, column].Unmark();
                }
            }
        }

        public void PrepareNextTurn()
        {
            ClearBoardMarks();
            logic.UpdatePlayersScore();
            logic.SwitchPlayer();
            ExecuteBefore();
        }

        public void ExecuteBefore()
        {
            IntPosition currentPosition = new IntPosition(3, 3);
            //List<IntPosition> directionsList = logic.GetNeighborsDirections(currentPosition);
            List<IntPosition> directionsList = logic.GetAllPossibleMoves();
            List<IntPosition> movements = new List<IntPosition>();

            /*
            foreach (IntPosition direction in directionsList)
            {
                if (logic.IsPossibleMove(currentPosition, direction, movements))
                {
                    IntPosition possibleMove = movements[movements.Count - 1];
                    grid.SlotsArray[possibleMove.Row, possibleMove.Column].Mark();
                }
                movements.Clear();
            }
            */
            foreach (IntPosition possibleMove in directionsList)
            {
                grid.SlotsArray[possibleMove.Row, possibleMove.Column].Mark();
            }

            if(directionsList.Count == 0)
            {
                PrepareNextTurn();
            }
        }

        public void OnEndOfTurnClicked(object sender, EventArgs e)
        {
            
        }

        public void OnSlotClicked(object sender, EventArgs e)
        {
            Slot slot = sender as Slot;
            IntPosition position = slot.GetPosition();

            if (grid.SlotsArray[position.Row, position.Column].IsMarked())
            {
                logic.GameBoard[position.Row, position.Column] = (int)logic.PlayerTurn;
                grid.SlotsArray[position.Row, position.Column].SetContent((SlotContent)logic.PlayerTurn);

                List<IntPosition> pawnsToFlip = logic.GetPawnsToFlip(position);
                foreach (IntPosition pawnPosition in pawnsToFlip)
                {
                    logic.GameBoard[pawnPosition.Row, pawnPosition.Column] = (int)logic.PlayerTurn;
                    grid.SlotsArray[pawnPosition.Row, pawnPosition.Column].SetContent((SlotContent)logic.PlayerTurn);
                }

                PrepareNextTurn();
            }
        }

        private void OnLoadClicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.LoadSaveGame();
        }

        private void item_save_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.SaveGame(logic);
        }

        private void item_quit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Quit();
        }
    }
}

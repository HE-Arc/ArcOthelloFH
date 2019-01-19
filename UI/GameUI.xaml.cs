using Microsoft.Win32;
using Othello;
using Othello.UI;
using Othello.Data;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Othello.UI
{
    /// <summary>
    /// Logique d'interaction pour GameUI.xaml
    /// </summary>
    public partial class GameUI : UserControl
    {
        private OthelloBoardLogic logic;
        private OthelloGrid grid;
        private bool hasPlayed = false;

        public GameUI(OthelloBoardLogic logic = null)
        {
            InitializeComponent();

            // Prepare data
            if (logic == null)
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
            this.game_controls_ui.EventUndoClicked += OnUndoClicked;
            this.game_controls_ui.EventEndTurnClicked += OnEndOfTurnClicked;

            // Init the game
            InitGame();

            // Execute 
            ExecuteBefore();
        }

        public void InitGame()
        {
            // Update the visual of the grid with data
            for (int row = 0; row < logic.Rows; row++)
            {
                for (int column = 0; column < logic.Columns; column++)
                {
                    grid.SlotsArray[row, column].SetContent((SlotContent)logic.GameBoard[row, column]);
                }
            }

            // Prepare the first turn for black pawns
        }

        /// <summary>
        /// Clears the board from any possible move mark (red background)
        /// </summary>
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
        }

        /// <summary>
        /// Mark any possible move on the board for the current player and 
        /// check if the player has to skip his turn
        /// </summary>
        public void ExecuteBefore()
        {
            IntPosition currentPosition = new IntPosition(3, 3);
            List<IntPosition> directionsList = logic.GetAllPossibleMoves();
            List<IntPosition> movements = new List<IntPosition>();
            PlayerData currentPlayerData = logic.GetPlayerData(logic.PlayerTurn);

            foreach (IntPosition possibleMove in directionsList)
            {
                grid.SlotsArray[possibleMove.Row, possibleMove.Column].Mark();
            }

            //skip turn if there's no move available
            if (directionsList.Count == 0)
            {
                PlayerData oppositePlayerData = logic.GetPlayerData(logic.GetOppositePlayer(logic.PlayerTurn));
                currentPlayerData.HasSkippedLastTurn = true;
                if (currentPlayerData.HasSkippedLastTurn == oppositePlayerData.HasSkippedLastTurn)
                {
                    //nobody can perform a move anymore, game ends
                    EndGame();
                }
                else
                {
                    PrepareNextTurn();
                }
            }
            else
            {
                currentPlayerData.HasSkippedLastTurn = false;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public void EndGame()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            PlayerData whitePlayerData = logic.GetWhitePlayerData();
            PlayerData blackPlayerData = logic.GetBlackPlayerData();
            String message;

            ClearBoardMarks();
            logic.UpdatePlayersScore();

            if(whitePlayerData.NumberOfPawns > blackPlayerData.NumberOfPawns)
            {
                message = "White player won";
            }
            else if(whitePlayerData.NumberOfPawns < blackPlayerData.NumberOfPawns)
            {
                message = "Black player won";
            }
            else
            {
                message = "Ex aequo";
            }

            MessageBox.Show(message, "Game is over");
            mainWindow.LaunchMainMenu();
        }

        public void OnEndOfTurnClicked(object sender, EventArgs e)
        {
            if(hasPlayed)
            {
                hasPlayed = false;
                logic.SwitchPlayer();
                ExecuteBefore();
            }
        }

        /// <summary>
        /// Updates both the logic and the UI to match the selected move.
        /// Each move is stored in the logic and can be played backwards.
        /// </summary>
        /// <param name="slotPosition"> the coordinates of the selected grid slot</param>
        public void PlayMove(IntPosition slotPosition)
        {
            if (grid.SlotsArray[slotPosition.Row, slotPosition.Column].IsMarked())
            {
                List<IntPosition> pawnsToFlip = logic.GetPawnsToFlip(slotPosition);
                pawnsToFlip.Add(slotPosition);

                foreach (IntPosition pawnPosition in pawnsToFlip)
                {
                    grid.SlotsArray[pawnPosition.Row, pawnPosition.Column].SetContent((SlotContent)logic.PlayerTurn);
                }

                logic.UpdateSlots(pawnsToFlip);

                PrepareNextTurn();
            }
        }

        /// <summary>
        /// Play the most recent move backwards and update the logic and UI accordingly.
        /// </summary>
        public void UndoMove()
        {
            var lastMove = logic.UndoMove();
            List<IntPosition> pawnsToFlip = lastMove.Item1;
            Player moveAuthor = lastMove.Item2;
            IntPosition pawnToRemove = lastMove.Item3;

            grid.SlotsArray[pawnToRemove.Row, pawnToRemove.Column].SetContent(SlotContent.Nothing);
            foreach (var pawnPosition in pawnsToFlip)
            {
                grid.SlotsArray[pawnPosition.Row, pawnPosition.Column].SetContent((SlotContent)logic.GetOppositePlayer(moveAuthor));
            }

            PrepareNextTurn();
        }

        public void OnUndoClicked(object sender, EventArgs e)
        {
            try
            {
                if(hasPlayed)   //check if the player performed any action before allowing to undo
                {
                    UndoMove();
                    hasPlayed = false;
                    ExecuteBefore();
                }
            }
            catch (Exception)
            {

            }
        }

        public void OnSlotClicked(object sender, EventArgs e)
        {
            Slot slot = sender as Slot;
            IntPosition position = slot.GetPosition();
            hasPlayed = true;
            PlayMove(position);
        }

        private void OnLoadClicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.LoadSaveGame();
        }

        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.SaveGame(logic);
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

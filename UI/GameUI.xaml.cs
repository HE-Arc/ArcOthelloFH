/// ----------------------------------------------------------------------------------
/// Othello
/// 
/// Course : C#
/// Authors: Malik Fleury, Thibault Haldenwang
/// ----------------------------------------------------------------------------------


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
        private OthelloLogic logic;
        private OthelloGrid grid;
        private bool hasPlayed = false;

        public GameUI(OthelloLogic logic = null)
        {
            InitializeComponent();

            // Prepare data
            if (logic == null)
            {
                this.logic = new OthelloLogic();
            }
            else
            {
                this.logic = logic;
            }

            // Define data context for each panel of the players
            this.player_ui_top.DataContext = this.logic.GetBlackPlayerData();
            this.player_ui_bot.DataContext = this.logic.GetWhitePlayerData();

            // Prepare the grid
            grid = new OthelloGrid(new IntPosition(this.logic.Columns, this.logic.Rows));
            Viewbox box = new Viewbox();
            box.Child = grid;
            box.Stretch = System.Windows.Media.Stretch.Uniform;
            main_dock_panel.Children.Add(box);

            // Events
            grid.EventSlotClicked += OnSlotClicked;

            // Init the game
            InitGame();

            // Execute 
            ExecuteBefore();
            ShowCurrentPlayerTurn();
        }


        /// <summary>
        /// Init the game : update the grid and change style of playerui
        /// </summary>
        public void InitGame()
        {
            // Update the visual of the grid with data
            for (int row = 0; row < logic.Rows; row++)
            {
                for (int column = 0; column < logic.Columns; column++)
                {
                    grid.SlotsArray[column, row].SetContent((SlotContent)logic.GameBoard[column, row]);
                }
            }

            // Prepare the first turn for black pawns
            // Update background for players
            this.player_ui_top.ChangeStyle(Player.Black);
            this.player_ui_bot.ChangeStyle(Player.White);
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
                    grid.SlotsArray[column, row].Unmark();
                }
            }
        }

        /// <summary>
        /// Display the player who is playing
        /// </summary>
        public void ShowCurrentPlayerTurn()
        {
            if(logic.PlayerTurn == Player.White)
            {
                player_ui_top.Opacity = 0.2;
                player_ui_bot.Opacity = 1.0;
            }
            else if(logic.PlayerTurn == Player.Black)
            {
                player_ui_top.Opacity = 1.0;
                player_ui_bot.Opacity = 0.2;
            }
        }

        /// <summary>
        /// Prepare all the stuff for the next turn
        /// </summary>
        public void PrepareNextTurn()
        {
            ClearBoardMarks();
            logic.UpdatePlayersScore();
        }

        /// <summary>
        /// Check if there is any playable slot and skip or end game if there is none
        /// </summary>
        public void ExecuteBefore()
        {
            List<IntPosition> directionsList = logic.GetAllPossibleMoves();
            if(directionsList.Count == 0)
            {
                if (logic.IsGameFinished)
                {
                    EndGame();
                }
                else
                {
                    PrepareNextTurn();
                    logic.SwitchPlayer();
                    ExecuteBefore();
                    ShowCurrentPlayerTurn();
                }
            }
            else
            {
                foreach (IntPosition possibleMove in directionsList)
                {
                    grid.SlotsArray[possibleMove.Column, possibleMove.Row].Mark();
                }
            }
        }

        /// <summary>
        /// Compare score and print who win the game. Return to main menu.
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

        /// <summary>
        /// End player turn.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnEndOfTurnClicked(object sender, EventArgs e)
        {
            if(hasPlayed)
            {
                hasPlayed = false;
                logic.SwitchPlayer();
                ExecuteBefore();
                ShowCurrentPlayerTurn();
            }
        }

        /// <summary>
        /// Updates both the logic and the UI to match the selected move.
        /// Each move is stored in the logic and can be played backwards.
        /// </summary>
        /// <param name="slotPosition"> the coordinates of the selected grid slot</param>
        public void PlayMove(IntPosition slotPosition)
        {
            if (grid.SlotsArray[slotPosition.Column, slotPosition.Row].IsMarked())
            {
                List<IntPosition> pawnsToFlip = logic.GetPawnsToFlip(slotPosition);
                pawnsToFlip.Add(slotPosition);

                foreach (IntPosition pawnPosition in pawnsToFlip)
                {
                    grid.SlotsArray[pawnPosition.Column, pawnPosition.Row].SetContent((SlotContent)logic.PlayerTurn);
                }

                            hasPlayed = true;

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

            grid.SlotsArray[pawnToRemove.Column, pawnToRemove.Row].SetContent(SlotContent.Nothing);
            foreach (var pawnPosition in pawnsToFlip)
            {
                grid.SlotsArray[pawnPosition.Column, pawnPosition.Row].SetContent((SlotContent)logic.GetOppositePlayer(moveAuthor));
            }

            PrepareNextTurn();
        }

        /// <summary>
        /// Execute undo
        /// </summary>
        /// <param name="sender">Component who sent the event</param>
        /// <param name="e">Event</param>
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

        /// <summary>
        /// Execute stuff when a slot is clicked
        /// </summary>
        /// <param name="sender">Component who sent the event</param>
        /// <param name="e">Event</param>
        public void OnSlotClicked(object sender, EventArgs e)
        {
            Slot slot = sender as Slot;
            IntPosition position = slot.GetPosition();
            PlayMove(position);
        }

        /// <summary>
        /// Load game from file
        /// </summary>
        /// <param name="sender">Component who sent the event</param>
        /// <param name="e">Event</param>
        private void OnLoadClicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.LoadSaveGame();
        }

        /// <summary>
        /// Save the current game to a file
        /// </summary>
        /// <param name="sender">Component who sent the event</param>
        /// <param name="e">Event</param>
        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.SaveGame(logic);
        }

        /// <summary>
        /// Quit the game
        /// </summary>
        /// <param name="sender">Component who sent the event</param>
        /// <param name="e">Event</param>
        private void OnQuitClicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.Quit();
        }

        /// <summary>
        /// Go to main menu
        /// </summary>
        /// <param name="sender">Component who sent the event</param>
        /// <param name="e">Event</param>
        private void OnReturnMenuClicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.LaunchMainMenu();
        }

        /// <summary>
        /// Focus the component at the end of loading
        /// </summary>
        /// <param name="sender">Component who sent the event</param>
        /// <param name="e">Event</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Focusable = true;
            Keyboard.Focus(this);
        }
    }
}

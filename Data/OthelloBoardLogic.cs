﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Othello.Data
{
    public enum SlotContent { Nothing = -1, White = 0, Black = 1 };

    [Serializable]
    public class OthelloBoardLogic
    {
        private readonly int NUMBER_OF_PLAYERS = 2;

        private PlayerData[] playerData;
        private int[,] gameBoard;
        private Player playerTurn;
        private List<Tuple<List<IntPosition>, Player>> moveHistory;

        [NonSerialized]
        private Timer timer;

        /// <summary>
        /// Default constructor : Construct othello game board and all his logic. The size of the game is 7x9
        /// </summary>
        public OthelloBoardLogic(): this(new IntPosition(7,9), new IntPosition(3,3))
        {
        }

        /// <summary>
        /// Overloaded constructor: can define the size of the board and the initial position of the pawns (top left corner)
        /// </summary>
        /// <param name="gridSize">Size of the board</param>
        /// <param name="initialPawnsPosition">Initial position of the pawns (top left corner)</param>
        public OthelloBoardLogic(IntPosition gridSize, IntPosition initialPawnsPosition)
        {
            InitAll(gridSize, initialPawnsPosition);
        }


        /// <summary>
        /// Initialise all the data of the game
        /// </summary>
        /// <param name="gridSize">Size of the board</param>
        /// <param name="initialPawnsPosition">Initial position of the pawns (top left corner)</param>
        public void InitAll(IntPosition gridSize, IntPosition initialPawnsPosition)
        {
            playerTurn = Player.Black;

            InitPlayersData();
            InitGameBoard(gridSize, initialPawnsPosition);
            InitTimer();
            moveHistory = new List<Tuple<List<IntPosition>, Player>>();
        }

        /// <summary>
        /// Init. of the players
        /// </summary>
        public void InitPlayersData()
        {
            playerData = new PlayerData[NUMBER_OF_PLAYERS];
            playerData[(int)Player.Black] = new PlayerData();
            playerData[(int)Player.White] = new PlayerData();
        }

        /// <summary>
        /// Init. the game board
        /// </summary>
        /// <param name="gridSize">Size of the board</param>
        /// <param name="initialPawnsPosition">Initial position of the pawns (top left corner)</param>
        public void InitGameBoard(IntPosition gridSize, IntPosition initialPawnsPosition)
        {
            gameBoard = new int[gridSize.Row, gridSize.Column];

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    gameBoard[row, column] = (int)SlotContent.Nothing;
                }
            }

            gameBoard[initialPawnsPosition.Row, initialPawnsPosition.Column] = (int)SlotContent.White;
            gameBoard[initialPawnsPosition.Row + 1, initialPawnsPosition.Column] = (int)SlotContent.Black;
            gameBoard[initialPawnsPosition.Row, initialPawnsPosition.Column + 1] = (int)SlotContent.Black;
            gameBoard[initialPawnsPosition.Row + 1, initialPawnsPosition.Column + 1] = (int)SlotContent.White;
        }

        /// <summary>
        /// Init. timer: increment player time every second
        /// </summary>
        public void InitTimer()
        {
            this.timer = new Timer(1000);
            this.timer.Elapsed += OnTimedEvent;
            this.timer.Enabled = true;
            this.timer.Start();
        }

        /// <summary>
        /// Update time of the current player
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            PlayerData playerData = this.GetPlayerData(playerTurn);
            playerData.SecondsElapsed++;
        }

        /// <summary>
        /// Updates game data by filling each of the given slots with the current player color.
        /// This move is then stored as a tuple containing the list of affected slots and which player is responsible for it
        /// </summary>
        /// <param name="slotsToUpdate"> The list of slots that need to be changed</param>
        public void UpdateSlots(List<IntPosition> slotsToUpdate)
        {
            foreach (var slotPosition in slotsToUpdate)
            {
                GameBoard[slotPosition.Row, slotPosition.Column] = (int)PlayerTurn;
            }
            var move = Tuple.Create(slotsToUpdate, PlayerTurn);
            moveHistory.Add(move);
        }


        /// <summary>
        /// Gets the most recent move stored and plays it backwards.
        /// </summary>
        /// <returns> 
        /// A tuple containing the coordinates of the affected slots, which 
        /// player the move belongs to and the coordinates of the pawn that 
        /// was added to the board (which now has to be removed).
        /// </returns>
        public Tuple<List<IntPosition>, Player, IntPosition> UndoMove()
        {
            var lastMove = moveHistory.Last();
            List<IntPosition> slotsToUndo = lastMove.Item1;
            Player moveAuthor = lastMove.Item2;
            IntPosition lastPawnPosition = slotsToUndo.Last();
            slotsToUndo.RemoveAt(slotsToUndo.Count - 1);
            foreach (var position in slotsToUndo)
            {
                GameBoard[position.Row, position.Column] = (int)GetOppositePlayer(moveAuthor);
            }
            GameBoard[lastPawnPosition.Row, lastPawnPosition.Column] = (int)SlotContent.Nothing;
            moveHistory.RemoveAt(moveHistory.Count - 1);
            return Tuple.Create(slotsToUndo, moveAuthor, lastPawnPosition);
        }

        /// <summary>
        /// Check if the position is inside the board
        /// </summary>
        /// <param name="position">Position to check</param>
        /// <returns>True if the position is inside board, false otherwise</returns>
        public bool IsPositionValid(IntPosition position)
        {
            return position.Row >= 0 && position.Row < Rows && position.Column >= 0 && position.Column < Columns;
        }

        /// <summary>
        /// Get the opposite player information
        /// </summary>
        /// <param name="player">Current</param>
        /// <returns></returns>
        public Player GetOppositePlayer(Player player)
        {
            Player result;

            if(player == Player.White)
            {
                result = Player.Black;
            }
            else
            {
                result = Player.White;
            }

            return result;
        }
        
        /// <summary>
        /// Get directions where there's a playable move
        /// </summary>
        /// <param name="position">Position to check</param>
        /// <returns>List of directions</returns>
        public List<IntPosition> GetNeighborsDirections(IntPosition position)
        {
            List<IntPosition> directionsList = new List<IntPosition>();
            Player currentPlayer = playerTurn;
            Player oppositePlayer = GetOppositePlayer(currentPlayer);
            
            for(int rowDelta = -1; rowDelta <= 1; rowDelta++)
            {
                for(int columnDelta = -1; columnDelta <= 1; columnDelta++)
                {
                    IntPosition nextPosition = new IntPosition(position.Row + rowDelta, position.Column + columnDelta);

                    if (IsPositionValid(nextPosition))
                    {
                        int slotContentId = gameBoard[nextPosition.Row, nextPosition.Column];

                        if ((position.Row != nextPosition.Row || position.Column != nextPosition.Column) &&
                           slotContentId == (int)oppositePlayer &&
                           IsPositionValid(nextPosition))
                        {
                            directionsList.Add(new IntPosition(rowDelta, columnDelta));
                        }
                    }
                }
            }

            return directionsList;
        }


        /// <summary>
        /// Walk the game board to get all possible moves for the current player.
        /// </summary>
        /// <returns>
        /// A list containing the position of each playable slot. 
        /// </returns>
        public List<IntPosition> GetAllPossibleMoves()
        {
            List<IntPosition> possibleMovesList = new List<IntPosition>();
            for(int rowIndex = 0; rowIndex < Rows; rowIndex++)
            {
                for(int columnIndex = 0; columnIndex < Columns; columnIndex++)
                {
                    IntPosition currentSlot = new IntPosition(rowIndex, columnIndex);
                    int slotContentId = gameBoard[currentSlot.Row, currentSlot.Column];
                    if(slotContentId == (int)playerTurn)
                    {
                        List<IntPosition> currentSlotPossibleMovesList = GetNeighborsDirections(currentSlot);
                        List<IntPosition> movements = new List<IntPosition>();
                        foreach (IntPosition direction in currentSlotPossibleMovesList)
                        {
                            if(IsPossibleMove(currentSlot, direction, movements))
                            {
                                IntPosition possibleMove = movements[movements.Count - 1];
                                possibleMovesList.Add(possibleMove);
                            }
                            movements.Clear();
                        }
                    }
                }
            }
            return possibleMovesList;
        }

        /// <summary>
        /// Check if there's a valid move in the given direction, if true, the "positions" list is filled with all the positions of the valid move
        /// </summary>
        /// <param name="pawnPosition">Position to check</param>
        /// <param name="direction">Direction to check</param>
        /// <param name="positions">Empty list given by the user, filled with severals position if there's a valid move</param>
        /// <returns></returns>
        public bool IsPossibleMove(IntPosition pawnPosition, IntPosition direction, List<IntPosition> positions)
        {
            IntPosition currentPosition = pawnPosition;
            bool result = false;

            Player currentPlayer = playerTurn;
            Player oppositePlayer = GetOppositePlayer(currentPlayer);

            positions.Add(pawnPosition);

            currentPosition += direction;
            while (IsPositionValid(currentPosition) && gameBoard[currentPosition.Row, currentPosition.Column] == (int)oppositePlayer)
            {
                positions.Add(currentPosition);
                currentPosition += direction;
            }
            
            if(result = IsPositionValid(currentPosition) && (gameBoard[currentPosition.Row, currentPosition.Column] == (int)SlotContent.Nothing))
            {
                positions.Add(currentPosition);
            }

            return result;
        }

        /// <summary>
        /// Get list of positions which indicate the pawns to flip 
        /// </summary>
        /// <param name="pawnPosition">Pawn position</param>
        /// <returns>List of positions where to flip pawns</returns>
        public List<IntPosition> GetPawnsToFlip(IntPosition pawnPosition)
        {
            List<IntPosition> pawnsToFlip = new List<IntPosition>();
            List<IntPosition> directionsList = GetNeighborsDirections(pawnPosition);
            foreach (IntPosition direction in directionsList)
            {
                List<IntPosition> currentPath = new List<IntPosition>();

                Player currentPlayer = playerTurn;
                Player oppositePlayer = GetOppositePlayer(currentPlayer);

                IntPosition currentPosition = pawnPosition;
                currentPosition += direction;
                while (IsPositionValid(currentPosition) && gameBoard[currentPosition.Row, currentPosition.Column] == (int)oppositePlayer)
                {
                    currentPath.Add(currentPosition);
                    currentPosition += direction;
                }

                if(IsPositionValid(currentPosition) && gameBoard[currentPosition.Row, currentPosition.Column] == (int)currentPlayer)
                {
                    pawnsToFlip.AddRange(currentPath);
                }
            }

            return pawnsToFlip;
        }

        /// <summary>
        /// Switch player turn
        /// </summary>
        public void SwitchPlayer()
        {
            playerTurn = GetOppositePlayer(playerTurn);
        }

        /// <summary>
        /// Clears the score of all players
        /// </summary>
        public void ClearPlayersScore()
        {
            foreach(PlayerData player in playerData)
            {
                player.ClearScore();
            }
        }

        /// <summary>
        /// Updates the score of the players
        /// </summary>
        public void UpdatePlayersScore()
        {
            ClearPlayersScore();

            Player oppositePlayer = GetOppositePlayer(playerTurn);
            int totalPawnsCurrentPlayer = 0;
            int totalPawnsOppositePlayer = 0;
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    if(gameBoard[row, column] == (int) playerTurn)
                    {
                        totalPawnsCurrentPlayer++;
                    }
                    else if(gameBoard[row, column] == (int) oppositePlayer)
                    {
                        totalPawnsOppositePlayer++;
                    }
                }
            }

            playerData[(int)playerTurn].NumberOfPawns = totalPawnsCurrentPlayer;
            playerData[(int)oppositePlayer].NumberOfPawns = totalPawnsOppositePlayer;
        }

        /// <summary>
        /// Get data of player
        /// </summary>
        /// <param name="player">Player</param>
        /// <returns>Data of the player</returns>
        public PlayerData GetPlayerData(Player player)
        {
            return playerData[(int)player];
        }

        /// <summary>
        /// Get data of the white player
        /// </summary>
        /// <returns>Data of the white player</returns>
        public PlayerData GetWhitePlayerData()
        {
            return GetPlayerData(Player.White);
        }

        /// <summary>
        /// Get data of the black player
        /// </summary>
        /// <returns>Data of the black player</returns>
        public PlayerData GetBlackPlayerData()
        {
            return GetPlayerData(Player.Black);
        }

        /// <summary>
        /// Get the number of rows
        /// </summary>
        public int Rows
        {
            get { return gameBoard.GetLength(0); }
        }

        /// <summary>
        ///  Get the number of columns
        /// </summary>
        public int Columns
        {
            get { return gameBoard.GetLength(1); }
        }

        /// <summary>
        /// Get the board
        /// </summary>
        public int[,] GameBoard
        {
            get { return gameBoard; }
        }

        /// <summary>
        /// Get the current player turn
        /// </summary>
        public Player PlayerTurn
        {
            get { return playerTurn; }
        }
    }
}

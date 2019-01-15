using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Othello.Data
{
    enum SlotContent { Nothing = -1, White = 0, Black = 1 };

    [Serializable]
    public class OthelloBoardLogic
    {
        private readonly int NUMBER_OF_PLAYERS = 2;

        private PlayerData[] playerData;
        private int[,] gameBoard;
        private Player playerTurn;
        private Timer timer;

        public OthelloBoardLogic(): this(new IntPosition(7,9), new IntPosition(3,3))
        {
        }

        public OthelloBoardLogic(IntPosition gridSize, IntPosition initialPawnsPosition)
        {
            InitAll(gridSize, initialPawnsPosition);
        }

        public void InitAll(IntPosition gridSize, IntPosition initialPawnsPosition)
        {
            playerTurn = Player.Black;

            InitPlayersData();
            InitGameBoard(gridSize, initialPawnsPosition);
            InitTimer();
        }

        public void InitPlayersData()
        {
            playerData = new PlayerData[NUMBER_OF_PLAYERS];
            playerData[(int)Player.Black] = new PlayerData();
            playerData[(int)Player.White] = new PlayerData();
        }

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

        public void InitTimer()
        {
            this.timer = new Timer(1000);
            this.timer.Elapsed += OnTimedEvent;
            this.timer.Enabled = true;
            this.timer.Start();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            PlayerData playerData = this.GetPlayerData(playerTurn);
            playerData.SecondsElapsed++;
        }

        public bool IsPositionValid(IntPosition position)
        {
            return position.Row >= 0 && position.Row < Rows && position.Column >= 0 && position.Column < Columns;
        }

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
                        //possibleMovesList.AddRange(currentSlotPossibleMovesList);
                    }
                }
            }

            return possibleMovesList;
        }

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

        public void SwitchPlayer()
        {
            playerTurn = GetOppositePlayer(playerTurn);
        }

        public void ClearPlayersScore()
        {
            foreach(PlayerData player in playerData)
            {
                player.ClearScore();
            }
        }

        /// <summary>
        /// Updates the score of the current player
        /// </summary>
        public void UpdatePlayerScore()
        {
            ClearPlayersScore();

            int totalPawns = 0;
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    if(gameBoard[row, column] == (int) playerTurn)
                    {
                        totalPawns++;
                    }
                }
            }
            playerData[(int)playerTurn].NumberOfPawns = totalPawns;
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

        public PlayerData GetBlackPlayerData()
        {
            return GetPlayerData(Player.Black);
        }

        public int Rows
        {
            get { return gameBoard.GetLength(0); }
        }

        public int Columns
        {
            get { return gameBoard.GetLength(1); }
        }

        public int[,] GameBoard
        {
            get { return gameBoard; }
        }

        public Player PlayerTurn
        {
            get { return playerTurn; }
        }
    }
}

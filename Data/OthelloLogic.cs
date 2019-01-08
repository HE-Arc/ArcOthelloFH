using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Data
{
    enum SlotContent { Nothing = -1, White = 0, Black = 1 };

    class OthelloLogic
    {
        private readonly int NUMBER_OF_PLAYERS = 2;

        private PlayerData[] playerData;
        private int[,] gameBoard;
        private Player playerColorTurn;

        public OthelloLogic(): this(new IntPosition(7,9), new IntPosition(3,3))
        {
        }

        public OthelloLogic(IntPosition gridSize, IntPosition initialPawnsPosition)
        {
            InitAll(gridSize, initialPawnsPosition);
        }

        public void InitAll(IntPosition gridSize, IntPosition initialPawnsPosition)
        {
            playerColorTurn = Player.White;

            InitPlayersData();
            InitGameBoard(gridSize, initialPawnsPosition);

            AnalyzeGameBoard();
        }

        public void InitPlayersData()
        {
            playerData = new PlayerData[NUMBER_OF_PLAYERS];
            playerData[(int)Player.Black] = new PlayerData(Player.Black);
            playerData[(int)Player.White] = new PlayerData(Player.White);
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

        public bool IsPositionValid(IntPosition position)
        {
            return position.Row >= 0 && position.Row < Rows && position.Column >= 0 && position.Column < Columns;
        }

        public Player GetOppositePlayerColor(Player color)
        {
            Player result;

            if(color == Player.White)
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
            Player currentPlayer = playerColorTurn;
            Player oppositePlayer = GetOppositePlayerColor(currentPlayer);
            
            for(int rowDelta = -1; rowDelta <= 1; rowDelta++)
            {
                for(int columnDelta = -1; columnDelta <= 1; columnDelta++)
                {
                    IntPosition nextPosition = new IntPosition(position.Row + rowDelta, position.Column + columnDelta);
                    int slotContentId = gameBoard[nextPosition.Row, nextPosition.Column];

                    if ((position.Row != nextPosition.Row || position.Column != nextPosition.Column) &&
                       slotContentId == (int)oppositePlayer && 
                       IsPositionValid(nextPosition))
                    {
                        directionsList.Add(new IntPosition(rowDelta, columnDelta));
                    }
                }
            }

            return directionsList;
        }

        public bool IsPossibleMove(IntPosition pawnPosition, IntPosition direction, List<IntPosition> positions)
        {
            IntPosition currentPosition = pawnPosition;
            bool result = false;

            Player currentPlayer = playerColorTurn;
            Player oppositePlayer = GetOppositePlayerColor(currentPlayer);

            positions.Add(pawnPosition);

            currentPosition += direction;
            while (IsPositionValid(currentPosition) && gameBoard[currentPosition.Row, currentPosition.Column] == (int)oppositePlayer)
            {
                positions.Add(currentPosition);
                currentPosition += direction;
            }
            
            if(result = (gameBoard[currentPosition.Row, currentPosition.Column] == (int)SlotContent.Nothing))
            {
                positions.Add(currentPosition);
            }

            return result;
        }

        public void AnalyzeGameBoard()
        {

        }

        public PlayerData GetPlayerDataFromColor(Player color)
        {
            return playerData[(int)color];
        }

        public PlayerData GetWhitePlayerData()
        {
            return GetPlayerDataFromColor(Player.White);
        }

        public PlayerData GetBlackPlayerData()
        {
            return GetPlayerDataFromColor(Player.Black);
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
    }
}

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
        private PlayerColor playerColorTurn;

        public OthelloLogic(): this(new IntPosition(7,9), new IntPosition(3,3))
        {
        }

        public OthelloLogic(IntPosition gridSize, IntPosition initialPawnsPosition)
        {
            InitAll(gridSize, initialPawnsPosition);
        }

        public void InitAll(IntPosition gridSize, IntPosition initialPawnsPosition)
        {
            playerColorTurn = PlayerColor.White;

            InitPlayersData();
            InitGameBoard(gridSize, initialPawnsPosition);
        }

        public void InitPlayersData()
        {
            playerData = new PlayerData[NUMBER_OF_PLAYERS];
            playerData[(int)PlayerColor.Black] = new PlayerData(PlayerColor.Black);
            playerData[(int)PlayerColor.White] = new PlayerData(PlayerColor.White);
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

        public PlayerData GetPlayerDataFromColor(PlayerColor color)
        {
            return playerData[(int)color];
        }

        public PlayerData GetWhitePlayerData()
        {
            return GetPlayerDataFromColor(PlayerColor.White);
        }

        public PlayerData GetBlackPlayerData()
        {
            return GetPlayerDataFromColor(PlayerColor.Black);
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

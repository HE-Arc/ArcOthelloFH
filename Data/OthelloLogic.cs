using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.Data
{
    enum EnumSlot { Empty, White, Black };
    enum EnumPlayer { One = 0, Two = 1 };

    class OthelloLogic
    {
        private readonly int NUMBER_OF_PLAYERS = 2;

        private PlayerData[] playerData;
        private int[,] gameBoard;

        public OthelloLogic(int rows = 7, int columns = 9, int initialPawnRow = 3, int initialPawnColumn = 3)
        {
            playerData = new PlayerData[NUMBER_OF_PLAYERS];
            gameBoard = new int[rows, columns];

            InitGameBoard();
        }

        public void InitGameBoard(int initialPawnRow = 3, int initialPawnColumn = 3)
        {
            int rows = gameBoard.GetLength(0);
            int columns = gameBoard.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    gameBoard[row, column] = (int)EnumSlot.Empty;
                }
            }

            gameBoard[initialPawnRow, initialPawnColumn] = (int)EnumSlot.White;
            gameBoard[initialPawnRow + 1, initialPawnColumn] = (int)EnumSlot.Black;
            gameBoard[initialPawnRow, initialPawnColumn + 1] = (int)EnumSlot.Black;
            gameBoard[initialPawnRow + 1, initialPawnColumn + 1] = (int)EnumSlot.White;
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

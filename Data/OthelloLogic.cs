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

        public OthelloLogic(int rows = 7, int columns = 9, int initialPawnRow = 3, int initialPawnColumn = 3)
        {
            playerData = new PlayerData[NUMBER_OF_PLAYERS];
            gameBoard = new int[rows, columns];

            InitGameBoard();
        }

        public void InitGameBoard(int initialPawnRow = 3, int initialPawnColumn = 3)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    gameBoard[row, column] = (int)SlotContent.Nothing;
                }
            }

            gameBoard[initialPawnRow, initialPawnColumn] = (int)SlotContent.White;
            gameBoard[initialPawnRow + 1, initialPawnColumn] = (int)SlotContent.Black;
            gameBoard[initialPawnRow, initialPawnColumn + 1] = (int)SlotContent.Black;
            gameBoard[initialPawnRow + 1, initialPawnColumn + 1] = (int)SlotContent.White;
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

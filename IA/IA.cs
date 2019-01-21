using Othello.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello.IA
{
    class AIBoard : OthelloLogic, IPlayable.IPlayable
    {
        public int GetBlackScore()
        {
            UpdatePlayersScore();
            return GetBlackPlayerData().NumberOfPawns;
        }

        public int[,] GetBoard()
        {
            return GameBoard;
        }

        public string GetName()
        {
            return "ArcOthelloFH";
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            OthelloLogic logic = new OthelloLogic
            {
                GameBoard = game,
                PlayerTurn = whiteTurn ? Player.White : Player.Black
            };
            List<IntPosition> possibleMoves = logic.GetAllPossibleMoves();
            if(possibleMoves.Count == 0)
            {
                return new Tuple<int, int>(-1, -1);
            }
            Random rnd = new Random();
            IntPosition move = possibleMoves[rnd.Next(0, possibleMoves.Count)];
            return new Tuple<int, int>(move.Column, move.Row);
        }

        public int GetWhiteScore()
        {
            UpdatePlayersScore();
            return GetWhitePlayerData().NumberOfPawns;
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            Player currentPlayer = PlayerTurn;
            PlayerTurn = isWhite ? Player.White : Player.Black;
            IntPosition positionToCheck = new IntPosition(column, line);
            List<IntPosition> playableSlots = GetAllPossibleMoves();
            PlayerTurn = currentPlayer;
            return playableSlots.Contains(positionToCheck);
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            PlayerTurn = isWhite ? Player.White : Player.Black;
            bool IsMoveValid = IsPlayable(column, line, isWhite);
            if(IsMoveValid)
            {
                IntPosition slotPosition = new IntPosition(column, line);
                List<IntPosition> pawnsToFlip = GetPawnsToFlip(slotPosition);
                pawnsToFlip.Add(slotPosition);
                UpdateSlots(pawnsToFlip);
            }
            return IsMoveValid;
        }
    }
}

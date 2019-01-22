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
            int[,] gameState = new int[9, 7];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    gameState[i, j] = game[i, j];
                }
            }

            AIBoard logic = new AIBoard
            {
                GameBoard = gameState,
                PlayerTurn = whiteTurn ? Player.White : Player.Black
            };

            List<IntPosition> possibleMoves = logic.GetAllPossibleMoves();
            if(possibleMoves.Count == 0)
            {
                return new Tuple<int, int>(-1, -1);
            }

            var nextMove = AlphaBeta(logic, level, int.MaxValue, true);
            return new Tuple<int, int>(nextMove.Item2.Column, nextMove.Item2.Row);
        }

        private bool IsTerminal()
        {
            var possibleMoves = GetAllPossibleMoves();
            if(possibleMoves.Count == 0)
            {
                SwitchPlayer();
                GetAllPossibleMoves();
                SwitchPlayer();
                return IsGameFinished;
            }
            else
            {
                return false;
            }
        }

        private Tuple<int, IntPosition> AlphaBeta(AIBoard nodeBoard, int depth, int parentValue, bool maximizingPlayer)
        {
            if(depth == 0 || nodeBoard.IsTerminal())
            {
                return new Tuple<int, IntPosition>(nodeBoard.CurrentPlayerData.NumberOfPawns, new IntPosition(-1,-1));
            }
            else
            {
                int optVal = maximizingPlayer ? -int.MaxValue : int.MaxValue;
                IntPosition optOp = new IntPosition(-1, -1);
                var childPositions = nodeBoard.GetAllPossibleMoves();
                foreach (var child in childPositions)
                {
                    var returnVal = AlphaBeta(PosToBoard(child, nodeBoard), depth - 1, optVal, !maximizingPlayer);
                    int minOrMax = maximizingPlayer ? 1 : -1;
                    if(returnVal.Item1 * minOrMax > optVal * minOrMax)
                    {
                        optVal = returnVal.Item1;
                        optOp = child;
                        if(optVal * minOrMax > parentValue * minOrMax)
                        {
                            break;
                        }
                    }
                }
                return new Tuple<int, IntPosition>(optVal, optOp);
            }
        }

        private AIBoard PosToBoard(IntPosition position, AIBoard sourceBoard)
        {
            AIBoard newBoard = new AIBoard
            {
                GameBoard = sourceBoard.GameBoard,
                PlayerTurn = sourceBoard.PlayerTurn
            };
            newBoard.PlayMove(position.Column, position.Row, PlayerTurn == Player.White);
            return newBoard;
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

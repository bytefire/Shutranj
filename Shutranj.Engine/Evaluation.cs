//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace Shutranj.Engine
//{
//    public class Evaluation
//    {
//        //public static Stopwatch EvaluationStopwatch = new Stopwatch();

//        private static BitHelper _bitHelper = new BitHelper();

//        // piece values
//        private const int PawnValue = 100;
//        private const int KnightValue = 320;
//        private const int BishopValue = 330;
//        private const int RookValue = 500;
//        private const int QueenValue = 900;
//        private const int KingValue = 20000;

//        #region Piece-Square Tables
//        private static int[] BlackPawnSquareTable = new int[64]
//        {
//            0,  0,  0,  0,  0,  0,  0,  0,
//            50, 50, 50, 50, 50, 50, 50, 50,
//            10, 10, 20, 30, 30, 20, 10, 10,
//             5,  5, 10, 25, 25, 10,  5,  5,
//             0,  0,  0, 20, 20,  0,  0,  0,
//             5, -5,-10,  0,  0,-10, -5,  5,
//             5, 10, 10,-20,-20, 10, 10,  5,
//             0,  0,  0,  0,  0,  0,  0,  0
//        };

//        private static int[] WhitePawnSquareTable = new int[64]
//        {
//            0,  0,  0,  0,  0,  0,  0,  0,
//            5, 10, 10,-20,-20, 10, 10,  5,
//            5, -5,-10,  0,  0,-10, -5,  5,
//            0,  0,  0, 20, 20,  0,  0,  0,
//            5,  5, 10, 25, 25, 10,  5,  5,
//            10, 10, 20, 30, 30, 20, 10, 10, 
//            50, 50, 50, 50, 50, 50, 50, 50, 
//            0,  0,  0,  0,  0,  0,  0,  0
//        };

//        private static int[] BlackKnightSquareTable = new int[64]
//        {
//            -50,-40,-30,-30,-30,-30,-40,-50,
//            -40,-20,  0,  0,  0,  0,-20,-40,
//            -30,  0, 10, 15, 15, 10,  0,-30,
//            -30,  5, 15, 20, 20, 15,  5,-30,
//            -30,  0, 15, 20, 20, 15,  0,-30,
//            -30,  5, 10, 15, 15, 10,  5,-30,
//            -40,-20,  0,  5,  5,  0,-20,-40,
//            -50,-40,-30,-30,-30,-30,-40,-50,
//        };

//        private static int[] WhiteKnightSquareTable = new int[64]
//        {
//            -50,-40,-30,-30,-30,-30,-40,-50,
//            -40,-20,  0,  5,  5,  0,-20,-40,
//            -30,  5, 10, 15, 15, 10,  5,-30,
//            -30,  0, 15, 20, 20, 15,  0,-30,
//            -30,  5, 15, 20, 20, 15,  5,-30,
//            -30,  0, 10, 15, 15, 10,  0,-30,
//            -40,-20,  0,  0,  0,  0,-20,-40,
//            -50,-40,-30,-30,-30,-30,-40,-50,
//        };

//        private static int[] BlackBishopSquareTable = new int[64]
//        {
//            -20,-10,-10,-10,-10,-10,-10,-20,
//            -10,  0,  0,  0,  0,  0,  0,-10,
//            -10,  0,  5, 10, 10,  5,  0,-10,
//            -10,  5,  5, 10, 10,  5,  5,-10,
//            -10,  0, 10, 10, 10, 10,  0,-10,
//            -10, 10, 10, 10, 10, 10, 10,-10,
//            -10,  5,  0,  0,  0,  0,  5,-10,
//            -20,-10,-10,-10,-10,-10,-10,-20,
//        };

//        private static int[] WhiteBishopSquareTable = new int[64]
//        {
//            -20,-10,-10,-10,-10,-10,-10,-20,
//            -10,  5,  0,  0,  0,  0,  5,-10,
//            -10, 10, 10, 10, 10, 10, 10,-10,
//            -10,  0, 10, 10, 10, 10,  0,-10,
//            -10,  5,  5, 10, 10,  5,  5,-10,
//            -10,  0,  5, 10, 10,  5,  0,-10,
//            -10,  0,  0,  0,  0,  0,  0,-10,
//            -20,-10,-10,-10,-10,-10,-10,-20,
//        };

//        private static int[] BlackRookSquareTable = new int[64]
//        {
//            0,  0,  0,  0,  0,  0,  0,  0,
//            5, 10, 10, 10, 10, 10, 10,  5,
//           -5,  0,  0,  0,  0,  0,  0, -5,
//           -5,  0,  0,  0,  0,  0,  0, -5,
//           -5,  0,  0,  0,  0,  0,  0, -5,
//           -5,  0,  0,  0,  0,  0,  0, -5,
//           -5,  0,  0,  0,  0,  0,  0, -5,
//            0,  0,  0,  5,  5,  0,  0,  0,
//        };

//        private static int[] WhiteRookSquareTable = new int[64]
//        {
//            0,  0,  0,  5,  5,  0,  0,  0,
//           -5,  0,  0,  0,  0,  0,  0, -5,
//           -5,  0,  0,  0,  0,  0,  0, -5,
//           -5,  0,  0,  0,  0,  0,  0, -5,
//           -5,  0,  0,  0,  0,  0,  0, -5,
//           -5,  0,  0,  0,  0,  0,  0, -5,
//            5, 10, 10, 10, 10, 10, 10,  5,
//            0,  0,  0,  0,  0,  0,  0,  0,
//        };

//        private static int[] BlackQueenSquareTable = new int[64]
//        {
//            -20,-10,-10, -5, -5,-10,-10,-20,
//            -10,  0,  0,  0,  0,  0,  0,-10,
//            -10,  0,  5,  5,  5,  5,  0,-10,
//             -5,  0,  5,  5,  5,  5,  0, -5,
//              0,  0,  5,  5,  5,  5,  0, -5,
//            -10,  5,  5,  5,  5,  5,  0,-10,
//            -10,  0,  5,  0,  0,  0,  0,-10,
//            -20,-10,-10, -5, -5,-10,-10,-20,
//        };

//        private static int[] WhiteQueenSquareTable = new int[64]
//        {
//            -20,-10,-10, -5, -5,-10,-10,-20,
//            -10,  0,  5,  0,  0,  0,  0,-10,
//            -10,  5,  5,  5,  5,  5,  0,-10,
//             0,  0,  5,  5,  5,  5,  0, -5,
//            -5,  0,  5,  5,  5,  5,  0, -5,
//            -10,  0,  5,  5,  5,  5,  0,-10,
//            -10,  0,  0,  0,  0,  0,  0,-10,
//            -20,-10,-10, -5, -5,-10,-10,-20,
//          };

//        private static int[] BlackKingMiddleGameSquareTable = new int[64]
//        {
//            -30,-40,-40,-50,-50,-40,-40,-30,
//            -30,-40,-40,-50,-50,-40,-40,-30,
//            -30,-40,-40,-50,-50,-40,-40,-30,
//            -30,-40,-40,-50,-50,-40,-40,-30,
//            -20,-30,-30,-40,-40,-30,-30,-20,
//            -10,-20,-20,-20,-20,-20,-20,-10,
//             20, 20,  0,  0,  0,  0, 20, 20,
//             20, 30, 10,  0,  0, 10, 30, 20,
//        };

//        private static int[] WhiteKingMiddleGameSquareTable = new int[64]
//        {
//            20, 30, 10,  0,  0, 10, 30, 20,
//            20, 20,  0,  0,  0,  0, 20, 20,
//            -10,-20,-20,-20,-20,-20,-20,-10,
//            -20,-30,-30,-40,-40,-30,-30,-20,
//            -30,-40,-40,-50,-50,-40,-40,-30,
//            -30,-40,-40,-50,-50,-40,-40,-30,
//            -30,-40,-40,-50,-50,-40,-40,-30,
//            -30,-40,-40,-50,-50,-40,-40,-30,    
//        };

//        private static int[] BlackKingEndGameSquareTable = new int[64]
//        {
//            -50,-40,-30,-20,-20,-30,-40,-50,
//            -30,-20,-10,  0,  0,-10,-20,-30,
//            -30,-10, 20, 30, 30, 20,-10,-30,
//            -30,-10, 30, 40, 40, 30,-10,-30,
//            -30,-10, 30, 40, 40, 30,-10,-30,
//            -30,-10, 20, 30, 30, 20,-10,-30,
//            -30,-30,  0,  0,  0,  0,-30,-30,
//            -50,-30,-30,-30,-30,-30,-30,-50,
//        };

//        private static int[] WhiteKingEndGameSquareTable = new int[64]
//        {
//            -50,-30,-30,-30,-30,-30,-30,-50,
//            -30,-30,  0,  0,  0,  0,-30,-30,
//            -30,-10, 20, 30, 30, 20,-10,-30,
//            -30,-10, 30, 40, 40, 30,-10,-30,
//            -30,-10, 30, 40, 40, 30,-10,-30,
//            -30,-10, 20, 30, 30, 20,-10,-30,
//            -30,-20,-10,  0,  0,-10,-20,-30,
//            -50,-40,-30,-20,-20,-30,-40,-50,
//        };

//        #endregion

//        public static void Initialise()
//        {
//            // dummy method to force calling static construction
//        }

//        /// <summary>
//        /// Evaluates from white's perspective. For negamax search, just multiply by -1 to get the score from black's perspective.
//        /// </summary>
//        /// <param name="board">The board to evaluate.</param>
//        /// <returns>Score from white's perspective.</returns>
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static int EvaluateFromPerspectiveOf(Board board, int side)
//        {
//            if (side == Side.White)
//            {
//                return EvaluateForWhite(board);
//            }
//            else
//            {
//                return EvaluateForBlack(board);
//            }
//        }
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        private static int EvaluateForWhite(Board board)
//        {
//            int score = 0;

//            // pawns
//            byte[] pieceIndices = BitHelper.GetSetBitIndexes2(board.PieceToBitboard[Constants.WhitePawnNumber + 6]);
//            int opponentPiecesCount = BitHelper.GetNumberOfSetBits(board.PieceToBitboard[Constants.BlackPawnNumber + 6]);
//            score += CalculateScore(pieceIndices, opponentPiecesCount, PawnValue, WhitePawnSquareTable);

//            // rooks
//            pieceIndices = BitHelper.GetSetBitIndexes2(board.PieceToBitboard[Constants.WhiteRookNumber + 6]);
//            opponentPiecesCount = BitHelper.GetNumberOfSetBits(board.PieceToBitboard[Constants.BlackRookNumber + 6]);
//            score += CalculateScore(pieceIndices, opponentPiecesCount, RookValue, WhiteRookSquareTable);

//            // knights
//            pieceIndices = BitHelper.GetSetBitIndexes2(board.PieceToBitboard[Constants.WhiteKnightNumber + 6]);
//            opponentPiecesCount = BitHelper.GetNumberOfSetBits(board.PieceToBitboard[Constants.BlackKnightNumber + 6]);
//            score += CalculateScore(pieceIndices, opponentPiecesCount, KnightValue, WhiteKnightSquareTable);

//            // bishops
//            pieceIndices = BitHelper.GetSetBitIndexes2(board.PieceToBitboard[Constants.WhiteBishopNumber + 6]);
//            opponentPiecesCount = BitHelper.GetNumberOfSetBits(board.PieceToBitboard[Constants.BlackBishopNumber + 6]);
//            score += CalculateScore(pieceIndices, opponentPiecesCount, BishopValue, WhiteBishopSquareTable);

//            // queens
//            pieceIndices = BitHelper.GetSetBitIndexes2(board.PieceToBitboard[Constants.WhiteQueenNumber + 6]);
//            opponentPiecesCount = BitHelper.GetNumberOfSetBits(board.PieceToBitboard[Constants.BlackQueenNumber + 6]);
//            score += CalculateScore(pieceIndices, opponentPiecesCount, QueenValue, WhiteQueenSquareTable);

//            // king
//            int[] kingSquareTable = (board.GetGameStage() == Constants.GameStage_Middle) ? WhiteKingMiddleGameSquareTable :
//                WhiteKingEndGameSquareTable;
//            pieceIndices = BitHelper.GetSetBitIndexes2(board.PieceToBitboard[Constants.WhiteKingNumber + 6]);
//            opponentPiecesCount = BitHelper.GetNumberOfSetBits(board.PieceToBitboard[Constants.BlackKingNumber + 6]);
//            score += CalculateScore(pieceIndices, opponentPiecesCount, KingValue, kingSquareTable);

//            return score;
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        private static int EvaluateForBlack(Board board)
//        {
//            int score = 0;

//            // pawns
//            byte[] pieceIndices = BitHelper.GetSetBitIndexes2(board.PieceToBitboard[Constants.BlackPawnNumber + 6]);
//            int opponentPiecesCount = BitHelper.GetNumberOfSetBits(board.PieceToBitboard[Constants.WhitePawnNumber + 6]);
//            score += CalculateScore(pieceIndices, opponentPiecesCount, PawnValue, BlackPawnSquareTable);

//            // rooks
//            pieceIndices = BitHelper.GetSetBitIndexes2(board.PieceToBitboard[Constants.BlackRookNumber + 6]);
//            opponentPiecesCount = BitHelper.GetNumberOfSetBits(board.PieceToBitboard[Constants.WhiteRookNumber + 6]);
//            score += CalculateScore(pieceIndices, opponentPiecesCount, RookValue, BlackRookSquareTable);

//            // knights
//            pieceIndices = BitHelper.GetSetBitIndexes2(board.PieceToBitboard[Constants.BlackKnightNumber + 6]);
//            opponentPiecesCount = BitHelper.GetNumberOfSetBits(board.PieceToBitboard[Constants.WhiteKnightNumber + 6]);
//            score += CalculateScore(pieceIndices, opponentPiecesCount, KnightValue, BlackKnightSquareTable);

//            // bishops
//            pieceIndices = BitHelper.GetSetBitIndexes2(board.PieceToBitboard[Constants.BlackBishopNumber + 6]);
//            opponentPiecesCount = BitHelper.GetNumberOfSetBits(board.PieceToBitboard[Constants.WhiteBishopNumber + 6]);
//            score += CalculateScore(pieceIndices, opponentPiecesCount, BishopValue, BlackBishopSquareTable);

//            // queens
//            pieceIndices = BitHelper.GetSetBitIndexes2(board.PieceToBitboard[Constants.BlackQueenNumber + 6]);
//            opponentPiecesCount = BitHelper.GetNumberOfSetBits(board.PieceToBitboard[Constants.WhiteQueenNumber + 6]);
//            score += CalculateScore(pieceIndices, opponentPiecesCount, QueenValue, BlackQueenSquareTable);

//            // king
//            int[] kingSquareTable = (board.GetGameStage() == Constants.GameStage_Middle) ? BlackKingMiddleGameSquareTable :
//                BlackKingEndGameSquareTable;
//            pieceIndices = BitHelper.GetSetBitIndexes2(board.PieceToBitboard[Constants.BlackKingNumber + 6]);
//            opponentPiecesCount = BitHelper.GetNumberOfSetBits(board.PieceToBitboard[Constants.WhiteKingNumber + 6]);
//            score += CalculateScore(pieceIndices, opponentPiecesCount, KingValue, kingSquareTable);

//            return score;
//        }

//        private static int CalculateScoreOld_(int[] pieceIndices, int pieceValue, int[] pieceSquareTable)
//        {
//            int score = pieceValue * pieceIndices.Length;

//            for (int i = 0; i < pieceIndices.Length; i++)
//            {
//                score += pieceSquareTable[pieceIndices[i]];
//            }
//            return score;
//        }

//        private static int CalculateScore(byte[] pieceIndices, int opponentPiecesCount, int pieceValue, int[] pieceSquareTable)
//        {
//            int score = pieceValue * (pieceIndices.Length - opponentPiecesCount);

//            for (int i = 0; i < pieceIndices.Length; i++)
//            {
//                score += pieceSquareTable[pieceIndices[i]];
//            }
//            return score;
//        }
//    }
//}

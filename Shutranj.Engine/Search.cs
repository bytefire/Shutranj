//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Shutranj.Engine
//{
//    public class Search
//    {
        
//        public static Stopwatch quiescenceStopWatch = new Stopwatch();
//        public static long nodesCount_ = 0;

//        public static Stack<Move> currentLine = new Stack<Move>();

//        private static TranspositionTable transpositionTable = new TranspositionTable();

//        #region NegaMax + Alpha-Beta Pruning
        
//        public static Tuple<Move, int> RootAlphaBetaParallel(Board board, sbyte side, int depth)
//        {
//            List<Move> moves = board.GenerateMoves(side);
//            Move bestMove = moves[0];
//            int maxScore = Constants.NegativeInfinity;
//            //double score;
//            sbyte oppositeSide = (sbyte)(side * -1);
//            //BoardState state;
//            int[] scores = new int[moves.Count];
//            Board[] boards = new Board[moves.Count];
//            //BoardState boardState = board.GetCurrentBoardState();

//            Parallel.For(0,moves.Count, i=>
//                {
//                    boards[i] = new Board(board.GetCurrentBoardState());
//                });
            
//            Parallel.For(0, moves.Count, i=>
//            {
//                //state = boards[i].GetCurrentBoardState();
//                if (!boards[i].MakeMove(moves[i]))
//                {
//                    scores[i] = Constants.NegativeInfinity;
//                    return;
//                }
//                scores[i] = -AlphaBeta(boards[i], Constants.NegativeInfinity, Constants.PositiveInfinity, depth - 1, oppositeSide);
//            });

//            for(int i=0;i<scores.Length;i++)
//            {
//                if(scores[i]>maxScore)
//                {
//                    maxScore = scores[i];
//                    bestMove = moves[i];
//                }
//            }
//            return new Tuple<Move, int>(bestMove, maxScore);
//        }

//        public static Tuple<Move, int> RootAlphaBeta(Board board, sbyte side, int depth)
//        {
//            List<Move> moves = board.GenerateMoves(side);
            
//            //nodesCount = moves.Count;

//            Move bestMove = moves[0];
//            int maxScore = Constants.NegativeInfinity;
//            int score;
//            sbyte oppositeSide = (sbyte)(side * -1);
//            BoardState state;
//            for (int i = 0; i < moves.Count; i++)
//            {
//                state = board.GetCurrentBoardState();
//                if (!board.MakeMove(moves[i]))
//                {
//                    continue;
//                }
//                score = -AlphaBeta(board, Constants.NegativeInfinity, Constants.PositiveInfinity, depth - 1, oppositeSide);
//                board.RestoreState(state);
//                if (score > maxScore)
//                {
//                    maxScore = score;
//                    bestMove = moves[i];
//                }
//            }
//            return new Tuple<Move, int>(bestMove, maxScore);
//        }

//        public static int AlphaBeta(Board board, int alpha, int beta, int depthLeft, sbyte side)
//        {
//            if (depthLeft == 0)
//            {
//                return Evaluation.EvaluateFromPerspectiveOf(board, side);
//                    // Quiescence(board, side, alpha, beta);
//            }
//            List<Move> moves = board.GenerateMoves(side);

//            //nodesCount += moves.Count;

//            BoardState state;
//            int score;
//            sbyte oppositeSide = (sbyte)(-1 * side);
//            // Iterative Deepening: keep a best move variable to store the move and its score. may by it should be passed in as by-ref argument.
//            //                      update that below where indicated.
//            for (int i = 0; i < moves.Count; i++)
//            {
//                state = board.GetCurrentBoardState();
//                if (!board.MakeMove(moves[i]))
//                {
//                    continue;
//                }
//                score = -AlphaBeta(board, -beta, -alpha, depthLeft - 1, oppositeSide);
//                board.RestoreState(state);
//                if (score >= beta)
//                {
//                    // Iterative Deepening: set the best move and score here. and insert it in a dictionary indexed by depth-left (int)
//                    return beta; // fail-hard beta cut off
//                }
//                if (score > alpha)
//                {
//                    // Iterative Deepening: set the best move and score here.
//                    alpha = score; // alpha is like max in MiniMax
//                }
//            }
//            // Iterative Deepening: insert the best move and depth left in a dictionar indexed by depth-left (int) (same dictionary as above).
//            return alpha;
//        }

//        #endregion

//        #region NegaMax

//        public static Tuple<Move, double> RootNegaMax(Board board, sbyte side, int depth)
//        {
//            List<Move> moves = board.GenerateMoves(side);
//            Move bestMove = moves[0];
//            double maxScore = double.MinValue;
//            double score;
//            sbyte oppositeSide = (sbyte)(side * -1);
//            BoardState state;
//            for (int i = 0; i < moves.Count; i++)
//            {
//                state = board.GetCurrentBoardState();
//                if (!board.MakeMove(moves[i]))
//                {
//                    continue;
//                }
//                score = -NegaMax(board, depth, oppositeSide);
//                board.RestoreState(state);
//                if (score > maxScore)
//                {
//                    maxScore = score;
//                    bestMove = moves[i];
//                }
//            }
//            return new Tuple<Move, double>(bestMove, maxScore);
//        }

//        public static double NegaMax(Board board, int depth, sbyte side)
//        {
//            double score;
//            if (depth == 0)
//            {
//                score = Evaluation.EvaluateFromPerspectiveOf(board, side); 
//                    // board.EvaluateFromPerspectiveOf(side);
//                return score;
//            }
//            double max = double.MinValue;
//            List<Move> moves = board.GenerateMoves(side);
//            sbyte oppositeSide = (sbyte)(side * -1);
//            BoardState state;
            
//            for (int i = 0; i < moves.Count; i++)
//            {
//                state = board.GetCurrentBoardState();
//                if (!board.MakeMove(moves[i]))
//                {
//                    continue;
//                }
//                score = -NegaMax(board, depth - 1, oppositeSide);
//                board.RestoreState(state);
//                if (score > max)
//                {
//                    max = score;
//                }
//            }
//            return max;
//        }
        
//        #endregion

//        #region Minimax
//        public static Tuple<Move, double, int> Minimax(Board board, int side, int depth = 2)
//        {
//            // Minimax search based op http://chessprogramming.wikispaces.com/Minimax#Implementation with a minor change
//            //          to the Mini method specified in the comments below (see Mini method definition)

//            Tuple<Move, double, int> bestMoveAndScore;
//            if (side == Side.White)
//            {
//                bestMoveAndScore = Maxi(board, depth);
//            }
//            else
//            {
//                bestMoveAndScore = Mini(board, depth);
//            }
//            return bestMoveAndScore;
//        }

//        private static Tuple<Move, double, int> Maxi(Board board, int depth)
//        {
//            double score;
//            if (depth == 0)
//            {
//                score = board.Evaluate();
//                return new Tuple<Move, double, int>(null, score, depth);
//            }
//            Move bestMove = null;
//            double max = Double.MinValue;
//            int bestMoveDepth = -1;
//            List<Move> allMoves = board.GenerateMoves(Side.White);
//            BoardState state;
//            for (int i = 0; i < allMoves.Count; i++)
//            {

//                if ((allMoves[i].ToString() == "34-43") && (depth == 3))
//                {
//                    Debug.WriteLine("illegal best move");
//                }

//                //string fenString = board.ToFenString();

//                state = board.GetCurrentBoardState();
//                if (!board.MakeMove(allMoves[i]))
//                {
//                    continue;
//                }

//                currentLine.Push(allMoves[i]);

//                //fenString = board.ToFenString();

//                score = Mini(board, depth - 1).Item2;

//                board.RestoreState(state);
//                currentLine.Pop();

//                //fenString = board.ToFenString();

//                if (max < score)
//                {
//                    max = score;
//                    bestMove = allMoves[i];
//                    bestMoveDepth = depth;
//                }
//            }

//            return new Tuple<Move, double, int>(bestMove, max, bestMoveDepth);
//        }

//        private static Tuple<Move, double, int> Mini(Board board, int depth)
//        {
//            double score;
//            if (depth == 0)
//            {
//                // Different from Minimax algo given here: http://chessprogramming.wikispaces.com/Minimax#Implementation.
//                // The algo says score = -board.Evaluate(). The minus sign has been removed because our Evaluate method
//                // calculates from white's perspective only. So a score for black side will be negative.
//                score = board.Evaluate();
//                return new Tuple<Move, double, int>(null, score, depth);
//            }
//            Move bestMove = null;
//            double min = Double.MaxValue;
//            int bestMoveDepth = -1;

//            List<Move> allMoves = board.GenerateMoves(Side.Black);
//            BoardState state;
//            for (int i = 0; i < allMoves.Count; i++)
//            {
//                /*
//                if (allMoves[i].ToString() == "42-27")
//                {
//                    Debug.WriteLine("Illegal Move");
//                }
//                */

//                //string fenString = board.ToFenString();

//                state = board.GetCurrentBoardState();

//                if (!board.MakeMove(allMoves[i]))
//                {
//                    continue;
//                }

//                currentLine.Push(allMoves[i]);

//                //fenString = board.ToFenString();

//                score = Maxi(board, depth - 1).Item2;

//                board.RestoreState(state);

//                currentLine.Pop();

//                //fenString = board.ToFenString();

//                if (min > score)
//                {
//                    min = score;
//                    bestMove = allMoves[i];
//                    bestMoveDepth = depth;
//                }
//            }

//            return new Tuple<Move, double, int>(bestMove, min, bestMoveDepth);
//        }

//        #endregion

//        #region Quiescence
//        private static int Quiescence(Board board, sbyte side, int alpha, int beta)
//        {
//            // from: http://chessprogramming.wikispaces.com/Quiescence+Search
//            int standingPat = Evaluation.EvaluateFromPerspectiveOf(board, side);

//            if (standingPat >= beta)
//            {
//                return beta;
//            }

//            if (alpha < standingPat)
//            {
//                alpha = standingPat;
//            }

//            sbyte oppositeSide = (sbyte)(-1 * side);

//            List<Move> moves = board.GenerateCaptureMoves(side);
//            int score;
//            BoardState state;
//            for (int i = 0; i < moves.Count; i++)
//            {
//                //nodesCount++;

//                state = board.GetCurrentBoardState();
//                if (!board.MakeMove(moves[i]))
//                {
//                    continue;
//                }
//                score = -Quiescence(board, oppositeSide, -beta, -alpha);
//                board.RestoreState(state);

//                if (score >= beta)
//                {
//                    return beta;
//                }
//                if (score > alpha)
//                {
//                    alpha = score;
//                }
//            }
//            return alpha;
//        }

//        #endregion
//    }
//}

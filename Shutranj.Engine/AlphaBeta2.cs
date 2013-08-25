using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.Engine
{
    public class AlphaBeta2
    {
        private const int MaxDepth = 20;
        private const int MinDepth = 6;
        private const int TimeLimitInMilliseconds = 10 * 1000; // 10 secs

        private static TranspositionTable transpositionTable = new TranspositionTable();

        // Iterative Deepening Aspiration Search Parallel
        public static Tuple<UInt16, int, long, int> IDASParallel(Board board, sbyte side)
        {
            // threshold value of one-third of a pawn value (100) based on http://www.frayn.net/beowulf/theory.html#aspiration
            int threshold = 30;

            // NOTE: following are dummy values to prevent compiler error.
            Tuple<UInt16, int> moveAndScore = new Tuple<UInt16, int>(0, -10000000);

            Stopwatch sw = new Stopwatch();

            int depth, depthSearched = 0;

            moveAndScore = RootAlphaBetaTTParallel(board, side, MinDepth, Constants.NegativeInfinity, Constants.PositiveInfinity);
            int best = moveAndScore.Item2;
            sw.Start();
            for (depth = MinDepth + 1; depth <= MaxDepth; depth++)
            {
                moveAndScore = RootAlphaBetaTTParallel(board, side, depth, best - threshold, best + threshold);
                if (moveAndScore.Item2 <= (best - threshold)) // failed low
                {
                    moveAndScore = RootAlphaBetaTTParallel(board, side, depth, Constants.NegativeInfinity, moveAndScore.Item2);
                }
                else if (moveAndScore.Item2 >= (best + threshold)) // failed high
                {
                    moveAndScore = RootAlphaBetaTTParallel(board, side, depth, moveAndScore.Item2, Constants.PositiveInfinity);
                }
                //else
                //{
                best = moveAndScore.Item2;
                //}
                depthSearched = depth;
                if (sw.ElapsedMilliseconds >= TimeLimitInMilliseconds)
                {
                    sw.Stop();
                    break;
                }
            }

            return new Tuple<ushort, int, long, int>(moveAndScore.Item1, depthSearched, sw.ElapsedMilliseconds, best);
        }

        public static Tuple<UInt16, int, long, int> IterativeDeepeningParallel(Board board, sbyte side)
        {
            // NOTE: following are dummy values to prevent compiler error.
            Tuple<UInt16, int> moveAndScore = new Tuple<UInt16, int>(0, -10000000);

            Stopwatch sw = new Stopwatch();

            int depth;

            sw.Start();
            for (depth = MinDepth; depth <= MaxDepth; depth++)
            {
                moveAndScore = RootAlphaBetaTTParallel(board, side, depth);
                if (sw.ElapsedMilliseconds >= TimeLimitInMilliseconds)
                {
                    sw.Stop();
                    break;
                }
            }

            return new Tuple<ushort, int, long, int>(moveAndScore.Item1, depth, sw.ElapsedMilliseconds, moveAndScore.Item2);
        }



        public static Tuple<UInt16, int> RootAlphaBetaTTParallel(Board board, sbyte side, int depth, 
            int alpha = Constants.NegativeInfinity, int beta = Constants.PositiveInfinity)
        {
            int movesCount;
            UInt16[] moves = board.GenerateMoves(out movesCount);

            
            UInt16 bestMove = moves[0];
            int maxScore = Constants.NegativeInfinity;
            sbyte oppositeSide = (sbyte)(side * -1);

            int[] scores = new int[movesCount];
            Board[] boards = new Board[movesCount];
            // Search[] searches = new Search[moves.Count];

            Parallel.For(0, movesCount, i =>
            {
                boards[i] = new Board(board.GetBoardState());
            });


            Parallel.For(0, movesCount, i =>
            {
                if (!boards[i].MakeMove(moves[i]))
                {
                    scores[i] = Constants.NegativeInfinity;
                    return;
                }

                scores[i] = -AlphaBetaTT(boards[i], -beta, -alpha, (byte)(depth - 1), oppositeSide,
                    new UInt16[depth], 0);

            });

            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i] > maxScore)
                {
                    maxScore = scores[i];
                    bestMove = moves[i];
                }
            }

            return new Tuple<UInt16, int>(bestMove, maxScore);
        }



        public static int AlphaBetaTT(Board board, int alpha, int beta, byte depthLeft, sbyte side, UInt16[] killers, int level)
        {
            int score;
            TranspositionTableEntry entry = transpositionTable.Retrieve(board.ZobristHash);

            if (entry.IsValid() && (TranspositionTableEntryHelper.GetDepthSearched(entry.Data) >= depthLeft))
            {
                if (TranspositionTableEntryHelper.GetEntryType(entry.Data) == TranspositionTableEntryHelper.EntryTypeExactValue)
                {
                    return TranspositionTableEntryHelper.GetScore(entry.Data);
                }
                if ((TranspositionTableEntryHelper.GetEntryType(entry.Data) == TranspositionTableEntryHelper.EntryTypeLowerBound) &&
                    (TranspositionTableEntryHelper.GetScore(entry.Data) > alpha))
                {
                    alpha = TranspositionTableEntryHelper.GetScore(entry.Data);
                }
                else if ((TranspositionTableEntryHelper.GetEntryType(entry.Data) == TranspositionTableEntryHelper.EntryTypeUpperBound) &&
                    (TranspositionTableEntryHelper.GetScore(entry.Data) < beta))
                {
                    beta = TranspositionTableEntryHelper.GetScore(entry.Data);
                }
                if (alpha >= beta)
                {
                    return TranspositionTableEntryHelper.GetScore(entry.Data);
                }
            }
            if (depthLeft == 0)
            {
                score = Quiescence_Limited(board, side, alpha, beta, (level + 1) % 2); // the last arg ensures that quiescence evaluates opponent's move as the last one.
                    // Quiescence(board, side, alpha, beta);
                    // Evaluation2.EvaluateFromPerspectiveOf(board, side);

                if (score <= alpha)
                {
                    transpositionTable.Add(new TranspositionTableEntry(board.ZobristHash,
                        TranspositionTableEntryHelper.ComposeData(score, TranspositionTableEntryHelper.InvalidMove,
                            depthLeft, TranspositionTableEntryHelper.EntryTypeLowerBound)));
                }
                else if (score >= beta)
                {
                    transpositionTable.Add(new TranspositionTableEntry(board.ZobristHash,
                        TranspositionTableEntryHelper.ComposeData(score, TranspositionTableEntryHelper.InvalidMove,
                            depthLeft, TranspositionTableEntryHelper.EntryTypeUpperBound)));
                }
                else
                {
                    transpositionTable.Add(new TranspositionTableEntry(board.ZobristHash,
                        TranspositionTableEntryHelper.ComposeData(score, TranspositionTableEntryHelper.InvalidMove,
                            depthLeft, TranspositionTableEntryHelper.EntryTypeExactValue)));
                }

                return score;
            }

            int movesCount;
            UInt16[] moves = board.GenerateMoves(out movesCount);

            #region Move Ordering
            // this is the index with which a good move will be swapped with...
            int indexToSwapWith = 0;

            /*************** Transposition Table Reordering **************/
            // NOTE (17 Jul 2013): There appears to be no gain from transposition table re-ordering, therefore
            //              commenting it out. 

            //if (entry.IsValid())
            //{
            //    UInt16 bestMoveFromTable = TranspositionTableEntryHelper.GetBestMove(entry.Data);
            //    if (bestMoveFromTable != TranspositionTableEntryHelper.InvalidMove)
            //    {
            //        for (int i = 0; i < movesCount; i++)
            //        {
            //            if (moves[i] == bestMoveFromTable)
            //            {
            //                // swap...
            //                UInt16 temp = moves[i];
            //                moves[i] = moves[indexToSwapWith];
            //                moves[indexToSwapWith] = temp;
            //                indexToSwapWith++;
            //                break;
            //            }
            //        }
            //    }
            //}
            /*************** End of Transposition Table Reordering **************/

            /*************** Killer Heuristic Reordering *****************/
            if (killers[level] != 0)
            {
                for (int i = 0; i < movesCount; i++)
                {
                    if (killers[level] == moves[i])
                    {
                        // if killer move found then swap that with the first move.
                        UInt16 temp = moves[i];
                        moves[i] = moves[indexToSwapWith];
                        moves[indexToSwapWith] = temp;
                        indexToSwapWith++;
                        break;
                    }
                }
            }
            // set killer for next level to 0
            killers[level + 1] = 0;
            /*************** End of Killer Heuristic Reordering *****************/

            #endregion
            // NOTE: diluting negative infinity by level so that the farther away the checkmate is from current
            //          board position the lower will be the score. this is to get the engine to checkmate opponent
            //          as soon as possible rather than wasting time in capture or other moves which ultimately lead to
            //          checkmate but are not the fastest route to checkmakte.
            int bestScore = Constants.NegativeInfinity + level;
            UInt16 bestMove = TranspositionTableEntryHelper.InvalidMove;

            BoardState state;
            sbyte oppositeSide = (sbyte)(-1 * side);

            for (int i = 0; i < movesCount; i++)
            {
                state = board.GetBoardState();
                if (!board.MakeMove(moves[i]))
                {
                    continue;
                }
                score = -AlphaBetaTT(board, -beta, -alpha, (byte)(depthLeft - 1), oppositeSide, killers, level + 1);
                board.RestoreState(state);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = moves[i];
                }
                if (bestScore > alpha)
                {
                    alpha = bestScore;
                }
                if (bestScore >= beta)
                {
                    // beta-cutoff, therefore update killer for this level
                    killers[level] = moves[i];
                    break;
                }
            }

            // following nested ifs check for stalemate.
            if (bestScore == (Constants.NegativeInfinity + level))
            {
                if (!board.IsInCheck(side))
                {
                    bestScore = Constants.DrawScore;
                }
            }

            if (bestScore <= alpha) // lower bound value
            {
                transpositionTable.Add(new TranspositionTableEntry(board.ZobristHash,
                        TranspositionTableEntryHelper.ComposeData(bestScore, bestMove,
                            depthLeft, TranspositionTableEntryHelper.EntryTypeLowerBound)));
            }
            else if (bestScore >= beta) // upper bound value
            {
                transpositionTable.Add(new TranspositionTableEntry(board.ZobristHash,
                        TranspositionTableEntryHelper.ComposeData(bestScore, bestMove,
                            depthLeft, TranspositionTableEntryHelper.EntryTypeUpperBound)));
            }
            else // true minimax value
            {
                transpositionTable.Add(new TranspositionTableEntry(board.ZobristHash,
                        TranspositionTableEntryHelper.ComposeData(bestScore, bestMove,
                            depthLeft, TranspositionTableEntryHelper.EntryTypeExactValue)));
            }

            return bestScore;
        }




        #region Non-Parallel Versions of AlphaBetaTT

        public static Tuple<UInt16, int> IterativeDeepening(Board board, sbyte side, int depth)
        {
            // NOTE: following are dummy values to prevent compiler error.
            Tuple<UInt16, int> moveAndScore = new Tuple<UInt16, int>(0, -10000000);
            for (int i = 2; i <= depth; i++)
            {
                moveAndScore = RootAlphaBetaTT(board, side, i);
            }
            return moveAndScore;
        }



        public static Tuple<UInt16, int> RootAlphaBetaTT(Board board, sbyte side, int depth)
        {
            int movesCount;
            UInt16[] moves = board.GenerateMoves(out movesCount);

            //nodesCount = moves.Count;

            UInt16 bestMove = moves[0];
            int maxScore = Constants.NegativeInfinity;
            int score;
            sbyte oppositeSide = (sbyte)(side * -1);
            BoardState state;
            // Search search = new Search();

            for (int i = 0; i < movesCount; i++)
            {
                state = board.GetBoardState();
                if (!board.MakeMove(moves[i]))
                {
                    continue;
                }
                score = -AlphaBetaTT(board, Constants.NegativeInfinity, Constants.PositiveInfinity, (byte)(depth - 1), oppositeSide,
                    new UInt16[depth], 0);

                board.RestoreState(state);
                if (score > maxScore)
                {
                    maxScore = score;
                    bestMove = moves[i];
                }
            }
            return new Tuple<UInt16, int>(bestMove, maxScore);
        }


        #endregion


        #region Quiescence
        private static int Quiescence(Board board, sbyte side, int alpha, int beta)
        {
            // from: http://chessprogramming.wikispaces.com/Quiescence+Search
            int standingPat = Evaluation2.EvaluateFromPerspectiveOf(board, side);

            if (standingPat >= beta)
            {
                return beta;
            }

            if (alpha < standingPat)
            {
                alpha = standingPat;
            }

            sbyte oppositeSide = (sbyte)(-1 * side);

            int movesCount;

            UInt16[] moves = board.GenerateCaptureMoves(out movesCount);
            int score;
            BoardState state;
            for (int i = 0; i < movesCount; i++)
            {
                state = board.GetBoardState();
                if (!board.MakeMove(moves[i]))
                {
                    continue;
                }
                score = -Quiescence(board, oppositeSide, -beta, -alpha);
                board.RestoreState(state);

                if (score >= beta)
                {
                    return beta;
                }
                if (score > alpha)
                {
                    alpha = score;
                }
            }
            return alpha;
        }

        // a depth limited quiescence to prevent this from going on and on...
        private static int Quiescence_Limited(Board board, sbyte side, int alpha, int beta, int depthLeft)
        {
            int standingPat = Evaluation2.EvaluateFromPerspectiveOf(board, side);

            if (depthLeft == 0)
            {
                return standingPat;
            }

            if (standingPat >= beta)
            {
                return beta;
            }

            if (alpha < standingPat)
            {
                alpha = standingPat;
            }

            sbyte oppositeSide = (sbyte)(-1 * side);

            int movesCount;

            UInt16[] moves = board.GenerateCaptureMoves(out movesCount);
            int score;
            BoardState state;
            for (int i = 0; i < movesCount; i++)
            {
                state = board.GetBoardState();
                if (!board.MakeMove(moves[i]))
                {
                    continue;
                }
                score = -Quiescence_Limited(board, oppositeSide, -beta, -alpha, depthLeft - 1);
                board.RestoreState(state);

                if (score >= beta)
                {
                    return beta;
                }
                if (score > alpha)
                {
                    alpha = score;
                }
            }
            return alpha;
        }
        #endregion
    }
}

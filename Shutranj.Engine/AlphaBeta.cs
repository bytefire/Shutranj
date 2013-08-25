//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Shutranj.Engine
//{
//    public class AlphaBeta
//    {
//        #region for performance testing only
//        private static int TTCutoffs = 0;
//        private static int ForLoopCount = 0;
//        #endregion


//        private static TranspositionTable transpositionTable = new TranspositionTable();


//        public static Tuple<Move, int> IterativeDeepeningParallel(Board board, sbyte side, int depth)
//        {
//            // NOTE: following are dummy values to prevent compiler error.
//            Tuple<Move, int> moveAndScore = new Tuple<Move, int>(new Move(-30, 255, 255), -10000000);
//            for (int i = 2; i <= depth; i++)
//            {
//                moveAndScore = RootAlphaBetaTTParallel(board, side, i);
//            }

//            Console.WriteLine("TTCutoffs = {0}; ForLoopCount = {1}; Diff: {2}", TTCutoffs, ForLoopCount, TTCutoffs - ForLoopCount);
//            return moveAndScore;
//        }



//        public static Tuple<Move, int> RootAlphaBetaTTParallel(Board board, sbyte side, int depth)
//        {
//            List<Move> moves = board.GenerateMoves(side);

//            Move bestMove = moves[0];
//            int maxScore = Constants.NegativeInfinity;
//            sbyte oppositeSide = (sbyte)(side * -1);

//            int[] scores = new int[moves.Count];
//            Board[] boards = new Board[moves.Count];
//            // Search[] searches = new Search[moves.Count];

//            Parallel.For(0, moves.Count, i =>
//            {
//                boards[i] = new Board(board.GetCurrentBoardState());
//                // searches[i] = new Search();
//            });


//            Parallel.For(0, moves.Count, i =>
//            {
//                if (!boards[i].MakeMove(moves[i]))
//                {
//                    scores[i] = Constants.NegativeInfinity;
//                    return;
//                }

//                scores[i] = -AlphaBetaTT(boards[i], Constants.NegativeInfinity, Constants.PositiveInfinity, (byte)(depth - 1), oppositeSide,
//                    new UInt16[depth], 0);

//            });

//            for (int i = 0; i < scores.Length; i++)
//            {
//                if (scores[i] > maxScore)
//                {
//                    maxScore = scores[i];
//                    bestMove = moves[i];
//                }
//            }

//            return new Tuple<Move, int>(bestMove, maxScore);
//        }



//        public static int AlphaBetaTT(Board board, int alpha, int beta, byte depthLeft, sbyte side, UInt16[] killers, int level)
//        {
//            int score;
//            TranspositionTableEntry entry = transpositionTable.Retrieve(board.ZobristHash);

//            if (entry.IsValid() && (TranspositionTableEntryHelper.GetDepthSearched(entry.Data) >= depthLeft))
//            {
//                if (TranspositionTableEntryHelper.GetEntryType(entry.Data) == TranspositionTableEntryHelper.EntryTypeExactValue)
//                {
//                    TTCutoffs++;
//                    return TranspositionTableEntryHelper.GetScore(entry.Data);
//                }
//                if ((TranspositionTableEntryHelper.GetEntryType(entry.Data) == TranspositionTableEntryHelper.EntryTypeLowerBound) &&
//                    (TranspositionTableEntryHelper.GetScore(entry.Data) > alpha))
//                {
//                    alpha = TranspositionTableEntryHelper.GetScore(entry.Data);
//                }
//                else if ((TranspositionTableEntryHelper.GetEntryType(entry.Data) == TranspositionTableEntryHelper.EntryTypeUpperBound) &&
//                    (TranspositionTableEntryHelper.GetScore(entry.Data) < beta))
//                {
//                    beta = TranspositionTableEntryHelper.GetScore(entry.Data);
//                }
//                if (alpha >= beta)
//                {
//                    return TranspositionTableEntryHelper.GetScore(entry.Data);
//                }
//            }
//            if (depthLeft == 0)
//            {
//                score = Evaluation.EvaluateFromPerspectiveOf(board, side);
//                // Quiescence(board, side, alpha, beta);
//                if (score <= alpha)
//                {
//                    transpositionTable.Add(new TranspositionTableEntry(board.ZobristHash,
//                        TranspositionTableEntryHelper.ComposeData(score, TranspositionTableEntryHelper.InvalidMove,
//                            depthLeft, TranspositionTableEntryHelper.EntryTypeLowerBound)));
//                }
//                else if (score >= beta)
//                {
//                    transpositionTable.Add(new TranspositionTableEntry(board.ZobristHash,
//                        TranspositionTableEntryHelper.ComposeData(score, TranspositionTableEntryHelper.InvalidMove,
//                            depthLeft, TranspositionTableEntryHelper.EntryTypeUpperBound)));
//                }
//                else
//                {
//                    transpositionTable.Add(new TranspositionTableEntry(board.ZobristHash,
//                        TranspositionTableEntryHelper.ComposeData(score, TranspositionTableEntryHelper.InvalidMove,
//                            depthLeft, TranspositionTableEntryHelper.EntryTypeExactValue)));
//                }

//                return score;
//            }

//            List<Move> moves = board.GenerateMoves(side);

//            #region Move Ordering
//            // this is the index with which a good move will be swapped with...
//            int indexToSwapWith = 0;

//            /*************** Transposition Table Reordering **************/
//            // NOTE (17 Jul 2013): There appears to be no gain from transposition table re-ordering, therefore
//            //              commenting it out. 

//            if (entry.IsValid())
//            {
//                UInt16 bestMoveFromTable = TranspositionTableEntryHelper.GetBestMove(entry.Data);
//                if (bestMoveFromTable != TranspositionTableEntryHelper.InvalidMove)
//                {
//                    for (int i = 0; i < moves.Count; i++)
//                    {
//                        if (moves[i].HashKey == bestMoveFromTable)
//                        {
//                            // swap...
//                            Move temp = moves[i];
//                            moves[i] = moves[indexToSwapWith];
//                            moves[indexToSwapWith] = temp;
//                            indexToSwapWith++;
//                            break;
//                        }
//                    }
//                }
//            }
//            /*************** End of Transposition Table Reordering **************/

//            /*************** Killer Heuristic Reordering *****************/
//            if (killers[level] != 0)
//            {
//                for (int i = 0; i < moves.Count; i++)
//                {
//                    if (killers[level] == moves[i].HashKey)
//                    {
//                        // if killer move found then swap that with the first move.
//                        Move temp = moves[i];
//                        moves[i] = moves[indexToSwapWith];
//                        moves[indexToSwapWith] = temp;
//                        indexToSwapWith++;
//                        break;
//                    }
//                }
//            }
//            // set killer for next level to 0
//            killers[level + 1] = 0;
//            /*************** End of Killer Heuristic Reordering *****************/

//            #endregion

//            int bestScore = Constants.NegativeInfinity - 1;
//            UInt16 bestMove = TranspositionTableEntryHelper.InvalidMove;

//            BoardState state;
//            sbyte oppositeSide = (sbyte)(-1 * side);

//            for (int i = 0; i < moves.Count; i++)
//            {
//                state = board.GetCurrentBoardState();
//                if (!board.MakeMove(moves[i]))
//                {
//                    continue;
//                }
//                score = -AlphaBetaTT(board, -beta, -alpha, (byte)(depthLeft - 1), oppositeSide, killers, level + 1);
//                board.RestoreState(state);

//                if (score > bestScore)
//                {
//                    bestScore = score;
//                    bestMove = moves[i].HashKey;
//                }
//                if (bestScore > alpha)
//                {
//                    alpha = bestScore;
//                }
//                if (bestScore >= beta)
//                {
//                    TTCutoffs++;
//                    // beta-cutoff, therefore update killer for this level
//                    killers[level] = moves[i].HashKey;
//                    break;
//                }
//            }

//            if (bestScore <= alpha) // lower bound value
//            {
//                transpositionTable.Add(new TranspositionTableEntry(board.ZobristHash,
//                        TranspositionTableEntryHelper.ComposeData(bestScore, bestMove,
//                            depthLeft, TranspositionTableEntryHelper.EntryTypeLowerBound)));
//            }
//            else if (bestScore >= beta) // upper bound value
//            {
//                transpositionTable.Add(new TranspositionTableEntry(board.ZobristHash,
//                        TranspositionTableEntryHelper.ComposeData(bestScore, bestMove,
//                            depthLeft, TranspositionTableEntryHelper.EntryTypeUpperBound)));
//            }
//            else // true minimax value
//            {
//                transpositionTable.Add(new TranspositionTableEntry(board.ZobristHash,
//                        TranspositionTableEntryHelper.ComposeData(bestScore, bestMove,
//                            depthLeft, TranspositionTableEntryHelper.EntryTypeExactValue)));
//            }

//            return bestScore;
//        }




//        #region Non-Parallel Versions of AlphaBetaTT

//        public static Tuple<Move, int> IterativeDeepening(Board board, sbyte side, int depth)
//        {
//            // NOTE: following are dummy values to prevent compiler error.
//            Tuple<Move, int> moveAndScore = new Tuple<Move, int>(new Move(-30, 255, 255), -10000000);
//            for (int i = 2; i <= depth; i++)
//            {
//                moveAndScore = RootAlphaBetaTT(board, side, i);
//            }
//            return moveAndScore;
//        }



//        public static Tuple<Move, int> RootAlphaBetaTT(Board board, sbyte side, int depth)
//        {
//            List<Move> moves = board.GenerateMoves(side);

//            //nodesCount = moves.Count;

//            Move bestMove = moves[0];
//            int maxScore = Constants.NegativeInfinity;
//            int score;
//            sbyte oppositeSide = (sbyte)(side * -1);
//            BoardState state;
//            // Search search = new Search();

//            for (int i = 0; i < moves.Count; i++)
//            {
//                state = board.GetCurrentBoardState();
//                if (!board.MakeMove(moves[i]))
//                {
//                    continue;
//                }
//                score = -AlphaBetaTT(board, Constants.NegativeInfinity, Constants.PositiveInfinity, (byte)(depth - 1), oppositeSide,
//                    new UInt16[depth], 0);

//                board.RestoreState(state);
//                if (score > maxScore)
//                {
//                    maxScore = score;
//                    bestMove = moves[i];
//                }
//            }
//            return new Tuple<Move, int>(bestMove, maxScore);
//        }


//        #endregion
//    }
//}

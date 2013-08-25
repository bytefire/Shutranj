using Shutranj.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // TestAlphaBetaParallel();
            TestIDAS();
            Console.ReadLine();
        }

        static void TestAlphaBetaParallel()
        {
            Board board = new Board();
            // TestingHelper.MakeSomeMoves(board);
            Evaluation2.Initialise();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            Tuple<UInt16, int, long, int> bestMoveAndScore = AlphaBeta2.IterativeDeepeningParallel(board, Side.White);
                // AlphaBeta2.IDASParallel(board, Side.White);
              //Tuple<UInt16, int> bestMoveAndScore = AlphaBeta2.RootAlphaBetaTTParallel(board, Side.White, 7, -120, -60);
            sw.Stop();

            //Console.WriteLine("Best Move For White: {0}. Depth: {1}. Score:{2}. TIME: {3} milliseconds",
            //   board.ConvertToAlgebraicNotation(bestMoveAndScore.Item1), 7, bestMoveAndScore.Item2, sw.ElapsedMilliseconds);

            Console.WriteLine("Best Move For White: {0}. Depth: {1}. Score:{2}. TIME: {3} milliseconds",
               board.ConvertToAlgebraicNotation(bestMoveAndScore.Item1), bestMoveAndScore.Item2, bestMoveAndScore.Item4, sw.ElapsedMilliseconds);
        }

        static void TestIDAS()
        {
            Board board = new Board();
            TestingHelper.MakeSomeMoves(board);
            Evaluation2.Initialise();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            Tuple<UInt16, int, long, int> bestMoveAndScore = AlphaBeta2.IDASParallel(board, Side.White);
                // AlphaBeta2.IterativeDeepeningParallel(board, Side.White);
            

            //Tuple<UInt16, int> bestMoveAndScore = AlphaBeta2.RootAlphaBetaTTParallel(board, Side.White, 6);
            //bestMoveAndScore = AlphaBeta2.RootAlphaBetaTTParallel(board, Side.White, 7);
                //, -120, -60);
            sw.Stop();

            //Console.WriteLine("Best Move For White: {0}. Depth: {1}. Score:{2}. TIME: {3} milliseconds",
            //   board.ConvertToAlgebraicNotation(bestMoveAndScore.Item1), 7, bestMoveAndScore.Item2, sw.ElapsedMilliseconds);

            Console.WriteLine("Best Move For White: {0}. Depth: {1}. Score:{2}. TIME: {3} milliseconds",
               board.ConvertToAlgebraicNotation(bestMoveAndScore.Item1), bestMoveAndScore.Item2, bestMoveAndScore.Item4, sw.ElapsedMilliseconds);
        }
    }
}

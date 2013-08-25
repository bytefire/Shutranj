using Shutranj.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatranj.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // variables
            sbyte ownSide;
            Board board = new Board();
            Stack<BoardState> undoStack = new Stack<BoardState>();

            // int depth = 6;
            UInt16 move;
            Tuple<UInt16, int, long, int> moveDepthTimeAndScore;

            string opponentMoveString;

            Console.WriteLine("Filthy Mind Chess");
            Console.WriteLine();

            /*********** Input opponent side ************/
            while (true)
            {
                Console.Write("Choose your side (w/b): ");
                char opponentSideChar = Console.ReadKey().KeyChar;
                Console.WriteLine();
                if (opponentSideChar == 'w')
                {
                    ownSide = Side.Black;
                    break;
                }
                else if (opponentSideChar == 'b')
                {
                    ownSide = Side.White;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid side.");
                }
            }
            /*************** End of input opponent side ******************/

            /*************** Start game *******************/
            Console.WriteLine("Start playing.");
            Console.WriteLine();
            if (ownSide == Side.White)
            {
                moveDepthTimeAndScore = AlphaBeta2.IDASParallel(board, ownSide);
                    // AlphaBeta2.IterativeDeepeningParallel(board, ownSide);
                    // AlphaBeta2.RootAlphaBetaTTParallel(board, ownSide, depth).Item1;
                move = moveDepthTimeAndScore.Item1;
                if (board.MakeMove(move))
                {
                    Console.WriteLine("Filthy Mind: {0} (Depth: {1} plies; Time: {2}ms; Score: {3})", board.ConvertToAlgebraicNotation(move),
                        moveDepthTimeAndScore.Item2, moveDepthTimeAndScore.Item3, moveDepthTimeAndScore.Item4);
                }
            }
            while (true)
            {
                Console.Write("You: ");
                opponentMoveString = Console.ReadLine();
                if (opponentMoveString.ToLower() == "exit")
                {
                    break;
                }

                if (opponentMoveString.ToLower() == "fen")
                {
                    Console.WriteLine("Board Position: {0}", board.ToString());
                    continue;
                }

                if (opponentMoveString.Trim().ToLower() == "undo")
                {
                    if (undoStack.Count > 0)
                    {
                        board.RestoreState(undoStack.Pop());
                        Console.WriteLine("Undid last move.");
                    }
                    else
                    {
                        Console.WriteLine("Nothing to undo!");
                    }
                    continue;
                }

                try
                {
                    undoStack.Push(board.GetBoardState());

                    if (!board.MakeUserMove(opponentMoveString))
                    {
                        Console.WriteLine("Failed to make move. Try again.");
                        undoStack.Pop();
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    undoStack.Pop();
                    Console.WriteLine("ERROR: {0}", ex.Message);
                    continue;
                }
                // make move for own side
                moveDepthTimeAndScore = AlphaBeta2.IDASParallel(board, ownSide);
                    // AlphaBeta2.IterativeDeepeningParallel(board, ownSide);
                    // AlphaBeta2.RootAlphaBetaTTParallel(board, ownSide, depth).Item1;

                move = moveDepthTimeAndScore.Item1;
                if (board.MakeMove(move))
                {
                    Console.WriteLine("Filthy Mind: {0} (Depth: {1} plies; Time: {2}ms; Score: {3})", board.ConvertToAlgebraicNotation(move),
                        moveDepthTimeAndScore.Item2, moveDepthTimeAndScore.Item3, moveDepthTimeAndScore.Item4);
                }
            }
            Console.WriteLine("End of the game. Press any key to close the window.");
            Console.ReadKey();
            /*************** End of game ******************/
        }
    }
}

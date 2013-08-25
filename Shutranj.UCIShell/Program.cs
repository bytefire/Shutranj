using Shutranj.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.UCIShell
{
    class Program
    {
        private const string LogFileName = "logs.txt";
        private const string BestMoveStringFormat = "bestmove {0}";
        static void Main(string[] args)
        {
            // variables
            sbyte ownSide;
            Board board = new Board();
            Stack<BoardState> undoStack = new Stack<BoardState>();

            UInt16 move;
            Tuple<UInt16, int, long, int> moveDepthTimeAndScore;

            string userMove, filthyMindMove = "INVALID_MOVE";
            File.Delete(LogFileName);
            string guiMessage = Console.ReadLine();
            File.AppendAllText(LogFileName, "\nGUI: " + guiMessage + Environment.NewLine);

            Console.WriteLine("id name Shutranj");
            Console.WriteLine("id author Okash Khawaja");
            Console.WriteLine("uciok");

            guiMessage = Console.ReadLine();
            File.AppendAllText(LogFileName, "GUI: " + guiMessage + Environment.NewLine);

            Console.WriteLine("readyok");

            guiMessage = Console.ReadLine();
            File.AppendAllText(LogFileName, "GUI: " + guiMessage + Environment.NewLine);

            guiMessage = Console.ReadLine();
            File.AppendAllText(LogFileName, "GUI: " + guiMessage + Environment.NewLine);

            guiMessage = Console.ReadLine();
            File.AppendAllText(LogFileName, "GUI: " + guiMessage + Environment.NewLine);

            /********** this is when the game begins **************/

            guiMessage = Console.ReadLine();
            File.AppendAllText(LogFileName, "GUI: " + guiMessage + Environment.NewLine);

            if (guiMessage == "quit" || guiMessage == "stop")
            {
                return;
            }
            userMove = ParseOutLastMove(guiMessage);

            guiMessage = Console.ReadLine();
            File.AppendAllText(LogFileName, "GUI: " + guiMessage + Environment.NewLine);

            // if empty then engine is playing white so make move first
            if (String.IsNullOrWhiteSpace(userMove))
            {
                ownSide = Side.White;
                
                moveDepthTimeAndScore = AlphaBeta2.IDASParallel(board, ownSide);

                move = moveDepthTimeAndScore.Item1;
                if (board.MakeMove(move))
                {
                    filthyMindMove = board.ConvertToAlgebraicNotation(move);
                    File.AppendAllText(LogFileName, String.Format(
                        "Filthy Mind: {0} (Depth: {1} plies; Time: {2}ms; Score: {3}){4}", filthyMindMove,
                        moveDepthTimeAndScore.Item2, moveDepthTimeAndScore.Item3, moveDepthTimeAndScore.Item4, Environment.NewLine));
                }

                // communicate move back to gui
                Console.WriteLine(BestMoveStringFormat, filthyMindMove);
            }
            else
            {
                ownSide = Side.Black;
            }

            
                

            while (true)
            {
                guiMessage = Console.ReadLine();
                File.AppendAllText(LogFileName, "GUI: " + guiMessage + Environment.NewLine);

                if (guiMessage == "quit" || guiMessage == "stop")
                {
                    break;
                }
                userMove = ParseOutLastMove(guiMessage);

                guiMessage = Console.ReadLine();
                File.AppendAllText(LogFileName, "GUI: " + guiMessage + Environment.NewLine);

                board.MakeUserMove(userMove);

                moveDepthTimeAndScore = AlphaBeta2.IDASParallel(board, ownSide);

                move = moveDepthTimeAndScore.Item1;
                if (board.MakeMove(move))
                {
                    filthyMindMove = board.ConvertToAlgebraicNotation(move);
                    File.AppendAllText(LogFileName, String.Format(
                        "Filthy Mind: {0} (Depth: {1} plies; Time: {2}ms; Score: {3}){4}", filthyMindMove,
                        moveDepthTimeAndScore.Item2, moveDepthTimeAndScore.Item3, moveDepthTimeAndScore.Item4, Environment.NewLine));
                }

                // communicate move back to gui
                Console.WriteLine(BestMoveStringFormat, filthyMindMove);

            }
        }

        private static string ParseOutLastMove(string moveCommand)
        {
            // sample move command: position startpos e2e4 b8c6
            string[] brokenBySpace = moveCommand.Split(' ');
            if (brokenBySpace.Length < 3)
            {
                return String.Empty;
            }
            return brokenBySpace[brokenBySpace.Length - 1];
        }
    }
}

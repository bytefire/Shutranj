using Shutranj.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            // variables
            sbyte opponentSide, ownSide;
            Board board = new Board();
            Move move;
            int depth = 4;
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
                    opponentSide = Side.White;
                    ownSide = Side.Black;
                    break;
                }
                else if (opponentSideChar == 'b')
                {
                    opponentSide = Side.Black;
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
                move = Search.RootAlphaBetaParallel(board, ownSide, depth).Item1;
                if (board.MakeMove(move))
                {
                    Console.WriteLine("Filthy Mind: {0}", move.ToString());
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
                try
                {
                    move = new Move(opponentSide, opponentMoveString);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: {0}", ex.Message);
                    continue;
                }
                if (!board.MakeMove(move))
                {
                    Console.WriteLine("Failed to make move. Try again.");
                    continue;
                }
                // make move for own side
                move = Search.RootAlphaBetaParallel(board, ownSide, depth).Item1;
                if (board.MakeMove(move))
                {
                    Console.WriteLine("Filthy Mind: {0}", move.ToString());
                }
            }
            Console.WriteLine("End of the game. Press any key to close the window.");
            Console.ReadKey();
            /*************** End of game ******************/
        }
    }
}

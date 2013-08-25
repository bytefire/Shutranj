using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shutranj.Engine;
using System.Diagnostics;

namespace Board2Test
{
    [TestClass]
    public class Board2Test
    {
        [TestMethod]
        public void TestMakingMoves()
        {
            Board board2 = new Board();
            TestingHelper.MakeSomeMoves(board2);

            Debug.WriteLine(board2.ToString());
        }

        [TestMethod]
        public void TestCastling()
        {
            Board board2 = new Board();
            TestingHelper.MakeSomeMoves(board2);


            board2.MakeUserMove("e1c1");

            board2.MakeUserMove("e8c8");

            Debug.WriteLine(board2.ToString());
        }

        [TestMethod]
        public void TestEnPassantCapture()
        {
            Board board2 = new Board();
            TestingHelper.MakeSomeMoves(board2);

            board2.MakeUserMove("c5d6");

            Debug.WriteLine(board2.ToString());
        }

        [TestMethod]
        public void TestPawnPromotion()
        {
            Board board2 = new Board();
            TestingHelper.MakeSomeMoves(board2);

            board2.MakeUserMove("d2e3");

            bool success = board2.MakeUserMove("d5d4");

            success = board2.MakeUserMove("e1g1");

            success = board2.MakeUserMove("d4d3");

            success = board2.MakeUserMove("a1c1");

            success = board2.MakeUserMove("d3d2");

            success = board2.MakeUserMove("f1e1");

            success = board2.MakeUserMove("d2e1q");
        }

        [TestMethod]
        public void TestFirstMoveGeneration()
        {
            Board board = new Board();

            TestingHelper.MakeSomeMoves(board);
            int movesCount;
            UInt16[] moves = board.GenerateMoves(out movesCount);
            Console.WriteLine(movesCount);
        }
    }
}

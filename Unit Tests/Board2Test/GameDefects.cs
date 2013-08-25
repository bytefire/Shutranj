using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shutranj.Engine;

namespace Board2Test
{
    [TestClass]
    public class GameDefects
    {
        [TestMethod]
        public void WhenPlayingAgainstHammad()
        {
            Board board = new Board();

            board.MakeUserMove("d2d4");
            board.MakeUserMove("g8f6");

            board.MakeUserMove("g1f3");
            board.MakeUserMove("d7d5");

            board.MakeUserMove("b1c3");
            board.MakeUserMove("b8c6");

            board.MakeUserMove("c1g5");
            board.MakeUserMove("f6e4");

            board.MakeUserMove("d1d2");
            board.MakeUserMove("e4d2");

            board.MakeUserMove("e1d2");
            board.MakeUserMove("h7h6");

            board.MakeUserMove("g5f4");
            board.MakeUserMove("e7e6");

            board.MakeUserMove("f4e5");
            board.MakeUserMove("f8b4");

            // now do search
            Tuple<UInt16, int> moveAndScore = AlphaBeta2.RootAlphaBetaTTParallel(board, -1, 6);
            board.MakeMove(moveAndScore.Item1);

        }

        [TestMethod]
        public void Defect2VsTitan()
        {
            Board board = new Board();

            board.MakeUserMove("e2e4");
            board.MakeUserMove("c7c6");

            board.MakeUserMove("b1c3");
            board.MakeUserMove("d7d5");

            board.MakeUserMove("e4d5");
            board.MakeUserMove("c6d5");

            board.MakeUserMove("f1b5");
            board.MakeUserMove("b8c6");

            board.MakeUserMove("d2d4");
            board.MakeUserMove("a7a6");

            board.MakeUserMove("b5c6");
            board.MakeUserMove("b7c6");

            board.MakeUserMove("a2a3");
            board.MakeUserMove("g8f6");

            board.MakeUserMove("g1e2");
            board.MakeUserMove("e7e6");

            board.MakeUserMove("e1g1");
            board.MakeUserMove("f8e7");

            board.MakeUserMove("c1g5");
            board.MakeUserMove("e8g8");

            board.MakeUserMove("d1d3");
            board.MakeUserMove("c8b7");

            board.MakeUserMove("h2h3");
            board.MakeUserMove("f8e8");

            board.MakeUserMove("a3a4");
            board.MakeUserMove("h7h6");

            board.MakeUserMove("g5f6");
            board.MakeUserMove("e7f6");

            board.MakeUserMove("c3d1");
            board.MakeUserMove("e6e5");

            board.MakeUserMove("d4e5");
            board.MakeUserMove("f6e5");
            board.MakeUserMove("c2c3");
            board.MakeUserMove("c6c5");

            board.MakeUserMove("d3c2");
            board.MakeUserMove("d5d4");

            board.MakeUserMove("c2b3");
            board.MakeUserMove("b7d5");

            board.MakeUserMove("c3c4");
            board.MakeUserMove("a8b8");

            board.MakeUserMove("b3d3");
            board.MakeUserMove("d5e4");

            board.MakeUserMove("d3d2");
            board.MakeUserMove("d8d6");

            board.MakeUserMove("f2f4");
            board.MakeUserMove("d6g6");

            board.MakeUserMove("g2g4");
            board.MakeUserMove("e5d6");
            board.MakeUserMove("a1a3");
            board.MakeUserMove("g6e6");

            board.MakeUserMove("b2b3");
            board.MakeUserMove("e4c6");

            board.MakeUserMove("e2g3");
            board.MakeUserMove("f7f6");

            board.MakeUserMove("d2c1");
            board.MakeUserMove("a6a5");

            board.MakeUserMove("c1d2");
            board.MakeUserMove("d6c7");

            board.MakeUserMove("d2c1");
            board.MakeUserMove("c7d6");

            board.MakeUserMove("c1d2");
            board.MakeUserMove("b8a8");

            board.MakeUserMove("a3a1");
            board.MakeUserMove("g7g6");
            board.MakeUserMove("d2c2");
            board.MakeUserMove("e6f7");

            board.MakeUserMove("c2d2");
            board.MakeUserMove("f7b7");

            board.MakeUserMove("d2b2");
            board.MakeUserMove("b7d7");

            // Generate move here for white to play
            Tuple<UInt16, int> moveAndScore = AlphaBeta2.RootAlphaBetaTTParallel(board, 1, 6);
            board.MakeMove(moveAndScore.Item1);

        }

        [TestMethod]
        public void Defect3VsTitan_Quiesc()
        {
            Board board = new Board();

            board.MakeUserMove("g1f3");
            board.MakeUserMove("g8f6");

            board.MakeUserMove("d2d4");
            board.MakeUserMove("e7e6");

            board.MakeUserMove("c1g5");
            board.MakeUserMove("c7c5");

            board.MakeUserMove("b1c3");
            board.MakeUserMove("c5d4");

            board.MakeUserMove("f3d4");
            board.MakeUserMove("e6e5");

            board.MakeUserMove("d4b3");
            board.MakeUserMove("f8e7");

            board.MakeUserMove("g5f6");
            board.MakeUserMove("e7f6");

            board.MakeUserMove("e2e4");
            board.MakeUserMove("e8g8");

            board.MakeUserMove("f1c4");
            board.MakeUserMove("b8c6");

            board.MakeUserMove("e1g1");
            board.MakeUserMove("d7d6");

            board.MakeUserMove("f1e1");
            board.MakeUserMove("f6g5");

            board.MakeUserMove("d1d3");
            bool success = board.MakeUserMove("d1d3");

            // Generate move here for white to play
            Tuple<UInt16, int> moveAndScore = AlphaBeta2.RootAlphaBetaTTParallel(board, 1, 6);
            board.MakeMove(moveAndScore.Item1);

        }

        [TestMethod]
        public void Game6VsTitan_BadMoveLostQueen()
        {
            Board board = new Board();

            board.MakeUserMove("b1c3 ");
            board.MakeUserMove("d7d5");

            board.MakeUserMove("e2e3 ");
            board.MakeUserMove("b8c6");

            board.MakeUserMove("f1b5 ");
            board.MakeUserMove("a7a6");

            board.MakeUserMove("b5c6 ");
            board.MakeUserMove("b7c6");

            board.MakeUserMove("d2d4");
            board.MakeUserMove("e7e5");

            board.MakeUserMove("d4e5 ");
            board.MakeUserMove("d8g5");

            board.MakeUserMove("e1f1 ");
            board.MakeUserMove("g5e5");

            board.MakeUserMove("g1f3 ");
            board.MakeUserMove("e5h5");

            board.MakeUserMove("d1d4 ");
            board.MakeUserMove("c8f5");

            board.MakeUserMove("d4a4 ");
            board.MakeUserMove("f5d7");

            board.MakeUserMove("a4b3 ");
            board.MakeUserMove("a6a5");

            board.MakeUserMove("b3b7  ");
            board.MakeUserMove("a8c8");

            board.MakeUserMove("c3e2  ");
            board.MakeUserMove("f8d6");

            board.MakeUserMove("e2f4  ");
            board.MakeUserMove("h5f5");

            board.MakeUserMove("f3d4  ");
            board.MakeUserMove("f5f6 ");

            board.MakeUserMove("d4c6 ");
            board.MakeUserMove("d6f4");

            board.MakeUserMove("c6a7  ");
            board.MakeUserMove("f4e3");

            board.MakeUserMove("c1e3  ");
            board.MakeUserMove("c8d8");

            board.MakeUserMove("a1e1  ");
            board.MakeUserMove("g8e7");

            board.MakeUserMove("e3c5  ");
            board.MakeUserMove("d7e6");

            board.MakeUserMove("c5e7  ");
            board.MakeUserMove("e8e7");

            board.MakeUserMove("b7c7   ");
            board.MakeUserMove("e7e8");

            board.MakeUserMove("a7c6   ");
            board.MakeUserMove("d5d4");

            board.MakeUserMove("h1g1   ");
            board.MakeUserMove("g7g6");

            board.MakeUserMove("c2c3   ");
            board.MakeUserMove("d8a8");

            board.MakeUserMove("c7b7   ");
            board.MakeUserMove("a8d8");

            board.MakeUserMove("c6d8   ");
            board.MakeUserMove("f6d8");

            board.MakeUserMove("b7b5   ");
            board.MakeUserMove("e8f8");

            board.MakeUserMove("e1d1   ");
            board.MakeUserMove("e6a2");

            board.MakeUserMove("d1d4   ");
            board.MakeUserMove("d8c7");

            board.MakeUserMove("b5a6   ");
            board.MakeUserMove("f8g7");

            board.MakeUserMove("f2f4    ");
            board.MakeUserMove("h8d8");


            // Generate move here for white to play
            Tuple<UInt16, int> moveAndScore = AlphaBeta2.RootAlphaBetaTTParallel(board, 1, 7);
            board.MakeMove(moveAndScore.Item1);



            // following was bad move by FM
            board.MakeUserMove("d4d8");
            board.MakeUserMove("a2c4");

        }

        [TestMethod]
        public void Game7VsTitan_NotCheckmating()
        {
            Board board = new Board();

            board.MakeUserMove("d2d4  ");
            board.MakeUserMove("d7d5");

            board.MakeUserMove("c2c3  ");
            board.MakeUserMove("g8f6");

            board.MakeUserMove("g2g3  ");
            board.MakeUserMove("e7e6");

            board.MakeUserMove("c1g5  ");
            board.MakeUserMove("f8d6");

            board.MakeUserMove("e2e4 ");
            board.MakeUserMove("d5e4");

            board.MakeUserMove("b1d2  ");
            board.MakeUserMove("d6e7");

            board.MakeUserMove("d1a4  ");
            board.MakeUserMove("d8d7");

            board.MakeUserMove("a4c2  ");
            board.MakeUserMove("e8g8");

            board.MakeUserMove("f1h3  ");
            board.MakeUserMove("b8c6");

            board.MakeUserMove("g5f6  ");
            board.MakeUserMove("g7f6");

            board.MakeUserMove("c2e4  ");
            board.MakeUserMove("f6f5");

            board.MakeUserMove("e4e3   ");
            board.MakeUserMove("b7b5");

            board.MakeUserMove("h3g2   ");
            board.MakeUserMove("c8b7");

            board.MakeUserMove("g1f3   ");
            board.MakeUserMove("f7f6");

            board.MakeUserMove("f3h4   ");
            board.MakeUserMove("f8b8 ");

            board.MakeUserMove("d2b3  ");
            board.MakeUserMove("b5b4");

            board.MakeUserMove("b3c5   ");
            board.MakeUserMove("e7c5");

            board.MakeUserMove("d4c5   ");
            board.MakeUserMove("a7a5");

            board.MakeUserMove("a2a3   ");
            board.MakeUserMove("e6e5");

            board.MakeUserMove("a1d1   ");
            board.MakeUserMove("d7e6");

            board.MakeUserMove("g2d5   ");
            board.MakeUserMove("e6d5");

            board.MakeUserMove("d1d5    ");
            board.MakeUserMove("c6e7");

            board.MakeUserMove("d5d7    ");
            board.MakeUserMove("b7h1");

            board.MakeUserMove("d7e7    ");
            board.MakeUserMove("b4a3 ");

            board.MakeUserMove("e3h6    ");
            board.MakeUserMove("g8h8");


            // Generate move here for white to play
            Tuple<UInt16, int> moveAndScore = AlphaBeta2.RootAlphaBetaTT(board, 1, 6);
                // AlphaBeta2.RootAlphaBetaTTParallel(board, 1, 6);
            board.MakeMove(moveAndScore.Item1);



            // following was bad move by FM
            board.MakeUserMove("d4d8");
            board.MakeUserMove("a2c4");

        }

        [TestMethod]
        public void TestQuiescenceDepth()
        {
            Board board = new Board();

            // Generate move here for white to play
            Tuple<UInt16, int> moveAndScore = AlphaBeta2.RootAlphaBetaTT(board, 1, 6);
            // AlphaBeta2.RootAlphaBetaTTParallel(board, 1, 6);
            board.MakeMove(moveAndScore.Item1);



            // following was bad move by FM
            board.MakeUserMove("d4d8");
            board.MakeUserMove("a2c4");

        }
    }
}

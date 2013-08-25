using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shutranj.Engine;
using System.Diagnostics;

namespace BoardRepresentationTests
{
    [TestClass]
    public class RayTests
    {
        [TestMethod]
        public void TestEastRayInitialisation()
        {
            Initialiser eastRayBitboard = new Initialiser();

            UInt64[] eastRayBoard = eastRayBitboard.InitialiseEast();
            TestingHelper helper = new TestingHelper();
            string eastRayBoardString = helper.BitboardToBoardString(eastRayBoard[0]);
            Debug.WriteLine(eastRayBoardString);
        }

        [TestMethod]
        public void TestWestRayInitialisation()
        {
            Initialiser westRayBitboard = new Initialiser();

            UInt64[] westRayBoard = westRayBitboard.InitialiseWest();
            TestingHelper helper = new TestingHelper();
            string westRayBoardString = helper.BitboardToBoardString(westRayBoard[48]);
            Debug.WriteLine(westRayBoardString);
        }

        [TestMethod]
        public void TestFileRaysInitialisation()
        {
            Initialiser fileRayBitboard = new Initialiser();
            UInt64[] fileRays = fileRayBitboard.InitialiseFileRays();
            TestingHelper helper = new TestingHelper();
            string fileRayBoardString = helper.BitboardToBoardString(fileRays[5]);
            Debug.WriteLine(fileRayBoardString);
        }

        [TestMethod]
        public void TestNorthRayInitialisation()
        {
            Initialiser northRayBitboard = new Initialiser();

            UInt64[] northRayBoard = northRayBitboard.InitialiseNorth();
            TestingHelper helper = new TestingHelper();
            string northRayBoardString = helper.BitboardToBoardString(northRayBoard[55]);
            Debug.WriteLine(northRayBoardString);
        }

        [TestMethod]
        public void TestSouthRayInitialisation()
        {
            Initialiser southRayBitboard = new Initialiser();

            UInt64[] southRayBoard = southRayBitboard.InitialiseSouth();
            TestingHelper helper = new TestingHelper();
            string southRayBoardString = helper.BitboardToBoardString(southRayBoard[63]);
            Debug.WriteLine(southRayBoardString);
        }

        [TestMethod]
        public void TestDiagonalValues()
        {
            TestingHelper helper = new TestingHelper();
            Initialiser bitboards = new Initialiser();
            UInt64[] rankDiags = bitboards.InitialiseRankDiagonals();
            UInt64 diag = rankDiags[0] << 18;
            string boardString = helper.BitboardToBoardString(diag);
            Debug.WriteLine(boardString);
        }

        [TestMethod]
        public void TestDiagonalForSquare()
        {
            TestingHelper helper = new TestingHelper();
            Initialiser bitboards = new Initialiser();
            UInt64 diag = bitboards.GetDiagonalForSquare(18);
            string boardString = helper.BitboardToBoardString(diag);
            Debug.WriteLine(boardString);
        }

        [TestMethod]
        public void TestFirstSquareOfDiagonal()
        {
            TestingHelper helper = new TestingHelper();
            Initialiser bitboards = new Initialiser();
            UInt64[] rankDiags = bitboards.InitialiseRankDiagonals();
            UInt64 diag = rankDiags[1];
            
            UInt64 firstSquare = diag & ~(diag - 1);
            // OptimiseTODO: consider bit-shifting instead of log. see http://community.topcoder.com/tc?module=Static&d1=tutorials&d2=bitManipulation
            int firstSquareIndex = (int)Math.Log(firstSquare, 2);
            Debug.WriteLine(firstSquareIndex);
            string boardString = helper.BitboardToBoardString(firstSquare);
            Debug.WriteLine(boardString);
        }

        [TestMethod]
        public void TestNorthEastRayInitialisation()
        {
            Initialiser northEastRayBitboard = new Initialiser();

            UInt64[] northEastRayBoard = northEastRayBitboard.InitialiseNorthEast();
            TestingHelper helper = new TestingHelper();
            string northEastRayBoardString = helper.BitboardToBoardString(northEastRayBoard[8]);
            Debug.WriteLine(northEastRayBoardString);
        }

        [TestMethod]
        public void TestMS1BMask()
        {
            TestingHelper testingHelper = new TestingHelper();
            Initialiser bitboards = new Initialiser();
            UInt64[] rankDiags = bitboards.InitialiseRankDiagonals();
            UInt64 diag = rankDiags[1];

            BitHelper bitHelper = new BitHelper();

            UInt64 mask = bitHelper.GetMostSignificant1BitMask(diag);
            Debug.WriteLine("Diagonal:");
            Debug.WriteLine(testingHelper.BitboardToBoardString(diag));
            Debug.WriteLine("Mask:");
            Debug.WriteLine(testingHelper.BitboardToBoardString(mask));
        }

        [TestMethod]
        public void TestMS1B()
        {
            TestingHelper testingHelper = new TestingHelper();
            Initialiser bitboards = new Initialiser();
            UInt64[] rankDiags = bitboards.InitialiseRankDiagonals();
            UInt64 diag = Constants.MainDiagonal;
                //rankDiags[1];

            BitHelper bitHelper = new BitHelper();

            UInt64 ms1b = bitHelper.GetMostSignificant1Bit(diag);
            Debug.WriteLine("Diagonal:");
            Debug.WriteLine(testingHelper.BitboardToBoardString(diag));
            Debug.WriteLine("MS1B:");
            Debug.WriteLine(testingHelper.BitboardToBoardString(ms1b));
        }

        [TestMethod]
        public void TestMS1BIndex()
        {
            TestingHelper testingHelper = new TestingHelper();
            Initialiser bitboards = new Initialiser();
            UInt64[] rankDiags = bitboards.InitialiseRankDiagonals();
            UInt64 diag = rankDiags[1];

            BitHelper bitHelper = new BitHelper();

            int ms1bIndex = BitHelper.GetMostSignificant1BitIndex2(diag);
                // bitHelper.GetMostSignificant1BitIndex(diag);
            Debug.WriteLine("Diagonal:");
            Debug.WriteLine(testingHelper.BitboardToBoardString(diag));
        }

        [TestMethod]
        public void TestSouthWestRayInitialisation()
        {
            Initialiser southWestRayBitboard = new Initialiser();

            UInt64[] southWestRayBoard = southWestRayBitboard.InitialiseSouthWest();
            TestingHelper helper = new TestingHelper();
            string southWestRayBoardString = helper.BitboardToBoardString(southWestRayBoard[34]);
            Debug.WriteLine(southWestRayBoardString);
        }

        [TestMethod]
        public void TestAntiDiagonals()
        {
            TestingHelper helper = new TestingHelper();
            Initialiser antiDiagsInitialiser = new Initialiser();
            UInt64[] fileAntiDiags = antiDiagsInitialiser.InitialiseFileAntiDiagonals();
            UInt64 northWestRay = fileAntiDiags[5]>>7;
            string antiDiagonalBoardString = helper.BitboardToBoardString(northWestRay);
            Debug.WriteLine(antiDiagonalBoardString);
        }

        [TestMethod]
        public void TestAntiDiagonalForSquare()
        {
            TestingHelper helper = new TestingHelper();
            Initialiser antiDiagsForSquare = new Initialiser();

            UInt64 antiDiag = antiDiagsForSquare.GetAntiDiagonalForSquare(63);
            string boardString = helper.BitboardToBoardString(antiDiag);
            Debug.WriteLine(boardString);
        }

        [TestMethod]
        public void TestNorthWestRayInitialisation()
        {
            Initialiser northWestRayBitboard = new Initialiser();

            UInt64[] northWestRayBoard = northWestRayBitboard.InitialiseNorthWest();
            TestingHelper helper = new TestingHelper();
            string northWestRayBoardString = helper.BitboardToBoardString(northWestRayBoard[19]);
            Debug.WriteLine(northWestRayBoardString);
        }

        [TestMethod]
        public void TestSouthEastRayInitialisation()
        {
            Initialiser southEastRayBitboard = new Initialiser();

            UInt64[] southEastRayBoard = southEastRayBitboard.InitialiseSouthEast();
            TestingHelper helper = new TestingHelper();
            string southEastRayBoardString = helper.BitboardToBoardString(southEastRayBoard[52]);
            Debug.WriteLine(southEastRayBoardString);
        }
    }
}
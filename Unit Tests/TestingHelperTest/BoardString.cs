using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shutranj.Engine;
using System.Diagnostics;

namespace TestingHelperTest
{
    [TestClass]
    public class BoardString
    {
        [TestMethod]
        public void TestConversionToBinary()
        {
            UInt64 bitboard = 1;
            TestingHelper helper = new TestingHelper();
            string binaryString = helper.BitboardToBinaryString(bitboard);

            Assert.AreEqual(binaryString, "1".PadLeft(64, '0'));
        }

        [TestMethod]
        public void TestConversionToBitboard()
        {
            UInt64 bitboard = 1000965;
            TestingHelper helper = new TestingHelper();
            string boardString = helper.BitboardToBoardString(bitboard);

            Debug.WriteLine(boardString);
        }
    }
}

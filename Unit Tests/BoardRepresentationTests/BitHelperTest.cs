using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shutranj.Engine;

namespace BoardRepresentationTests
{
    [TestClass]
    public class BitHelperTest
    {
        [TestMethod]
        public void NumberSetBitsTest()
        {
            int setBitsCount =  BitHelper.GetNumberOfSetBits(235562);
            Assert.AreEqual(8, setBitsCount);
        }

        [TestMethod]
        public void GetLS1BIndexTest()
        {
            int ls1bIndex = BitHelper.GetLeastSignificant1BitIndex2(140737488355328);
            Assert.AreEqual(47, ls1bIndex);
        }

        [TestMethod]
        public void GetMS1BIndexTest()
        {
            int ms1bIndex = BitHelper.GetMostSignificant1BitIndex2(562949953421315);
            Assert.AreEqual(49, ms1bIndex);
        }

        [TestMethod]
        public void TestGetSetBitIndexes()
        {
            byte[] setBitIndexes = BitHelper.GetSetBitIndexes2(235562);
            Assert.AreNotEqual(setBitIndexes.Length, 0);
        }
    }
}

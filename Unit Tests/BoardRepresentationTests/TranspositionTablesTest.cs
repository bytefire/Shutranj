using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shutranj.Engine;

namespace BoardRepresentationTests
{
    [TestClass]
    public class TranspositionTablesTest
    {
        [TestMethod]
        public void TestEntryHelper()
        {
            UInt64 data = TranspositionTableEntryHelper.ComposeData(-341, 0x0112, 7, 2);

            int score = TranspositionTableEntryHelper.GetScore(data);
            Assert.AreEqual(-341, score);

            UInt16 bestMoveIndex = TranspositionTableEntryHelper.GetBestMove(data);
            Assert.AreEqual(0x0112, bestMoveIndex);

            int depthSearched = TranspositionTableEntryHelper.GetDepthSearched(data);
            Assert.AreEqual(7, depthSearched);

            int entryType = TranspositionTableEntryHelper.GetEntryType(data);
            Assert.AreEqual(2, entryType);
        }
    }
}

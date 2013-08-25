using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.Engine
{
    public struct TranspositionTableEntry
    {
        // NOTE: Separating this class and TranspositionTableEntryHelper class in order to make this class light weight.
        //      This class instances will be saved in the transposition table. A 64-bit copy operation is expected to be atomic.
        //      The arrangement to separate this class from helper class is to enable lockless transposition table for parallel
        //      alpha-beta search as shown here: http://www.cis.uab.edu/hyatt/hashing.html.

        public UInt64 ZobristHash;
        public UInt64 Data;

        /***** Composition of Data *********
         * 
         * Bits 2-0: entryType (3 bits)
         * Bits 7-3: depthSearched (5 bits)
         * Bits 15-8: bestMoveIndex (byte)
         * Bits 47-16: score (Int32)
         * Bits 63-48: reserved for future use (16 bits)
         * 
         * **********************************/

        public TranspositionTableEntry(UInt64 hash, UInt64 data)
        {
            ZobristHash = hash;
            Data = data;
        }

        public bool IsValid()
        {
            return (Data != 0);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.Engine
{
    public class TranspositionTable
    {
        private readonly TranspositionTableEntry InvalidEntry = new TranspositionTableEntry(0, 0);

        private const int TableSize = 18000000;
        // NOTE: make sure to change this when you change TableSize.
        private const UInt64 TableIndexMask = 0x00000000000003FF; // max value = 1023

        private TranspositionTableEntry[] _entries = new TranspositionTableEntry[TableSize];

        public void Add(TranspositionTableEntry entry)
        {
            // NOTE: in case this still causes problems in multi-threaded environment (how do we know there are problems??)
            //          then consider using a Tuple<UInt64, UInt64> instead.

            // based upon Lockless Transposition Table described here: http://www.cis.uab.edu/hyatt/hashing.html
            //int index = (int)(entry.ZobristHash & TableIndexMask);
            int index = (int)(entry.ZobristHash % TableSize);
            entry.ZobristHash ^= entry.Data;
            _entries[index] = entry;

        }

        public TranspositionTableEntry Retrieve(UInt64 hash)
        {

            // based upon Lockless Transposition Table described here: http://www.cis.uab.edu/hyatt/hashing.html
            //int index = (int)(hash & TableIndexMask);
            int index = (int)(hash % TableSize);

            TranspositionTableEntry entry = _entries[index];
            if ((entry.ZobristHash ^ entry.Data) == hash)
            {
                return entry;
            }
            return InvalidEntry;

        }
    }
}

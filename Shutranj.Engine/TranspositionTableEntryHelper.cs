using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.Engine
{
    public static class TranspositionTableEntryHelper
    {
        public const byte EntryTypeExactValue = 1;
        public const byte EntryTypeLowerBound = 2;
        public const byte EntryTypeUpperBound = 3;

        /***** Composition of Data *********
         * 
         * Bits 2-0: entryType (3 bits)
         * Bits 7-3: depthSearched (5 bits)
         * Bits 23-8: best move (UInt16)
         * Bits 55-24: score (Int32)
         * Bits 63-56: reserved for future use (8 bits)
         * 
         * **********************************/

        private const UInt64 LeastSignificant16BitsMask = 0x000000000000FFFF;
        private const UInt64 LeastSignificant8BitsMask = 0x00000000000000FF;
        private const UInt64 LeastSignificant3BitsMask = 0x0000000000000007;

        private const int LengthOfMoveDepthAndEntryTypeInBits = 24;
        private const int LengthOfDepthAndEntryTypeInBits = 8;
        private const int LengthOfEntryTypeInBits = 3;
        public const int InvalidMove = 0x0000;
        
        public static int GetScore(UInt64 data)
        {
            return (int)(data >> LengthOfMoveDepthAndEntryTypeInBits);
        }

        public static UInt16 GetBestMove(UInt64 data)
        {
            return (UInt16)((data >> LengthOfDepthAndEntryTypeInBits) & LeastSignificant16BitsMask);
        }

        public static byte GetDepthSearched(UInt64 data)
        {
            UInt64 depthSearchedBits = data & LeastSignificant8BitsMask;
            return (byte)(depthSearchedBits >> LengthOfEntryTypeInBits);
        }

        public static byte GetEntryType(UInt64 data)
        {
            return (byte)(data & LeastSignificant3BitsMask);
        }

        public static UInt64 ComposeData(int score, UInt16 bestMove, byte depthSearched, byte entryType)
        {
            // create 24 empty spaces to right for best move index, depth and entry type.
            UInt64 data = ((UInt64)score) << LengthOfMoveDepthAndEntryTypeInBits;
            // create space on right for 8 bits of depth + entry type.
            UInt64 temp = ((UInt64)bestMove) << LengthOfDepthAndEntryTypeInBits;
            data ^= temp;
            // create 3 bits space on right for entry type.
            temp = ((UInt64)depthSearched) << LengthOfEntryTypeInBits;
            temp ^= (UInt64)entryType;

            data ^= temp;

            return data;
        }
    }
}

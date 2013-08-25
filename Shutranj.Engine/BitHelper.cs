using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.Engine
{
    public class BitHelper
    {
        private static byte[] Index64 = new byte[64] 
        {
                0, 47,  1, 56, 48, 27,  2, 60,
               57, 49, 41, 37, 28, 16,  3, 61,
               54, 58, 35, 52, 50, 42, 21, 44,
               38, 32, 29, 23, 17, 11,  4, 62,
               46, 55, 26, 59, 40, 36, 15, 53,
               34, 51, 20, 43, 31, 22, 10, 45,
               25, 39, 14, 33, 19, 30,  9, 24,
               13, 18,  8, 12,  7,  6,  5, 63
        };
        /// <summary>
        /// Gets the zero-based index from LSB of the first set bit (i.e. bit with value 1).
        /// </summary>
        /// <param name="val">Value to search for LS1B</param>
        /// <returns>Zero-based integer index of LS1B.</returns>
        public static byte GetLeastSignificant1BitIndex2(UInt64 val)
        {
            // from: https://chessprogramming.wikispaces.com/BitScan#Bitscan%20forward-De%20Bruijn%20Multiplication-With%20separated%20LS1B
            UInt64 debruijn64 = 0x03f79d71b4cb0a89;
            return Index64[((val ^ (val - 1)) * debruijn64) >> 58];
        }

        public static UInt64 GetLeastSignificantBit(UInt64 val)
        {
            return val & ~(val - 1); ;
        }

        // OptmiseTODO: room for optimisation by getting rid of the loop.
        /// <summary>
        /// Gets a mask with least significant 1 bit and all more significant bits set to 1
        /// for the given value.
        /// </summary>
        /// <param name="val">Value for which the mask is needed.</param>
        /// <returns>The mask.</returns>
        public UInt64 GetLeastSignificant1BitMask(UInt64 val)
        {
            UInt64 leastSignificant1Bit = val & ~(val - 1);
            UInt64 mask = leastSignificant1Bit;
            for (int i = 1; i <= 63; i++)
            {
                mask = mask | (leastSignificant1Bit << i);
            }
            return mask;
        }
        /// <summary>
        /// Gets a mask with most significant 1 bit and all lesser significant bits set to 1
        /// for the given value.
        /// </summary>
        /// <param name="val">Value for which the mask is needed.</param>
        /// <returns>The mask.</returns>
        public UInt64 GetMostSignificant1BitMask(UInt64 val)
        {
            // Implementation based on http://www.cs.utoronto.ca/~neto/code/first1bit.html 
            val = val | (val >> 1);
            val = val | (val >> 2);
            val = val | (val >> 4);
            val = val | (val >> 8);
            val = val | (val >> 16);
            val = val | (val >> 32);
            return val;
        }

        public UInt64 GetMostSignificant1Bit(UInt64 val)
        {
            // see http://aggregate.org/MAGIC/#Most%20Significant%201%20Bit
            UInt64 ms1b = GetMostSignificant1BitMask(val);
            ms1b = ms1b & ~(ms1b >> 1);
            return ms1b;
        }

        public static int GetMostSignificant1BitIndex2(UInt64 val)
        {
            UInt64 debruijn64 = 0x03f79d71b4cb0a89;
            val |= val >> 1;
            val |= val >> 2;
            val |= val >> 4;
            val |= val >> 8;
            val |= val >> 16;
            val |= val >> 32;
            return Index64[(val * debruijn64) >> 58];
        }

        public static byte[] GetSetBitIndexes2(UInt64 bitboard)
        {
            // from: https://chessprogramming.wikispaces.com/Bitboard+Serialization#Converting%20Sets%20to%20Lists-Square%20Index%20Serialization-Scanning%20Forward
            List<byte> indexes = new List<byte>(64);
            while (bitboard != 0)
            {
                indexes.Add(GetLeastSignificant1BitIndex2(bitboard));
                bitboard &= bitboard - 1;
            }
            return indexes.ToArray();
        }

        /*
        public int[] GetSetBitIndexes2(UInt64 bitboard)
        {
            List<int> indexes = new List<int>();
        }
        */

        public static int GetNumberOfSetBits(UInt64 bitboard)
        {
            bitboard = bitboard - ((bitboard >> 1) & 0x5555555555555555UL);
            bitboard = (bitboard & 0x3333333333333333UL) + ((bitboard >> 2) & 0x3333333333333333UL);
            return (int)(unchecked(((bitboard + (bitboard >> 4)) & 0xF0F0F0F0F0F0F0FUL) * 0x101010101010101UL) >> 56);
        }
    }
}

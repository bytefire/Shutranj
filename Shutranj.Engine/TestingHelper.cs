using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.Engine
{
    public class TestingHelper
    {
        /// <summary>
        /// Gets the binary representation of bitboard as it would appear if it was a chess board.
        /// LBS = a1, 8th bit = h1 and MSB = h8.
        /// </summary>
        /// <param name="bitboard">The bitboard to convert</param>
        /// <returns>A string representation of the bitboard.</returns>
        public string BitboardToBoardString(UInt64 bitboard)
        {
            string binaryString = BitboardToBinaryString(bitboard);
            string bitboardString = BinaryToBoardString(binaryString);
            return bitboardString;
        }
        /// <summary>
        /// Converts a UInt64 to binary string representation.
        /// </summary>
        /// <param name="bitboard">The UInt64 bitboard to convert.</param>
        /// <returns>String containing 64 binary characters.</returns>
        public string BitboardToBinaryString(UInt64 bitboard)
        {
            char[] binaryCharArray = new char[64];
            UInt64 bitIndex = 63;
            for (int shiftCount = 0; shiftCount < 64; shiftCount++)
            {
                if ((bitboard & ((UInt64)1 << shiftCount)) > 0)
                {
                    binaryCharArray[bitIndex] = '1';
                }
                else
                {
                    binaryCharArray[bitIndex] = '0';
                }
                bitIndex--;
            }

            string binaryString = new String(binaryCharArray);
            return binaryString;
        }
        /// <summary>
        /// Converts a 64-bit binary string to chess board string with a1=LSB, h1=8th bit and h8=MSB.
        /// </summary>
        /// <param name="binaryString">Binary string to convert.</param>
        /// <returns>Chess board representation of binary string.</returns>
        public string BinaryToBoardString(string binaryString)
        {
            if (binaryString.Length != 64)
            {
                throw new ArgumentException("Binary string not 64 characters long.");
            }

            string reversed = new String(binaryString.Reverse().ToArray());
            StringBuilder bitboardString = new StringBuilder();
            for (int multiplier = 7, index; multiplier >= 0; multiplier--)
            {
                index = 8 * multiplier;
                bitboardString.AppendLine(reversed.Substring(index, 8));
            }
            bitboardString.Remove(bitboardString.Length - 1, 1);
            return bitboardString.ToString();
        }

        public static void MakeSomeMoves(Board board)
        {
            List<UInt64> hashesSoFar = new List<UInt64>();
            hashesSoFar.Add(board.ZobristHash);
            bool success;

            board.MakeUserMove("e2e4");

            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            string fenString = board.ToString();
            Debug.WriteLine(fenString);

            board.MakeUserMove("c7c5");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            fenString = board.ToString();
            Debug.WriteLine(fenString);

            board.MakeUserMove("g1f3");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            fenString = board.ToString();
            Debug.WriteLine(fenString);

            board.MakeUserMove("b8c6");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            fenString = board.ToString();
            Debug.WriteLine(fenString);

            board.MakeUserMove("d2d4");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            fenString = board.ToString();
            Debug.WriteLine(fenString);

            board.MakeUserMove("e7e6");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            fenString = board.ToString();
            Debug.WriteLine(fenString);


            board.MakeUserMove("c1e3");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            fenString = board.ToString();
            Debug.WriteLine(fenString);

            board.MakeUserMove("g7g6");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            fenString = board.ToString();
            Debug.WriteLine(fenString);

            board.MakeUserMove("d4c5");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            fenString = board.ToString();
            Debug.WriteLine(fenString);

            board.MakeUserMove("f8g7");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            board.MakeUserMove("f1c4");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            board.MakeUserMove("g8f6");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            board.MakeUserMove("b1c3");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            board.MakeUserMove("b7b6");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            board.MakeUserMove("d1d2");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            board.MakeUserMove("c8b7");

            board.MakeUserMove("e3f4");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            fenString = board.ToString();
            Debug.WriteLine(fenString);

            success = board.MakeUserMove("d8e7");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            success = board.MakeUserMove("e4e5");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            success = board.MakeUserMove("d7d5");
            if (hashesSoFar.Contains(board.ZobristHash))
            {
                throw new InvalidOperationException("Hash Already Included!!");
            }
            hashesSoFar.Add(board.ZobristHash);

            fenString = board.ToString();
            Debug.WriteLine(fenString);
        }
    }
}

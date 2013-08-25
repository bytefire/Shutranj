using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.Engine
{
    public class Initialiser
    {
        private BitHelper _bitHelper = new BitHelper();
        UInt64[] _rankDiagonals;
        UInt64[] _fileDiagonals;
        UInt64[] _rankAntiDiagonals;
        UInt64[] _fileAntiDiagonals;
        
        public Initialiser()
        {
            _rankDiagonals = InitialiseRankDiagonals();
            _fileDiagonals = InitialiseFileDiagonals();
            _rankAntiDiagonals = InitialiseRankAntiDiagonals();
            _fileAntiDiagonals = InitialiseFileAntiDiagonals();
        }

        public UInt64[][] GenerateRayAttacks()
        {
            UInt64[][] rayAttacks = new UInt64[8][];
            rayAttacks[RayDirection.East] = InitialiseEast();
            rayAttacks[RayDirection.West] = InitialiseWest();
            rayAttacks[RayDirection.North] = InitialiseNorth();
            rayAttacks[RayDirection.South] = InitialiseSouth();
            rayAttacks[RayDirection.NorthEast] = InitialiseNorthEast();
            rayAttacks[RayDirection.SouthWest] = InitialiseSouthWest();
            rayAttacks[RayDirection.NorthWest] = InitialiseNorthWest();
            rayAttacks[RayDirection.SouthEast] = InitialiseSouthEast();

            return rayAttacks;
        }

        public UInt64[] InitialiseEast()
        {
            UInt64[] eastRay = new UInt64[64];
            UInt64 currentRay = 0;
            UInt64 shiftedOne = 0; 
            for (int i = 64; i > 0; i--)
            {
                if (i % 8 == 0)
                {
                    currentRay = 0;
                }
                else
                {
                    shiftedOne = ((UInt64)1) << i;
                    currentRay = currentRay | shiftedOne;
                }
                eastRay[i - 1] = currentRay;
            }
            return eastRay;
        }

        public UInt64[] InitialiseWest()
        {
            UInt64[] westRay = new UInt64[64];
            UInt64 currentRay = 0;
            UInt64 shiftedOne = 0;

            for (int i = 0; i < 64; i++)
            {
                if (i % 8 == 0)
                {
                    currentRay = 0;
                }
                else
                {
                    shiftedOne = ((UInt64)1) << (i - 1);
                    currentRay = currentRay | shiftedOne;
                }
                westRay[i] = currentRay;
            }
            return westRay;
        }

        public UInt64[] InitialiseNorth()
        {
            // OptimiseTODO: can this be implemented without method call to initialise file rays? that would save space,
            //              and overhead of extra method call.
            UInt64[] northRays = new UInt64[64];

            // initlialise file rays
            UInt64[] fileRays = InitialiseFileRays();
            // initialise north
            int file = 0;
            int shiftBy = 0;
            for (int i = 0; i < 64; i++)
            {
                file = i % 8;
                // following division followed by multiplication ensures that the shift is a always by multiples of 8.
                shiftBy = (i / 8) * 8 + 8;
                // special case for shifting by 64 bits because of the way bit shifting is computed in C# 3.0 language
                // specification.
                // 7.8 "Shift operators":
                // When the type of x is long or ulong, the shift count is given by the low-order six bits of count. 
                // In other words, the shift count is computed from count & 0x3F.
                // See http://stackoverflow.com/questions/2505550/c-shift-left-assignment-operator-behavior
                if (shiftBy == 64)
                {
                    northRays[i] = 0;
                }
                else
                {
                    northRays[i] = fileRays[file] << shiftBy;
                }
            }
            return northRays;
        }

        public UInt64[] InitialiseSouth()
        {
            // OptimiseTODO: can this be implemented without method call to initialise file rays? that would save space,
            //              and overhead of extra method call.

            UInt64[] southRays = new UInt64[64];

            // initlialise file rays
            UInt64[] fileRays = InitialiseFileRays();
            // initialise north
            int file = 0; int temp = 0;
            int shiftBy = 0;
            for (int i = 0; i < 64; i++)
            {
                file = i % 8;
                temp = 63 - i;
                // following division followed by multiplication ensures that the shift is a always by multiples of 8.
                shiftBy = (temp / 8) * 8 + 8;
                // special case for shifting by 64 bits because of the way bit shifting is computed in C# 3.0 language
                // specification.
                // 7.8 "Shift operators":
                // When the type of x is long or ulong, the shift count is given by the low-order six bits of count. 
                // In other words, the shift count is computed from count & 0x3F.
                // See http://stackoverflow.com/questions/2505550/c-shift-left-assignment-operator-behavior
                if (shiftBy == 64)
                {
                    southRays[i] = 0;
                }
                else
                {
                    southRays[i] = fileRays[file] >> shiftBy;
                }
            }
            return southRays;
        }

        public UInt64[] InitialiseNorthEast()
        {
            UInt64[] northEastRays = new UInt64[64];
            UInt64 diagonal = 0;
            UInt64 mask = 0;
            int firstSquareIndex = 0;
            int shiftBy = 0;
            for (int i = 0; i < 64; i++)
            {
                diagonal = GetDiagonalForSquare(i);
                mask = _bitHelper.GetMostSignificant1BitMask(diagonal);
                firstSquareIndex = BitHelper.GetLeastSignificant1BitIndex2(diagonal);
                    //_bitHelper.GetLeastSignificant1BitIndex(diagonal);
                shiftBy = i - firstSquareIndex + 9;
                // this check is important because of the peculair behaviour when left shifting a number whose MSB is set!
                // it gives odd results.
                if (shiftBy > 63)
                {
                    northEastRays[i] = 0;
                }
                else
                {
                    northEastRays[i] = diagonal << shiftBy;
                }
                // clear any bits that are above the diagonal ray (this happens in rank diagonals; for file diagonals
                //  and main diagonal the shift causes the 1 bits to drop off the left end of the 64-bit sequence).
                northEastRays[i] = northEastRays[i] & mask;
            }
            return northEastRays;
        }

        public UInt64[] InitialiseSouthWest()
        {
            UInt64[] southWestRays = new UInt64[64];
            UInt64 diagonal = 0;
            UInt64 mask = 0;
            int lastSquareIndex = 0;
            int shiftBy = 0;

            for (int i = 63; i >= 0; i--)
            {
                diagonal = GetDiagonalForSquare(i);
                mask = _bitHelper.GetLeastSignificant1BitMask(diagonal);
                lastSquareIndex = BitHelper.GetMostSignificant1BitIndex2(diagonal);

                shiftBy = lastSquareIndex - i + 9;

                // this check is due to a peculiarity of .NET that when shifting right by more than 63 on a UInt64,
                // it gives unreliable results.
                if (shiftBy > 63)
                {
                    southWestRays[i] = 0;
                }
                else
                {
                    southWestRays[i] = diagonal >> shiftBy;
                }
                // remove any bits that wrap around when shifted right.
                southWestRays[i] = southWestRays[i] & mask;
            }
            return southWestRays;
        }

        public UInt64[] InitialiseNorthWest()
        {
            UInt64[] northWestRays = new UInt64[64];
            UInt64 antiDiagonal = 0;
            UInt64 mask = 0;
            int firstSquareIndex = 0;
            int shiftBy = 0;

            for (int i = 0; i < 64; i++)
            {
                antiDiagonal = GetAntiDiagonalForSquare(i);
                mask = _bitHelper.GetMostSignificant1BitMask(antiDiagonal);
                firstSquareIndex = BitHelper.GetLeastSignificant1BitIndex2(antiDiagonal);
                    // _bitHelper.GetLeastSignificant1BitIndex(antiDiagonal);
                shiftBy = i - firstSquareIndex + 7;

                northWestRays[i] = antiDiagonal << shiftBy;
                // remove any bits that wrap around when shifted left.
                northWestRays[i] = northWestRays[i] & mask;
            }
            return northWestRays;
        }

        public UInt64[] InitialiseSouthEast()
        {
            UInt64[] southEastRays = new UInt64[64];
            UInt64 antiDiagonal = 0;
            UInt64 mask = 0;
            int lastSquareIndex = 0;
            int shiftBy = 0;

            for (int i = 0; i < 64; i++)
            {
                antiDiagonal = GetAntiDiagonalForSquare(i);
                mask = _bitHelper.GetLeastSignificant1BitMask(antiDiagonal);
                lastSquareIndex = BitHelper.GetMostSignificant1BitIndex2(antiDiagonal);
                    // _bitHelper.GetMostSignificant1BitIndex(antiDiagonal);
                shiftBy = lastSquareIndex - i + 7;
                southEastRays[i] = antiDiagonal >> shiftBy;
                // remove any bits that wrap around when shifted right.
                southEastRays[i] = southEastRays[i] & mask;
            }
            return southEastRays;
        }

        public UInt64 GetDiagonalForSquare(int square)
        {
            // OptimiseTODO: the rank and file diagonals should be initialised only once and not every time this method is called.
            
            UInt64 diagonalForSquare = 0;
            int rank = square / 8;
            int mainDiagonalSquare = 9 * rank;
            int squareDifference = square - mainDiagonalSquare;

            if (squareDifference < 0)
            {
                diagonalForSquare = _fileDiagonals[(-squareDifference) - 1];
            }
            else if (squareDifference == 0)
            {
                diagonalForSquare = Constants.MainDiagonal;
            }
            else // squareDifference > 0
            {
                diagonalForSquare = _rankDiagonals[squareDifference - 1];
            }
            return diagonalForSquare;
        }

        public UInt64 GetAntiDiagonalForSquare(int square)
        {
            UInt64 antiDiagonalForSquare = 0;
            int rank = square / 8;
            int mainAntiDiagonalSquare = 7 * (rank + 1);
            int squareDifference = square - mainAntiDiagonalSquare;

            if (squareDifference < 0)
            {
                antiDiagonalForSquare = _rankAntiDiagonals[7 + squareDifference];
            }
            else if (squareDifference == 0)
            {
                antiDiagonalForSquare = Constants.MainAntiDiagonal;
            }
            else // squareDifference > 0
            {
                antiDiagonalForSquare = _fileAntiDiagonals[squareDifference - 1];
            }
            return antiDiagonalForSquare;
        }

        // diagonals b1-h7, c1-h6 and so on.
        public UInt64[] InitialiseRankDiagonals()
        {
            UInt64[] rankDiagonals = new UInt64[7];
            for (int i = 0; i < 7; i++)
            {
                rankDiagonals[i] = Constants.MainDiagonal >> (8 * (i + 1));
            }
            return rankDiagonals;
        }

        // diagonals a2-g8, a3-f8 and so on.
        public UInt64[] InitialiseFileDiagonals()
        {
            UInt64[] fileDiagonals = new UInt64[7];
            for (int i = 0; i < 7; i++)
            {
                fileDiagonals[i] = Constants.MainDiagonal << (8 * (i + 1));
            }
            return fileDiagonals;
        }

        // diagonals g1-a7, f1-a6 and so on.
        public UInt64[] InitialiseRankAntiDiagonals()
        {
            UInt64[] rankAntiDiagonals = new UInt64[7];
            for (int i = 0; i < 7; i++)
            {
                rankAntiDiagonals[i] = Constants.MainAntiDiagonal >> (8 * (7-i));
            }
            return rankAntiDiagonals;
        }

        // diagonals h2-b8, h3-c8 and so on.
        public UInt64[] InitialiseFileAntiDiagonals()
        {
            UInt64[] fileAntiDiagonals = new UInt64[7];
            for (int i = 0; i < 7; i++)
            {
                fileAntiDiagonals[i] = Constants.MainAntiDiagonal << (8 * (i + 1));
            }
            return fileAntiDiagonals;
        }

        public UInt64[] InitialiseFileRays()
        {
            UInt64[] fileRays = new UInt64[8];
            UInt64 byteValue = 0; UInt64 fileRay = 0;
            for (int file = 0; file < 8; file++)
            {
                byteValue = (UInt64)Math.Pow(2, file);
                fileRay = byteValue;
                for (int i = 1; i < 8; i++)
                {
                    fileRay = fileRay * 0x100 + byteValue;
                }
                fileRays[file] = fileRay;
            }
            return fileRays;
        }
    }
}

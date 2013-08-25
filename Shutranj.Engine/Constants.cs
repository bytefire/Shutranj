using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.Engine
{
    /// <summary>
    /// Contains all the constants.
    /// </summary>
    public static class Constants
    {
        public static Dictionary<string, byte> AlgebraicToIntegerIndex = new Dictionary<string, byte>()
        {
            {"a1", 0}, {"a2", 8}, {"a3", 16}, {"a4", 24}, {"a5", 32}, {"a6", 40}, {"a7", 48}, {"a8", 56},
            {"b1", 1}, {"b2", 9}, {"b3", 17}, {"b4", 25}, {"b5", 33}, {"b6", 41}, {"b7", 49}, {"b8", 57},
            {"c1", 2}, {"c2", 10}, {"c3", 18}, {"c4", 26}, {"c5", 34}, {"c6", 42}, {"c7", 50}, {"c8", 58}, 
            {"d1", 3}, {"d2", 11}, {"d3", 19}, {"d4", 27}, {"d5", 35}, {"d6", 43}, {"d7", 51}, {"d8", 59}, 
            {"e1", 4}, {"e2", 12}, {"e3", 20}, {"e4", 28}, {"e5", 36}, {"e6", 44}, {"e7", 52}, {"e8", 60}, 
            {"f1", 5}, {"f2", 13}, {"f3", 21}, {"f4", 29}, {"f5", 37}, {"f6", 45}, {"f7", 53}, {"f8", 61}, 
            {"g1", 6}, {"g2", 14}, {"g3", 22}, {"g4", 30}, {"g5", 38}, {"g6", 46}, {"g7", 54}, {"g8", 62}, 
            {"h1", 7}, {"h2", 15}, {"h3", 23}, {"h4", 31}, {"h5", 39}, {"h6", 47}, {"h7", 55}, {"h8", 63}, 
        };

        public static Dictionary<Int16, string> IntegerIndexToAlgebraic = new Dictionary<Int16, string>()
        {
            {0, "a1"}, {8, "a2"}, {16, "a3"}, {24, "a4"}, {32, "a5"}, {40, "a6"}, {48, "a7"}, {56, "a8"},
            {1, "b1"}, {9, "b2"}, {17, "b3"}, {25, "b4"}, {33, "b5"}, {41, "b6"}, {49, "b7"}, {57, "b8"},
            {2, "c1"}, {10, "c2"}, {18, "c3"}, {26, "c4"}, {34, "c5"}, {42, "c6"}, {50, "c7"}, {58, "c8"}, 
            {3, "d1"}, {11, "d2"}, {19, "d3"}, {27, "d4"}, {35, "d5"}, {43, "d6"}, {51, "d7"}, {59, "d8"}, 
            {4, "e1"}, {12, "e2"}, {20, "e3"}, {28, "e4"}, {36, "e5"}, {44, "e6"}, {52, "e7"}, {60, "e8"}, 
            {5, "f1"}, {13, "f2"}, {21, "f3"}, {29, "f4"}, {37, "f5"}, {45, "f6"}, {53, "f7"}, {61, "f8"}, 
            {6, "g1"}, {14, "g2"}, {22, "g3"}, {30, "g4"}, {38, "g5"}, {46, "g6"}, {54, "g7"}, {62, "g8"}, 
            {7, "h1"}, {15, "h2"}, {23, "h3"}, {31, "h4"}, {39, "h5"}, {47, "h6"}, {55, "h7"}, {63, "h8"}, 
        };
        
        #region Pieces
        public const int NoPieceNumber = 0;
        public const int BlackKingNumber = -1;
        public const int BlackQueenNumber = -2;
        public const int BlackRookNumber = -3;
        public const int BlackBishopNumber = -4;
        public const int BlackKnightNumber = -5;
        public const int BlackPawnNumber = -6;
        public const int WhiteKingNumber = 1;
        public const int WhiteQueenNumber = 2;
        public const int WhiteRookNumber = 3;
        public const int WhiteBishopNumber = 4;
        public const int WhiteKnightNumber = 5;
        public const int WhitePawnNumber = 6;

        public static readonly Piece[] AllPieces = new Piece[12]
        {
            // NOTE: order of elements in this array is linked to Zobrist Key Indexes (see below).
            Piece.BlackPawn, Piece.BlackKnight, Piece.BlackBishop, Piece.BlackRook, Piece.BlackQueen, Piece.BlackKing,
            Piece.WhitePawn, Piece.WhiteKnight, Piece.WhiteBishop, Piece.WhiteRook, Piece.WhiteQueen, Piece.WhiteKing
        };
        #endregion

        // Initial bitboards from the binary values given in http://pages.cs.wisc.edu/~psilord/blog/data/chess-pages/physical.html 

        public const UInt64 InitialBlackKingBitboard = 0x1000000000000000;
        public const UInt64 InitialBlackQueensBitboard = 0x800000000000000;
        public const UInt64 InitialBlackRooksBitboard = 0x8100000000000000;
        public const UInt64 InitialBlackBishopsBitboard = 0x2400000000000000;
        public const UInt64 InitialBlackKnightsBitboard = 0x4200000000000000;
        public const UInt64 InitialBlackPawnsBitboard = 0xFF000000000000;

        public const UInt64 InitialWhiteKingBitboard = 0x10;
        public const UInt64 InitialWhiteQueensBitboard = 0x8;
        public const UInt64 InitialWhiteRooksBitboard = 0x81;
        public const UInt64 InitialWhiteBishopsBitboard = 0x24;
        public const UInt64 InitialWhiteKnightsBitboard = 0x42;
        public const UInt64 InitialWhitePawnsBitboard = 0xFF00;

        public const UInt64 ClearFileAMask = 0xFEFEFEFEFEFEFEFE;
        public const UInt64 ClearFileBMask = 0xFDFDFDFDFDFDFDFD;
        public const UInt64 ClearFileGMask = 0xBFBFBFBFBFBFBFBF;
        public const UInt64 ClearFileHMask = 0x7F7F7F7F7F7F7F7F;

        // The main diagonal
        public const UInt64 MainDiagonal = 0x8040201008040201;
        // The main anti-diagonal
        public const UInt64 MainAntiDiagonal = 0x102040810204080;
        // following four are used for castling
        public const UInt64 G1Square = 0x0000000000000040;
        public const UInt64 C1Square = 0x0000000000000004;
        public const UInt64 G8Square = 0x4000000000000000;
        public const UInt64 C8Square = 0x0400000000000000;

        public const int InvalidEnPassantSquare = -1;

        // game stages
        internal const int GameStage_Middle = 1;
        internal const int GameStage_End = 2;
        
        // use these instead of int.MinValue and int.MaxValue in search algos, to avoid wrapping around.
        internal const int NegativeInfinity = -100000000;
        internal const int PositiveInfinity = 100000000;
        // the score when game is a draw.
        internal const int DrawScore = 0;

        #region Zobrist Key Indexes
        // NOTE: these indexes are linked to the order of pieces in Constants.AllPieces array (see above).
        internal const int ZobristBlackPawnStartingIndex = 0;
        internal const int ZobristBlackKnightStartingIndex = 64;
        internal const int ZobristBlackBishopStartingIndex = 128;
        internal const int ZobristBlackRookStartingIndex = 192;
        internal const int ZobristBlackQueenStartingIndex = 256;
        internal const int ZobristBlackKingStartingIndex = 320;

        internal const int ZobristWhitePawnStartingIndex = 384;
        internal const int ZobristWhiteKnightStartingIndex = 448;
        internal const int ZobristWhiteBishopStartingIndex = 512;
        internal const int ZobristWhiteRookStartingIndex = 576;
        internal const int ZobristWhiteQueenStartingIndex = 640;
        internal const int ZobristWhiteKingStartingIndex = 704;

        internal const int ZobristBlackToMoveIndex = 768;
        internal const int ZobristBlackKingSideCastlingIndex = 769;
        internal const int ZobristBlackQueenSideCastlingIndex = 770;
        internal const int ZobristWhiteKingSideCastlingIndex = 771;
        internal const int ZobristWhiteQueenSideCastlingIndex = 772;
        internal const int ZobristEnPassantStartingIndex = 773;
        #endregion
    }
}

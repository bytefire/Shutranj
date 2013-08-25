using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.Engine
{
    /// <summary>
    /// Represent distinct chess pieces. E.g. there will be only one black pawn rather that eight.
    /// </summary>
    public struct Piece
    {
        public static Piece None = new Piece(Constants.NoPieceNumber, int.MinValue);
        public static Piece BlackBishop = new Piece(Constants.BlackBishopNumber, Constants.ZobristBlackBishopStartingIndex);
        public static Piece BlackKing = new Piece(Constants.BlackKingNumber, Constants.ZobristBlackKingStartingIndex);
        public static Piece BlackKnight = new Piece(Constants.BlackKnightNumber, Constants.ZobristBlackKnightStartingIndex);
        public static Piece BlackPawn = new Piece(Constants.BlackPawnNumber, Constants.ZobristBlackPawnStartingIndex);
        public static Piece BlackQueen = new Piece(Constants.BlackQueenNumber, Constants.ZobristBlackQueenStartingIndex);
        public static Piece BlackRook = new Piece(Constants.BlackRookNumber, Constants.ZobristBlackRookStartingIndex);
        public static Piece WhiteBishop = new Piece(Constants.WhiteBishopNumber, Constants.ZobristWhiteBishopStartingIndex);
        public static Piece WhiteKing = new Piece(Constants.WhiteKingNumber, Constants.ZobristWhiteKingStartingIndex);
        public static Piece WhiteKnight = new Piece(Constants.WhiteKnightNumber, Constants.ZobristWhiteKnightStartingIndex);
        public static Piece WhitePawn = new Piece(Constants.WhitePawnNumber, Constants.ZobristWhitePawnStartingIndex);
        public static Piece WhiteQueen = new Piece(Constants.WhiteQueenNumber, Constants.ZobristWhiteQueenStartingIndex);
        public static Piece WhiteRook = new Piece(Constants.WhiteRookNumber, Constants.ZobristWhiteRookStartingIndex);

        public readonly int Number;
        public readonly int Index;
        public readonly int ZobristStartingIndex;

        public Piece(int pieceNumber, int zobristStartingIndex)
        {
            Number = pieceNumber;
            Index = pieceNumber + 6;
            ZobristStartingIndex = zobristStartingIndex;
        }

        public static bool operator ==(Piece a, Piece b)
        {
            return (a.Number == b.Number);
        }

        public static bool operator !=(Piece a, Piece b)
        {
            return (a.Number != b.Number);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}

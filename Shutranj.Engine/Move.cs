using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.Engine
{
    /// <summary>
    /// Contains data relating to a chess move.
    /// OptimisationTODO: encode moves in 16 bits (6 bits for from-square + 6 bits for to-square and other four bits for other things), as
    /// shown here: http://chessprogramming.wikispaces.com/Encoding+Moves#Information%20Required-C++%20Sample
    /// </summary>
    public class Move
    {
        /// <summary>
        /// from-square and to-square concatenated together.
        /// </summary>
        public UInt16 HashKey;

        /// <summary>
        /// Denotes whether it is a black move or a white move.
        /// </summary>
        public sbyte Side;

        /// <summary>
        /// Starting position of the move.
        /// </summary>
        public byte StartPosition;
        
        /// <summary>
        /// Enging position of the move.
        /// </summary>
        public byte EndPosition;
        
        /// <summary>
        /// Flag to indicate if this move is a pawn promotion.
        /// </summary>
        public bool IsPawnPromotion;
        
        /// <summary>
        /// The promoted piece if the move is a pawn promotion. By default this will be set to None.
        /// </summary>
        public Piece PromotedPiece;
        
        /// <summary>
        /// Instantiates and initialises a move object with move data supplied in the argument.
        /// </summary>
        /// <param name="side">The side, i.e. black or white.</param>
        /// <param name="longAlgebraicNotation">The move string, e.g. a8a5.</param>
        public Move(sbyte side, string longAlgebraicNotation)
        {
            Set(side, longAlgebraicNotation);
        }

        public Move(sbyte side, byte startPosition, byte endPosition)
            :this(side, startPosition, endPosition, Piece.None)
        {   
        }

        public Move(sbyte side, byte startPosition, byte endPosition, Piece promotedPiece)
        {
            Side = side;
            StartPosition = startPosition;
            EndPosition = endPosition;
            PromotedPiece = promotedPiece;

            HashKey = (UInt16)((StartPosition << 8) ^ EndPosition);
        }

        /// <summary>
        /// Sets the move data by parsing in a long algebraic notation as described in
        /// this document: http://wbec-ridderkerk.nl/html/UCIProtocol.html (see Move format).
        /// </summary>
        /// <param name="longAlgebraicNotationMove">The move string, e.g. a8a5.</param>
        public void Set(sbyte side, string longAlgebraicNotationMove)
        {
            Side = side;
            string start = longAlgebraicNotationMove.Substring(0, 2);
            StartPosition = Constants.AlgebraicToIntegerIndex[start];
            string end = longAlgebraicNotationMove.Substring(2, 2);
            EndPosition = Constants.AlgebraicToIntegerIndex[end];
            // if length is greater than 5, e.g. b7b8q, then it means a pawn promotion.
            if (longAlgebraicNotationMove.Length == 5)
            {
                IsPawnPromotion = true;
                string promotedPiece = longAlgebraicNotationMove.Substring(4, 1);
                switch (promotedPiece)
                {
                    case "q": PromotedPiece = (Side == Engine.Side.Black) ? Piece.BlackQueen : Piece.WhiteQueen;
                        break;
                    case "r": PromotedPiece = (Side == Engine.Side.Black) ? Piece.BlackRook : Piece.WhiteRook;
                        break;
                    case "b": PromotedPiece = (Side == Engine.Side.Black) ? Piece.BlackBishop : Piece.WhiteBishop;
                        break;
                    case "n": PromotedPiece = (Side == Engine.Side.Black) ? Piece.BlackKnight : Piece.WhiteKnight;
                        break;
                    case "p": PromotedPiece = (Side == Engine.Side.Black) ? Piece.BlackPawn : Piece.WhitePawn;
                        break;
                    default:
                        throw new ArgumentException("Invalid promoted piece.");
                }
            }
            else
            {
                IsPawnPromotion = false;
                PromotedPiece = Piece.None;
            }
            HashKey = (UInt16)((StartPosition << 8) ^ EndPosition);
        }

        public override string ToString()
        {
            string str = Constants.IntegerIndexToAlgebraic[StartPosition] + Constants.IntegerIndexToAlgebraic[EndPosition];
            return str;
        }
    }
}

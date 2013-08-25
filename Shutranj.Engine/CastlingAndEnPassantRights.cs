using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.Engine
{
    public class CastlingAndEnPassantRights
    {
        private const byte CastleBlackKingSideMask = 1;
        private const byte CastleBlackQueenSideMask = 2;
        private const byte CastleWhiteKingSideMask = 4;
        private const byte CastleWhiteQueenSideMask = 8;

        // OptimiseTODO: use a single UInt16. use its lowest four bits for castling rights and 
        // highest eight bits for enpassant square number. See: http://www.talkchess.com/forum/viewtopic.php?topic_view=threads&p=234101&t=25096
        private byte _castlingRights = 0x0F;

        private Int16 _enPassantSquare = Constants.InvalidEnPassantSquare;

        public Int16 EnPassantSquare
        {
            get
            {
                return _enPassantSquare;
            }
        }

        public CastlingAndEnPassantRights()
        {
        }

        public CastlingAndEnPassantRights(CastlingAndEnPassantRights castlingAndEnPassantRights)
        {
            _castlingRights = castlingAndEnPassantRights._castlingRights;
            _enPassantSquare = castlingAndEnPassantRights._enPassantSquare;
        }

        public void DisableBlackKingSideCastling()
        {
            DisableCastlingRights(CastleBlackKingSideMask);
        }
        
        public void EnableBlackKingSideCastling()
        {
            EnableCastlingRights(CastleBlackKingSideMask);
        }

        public bool CanBlackCastleKingSide()
        {
            return ((_castlingRights & CastleBlackKingSideMask) > 0);
        }

        public void DisableBlackQueenSideCastling()
        {
            DisableCastlingRights(CastleBlackQueenSideMask);
        }

        public void EnableBlackQueenSideCastling()
        {
            EnableCastlingRights(CastleBlackQueenSideMask);
        }

        public bool CanBlackCastleQueenSide()
        {
            return ((_castlingRights & CastleBlackQueenSideMask) > 0);
        }

        public void DisableWhiteKingSideCastling()
        {
            DisableCastlingRights(CastleWhiteKingSideMask);
        }

        public void EnableWhiteKingSideCastling()
        {
            EnableCastlingRights(CastleWhiteKingSideMask);
        }

        public bool CanWhiteCastleKingSide()
        {
            return ((_castlingRights & CastleWhiteKingSideMask) > 0);
        }

        public void DisableWhiteQueenSideCastling()
        {
            DisableCastlingRights(CastleWhiteQueenSideMask);
        }

        public void EnableWhiteQueenSideCastling()
        {
            EnableCastlingRights(CastleWhiteQueenSideMask);
        }

        public bool CanWhiteCastleQueenSide()
        {
            return ((_castlingRights & CastleWhiteQueenSideMask) > 0);
        }

        public void SetEnPassantSquare(Int16 square)
        {
            _enPassantSquare = square;
        }

        public void ResetEnPassantSquare()
        {
            _enPassantSquare = Constants.InvalidEnPassantSquare;
        }

        private void DisableCastlingRights(byte mask)
        {
            _castlingRights = (byte)(_castlingRights & (~mask));
        }

        private void EnableCastlingRights(byte mask)
        {
            _castlingRights = (byte)(_castlingRights | mask);
        }
    }
}

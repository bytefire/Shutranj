using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.Engine
{
    public class BoardState
    {
        public Piece[] CurrentBoardPosition;
        public UInt64[] PieceToBitboard;
        public CastlingAndEnPassantRights CastlingAndEnPassant;
        public int SideToMove;
        public UInt64 ZobristHash;

        public BoardState(Piece[] currentBoardPosition, UInt64[] pieceToBitboard,
            CastlingAndEnPassantRights castlingAndEnPassant, int sideToMove, UInt64 zobristHash)
        {
            CurrentBoardPosition = currentBoardPosition;
            PieceToBitboard = pieceToBitboard;
            CastlingAndEnPassant = castlingAndEnPassant;
            SideToMove = sideToMove;
            ZobristHash = zobristHash;
        }

        public BoardState(string fenString)
        {
            throw new NotImplementedException();
            // OkashTODO: initialise PieceToBitboard, sideToMove and CurrentBoardPosition from string.
            //          Zobrist hash needs to be initialised in the same way as for board2, but add no pieces
            //          into it. then only add the pieces that are in fen string.
        }

        #region FEN String conversion

        // OkashTODO: this should be part of board state class. board state class should also contain a method
        // to create a board state from a FEN string.
        public string ToFenString()
        {
            string fenStringFormat = "{0} w KQkq - 0 1";
            UInt64 pieceBitboard = 0;
            char[] fenCharArray = new char[64];
            // initialise the FEN char array
            for (int i = 0; i < 64; i++)
            {
                fenCharArray[i] = '1';
            }
            pieceBitboard = PieceToBitboard[Piece.BlackBishop.Index];
            PopulateFenCharArray(ref fenCharArray, pieceBitboard, 'b');

            pieceBitboard = PieceToBitboard[Piece.BlackKing.Index];
            PopulateFenCharArray(ref fenCharArray, pieceBitboard, 'k');

            pieceBitboard = PieceToBitboard[Piece.BlackKnight.Index];
            PopulateFenCharArray(ref fenCharArray, pieceBitboard, 'n');

            pieceBitboard = PieceToBitboard[Piece.BlackPawn.Index];
            PopulateFenCharArray(ref fenCharArray, pieceBitboard, 'p');

            pieceBitboard = PieceToBitboard[Piece.BlackQueen.Index];
            PopulateFenCharArray(ref fenCharArray, pieceBitboard, 'q');

            pieceBitboard = PieceToBitboard[Piece.BlackRook.Index];
            PopulateFenCharArray(ref fenCharArray, pieceBitboard, 'r');

            pieceBitboard = PieceToBitboard[Piece.WhiteBishop.Index];
            PopulateFenCharArray(ref fenCharArray, pieceBitboard, 'B');

            pieceBitboard = PieceToBitboard[Piece.WhiteKing.Index];
            PopulateFenCharArray(ref fenCharArray, pieceBitboard, 'K');

            pieceBitboard = PieceToBitboard[Piece.WhiteKnight.Index];
            PopulateFenCharArray(ref fenCharArray, pieceBitboard, 'N');

            pieceBitboard = PieceToBitboard[Piece.WhitePawn.Index];
            PopulateFenCharArray(ref fenCharArray, pieceBitboard, 'P');

            pieceBitboard = PieceToBitboard[Piece.WhiteQueen.Index];
            PopulateFenCharArray(ref fenCharArray, pieceBitboard, 'Q');

            pieceBitboard = PieceToBitboard[Piece.WhiteRook.Index];
            PopulateFenCharArray(ref fenCharArray, pieceBitboard, 'R');

            string fenPositionString = GetFenPositionString(fenCharArray);

            string fenString = String.Format(fenStringFormat, fenPositionString);

            return fenString;
        }
        private void PopulateFenCharArray(ref char[] fenCharArray, UInt64 pieceBitboard, char pieceChar)
        {
            int i = 0;
            int remainder = 0;
            while (pieceBitboard > 0)
            {
                remainder = (int)(pieceBitboard % 2);
                if (remainder == 1)
                {
                    fenCharArray[i] = pieceChar;
                }
                pieceBitboard = pieceBitboard / 2;
                i++;
            }
        }

        private string GetFenPositionString(char[] fenCharArray)
        {
            string positionString = string.Empty;
            StringBuilder rankPositionBuilder = new StringBuilder();

            int emptySquareCount = 0;

            for (int i = 0; i < 64; i++)
            {
                if (fenCharArray[i] == '1')
                {
                    emptySquareCount++;
                }
                else
                {
                    if (emptySquareCount > 0)
                    {
                        rankPositionBuilder.Append(emptySquareCount.ToString());
                        emptySquareCount = 0;
                    }
                    rankPositionBuilder.Append(fenCharArray[i]);
                }

                if ((i + 1) % 8 == 0)
                {
                    if (emptySquareCount > 0)
                    {
                        rankPositionBuilder.Append(emptySquareCount.ToString());
                        emptySquareCount = 0;
                    }
                    positionString = @"/" + rankPositionBuilder.ToString() + positionString;
                    rankPositionBuilder.Clear();
                }
            }
            return positionString.Substring(1);
        }
        #endregion
    }
}

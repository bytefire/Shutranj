using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.Engine
{
    public  class Board
    {
        #region Key Defining Fields: These Define Board
        
        private  Piece[] squareToPiece = new Piece[64];
        internal  UInt64[] PieceToBitboard = new UInt64[13];
        public  UInt64 ZobristHash = 0;
        // OptimiseTODO: get rid of the class and inline castling and en passant inside this class and 
        // pack them in UInt16 as shown here: http://www.talkchess.com/forum/viewtopic.php?topic_view=threads&p=234101&t=25096
        private  CastlingAndEnPassantRights castlingAndEnPassant = new CastlingAndEnPassantRights();
        private  int sideToMove = Side.White;

        #endregion

        #region Other Fields
        private UInt64 _blackPieces
        {
            get
            {
                return PieceToBitboard[Piece.BlackKing.Index] | PieceToBitboard[Piece.BlackQueen.Index] |
                    PieceToBitboard[Piece.BlackRook.Index] | PieceToBitboard[Piece.BlackBishop.Index] |
                    PieceToBitboard[Piece.BlackKnight.Index] | PieceToBitboard[Piece.BlackPawn.Index];
            }
        }

        private UInt64 _whitePieces
        {
            get
            {
                return PieceToBitboard[Piece.WhiteKing.Index] | PieceToBitboard[Piece.WhiteQueen.Index] |
                    PieceToBitboard[Piece.WhiteRook.Index] | PieceToBitboard[Piece.WhiteBishop.Index] |
                    PieceToBitboard[Piece.WhiteKnight.Index] | PieceToBitboard[Piece.WhitePawn.Index];
            }
        }

        private UInt64 _allPieces
        {
            get
            {
                return _blackPieces | _whitePieces;
            }
        }
        private  UInt64[][] _rayAttacks;
        private  UInt64[] _rankMasks;

        // Although 218 seems to be the max number of moves in any position, setting it to 400 
        // just in case (e.g. if there are more pseudo legal moves and 218 might be the max no of strictly legal moves)
        private const int MaxNumberOfMoves = 400;

        #endregion

        #region Initialisation
        private  UInt64[] _zobristKeys = new UInt64[781];

        public Board(BoardState state)
            : this()
        {
            RestoreState(state);
        }

        public Board()
        {
            Initialise();
        }

        private  void Initialise()
        {
            // initialise the current position board
            for (Int16 i = 0; i < 64; i++)
            {
                squareToPiece[i] = Piece.None;
            }
            squareToPiece[0] = Piece.WhiteRook;
            squareToPiece[1] = Piece.WhiteKnight;
            squareToPiece[2] = Piece.WhiteBishop;
            squareToPiece[3] = Piece.WhiteQueen;
            squareToPiece[4] = Piece.WhiteKing;
            squareToPiece[5] = Piece.WhiteBishop;
            squareToPiece[6] = Piece.WhiteKnight;
            squareToPiece[7] = Piece.WhiteRook;
            squareToPiece[8] = Piece.WhitePawn;
            squareToPiece[9] = Piece.WhitePawn;
            squareToPiece[10] = Piece.WhitePawn;
            squareToPiece[11] = Piece.WhitePawn;
            squareToPiece[12] = Piece.WhitePawn;
            squareToPiece[13] = Piece.WhitePawn;
            squareToPiece[14] = Piece.WhitePawn;
            squareToPiece[15] = Piece.WhitePawn;

            squareToPiece[63] = Piece.BlackRook;
            squareToPiece[62] = Piece.BlackKnight;
            squareToPiece[61] = Piece.BlackBishop;
            squareToPiece[60] = Piece.BlackKing;
            squareToPiece[59] = Piece.BlackQueen;
            squareToPiece[58] = Piece.BlackBishop;
            squareToPiece[57] = Piece.BlackKnight;
            squareToPiece[56] = Piece.BlackRook;
            squareToPiece[55] = Piece.BlackPawn;
            squareToPiece[54] = Piece.BlackPawn;
            squareToPiece[53] = Piece.BlackPawn;
            squareToPiece[52] = Piece.BlackPawn;
            squareToPiece[51] = Piece.BlackPawn;
            squareToPiece[50] = Piece.BlackPawn;
            squareToPiece[49] = Piece.BlackPawn;
            squareToPiece[48] = Piece.BlackPawn;

            // initialise piece to bitboard dictionary
            PieceToBitboard[Piece.BlackKing.Index] = Constants.InitialBlackKingBitboard;
            PieceToBitboard[Piece.BlackQueen.Index] = Constants.InitialBlackQueensBitboard;
            PieceToBitboard[Piece.BlackRook.Index] = Constants.InitialBlackRooksBitboard;
            PieceToBitboard[Piece.BlackBishop.Index] = Constants.InitialBlackBishopsBitboard;
            PieceToBitboard[Piece.BlackKnight.Index] = Constants.InitialBlackKnightsBitboard;
            PieceToBitboard[Piece.BlackPawn.Index] = Constants.InitialBlackPawnsBitboard;
            PieceToBitboard[Piece.WhiteKing.Index] = Constants.InitialWhiteKingBitboard;
            PieceToBitboard[Piece.WhiteQueen.Index] = Constants.InitialWhiteQueensBitboard;
            PieceToBitboard[Piece.WhiteRook.Index] = Constants.InitialWhiteRooksBitboard;
            PieceToBitboard[Piece.WhiteBishop.Index] = Constants.InitialWhiteBishopsBitboard;
            PieceToBitboard[Piece.WhiteKnight.Index] = Constants.InitialWhiteKnightsBitboard;
            PieceToBitboard[Piece.WhitePawn.Index] = Constants.InitialWhitePawnsBitboard;
            PieceToBitboard[Piece.None.Index] = 0;

            Initialiser initialiser = new Initialiser();
            _rayAttacks = initialiser.GenerateRayAttacks();

            // initialise rank masks
            _rankMasks = new UInt64[8];
            UInt64 zerothRankMask = 0x00000000000000FF;
            int shiftBy = 0;
            for (int i = 0; i < 8; i++)
            {
                shiftBy = 8 * i;
                _rankMasks[i] = zerothRankMask << shiftBy;
            }

            // set up zobrist hash
            InitialiseZobristHash();
        }

        private  void InitialiseZobristHash()
        {
            /*
             * 781 = 12*64 + 1 + 4 + 8
             * ------------------------
             * 
             * 12*64: 12 pieces for each square on chessboard
             * 1: included in hash if the side to move is black
             * 4: castling rights (Black-Kingside, Black-Queenside, White-Kingside, White-Queenside)
             * 8: denote en passant square number
             */

            // first initialise the keys with pseudo-random numbers
            for (int i = 0; i < 781; i++)
            {
                _zobristKeys[i] = Utility.GetPseudoRandomNumber();
            }
            // initialise board pieces' positions
            int squareIndex;
            UInt64 pieceBitboard;
            for (int zobristPieceIndex = 0; zobristPieceIndex < Constants.AllPieces.Length; zobristPieceIndex++)
            {
                // NOTE: Zobrist piece indexes are linked to the order of pieces in Constants.AllPieces array
                pieceBitboard = PieceToBitboard[Constants.AllPieces[zobristPieceIndex].Index];
                while (pieceBitboard != 0)
                {
                    squareIndex = BitHelper.GetLeastSignificant1BitIndex2(pieceBitboard);
                    ZobristHash ^= _zobristKeys[(zobristPieceIndex * 64) + squareIndex];
                    pieceBitboard &= pieceBitboard - 1;
                }
            }
            // side-to-move 
            if (sideToMove == Side.Black)
            {
                ZobristHash ^= _zobristKeys[Constants.ZobristBlackToMoveIndex];
            }
            // castling
            if (castlingAndEnPassant.CanBlackCastleKingSide())
            {
                ZobristHash ^= _zobristKeys[Constants.ZobristBlackKingSideCastlingIndex];
            }

            if (castlingAndEnPassant.CanBlackCastleQueenSide())
            {
                ZobristHash ^= _zobristKeys[Constants.ZobristBlackQueenSideCastlingIndex];
            }

            if (castlingAndEnPassant.CanWhiteCastleKingSide())
            {
                ZobristHash ^= _zobristKeys[Constants.ZobristWhiteKingSideCastlingIndex];
            }

            if (castlingAndEnPassant.CanWhiteCastleQueenSide())
            {
                ZobristHash ^= _zobristKeys[Constants.ZobristWhiteQueenSideCastlingIndex];
            }

            // en passant
            if (castlingAndEnPassant.EnPassantSquare != Constants.InvalidEnPassantSquare)
            {
                int enPassantFile = castlingAndEnPassant.EnPassantSquare % 8;
                ZobristHash ^= _zobristKeys[Constants.ZobristEnPassantStartingIndex + enPassantFile];
            }
        }
        #endregion

        // NOTE: this method is only used to validate moves entered manually or by an external syste.
        //      the engine iteself generates pseudo-legal moves only so it doesn't need this method.
        public bool IsPseudoLegalMove(UInt16 move)
        {
            byte fromSquare = GetFromSquare(move);
            byte toSquare = GetToSquare(move);

            Piece piece = squareToPiece[fromSquare];
            // make sure that the piece is of the same colour as the side making the move
            if (!IsSameSide(piece, sideToMove))
            {
                return false;
            }
            // get own board
            UInt64 ownBoard = GetBoardForSide(sideToMove);
            // get attack set for the piece.
            UInt64 validMoves = GetValidMovesForPiece(piece, fromSquare, ownBoard);

            // check if the destination square of the move is covered by validMoves board.
            UInt64 destinationSquareBitboard = ((UInt64)1) << toSquare;
            UInt64 isValidMove = validMoves & destinationSquareBitboard;
            return (isValidMove > 0);
        }

        #region Make Move

        // makes a move made by a human user or another chess engine. it is not used by this chess engine.
        // it validates the move and then makes it. it returns false if the move is not valid.
        public bool MakeUserMove(string longAlgebraicNotation)
        {
            longAlgebraicNotation = longAlgebraicNotation.Trim();
            UInt16 encodedMove = EncodeMove(longAlgebraicNotation);

            if (!IsPseudoLegalMove(encodedMove))
            {
                return false;
            }
            if (MakeMove(encodedMove))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public  bool MakeMove(UInt16 move)
        {
            // save the state...
            BoardState beforeMove = GetBoardState();

            byte fromSquare = GetFromSquare(move);
            Piece movingPiece = squareToPiece[fromSquare];
            byte toSquare = GetToSquare(move);

            /* Capture */
            bool isCapture = IsCapture(move);
            if (isCapture)
            {
                byte capturedSquare = toSquare;

                bool isEnPassant = IsEnPassant(move);
                if (isEnPassant)
                {
                    // following gives toSquare + 8 if the current move is black's and
                    // toSquare - 8 if the current move is white's
                    capturedSquare = (byte)(toSquare - (sideToMove * 8));
                }
                Piece capturedPiece = squareToPiece[capturedSquare];

                /* Update KDFs */

                // KDF 1: piece to BB array:
                PieceToBitboard[capturedPiece.Index] ^= (((UInt64)1) << capturedSquare);

                // KDF 2: zobrist:
                // if it is a capture then remove current position of captured piece (i.e. XOR out)
                ZobristHash ^= _zobristKeys[capturedPiece.ZobristStartingIndex + capturedSquare];

                // KDF 3: square to piece array:
                squareToPiece[capturedSquare] = Piece.None;

                // KDF 4: castling and EP
                #region Captured Piece Castling Rights Updating
                // if captured piece is a rook then that opponent's castling on that rook's side will be disabled.
                if (capturedPiece == Piece.WhiteRook)
                {
                    if ((toSquare == 0) && castlingAndEnPassant.CanWhiteCastleQueenSide())
                    {
                        castlingAndEnPassant.DisableWhiteQueenSideCastling();
                        // update zobrist hash: XOR out white queen side castling
                        ZobristHash ^= _zobristKeys[Constants.ZobristWhiteQueenSideCastlingIndex];
                    }
                    else if ((toSquare == 7) && castlingAndEnPassant.CanWhiteCastleKingSide())
                    {
                        castlingAndEnPassant.DisableWhiteKingSideCastling();
                        // update zobrist hash: XOR out white king side castling
                        ZobristHash ^= _zobristKeys[Constants.ZobristWhiteKingSideCastlingIndex];
                    }
                }
                if (capturedPiece == Piece.BlackRook)
                {
                    if ((toSquare == 56) && castlingAndEnPassant.CanBlackCastleQueenSide())
                    {
                        castlingAndEnPassant.DisableBlackQueenSideCastling();
                        // update zobrist hash: XOR out black queen side castling
                        ZobristHash ^= _zobristKeys[Constants.ZobristBlackQueenSideCastlingIndex];
                    }
                    else if ((toSquare == 63) && castlingAndEnPassant.CanBlackCastleKingSide())
                    {
                        castlingAndEnPassant.DisableBlackKingSideCastling();
                        // update zobrist hash: XOR out black king side castling
                        ZobristHash ^= _zobristKeys[Constants.ZobristBlackKingSideCastlingIndex];
                    }
                }
                #endregion

                /* End of Update KDFs */
            }
            /* Capture */

            /* Update KDFs */

            JustMoveThePiece(movingPiece, fromSquare, toSquare);

            #region Commented code. call JustMoveThePiece method instead. Remove this after testing.
            //// KDF 1: piece to BB array
            //UInt64 startEndBitboard = (((UInt64)1) << fromSquare) ^ (((UInt64)1) << toSquare);
            //PieceToBitboard[movingPiece.Index] ^= startEndBitboard;

            //// KDF 2: zobrist hash
            //// update zobrist hash (http://stackoverflow.com/questions/10067514/correctly-implementing-zobrist-hashing)
            //// remove starting position of moving piece from hash (i.e. XOR out)
            //ZobristHash ^= _zobristKeys[movingPiece.ZobristStartingIndex + fromSquare];
            //// include ending position of moving piece into hash (i.e. XOR in)
            //ZobristHash ^= _zobristKeys[movingPiece.ZobristStartingIndex + toSquare];

            //// KDF 3: square to piece array
            //// square to piece array:
            //squareToPiece[fromSquare] = Piece.None;
            //squareToPiece[toSquare] = movingPiece;
            #endregion

            /* Castling */

            if (IsKingSideCastle(move))
            {
                if (movingPiece == Piece.WhiteKing)
                {
                    // move rook on square 7 to square 5
                    JustMoveThePiece(Piece.WhiteRook, 7, 5);
                }
                else if (movingPiece == Piece.BlackKing)
                {
                    // move rook on square 63 to square 61
                    JustMoveThePiece(Piece.BlackRook, 63, 61);
                }
            }
            else if (IsQueenSideCastle(move))
            {
                if (movingPiece == Piece.WhiteKing)
                {
                    // move rook on square 0 to square 3
                    JustMoveThePiece(Piece.WhiteRook, 0, 3);
                }
                else if (movingPiece == Piece.BlackKing)
                {
                    // move rook on square 56 to square 59
                    JustMoveThePiece(Piece.BlackRook, 56, 59);
                }
            }

            /* End of Castling */

            // KDF 4: castling and EP:
            #region Castling rights for the moving piece side
            // first check white queen side
            if (castlingAndEnPassant.CanWhiteCastleQueenSide() &&
                (movingPiece == Piece.WhiteRook) && (fromSquare == 0))
            {
                castlingAndEnPassant.DisableWhiteQueenSideCastling();
                // update zobrist hash: XOR out white queen side castling
                ZobristHash ^= _zobristKeys[Constants.ZobristWhiteQueenSideCastlingIndex];
            }
            // check white king side
            if (castlingAndEnPassant.CanWhiteCastleKingSide() &&
                (movingPiece == Piece.WhiteRook) && (fromSquare == 7))
            {
                castlingAndEnPassant.DisableWhiteKingSideCastling();
                // update zobrist hash: XOR out white king side castling
                ZobristHash ^= _zobristKeys[Constants.ZobristWhiteKingSideCastlingIndex];
            }
            // check white both sides
            if ((movingPiece == Piece.WhiteKing) && (fromSquare == 4))
            {
                if (castlingAndEnPassant.CanWhiteCastleQueenSide())
                {
                    castlingAndEnPassant.DisableWhiteQueenSideCastling();
                    // update zobrist hash: XOR out white queen side castling
                    ZobristHash ^= _zobristKeys[Constants.ZobristWhiteQueenSideCastlingIndex];
                }
                if (castlingAndEnPassant.CanWhiteCastleKingSide())
                {
                    castlingAndEnPassant.DisableWhiteKingSideCastling();
                    // update zobrist hash: XOR out white king side castling
                    ZobristHash ^= _zobristKeys[Constants.ZobristWhiteKingSideCastlingIndex];
                }
            }
            // black queen side
            if (castlingAndEnPassant.CanBlackCastleQueenSide() &&
                (movingPiece == Piece.BlackRook) && (fromSquare == 56))
            {
                castlingAndEnPassant.DisableBlackQueenSideCastling();
                // update zobrist hash: XOR out black queen side castling
                ZobristHash ^= _zobristKeys[Constants.ZobristBlackQueenSideCastlingIndex];
            }
            // check black king side
            if (castlingAndEnPassant.CanBlackCastleKingSide() &&
                (movingPiece == Piece.BlackRook) && (fromSquare == 63))
            {
                castlingAndEnPassant.DisableBlackKingSideCastling();
                // update zobrist hash: XOR out black king side castling
                ZobristHash ^= _zobristKeys[Constants.ZobristBlackKingSideCastlingIndex];
            }
            // check black both sides
            if ((movingPiece == Piece.BlackKing) && (fromSquare == 60))
            {
                if (castlingAndEnPassant.CanBlackCastleQueenSide())
                {
                    castlingAndEnPassant.DisableBlackQueenSideCastling();
                    // update zobrist hash: XOR out black queen side castling
                    ZobristHash ^= _zobristKeys[Constants.ZobristBlackQueenSideCastlingIndex];
                }
                if (castlingAndEnPassant.CanBlackCastleKingSide())
                {
                    castlingAndEnPassant.DisableBlackKingSideCastling();
                    // update zobrist hash: XOR out black king side castling
                    ZobristHash ^= _zobristKeys[Constants.ZobristBlackKingSideCastlingIndex];
                }
            }
            #endregion

            #region En Passant Rights

            // update zobrist hash: XOR out any previous en passant file before resetting en passant square
            if (castlingAndEnPassant.EnPassantSquare != Constants.InvalidEnPassantSquare)
            {
                ZobristHash ^= _zobristKeys[Constants.ZobristEnPassantStartingIndex + (castlingAndEnPassant.EnPassantSquare % 8)];
            }
            castlingAndEnPassant.ResetEnPassantSquare();
            
            if ((movingPiece == Piece.BlackPawn) || (movingPiece == Piece.WhitePawn))
            {
                int absoluteDifference = toSquare - fromSquare;
                // if white pawn moved two steps
                if (absoluteDifference == 16)
                {
                    castlingAndEnPassant.SetEnPassantSquare((Int16)(toSquare - 8));
                }
                // if black pawn moved two steps
                else if (absoluteDifference == -16)
                {
                    castlingAndEnPassant.SetEnPassantSquare((Int16)(toSquare + 8));
                }
            }
            // update zobrist hash: XOR in any en passant file
            if (castlingAndEnPassant.EnPassantSquare != Constants.InvalidEnPassantSquare)
            {
                ZobristHash ^= _zobristKeys[Constants.ZobristEnPassantStartingIndex + (castlingAndEnPassant.EnPassantSquare % 8)];
            }

            #endregion

            /* End of Update KDFs */

            //  if promo then remove the moving piece from its BB + update its zobrist + add queen at that location in queen BB + update queen zobrist
            // NOTE: handling promo must come after the move has been made.
            if (IsPromo(move))
            {
                if (movingPiece == Piece.WhitePawn)
                {
                    // OkashTODO: at the moment always promoting to queen. need to read from the move which piece is promo to.
                    ReplacePawnWithPromotedPiece(Piece.WhiteQueen, Piece.WhitePawn, toSquare);
                }
                else if (movingPiece == Piece.BlackPawn)
                {
                    // OkashTODO: at the moment always promoting to queen. need to read from the move which piece is promo to.
                    ReplacePawnWithPromotedPiece(Piece.BlackQueen, Piece.BlackPawn, toSquare);
                }
            }

            // KDF 5: Side to move
            // change the side to move:
            sideToMove = -sideToMove;

            // update zobrist hash for the side to move: if the next move is black move (i.e. current move is white) then XOR in the side-
            //              to-move key. if next move is white then XOR out the side-to-move key. in either case, all we need to 
            //              do is XOR the side-to-move key because when initialising Zobrist hash we didn't include side-to-move key indicating
            //              the next move (i.e. the first move) will be a white move.
            ZobristHash ^= _zobristKeys[Constants.ZobristBlackToMoveIndex];

            if (IsInCheck(-sideToMove))
            {
                RestoreState(beforeMove);
                return false;
            }
            return true;

        }

        public BoardState GetBoardState()
        {
            BoardState currentBoardState = new BoardState((Piece[])squareToPiece.Clone(),
                (UInt64[])PieceToBitboard.Clone(),
                new CastlingAndEnPassantRights(castlingAndEnPassant),
                sideToMove, ZobristHash);

            return currentBoardState;
        }

        public void RestoreState(BoardState stateToRestore)
        {
            squareToPiece = stateToRestore.CurrentBoardPosition;
            PieceToBitboard = stateToRestore.PieceToBitboard;
            castlingAndEnPassant = stateToRestore.CastlingAndEnPassant;
            sideToMove = stateToRestore.SideToMove;
            ZobristHash = stateToRestore.ZobristHash;
        }

        internal int GetGameStage()
        {
            if ((PieceToBitboard[Constants.BlackQueenNumber + 6] == 0) &&
                (PieceToBitboard[Constants.WhiteQueenNumber + 6] == 0))
            {
                return Constants.GameStage_End;
            }

            if (PieceToBitboard[Constants.BlackQueenNumber + 6] != 0)
            {
                int blackMinorPieceCount = BitHelper.GetNumberOfSetBits(PieceToBitboard[Constants.BlackKnightNumber + 6]);
                blackMinorPieceCount += BitHelper.GetNumberOfSetBits(PieceToBitboard[Constants.BlackBishopNumber + 6]);

                if (blackMinorPieceCount > 1)
                {
                    return Constants.GameStage_Middle;
                }
            }

            if (PieceToBitboard[Constants.WhiteQueenNumber + 6] != 0)
            {
                int whiteMinorPieceCount = BitHelper.GetNumberOfSetBits(PieceToBitboard[Constants.WhiteKnightNumber + 6]);
                whiteMinorPieceCount += BitHelper.GetNumberOfSetBits(PieceToBitboard[Constants.WhiteBishopNumber + 6]);

                if (whiteMinorPieceCount > 1)
                {
                    return Constants.GameStage_Middle;
                }
            }
            return Constants.GameStage_End;
        }
        #endregion

        #region Move Generation

        public UInt16[] GenerateMoves(out int movesCount)
        {
            // Important thing to do: reset moves index to 0...
            movesCount = 0;
            int quietMovesCount = 0;

            UInt16[] moves = new UInt16[MaxNumberOfMoves];
            UInt16[] quietMoves = new UInt16[281];

            foreach (Piece piece in Constants.AllPieces)
            {
                // OptimiseTODO: have a 2 by 6 2D array. store white and black pieces at two indexes. then
                // no need to check for is same side...
                if (IsSameSide(piece, sideToMove))
                {
                    GenerateMoves(piece, moves, quietMoves, ref movesCount, ref quietMovesCount);
                }
            }
            // add quite moves to the end of moves array:
            for (int i = 0; i < quietMovesCount; i++)
            {
                moves[movesCount] = quietMoves[i];
                movesCount++;
            }
            return moves;
        }

        private void GenerateMoves(Piece piece, UInt16[] moves, UInt16[] quietMoves, ref int movesCount, ref int quietMovesCount)
        {
            UInt64 pieceBitboard = PieceToBitboard[piece.Index];
            byte[] startPositions = BitHelper.GetSetBitIndexes2(pieceBitboard);
            UInt64 ownBoard = GetBoardForSide(sideToMove);

            for (int i = 0; i < startPositions.Length; i++)
            {
                UInt64 validMoves = GetValidMovesForPiece(piece, startPositions[i], ownBoard);
                ExtractPseudoLegalMoves(validMoves, startPositions[i], moves, quietMoves, ref movesCount, ref quietMovesCount);
            }
        }

        private void ExtractPseudoLegalMoves(UInt64 validMoves, byte fromSquare, UInt16[] moves,
            UInt16[] quietMoves, ref int movesCount, ref int quietMovesCount)
        {
            byte toSquare;
            UInt16 encodedMove;
            while (validMoves != 0)
            {
                toSquare = BitHelper.GetLeastSignificant1BitIndex2(validMoves);
                // NOTE: any move ordering (e.g. capture moves coming before quiet ones) should be done here...
                encodedMove = EncodeMove(fromSquare, toSquare);

                if (IsCapture(encodedMove))
                {
                    moves[movesCount] = encodedMove;
                    movesCount++;
                }
                else
                {
                    quietMoves[quietMovesCount] = encodedMove;
                    quietMovesCount++;
                }
                validMoves &= validMoves - 1;
            }
        }

        #region Generate Capture Moves (Shadows GenerateMoves method sequence above)

        public UInt16[] GenerateCaptureMoves(out int movesCount)
        {
            // Important thing to do: reset moves index to 0...
            movesCount = 0;

            UInt16[] moves = new UInt16[MaxNumberOfMoves];

            foreach (Piece piece in Constants.AllPieces)
            {
                // OptimiseTODO: have a 2 by 6 2D array. store white and black pieces at two indexes. then
                // no need to check for is same side...
                if (IsSameSide(piece, sideToMove))
                {
                    GenerateCaptureMoves(piece, moves, ref movesCount);
                }
            }
            return moves;
        }

        private void GenerateCaptureMoves(Piece piece, UInt16[] moves, ref int movesCount)
        {
            UInt64 pieceBitboard = PieceToBitboard[piece.Index];
            byte[] startPositions = BitHelper.GetSetBitIndexes2(pieceBitboard);
            UInt64 ownBoard = GetBoardForSide(sideToMove);

            for (int i = 0; i < startPositions.Length; i++)
            {
                UInt64 validMoves = GetValidMovesForPiece(piece, startPositions[i], ownBoard);
                ExtractPseudoLegalCaptureMoves(validMoves, startPositions[i], moves, ref movesCount);
            }
        }

        private void ExtractPseudoLegalCaptureMoves(UInt64 validMoves, byte fromSquare, UInt16[] moves, ref int movesCount)
        {
            byte toSquare;
            UInt16 encodedMove;
            while (validMoves != 0)
            {
                toSquare = BitHelper.GetLeastSignificant1BitIndex2(validMoves);
                encodedMove = EncodeMove(fromSquare, toSquare);

                if (IsCapture(encodedMove))
                {
                    moves[movesCount] = encodedMove;
                    movesCount++;
                }
                
                validMoves &= validMoves - 1;
            }
        }
        #endregion

        #endregion

        #region Square Attacked
        public bool IsInCheck(int sideToCheck)
        {
            UInt64 kingBitboard = PieceToBitboard[Piece.WhiteKing.Index];
            if (sideToCheck == Side.Black)
            {
                kingBitboard = PieceToBitboard[Piece.BlackKing.Index];
            }
            int attackingSide = sideToCheck * -1;
            bool isInCheck = IsAttacked(kingBitboard, attackingSide);
            return isInCheck;
        }
        public bool IsAttacked(int square, int attackingSide)
        {
            // OptimiseTODO: better performance through the use of "Standard Bitboard Definition" as shown
            //  here: http://chessprogramming.wikispaces.com/Square+Attacked+By#AnyAttackBySide (on this page see the link to Standard Bitboard definition).
            UInt64 attackedSquare = ((UInt64)1) << square;
            return IsAttacked(attackedSquare, attackingSide);
        }

        private bool IsAttacked(UInt64 attackedSquare, int attackingSide)
        {
            // 1. attacked by pawns?

            UInt64 pawnAttacks = GetAllPawnAttacks(attackingSide);
            if ((attackedSquare & pawnAttacks) != 0)
            {
                return true;
            }

            // 2. attacked by rooks?
            UInt64 rookAttacks = GetAllRookAttacks(attackingSide);
            if ((attackedSquare & rookAttacks) != 0)
            {
                return true;
            }

            // 3. attacked by knights?
            UInt64 knightAttacks = GetAllKnightAttacks(attackingSide);
            if ((attackedSquare & knightAttacks) != 0)
            {
                return true;
            }

            // 4. attacked by bishops?
            UInt64 bishopAttacks = GetAllBishopAttacks(attackingSide);
            if ((attackedSquare & bishopAttacks) != 0)
            {
                return true;
            }

            // 5. attacked by queens?
            UInt64 queenAttacks = GetAllQueenAttacks(attackingSide);
            if ((attackedSquare & queenAttacks) != 0)
            {
                return true;
            }

            // 6. attacked by king?
            UInt64 kingAttacks = GetAllKingAttacks(attackingSide);
            if ((attackedSquare & kingAttacks) != 0)
            {
                return true;
            }

            return false;
        }

        private UInt64 GetAllPawnAttacks(int attackingSide)
        {
            Piece piece = attackingSide == Side.Black ? Piece.BlackPawn : Piece.WhitePawn;
            UInt64 pawns = PieceToBitboard[piece.Index];
            byte[] pawnSquares = BitHelper.GetSetBitIndexes2(pawns);
            //_bitHelper.GetSetBitIndexes(pawns);
            UInt64 pawnAttacks = 0;
            // NOTE: Gettting ATTACKS for pawns rather than VALID moves as in the case of all other pieces. This is
            //      because of the special property of pawns to attack on different squares than where it can move.
            Func<int, UInt64> getPawnAttacks = GetAttacksForWhitePawn;
            UInt64 ownBoard = _whitePieces;
            if (attackingSide == Side.Black)
            {
                getPawnAttacks = GetAttacksForBlackPawn;
                ownBoard = _blackPieces;
            }

            for (int i = 0; i < pawnSquares.Length; i++)
            {
                pawnAttacks = pawnAttacks | getPawnAttacks(pawnSquares[i]);
            }
            return pawnAttacks;
        }

        private UInt64 GetAllRookAttacks(int attackingSide)
        {
            Piece piece = attackingSide == Side.Black ? Piece.BlackRook : Piece.WhiteRook;
            UInt64 rooks = PieceToBitboard[piece.Index];
            byte[] rookSquares = BitHelper.GetSetBitIndexes2(rooks);
            // _bitHelper.GetSetBitIndexes(rooks);
            UInt64 rookAttacks = 0;
            UInt64 ownBoard = _whitePieces;
            if (attackingSide == Side.Black)
            {
                ownBoard = _blackPieces;
            }
            for (int i = 0; i < rookSquares.Length; i++)
            {
                rookAttacks = rookAttacks | GetValidMovesForRook(rookSquares[i], ownBoard);
            }
            return rookAttacks;
        }

        private UInt64 GetAllKnightAttacks(int attackingSide)
        {
            Piece piece = attackingSide == Side.Black ? Piece.BlackKnight : Piece.WhiteKnight;
            UInt64 knights = PieceToBitboard[piece.Index];
            byte[] knightSquares = BitHelper.GetSetBitIndexes2(knights);
            //_bitHelper.GetSetBitIndexes(knights);
            UInt64 knightAttacks = 0;
            UInt64 ownBoard = _whitePieces;
            if (attackingSide == Side.Black)
            {
                ownBoard = _blackPieces;
            }
            for (int i = 0; i < knightSquares.Length; i++)
            {
                knightAttacks = knightAttacks | GetValidMovesForKnight(knightSquares[i], ownBoard);
            }
            return knightAttacks;
        }

        private UInt64 GetAllBishopAttacks(int attackingSide)
        {
            Piece piece = attackingSide == Side.Black ? Piece.BlackBishop : Piece.WhiteBishop;
            UInt64 bishops = PieceToBitboard[piece.Index];
            byte[] bishopSquares = BitHelper.GetSetBitIndexes2(bishops);
            // _bitHelper.GetSetBitIndexes(bishops);
            UInt64 bishopAttacks = 0;
            UInt64 ownBoard = _whitePieces;
            if (attackingSide == Side.Black)
            {
                ownBoard = _blackPieces;
            }
            for (int i = 0; i < bishopSquares.Length; i++)
            {
                bishopAttacks = bishopAttacks | GetValidMovesForBishop(bishopSquares[i], ownBoard);
            }
            return bishopAttacks;
        }

        private UInt64 GetAllQueenAttacks(int attackingSide)
        {
            Piece piece = attackingSide == Side.Black ? Piece.BlackQueen : Piece.WhiteQueen;
            UInt64 queens = PieceToBitboard[piece.Index];
            byte[] queenSquares = BitHelper.GetSetBitIndexes2(queens);
            //_bitHelper.GetSetBitIndexes(queens);
            UInt64 queenAttacks = 0;
            UInt64 ownBoard = _whitePieces;
            if (attackingSide == Side.Black)
            {
                ownBoard = _blackPieces;
            }
            for (int i = 0; i < queenSquares.Length; i++)
            {
                queenAttacks = queenAttacks | GetValidMovesForQueen(queenSquares[i], ownBoard);
            }
            return queenAttacks;
        }

        private UInt64 GetAllKingAttacks(int attackingSide)
        {
            Piece piece = attackingSide == Side.Black ? Piece.BlackKing : Piece.WhiteKing;
            UInt64 king = PieceToBitboard[piece.Index];

            // NOTE: although king should never be zero, this check is required when performing evaluation where we check if the
            //      move generated is strictly legal and thus need attacks by all opponent pieces. we don't have a special check
            //      for other pieces because we loop through the array of that piece's squares. if that piece's bitboard is zero
            //      then there will be no squares to loop through and we would end up returning zero anyways.
            if (king == 0)
            {
                return 0;
            }

            byte[] kingSquare = BitHelper.GetSetBitIndexes2(king);
            //_bitHelper.GetSetBitIndexes(king);
            UInt64 kingAttacks = 0;
            UInt64 ownBoard = _whitePieces;
            if (attackingSide == Side.Black)
            {
                ownBoard = _blackPieces;
            }
            // exclude castling because king cannot attack while castling.
            // NOTE: including castling results in infinite recursion when checking if the king can castle.
            kingAttacks = GetValidMovesForKing(kingSquare[0], ownBoard, true);
            return kingAttacks;
        }

        #endregion

        #region Getting Valid Moves For Pieces
        private UInt64 GetValidMovesForPiece(Piece piece, int square, UInt64 ownBoard)
        {
            // gets all the spots that this piece can move to assuming that this is the only piece on the board.
            switch (piece.Number)
            {
                case Constants.BlackKingNumber:
                case Constants.WhiteKingNumber:
                    return GetValidMovesForKing(square, ownBoard);
                case Constants.BlackKnightNumber:
                case Constants.WhiteKnightNumber:
                    return GetValidMovesForKnight(square, ownBoard);
                case Constants.WhitePawnNumber:
                    return GetValidMovesForWhitePawn(square);
                case Constants.BlackPawnNumber:
                    return GetValidMovesForBlackPawn(square);
                case Constants.BlackRookNumber:
                case Constants.WhiteRookNumber:
                    return GetValidMovesForRook(square, ownBoard);
                case Constants.BlackBishopNumber:
                case Constants.WhiteBishopNumber:
                    return GetValidMovesForBishop(square, ownBoard);
                case Constants.BlackQueenNumber:
                case Constants.WhiteQueenNumber:
                    return GetValidMovesForQueen(square, ownBoard);
                default:
                    throw new ArgumentException("Invalid piece.");
            }
        }

        private UInt64 GetValidMovesForKing(int square, UInt64 ownBoard, bool excludeCastlingSpots = false)
        {
            // NOTE:  the reason for excludeCastling parameter is that including castling results in infinite 
            //        recursion when checking if the king can castle.

            UInt64 kingLocation = ((UInt64)1) << square;

            // from http://pages.cs.wisc.edu/~psilord/blog/data/chess-pages/nonsliding.html

            UInt64 kingClipFileA = kingLocation & Constants.ClearFileAMask;
            UInt64 kingClipFileH = kingLocation & Constants.ClearFileHMask;

            UInt64 spot1 = kingClipFileA << 7;
            UInt64 spot2 = kingLocation << 8;
            UInt64 spot3 = kingClipFileH << 9;
            UInt64 spot4 = kingClipFileH << 1;

            UInt64 spot5 = kingClipFileH >> 7;
            UInt64 spot6 = kingLocation >> 8;
            UInt64 spot7 = kingClipFileA >> 9;
            UInt64 spot8 = kingClipFileA >> 1;
            UInt64 castlingSpots = excludeCastlingSpots ? 0 : GetCastlingSpots(square);

            UInt64 kingAttackSet = spot1 | spot2 | spot3 | spot4 |
                spot5 | spot6 | spot7 | spot8 | castlingSpots;
            UInt64 kingValidMoves = kingAttackSet & (~ownBoard);
            return kingValidMoves;
        }

        private UInt64 GetCastlingSpots(int square)
        {
            UInt64 castlingSpots = 0;
            // white king's castling
            if (square == 4)
            {
                // white castling king side
                if (castlingAndEnPassant.CanWhiteCastleKingSide())
                {
                    if (IsEmpty(5) && IsEmpty(6))
                    {
                        // make sure that e1, f1 and g1 are not under attack.
                        if (!IsAttacked(4, Side.Black) && !IsAttacked(5, Side.Black) && !IsAttacked(6, Side.Black))
                        {
                            castlingSpots = castlingSpots | Constants.G1Square;
                        }
                    }
                }
                // white castle queen side
                if (castlingAndEnPassant.CanWhiteCastleQueenSide())
                {
                    if (IsEmpty(3) && IsEmpty(2) && IsEmpty(1))
                    {
                        // make sure that square e1, d1 and c1 are not under attack
                        if (!IsAttacked(4, Side.Black) && !IsAttacked(3, Side.Black) && !IsAttacked(2, Side.Black))
                        {
                            castlingSpots = castlingSpots | Constants.C1Square;
                        }
                    }
                }
                return castlingSpots;
            }

            // black king's castling
            if (square == 60)
            {
                // black castle king side
                if (castlingAndEnPassant.CanBlackCastleKingSide())
                {
                    if (IsEmpty(61) && IsEmpty(62))
                    {
                        // make sure that e8, f8 and g8 are not under attack
                        if (!IsAttacked(60, Side.White) && !IsAttacked(61, Side.White) && !IsAttacked(62, Side.White))
                        {
                            castlingSpots = castlingSpots | Constants.G8Square;
                        }
                    }
                }
                if (castlingAndEnPassant.CanBlackCastleQueenSide())
                {
                    // black castle queen side
                    if (IsEmpty(59) && IsEmpty(58) && IsEmpty(57))
                    {
                        // make sure that e8, d8 and c8 are not under attack
                        if (!IsAttacked(60, Side.White) && !IsAttacked(59, Side.White) && !IsAttacked(58, Side.White))
                        {
                            castlingSpots = castlingSpots | Constants.C8Square;
                        }
                    }
                }
                return castlingSpots;
            }
            return castlingSpots;
        }

        private UInt64 GetValidMovesForKnight(int square, UInt64 ownBoard)
        {
            UInt64 knightLocation = ((UInt64)1) << square;
            // from http://pages.cs.wisc.edu/~psilord/blog/data/chess-pages/nonsliding.html

            UInt64 spot1Clip = Constants.ClearFileAMask & Constants.ClearFileBMask;
            UInt64 spot2Clip = Constants.ClearFileAMask;
            UInt64 spot3Clip = Constants.ClearFileHMask;
            UInt64 spot4Clip = Constants.ClearFileHMask & Constants.ClearFileGMask;

            UInt64 spot5Clip = Constants.ClearFileHMask & Constants.ClearFileGMask;
            UInt64 spot6Clip = Constants.ClearFileHMask;
            UInt64 spot7Clip = Constants.ClearFileAMask;
            UInt64 spot8Clip = Constants.ClearFileAMask & Constants.ClearFileBMask;
            // Calculate spots:
            UInt64 spot1 = (knightLocation & spot1Clip) << 6;
            UInt64 spot2 = (knightLocation & spot2Clip) << 15;
            UInt64 spot3 = (knightLocation & spot3Clip) << 17;
            UInt64 spot4 = (knightLocation & spot4Clip) << 10;

            UInt64 spot5 = (knightLocation & spot5Clip) >> 6;
            UInt64 spot6 = (knightLocation & spot6Clip) >> 15;
            UInt64 spot7 = (knightLocation & spot7Clip) >> 17;
            UInt64 spot8 = (knightLocation & spot8Clip) >> 10;

            UInt64 knightAttackSet = spot1 | spot2 | spot3 | spot4 |
                spot5 | spot6 | spot7 | spot8;
            UInt64 knightValid = knightAttackSet & (~ownBoard);
            return knightValid;
        }

        private UInt64 GetValidMovesForWhitePawn(int square)
        {
            // from http://pages.cs.wisc.edu/~psilord/blog/data/chess-pages/nonsliding.html

            // first get pawn pushes
            UInt64 pawnLocation = ((UInt64)1) << square;
            UInt64 pawnOneStep = (pawnLocation << 8) & (~_allPieces);
            UInt64 pawnTwoSteps = ((pawnOneStep & _rankMasks[2]) << 8) & ~_allPieces;
            UInt64 validPawnPushes = pawnOneStep | pawnTwoSteps;
            UInt64 enPassantSpot = 0;
            if (castlingAndEnPassant.EnPassantSquare != Constants.InvalidEnPassantSquare)
            {
                enPassantSpot = ((UInt64)1) << castlingAndEnPassant.EnPassantSquare;
            }
            // next calculate pawn captures
            UInt64 pawnAttacks = GetAttacksForWhitePawn(pawnLocation);
            UInt64 validPawnAttacks = pawnAttacks & (_blackPieces | enPassantSpot);
            // finally union pawn pushes and pawn captures to get valid pawn moves
            UInt64 validPawnMoves = validPawnPushes | validPawnAttacks;

            return validPawnMoves;
        }

        private UInt64 GetAttacksForWhitePawn(int square)
        {
            UInt64 pawnLoacation = ((UInt64)1) << square;
            UInt64 pawnAttacks = GetAttacksForWhitePawn(pawnLoacation);
            return pawnAttacks;
        }

        private UInt64 GetAttacksForWhitePawn(UInt64 pawnLocation)
        {
            UInt64 pawnLeftAttack = (pawnLocation & Constants.ClearFileAMask) << 7;
            UInt64 pawnRightAttack = (pawnLocation & Constants.ClearFileHMask) << 9;
            UInt64 pawnAttacks = pawnLeftAttack | pawnRightAttack;
            return pawnAttacks;
        }

        private UInt64 GetValidMovesForBlackPawn(int square)
        {
            // from http://pages.cs.wisc.edu/~psilord/blog/data/chess-pages/nonsliding.html

            // first get pawn pushes
            UInt64 pawnLocation = ((UInt64)1) << square;
            UInt64 pawnOneStep = (pawnLocation >> 8) & (~_allPieces);
            UInt64 pawnTwoSteps = ((pawnOneStep & _rankMasks[5]) >> 8) & ~_allPieces;
            UInt64 validPawnPushes = pawnOneStep | pawnTwoSteps;
            UInt64 enPassantSpot = 0;
            if (castlingAndEnPassant.EnPassantSquare != Constants.InvalidEnPassantSquare)
            {
                enPassantSpot = ((UInt64)1) << castlingAndEnPassant.EnPassantSquare;
            }

            // next calculate pawn captures
            UInt64 pawnAttacks = GetAttacksForBlackPawn(pawnLocation);
            UInt64 validPawnAttacks = pawnAttacks & (_whitePieces | enPassantSpot);
            // finally union pawn pushes and pawn captures to get valid pawn moves
            UInt64 validPawnMoves = validPawnPushes | validPawnAttacks;

            return validPawnMoves;
        }

        private UInt64 GetAttacksForBlackPawn(int square)
        {
            UInt64 pawnLocation = ((UInt64)1) << square;
            UInt64 pawnAttacks = GetAttacksForBlackPawn(pawnLocation);
            return pawnAttacks;
        }

        private UInt64 GetAttacksForBlackPawn(UInt64 pawnLocation)
        {
            UInt64 pawnLeftAttack = (pawnLocation & Constants.ClearFileHMask) >> 7;
            UInt64 pawnRightAttack = (pawnLocation & Constants.ClearFileAMask) >> 9;
            UInt64 pawnAttacks = pawnLeftAttack | pawnRightAttack;
            return pawnAttacks;
        }

        private UInt64 GetValidMovesForRook(int square, UInt64 ownBoard)
        {
            UInt64 rookAttacks = GetRookAttacks(_allPieces, square);
            UInt64 validRookMoves = rookAttacks & (~ownBoard);
            return validRookMoves;
        }

        private UInt64 GetValidMovesForBishop(int square, UInt64 ownBoard)
        {
            UInt64 bishopAttacks = GetBishopAttacks(_allPieces, square);
            UInt64 validBishipMoves = bishopAttacks & (~ownBoard);
            return validBishipMoves;
        }

        private UInt64 GetValidMovesForQueen(int square, UInt64 ownBoard)
        {
            UInt64 queenAttacks = GetQueenAttacks(_allPieces, square);
            UInt64 validQueenMoves = queenAttacks & (~ownBoard);
            return validQueenMoves;
        }

        // determines whether the given square is empty
        private bool IsEmpty(int square)
        {
            UInt64 squareBitboard = ((UInt64)1) << square;
            UInt64 result = _allPieces & squareBitboard;
            return (result == 0);
        }

        // OptimiseTODO: there is a branchless version of following method. See http://chessprogramming.wikispaces.com/Classical+Approach#Ray%20Attacks-Positive%20Rays-Branchless
        private UInt64 GetPositiveRayAttacks(UInt64 occupiedBitboard, int rayDirection, int square)
        {
            UInt64 attacks = _rayAttacks[rayDirection][square];
            UInt64 blocker = attacks & occupiedBitboard;
            if (blocker != 0)
            {
                square = BitHelper.GetLeastSignificant1BitIndex2(blocker);
                // _bitHelper.GetLeastSignificant1BitIndex(blocker);
                attacks = attacks ^ _rayAttacks[rayDirection][square];
            }
            return attacks;
        }

        // OptimiseTODO: there is a branchless version of following method. See http://chessprogramming.wikispaces.com/Classical+Approach#Ray%20Attacks-Negative%20Rays-Branchless
        private UInt64 GetNegativeRayAttacks(UInt64 occupiedBitboard, int rayDirection, int square)
        {
            UInt64 attacks = _rayAttacks[rayDirection][square];
            UInt64 blocker = attacks & occupiedBitboard;
            if (blocker != 0)
            {
                square = BitHelper.GetMostSignificant1BitIndex2(blocker);
                // _bitHelper.GetMostSignificant1BitIndex(blocker);
                attacks = attacks ^ _rayAttacks[rayDirection][square];
            }
            return attacks;
        }

        private UInt64 GetDiagonalAttacks(UInt64 occupiedBitboard, int square)
        {
            UInt64 positiveRayAttacks = GetPositiveRayAttacks(occupiedBitboard, RayDirection.NorthEast, square);
            UInt64 negativeRayAttacks = GetNegativeRayAttacks(occupiedBitboard, RayDirection.SouthWest, square);
            UInt64 diagonalAttacks = positiveRayAttacks | negativeRayAttacks;
            return diagonalAttacks;
        }

        private UInt64 GetAntiDiagonalAttacks(UInt64 occupiedBitboard, int square)
        {
            UInt64 positiveRayAttacks = GetPositiveRayAttacks(occupiedBitboard, RayDirection.NorthWest, square);
            UInt64 negativeRayAttacks = GetNegativeRayAttacks(occupiedBitboard, RayDirection.SouthEast, square);
            UInt64 antiDiagonalRayAttacks = positiveRayAttacks | negativeRayAttacks;
            return antiDiagonalRayAttacks;
        }

        private UInt64 GetFileAttacks(UInt64 occupiedBitboard, int square)
        {
            UInt64 positiveRayAttacks = GetPositiveRayAttacks(occupiedBitboard, RayDirection.North, square);
            UInt64 negativeRayAttacks = GetNegativeRayAttacks(occupiedBitboard, RayDirection.South, square);
            UInt64 fileAttacks = positiveRayAttacks | negativeRayAttacks;
            return fileAttacks;
        }

        private UInt64 GetRankAttacks(UInt64 occupiedBitboard, int square)
        {
            UInt64 positiveRayAttacks = GetPositiveRayAttacks(occupiedBitboard, RayDirection.East, square);
            UInt64 negativeRayAttacks = GetNegativeRayAttacks(occupiedBitboard, RayDirection.West, square);
            UInt64 rankAttacks = positiveRayAttacks | negativeRayAttacks;
            return rankAttacks;
        }

        private UInt64 GetRookAttacks(UInt64 occupiedBitboard, int square)
        {
            UInt64 fileAttacks = GetFileAttacks(occupiedBitboard, square);
            UInt64 rankAttacks = GetRankAttacks(occupiedBitboard, square);
            UInt64 rookAttacks = fileAttacks | rankAttacks;
            return rookAttacks;
        }

        private UInt64 GetBishopAttacks(UInt64 occupiedBitboard, int square)
        {
            UInt64 diagonalAttacks = GetDiagonalAttacks(occupiedBitboard, square);
            UInt64 antiDiagonalAttacks = GetAntiDiagonalAttacks(occupiedBitboard, square);
            UInt64 bishopAttacks = diagonalAttacks | antiDiagonalAttacks;
            return bishopAttacks;
        }

        private UInt64 GetQueenAttacks(UInt64 occupiedBitboard, int square)
        {
            UInt64 rookAttacks = GetRookAttacks(occupiedBitboard, square);
            UInt64 bishopAttacks = GetBishopAttacks(occupiedBitboard, square);
            UInt64 queenAttacks = rookAttacks | bishopAttacks;
            return queenAttacks;
        }

        #endregion

        public override string ToString()
        {
            return GetBoardState().ToFenString();
        }


        #region Helper Methods

        #region MakeMove Related

        private void ReplacePawnWithPromotedPiece(Piece promotedPiece, Piece pawnToReplace, byte promotionSquare)
        {
            UInt64 pawnQueenMask = (UInt64)1 << promotionSquare;
            PieceToBitboard[pawnToReplace.Index] = PieceToBitboard[pawnToReplace.Index] ^ pawnQueenMask;
            PieceToBitboard[promotedPiece.Index] = PieceToBitboard[promotedPiece.Index] | pawnQueenMask;

            squareToPiece[promotionSquare] = promotedPiece;
            // update zobrist hash: XOR out the pawn and XOR in the promoted piece
            ZobristHash ^= _zobristKeys[pawnToReplace.ZobristStartingIndex + promotionSquare];
            ZobristHash ^= _zobristKeys[promotedPiece.ZobristStartingIndex + promotionSquare];
        }

        // updates: 1. piece to BB for that piece, 2. zobrist, 3. square to piece array.
        private void JustMoveThePiece(Piece movingPiece, byte fromSquare, byte toSquare)
        {
            /* Update KDFs */
            // KDF 1: piece to BB array
            UInt64 startEndBitboard = (((UInt64)1) << fromSquare) ^ (((UInt64)1) << toSquare);
            PieceToBitboard[movingPiece.Index] ^= startEndBitboard;

            // KDF 2: zobrist hash
            // update zobrist hash (http://stackoverflow.com/questions/10067514/correctly-implementing-zobrist-hashing)
            // remove starting position of moving piece from hash (i.e. XOR out)
            ZobristHash ^= _zobristKeys[movingPiece.ZobristStartingIndex + fromSquare];
            // include ending position of moving piece into hash (i.e. XOR in)
            ZobristHash ^= _zobristKeys[movingPiece.ZobristStartingIndex + toSquare];

            // KDF 3: square to piece array
            // square to piece array:
            squareToPiece[fromSquare] = Piece.None;
            squareToPiece[toSquare] = movingPiece;
        }

        #endregion

        #region Move Related
        /* Move Composition:
         * 
         * Bits 0-3 (4 bits): Flags (see: http://chessprogramming.wikispaces.com/Encoding+Moves#Information%20Required-From-To%20Based)
         * Bits 4-9 (6 bits): To Square
         * Bits 10-15 (6 bits): From Square
         * 
         * */

        private const byte ToSquareAndMoveFlagsLength = 10;
        private const byte MoveFlagsLegth = 4;

        private const UInt16 LS4BMask = 0x000F;
        private const UInt16 LS6BMask = 0x003F;
        private const UInt16 ThirdBitMask = 0x0004;
        private const UInt16 ThirdAndFirstBitMask = 0x0005;
        private const UInt16 FourthBitMask = 0x0008;
        private const UInt16 SecondBitMask = 0x0002;
        private const UInt16 SecondAndFirstBitMask = 0x0003;

        internal byte GetFromSquare(UInt16 move)
        {
            return (byte)(move >> ToSquareAndMoveFlagsLength);
        }

        internal byte GetToSquare(UInt16 move)
        {
            return (byte)((move >> MoveFlagsLegth) & LS6BMask);
        }

        private bool IsCapture(UInt16 move)
        {
            // true if third bit is set
            return ((move & ThirdBitMask) == ThirdBitMask);
        }

        private bool IsEnPassant(UInt16 move)
        {
            // true if third and first bit ( i.e. bits 2 and 0) are set
            return ((move & ThirdAndFirstBitMask) == ThirdAndFirstBitMask);
        }

        private bool IsPromo(UInt16 move)
        {
            // true if fourth bit is set (i.e. bit at index 3)
            return ((move & FourthBitMask) == FourthBitMask);
        }

        private bool IsKingSideCastle(UInt16 move)
        {
            // true if least significant four bits are 0010 (http://chessprogramming.wikispaces.com/Encoding+Moves#Information%20Required-From-To%20Based)
            return ((move & LS4BMask) == 0x2);
        }

        private bool IsQueenSideCastle(UInt16 move)
        {
            // true if least significant four bits are 0011 (http://chessprogramming.wikispaces.com/Encoding+Moves#Information%20Required-From-To%20Based)
            return ((move & LS4BMask) == 0x3);
        }
        
        private UInt16 EncodeMove(string longAlgebraicNotation)
        {
            string from = longAlgebraicNotation.Substring(0, 2);
            byte fromSquare = Constants.AlgebraicToIntegerIndex[from];
            string to = longAlgebraicNotation.Substring(2, 2);
            byte toSquare = Constants.AlgebraicToIntegerIndex[to];

            return EncodeMove(fromSquare, toSquare);
        }

        private UInt16 EncodeMove(byte fromSquare, byte toSquare)
        {
            UInt16 encodedMove = (UInt16)((fromSquare << ToSquareAndMoveFlagsLength) | (toSquare << MoveFlagsLegth));
            
            Piece movingPiece = squareToPiece[fromSquare];
            
            /* Flags */

            // check for pawn promotion:
            if ((movingPiece == Piece.WhitePawn) && (toSquare / 8 == 7))
            {
                encodedMove = (UInt16)(encodedMove | FourthBitMask);
            }
            else if ((movingPiece == Piece.BlackPawn) && (toSquare / 8 == 0))
            {
                encodedMove = (UInt16)(encodedMove | FourthBitMask);
            }

            // check for capture:
            if (squareToPiece[toSquare] != Piece.None)
            {
                // set capture bit
                encodedMove = (UInt16)(encodedMove | ThirdBitMask);
            }
            // check for en passant capture:
            if ((toSquare == castlingAndEnPassant.EnPassantSquare) &&
                ((movingPiece == Piece.WhitePawn) || (movingPiece == Piece.BlackPawn)))
            {
                // set en passant bits
                encodedMove = (UInt16)(encodedMove | ThirdAndFirstBitMask);
            }

            // check for castling:
            #region Castling
            if (movingPiece == Piece.WhiteKing)
            {
                if (fromSquare == 4)
                {
                    // king side castling
                    if (toSquare == 6)
                    {
                        // set king side castling
                        encodedMove = (UInt16)(encodedMove | SecondBitMask);
                    }
                    // queen side castling
                    else if (toSquare == 2)
                    {
                        // set queen side castling
                        encodedMove = (UInt16)(encodedMove | SecondAndFirstBitMask);
                    }
                }
            }
            else if (movingPiece == Piece.BlackKing)
            {
                if (fromSquare == 60)
                {
                    if (toSquare == 62)
                    {
                        // set king side castling
                        encodedMove = (UInt16)(encodedMove | SecondBitMask);
                    }
                    else if (toSquare == 58)
                    {
                        // set queen side castling
                        encodedMove = (UInt16)(encodedMove | SecondAndFirstBitMask);
                    }
                }
            }
            #endregion

            return encodedMove;
        }

        // NOTE: this method is not required by this board. it is only used when interfacing with external system (either a 
        // human or another chess engine).
        public string ConvertToAlgebraicNotation(UInt16 move)
        {
            byte fromSquare = GetFromSquare(move);
            byte toSquare = GetToSquare(move);
            string algebraic = Constants.IntegerIndexToAlgebraic[fromSquare] + Constants.IntegerIndexToAlgebraic[toSquare];
            
            if (IsPromo(move))
            {
                algebraic += "q";
            }
            
            return algebraic;
        }
        #endregion

        #region Move Generation Related

        private bool IsSameSide(Piece piece, int side)
        {
            return ((piece.Number * side) > 0);
        }

        private UInt64 GetBoardForSide(int side)
        {
            if (side == Side.Black)
            {
                return _blackPieces;
            }
            else
            {
                return _whitePieces;
            }
        }

        #endregion
        #endregion
    }
}

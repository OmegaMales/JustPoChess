﻿﻿using System;
using System.Collections.Generic;
using JustPoChess.Client.MVC.Model.Contracts;
using JustPoChess.Client.MVC.Model.Entities.Pieces;
using JustPoChess.Client.MVC.Model.Entities.Pieces.Abstract;
using JustPoChess.Client.MVC.Model.Entities.Pieces.PiecePosition;
using JustPoChess.Client.MVC.Model.Entities.Pieces.PiecesEnums;

namespace JustPoChess.Client.MVC.Model.Entities.Board
{
    public class Board:IBoard
    {
        public const int BoardSize = 8;

        private IPiece[,] boardState;
        private IPiece[,] testBoardState;
        private IDictionary<IBoard, int> positionOccurences;

        private PieceColor currentPlayerToMove = PieceColor.White;

        public Board()
        {
            
        }

        public PieceColor CurrentPlayerToMove
        {
            get { return this.currentPlayerToMove; }
            set { this.currentPlayerToMove = value; }
        }

        public IDictionary<IBoard, int> PositionOccurences
        {
            get { return this.positionOccurences; }
            set { this.positionOccurences = value; }
        }

        public IPiece[,] BoardState
        {
            get
            {
                return this.boardState;
            }
            set
            {
                //Validation
                this.boardState = value;
            }
        }

        public IPiece[,] TestBoardState
        {
            get
            {
                return this.testBoardState;
            }
            set
            {
                //Validation
                this.testBoardState = value;
            }
        }

        public bool WhiteLeftCastlePossible { get; set; } = true;

        public bool WhiteRightCastlePossible { get; set; } = true;

        public bool BlackLeftCastlePossible { get; set; } = true;

        public bool BlackRightCastlePossible { get; set; } = true;

        public void RevertTestBoardState()
        {
            this.testBoardState = this.BoardDeepCopy();
        }
        
        public void InitBoard()
        {
            IPiece[,] state =
            {
                { new Rook(PieceColor.Black, new Position(0, 0)), new Knight(PieceColor.Black, new Position(0, 1)), new Bishop(PieceColor.Black, new Position(0, 2)), new Queen(PieceColor.Black, new Position(0, 3)), new King(PieceColor.Black, new Position(0, 4)), new Bishop(PieceColor.Black, new Position(0, 5)), new Knight(PieceColor.Black, new Position(0, 6)), new Rook(PieceColor.Black, new Position(0, 7)) },
                { new Pawn(PieceColor.Black, new Position(1, 0)), new Pawn(PieceColor.Black, new Position(1, 1)), new Pawn(PieceColor.Black, new Position(1, 2)), new Pawn(PieceColor.Black, new Position(1, 3)), new Pawn(PieceColor.Black, new Position(1, 4)), new Pawn(PieceColor.Black, new Position(1, 5)), new Pawn(PieceColor.Black, new Position(1, 6)), new Pawn(PieceColor.Black, new Position(1, 7)) },
                { null, null, null, null, null, null, null, null },
                { null, null, null, null, null, null, null, null },
                { null, null, null, null, null, null, null, null },
                { null, null, null, null, null, null, null, null },
                { new Pawn(PieceColor.White, new Position(6, 0)), new Pawn(PieceColor.White, new Position(6, 1)), new Pawn(PieceColor.White, new Position(6, 2)), new Pawn(PieceColor.White, new Position(6, 3)), new Pawn(PieceColor.White, new Position(6, 4)), new Pawn(PieceColor.White, new Position(6, 5)), new Pawn(PieceColor.White, new Position(6, 6)), new Pawn(PieceColor.White, new Position(6, 7)) },
                { new Rook(PieceColor.White, new Position(7, 0)), new Knight(PieceColor.White, new Position(7, 1)), new Bishop(PieceColor.White, new Position(7, 2)), new Queen(PieceColor.White, new Position(7, 3)), new King(PieceColor.White, new Position(7, 4)), new Bishop(PieceColor.White, new Position(7, 5)), new Knight(PieceColor.White, new Position(7, 6)), new Rook(PieceColor.White, new Position(7, 7)) }
            };
            this.CurrentPlayerToMove = PieceColor.White;
            this.SetBoardState(state, PieceColor.White);

            this.TestBoardState = this.BoardDeepCopy();
            this.PositionOccurences.Add(this, 1);
        }
        
        public void SetBoardState(IPiece[,] state, PieceColor color)
        {
            this.CurrentPlayerToMove = color;

            if (color == PieceColor.Black)
            {
                if (state[0, 4] is King)
                {
                    if (state[0, 0] is Rook)
                    {
                        this.BlackLeftCastlePossible = true;
                    }
                    else
                    {
                        this.BlackLeftCastlePossible = false;
                    }
                    if (state[0, 7] is Rook)
                    {
                        this.BlackRightCastlePossible = true;
                    }
                    else
                    {
                        this.BlackRightCastlePossible = false;
                    }
                }
                else
                {
                    this.BlackLeftCastlePossible = false;
                    this.BlackRightCastlePossible = false;
                }
            }
            else
            {
                if (state[7, 4] is King)
                {
                    if (state[7, 0] is Rook)
                    {
                        this.WhiteLeftCastlePossible = true;
                    }
                    else
                    {
                        this.WhiteLeftCastlePossible = false;
                    }
                    if (state[7, 7] is Rook)
                    {
                        this.WhiteRightCastlePossible = true;
                    }
                    else
                    {
                        this.WhiteRightCastlePossible = false;
                    }
                }
                else
                {
                    this.WhiteLeftCastlePossible = false;
                    this.WhiteRightCastlePossible = false;
                }
            }
            this.BoardState = state;
        }

        public void PerformMove(IModel model, IMove move)
        {
            if (move == null)
            {
                //yes, it must be just argument and not argument null exception
                throw new ArgumentException("Move not possible");
            }
            IPiece piece = this.BoardState[move.CurrentPosition.Row, move.CurrentPosition.Col];
            if (piece.PieceType == PieceType.Pawn
                && move.CurrentPosition.Col != move.NextPosition.Col
                && BoardState[move.NextPosition.Row, move.NextPosition.Col] == null) //pawn that takes a piece
            {
                switch (piece.PieceColor)
                {
                    case PieceColor.White:
                        boardState[move.NextPosition.Row + 1, move.NextPosition.Col] = null;
                        break;
                    case PieceColor.Black:
                        boardState[move.NextPosition.Row - 1, move.NextPosition.Col] = null;
                        break;
                }
            }
            if (piece.PieceType == PieceType.King && Math.Abs(move.CurrentPosition.Col - move.NextPosition.Col) > 1) //castling
            {
                switch (piece.PieceColor)
                {
                    case PieceColor.White:
                        switch (move.NextPosition.Col)
                        {
                            case 2:
                                BoardState[7, 0] = null;
                                BoardState[7, 3] = new Rook(PieceColor.White, new Position(7, 3));
                                break;
                            case 6:
                                BoardState[7, 7] = null;
                                BoardState[7, 5] = new Rook(PieceColor.White, new Position(7, 3));
                                break;
                        }
                        break;
                    case PieceColor.Black:
                        switch (move.NextPosition.Col)
                        {
                            case 2:
                                BoardState[1, 0] = null;
                                BoardState[1, 3] = new Rook(PieceColor.Black, new Position(1, 3));
                                break;
                            case 6:
                                BoardState[1, 7] = null;
                                BoardState[1, 5] = new Rook(PieceColor.Black, new Position(1, 3));
                                break;
                        }
                        break;
                }
            }
            this.BoardState[move.CurrentPosition.Row, move.CurrentPosition.Col] = null;
            this.BoardState[move.NextPosition.Row, move.NextPosition.Col] = piece;
            piece.PiecePosition = new Position(move.NextPosition.Row, move.NextPosition.Col);

            if (move.CurrentPosition.Row == 7)
            {
                if (move.CurrentPosition.Col == 0)
                {
                    this.WhiteLeftCastlePossible = false;
                }
                if (move.CurrentPosition.Col == 4)
                {
                    this.WhiteLeftCastlePossible = false;
                    this.WhiteRightCastlePossible = false;
                }
                if (move.CurrentPosition.Col == 7)
                {
                    this.WhiteRightCastlePossible = false;
                }
            }
            if (move.CurrentPosition.Row == 0)
            {
                if (move.CurrentPosition.Col == 0)
                {
                    this.BlackLeftCastlePossible = false;
                }
                if (move.CurrentPosition.Col == 4)
                {
                    this.BlackLeftCastlePossible = false;
                    this.BlackRightCastlePossible = false;
                }
                if (move.CurrentPosition.Col == 7)
                {
                    this.BlackRightCastlePossible = false;
                }
            }
            if (move.NextPosition.Row == 7)
            {
                if (move.NextPosition.Col == 0)
                {

                    this.WhiteLeftCastlePossible = false;
                }
                if (move.NextPosition.Col == 4)
                {
                    this.WhiteLeftCastlePossible = false;
                    this.WhiteRightCastlePossible = false;
                }
                if (move.NextPosition.Col == 7)
                {
                    this.WhiteRightCastlePossible = false;
                }
            }
            if (move.NextPosition.Row == 0)
            {
                if (move.NextPosition.Col == 0)
                {
                    this.BlackLeftCastlePossible = false;
                }
                if (move.NextPosition.Col == 4)
                {
                    this.BlackLeftCastlePossible = false;
                    this.BlackRightCastlePossible = false;
                }
                if (move.NextPosition.Col == 7)
                {
                    this.BlackRightCastlePossible = false;
                }
            }
            //will need that for en passant pawn move
            model.LastMove = move;

            if (CurrentPlayerToMove == PieceColor.White)
            {
                CurrentPlayerToMove = PieceColor.Black;
            }
            else
            {
                CurrentPlayerToMove = PieceColor.White;
            }
            this.TestBoardState = this.BoardDeepCopy();

            if (positionOccurences.ContainsKey(this))
            {
                int count = 0;
                positionOccurences.TryGetValue(this, out count);
                positionOccurences.Remove(this);
                positionOccurences.Add(this, count++);
            }
            else
            {
                this.positionOccurences.Add(this, 1);
            }
        }

        public void PerformMoveOnTestBoard(IMove move)
        {
            if (move == null)
            {
                //yes, it must be just argument and not argument null exception
                throw new ArgumentException("Move not possible");
            }
            IPiece piece = this.TestBoardState[move.CurrentPosition.Row, move.CurrentPosition.Col];
			if (piece == null)
			{
                return;
            }
            if (piece.PieceType == PieceType.Pawn && move.CurrentPosition.Col != move.NextPosition.Col) //pawn that takes a piece
            {
                if (testBoardState[move.NextPosition.Row, move.NextPosition.Col] == null)
                {
                    switch (piece.PieceColor)
                    {
                        case PieceColor.White:
                            testBoardState[move.NextPosition.Row + 1, move.NextPosition.Col] = null;
                            break;
                        case PieceColor.Black:
                            testBoardState[move.NextPosition.Row - 1, move.NextPosition.Col] = null;
                            break;
                    }
                }
            }
            if (piece.PieceType == PieceType.King && Math.Abs(move.CurrentPosition.Col - move.NextPosition.Col) > 1) //castling
            {
                switch (piece.PieceColor)
                {
                    case PieceColor.White:
                        switch (move.NextPosition.Col)
                        {
                            case 2:
                                TestBoardState[7, 0] = null;
                                TestBoardState[7, 3] = new Rook(PieceColor.White, new Position(7, 3));
                                break;
                            case 6:
                                TestBoardState[7, 7] = null;
                                TestBoardState[7, 5] = new Rook(PieceColor.White, new Position(7, 3));
                                break;
                        }
                        break;
                    case PieceColor.Black:
                        switch (move.NextPosition.Col)
                        {
                            case 2:
                                TestBoardState[1, 0] = null;
                                TestBoardState[1, 3] = new Rook(PieceColor.Black, new Position(1, 3));
                                break;
                            case 6:
                                TestBoardState[1, 7] = null;
                                TestBoardState[1, 5] = new Rook(PieceColor.Black, new Position(1, 3));
                                break;
                        }
                        break;
                }
            }
            IPiece newPiece = Piece.NewPiece(piece);
            this.TestBoardState[move.CurrentPosition.Row, move.CurrentPosition.Col] = null;
            this.TestBoardState[move.NextPosition.Row, move.NextPosition.Col] = newPiece;
            piece.PiecePosition = new Position(move.NextPosition.Row, move.NextPosition.Col);
            newPiece.PiecePosition = new Position(move.NextPosition.Row, move.NextPosition.Col);
        }

        public IPiece[,] BoardDeepCopy()
        {
            IPiece[,] boardCopy = new IPiece[BoardSize, BoardSize];
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (this.BoardState[row, col] == null)
                    {
                        boardCopy[row, col] = null;
                    }
                    else
                    {
                        IPiece piece = this.BoardState[row, col];
                        boardCopy[row, col] = Piece.NewPiece(piece);
                    }
                }
            }
            return boardCopy;
        }
    }
}

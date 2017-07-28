﻿using JustPoChess.Client.Model.Entities.Pieces.PiecePosition;
using JustPoChess.Client.Model.Entities.Pieces.PiecesEnums;

namespace JustPoChess.Client.Model.Contracts
{
    public interface IPiece
    {
        Position PiecePosition { get; set; }
        PieceColor PieceColor { get; }
        PieceType PieceType { get; }

        void Draw();
        void Move();
    }
}
using System;

namespace Chess
{
    public class Board
    {
        const byte size = 8;
        Piece[,] pieces;

        public Board()
        {
            pieces = new Piece[size,size];
            Random random = new Random();

            bool white = 5 < random.Next(0, 10);

            for (int y = 0; y < size; y += size-1, white = !white)
            {
                pieces[0,y] = new Rook(white);
                pieces[1,y] = new Knight(white);
                pieces[2,y] = new Bishop(white);

                pieces[3,y] = new Queen(white);
                pieces[4,y] = new King(white);

                pieces[5,y] = new Bishop(white);
                pieces[6,y] = new Knight(white);
                pieces[7,y] = new Rook(white);
            }
            for (int y = 1; y < size; y += size-2, white = !white)
                for (int x = 0; x < size; x++) 
                    pieces[x,y] = new Pawn(white);
        }

        public Piece GetPiece(Position position) => pieces[position._X,position._Y];
    }

    public class Move
    {
        readonly Position start;
        readonly Position end;
        readonly Piece piece;

        public Move(Position start, Piece piece, Position end)
        {
            this.start = start;
            this.piece = piece;
            this.end = end;
        }

        public override string ToString() => $"{piece} {start} -> {end}";
    }
    public abstract class Piece
    {
        protected bool isWhite;

        public Piece (bool white)
        {
            isWhite = white;
        }
        public abstract Move[] GetMoves(Position self, ref Board board);
        public bool Teammate(Piece piece) => isWhite == piece.isWhite;
    }
    class Pawn : Piece
    {
        protected sbyte directionMod = 1;
        public Pawn (bool white) : base (white)
        {

        }
        public override Move[] GetMoves(Position self, ref Board board)
        {
            throw new NotImplementedException();
        }
    }
    class Rook : Piece
    {
        public Rook (bool white) : base (white)
        {

        }
        public override Move[] GetMoves(Position self, ref Board board)
        {
            throw new NotImplementedException();
        }
    }
    class Knight : Piece
    {
        public Knight (bool white) : base (white)
        {

        }
        public override Move[] GetMoves(Position self, ref Board board)
        {
            throw new NotImplementedException();
        }
        
    }
    class Bishop : Piece
    {
        public Bishop (bool white) : base (white)
        {

        }
        public override Move[] GetMoves(Position self, ref Board board)
        {
            throw new NotImplementedException();
        }

    }
    class King : Piece
    {
        public King (bool white) : base (white)
        {

        }
        public override Move[] GetMoves(Position self, ref Board board)
        {
            throw new NotImplementedException();
        }

    }
    class Queen : Piece
    {
        public Queen (bool white) : base (white)
        {

        }
        public override Move[] GetMoves(Position self, ref Board board)
        {
            throw new NotImplementedException();
        }

    }

    public class Position
    {
        X x; Y y;

        public byte _X => (byte)x;
        public byte _Y => (byte)x;

        public override string ToString()
        {
            char? c1 = Enum.GetName(typeof(X), x)?.ToLower().ToCharArray()[0];
            char? c2 = Enum.GetName(typeof(Y), y)?.ToLower().ToCharArray()[1];
            return $"{c1}{c2}";
        }

        public override bool Equals(object obj) => obj is Position p && p.x == x && y == p.y;
        public override int GetHashCode() => (byte)x * 10 + (byte)y;



        private enum X : byte {A,B,C,D,E,F,G,H}
        private enum Y : byte {_1,_2,_3,_4,_5,_6,_7,_8}
    }
}
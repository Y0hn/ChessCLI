using System;
using System.Collections;

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
        public bool OutPiece(Position position, out Piece piece)
        {
            piece = pieces[position._X,position._Y];
            return piece != null;
        } 
        public bool CheckSavety(Position position) => true;

        public override string ToString()
        {
            string brd = "";
            Position temp;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    temp = new(x,y)
                    brd+=$"{(OutPiece(temp, out Piece p) ? p : " "))}";
                }
                brd+="\n";
            }
            return brd;
        }
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

        public static Position Straight(Position pos, sbyte mod, bool vertical = true)
        {
            mod += (vertical ? pos._X : pos._Y);
            Position move = null;

            if (0 <= mod && mod < (vertical ? Position.Size_X : Position.Size_Y))
                move = (vertical) ? new(mod, pos._Y) : new(pos._X, mod);

            return move;
        }

        public static Position Diagonal(Position pos, byte modV, byte modH)
        {
            pos = Horizontal(pos, modH);
            if (pos != null) 
                pos = Vertical(pos, modV);
            return pos;
        }

        public override string ToString() => $"{piece} {start} -> {end}";
    }

    public class Position
    {
        X x; Y y;

        public byte _X => (byte)x;
        public byte _Y => (byte)y;

        public static byte Size_X => Enum.GetNames(typeof(X)).Lenght;
        public static byte Size_Y => Enum.GetNames(typeof(Y)).Lenght;

        public Position(byte _x, byte _y)
        {
            x = (X)_x;
            y = (Y)_y;
        }
        
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
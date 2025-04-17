using System;
using System.Collections;
using System.Drawing;

namespace Chess
{
    public class Board
    {
        const byte size = 8;
        Piece[,] pieces;

        public Board()
        {
            pieces = new Piece[size,size];
            Random random = new();

            // nahodne urcena farba
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
                
                sbyte dir = (sbyte)(y == 0 ? 1 : -1);
                int pY = y + dir;
                for (int x = 0; x < size; x++) 
                    pieces[x,pY] = new Pawn(white, dir);
            }
        }

        public Piece GetPiece(Position position) 
            => pieces[position._X,position._Y];
        public bool OutPiece(Position position, out Piece piece)
        {
            piece = pieces[position._X,position._Y];
            return piece != null;
        } 
        public bool CheckSavety(Position position) 
        {
            bool save = true;
            // tu impementacia kontroly bezpecnosti
            return save;
        }

        public override string ToString()
        {
            string brd = "";
            Position temp;
            for (byte y = 0; y < size; y++)
            {
                for (byte x = 0; x < size; x++)
                {
                    temp = new(x,y);
                    brd+=$"{(OutPiece(temp, out Piece p) ? p : " ")}";
                }
                brd+="\n";
            }
            return brd;
        }
        public void ConsoleWriteOut()
        {
            Console.Clear();
            ConsoleColor prev = Console.BackgroundColor;

            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    Console.BackgroundColor = (x+y)%2 == 1 ? ConsoleColor.Black : ConsoleColor.White;
                    Console.Write($" {pieces[x,y]} ");
                }

            Console.BackgroundColor = prev;
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

        public static Position? Straight(Position? pos, sbyte mod, bool vertical = true)
        {
            Position? move = null;
            if (pos != null)
            {                
                byte m = (byte)(mod + (vertical ? pos._X : pos._Y));

                if (0 <= mod && mod < (vertical ? Position.Size_X : Position.Size_Y))
                    move = vertical ? new(m, pos._Y) : new(pos._X, m);
            }
            return move;
        }

        public static Position? Diagonal(Position? pos, sbyte modV, sbyte modH)
        {
            pos = Straight(pos, modH, false);
            pos = Straight(pos, modV, true);
            return pos;
        }

        public override string ToString() 
            => $"{piece} {start} -> {end}";
    }

    public class Position(byte _x, byte _y)
    {
        X x = (X)_x; Y y = (Y)_y;

        public byte _X 
        {
            get => (byte)x;
            set => x = (X)value;
        } 
        public byte _Y
        {
            get => (byte)y;
            set => y = (Y)value;
        } 

        public static byte Size_X => (byte)Enum.GetNames(typeof(X)).Length;
        public static byte Size_Y => (byte)Enum.GetNames(typeof(Y)).Length;

        public override string ToString()
        {
            char? c1 = Enum.GetName(typeof(X), x)?.ToLower().ToCharArray()[0];
            char? c2 = Enum.GetName(typeof(Y), y)?.ToLower().ToCharArray()[1];
            return $"{c1}{c2}";
        }
        public override bool Equals(object? obj) 
            => obj != null && obj is Position p && p.x == x && y == p.y;
        public override int GetHashCode() 
            => (byte)x * 10 + (byte)y;



        private enum X : byte {A,B,C,D,E,F,G,H}
        private enum Y : byte {_1,_2,_3,_4,_5,_6,_7,_8}
    }
}
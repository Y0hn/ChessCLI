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

        public static Position Horizontal(Position pos, sbyte mod)
        {
            int x = pos._X + mod;
            return (0 <= x && x < Position.Size_X) ? new Position(x, pos._Y) : null;
        }
        public static Position Vertical(Position pos, sbyte mod)
        {
            int y = pos._Y + mod;
            return (0 <= y && y < Position.Size_Y) ? new Position(pos._X, y) : null;            
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
    public abstract class Piece
    {
        protected bool isWhite;

        public Piece (bool white)
        {
            isWhite = white;
        }
        public abstract List<Position> GetMoves(Position self, ref Board brd);
        public bool Capturable(Piece attacker) => isWhite == piece.isWhite;
        protected bool CanMoveTo(Position move, ref Board brd, bool forceTake = false, bool forceFree = false) 
        => move != null 
            && 
            (!(brd.OutPiece(move, out Piece p) 
                    && 
                    forceTake) 
                || 
            (p.Capturable(this) 
                    && 
                    !forceFree));
    }
    class Pawn : Piece
    {
        protected sbyte dMod = 1;
        public Pawn (bool white, sbyte direction) : base (white)
        {
            dMod = direction;
        }
        public override List<Position> GetMoves(Position self, ref Board brd)
        {
            List<Position> list = new();
            Position move;

            move = Move.Vertical(self, dMod);
            if (CanMoveTo(move, ref brd, false, true))
                list.Add(move);

            move = Move.Vertical(self, dMod * 2);
            if (CanMoveTo(move, ref brd, false, true) && (self._Y == 1 || 7 == self._Y))
                list.Add(move);

            move = Diagonal(self, dMod, -1);
            if (CanMoveTo(move, ref brd, true))
                list.Add(move);

            move = Diagonal(self, dMod,  1);
            if (CanMoveTo(move, ref brd, true))
                list.Add(move);

            // en passant //
            return list;
        }
        public override string ToString() => (isWhite) ? "♙" : "♟";
    }
    class King : Piece
    {
        public King (bool white) : base (white) {}
        public override List<Position> GetMoves(Position self, ref Board brd)
        {
            List<Position> list = new();
            Position move;
            for (int mod = -1; mod < 2; mod += 2)
            {
                move = Move.Horizontal(self, mod);
                if (CanMoveTo(move, ref brd) && brd.CheckSavety(move))
                    list.Add(move);
                move = Move.Vertical(self, mod);
                if (CanMoveTo(move, ref brd) && brd.CheckSavety(move))
                    list.Add(move);

                for (int modII = -1; mod < 2; mod += 2)
                {
                    move = Move.Diagonal(self, mod, modII);
                    if (CanMoveTo(move, ref brd) && brd.CheckSavety(move))
                        list.Add(move);
                }
            }
            return list;
        }
        public override string ToString() => (isWhite) ? "♔" : "♚"; 
    }
    class Knight : Piece
    {
        public Knight (bool white) : base (white) {}
        public override List<Position> GetMoves(Position self, ref Board brd)
        {
            List<Position> list = new();
            Position pre_move, move;
            for (int mod = -2; mod < 3; mod += 4)
            {
                pre_move = Horizontal(self, mod);
                for (int modII = -1; pre_move != null && mod < 2; mod += 2)
                {
                    move = Vertical(pre_move, modII);
                    if (CanMoveTo(move, ref brd))
                        list.Add(move);
                }

                pre_move = Vertical(self, mod);
                for (int modII = -1; pre_move != null && mod < 2; mod += 2)
                {
                    move = Horizontal(pre_move, modII);
                    if (CanMoveTo(move, ref brd))
                        list.Add(move);
                }
            }
            
            return list;
        }
        public override string ToString() => (isWhite) ? "♘" : "♞";        
    }
    class Rook : Piece
    {
        public Rook (bool white) : base (white) {}
        public override List<Position> GetMoves(Position self, ref Board brd)
        {
            List<Position> list = new();
            Position move;
            for (int mod = -1; mod < 2; mod += 2)
            {
                move = self;
                do {
                    move = Horizontal(move, mod);
                    if (CanMoveTo(move, ref brd))
                        list.Add(move);
                    else
                        break;
                } while (brd.GetPiece(move) == null)

                move = self;
                do {
                    move = Vertical(move, mod);
                    if (CanMoveTo(move, ref brd))
                        list.Add(move);
                    else
                        break;
                } while (brd.GetPiece(move) == null)
            }
            return list;
        }
        public override string ToString() => (isWhite) ? "♖" : "♜";
    }
    class Bishop : Piece
    {
        public Bishop (bool white) : base (white) {}
        public override Position[] GetMoves(Position self)
        {
            List<Position> list = new();
            Position move;
            for (int mod = -1; mod < 2; mod += 2)
                for (int modII = -1; modII < 2; modII += 2)
                {
                    move = self;
                    do {
                        move = Diagonal(move, mod, modII);
                        if (CanMoveTo(move, ref brd))
                            list.Add(move);
                        else
                            break;
                    } while (brd.GetPiece(move) == null)
                }
            return list;
        }
        public override string ToString() => (isWhite) ? "♗" : "♝"; 
    }
    class Queen : Piece
    {
        public Queen (bool white) : base (white) {}
        public override Position[] GetMoves(Position self)
        {
            List<Position> list = new();
            Position move;

            for (int mod = -1; mod < 2; mod += 2)
            {
                move = self;
                do {
                    move = Horizontal(move, mod);
                    if (CanMoveTo(move, ref brd))
                        list.Add(move);
                    else
                        break;
                } while (brd.GetPiece(move) == null)
                
                Position move = self;
                do {
                    move = Vertical(move, mod);
                    if (CanMoveTo(move, ref brd))
                        list.Add(move);
                    else
                        break;
                } while (brd.GetPiece(move) == null)
            }

            for (int mod = -1; mod < 2; mod += 2)
                for (int modII = -1; modII < 2; modII += 2)
                {
                    move = self;
                    do {
                        move = Diagonal(move, mod, modII);
                        if (CanMoveTo(move, ref brd))
                            list.Add(move);
                        else
                            break;
                    } while (brd.GetPiece(move) == null)
                }
            return list;
        }
        public override string ToString() => (isWhite) ? "♕" : "♛"; 
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
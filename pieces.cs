#pragma warning disable CS8604 // Possible null reference argument.

namespace Chess
{
    public abstract class Piece(bool white)
    {
        protected bool isWhite = white;

        public abstract List<Position> GetMoves(Position self, ref Board brd);
        public bool Capturable(Piece attacker) 
            => isWhite == attacker.isWhite;
        protected bool CanMoveTo(Position? move, ref Board brd, bool forceTake = false, bool forceFree = false) 
            => move != null && (!(brd.OutPiece(move, out Piece p) && forceTake) 
                            || (p.Capturable(this) && !forceFree));
    }
    class Pawn(bool white, sbyte direction) : Piece(white)
    {
        protected sbyte dMod = direction;
        public override List<Position> GetMoves(Position self, ref Board brd)
        {
            List<Position> list = [];
            Position? move;

            move = Move.Straight(self, dMod);
            if (CanMoveTo(move, ref brd, false, true))
                list.Add(move);

            move = Move.Straight(self, (sbyte)(dMod * 2));
            if (CanMoveTo(move, ref brd, false, true) && (self._Y == 1 || 7 == self._Y))
                list.Add(move);

            move = Move.Diagonal(self, dMod, -1);
            if (CanMoveTo(move, ref brd, true))
                list.Add(move);

            move = Move.Diagonal(self, dMod,  1);
            if (CanMoveTo(move, ref brd, true))
                list.Add(move);

            // en passant //
            return list;
        }
        public override string ToString() 
            => isWhite ? "♙" : "♟";
    }
    class King(bool white) : Piece(white)
    {
        public override List<Position> GetMoves(Position self, ref Board brd)
        {
            List<Position> list = [];
            Position? move;
            for (sbyte mod = -1; mod < 2; mod += 2)
            {
                for (sbyte i = 0; i < 2; i++)
                {
                    move = Move.Straight(self, mod, i==0);
                    if (CanMoveTo(move, ref brd) && brd.CheckSavety(move))
                        list.Add(move);
                }

                for (sbyte modII = -1; mod < 2; mod += 2)
                {
                    move = Move.Diagonal(self, mod, modII);
                    if (CanMoveTo(move, ref brd) && brd.CheckSavety(move))
                        list.Add(move);
                }
            }
            return list;
        }
        public override string ToString() 
            => isWhite ? "♔" : "♚"; 
    }
    class Knight(bool white) : Piece(white)
    {
        public override List<Position> GetMoves(Position self, ref Board brd)
        {
            List<Position> list = [];
            Position? pre_move, move;
            for (sbyte mod = -2; mod < 3; mod += 4)
            {
                for (int i = 0; i < 2; i++)
                {
                    pre_move = Move.Straight(self, mod, i==0);
                    for (sbyte modII = -1; pre_move != null && mod < 2; mod += 2)
                    {
                        move = Move.Straight(pre_move, modII, i!=0);
                        if (CanMoveTo(move, ref brd))
                            list.Add(move);
                    }
                }
            }            
            return list;
        }
        public override string ToString() 
            => isWhite ? "♘" : "♞";        
    }
    class Rook(bool white) : Piece(white)
    {
        public override List<Position> GetMoves(Position self, ref Board brd)
        {
            List<Position> list = [];
            Position? move;
            for (sbyte mod = -1; mod < 2; mod += 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    move = self;
                    do {
                        move = Move.Straight(move, mod, i==0);
                        if (CanMoveTo(move, ref brd))
                            list.Add(move);
                        else
                            break;
                    } while (brd.GetPiece(move) == null);                 
                }
            }
            return list;
        }
        public override string ToString() 
            => isWhite ? "♖" : "♜";
    }
    class Bishop(bool white) : Piece(white)
    {
        public override List<Position> GetMoves(Position self, ref Board brd)
        {
            List<Position> list = new();
            Position? move;
            for (sbyte mod = -1; mod < 2; mod += 2)
            {
                for (sbyte modII = -1; modII < 2; modII += 2)
                {
                    move = self;
                    do {
                        move = Move.Diagonal(move, mod, modII);
                        if (CanMoveTo(move, ref brd))
                            list.Add(move);
                        else
                            break;
                    } while (brd.GetPiece(move) == null);
                }
            }
            return list;
        }
        public override string ToString() 
            => isWhite ? "♗" : "♝"; 
    }
    class Queen(bool white) : Piece(white)
    {
        public override List<Position> GetMoves(Position self, ref Board brd)
        {
            List<Position> list = new();
            Position? move;
            for (sbyte mod = -1; mod < 2; mod += 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    move = self;
                    do {
                        move = Move.Straight(move, mod, i==0);
                        if (CanMoveTo(move, ref brd))
                            list.Add(move);
                        else
                            break;
                    } while (brd.GetPiece(move) == null);
                }
            }
            for (sbyte mod = -1; mod < 2; mod += 2)
            {
                for (sbyte modII = -1; modII < 2; modII += 2)
                {
                    move = self;
                    do {
                        move = Move.Diagonal(move, mod, modII);
                        if (CanMoveTo(move, ref brd))
                            list.Add(move);
                        else
                            break;
                    } while (brd.GetPiece(move) == null);
                }
            }
            return list;
        }

        public override string ToString() 
            => isWhite ? "♕" : "♛"; 
    }
}
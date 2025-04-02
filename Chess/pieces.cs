namespace Chess
{
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

            move = Move.Straight(self, dMod);
            if (CanMoveTo(move, ref brd, false, true))
                list.Add(move);

            move = Move.Straight(self, dMod * 2);
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
                for (int i = 0; i < 2; i++)
                {
                    move = Move.Straight(self, mod, i==0);
                    if (CanMoveTo(move, ref brd) && brd.CheckSavety(move))
                        list.Add(move);
                }

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
                for (int i = 0; i < 2; i++)
                {
                    pre_move = Move.Straight(self, mod, i==0);
                    for (int modII = -1; pre_move != null && mod < 2; mod += 2)
                    {
                        move = Move.Straight(pre_move, modII, i!=0);
                        if (CanMoveTo(move, ref brd))
                            list.Add(move);
                    }
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
                for (int i = 0; i < 2; i++)
                {
                    move = self;
                    do {
                        move = Move.Straight(move, mod, i==0);
                        if (CanMoveTo(move, ref brd))
                            list.Add(move);
                        else
                            break;
                    } while (brd.GetPiece(move) == null)                   
                }
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
            {
                for (int modII = -1; modII < 2; modII += 2)
                {
                    move = self;
                    do {
                        move = Move.Diagonal(move, mod, modII);
                        if (CanMoveTo(move, ref brd))
                            list.Add(move);
                        else
                            break;
                    } while (brd.GetPiece(move) == null)
                }
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
                for (int i = 0; i < 2; i++)
                {
                    move = self;
                    do {
                        move = Move.Straight(move, mod, i==0);
                        if (CanMoveTo(move, ref brd))
                            list.Add(move);
                        else
                            break;
                    } while (brd.GetPiece(move) == null)
                }
            }
            for (int mod = -1; mod < 2; mod += 2)
            {
                for (int modII = -1; modII < 2; modII += 2)
                {
                    move = self;
                    do {
                        move = Move.Diagonal(move, mod, modII);
                        if (CanMoveTo(move, ref brd))
                            list.Add(move);
                        else
                            break;
                    } while (brd.GetPiece(move) == null)
                }
            }
            return list;
        }
        public override string ToString() => (isWhite) ? "♕" : "♛"; 
    }
}
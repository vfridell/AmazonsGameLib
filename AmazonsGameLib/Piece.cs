using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public enum PieceName { Open, Amazon, Arrow, Wall };
    public enum Owner { None = 0, Player1 = 1, Player2 = 2 };
    public abstract class Piece
    {
        protected Piece() { }

        public abstract PieceName Name { get; }
        public abstract bool Impassible { get; }
        public abstract Owner Owner { get; }

        protected static Piece[,] _pieces = new Piece[4, 2];
        static Piece()
        {
            _pieces[(int)PieceName.Open, 0] = new Open();
            _pieces[(int)PieceName.Open, 1] = new Open();
            _pieces[(int)PieceName.Amazon, 0] = new AmazonPlayer1();
            _pieces[(int)PieceName.Amazon, 1] = new AmazonPlayer2();
            _pieces[(int)PieceName.Arrow, 0] = new ArrowPlayer1();
            _pieces[(int)PieceName.Arrow, 1] = new ArrowPlayer2();
            _pieces[(int)PieceName.Wall, 0] = new Wall();
            _pieces[(int)PieceName.Wall, 1] = new Wall();
        }

        public static Piece Get(PieceName pieceName)
        {
            if(pieceName == PieceName.Amazon || pieceName == PieceName.Arrow)
            {
                throw new ArgumentException("Don't use this for amazons or arrows. Must specify an owner");
            }
            return _pieces[(int)pieceName, 0];
        }
        public static Piece Get(PieceName pieceName, Owner owner)
        {
            int ownerNum = (int)owner;
            if (ownerNum > 0) ownerNum--;
            return _pieces[(int)pieceName, ownerNum];
        }

    }
}
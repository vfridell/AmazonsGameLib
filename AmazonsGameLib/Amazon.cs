using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public abstract class Amazon : Piece
    {
        protected Amazon() { }
        public override PieceName Name => PieceName.Amazon;
        public override bool Impassible => true;
        public abstract Arrow GetArrow();
    }

    public class AmazonPlayer1 : Amazon
    {
        internal AmazonPlayer1() { }
        public override Owner Owner => Owner.Player1;

        public override Arrow GetArrow() => ArrowPlayer1.Get();
        public static AmazonPlayer1 Get() => (AmazonPlayer1)Get(PieceName.Amazon, Owner.Player1);
    }

    public class AmazonPlayer2 : Amazon
    {
        internal AmazonPlayer2() { }
        public override Owner Owner => Owner.Player2;
        public override Arrow GetArrow() => ArrowPlayer2.Get();
        public static AmazonPlayer2 Get() => (AmazonPlayer2)Get(PieceName.Amazon, Owner.Player2);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public abstract class Arrow : Piece
    {
        public override PieceName Name => PieceName.Arrow;
        public override bool Impassible => true;
    }

    public class ArrowPlayer1 : Arrow
    {
        internal ArrowPlayer1() { }
        public override Owner Owner => Owner.Player1;
        public static ArrowPlayer1 Get() => (ArrowPlayer1)Get(PieceName.Arrow, Owner.Player1);
    }

    public class ArrowPlayer2 : Arrow
    {
        internal ArrowPlayer2() { }
        public override Owner Owner => Owner.Player2;
        public static ArrowPlayer2 Get() => (ArrowPlayer2)Get(PieceName.Arrow, Owner.Player2);
    }
}

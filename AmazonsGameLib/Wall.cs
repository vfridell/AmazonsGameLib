using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public class Wall : Piece
    {
        internal Wall() { }
        public override PieceName Name => PieceName.Wall;
        public override bool Impassible => true;
        public override Owner Owner => Owner.None;
        public static Wall Get() => (Wall)Get(PieceName.Wall);
    }
}

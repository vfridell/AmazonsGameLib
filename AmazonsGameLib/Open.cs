using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public class Open : Piece
    {
        internal Open() { }
        public override PieceName Name => PieceName.Open;
        public override bool Impassible => false;
        public override Owner Owner => Owner.None;
        public static Open Get() => (Open)Get(PieceName.Open);
    }
}

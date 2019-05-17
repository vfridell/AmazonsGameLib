using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public class Move
    {
        public Move() { }
        public Move(Point origin, Point amazon, Point arrow)
        {
            Origin = origin;
            AmazonsPoint = amazon;
            ArrowPoint = arrow;
        }
        public Point Origin { get; set; }
        public Point AmazonsPoint { get; set; }
        public Point ArrowPoint { get; set; }

        public override bool Equals(object obj)
        {
            Move other = obj as Move;
            if (other == null) return false;
            return Equals(other);
        }

        public bool Equals(Move other) => Origin.Equals(other.Origin) && AmazonsPoint.Equals(other.AmazonsPoint) && ArrowPoint.Equals(other.ArrowPoint);

        public override int GetHashCode() => (Origin.GetHashCode() * 397) ^ ((AmazonsPoint.GetHashCode() * 397) ^ ArrowPoint.GetHashCode());

        public override string ToString() => $"From {Origin}: Amazon to {AmazonsPoint}, Arrow to {ArrowPoint}";

    }
}

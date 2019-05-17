using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public static class PointHelpers
    {
        public enum GridDirection { UpLeft, Up, UpRight, Left, Right, DownLeft, Down, DownRight };
        public static Point [] AdjacentPointDeltas = {
            Point.Get(-1,1),
            Point.Get(0,1),
            Point.Get(1,1),
            Point.Get(-1,0),
            Point.Get(1,0),
            Point.Get(-1,-1),
            Point.Get(0,-1),
            Point.Get(1,-1),
        };
        public static IEnumerable<Point> GetAdjacentPoints(this Point centerPoint)
        {
            for (int i = 0; i < 8; i++)
            {
                yield return AdjacentPointDeltas[i] + centerPoint;
            }
        }
        public static IEnumerable<Point> GetAdjacentDeltas(this Point centerPoint)
        {
            for (int i = 0; i < 8; i++)
            {
                yield return AdjacentPointDeltas[i];
            }
        }
    }
}

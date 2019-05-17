using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public static class PieceHelpers
    {

        public static IDictionary<Point, Amazon> GetInitialAmazonPositions(int size)
        {
            switch(size)
            {
                case 10:
                    return Initial10;
                default:
                    throw new ArgumentException("I only support size 10", "size");
            }
        }

        public static readonly Dictionary<Point, Amazon> Initial10 = new Dictionary<Point, Amazon>()
        {
            { Point.Get(0,3), AmazonPlayer1.Get() },
            { Point.Get(3,0), AmazonPlayer1.Get() },
            { Point.Get(0,6), AmazonPlayer1.Get() },
            { Point.Get(9,3), AmazonPlayer1.Get() },
            { Point.Get(9,6), AmazonPlayer2.Get() },
            { Point.Get(6,0), AmazonPlayer2.Get() },
            { Point.Get(3,9), AmazonPlayer2.Get() },
            { Point.Get(6,9), AmazonPlayer2.Get() },
        };
    }
}

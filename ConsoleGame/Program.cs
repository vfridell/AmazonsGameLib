using AmazonsGameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Point[] arrowPoints = { Point.Get(2, 9), Point.Get(4, 9), Point.Get(3, 8), Point.Get(4, 8), Point.Get(2, 7), Point.Get(3, 7), Point.Get(5, 7), Point.Get(6, 7), Point.Get(7, 7), Point.Get(8, 7), Point.Get(1, 6), Point.Get(2, 6), Point.Get(4, 6), Point.Get(8, 6), Point.Get(0, 5), Point.Get(5, 5), Point.Get(1, 4), Point.Get(2, 4), Point.Get(5, 4), Point.Get(6, 4), Point.Get(7, 4), Point.Get(7, 3), Point.Get(1, 2), Point.Get(9, 2), Point.Get(0, 0), Point.Get(7, 0), };
            Dictionary<Point, Amazon> amazonsDict = new Dictionary<Point, Amazon> {
                { Point.Get(2,8), AmazonPlayer1.Get() }, { Point.Get(6,6), AmazonPlayer1.Get() }, { Point.Get(3,3), AmazonPlayer1.Get() }, { Point.Get(7,2), AmazonPlayer1.Get() },
                { Point.Get(3,9), AmazonPlayer2.Get() }, { Point.Get(4,7), AmazonPlayer2.Get() }, { Point.Get(3,1), AmazonPlayer2.Get() }, { Point.Get(6,1), AmazonPlayer2.Get() }
            };
            PieceGrid grid = new PieceGrid(10, amazonsDict);
            foreach (Point p in arrowPoints)
            {
                grid.PointPiecesDict[p] = ArrowPlayer1.Get();
            }

            Game game = new Game();
            game.Begin(null, null, 10);
            game.CurrentBoard.PieceGrid = grid;

            AmazonConsoleRenderer.Render(game);

        }
    }
}

using AmazonsGameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    public static class AmazonConsoleRenderer
    {
        public static void Render(Game game)
        {
            for (int x = 0; x < game.BoardSize + 2; x++) Console.Write('#');
            Console.WriteLine();
            
            for(int y = game.BoardSize - 1; y >= 0; y-- )
            {
                Console.Write('#');

                for (int x = 0; x < game.BoardSize; x++)
                {
                    Piece piece = game.CurrentBoard.PieceGrid.PointPiecesDict[Point.Get(x, y)];
                    Console.Write(GetPieceCharacter(piece));
                }

                Console.Write('#');
                Console.WriteLine();
            }

            for (int x = 0; x < game.BoardSize + 2; x++) Console.Write('#');
            Console.WriteLine();
        }

        private static char GetPieceCharacter(Piece p)
        {
            if (p is Wall) return 'W';
            else if (p is ArrowPlayer1) return '/';
            else if (p is ArrowPlayer2) return '\\';
            else if (p is AmazonPlayer1) return '1';
            else if (p is AmazonPlayer2) return '2';
            else if (p is Open) return '.';
            else return '?';
        }
    }
}

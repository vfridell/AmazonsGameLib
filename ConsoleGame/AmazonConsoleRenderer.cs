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
        public static void Render(Game game, AnalysisGraph analysisGraph)
        {
            for (int x = 0; x < game.BoardSize + 2; x++) Console.Write('#');
            Console.WriteLine();
            
            for(int y = game.BoardSize - 1; y >= 0; y-- )
            {
                Console.Write('#');

                StringBuilder mobilityStringBuilder = new StringBuilder(" ");
                for (int x = 0; x < game.BoardSize; x++)
                {
                    Piece piece = game.CurrentBoard.PieceGrid.PointPieces[Point.Get(x, y)];
                    Console.Write(GetPieceCharacter(piece));
                    if (piece is Amazon) mobilityStringBuilder.Append($"{analysisGraph.AmazonMobilityScores[Point.Get(x,y)]:0.00}, ");
                }

                Console.Write('#');

                Console.Write($"{mobilityStringBuilder.ToString(),30}");

                if (y == game.BoardSize - 1) Console.Write($" W: {analysisGraph.W,6:0.00}");
                if (y == game.BoardSize - 2) Console.Write($" T1: {analysisGraph.T1,5:0.00}");
                if (y == game.BoardSize - 3) Console.Write($" T2: {analysisGraph.T2,5:0.00}");
                if (y == game.BoardSize - 4) Console.Write($" C1: {analysisGraph.C1,5:0.00}");
                if (y == game.BoardSize - 5) Console.Write($" C2: {analysisGraph.C2,5:0.00}");
                if (y == game.BoardSize - 6) Console.Write($" T: {analysisGraph.T,6:0.00}");
                if (y == game.BoardSize - 7) Console.Write($" M: {analysisGraph.M,6:0.00}");
                if (y == game.BoardSize - 8) Console.Write($" T+M: {analysisGraph.T + analysisGraph.M,4:0.00}");
                if (y == game.BoardSize - 9) Console.Write($" Moves: {game.CurrentMoves.Count(),2:0}");

                Console.WriteLine();
            }

            for (int x = 0; x < game.BoardSize + 2; x++) Console.Write('#');
            Console.WriteLine();
        }

        private static char GetPieceCharacter(Piece p)
        {
            if (p is Wall) return 'W';
            else if (p is ArrowPlayer1) return '*';
            else if (p is ArrowPlayer2) return '*';
            else if (p is AmazonPlayer1) return '1';
            else if (p is AmazonPlayer2) return '2';
            else if (p is Open) return ' ';
            else return '?';
        }
    }
}

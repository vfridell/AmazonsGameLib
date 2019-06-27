using AmazonsGameLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            //RenderSpecificState();
            //PlayRandomGame();
            //PlayRandomVsOptimusDeepGame();
            PlayOneHundredGames();
            PlayFourRandomMovesAhead();
        }

        public static void PlayFourRandomMovesAhead()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            AnalysisGraph analysisGraph = new AnalysisGraph();
            List<(Game, IAnalysisResult)> games = new List<(Game, IAnalysisResult)>();
            for (int i = 0; i < 100; i++)
            {
                Random rnd = new Random();
                Game game = new Game();
                game.Begin(null, null, 10);
                int n = 4;
                while (n > 0 && !game.IsComplete())
                {
                    int randomMoveNum = rnd.Next(0, game.CurrentMoves.Count - 1);
                    game.ApplyMove(game.CurrentMoves.ElementAt(randomMoveNum));
                    n--;
                }

                games.Add((game, analysisGraph.Analyze(game.CurrentBoard)));
            }
            sw.Stop();

            Console.WriteLine($"Elapsed: {sw.Elapsed}");

        }

        public static void PlayOneHundredGames()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            List<Game> games = new List<Game>();
            for (int i = 0; i < 100; i++)
            {
                Random rnd = new Random();
                Game game = new Game();
                game.Begin(null, null, 10);
                while (!game.IsComplete())
                {
                    int randomMoveNum = rnd.Next(0, game.CurrentMoves.Count - 1);
                    game.ApplyMove(game.CurrentMoves.ElementAt(randomMoveNum));
                }
                games.Add(game);
            }
            sw.Stop();

            Console.WriteLine($"Elapsed: {sw.Elapsed}");

            double player1Wins = games.Count(g => g.GetGameResult() == GameResult.Player1Won);
            double player2Wins = games.Count(g => g.GetGameResult() == GameResult.Player2Won);

            Console.WriteLine($"{player1Wins}/{player2Wins} = {player1Wins / player2Wins:0.00}");

        }

static void ShowMobilityScorePairs()
        {
            Game game = new Game();
            game.Begin(null, null, 10);

            AnalysisGraph analysisGraph = new AnalysisGraph();
            analysisGraph.BuildAnalysis(game.CurrentBoard.PieceGrid, game.CurrentPlayer);

            foreach (var p in game.CurrentBoard.PieceGrid.Amazon1Points.Union(game.CurrentBoard.PieceGrid.Amazon2Points))
            {
                Console.WriteLine($"{analysisGraph.W:0.00},{analysisGraph.AmazonMobilityScores[p]:0.00}");
            }

            Random rnd = new Random();
            while (!game.IsComplete())
            {
                int randomMoveNum = rnd.Next(0, game.CurrentMoves.Count - 1);
                game.ApplyMove(game.CurrentMoves.ElementAt(randomMoveNum));

                analysisGraph.BuildAnalysis(game.CurrentBoard.PieceGrid, game.CurrentPlayer);

                foreach (var p in game.CurrentBoard.PieceGrid.Amazon1Points.Union(game.CurrentBoard.PieceGrid.Amazon2Points))
                {
                    Console.WriteLine($"{analysisGraph.W:0.00},{analysisGraph.AmazonMobilityScores[p]:0.00}");
                }
            }
        }

        static void PlayRandomVsOptimusDeepGame()
        {
            Game game = new Game();
            game.Begin(null, null, 10);

            AnalysisGraph analysisGraph = new AnalysisGraph();
            analysisGraph.BuildAnalysis(game.CurrentBoard.PieceGrid, game.CurrentPlayer);

            AmazonConsoleRenderer.Render(game, analysisGraph);
            OptimusDeep optimusDeep = new OptimusDeep(3, analysisGraph);
            optimusDeep.BeginNewGame(Owner.Player2, 10);

            Random rnd = new Random();
            while (!game.IsComplete())
            {
                int randomMoveNum = rnd.Next(0, game.CurrentMoves.Count - 1);
                game.ApplyMove(game.CurrentMoves.ElementAt(randomMoveNum));

                analysisGraph.BuildAnalysis(game.CurrentBoard.PieceGrid, game.CurrentPlayer);

                AmazonConsoleRenderer.Render(game, analysisGraph);

                if (game.IsComplete()) continue;

                var cancellationTokenSrc = new CancellationTokenSource();
                var bestMoveTask = Task<Move>.Run(() => optimusDeep.PickBestMoveAsync(game.CurrentBoard, cancellationTokenSrc.Token));
                Task.Delay(1000).Wait();
                if (!bestMoveTask.IsCompleted) cancellationTokenSrc.Cancel();
                Move bestMove = bestMoveTask.Result;
                game.ApplyMove(bestMove);

                analysisGraph.BuildAnalysis(game.CurrentBoard.PieceGrid, game.CurrentPlayer);
                AmazonConsoleRenderer.Render(game, analysisGraph);
            }
            Console.WriteLine(game.GetGameResult().ToString());
        }

        static void PlayRandomGame()
        {
            Game game = new Game();
            game.Begin(null, null, 10);

            AnalysisGraph analysisGraph = new AnalysisGraph();
            analysisGraph.BuildAnalysis(game.CurrentBoard.PieceGrid, game.CurrentPlayer);

            AmazonConsoleRenderer.Render(game, analysisGraph);

            Random rnd = new Random();
            while (!game.IsComplete())
            {
                int randomMoveNum = rnd.Next(0, game.CurrentMoves.Count - 1);
                game.ApplyMove(game.CurrentMoves.ElementAt(randomMoveNum));

                analysisGraph.BuildAnalysis(game.CurrentBoard.PieceGrid, game.CurrentPlayer);

                AmazonConsoleRenderer.Render(game, analysisGraph);
            }
            Console.WriteLine(game.GetGameResult().ToString());
        }

        static void RenderSpecificState()
        {
            Point[] arrowPoints = { Point.Get(2, 9), Point.Get(4, 9), Point.Get(3, 8), Point.Get(4, 8), Point.Get(2, 7), Point.Get(3, 7), Point.Get(5, 7), Point.Get(6, 7), Point.Get(7, 7), Point.Get(8, 7), Point.Get(1, 6), Point.Get(2, 6), Point.Get(4, 6), Point.Get(8, 6), Point.Get(0, 5), Point.Get(5, 5), Point.Get(1, 4), Point.Get(2, 4), Point.Get(5, 4), Point.Get(6, 4), Point.Get(7, 4), Point.Get(7, 3), Point.Get(1, 2), Point.Get(9, 2), Point.Get(0, 0), Point.Get(7, 0), };
            Dictionary<Point, Amazon> amazonsDict = new Dictionary<Point, Amazon> {
                { Point.Get(2,8), AmazonPlayer1.Get() }, { Point.Get(6,6), AmazonPlayer1.Get() }, { Point.Get(3,3), AmazonPlayer1.Get() }, { Point.Get(7,2), AmazonPlayer1.Get() },
                { Point.Get(3,9), AmazonPlayer2.Get() }, { Point.Get(4,7), AmazonPlayer2.Get() }, { Point.Get(3,1), AmazonPlayer2.Get() }, { Point.Get(6,1), AmazonPlayer2.Get() }
            };
            PieceGrid grid = new PieceGrid(10, amazonsDict);
            foreach (Point p in arrowPoints)
            {
                grid.PointPieces[p] = ArrowPlayer1.Get();
            }

            Game game = new Game();
            game.Begin(null, null, 10);
            game.CurrentBoard.PieceGrid = grid;

            AnalysisGraph analysisGraph = new AnalysisGraph();
            analysisGraph.BuildAnalysis(game.CurrentBoard.PieceGrid, game.CurrentPlayer);
            AmazonConsoleRenderer.Render(game, analysisGraph);
        }
    }
}

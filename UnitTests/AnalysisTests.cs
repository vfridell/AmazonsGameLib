using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AmazonsGameLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class AnalysisTests
    {
        [TestMethod]
        public void Distances()
        {
            Dictionary<Point, double> player1QueenDistancesAnswer = new Dictionary<Point, double> { { Point.Get(0, 1), 2 }, { Point.Get(0, 2), 2 }, { Point.Get(0, 3), 1 }, { Point.Get(0, 4), 2 }, { Point.Get(0, 6), 1 }, { Point.Get(0, 7), 2 }, { Point.Get(0, 8), 1 }, { Point.Get(0, 9), 2 }, { Point.Get(1, 0), 2 }, { Point.Get(1, 1), 1 }, { Point.Get(1, 3), 1 }, { Point.Get(1, 5), 2 }, { Point.Get(1, 7), 1 }, { Point.Get(1, 8), 1 }, { Point.Get(1, 9), 1 }, { Point.Get(2, 0), 2 }, { Point.Get(2, 1), 2 }, { Point.Get(2, 2), 1 }, { Point.Get(2, 3), 1 }, { Point.Get(2, 5), 2 }, { Point.Get(3, 0), 2 }, { Point.Get(3, 2), 1 }, { Point.Get(3, 4), 1 }, { Point.Get(3, 5), 1 }, { Point.Get(3, 6), 1 }, { Point.Get(4, 0), 2 }, { Point.Get(4, 1), 2 }, { Point.Get(4, 2), 1 }, { Point.Get(4, 3), 1 }, { Point.Get(4, 4), 1 }, { Point.Get(4, 5), 2 }, { Point.Get(5, 0), 2 }, { Point.Get(5, 1), 1 }, { Point.Get(5, 2), 1 }, { Point.Get(5, 3), 1 }, { Point.Get(5, 6), 1 }, { Point.Get(5, 8), 3 }, { Point.Get(5, 9), 3 }, { Point.Get(6, 0), 1 }, { Point.Get(6, 2), 1 }, { Point.Get(6, 3), 1 }, { Point.Get(6, 5), 1 }, { Point.Get(6, 8), 3 }, { Point.Get(6, 9), 3 }, { Point.Get(7, 1), 1 }, { Point.Get(7, 5), 1 }, { Point.Get(7, 6), 1 }, { Point.Get(7, 8), 3 }, { Point.Get(7, 9), 3 }, { Point.Get(8, 0), 2 }, { Point.Get(8, 1), 1 }, { Point.Get(8, 2), 1 }, { Point.Get(8, 3), 1 }, { Point.Get(8, 4), 1 }, { Point.Get(8, 5), 2 }, { Point.Get(8, 8), 3 }, { Point.Get(8, 9), 3 }, { Point.Get(9, 0), 1 }, { Point.Get(9, 1), 2 }, { Point.Get(9, 3), 1 }, { Point.Get(9, 4), 1 }, { Point.Get(9, 5), 2 }, { Point.Get(9, 6), 2 }, { Point.Get(9, 7), 2 }, { Point.Get(9, 8), 2 }, { Point.Get(9, 9), 2 },};
            Dictionary<Point, double> player2QueenDistancesAnswer = new Dictionary<Point, double> { { Point.Get(0, 1), 1 }, { Point.Get(0, 2), 2 }, { Point.Get(0, 3), 2 }, { Point.Get(0, 4), 1 }, { Point.Get(0, 6), 3 }, { Point.Get(0, 7), 4 }, { Point.Get(0, 8), 4 }, { Point.Get(0, 9), 4 }, { Point.Get(1, 0), 2 }, { Point.Get(1, 1), 1 }, { Point.Get(1, 3), 1 }, { Point.Get(1, 5), 2 }, { Point.Get(1, 7), 4 }, { Point.Get(1, 8), 5 }, { Point.Get(1, 9), 5 }, { Point.Get(2, 0), 1 }, { Point.Get(2, 1), 1 }, { Point.Get(2, 2), 1 }, { Point.Get(2, 3), 2 }, { Point.Get(2, 5), 1 }, { Point.Get(3, 0), 1 }, { Point.Get(3, 2), 1 }, { Point.Get(3, 4), 1 }, { Point.Get(3, 5), 2 }, { Point.Get(3, 6), 1 }, { Point.Get(4, 0), 1 }, { Point.Get(4, 1), 1 }, { Point.Get(4, 2), 1 }, { Point.Get(4, 3), 1 }, { Point.Get(4, 4), 2 }, { Point.Get(4, 5), 2 }, { Point.Get(5, 0), 1 }, { Point.Get(5, 1), 1 }, { Point.Get(5, 2), 1 }, { Point.Get(5, 3), 1 }, { Point.Get(5, 6), 1 }, { Point.Get(5, 8), 1 }, { Point.Get(5, 9), 2 }, { Point.Get(6, 0), 1 }, { Point.Get(6, 2), 1 }, { Point.Get(6, 3), 1 }, { Point.Get(6, 5), 1 }, { Point.Get(6, 8), 2 }, { Point.Get(6, 9), 1 }, { Point.Get(7, 1), 1 }, { Point.Get(7, 5), 2 }, { Point.Get(7, 6), 2 }, { Point.Get(7, 8), 2 }, { Point.Get(7, 9), 2 }, { Point.Get(8, 0), 2 }, { Point.Get(8, 1), 1 }, { Point.Get(8, 2), 2 }, { Point.Get(8, 3), 2 }, { Point.Get(8, 4), 2 }, { Point.Get(8, 5), 2 }, { Point.Get(8, 8), 2 }, { Point.Get(8, 9), 2 }, { Point.Get(9, 0), 2 }, { Point.Get(9, 1), 1 }, { Point.Get(9, 3), 2 }, { Point.Get(9, 4), 3 }, { Point.Get(9, 5), 2 }, { Point.Get(9, 6), 3 }, { Point.Get(9, 7), 3 }, { Point.Get(9, 8), 2 }, { Point.Get(9, 9), 2 },};
            Dictionary<Point, double> player1KingDistancesAnswer = new Dictionary<Point, double> { { Point.Get(0, 1), 3 }, { Point.Get(0, 2), 3 }, { Point.Get(0, 3), 3 }, { Point.Get(0, 4), 3 }, { Point.Get(0, 6), 2 }, { Point.Get(0, 7), 2 }, { Point.Get(0, 8), 2 }, { Point.Get(0, 9), 2 }, { Point.Get(1, 0), 3 }, { Point.Get(1, 1), 2 }, { Point.Get(1, 3), 2 }, { Point.Get(1, 5), 3 }, { Point.Get(1, 7), 1 }, { Point.Get(1, 8), 1 }, { Point.Get(1, 9), 1 }, { Point.Get(2, 0), 3 }, { Point.Get(2, 1), 2 }, { Point.Get(2, 2), 1 }, { Point.Get(2, 3), 1 }, { Point.Get(2, 5), 2 }, { Point.Get(3, 0), 3 }, { Point.Get(3, 2), 1 }, { Point.Get(3, 4), 1 }, { Point.Get(3, 5), 2 }, { Point.Get(3, 6), 3 }, { Point.Get(4, 0), 3 }, { Point.Get(4, 1), 2 }, { Point.Get(4, 2), 1 }, { Point.Get(4, 3), 1 }, { Point.Get(4, 4), 1 }, { Point.Get(4, 5), 2 }, { Point.Get(5, 0), 3 }, { Point.Get(5, 1), 2 }, { Point.Get(5, 2), 2 }, { Point.Get(5, 3), 2 }, { Point.Get(5, 6), 1 }, { Point.Get(5, 8), 8 }, { Point.Get(5, 9), 8 }, { Point.Get(6, 0), 2 }, { Point.Get(6, 2), 1 }, { Point.Get(6, 3), 1 }, { Point.Get(6, 5), 1 }, { Point.Get(6, 8), 7 }, { Point.Get(6, 9), 7 }, { Point.Get(7, 1), 1 }, { Point.Get(7, 5), 1 }, { Point.Get(7, 6), 1 }, { Point.Get(7, 8), 6 }, { Point.Get(7, 9), 6 }, { Point.Get(8, 0), 2 }, { Point.Get(8, 1), 1 }, { Point.Get(8, 2), 1 }, { Point.Get(8, 3), 1 }, { Point.Get(8, 4), 2 }, { Point.Get(8, 5), 2 }, { Point.Get(8, 8), 5 }, { Point.Get(8, 9), 6 }, { Point.Get(9, 0), 2 }, { Point.Get(9, 1), 2 }, { Point.Get(9, 3), 2 }, { Point.Get(9, 4), 2 }, { Point.Get(9, 5), 3 }, { Point.Get(9, 6), 3 }, { Point.Get(9, 7), 4 }, { Point.Get(9, 8), 5 }, { Point.Get(9, 9), 6 },};
            Dictionary<Point, double> player2KingDistancesAnswer = new Dictionary<Point, double> { { Point.Get(0, 1), 3 }, { Point.Get(0, 2), 3 }, { Point.Get(0, 3), 3 }, { Point.Get(0, 4), 3 }, { Point.Get(0, 6), 4 }, { Point.Get(0, 7), 5 }, { Point.Get(0, 8), 6 }, { Point.Get(0, 9), 7 }, { Point.Get(1, 0), 2 }, { Point.Get(1, 1), 2 }, { Point.Get(1, 3), 2 }, { Point.Get(1, 5), 3 }, { Point.Get(1, 7), 5 }, { Point.Get(1, 8), 6 }, { Point.Get(1, 9), 7 }, { Point.Get(2, 0), 1 }, { Point.Get(2, 1), 1 }, { Point.Get(2, 2), 1 }, { Point.Get(2, 3), 2 }, { Point.Get(2, 5), 2 }, { Point.Get(3, 0), 1 }, { Point.Get(3, 2), 1 }, { Point.Get(3, 4), 3 }, { Point.Get(3, 5), 2 }, { Point.Get(3, 6), 1 }, { Point.Get(4, 0), 1 }, { Point.Get(4, 1), 1 }, { Point.Get(4, 2), 1 }, { Point.Get(4, 3), 2 }, { Point.Get(4, 4), 3 }, { Point.Get(4, 5), 2 }, { Point.Get(5, 0), 1 }, { Point.Get(5, 1), 1 }, { Point.Get(5, 2), 1 }, { Point.Get(5, 3), 2 }, { Point.Get(5, 6), 1 }, { Point.Get(5, 8), 1 }, { Point.Get(5, 9), 2 }, { Point.Get(6, 0), 1 }, { Point.Get(6, 2), 1 }, { Point.Get(6, 3), 2 }, { Point.Get(6, 5), 2 }, { Point.Get(6, 8), 2 }, { Point.Get(6, 9), 2 }, { Point.Get(7, 1), 1 }, { Point.Get(7, 5), 3 }, { Point.Get(7, 6), 3 }, { Point.Get(7, 8), 3 }, { Point.Get(7, 9), 3 }, { Point.Get(8, 0), 2 }, { Point.Get(8, 1), 2 }, { Point.Get(8, 2), 2 }, { Point.Get(8, 3), 3 }, { Point.Get(8, 4), 4 }, { Point.Get(8, 5), 4 }, { Point.Get(8, 8), 4 }, { Point.Get(8, 9), 4 }, { Point.Get(9, 0), 3 }, { Point.Get(9, 1), 3 }, { Point.Get(9, 3), 3 }, { Point.Get(9, 4), 4 }, { Point.Get(9, 5), 5 }, { Point.Get(9, 6), 5 }, { Point.Get(9, 7), 5 }, { Point.Get(9, 8), 5 }, { Point.Get(9, 9), 5 }, };
;
            PieceGrid grid = GetPieceGrid();

            var analysisGraph = new AnalysisGraph();
            analysisGraph.BuildAnalysis(grid, Owner.Player1);

            foreach (var kvp in player1QueenDistancesAnswer)
            {
                Assert.IsTrue(analysisGraph.Player1QueenMinDistances.ContainsKey(kvp.Key));
                Assert.AreEqual(kvp.Value, analysisGraph.Player1QueenMinDistances[kvp.Key]);
            }
            foreach (var kvp in player2QueenDistancesAnswer)
            {
                Assert.IsTrue(analysisGraph.Player2QueenMinDistances.ContainsKey(kvp.Key));
                Assert.AreEqual(kvp.Value, analysisGraph.Player2QueenMinDistances[kvp.Key]);
            }
            foreach (var kvp in player1KingDistancesAnswer)
            {
                Assert.IsTrue(analysisGraph.Player1KingMinDistances.ContainsKey(kvp.Key));
                Assert.AreEqual(kvp.Value, analysisGraph.Player1KingMinDistances[kvp.Key]);
            }
            foreach (var kvp in player2KingDistancesAnswer)
            {
                Assert.IsTrue(analysisGraph.Player2KingMinDistances.ContainsKey(kvp.Key));
                Assert.AreEqual(kvp.Value, analysisGraph.Player2KingMinDistances[kvp.Key]);
            }
        }

        [TestMethod]
        public void AllFirstMovesAnalyzed()
        {
            Game game = new Game();
            game.Begin(null, null, 10);

            Parallel.ForEach(game.CurrentMoves, (move) =>
           {
               var analysisGraph = new AnalysisGraph();
               Board nextBoard = game.CurrentBoard.Clone();
               nextBoard.ApplyMove(move);
               analysisGraph.BuildAnalysis(nextBoard.PieceGrid, Owner.Player1);
           });
        }

        [TestMethod]
        public void AllFirstMovesAnalyzedLite()
        {
            Game game = new Game();
            game.Begin(null, null, 10);

            Parallel.ForEach(game.CurrentMoves, (move) =>
           {
               var analysisGraph = new AnalysisGraphLite();
               Board nextBoard = game.CurrentBoard.Clone();
               nextBoard.ApplyMove(move);
               analysisGraph.BuildAnalysis(nextBoard.PieceGrid, Owner.Player1);
           });
        }

        [TestMethod]
        public void AllFirstMoves()
        {
            Game game = new Game();
            game.Begin(null, null, 10);

            foreach (Move move in game.CurrentMoves)
            {
                Board nextBoard = game.CurrentBoard.Clone();
                nextBoard.ApplyMove(move);
            }
        }


        [TestMethod]
        public void AllSecondMoves()
        {
            Game game = new Game();
            game.Begin(null, null, 10);

            foreach (Move move in game.CurrentMoves)
            {
                Board nextBoard = game.CurrentBoard.Clone();
                nextBoard.ApplyMove(move);
                IEnumerable<Move> moves = nextBoard.GetAvailableMovesForCurrentPlayer();
                System.Threading.Tasks.Parallel.ForEach(moves, (move2) =>
                {
                    Board nextBoard2 = nextBoard.Clone();
                    nextBoard2.ApplyMove(move2);
                });
            }
        }

        [TestMethod]
        public void EvaluationValues()
        {
            PieceGrid grid = GetPieceGrid();
            var analysisGraph = new AnalysisGraph();
            analysisGraph.BuildAnalysis(grid, Owner.Player1);
            Assert.AreEqual("43.38", $"{analysisGraph.W:0.00}");
            Assert.AreEqual("8.40", $"{analysisGraph.T1:0.00}");
            Assert.AreEqual("-3.20", $"{analysisGraph.T2:0.00}");
            Assert.AreEqual("3.88", $"{analysisGraph.C1:0.00}");
            Assert.AreEqual("1.83", $"{analysisGraph.C2:0.00}");
            Assert.AreEqual("0.87", $"{analysisGraph.T:0.00}");
            Assert.AreEqual("17.51", $"{analysisGraph.M:0.00}");
            Assert.AreEqual("18.38", $"{analysisGraph.T + analysisGraph.M:0.00}");
        }

        [TestMethod]
        public void EvaluationValuesLite()
        {
            PieceGrid grid = GetPieceGrid();
            var analysisGraph = new AnalysisGraphLite();
            analysisGraph.BuildAnalysis(grid, Owner.Player1);
            Assert.AreEqual("8.40", $"{analysisGraph.T1:0.00}");
        }

        private PieceGrid GetPieceGrid()
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
            return grid;
        }
    }
}

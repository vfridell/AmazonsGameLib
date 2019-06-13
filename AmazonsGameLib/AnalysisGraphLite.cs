using QuickGraph;
using QuickGraph.Algorithms.ConnectedComponents;
using QuickGraph.Algorithms.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    /// <summary>
    /// This analysis class for The Game of Amazons is based on:
    /// J. Lieberum, An evaluation function for the game of amazons, Theoretical Computer Science 349 (2005) 230 – 244
    /// </summary>
    public class AnalysisGraphLite : IBoardAnalyzer
    {
        Guid LastAnalyzedPieceGridId { get; set; }

        /// <summary>
        /// Each <see cref="Point"/> mapped to a <see cref="LocalAdvantage"/>
        /// </summary>
        public PointSquareArray<LocalAdvantage> LocalAdvantages;
        /// <summary>
        /// Minimum queen move distances for player 1 at each open, reachable point on the PieceGrid
        /// </summary>
        public PointSquareArray<double?> Player1QueenMinDistances;
        /// <summary>
        /// Minimum queen move distances for player 2 at each open, reachable point on the PieceGrid
        /// </summary>
        public PointSquareArray<double?> Player2QueenMinDistances;

        /// <summary>
        /// Sum of local queen move advantages for each player. Greater distances have no affect on the local score
        /// Positive values indicate advantage for player 1, negative player 2
        /// </summary>
        public double T1 { get; set; }

        /// <summary>
        /// Build all the internal analysis data for the given PieceGrid
        /// </summary>
        /// <param name="pieceGrid">PieceGrid to analyze</param>
        /// <param name="playerToMove">Player whos turn it is</param>
        public void BuildAnalysis(PieceGrid pieceGrid, Owner playerToMove)
        {
            if (pieceGrid.Id == LastAnalyzedPieceGridId) return;
            LastAnalyzedPieceGridId = pieceGrid.Id;

            LocalAdvantages = new PointSquareArray<LocalAdvantage>(pieceGrid.Size);

            Player1QueenMinDistances = BuildDistancesDictionary(pieceGrid, Owner.Player1);
            Player2QueenMinDistances = BuildDistancesDictionary(pieceGrid, Owner.Player2);

            CalculateLocalAdvantages(pieceGrid, playerToMove);

            T1 = LocalAdvantages.Where(a => a.Value != null).Sum( a => a.Value.DeltaQueen );
        }

        /// <summary>
        /// Build <see cref="LocalAdvantage"/> objects for each open space on the PieceGrid
        /// </summary>
        /// <param name="pieceGrid">PieceGrid to analyze</param>
        /// <param name="playerToMove">Player whos turn it is</param>
        private void CalculateLocalAdvantages(PieceGrid pieceGrid, Owner playerToMove)
        {
            LocalAdvantages.Clear();
            foreach (var kvp in pieceGrid.PointPieces)
            {
                if (!(kvp.Value is Open)) continue;
                double player1QueenDistance = Player1QueenMinDistances[kvp.Key] ?? 1000d;
                double player2QueenDistance = Player2QueenMinDistances[kvp.Key] ?? 1000d;
                LocalAdvantage advantageValue = new LocalAdvantage
                {
                    Player1QueenDistance = player1QueenDistance,
                    Player2QueenDistance = player2QueenDistance,
                    PlayerToMove = playerToMove,
                };
                LocalAdvantages[kvp.Key] = advantageValue;
            }
        }

        private void FloodFillMinDistancesQueen(Point point, PieceGrid pieceGrid, PointSquareArray<double?> result)
        {
            ISet<Point> visited = new HashSet<Point>();
            Queue<(Point, double)> toVisit = new Queue<(Point, double)>();
            foreach (Point p in pieceGrid.GetOpenPointsOutFrom(point))
            {
                toVisit.Enqueue((p, 1));
            }

            while (toVisit.Any())
            {
                (Point, double) p = toVisit.Dequeue();
                if (visited.Contains(p.Item1)) continue;
                if (result[p.Item1].HasValue) result[p.Item1] = Math.Min(p.Item2, result[p.Item1].Value);
                else result.Add(p.Item1, p.Item2);
                visited.Add(p.Item1);
                foreach (Point pNext in pieceGrid.GetOpenPointsOutFrom(p.Item1)
                                               .Where(adj => !visited.Contains(adj)))
                {
                    toVisit.Enqueue((pNext, p.Item2 + 1));
                }
            }
        }

        /// <summary>
        /// Create a dictionary of points mapped to the minimum distance to that point for the given player and move type.
        /// As a side effect, this fills the Specific*Distances dictionary for the given <paramref name="owner"/>
        /// </summary>
        /// <param name="pieceGrid">PieceGrid to analyze</param>
        /// <param name="owner">Player whos amazons we are calculating distance for</param>
        /// <param name="queen">True for Queen distances, false for King distances</param>
        /// <returns>Dictionary of Points with their minimum distance values for the given player and move type</returns>
        private PointSquareArray<double?> BuildDistancesDictionary(PieceGrid pieceGrid, Owner owner)
        {
            var distancesDictionary = new PointSquareArray<double?>(pieceGrid.Size);
            ISet<Point> amazonPoints;
            if (owner == Owner.Player1) amazonPoints = pieceGrid.Amazon1Points;
            else amazonPoints = pieceGrid.Amazon2Points;
            foreach (Point amazonPoint in amazonPoints)
            {
                FloodFillMinDistancesQueen(amazonPoint, pieceGrid, distancesDictionary);
            }
            return distancesDictionary;
        }

        public IAnalysisResult Analyze(Board board)
        {
            BuildAnalysis(board.PieceGrid, board.CurrentPlayer);

            var result = new AnalysisGraphLiteResult() { player1Advantage = T1 };
            if (board.GetAvailableMovesForCurrentPlayer().Any()) result.gameResult = GameResult.Incomplete;
            else if (board.CurrentPlayer == Owner.Player1) result.gameResult = GameResult.Player2Won;
            else result.gameResult = GameResult.Player1Won;
            return result;
        }
    }

    public class AnalysisGraphLiteResult : IAnalysisResult
    {
        public double player1Advantage { get; set; }

        public GameResult gameResult { get; set; }
    }
}

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
    public class AnalysisGraph : IBoardAnalyzer
    {
        Guid LastAnalyzedPieceGridId { get; set; }

        /// <summary>
        /// An undirected graph representing queen moves between points on the PieceGrid
        /// </summary>
        public UndirectedGraph<Point, UndirectedEdge<Point>> QueenAdjacencyGraph = new UndirectedGraph<Point, UndirectedEdge<Point>>();
        /// <summary>
        /// An undirected graph representing king moves between points on the PieceGrid
        /// </summary>
        public UndirectedGraph<Point, UndirectedEdge<Point>> KingAdjacencyGraph = new UndirectedGraph<Point, UndirectedEdge<Point>>();
        /// <summary>
        /// Each <see cref="Point"/> mapped to a <see cref="LocalAdvantage"/>
        /// </summary>
        public Dictionary<Point, LocalAdvantage> LocalAdvantages = new Dictionary<Point, LocalAdvantage>();
        /// <summary>
        /// Each Point on the grid with an amazon on it mapped to a computed mobility score
        /// </summary>
        public Dictionary<Point, double> AmazonMobilityScores = new Dictionary<Point, double>();
        /// <summary>
        /// The set of articulation points on the PieceGrid
        /// </summary>
        public HashSet<Point> ArticulationPoints = new HashSet<Point>();
        /// <summary>
        /// Minimum queen move distances for player 1 at each open, reachable point on the PieceGrid
        /// </summary>
        public IDictionary<Point, double> Player1QueenMinDistances;
        /// <summary>
        /// Minimum king move distances for player 1 at each open, reachable point on the PieceGrid
        /// </summary>
        public IDictionary<Point, double> Player1KingMinDistances;
        /// <summary>
        /// Minimum queen move distances for player 2 at each open, reachable point on the PieceGrid
        /// </summary>
        public IDictionary<Point, double> Player2QueenMinDistances;
        /// <summary>
        /// Minimum king move distances for player 2 at each open, reachable point on the PieceGrid
        /// </summary>
        public IDictionary<Point, double> Player2KingMinDistances;
        /// <summary>
        /// Queen move distance for each specific amazon to each other open, reachable point on the PieceGrid
        /// </summary>
        public IDictionary<Point, IDictionary<Point, double>> SpecificQueenDistances = new Dictionary<Point, IDictionary<Point,double>>();
        /// <summary>
        /// King move distance for each specific amazon to each other open, reachable point on the PieceGrid
        /// </summary>
        public IDictionary<Point, IDictionary<Point, double>> SpecificKingDistances = new Dictionary<Point, IDictionary<Point,double>>();

        /// <summary>
        /// A weight >= 0 that indicates global competitive reach of breathing space. Will typically decrease each move, will
        /// definitely decrease from start of game to finish.
        /// Will be 0 if and only if we reach point of the game where each amazon is enclosed in a separate space
        /// </summary>
        public double W { get; set; }
        /// <summary>
        /// Sum of the local queen move advantages for each player with greater distance differences being rewarded/penalized more
        /// Positive values indicate advantage for player 1, negative player 2
        /// </summary>
        public double C1 { get; set; }
        /// <summary>
        /// Sum of the local king move advantages for each player with greater distance differences being rewarded/penalized more
        /// Positive values indicate advantage for player 1, negative player 2
        /// </summary>
        public double C2 { get; set; }
        /// <summary>
        /// Sum of local queen move advantages for each player. Greater distances have no affect on the local score
        /// Positive values indicate advantage for player 1, negative player 2
        /// </summary>
        public double T1 { get; set; }
        /// <summary>
        /// Sum of local king move advantages for each player. Greater distances have no affect on the local score
        /// Positive values indicate advantage for player 1, negative player 2
        /// </summary>
        public double T2 { get; set; }

        public double T { get; set; }

        public double M { get; set; }

        public double IndividualWeightForM(double w, double mobilityScore)
        {
            if (w < 0) throw new ArgumentException($"w must be greater than or equal to zero {w}");
            // z = x / (y + 2)
            return w / (mobilityScore + 2d);
        }

        // T1
        //http://www.mathopenref.com/graphfunctions.html?fx=(1 - (1 + ((-1)/(1 + 30 * E^(-.2 *x)))))/3&gx=1 + ((-1)/(1 + 30 * E^(-.2 *x)))&xh=70&xl=-0.1&yh=1.1&yl=-0.1&a=1.5&cr=t&cx=26.6979
        public double F1(double w)
        {
            if (w < 0) throw new ArgumentException($"w must be greater than or equal to zero {w}");
            if (w == 0) return 1d;
            return 1 + ((-1) / (1 + 30 * Math.Pow(Math.E, -.2 * w)));
        }

        // T2
        public double F2(double w)
        {
            if (w < 0) throw new ArgumentException($"w must be greater than or equal to zero {w}");
            double result = (1 - F1(w)) / 3;
            if (result < 0) return 0;
            return result;
        }

        // C1
        public double F3(double w)
        {
            return F2(w);
        }

        // C2
        public double F4(double w)
        {
            return F2(w);
        }

        /// <summary>
        /// Build an adjacency graph of all open or amazon nodes to represent the current moveable board area
        /// Nodes (points) on the same line are all given edges to each other to represent "queen" moves
        /// </summary>
        /// <param name="pieceGrid">The PieceGrid to build an adjacency graph from</param>
        private void InitializeQueenAdjacencyGraph(PieceGrid pieceGrid)
        {
            QueenAdjacencyGraph.Clear();
            for (int x = 0; x < pieceGrid.Size; x++)
            {
                for (int y = 0; y < pieceGrid.Size; y++)
                {
                    Point point = Point.Get(x, y);
                    Piece piece = pieceGrid.PointPiecesDict[point];

                    if (piece is Open || piece is Amazon)
                    {
                        if (!QueenAdjacencyGraph.ContainsVertex(point)) QueenAdjacencyGraph.AddVertex(point);
                        IEnumerable<UndirectedEdge<Point>> edges = pieceGrid.GetOpenPointsOutFrom(point).Select(op => new UndirectedEdge<Point>(point, op));
                        //IEnumerable<UndirectedEdge<Point>> edges = pieceGrid.GetNonArrowPointsOutFrom(point).Select(op => new UndirectedEdge<Point>(point, op));
                        foreach(UndirectedEdge<Point> edge in edges)
                        {
                            if (!QueenAdjacencyGraph.ContainsVertex(edge.Target)) QueenAdjacencyGraph.AddVertex(edge.Target);
                            if (!QueenAdjacencyGraph.ContainsEdge(edge)) QueenAdjacencyGraph.AddEdge(edge);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Build an adjacency graph of all open or amazon nodes to represent the current moveable board area
        /// Only points directly adjacent are given edges to each other to represent "king" moves
        /// </summary>
        /// <param name="pieceGrid">The PieceGrid to build an adjacency graph from</param>
        private void InitializeKingAdjacencyGraph(PieceGrid pieceGrid)
        {
            KingAdjacencyGraph.Clear();
            for (int x = 0; x < pieceGrid.Size; x++)
            {
                for (int y = 0; y < pieceGrid.Size; y++)
                {
                    Point point = Point.Get(x, y);
                    Piece piece = pieceGrid.PointPiecesDict[point];

                    if (piece is Open || piece is Amazon)
                    {
                        foreach(Point adjacentPoint in PointHelpers.GetAdjacentPoints(point))
                        {
                            if (pieceGrid.IsOutOfBounds(adjacentPoint)) continue;
                            Piece adjacentPiece = pieceGrid.PointPiecesDict[adjacentPoint];
                            if (adjacentPiece is Open)
                            {
                                if (!KingAdjacencyGraph.ContainsVertex(point)) KingAdjacencyGraph.AddVertex(point);
                                if (!KingAdjacencyGraph.ContainsVertex(adjacentPoint)) KingAdjacencyGraph.AddVertex(adjacentPoint);
                                UndirectedEdge<Point> edge = new UndirectedEdge<Point>(point, adjacentPoint);
                                if (!KingAdjacencyGraph.ContainsEdge(edge)) KingAdjacencyGraph.AddEdge(edge);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Build all the internal analysis data for the given PieceGrid
        /// </summary>
        /// <param name="pieceGrid">PieceGrid to analyze</param>
        /// <param name="playerToMove">Player whos turn it is</param>
        public void BuildAnalysis(PieceGrid pieceGrid, Owner playerToMove)
        {
            if (pieceGrid.Id == LastAnalyzedPieceGridId) return;
            LastAnalyzedPieceGridId = pieceGrid.Id;

            InitializeQueenAdjacencyGraph(pieceGrid);
            InitializeKingAdjacencyGraph(pieceGrid);

            Player1QueenMinDistances = BuildDistancesDictionary(pieceGrid, Owner.Player1, true, QueenAdjacencyGraph);
            Player1KingMinDistances = BuildDistancesDictionary(pieceGrid, Owner.Player1, false, KingAdjacencyGraph);
            Player2QueenMinDistances = BuildDistancesDictionary(pieceGrid, Owner.Player2, true, QueenAdjacencyGraph);
            Player2KingMinDistances = BuildDistancesDictionary(pieceGrid, Owner.Player2, false, KingAdjacencyGraph);

            //FindArticulationPoints();

            CalculateLocalAdvantages(pieceGrid, playerToMove);
            CalculateAllAmazonMobility(pieceGrid);

            W = LocalAdvantages.Where( a => a.Value.Player1Reachable && a.Value.Player2Reachable )
                               .Sum( a => Math.Pow(2, -(Math.Abs(a.Value.Player1QueenDistance - a.Value.Player2QueenDistance))) );
            C1 = 2 * LocalAdvantages.Sum( a => Math.Pow(2, -(a.Value.Player1QueenDistance)) - Math.Pow(2, -(a.Value.Player2QueenDistance)) );
            C2 = LocalAdvantages.Sum( a => Math.Min(1, Math.Max(-1, (a.Value.Player2KingDistance-a.Value.Player1KingDistance) / 6d)) );
            T1 = LocalAdvantages.Sum( a => a.Value.DeltaQueen );
            T2 = LocalAdvantages.Sum( a => a.Value.DeltaKing );


            T = (F1(W) * T1) + (F2(W) * C1) + (F3(W) * C2) + (F4(W) * T2);

            double player1MobilitySum = AmazonMobilityScores.Where(s => pieceGrid.Amazon1Points.Contains(s.Key))
                                                            .Sum(s => IndividualWeightForM(W, s.Value));
            double player2MobilitySum = AmazonMobilityScores.Where(s => pieceGrid.Amazon2Points.Contains(s.Key))
                                                            .Sum(s => IndividualWeightForM(W, s.Value));
            M = player2MobilitySum - player1MobilitySum;
        }

        /// <summary>
        /// Calculate the mobility score for all amazons on the PieceGrid
        /// </summary>
        /// <param name="pieceGrid">PieceGrid to analyze</param>
        private void CalculateAllAmazonMobility(PieceGrid pieceGrid)
        {
            AmazonMobilityScores.Clear();
            foreach(Point p in pieceGrid.Amazon1Points)
            {
                AmazonMobilityScores.Add(p, CalculateAmazonMobility(p, Owner.Player1, pieceGrid));
            }
            foreach (Point p in pieceGrid.Amazon2Points)
            {
                AmazonMobilityScores.Add(p, CalculateAmazonMobility(p, Owner.Player2, pieceGrid));
            }
        }

        /// <summary>
        /// Calculate the mobility score for a specific amazon point on the PieceGrid.
        /// </summary>
        /// <remarks>
        /// Mobility is based on the breathing space of each space that can be moved to by an 
        /// amazon at the given point. Closer (king distance) spaces are more valuable than 
        /// distant ones.
        /// </remarks>
        /// <param name="p">Point containing an amazon</param>
        /// <param name="pieceGrid">PieceGrid to analyze</param>
        /// <returns>Numeric mobility score, higher numbers = more mobile</returns>
        private double CalculateAmazonMobility(Point p, Owner owner, PieceGrid pieceGrid)
        {
            IDictionary<Point, double> minDistancesOppositePlayer;
            if (owner == Owner.Player1) minDistancesOppositePlayer = Player2QueenMinDistances;
            else if (owner == Owner.Player2) minDistancesOppositePlayer = Player1QueenMinDistances;
            else throw new ArgumentException($"Point {p} doesn't have an amazon for either player!");

            var edges = QueenAdjacencyGraph.AdjacentEdges(p);
            double mobility = 0d;
            foreach(var edge in edges)
            {
                if (!minDistancesOppositePlayer.ContainsKey(edge.Target)) continue;
                if (owner == Owner.Player1 && !LocalAdvantages[edge.Target].Player2Reachable) continue;
                if (owner == Owner.Player2 && !LocalAdvantages[edge.Target].Player1Reachable) continue;
                double localMobility = Math.Pow(2, -(SpecificKingDistances[edge.Source][edge.Target])) * QueenAdjacencyGraph.AdjacentDegree(edge.Target);
                mobility += localMobility;
            }
            return mobility;
        }

        /// <summary>
        /// Build <see cref="LocalAdvantage"/> objects for each open space on the PieceGrid
        /// </summary>
        /// <param name="pieceGrid">PieceGrid to analyze</param>
        /// <param name="playerToMove">Player whos turn it is</param>
        private void CalculateLocalAdvantages(PieceGrid pieceGrid, Owner playerToMove)
        {
            LocalAdvantages.Clear();
            foreach (var kvp in pieceGrid.PointPiecesDict)
            {
                if (!(kvp.Value is Open)) continue;
                if (!Player1QueenMinDistances.TryGetValue(kvp.Key, out double player1QueenDistance)) player1QueenDistance = 1000d;
                if (!Player2QueenMinDistances.TryGetValue(kvp.Key, out double player2QueenDistance)) player2QueenDistance = 1000d;
                if (!Player1KingMinDistances.TryGetValue(kvp.Key, out double player1KingDistance)) player1KingDistance = 1000d;
                if (!Player2KingMinDistances.TryGetValue(kvp.Key, out double player2KingDistance)) player2KingDistance = 1000d;
                LocalAdvantage advantageValue = new LocalAdvantage
                {
                    Player1QueenDistance = player1QueenDistance,
                    Player2QueenDistance = player2QueenDistance,
                    Player1KingDistance = player1KingDistance,
                    Player2KingDistance = player2KingDistance,
                    PlayerToMove = playerToMove,
                };
                LocalAdvantages[kvp.Key] = advantageValue;
            }
        }

        /// <summary>
        /// Find all articulation points on the grid.
        /// An articulation point is a node whose removal causes one subgraph to become two. In Game of Amazons, this 
        /// indicates a nearly enclosed space
        /// </summary>
        private void FindArticulationPoints()
        {
            var connectedComponentsAlgorithm = new ConnectedComponentsAlgorithm<Point, UndirectedEdge<Point>>(KingAdjacencyGraph);
            connectedComponentsAlgorithm.Compute();

            ArticulationPoints.Clear();
            foreach (var c in connectedComponentsAlgorithm.Components.GroupBy(kvp => kvp.Value))
            {
                Point root = c.First().Key;

                var articulationPointObserver = new UndirectedArticulationPointObserver<Point, UndirectedEdge<Point>>(ArticulationPoints);
                var dfs = new UndirectedDepthFirstSearchAlgorithm<Point, UndirectedEdge<Point>>(KingAdjacencyGraph);

                using (articulationPointObserver.Attach(dfs))
                {
                    dfs.Compute(root);
                }
            }
        }

        /// <summary>
        /// Create a dictionary of points mapped to the minimum distance to that point for the given player and move type
        /// </summary>
        /// <param name="pieceGrid">PieceGrid to analyze</param>
        /// <param name="owner">Player whos amazons we are calculating distance for</param>
        /// <param name="queen">True for Queen distances, false for King distances</param>
        /// <param name="adjacencyGraph">The adjacency graph to use for distances. Must match the <paramref name="queen"/> parameter</param>
        /// <returns>Dictionary of Points with their minimum distance values for the given player and move type</returns>
        private IDictionary<Point, double> BuildDistancesDictionary(PieceGrid pieceGrid, Owner owner, bool queen, UndirectedGraph<Point, UndirectedEdge<Point>> adjacencyGraph)
        {
            var distancesDictionary = new Dictionary<Point, double>();
            ISet<Point> amazonPoints;
            if (owner == Owner.Player1) amazonPoints = pieceGrid.Amazon1Points;
            else amazonPoints = pieceGrid.Amazon2Points;
            foreach (Point amazonPoint in amazonPoints)
            {
                var clonedAdjacencyGraph = new UndirectedGraph<Point, UndirectedEdge<Point>>();
                clonedAdjacencyGraph.AddVerticesAndEdgeRange(adjacencyGraph.Edges);
                // remove all the graph vertices for amazons other than the current one
                clonedAdjacencyGraph.RemoveVertexIf(v => !amazonPoint.Equals(v) && (pieceGrid.Amazon1Points.Contains(v) || pieceGrid.Amazon2Points.Contains(v)));
                // amazon is trapped?
                if (!clonedAdjacencyGraph.ContainsVertex(amazonPoint)) continue;
                // find the distance to all other points from this amazon
                var shortestPathAlgorithm = new QuickGraph.Algorithms.ShortestPath.UndirectedDijkstraShortestPathAlgorithm<Point, UndirectedEdge<Point>>(clonedAdjacencyGraph, w => 1d);
                shortestPathAlgorithm.Compute(amazonPoint);

                if (queen)
                {
                    if (SpecificQueenDistances.ContainsKey(amazonPoint)) SpecificQueenDistances.Remove(amazonPoint);
                    SpecificQueenDistances.Add(amazonPoint, shortestPathAlgorithm.Distances);
                }
                else
                {
                    if (SpecificKingDistances.ContainsKey(amazonPoint)) SpecificKingDistances.Remove(amazonPoint);
                    SpecificKingDistances.Add(amazonPoint, shortestPathAlgorithm.Distances);
                }

                foreach (var kvp in shortestPathAlgorithm.Distances)
                {
                    if (!distancesDictionary.ContainsKey(kvp.Key)) distancesDictionary.Add(kvp.Key, kvp.Value);
                    else distancesDictionary[kvp.Key] = Math.Min(distancesDictionary[kvp.Key], kvp.Value);
                }
            }
            return distancesDictionary;
        }

        public IAnalysisResult Analyze(Board board)
        {
            InitializeQueenAdjacencyGraph(board.PieceGrid);
            InitializeKingAdjacencyGraph(board.PieceGrid);
            BuildAnalysis(board.PieceGrid, board.CurrentPlayer);

            var result = new AnalysisGraphResult() { player1Advantage = T + M };
            if (board.GetAvailableMovesForCurrentPlayer().Count > 0) result.gameResult = GameResult.Incomplete;
            else if (board.CurrentPlayer == Owner.Player1) result.gameResult = GameResult.Player2Won;
            else result.gameResult = GameResult.Player1Won;
            return result;
        }
    }

    public class AnalysisGraphResult : IAnalysisResult
    {
        public double player1Advantage { get; set; }

        public GameResult gameResult { get; set; }
    }
}

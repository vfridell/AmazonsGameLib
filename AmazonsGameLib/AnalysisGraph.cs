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
    public class AnalysisGraph
    {
        public UndirectedGraph<Point, UndirectedEdge<Point>> QueenAdjacencyGraph = new UndirectedGraph<Point, UndirectedEdge<Point>>();
        public UndirectedGraph<Point, UndirectedEdge<Point>> KingAdjacencyGraph = new UndirectedGraph<Point, UndirectedEdge<Point>>();
        public Dictionary<Point, LocalAdvantage> LocalAdvantages = new Dictionary<Point, LocalAdvantage>();
        public Dictionary<Point, double> AmazonMobilityScores = new Dictionary<Point, double>();

        public HashSet<Point> ArticulationPoints = new HashSet<Point>();
        public IDictionary<Point, double> Player1QueenMinDistances;
        public IDictionary<Point, double> Player1KingMinDistances;
        public IDictionary<Point, double> Player2QueenMinDistances;
        public IDictionary<Point, double> Player2KingMinDistances;

        public IDictionary<Point, IDictionary<Point, double>> SpecificQueenDistances = new Dictionary<Point, IDictionary<Point,double>>();
        public IDictionary<Point, IDictionary<Point, double>> SpecificKingDistances = new Dictionary<Point, IDictionary<Point,double>>();

        public double W { get; set; }
        public double C1 { get; set; }
        public double C2 { get; set; }
        public double T1 { get; set; }
        public double T2 { get; set; }

        /// <summary>
        /// Build an adjacency graph of all open or amazon nodes to represent the current moveable board area
        /// Nodes (points) on the same line are all given edges to each other to represent "queen" moves
        /// </summary>
        /// <param name="pieceGrid">The PieceGrid to build an adjacency graph from</param>
        public void InitializeQueenAdjacencyGraph(PieceGrid pieceGrid)
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
        public void InitializeKingAdjacencyGraph(PieceGrid pieceGrid)
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

        public void BuildAnalysis(PieceGrid pieceGrid, Owner playerToMove)
        {
            Player1QueenMinDistances = BuildDistancesDictionary(pieceGrid, Owner.Player1, true, QueenAdjacencyGraph);
            Player1KingMinDistances = BuildDistancesDictionary(pieceGrid, Owner.Player1, false, KingAdjacencyGraph);
            Player2QueenMinDistances = BuildDistancesDictionary(pieceGrid, Owner.Player2, true, QueenAdjacencyGraph);
            Player2KingMinDistances = BuildDistancesDictionary(pieceGrid, Owner.Player2, false, KingAdjacencyGraph);

            FindArticulationPoints();

            CalculateLocalAdvantages(pieceGrid, playerToMove);
            CalculateAllAmazonMobility(pieceGrid);

            W = LocalAdvantages.Where( a => !double.IsInfinity(a.Value.Player1QueenDistance) && !double.IsInfinity(a.Value.Player2QueenDistance) )
                                                 .Sum( a => Math.Pow(2, -(Math.Abs(a.Value.Player1QueenDistance - a.Value.Player2QueenDistance))) );
            C1 = 2 * LocalAdvantages.Sum( a => Math.Pow(2, -(a.Value.Player1QueenDistance)) - Math.Pow(2, -(a.Value.Player2QueenDistance)) );
            C2 = LocalAdvantages.Sum( a => Math.Min(1, Math.Max(-1, (a.Value.Player2KingDistance-a.Value.Player1KingDistance) / 6d)) );
            T1 = LocalAdvantages.Sum( a => a.Value.DeltaQueen );
            T2 = LocalAdvantages.Sum( a => a.Value.DeltaKing );
        }

        private void CalculateAllAmazonMobility(PieceGrid pieceGrid)
        {
            AmazonMobilityScores.Clear();
            foreach(Point p in pieceGrid.Amazon1Points)
            {
                AmazonMobilityScores.Add(p, CalculateAmazonMobility(p, pieceGrid));
            }
            foreach (Point p in pieceGrid.Amazon2Points)
            {
                AmazonMobilityScores.Add(p, CalculateAmazonMobility(p, pieceGrid));
            }
        }

        private double CalculateAmazonMobility(Point p, PieceGrid pieceGrid)
        {
            var edges = QueenAdjacencyGraph.AdjacentEdges(p);
            double mobility = 0d;
            foreach(var edge in edges)
            {
                double localMobility = Math.Pow(2, -(SpecificKingDistances[edge.Source][edge.Target])) * QueenAdjacencyGraph.AdjacentDegree(edge.Target);
                mobility += localMobility;
            }
            return mobility;
        }

        private void CalculateLocalAdvantages(PieceGrid pieceGrid, Owner playerToMove)
        {
            LocalAdvantages.Clear();
            foreach (var kvp in pieceGrid.PointPiecesDict)
            {
                if (!(kvp.Value is Open)) continue;
                if (!Player1QueenMinDistances.TryGetValue(kvp.Key, out double player1QueenDistance)) player1QueenDistance = double.PositiveInfinity;
                if (!Player2QueenMinDistances.TryGetValue(kvp.Key, out double player2QueenDistance)) player2QueenDistance = double.PositiveInfinity;
                if (!Player1KingMinDistances.TryGetValue(kvp.Key, out double player1KingDistance)) player1KingDistance = double.PositiveInfinity;
                if (!Player2KingMinDistances.TryGetValue(kvp.Key, out double player2KingDistance)) player2KingDistance = double.PositiveInfinity;
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

        private void FindArticulationPoints()
        {
            var connectedComponentsAlgorithm = new ConnectedComponentsAlgorithm<Point, UndirectedEdge<Point>>(KingAdjacencyGraph);
            connectedComponentsAlgorithm.Compute();

            ArticulationPoints.Clear();
            foreach (var c in connectedComponentsAlgorithm.Components.GroupBy(kvp => kvp.Value))
            {
                Point root = c.First().Key;

                // An articulation point is a node whose removal causes one subgraph to become two
                var articulationPointObserver = new UndirectedArticulationPointObserver<Point, UndirectedEdge<Point>>(ArticulationPoints);
                var dfs = new UndirectedDepthFirstSearchAlgorithm<Point, UndirectedEdge<Point>>(KingAdjacencyGraph);

                using (articulationPointObserver.Attach(dfs))
                {
                    dfs.Compute(root);
                }
            }
        }

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

    }
}

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

        public HashSet<Point> ArticulationPoints = new HashSet<Point>();

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
                        IEnumerable<UndirectedEdge<Point>> edges = pieceGrid.GetNonArrowPointsOutFrom(point).Select(op => new UndirectedEdge<Point>(point, op));
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
                        if (!KingAdjacencyGraph.ContainsVertex(point)) KingAdjacencyGraph.AddVertex(point);
                        foreach(Point adjacentPoint in PointHelpers.GetAdjacentPoints(point))
                        {
                            if (pieceGrid.IsOutOfBounds(adjacentPoint)) continue;
                            Piece adjacentPiece = pieceGrid.PointPiecesDict[point];
                            if (adjacentPiece is Open || adjacentPiece is Amazon)
                            {
                                if (!KingAdjacencyGraph.ContainsVertex(adjacentPoint)) KingAdjacencyGraph.AddVertex(adjacentPoint);
                                UndirectedEdge<Point> edge = new UndirectedEdge<Point>(point, adjacentPoint);
                                if (!KingAdjacencyGraph.ContainsEdge(edge)) KingAdjacencyGraph.AddEdge(edge);
                            }
                        }
                    }
                }
            }
        }

        public void BuildAnalysis(PieceGrid pieceGrid)
        {
            var connectedComponentsAlgorithm = new ConnectedComponentsAlgorithm<Point, UndirectedEdge<Point>>(QueenAdjacencyGraph);
            connectedComponentsAlgorithm.Compute();

            var queenDistancesPlayer1 = new Dictionary<Point, double>();
            var kingDistancesPlayer1 = new Dictionary<Point, double>();
            foreach(Point amazonPoint in pieceGrid.Amazon1Points)
            {
                var clonedAdjacencyGraph = new UndirectedGraph<Point, UndirectedEdge<Point>>();
                clonedAdjacencyGraph.AddVerticesAndEdgeRange(QueenAdjacencyGraph.Edges);
                // remove all the graph vertices for amazons other than the current one
                clonedAdjacencyGraph.RemoveVertexIf(v => !amazonPoint.Equals(v) && (pieceGrid.Amazon1Points.Contains(v) || pieceGrid.Amazon2Points.Contains(v)));
                // find the distance to all other points from this amazon
                var shortestPathAlgorithm = new QuickGraph.Algorithms.ShortestPath.UndirectedDijkstraShortestPathAlgorithm<Point, UndirectedEdge<Point>>(QueenAdjacencyGraph, w => 1d);
                shortestPathAlgorithm.Compute(amazonPoint);
                foreach (var kvp in shortestPathAlgorithm.Distances)
                {
                    if (!queenDistancesPlayer1.ContainsKey(kvp.Key)) queenDistancesPlayer1.Add(kvp.Key, kvp.Value);
                    else queenDistancesPlayer1[kvp.Key] = Math.Min(queenDistancesPlayer1[kvp.Key], kvp.Value);
                }
            }


            /*
            // An articulation point is a node whose removal causes one subgraph to become two
            var articulationPointObserver = new UndirectedArticulationPointObserver<Point, UndirectedEdge<Point>>(ArticulationPoints);
            var dfs = new UndirectedDepthFirstSearchAlgorithm<Point, UndirectedEdge<Point>>(QueenAdjacencyGraph);

            ArticulationPoints.Clear();

            using (articulationPointObserver.Attach(dfs))
            {
                Point root = _playedPoints.Keys.First();
                dfs.Compute(root);
            }
            */
        }

    }
}

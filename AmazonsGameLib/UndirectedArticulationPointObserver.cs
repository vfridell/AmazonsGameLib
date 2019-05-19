using System;
using System.Collections.Generic;
using QuickGraph;
using QuickGraph.Algorithms.Search;

namespace AmazonsGameLib
{
    public class UndirectedArticulationPointObserver<TVertex, TEdge> : IObserver<UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge>> where TEdge : global::QuickGraph.UndirectedEdge<TVertex>
    {
        public UndirectedArticulationPointObserver(ISet<TVertex> articulationPoints)
        {
            _articulationPoints = articulationPoints;
        }

        public UndirectedArticulationPointObserver()
            : this(new HashSet<TVertex>())
        { }

        public ISet<TVertex> articulationPoints
        {
            get { return this._articulationPoints; }
        }

        private Dictionary<TVertex, int> _discoverTimes = new Dictionary<TVertex, int>();
        private Dictionary<TVertex, int> _lowDiscoverTimes = new Dictionary<TVertex, int>();
        private Dictionary<TVertex, int> _dfsChildren = new Dictionary<TVertex, int>();
        private IDictionary<TVertex, TVertex> _vertexPredecessors = new Dictionary<TVertex, TVertex>();
        private readonly ISet<TVertex> _articulationPoints;
        private int _time = 0;

        public IDisposable Attach(UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge> algorithm)
        {
            algorithm.DiscoverVertex += dfs_DiscoverVertex;
            algorithm.FinishVertex += dfs_FinishVertex;
            algorithm.BackEdge += dfs_BackEdge;
            algorithm.TreeEdge += dfs_TreeEdge;

            return new DisposableAction(
            () =>
            {
                algorithm.DiscoverVertex -= new VertexAction<TVertex>(dfs_DiscoverVertex);
                algorithm.FinishVertex -= new VertexAction<TVertex>(dfs_FinishVertex);
                algorithm.BackEdge -= new UndirectedEdgeAction<TVertex, TEdge>(dfs_BackEdge);
                algorithm.TreeEdge -= new UndirectedEdgeAction<TVertex,TEdge>(dfs_TreeEdge);
            });
        }

        private void dfs_DiscoverVertex(TVertex vertex)
        {
            _time++;
            _dfsChildren[vertex] = 0;
            _lowDiscoverTimes[vertex] = _time;
            _discoverTimes[vertex] = _time;
        }

        private void dfs_FinishVertex(TVertex vertex)
        {
            if (_vertexPredecessors.ContainsKey(vertex))
            {
                // not the root
                TVertex parent = _vertexPredecessors[vertex];
                _lowDiscoverTimes[parent] = Math.Min(Math.Min(_lowDiscoverTimes[vertex], _discoverTimes[parent]), _lowDiscoverTimes[parent]);
                // is my parent an articulation point?
                if (_discoverTimes[parent] <= _lowDiscoverTimes[vertex])
                {
                    if(_discoverTimes[parent] != 1) _articulationPoints.Add(parent);
                }
            }
            else
            {
                // this is the root of the DFS 
                if (_dfsChildren[vertex] > 1)
                {
                    // more than one child in the predessor tree means articulation point
                    _articulationPoints.Add(vertex);
                }
            }
        }

        private void dfs_BackEdge(object sender, UndirectedEdgeEventArgs<TVertex, TEdge> e)
        {
            //_lowDiscoverTimes[e.Source] = _discoverTimes[e.Target];
            _lowDiscoverTimes[e.Source] = Math.Min(_discoverTimes[e.Target], _lowDiscoverTimes[e.Source]);
        }

        private void dfs_TreeEdge(object sender, UndirectedEdgeEventArgs<TVertex, TEdge> e)
        {
            // count children of nodes in predessor tree
            _vertexPredecessors[e.Target] = e.Source;
            _dfsChildren[e.Source] += 1;
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(UndirectedDepthFirstSearchAlgorithm<TVertex, TEdge> value)
        {
        }
    }
}

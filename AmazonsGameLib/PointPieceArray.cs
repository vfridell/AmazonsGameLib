using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public class PointPieceArray : IDictionary<Point, Piece>
    {
        private Piece[,] _pieces;
        private int _arraySize;

        public PointPieceArray(int size)
        {
            _arraySize = size;
            _pieces = new Piece[size,size];
        }

        public Piece this[Point p] { get => _pieces[p.X, p.Y]; set => _pieces[p.X, p.Y] = value; }

        public ICollection<Point> Keys => throw new NotImplementedException();

        public ICollection<Piece> Values => throw new NotImplementedException();

        public int Count => _arraySize * _arraySize;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(Point key, Piece value) => _pieces[key.X, key.Y] = value;

        public void Add(KeyValuePair<Point, Piece> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<Point, Piece> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(Point key) => key.X >= 0 && key.X < _arraySize && key.Y >= 0 && key.Y < _arraySize;

        public void CopyTo(KeyValuePair<Point, Piece>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<Point, Piece>> GetEnumerator()
        {
            for (int x = 0; x < _arraySize; x++)
            {
                for (int y = 0; y < _arraySize; y++)
                {
                    yield return new KeyValuePair<Point, Piece>(Point.Get(x, y), _pieces[x, y]);
                }
            }
        }


        public bool Remove(Point key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<Point, Piece> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(Point key, out Piece value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            
    }
}

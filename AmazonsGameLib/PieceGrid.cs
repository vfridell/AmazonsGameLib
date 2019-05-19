using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    /// <summary>
    /// Define a grid of points with pieces. Each point will have a piece, even if that piece is an open space.
    /// The grid is a square with sides of a given size. 
    /// The square contains a set of x,y coordinate points with the lower left corner (0, 0)
    /// For convienience, we also keep track of where the Amazons for each player are in separate hashsets
    /// </summary>
    public class PieceGrid
    {
        public PieceGrid() { }

        public PieceGrid(int size)
        {
            Size = size;
            PointPiecesDict = new Dictionary<Point, Piece>();
            Amazon1Points = new HashSet<Point>();
            Amazon2Points = new HashSet<Point>();
        }

        public PieceGrid(int size, IDictionary<Point, Amazon> playerPieces)
        {
            Size = size;
            PointPiecesDict = new Dictionary<Point, Piece>();
            Amazon1Points = new HashSet<Point>();
            Amazon2Points = new HashSet<Point>();
            Initialize(playerPieces);
        }

        public void Initialize(IDictionary<Point, Piece> allPieces)
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Point p = Point.Get(x, y);
                    switch(allPieces[p].Name)
                    {
                        case PieceName.Amazon:
                            PointPiecesDict.Add(p, allPieces[p]);
                            Amazon amazon = (Amazon)allPieces[p];
                            if (amazon.Owner == Owner.Player1) Amazon1Points.Add(p);
                            else Amazon2Points.Add(p);
                            break;
                        default:
                            PointPiecesDict.Add(p, allPieces[p]);
                            break;
                    }
                }
            }
        }

        public void Initialize(IDictionary<Point, Amazon> playerPieces)
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Point p = Point.Get(x, y);
                    if (playerPieces.TryGetValue(p, out Amazon amazon))
                    {
                        PointPiecesDict.Add(p, amazon);
                        if (amazon.Owner == Owner.Player1) Amazon1Points.Add(p);
                        else Amazon2Points.Add(p);
                    }
                    else
                        PointPiecesDict.Add(p, Open.Get());
                }
            }
        }

        public readonly int Size;
        public readonly IDictionary<Point, Piece> PointPiecesDict;
        public readonly ISet<Point> Amazon1Points;
        public readonly ISet<Point> Amazon2Points;

        public ISet<Point> GetOpenPointsOutFrom(Point centerPoint)
        {
            return GetOpenPointsOutFrom(centerPoint, null);
        }

        // for the second part of the move (shooting the arrow) we ignore the amazon that moved
        // even though it may still be in that spot
        public ISet<Point> GetOpenPointsOutFrom(Point centerPoint, Point ignoreAmazonPoint)
        {
            if (IsOutOfBounds(centerPoint)) throw new ArgumentException($"Center point {centerPoint} is out of grid bounds size {Size}");

            HashSet<Point> returnSet = new HashSet<Point>();
            foreach(Point delta in centerPoint.GetAdjacentDeltas())
            {
                Point nextPoint = centerPoint + delta;
                while(!IsOutOfBounds(nextPoint) && 
                    (nextPoint.Equals(ignoreAmazonPoint) || !PointPiecesDict[nextPoint].Impassible))
                {
                    returnSet.Add(nextPoint);
                    nextPoint = nextPoint + delta;
                }
            }
            return returnSet;
        }

        public ISet<Point> GetNonArrowPointsOutFrom(Point centerPoint)
        {
            if (IsOutOfBounds(centerPoint)) throw new ArgumentException($"Center point {centerPoint} is out of grid bounds size {Size}");

            HashSet<Point> returnSet = new HashSet<Point>();
            foreach (Point delta in centerPoint.GetAdjacentDeltas())
            {
                Point nextPoint = centerPoint + delta;
                while (!IsOutOfBounds(nextPoint) && !(PointPiecesDict[nextPoint] is Arrow) )
                {
                    returnSet.Add(nextPoint);
                    nextPoint = nextPoint + delta;
                }
            }
            return returnSet;
        }

        public ISet<Move> GetMovesFromPoint(Point centerPoint)
        {
            HashSet<Move> moves = new HashSet<Move>();
            ISet<Point> amazonMovePoints = GetOpenPointsOutFrom(centerPoint);
            foreach(Point amazonPoint in amazonMovePoints)
            {
                moves.UnionWith(GetOpenPointsOutFrom(amazonPoint, centerPoint)
                                .Select(arrowPoint => new Move(centerPoint, amazonPoint, arrowPoint)));
            }
            return moves;
        }

        public void ApplyMove(Move move)
        {
            Amazon movingPiece = PointPiecesDict[move.Origin] as Amazon;
            if (movingPiece == null) throw new ArgumentException($"You can only move Amazons. Tried to move {PointPiecesDict[move.Origin].Name}");
            if(PointPiecesDict[move.AmazonsPoint].Name != PieceName.Open) throw new ArgumentException($"You can only move to an open spot. Tried to move to {PointPiecesDict[move.AmazonsPoint].Name}");
            if(PointPiecesDict[move.ArrowPoint].Name != PieceName.Open) throw new ArgumentException($"You can only shoot to an open spot. Tried to shoot to {PointPiecesDict[move.ArrowPoint].Name}");
            PointPiecesDict[move.Origin] = Open.Get();
            PointPiecesDict[move.AmazonsPoint] = movingPiece;
            PointPiecesDict[move.ArrowPoint] = movingPiece.GetArrow();
            if (movingPiece.Owner == Owner.Player1)
            {
                Amazon1Points.Remove(move.Origin);
                Amazon1Points.Add(move.AmazonsPoint);
            }
            else
            {
                Amazon2Points.Remove(move.Origin);
                Amazon2Points.Add(move.AmazonsPoint);
            }
        }

        public bool IsOutOfBounds(Point point) => point.X < 0 || point.X >= Size || point.Y < 0 || point.Y >= Size;

        public PieceGrid Clone()
        {
            PieceGrid newGrid = new PieceGrid(Size);
            newGrid.Initialize(PointPiecesDict);
            return newGrid;
        }

    }
}

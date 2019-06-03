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
        private Guid _id;
        public Guid Id { get => _id; }

        /// <summary>
        /// Create a new un-initialized PieceGrid
        /// </summary>
        public PieceGrid() { _id = Guid.NewGuid(); }

        /// <summary>
        /// Create a new un-initialized PieceGrid of a given size
        /// </summary>
        /// <param name="size">Size of the square grid</param>
        public PieceGrid(int size)
        {
            _id = Guid.NewGuid();
            Size = size;
            PointPieces = new PointPieceArray(size);
            Amazon1Points = new HashSet<Point>();
            Amazon2Points = new HashSet<Point>();
        }

        /// <summary>
        /// Create a new, empty PieceGrid of a given size and initialize it with a set of amazons
        /// </summary>
        /// <param name="size">Size of the square grid</param>
        /// <param name="playerPieces">(Point, Amazon) dictionary to set up player pieces</param>
        public PieceGrid(int size, IDictionary<Point, Amazon> playerPieces)
        {
            _id = Guid.NewGuid();
            Size = size;
            PointPieces = new PointPieceArray(size);
            Amazon1Points = new HashSet<Point>();
            Amazon2Points = new HashSet<Point>();
            Initialize(playerPieces);
        }

        /// <summary>
        /// Set up a PieceGrid with a completely defined set of points
        /// </summary>
        /// <param name="allPieces">(Point, Piece) dictionary to set up all pieces</param>
        /// <exception cref="ArgumentException">You must specify all points on the grid in the allPieces dictionary</exception>
        public void Initialize(IDictionary<Point, Piece> allPieces)
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Point p = Point.Get(x, y);
                    if (!allPieces.ContainsKey(p)) throw new ArgumentException("You must specify all points on the grid in the allPieces dictionary");
                    switch(allPieces[p].Name)
                    {
                        case PieceName.Amazon:
                            PointPieces.Add(p, allPieces[p]);
                            Amazon amazon = (Amazon)allPieces[p];
                            if (amazon.Owner == Owner.Player1) Amazon1Points.Add(p);
                            else Amazon2Points.Add(p);
                            break;
                        default:
                            PointPieces.Add(p, allPieces[p]);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Set up an empty PieceGrid with amazons at the given points
        /// </summary>
        /// <param name="playerPieces">(Point, Amazon) dictionary to set up player pieces</param>
        public void Initialize(IDictionary<Point, Amazon> playerPieces)
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    Point p = Point.Get(x, y);
                    if (playerPieces.TryGetValue(p, out Amazon amazon))
                    {
                        PointPieces.Add(p, amazon);
                        if (amazon.Owner == Owner.Player1) Amazon1Points.Add(p);
                        else Amazon2Points.Add(p);
                    }
                    else
                        PointPieces.Add(p, Open.Get());
                }
            }
        }

        /// <summary>
        /// Size of the square grid
        /// </summary>
        public readonly int Size;
        /// <summary>
        /// All points and pieces on the grid
        /// </summary>
        public readonly PointPieceArray PointPieces;
        /// <summary>
        /// Player 1 amazon positions (points)
        /// </summary>
        public readonly ISet<Point> Amazon1Points;
        /// <summary>
        /// Player 2 amazon positions (points)
        /// </summary>
        public readonly ISet<Point> Amazon2Points;

        /// <summary>
        /// Get all the contiguous points on the grid from the given center point out to the first impassible
        /// point or the edge of the grid. This corresponds to a "queen" move in chess.
        /// </summary>
        /// <param name="centerPoint">The origin point to calculate from</param>
        /// <returns>Set of available points from the center point</returns>
        public IEnumerable<Point> GetOpenPointsOutFrom(Point centerPoint)
        {
            return GetOpenPointsOutFrom(centerPoint, null);
        }

        /// <summary>
        /// Get all the contiguous points on the grid from the given center point out to the first impassible
        /// point or the edge of the grid. This corresponds to a "queen" move in chess.
        /// </summary>
        /// <param name="centerPoint">The origin point to calculate from</param>
        /// <param name="ignoreAmazonPoint">A specific point to ignore impassible status on. This is used in the 
        /// second part of the move (arrow shoot) to ignore the spot the amazon came from</param>
        /// <returns>Set of available points from the center point</returns>
        /// <exception cref="ArgumentException">Invalid centerPoint</exception>
        public IEnumerable<Point> GetOpenPointsOutFrom(Point centerPoint, Point ignoreAmazonPoint)
        {
            if (IsOutOfBounds(centerPoint)) throw new ArgumentException($"Center point {centerPoint} is out of grid bounds size {Size}");

            HashSet<Point> returnSet = new HashSet<Point>();
            foreach(Point delta in centerPoint.GetAdjacentDeltas())
            {
                Point nextPoint = centerPoint + delta;
                while(!IsOutOfBounds(nextPoint) && 
                    (nextPoint.Equals(ignoreAmazonPoint) || !PointPieces[nextPoint].Impassible))
                {
                    returnSet.Add(nextPoint);
                    nextPoint = nextPoint + delta;
                }
            }
            return returnSet;
        }


        /// <summary>
        /// Get all the available amazon moves (move + shoot) from the given point
        /// </summary>
        /// <param name="centerPoint">The point to move from</param>
        /// <returns>Set of available moves</returns>
        public IEnumerable<Move> GetMovesFromPoint(Point centerPoint)
        {
            HashSet<Move> moves = new HashSet<Move>();
            IEnumerable<Point> amazonMovePoints = GetOpenPointsOutFrom(centerPoint);
            foreach(Point amazonPoint in amazonMovePoints)
            {
                moves.UnionWith(GetOpenPointsOutFrom(amazonPoint, centerPoint)
                                .Select(arrowPoint => new Move(centerPoint, amazonPoint, arrowPoint)));
            }
            return moves;
        }

        /// <summary>
        /// Apply a move to the grid
        /// </summary>
        /// <param name="move">Move to apply</param>
        /// <exception cref="ArgumentException">Invalid move</exception>
        public void ApplyMove(Move move)
        {
            Amazon movingPiece = PointPieces[move.Origin] as Amazon;
            if (movingPiece == null) throw new ArgumentException($"You can only move Amazons. Tried to move {PointPieces[move.Origin].Name}");
            if(PointPieces[move.AmazonsPoint].Name != PieceName.Open) throw new ArgumentException($"You can only move to an open spot. Tried to move to {PointPieces[move.AmazonsPoint].Name}");
            if(move.ArrowPoint != move.Origin && PointPieces[move.ArrowPoint].Name != PieceName.Open) throw new ArgumentException($"You can only shoot to an open spot. Tried to shoot to {PointPieces[move.ArrowPoint].Name}");
            PointPieces[move.Origin] = Open.Get();
            PointPieces[move.AmazonsPoint] = movingPiece;
            PointPieces[move.ArrowPoint] = movingPiece.GetArrow();
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
            _id = Guid.NewGuid();
        }

        /// <summary>
        /// Check if a point is off the grid
        /// </summary>
        /// <param name="point">Point to check</param>
        /// <returns>True if the given point is off the grid, false otherwise</returns>
        public bool IsOutOfBounds(Point point) => point.X < 0 || point.X >= Size || point.Y < 0 || point.Y >= Size;

        /// <summary>
        /// Make a deep clone of this PieceGrid
        /// </summary>
        /// <returns>Cloned PieceGrid</returns>
        public PieceGrid Clone()
        {
            PieceGrid newGrid = new PieceGrid(Size);
            newGrid.Initialize(PointPieces);
            return newGrid;
        }

    }
}

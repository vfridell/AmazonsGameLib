using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public class Board
    {
        public PieceGrid PieceGrid { get; set; }

        public Board() { }

        public Board(int size)
        {
            PieceGrid = new PieceGrid(size, PieceHelpers.GetInitialAmazonPositions(size));
        }

        public int Size => PieceGrid.Size;
        public int Player1MoveCount { get; private set; }
        public int Player2MoveCount { get; private set; }
        public bool IsPlayer1Turn => Player1MoveCount == Player2MoveCount;
        public Owner CurrentPlayer => IsPlayer1Turn ? Owner.Player1 : Owner.Player2;

        public ISet<Move> GetAvailableMovesForCurrentPlayer() => GetAvailableMoves(CurrentPlayer);

        public ISet<Move> GetAvailableMoves(Owner owner = Owner.None)
        {
            HashSet<Move> results = new HashSet<Move>();
            IEnumerable<Point> sourceSet;
            if (owner == Owner.Player1)
                sourceSet = PieceGrid.Amazon1Points;
            else if (owner == Owner.Player2)
                sourceSet = PieceGrid.Amazon2Points;
            else
                sourceSet = PieceGrid.Amazon1Points.Union(PieceGrid.Amazon2Points);

            foreach (Point p in sourceSet)
            {
                results.UnionWith(PieceGrid.GetMovesFromPoint(p));
            }

            return results;
        }

        public void ApplyMove(Move move)
        {
            Owner owner = CurrentPlayer;

            if (PieceGrid.PointPiecesDict[move.Origin].Owner != owner)
                throw new ArgumentException($"Move given doesn't correspond to {owner} turn. Move: {move}");
            if (PieceGrid.PointPiecesDict[move.Origin].Name != PieceName.Amazon)
                throw new ArgumentException($"Move given isn't on an Amazon. You cannot move a {PieceGrid.PointPiecesDict[move.Origin].Name}. Move: {move}");

            PieceGrid.ApplyMove(move);

            if (owner == Owner.Player1) Player1MoveCount++;
            else Player2MoveCount++;
        }

        public Board Clone()
        {
            Board newBoard = new Board();
            newBoard.PieceGrid = PieceGrid.Clone();
            newBoard.Player1MoveCount = Player1MoveCount;
            newBoard.Player2MoveCount = Player2MoveCount;
            return newBoard;
        }
    }
}

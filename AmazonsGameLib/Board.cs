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

        /// <summary>
        /// return false if the board represents a completed game and there is a winner, true otherwise
        /// </summary>
        public bool IsPlayable => GetAvailableMovesForCurrentPlayer().Any();

        private Dictionary<Owner, IList<Move>> _moves { get; set; } = new Dictionary<Owner, IList<Move>>();

        public IEnumerable<Move> GetAvailableMovesForCurrentPlayer() => GetAvailableMoves(CurrentPlayer);

        public IEnumerable<Move> GetAvailableMoves(Owner owner = Owner.None)
        {
            bool cached = false;
            if (owner == Owner.None) cached = _moves.ContainsKey(Owner.Player1) && _moves.ContainsKey(Owner.Player2);
            else cached = _moves.ContainsKey(owner);

            if (!cached)
            {
                IEnumerable<Point> sourceSet;
                if (owner == Owner.Player1)
                {
                    List<Move> results = new List<Move>();
                    foreach (Point p in PieceGrid.Amazon1Points)
                    {
                        results.AddRange(PieceGrid.GetMovesFromPoint(p));
                    }
                    _moves[owner] = results;
                }
                else if (owner == Owner.Player2)
                {
                    List<Move> results = new List<Move>();
                    foreach (Point p in PieceGrid.Amazon2Points)
                    {
                        results.AddRange(PieceGrid.GetMovesFromPoint(p));
                    }
                    _moves[owner] = results;
                }
                else
                {
                    List<Move> results1 = new List<Move>();
                    List<Move> results2 = new List<Move>();
                    sourceSet = PieceGrid.Amazon1Points.Union(PieceGrid.Amazon2Points);
                    foreach (Point p in PieceGrid.Amazon1Points)
                    {
                        results1.AddRange(PieceGrid.GetMovesFromPoint(p));
                    }
                    _moves[Owner.Player1] = results1;
                    foreach (Point p in PieceGrid.Amazon2Points)
                    {
                        results2.AddRange(PieceGrid.GetMovesFromPoint(p));
                    }
                    _moves[Owner.Player2] = results2;
                }
            }

            if (owner == Owner.None) return _moves[Owner.Player1].Union(_moves[Owner.Player2]);
            else return _moves[owner];
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

        public static Board ComputeFutureBoard(Board board, Move move)
        {
            Board newBoard = board.Clone();
            newBoard.ApplyMove(move);
            return newBoard;
        }
    }
}

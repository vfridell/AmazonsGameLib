using Newtonsoft.Json;
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

        public Board(PieceGrid grid)
        {
            PieceGrid = grid;
            Player1MoveCount = grid.PointPieces.Count(kvp => kvp.Value is ArrowPlayer1);
            Player2MoveCount = grid.PointPieces.Count(kvp => kvp.Value is ArrowPlayer2);
        }

        public int Size => PieceGrid.Size;
        [JsonProperty]
        public int Player1MoveCount { get; private set; }
        [JsonProperty]
        public int Player2MoveCount { get; private set; }
        public bool IsPlayer1Turn => Player1MoveCount == Player2MoveCount;
        public Owner CurrentPlayer => IsPlayer1Turn ? Owner.Player1 : Owner.Player2;
        public Owner PreviousPlayer => IsPlayer1Turn ? Owner.Player2 : Owner.Player1;

        /// <summary>
        /// return false if the board represents a completed game and there is a winner, true otherwise
        /// </summary>
        public bool IsPlayable => GetAvailableMovesForCurrentPlayer().Any();

        [JsonIgnore]
        private Dictionary<Owner, IList<Move>> _moves { get; set; } = new Dictionary<Owner, IList<Move>>();

        public IEnumerable<Move> GetAvailableMovesForCurrentPlayer() => GetAvailableMoves(CurrentPlayer);
        public IEnumerable<Move> GetAvailableReverseMovesForPreviousPlayer() => GetAvailableMoves(PreviousPlayer, true);

        public IEnumerable<Move> GetAvailableMoves(Owner owner = Owner.None, bool reverse = false)
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
                        if (reverse)
                            results.AddRange(PieceGrid.GetReverseMovesFromPoint(p, owner));
                        else
                            results.AddRange(PieceGrid.GetMovesFromPoint(p));
                    }
                    _moves[owner] = results;
                }
                else if (owner == Owner.Player2)
                {
                    List<Move> results = new List<Move>();
                    foreach (Point p in PieceGrid.Amazon2Points)
                    {
                        if (reverse)
                            results.AddRange(PieceGrid.GetReverseMovesFromPoint(p, owner));
                        else
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
                        if (reverse)
                            results1.AddRange(PieceGrid.GetReverseMovesFromPoint(p, Owner.Player1));
                        else
                            results1.AddRange(PieceGrid.GetMovesFromPoint(p));
                    }
                    _moves[Owner.Player1] = results1;
                    foreach (Point p in PieceGrid.Amazon2Points)
                    {
                        if (reverse)
                            results2.AddRange(PieceGrid.GetReverseMovesFromPoint(p, Owner.Player2));
                        else
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

            if (PieceGrid.PointPieces[move.Origin].Owner != owner)
                throw new ArgumentException($"Move given doesn't correspond to {owner} turn. Move: {move}");
            if (PieceGrid.PointPieces[move.Origin].Name != PieceName.Amazon)
                throw new ArgumentException($"Move given isn't on an Amazon. You cannot move a {PieceGrid.PointPieces[move.Origin].Name}. Move: {move}");

            PieceGrid.ApplyMove(move);

            if (owner == Owner.Player1) Player1MoveCount++;
            else Player2MoveCount++;

            _moves.Clear();
        }

        public void ApplyReverseMove(Move move)
        {
            Owner owner = PreviousPlayer;

            if (PieceGrid.PointPieces[move.AmazonsPoint].Owner != owner)
                throw new ArgumentException($"Reverse move given doesn't correspond to {owner} previous turn. Move: {move}");
            if (PieceGrid.PointPieces[move.AmazonsPoint].Name != PieceName.Amazon)
                throw new ArgumentException($"Reverse move given isn't on an Amazon. You cannot move a {PieceGrid.PointPieces[move.Origin].Name}. Move: {move}");

            PieceGrid.ApplyReverseMove(move);

            if (owner == Owner.Player1) Player1MoveCount--;
            else Player2MoveCount--;

            _moves.Clear();
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

        public static Board ComputePreviousBoard(Board board, Move move)
        {
            Board newBoard = board.Clone();
            newBoard.ApplyReverseMove(move);
            return newBoard;
        }
    }
}

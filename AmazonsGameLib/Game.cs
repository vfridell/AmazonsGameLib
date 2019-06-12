using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public enum GameResult { Incomplete, Player1Won, Player2Won };

    public class Game
    {

        public List<Board> Boards { get; set; } = new List<Board>();
        [JsonIgnore]
        public List<Move> CurrentMoves { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        [JsonIgnore]
        public Board CurrentBoard => Boards.Last();
        public int BoardSize => CurrentBoard.Size;
        public Owner CurrentPlayer => CurrentBoard.CurrentPlayer;
        public GameResult GetGameResult()
        {
            if (!IsComplete()) return GameResult.Incomplete;
            if (CurrentPlayer == Owner.Player1) return GameResult.Player2Won;
            else return GameResult.Player1Won;
        }


        public void Begin(Player player1, Player player2, int boardSize)
        {
            Player1 = player1;
            Player2 = player2;
            Board board = new Board(boardSize);
            Boards.Add(board);
            CurrentMoves = CurrentBoard.GetAvailableMovesForCurrentPlayer().ToList();
        }

        public bool IsComplete() => !CurrentMoves.Any();

        public void ApplyMove(Move move)
        {
            Boards.Add(CurrentBoard.Clone());
            CurrentBoard.ApplyMove(move);
            CurrentMoves = CurrentBoard.GetAvailableMovesForCurrentPlayer().ToList();
        }

        public void UndoLastMove()
        {
            Boards.Remove(CurrentBoard);
            CurrentMoves = CurrentBoard.GetAvailableMovesForCurrentPlayer().ToList();
        }

        public void ApplyReverseMove(Move move)
        {
            Boards.Add(CurrentBoard.Clone());
            CurrentBoard.ApplyReverseMove(move);
            CurrentMoves = CurrentBoard.GetAvailableMovesForCurrentPlayer().ToList();
        }
    }
}

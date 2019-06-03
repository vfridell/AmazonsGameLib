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
        public List<Move> CurrentMoves { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Board CurrentBoard { get; set; }
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
            CurrentBoard = new Board(boardSize);
            Boards.Add(CurrentBoard);
            CurrentMoves = CurrentBoard.GetAvailableMovesForCurrentPlayer().ToList();
        }

        public bool IsComplete() => !CurrentMoves.Any();

        public void ApplyMove(Move move)
        {
            CurrentBoard = CurrentBoard.Clone();
            CurrentBoard.ApplyMove(move);
            Boards.Add(CurrentBoard);
            CurrentMoves = CurrentBoard.GetAvailableMovesForCurrentPlayer().ToList();
        }
    }
}

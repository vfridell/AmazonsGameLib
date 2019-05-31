namespace AmazonsGameLib
{
    public class NegamaxContext
    {
        public NegamaxContext(Move move, Board board, double score, bool calculated)
        {
            Move = move;
            Board = board;
            Score = score;
            ScoreCalculated = calculated;
        }

        public Move Move { get; set; }
        public Board Board { get; set; }
        public double Score { get; set; }
        public bool ScoreCalculated { get; set; }
    }
}

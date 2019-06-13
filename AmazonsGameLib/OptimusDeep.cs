using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public class OptimusDeep : Player
    {
        public Owner PlayingAs { get; set; }
        private int _depth;
        private IBoardAnalyzer _analyzer;
        public readonly double MinValue = double.MinValue;
        public readonly double MaxValue = double.MaxValue;
        public Move BestMove { get; set; }

        public OptimusDeep(int depth, IBoardAnalyzer boardAnalyzer)
        {
            _analyzer = boardAnalyzer;
            _depth = depth;
        }

        public OptimusDeep(int depth, IBoardAnalyzer boardAnalyzer, string name)
            : this(depth, boardAnalyzer)
        {
            _name = name;
        }

        public bool isPlayer1 { get { return PlayingAs == Owner.Player1; } }

        public Move MakeBestMove(Game game)
        {
            Board board = game.CurrentBoard;
            Move move;
            if (PlayingAs == board.CurrentPlayer)
            {
                move = PickBestMove(board);
                game.ApplyMove(move);
            }
            else
            {
                throw new Exception("It is not my move :(");
            }
            return move;
        }


        public Move PickBestMove(Board board)
        {
            var cancelSource = new CancellationTokenSource();
            var initialContext = new NegamaxContext(null, board, 0, false);
            Move bestMove = NegamaxRoot(initialContext, MinValue, MaxValue, _depth, isPlayer1 ? 1 : -1, cancelSource.Token).Result;
            return bestMove;
        }

        public async Task<Move> PickBestMoveAsync(Board board, CancellationToken aiCancelToken)
        {
            var initialContext = new NegamaxContext(null, board, 0, false);
            Move bestMove = await NegamaxRoot(initialContext, MinValue, MaxValue, _depth, isPlayer1 ? 1 : -1, aiCancelToken);
            return bestMove;
        }

        public void BeginNewGame(Owner playingAs, int boardSize)
        {
            PlayingAs = playingAs;
        }

        private async Task<Move> NegamaxRoot(NegamaxContext context, double alpha, double beta, int depth, int color, CancellationToken aiCancelToken)
        {
            IEnumerable<NegamaxContext> orderedAnalysis = GetSortedMoves(context.Board, color, aiCancelToken);
            double bestScore = MinValue;
            Move localBestMove = orderedAnalysis.First().Move;

            foreach (var nextContext in orderedAnalysis)
            {
                if (aiCancelToken.IsCancellationRequested) return localBestMove;
                double score = -(await Negamax(nextContext, -beta, -alpha, depth - 1, -color, aiCancelToken));
                if (score > bestScore) localBestMove = nextContext.Move;
                bestScore = Math.Max(bestScore, score);
                alpha = Math.Max(alpha, score);
                if (alpha >= beta)
                    break;
            };

            return localBestMove;
        }

        private async Task<double> Negamax(NegamaxContext context, double alpha, double beta, int depth, int color, CancellationToken aiCancelToken)
        {
            if (depth == 0 || !context.Board.IsPlayable)
            {
                if(context.ScoreCalculated) return context.Score * color;
                else
                {
                    return _analyzer.Analyze(context.Board).player1Advantage * color;
                }
            }

            IEnumerable<NegamaxContext> orderedAnalysis = GetSortedMoves(context.Board, color, aiCancelToken);
            double bestScore = MinValue;

            foreach (var nextContext in orderedAnalysis)
            {
                double score = -(await Negamax(nextContext, -beta, -alpha, depth - 1, -color, aiCancelToken));
                bestScore = Math.Max(bestScore, score);
                alpha = Math.Max(alpha, score);
                if (alpha >= beta)
                    break;

                if (aiCancelToken.IsCancellationRequested) return bestScore;
            };

            return bestScore;
        }

        private IEnumerable<NegamaxContext> GetMoves(Board board, int color, CancellationToken aiCancelToken)
        {
            var advantageDict = new SortedDictionary<double, HashSet<NegamaxContext>>();
            var allMoves = board.GetAvailableMovesForCurrentPlayer();
            foreach (Move move in allMoves)
            {
                var futureBoard = Board.ComputeFutureBoard(board, move);
                yield return new NegamaxContext(move, futureBoard, 0, false);
            }
        }

        private IEnumerable<NegamaxContext> GetSortedMoves(Board board, int color, CancellationToken aiCancelToken)
        {
            /*
            var advantageDict = new SortedDictionary<double, HashSet<NegamaxContext>>();
            var allMoves = board.GetAvailableMovesForCurrentPlayer();
            foreach (Move move in allMoves)
            {
                if (aiCancelToken.IsCancellationRequested) yield break;

                var futureBoard = Board.ComputeFutureBoard(board, move);
                double currentAdvantage = _analyzer.Analyze(futureBoard).player1Advantage;
                if (!advantageDict.ContainsKey(currentAdvantage))
                    advantageDict.Add(currentAdvantage, new HashSet<NegamaxContext>());
                advantageDict[currentAdvantage].Add(new NegamaxContext(move, futureBoard, currentAdvantage, true));
            }
            foreach (var kvp in color == 1 ? advantageDict.Reverse() : advantageDict)
            {
                foreach (var pair in kvp.Value)
                {
                    if (aiCancelToken.IsCancellationRequested) yield break;
                    else yield return pair;
                }
            }
           */

            var advantageBag = new ConcurrentBag<NegamaxContext>();
            var allMoves = board.GetAvailableMovesForCurrentPlayer();
            Parallel.ForEach(allMoves, (move, parallelLoopState) =>
            {
                if (aiCancelToken.IsCancellationRequested) parallelLoopState.Break();
                if (parallelLoopState.ShouldExitCurrentIteration) return;
                var futureBoard = Board.ComputeFutureBoard(board, move);
                double currentAdvantage = _analyzer.Analyze(futureBoard).player1Advantage;
                advantageBag.Add(new NegamaxContext(move, futureBoard, currentAdvantage, true));
            });

            IEnumerable<NegamaxContext> sortedContexts = color == 1 ? advantageBag.ToList().OrderByDescending(c => c.Score) : advantageBag.ToList().OrderBy(c => c.Score);

            foreach (var context in sortedContexts)
            {
                yield return context;
            }
        }



        private string _name;
        public override string Name
        {
            get { return string.IsNullOrEmpty(_name) ? string.Format("Optimus{0}Deep", _depth) : _name; }
        }
    }
}

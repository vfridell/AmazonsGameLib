using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AmazonsGameLib
{
    public class TacoCat : Player
    {
        public Owner PlayingAs { get; set; }
        private IBoardAnalyzer _analyzer;
        public readonly double MinValue = double.MinValue;
        public readonly double MaxValue = double.MaxValue;
        public Move BestMove { get; set; }

        public TacoCat(IBoardAnalyzer boardAnalyzer)
        {
            _analyzer = boardAnalyzer;
        }

        public TacoCat(IBoardAnalyzer boardAnalyzer, string name)
            : this(boardAnalyzer)
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
            Move bestMove = NegamaxRoot(initialContext, MinValue, MaxValue, 3, isPlayer1 ? 1 : -1, cancelSource.Token).Result;
            return bestMove;
        }

        public async Task<Move> PickBestMoveAsync(Board board, CancellationToken aiCancelToken)
        {
            // calc all the possible next boards
            List<Board> nextBoards = new List<Board>();
            foreach(var move in board.GetAvailableMovesForCurrentPlayer())
            {
                nextBoards.Add(Board.ComputeFutureBoard(board, move));
            }

            var initialContext = new NegamaxContext(null, board, 0, false);
            Move bestMove = await NegamaxRoot(initialContext, MinValue, MaxValue, 3, isPlayer1 ? 1 : -1, aiCancelToken);
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

        public Board ImagineWinningBoard(Board board)
        {
            AnalysisGraph analysis = new AnalysisGraph();
            analysis.BuildAnalysis(board.PieceGrid, board.CurrentPlayer);
            if (analysis.W == 0) return null;

            Random rnd = new Random();
            List<Point> twoByTwos = board.PieceGrid.GetEmpyTwoByTwos().ToList();
            if (twoByTwos.Count < 8) return null;

            twoByTwos.Shuffle(rnd);
            List<Point> pockets = new List<Point>();
            List<Point> fill = new List<Point>();
            while (pockets.Count < 8)
            {
                foreach (Point p in twoByTwos)
                {
                    pockets.Add(p);
                    for (int x = -1; x < 3; x++)
                    {
                        for (int y = -1; y < 3; y++)
                        {
                            Point removePoint = p + Point.Get(x, y);
                            if (!board.PieceGrid.IsOutOfBounds(removePoint))
                                twoByTwos.Remove(removePoint);
                            if (x == -1 || y == -1 || x == 2 || y == 2) fill.Add(removePoint);
                        }
                    }
                    break;
                }
            }

            Board winningBoard = board.Clone();
            Owner owner = PlayingAs;

            foreach(Point p in winningBoard.PieceGrid.Amazon1Points.Union(winningBoard.PieceGrid.Amazon2Points))
            {
                winningBoard.PieceGrid.PointPieces[p] = Piece.Get(PieceName.Open);
            }
            winningBoard.PieceGrid.Amazon1Points.Clear();
            winningBoard.PieceGrid.Amazon2Points.Clear();

            foreach (Point p in pockets)
            {
                List<int> a = new List<int> { rnd.Next(), rnd.Next(), rnd.Next(), rnd.Next() };
                int index = a.IndexOf(a.Max());

                switch (index)
                {
                    case 0:
                        winningBoard.PieceGrid.PointPieces[p] = Piece.Get(PieceName.Amazon, owner);
                        if (owner == Owner.Player1) winningBoard.PieceGrid.Amazon1Points.Add(p);
                        else winningBoard.PieceGrid.Amazon2Points.Add(p);
                        break;
                    case 1:
                        winningBoard.PieceGrid.PointPieces[p + Point.Get(1, 0)] = Piece.Get(PieceName.Amazon, owner);
                        if (owner == Owner.Player1) winningBoard.PieceGrid.Amazon1Points.Add(p + Point.Get(1, 0));
                        else winningBoard.PieceGrid.Amazon2Points.Add(p + Point.Get(1, 0));
                        break;
                    case 2:
                        winningBoard.PieceGrid.PointPieces[p + Point.Get(0, 1)] = Piece.Get(PieceName.Amazon, owner);
                        if (owner == Owner.Player1) winningBoard.PieceGrid.Amazon1Points.Add(p + Point.Get(0, 1));
                        else winningBoard.PieceGrid.Amazon2Points.Add(p + Point.Get(0, 1));
                        break;
                    case 3:
                        winningBoard.PieceGrid.PointPieces[p + Point.Get(1, 1)] = Piece.Get(PieceName.Amazon, owner);
                        if (owner == Owner.Player1) winningBoard.PieceGrid.Amazon1Points.Add(p + Point.Get(1, 1));
                        else winningBoard.PieceGrid.Amazon2Points.Add(p + Point.Get(1, 1));
                        break;
                }

                owner = owner == Owner.Player1 ? Owner.Player2 : Owner.Player1;
            }
            

            Owner arrowOwner = winningBoard.CurrentPlayer;
            fill.Shuffle(rnd);
            foreach(Point p in fill)
            {
                if (winningBoard.PieceGrid.IsOutOfBounds(p) || winningBoard.PieceGrid.PointPieces[p].Impassible) continue;
                winningBoard.PieceGrid.PointPieces[p] = Piece.Get(PieceName.Arrow, arrowOwner);
                arrowOwner = arrowOwner == Owner.Player1 ? Owner.Player2 : Owner.Player1;
            }

            analysis.BuildAnalysis(winningBoard.PieceGrid, winningBoard.CurrentPlayer);
            IEnumerable<Point> unreachablePoints = analysis.LocalAdvantages.Where(la => la.Value != null && !la.Value.Player1Reachable && !la.Value.Player2Reachable).Select(la => la.Key);
            IEnumerable<Point> player1Points = analysis.LocalAdvantages.Where(la => la.Value != null && la.Value.Player1Reachable && !la.Value.Player2Reachable).Select(la => la.Key);
            IEnumerable<Point> player2Points = analysis.LocalAdvantages.Where(la => la.Value != null && !la.Value.Player1Reachable && la.Value.Player2Reachable).Select(la => la.Key);

            if (analysis.W != 0) throw new Exception("Not in the filling phase!");
            double player1Advantage = analysis.T + analysis.M;
            if(player1Advantage >= 0 && PlayingAs == Owner.Player2)
            {
                // add in some player1 arrows
                
                for (int i=0; i < Math.Abs(player1Advantage) + 1; i++)
                {
                    winningBoard.PieceGrid.PointPieces[player1Points.Skip(i).First()] = Piece.Get(PieceName.Arrow, Owner.Player1);
                    winningBoard.PieceGrid.PointPieces[unreachablePoints.Skip(i).First()] = Piece.Get(PieceName.Arrow, Owner.Player2);
                }
            }
            else if(player1Advantage <= 0 && PlayingAs == Owner.Player1)
            {
                // add in some player2 arrows
                for (int i = 0; i < Math.Abs(player1Advantage) + 1; i++)
                {
                    winningBoard.PieceGrid.PointPieces[player2Points.Skip(i).First()] = Piece.Get(PieceName.Arrow, Owner.Player2);
                    winningBoard.PieceGrid.PointPieces[unreachablePoints.Skip(i).First()] = Piece.Get(PieceName.Arrow, Owner.Player1);
                }
            }

            return new Board(winningBoard.PieceGrid);
        }

        private string _name;
        public override string Name
        {
            get { return string.IsNullOrEmpty(_name) ? "TacoCat" : _name; }
        }
    }
}

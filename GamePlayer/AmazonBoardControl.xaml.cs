using AmazonsGameLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Point = AmazonsGameLib.Point;

namespace GamePlayer
{
    /// <summary>
    /// Interaction logic for AmazonBoardControl.xaml
    /// </summary>
    public partial class AmazonBoardControl : UserControl
    {
        public Board Board { get; set; }

        public AmazonBoardControl(Board board)
        {
            InitializeComponent();
            Board = board;
            DrawBoard();

        }

        private void DrawBoard()
        {
            foreach (var kvp in Board.PieceGrid.PointPieces)
            {
                (int row, int column) = GetRowColumn(Board.Size, kvp.Key);
                Rectangle rectangle = new Rectangle();
                if ((kvp.Key.X + kvp.Key.Y) % 2 == 0)
                    rectangle.Fill = Brushes.SlateGray;
                else
                    rectangle.Fill = Brushes.WhiteSmoke;
                Grid.SetRow(rectangle, row);
                Grid.SetColumn(rectangle, column);
                BoardGrid.Children.Add(rectangle);

                AmazonPieceControl amazonPieceControl = new AmazonPieceControl(kvp.Value.Name, kvp.Value.Owner);
                Grid.SetRow(amazonPieceControl, row);
                Grid.SetColumn(amazonPieceControl, column);
                BoardGrid.Children.Add(amazonPieceControl);

                if (kvp.Value.Name == PieceName.Amazon && kvp.Value.Owner == Board.CurrentPlayer)
                {
                    amazonPieceControl.MouseDown += (sender, e) => { InitiateMove(kvp.Key); };
                }
                else if (kvp.Value.Name == PieceName.Open)
                {
                    rectangle.MouseDown += (sender, e) => { ClearInitialMoves(); };
                }
            }
        }

        private void ClearInitialMoves()
        {
            foreach(var element in BoardGrid.Children.OfType<Ellipse>().ToList())
            {
                BoardGrid.Children.Remove(element);
            }
            BoardGrid.UpdateLayout();
        }

        private Point _originMovePoint;
        private Point _amazonMovePoint;

        private void InitiateMove(Point p)
        {
            _originMovePoint = p;
            _amazonMovePoint = null;
            IEnumerable<Point> points = Board.GetAvailableMovesForCurrentPlayer().Where(m => m.Origin.Equals(p)).Select(m => m.AmazonsPoint).Distinct();
            HighlightCells(points, false);
        }

        private void MovePart1(Point p)
        {
            _amazonMovePoint = p;
            IEnumerable<Point> points = Board.GetAvailableMovesForCurrentPlayer().Where(m => m.AmazonsPoint.Equals(p)).Select(m => m.ArrowPoint).Distinct();
            HighlightCells(points, true);
        }

        private void MovePart2(Point p)
        {
            Move move = new Move(_originMovePoint, _amazonMovePoint, p);
            if (!Board.GetAvailableMovesForCurrentPlayer().Contains(move)) throw new Exception($"Invalid move: {move}");
            Board.ApplyMove(move);
            BoardGrid.Children.Clear();
            BoardGrid.UpdateLayout();
            DrawBoard();
            BoardGrid.UpdateLayout();
        }

        private void HighlightCells(IEnumerable<Point> points, bool part2)
        {
            ClearInitialMoves();
            foreach(Point p in points)
            {
                (int row, int column) = GetRowColumn(Board.Size, p);
                System.Windows.Shapes.Ellipse ellipse = new Ellipse();
                ellipse.Fill = Brushes.Coral;
                Grid.SetRow(ellipse, row);
                Grid.SetColumn(ellipse, column);
                BoardGrid.Children.Add(ellipse);
                if(part2)
                    ellipse.MouseDown += (sender, e) => { MovePart2(p); };
                else
                    ellipse.MouseDown += (sender, e) => { MovePart1(p); };
            }
            BoardGrid.UpdateLayout();
        }

        private (int, int) GetRowColumn(int size, Point p) => (size - (p.Y + 1), p.X);
    }
}

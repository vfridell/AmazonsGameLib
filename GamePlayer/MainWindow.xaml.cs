using AmazonsGameLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand ImagineCommand = new RoutedCommand();

        public Game Game { get; set; }
        public Stack<(Move, bool)> MoveHistoryStack = new Stack<(Move, bool)>();
        public Stack<(Move, bool)> MoveUndoHistoryStack = new Stack<(Move, bool)>();
        Owner ComputerPlaying = AmazonsGameLib.Owner.None;

        public AmazonBoardControl BoardControl { get; set; }

        public MainWindow()
        {

        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MainGrid.Children.Clear();
            Game = new Game();
            Game.Begin(null, null, 10);
            MoveHistoryStack.Clear();
            BoardControl = new AmazonBoardControl(Game.CurrentBoard.Clone(), false);
            BoardControl.MoveUpdated += BoardControl_MoveUpdated;
            MainGrid.Children.Add(BoardControl);
            MainGrid.UpdateLayout();
        }

        private void BoardControl_MoveUpdated(Move move, bool reverse)
        {
            MoveHistoryStack.Push((move, reverse));
            if (reverse)
                Game.ApplyReverseMove(move);
            else
                Game.ApplyMove(move);
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Game != null && BoardControl != null;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "json";
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "json|*.json";
            saveFileDialog.Title = "Save Game";
            if (saveFileDialog.ShowDialog(this).Value)
            {
                string json = Serializer.Serialize(Game);
                using (StreamWriter writer = new StreamWriter(File.Open(saveFileDialog.FileName, FileMode.Create)))
                {
                    writer.Write(json);
                }
            }
        }

        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "json";
            openFileDialog.Title = "Open Game";
            if (openFileDialog.ShowDialog(this).Value)
            {
                string json = "";
                using (StreamReader reader = new StreamReader(File.Open(openFileDialog.FileName, FileMode.Open)))
                {
                    json = reader.ReadToEnd();
                }
                Game openGame = Serializer.Deserialize<Game>(json);
                if (openGame == null)
                {
                    MessageBox.Show($"Error creating game from file {openFileDialog.FileName}");
                    return;
                }
                Game = openGame;
                BoardControl = new AmazonBoardControl(Game.CurrentBoard.Clone(), false);
                BoardControl.MoveUpdated += BoardControl_MoveUpdated;
                MainGrid.Children.Clear();
                MainGrid.Children.Add(BoardControl);
                MainGrid.UpdateLayout();
            }
        }

        private void UndoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = MoveHistoryStack.Count > 0;
        }

        private void UndoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            (Move move, bool reverse) = MoveHistoryStack.Pop();
            MoveUndoHistoryStack.Push((move, reverse));
            Game.UndoLastMove();
            BoardControl.SetBoard(Game.CurrentBoard.Clone());
        }

        private void RedoCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = MoveUndoHistoryStack.Count > 0;
        }

        private void RedoCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            (Move move, bool reverse) = MoveUndoHistoryStack.Pop();
            MoveHistoryStack.Push((move, reverse));
            if (reverse) Game.ApplyReverseMove(move);
            else Game.ApplyMove(move);
            BoardControl.SetBoard(Game.CurrentBoard);
        }

        private void PlayCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Game != null && BoardControl != null && !Game.IsComplete();
        }

        private void PlayCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AnalysisGraph analysisGraph = new AnalysisGraph();
            var optimusDeep = new OptimusDeep(3, analysisGraph);
            optimusDeep.BeginNewGame(Game.CurrentPlayer, 10);
            ComputerPlaying = Game.CurrentPlayer;

            Cursor orgCursor = Cursor;
            Cursor = Cursors.Wait;
            BoardControl.SetReadOnlyTillNextDraw();
            Task.Run(() =>
            {
                var cancellationTokenSrc = new CancellationTokenSource(9000);
                var bestMoveTask = optimusDeep.PickBestMoveAsync(Game.CurrentBoard, cancellationTokenSrc.Token);
                return bestMoveTask.Result;
            }).ContinueWith((t) =>
            {
                Move move = t.Result;
                Dispatcher.Invoke(() =>
                {
                    BoardControl.ApplyMove(move, false);
                    Cursor = orgCursor;
                });

            });
        }

        private void ImagineCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Game != null && BoardControl != null;
        }

        private void ImagineCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Cursor orgCursor = Cursor;
            Cursor = Cursors.Wait;
            BoardControl.SetReadOnlyTillNextDraw();

            try
            {
                AnalysisGraph analysisGraph = new AnalysisGraph();
                TacoCat tacoCat = new TacoCat(analysisGraph);
                tacoCat.BeginNewGame(Game.CurrentPlayer, Game.BoardSize);
                Board winningBoard = tacoCat.ImagineWinningBoard(Game.CurrentBoard);
                if (winningBoard == null) return;

                MainGrid.Children.Clear();
                Game = new Game();
                Game.Begin(null, null, winningBoard.Clone());
                MoveHistoryStack.Clear();
                BoardControl = new AmazonBoardControl(winningBoard.Clone(), false);
                BoardControl.MoveUpdated += BoardControl_MoveUpdated;
                MainGrid.Children.Add(BoardControl);
                MainGrid.UpdateLayout();
            }
            finally
            {
                Cursor = orgCursor;
            }
        }
    }
}

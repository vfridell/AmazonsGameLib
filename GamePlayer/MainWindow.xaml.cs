using AmazonsGameLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Game Game { get; set; }

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
            AmazonBoardControl boardControl = new AmazonBoardControl(Game.CurrentBoard.Clone(), false);
            boardControl.MoveUpdated += BoardControl_MoveUpdated;
            MainGrid.Children.Add(boardControl);
            MainGrid.UpdateLayout();
        }

        private void BoardControl_MoveUpdated(Move move, bool reverse)
        {
            if (reverse)
                Game.ApplyReverseMove(move);
            else
                Game.ApplyMove(move);
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Game != null && MainGrid.Children.OfType<AmazonBoardControl>().Any();
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
                AmazonBoardControl boardControl = new AmazonBoardControl(Game.CurrentBoard.Clone(), false);
                boardControl.MoveUpdated += BoardControl_MoveUpdated;
                MainGrid.Children.Add(boardControl);
                MainGrid.UpdateLayout();
            }
        }
    }
}

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
            Game game = new Game();
            game.Begin(null, null, 10);
            AmazonBoardControl boardControl = new AmazonBoardControl(game.CurrentBoard, false);
            MainGrid.Children.Add(boardControl);
            MainGrid.UpdateLayout();
        }
    }
}

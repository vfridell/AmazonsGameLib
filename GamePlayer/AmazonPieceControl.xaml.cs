using AmazonsGameLib;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using Image = System.Windows.Controls.Image;

namespace GamePlayer
{
    /// <summary>
    /// Interaction logic for AmazonPieceControl.xaml
    /// </summary>
    public partial class AmazonPieceControl : UserControl
    {
        public Piece Piece { get; set; }
        Image PieceImage { get; set; }

        public AmazonPieceControl()
        {
            InitializeComponent();
            Piece = Piece.Get(PieceName.Open);
        }

        public AmazonPieceControl(PieceName pieceName, Owner owner)
        {
            InitializeComponent();
            Piece = Piece.Get(pieceName, owner);
            PieceImage = PieceToImage(Piece);
            if(PieceImage != null)
            {
                Cell.Children.Add(PieceImage);
            }
        }

        public static Image PieceToImage(Piece piece)
        {
            string imagePath;
            switch (piece.Name)
            {
                case PieceName.Amazon:
                    if (piece.Owner == Owner.Player1) imagePath = "Player1Amazon.png";
                    else imagePath = "Player2Amazon.png";
                    break;
                case PieceName.Arrow:
                    if (piece.Owner == Owner.Player1) imagePath = "Arrow1.png";
                    else imagePath = "Arrow2.png";
                    break;
                default:
                    return null;
            }
            var bitmap = new Bitmap(imagePath);
            Image image = new Image();
            image.Source = BitmapToImageSource(bitmap);
            return image;
        }

        public static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}

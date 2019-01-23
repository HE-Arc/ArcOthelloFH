using Othello.Data;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Othello.UI
{
    /// <summary>
    /// Logique d'interaction pour PlayerUI.xaml
    /// </summary>
    public partial class PlayerUI : UserControl
    {
        private static readonly BitmapImage WHITE_BACKGROUND = null;
        private static readonly BitmapImage BLACK_BACKGROUND = null;

        static PlayerUI()
        {
            Uri geraltAvatar = new Uri("pack://application:,,,/Othello;component/Resources/Geralt_Avatar.png", UriKind.Absolute);
            Uri yenAvatar = new Uri("pack://application:,,,/Othello;component/Resources/Yen_Avatar.png", UriKind.Absolute);

            WHITE_BACKGROUND = new BitmapImage(geraltAvatar);
            BLACK_BACKGROUND = new BitmapImage(yenAvatar);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PlayerUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Change style: Player.Black -> Yennefer, Player.White -> Geralt
        /// </summary>
        /// <param name="player">Player color: Player.Black -> Yennefer, Player.White -> Geralt</param>
        public void ChangeStyle(Player player)
        {
            ImageBrush imageBrush =  new ImageBrush();
            Brush backgroundBrush = null;

            switch (player)
            {
                case Player.White:
                    imageBrush.ImageSource = WHITE_BACKGROUND;
                    imageBrush.Stretch = Stretch.Uniform;
                    backgroundBrush = Brushes.White;
                    lbl_txt_score.Foreground = Brushes.Black;
                    lbl_txt_time.Foreground = Brushes.Black;
                    lbl_score.Foreground = Brushes.Black;
                    lbl_time.Foreground = Brushes.Black;
                    break;
                case Player.Black:
                    imageBrush.ImageSource = BLACK_BACKGROUND;
                    imageBrush.Stretch = Stretch.Uniform;
                    backgroundBrush = Brushes.Black;
                    lbl_txt_score.Foreground = Brushes.White;
                    lbl_txt_time.Foreground = Brushes.White;
                    lbl_score.Foreground = Brushes.White;
                    lbl_time.Foreground = Brushes.White;
                    break;
            }

            imageBrush.Stretch = Stretch.Uniform;
            imageBrush.AlignmentY = AlignmentY.Bottom;

            this.Background = backgroundBrush;
            grid_player.Background = imageBrush;
        }
    }
}

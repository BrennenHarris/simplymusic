using simplymusic.AppDate;
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
using System.Windows.Threading;

namespace simplymusic
{
    /// <summary>
    /// Interaction logic for visualizerPage.xaml
    /// </summary>
    public partial class visualizerPage : Page
    {
        DispatcherTimer timer;
        public visualizerPage()
        {
            InitializeComponent();
            startTimer();

         
        }

        private void startTimer()
        {
            try
            {


                if (SongHandler.getAlbumCover() != null)
                {
                    string albumCoverPath = SongHandler.getAlbumCover();
                    visualGridIMG.Source = new BitmapImage(new Uri(albumCoverPath));
                    gradientColor1.Color = Brushes.Black.Color;
                    gradientColor2.Color = Brushes.Black.Color;
                }

                timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(5)
                };
                timer.Tick += (o, e) =>
                {
                    if (SongHandler.getAlbumCover() != null)
                    {
                        string albumCoverPath = SongHandler.getAlbumCover();
                        visualGridIMG.Source = new BitmapImage(new Uri(albumCoverPath));
                        gradientColor1.Color = Brushes.Black.Color;
                        gradientColor2.Color = Brushes.Black.Color;
                    }
                    else
                    {
                        visualGridIMG.Source = null;
                        gradientColor1.Color = Brushes.Black.Color;
                        gradientColor2.Color = Brushes.Black.Color;
                    }
                };
                timer.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Song Not Playing");
            }
        }
    }
}

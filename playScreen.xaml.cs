using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.VisualBasic.ApplicationServices;
using NAudio.Wave;
using simplymusic.AppDate;

namespace simplymusic
{
    /// <summary>
    /// Interaction logic for playScreen.xaml
    /// </summary>
    public partial class playScreen : Window
    {
        //HashSet<Song> songHashset = new HashSet<Song>() { };
        List<Song> songList = new List<Song>() { };
        bool shuffleOn = false;
        public DispatcherTimer timer;

        double newPosition;
     
     
       


        public playScreen()
        {
            InitializeComponent();
            volumeSlider.Value = 5;
            loadStuff();
            startTimer();
            
        }
        private void loadStuff()
        {
            shuffleImage.Opacity = 1.0;
            shuffleBTN.Width = 30;
            shuffleBTN.Height = 30;

         

        }

        private void libaryBN_Click(object sender, RoutedEventArgs e)
        {
            libaryPage lbPage = new libaryPage();
            pageFrame.Content = lbPage;
        }

        private void settingBN_Click(object sender, RoutedEventArgs e)
        {
            SetPage settings = new SetPage();
            pageFrame.Content = settings;
        }


        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            SongHandler.playSong();
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            SongHandler.playSong();
        }

        private void skipforwardButton_Click(object sender, RoutedEventArgs e)
        {
            loadNextSong();

        }

        private void loadNextSong()
        {
            if (timer != null)
            {
                stopTimer();
                filePositionSlider.Value = 0;
            }

            if (songList != null)
            {
                songList = LibarySongs.getCurrentList();

               
                    
                    if (shuffleOn)
                    {
                        Random rnd = new Random();
                        int randomSong = rnd.Next(0, songList.Count);
                        SongHandler.loadSong(songList[randomSong]);
                        float volume = (float)volumeSlider.Value / 10f;
                        SongHandler.setVolume(volume);
                        startTimer();
                    }
                    else
                    {
                        int currentIndex = songList.IndexOf(SongHandler.currSong);
                        int nextIndex = currentIndex + 1;
                        if (nextIndex >= songList.Count)
                        {
                            nextIndex = 0;
                            SongHandler.loadSong(songList[nextIndex]);
                            float volume = (float)volumeSlider.Value / 10f;
                            SongHandler.setVolume(volume);
                            startTimer();
                        }
                        else
                        {
                            SongHandler.loadSong(songList[nextIndex]);
                            float volume = (float)volumeSlider.Value / 10f;
                            SongHandler.setVolume(volume);
                            startTimer();

                        }

                    }
                   

                

            }

        }

        private void skipbackButton_Click(object sender, RoutedEventArgs e)
        {
            if (timer != null)
            {
                stopTimer();
                filePositionSlider.Value = 0;
            }

            if (songList != null)
            {
                songList = LibarySongs.getCurrentList();
               

                    int currentIndex = songList.IndexOf(SongHandler.currSong);
                    int nextIndex = currentIndex - 1;
                    if (nextIndex < 0)
                    {
                        nextIndex = 0;
                        SongHandler.loadSong(songList[nextIndex]);
                        float volume = (float)volumeSlider.Value / 10f;
                        SongHandler.setVolume(volume);
                        startTimer();
                    }
                    else
                    {
                        SongHandler.loadSong(songList[nextIndex]);
                        float volume = (float)volumeSlider.Value / 10f;
                        SongHandler.setVolume(volume);
                        startTimer();

                    }
                

            }

        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SongHandler.outputIsEmpty()) {
                return;
            }
            else
            {
                float volume = (float)volumeSlider.Value / 10f;
                SongHandler.setVolume(volume);
            }
        }

        private void playlist_Click(object sender, RoutedEventArgs e)
        {
            ViewPlaylist vp = new ViewPlaylist();
            pageFrame.Content = vp;

        }

        private void shuffleBTN_Click(object sender, RoutedEventArgs e)
        {
            if (shuffleOn)
            {
                shuffleOn = false;
                shuffleImage.Opacity = 1.0;
                shuffleBTN.Background = System.Windows.Media.Brushes.Transparent;
            }
            else
            {
                shuffleOn = true;
                shuffleImage.Opacity = 0.5;
                shuffleBTN.Background = System.Windows.Media.Brushes.Green;
            }
        }

        private void filePositionSlider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            stopTimer();
            newPosition = filePositionSlider.Value;
            SongHandler.updateSongPosition(newPosition);
            startTimer();

        }
      
        private void filePositionSlider_MouseDown(object sender, MouseButtonEventArgs e)
        {
            stopTimer();
        }


        private void stopTimer()
        {
            if(timer != null)
            {
                timer.IsEnabled = false;
                timer.Stop();
                timer = null;
            }
        }

        private void startTimer()
        {
            if(timer == null)
            {
                timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(.2)
                };
                timer.Tick += (o, e) =>
                {
                  updateSlider();
                };
                timer.IsEnabled = true;
            }
        }

        private void updateSlider()
        {
            var currTime = SongHandler.getCurrentTime();
            if(currTime != 0)
            {
                filePositionSlider.Value = (double)currTime * 100 / SongHandler.getSongLength();
                songTimeLabel.Content = SongHandler.getSongPositionAsString();
            }
            if (SongHandler.newSong)
            {
                if (SongHandler.getAlbumCover() != null)
                {
                    string albumCoverPath = SongHandler.getAlbumCover();
                    albumImageContainer.Source = new BitmapImage(new Uri(albumCoverPath));
                }
                else
                    albumImageContainer.Source = null;

                songNameLabel.Content = SongHandler.currSong.title;
                songArtistLabel.Content = SongHandler.currSong.artist;
                SongHandler.updatedSong();
            }
            if( currTime == -1)
            {
                loadNextSong();
            }
            
        }

        private void visualizerBTN_Click(object sender, RoutedEventArgs e)
        {
            visualizerPage vPage = new visualizerPage();
            pageFrame.Content = vPage;

        }
    }
}

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

namespace simplymusic
{
    /// <summary>
    /// Interaction logic for albumPlaylists.xaml
    /// </summary>
    public partial class albumPlaylists : Page
    {
        private List<string> Albums { get; set; } = new List<string>();
        static HashSet<Song> songHashset = new HashSet<Song>() { };

        public albumPlaylists()
        {
            InitializeComponent();
            loadAlbums();
            DataContext = this;
            getSelectedSong();
        }

        private void loadAlbums()
        {
            try
            {
                Albums = LibarySongs.RetrieveAlbums();

                if (Albums != null)

                    foreach (string album in Albums)
                    {
                        albumsListBox.Items.Add(album);
                    }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private void AlbumListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (albumsListBox.SelectedItem != null)
            {
                string selectedAlbum = albumsListBox.SelectedItem.ToString();
                loadSelectedAlbum(selectedAlbum);
            }
        }

        private void loadSelectedAlbum(string selectedAlbum)
        {
            albumsListBox.IsEnabled = false;
            albumsListBox.Visibility = Visibility.Hidden;
            albumLibary.IsEnabled = true;
            albumLibary.Visibility = Visibility.Visible;

            albumLibary.Items.Clear();
            List<Song> songs = LibarySongs.GetSongsByAlbum(selectedAlbum);

            //Order songs list by track number
            songs = songs.OrderBy(o => o.tracknumber).ToList();
            albumLibary.ItemsSource = songs;
            songHashset = new HashSet<Song>(songs);
        }

        private void playBN_Colmn(object sender, RoutedEventArgs e)
        {
            Song selectedSong = (Song)albumLibary.SelectedItem;
            SongHandler.loadSong(selectedSong);
            LibarySongs.setCurrentList(songHashset);
        }

        private void getSelectedSong()
        {
            if (songHashset.Contains(SongHandler.currSong))
            {
                albumLibary.SelectedItem = SongHandler.currSong;
            }

        }
    }
}

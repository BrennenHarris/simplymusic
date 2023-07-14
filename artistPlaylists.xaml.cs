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
    /// Interaction logic for artistPlaylists.xaml
    /// </summary>
    public partial class artistPlaylists : Page
    {
        private List<string> Artists { get; set; } = new List<string>();
        List<Song> songs = new List<Song>();

        //static HashSet<Song> songHashset = new HashSet<Song>() { };

        public artistPlaylists()
        {
            InitializeComponent();
            loadArtists();
            DataContext = this;
            getSelectedSong();
        }

        private void loadArtists()
        {
            try
            {
                Artists = LibarySongs.RetrieveArtists();

                if (Artists != null)
               
                foreach (string artist in Artists)
                {
                    listBoxTest.Items.Add(artist);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private void ArtistListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(listBoxTest.SelectedItem != null)
            {
                string selectedArtist = listBoxTest.SelectedItem.ToString();
               loadSelectedArtist(selectedArtist);
            }
        }

        private void loadSelectedArtist(string selectedArtist)
        {
            listBoxTest.IsEnabled = false;
            listBoxTest.Visibility = Visibility.Hidden;
            artistLibary.IsEnabled = true;
            artistLibary.Visibility = Visibility.Visible;
          
            artistLibary.Items.Clear();
            songs = LibarySongs.GetSongsByArtist(selectedArtist);
            artistLibary.ItemsSource =songs;
            
        }

        private void playBN_Colmn(object sender, RoutedEventArgs e)
        {
            Song selectedSong = (Song)artistLibary.SelectedItem;
            SongHandler.loadSong(selectedSong);
            LibarySongs.setCurrentList(songs);
        }

        private void getSelectedSong()
        {
      
                artistLibary.SelectedItem = SongHandler.currSong;
            

        }
    }
}

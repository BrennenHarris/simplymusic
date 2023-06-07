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
    /// Interaction logic for UserPlaylist.xaml
    /// </summary>
    public partial class UserPlaylist : Page
    {
        static HashSet<Song> songHashset = new HashSet<Song>() { };
        public UserPlaylist()
        {
            InitializeComponent();
            loadPlaylist();
            getSelectedSong();
        }

        private void loadPlaylist()
        {
            playlistListBox.Items.Clear();

            List<string> playlists = LibarySongs.playlistKeys;

            foreach (string playlist in playlists)
            {
                playlistListBox.Items.Add(playlist);
            }
        }

        private void playlistListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (playlistListBox.SelectedItem != null)
            {
                string selectedPlaylist = playlistListBox.SelectedItem.ToString();
                loaddSelectedPlaylist(selectedPlaylist);
            }

        }
        private void loaddSelectedPlaylist(string selectedPlaylist)
        {
            playlistListBox.IsEnabled = false;
            playlistListBox.Visibility = Visibility.Hidden;
            playlistLibary.IsEnabled = true;
            playlistLibary.Visibility = Visibility.Visible;

            playlistLibary.Items.Clear();

            List<Song> songs = LibarySongs.GetPlaylistByKey(selectedPlaylist);
            playlistLibary.ItemsSource = songs;
            songHashset = new HashSet<Song>(songs);
        }

        private void playBN_Colmn(object sender, RoutedEventArgs e)
        {
            Song selectedSong = (Song)playlistLibary.SelectedItem;
            SongHandler.loadSong(selectedSong);
            LibarySongs.setCurrentList(songHashset);
        }
        private void getSelectedSong()
        {
            if (songHashset.Contains(SongHandler.currSong))
            {
                playlistLibary.SelectedItem = SongHandler.currSong;
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle delete button click here
            Button deleteButton = (Button)sender;
            string item = deleteButton.DataContext as string;
            LibarySongs.removePlaylist(item);
           loadPlaylist();

        }
    }
}

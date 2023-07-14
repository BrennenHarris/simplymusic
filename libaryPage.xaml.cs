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
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using NAudio.Wave;
using simplymusic.AppDate;
////using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace simplymusic
{
    /// <summary>
    /// Interaction logic for libaryPage.xaml
    /// </summary>
    public partial class libaryPage : Page
    {
        
        List<Song> songs = new List<Song>();
        //static HashSet<Song> songHashset = new HashSet<Song>() { };
       

        public libaryPage()
        {
            InitializeComponent();
            getMusicFiles();
        }

        public void getMusicFiles()
        {
            songs = LibarySongs.RetrieveSongs();
            musicLibary.ItemsSource = songs;
          
        }

        private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void playBN_Colmn(object sender, RoutedEventArgs e)
        {
            Song selectedSong = (Song)musicLibary.SelectedItem;
            //songHashset = new HashSet<Song>(songs);
            LibarySongs.setCurrentList(songs);


            sendSong(selectedSong);

        }

        private  void sendSong(Song selectedSong)
        {
           SongHandler.loadSong(selectedSong);
        }

        private void addToBN_Colmn(object sender, RoutedEventArgs e)
        {
            PlaylistAddDialog.Visibility = Visibility.Visible;
            PlaylistAddDialog.IsEnabled = true;
            musicLibary.IsEnabled = false;
            loadPlaylists();

        }

        private void closeDialogBTN_Clicked(object sender, RoutedEventArgs e)
        {
            closeDialog();
        }

        private void addPlaylistKeyBTN_Clicked(object sender, RoutedEventArgs e)
        {
            string playlistKey = playlistKeyInput.Text;
            if (playlistKey != "")
            {
                LibarySongs.addToPlaylist(playlistKey, (Song)musicLibary.SelectedItem);
                PlaylistKeysListBox.Items.Add(playlistKey);
                playlistKeyInput.Text = "";
            }

        }

        private void loadPlaylists()
        {
            List<string> playlists = LibarySongs.playlistKeys;
            PlaylistKeysListBox.Items.Clear();
            foreach (string key in playlists)
            {
                PlaylistKeysListBox.Items.Add(key);
            }
        }

        private void PlaylistKeysListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (PlaylistKeysListBox.SelectedItem != null)
            {
                string playlistKey = PlaylistKeysListBox.SelectedItem.ToString();
                LibarySongs.addToPlaylist(playlistKey, (Song)musicLibary.SelectedItem);
                PlaylistKeysListBox.Items.Add(playlistKey);
                playlistKeyInput.Text = "";
                closeDialog();
            }

        }

        private void closeDialog()
        {
            PlaylistAddDialog.Visibility = Visibility.Hidden;
            PlaylistAddDialog.IsEnabled = false;
            musicLibary.IsEnabled = true;
        }

        

        private void searchTXTBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(searchTXTBox.Text == "")
            {
                getMusicFiles();
            }
            else if(searchTXTBox.Text != "")
            {
                List<Song> searchedSongs = new List<Song>();
                foreach(Song song in songs)
                {
                    if(song.title.ToLower().Contains(searchTXTBox.Text.ToLower()) || song.album != null && song.album.ToLower().Contains(searchTXTBox.Text.ToLower()) || song.artist != null && song.artist.ToLower().Contains(searchTXTBox.Text.ToLower()))
                    {
                        searchedSongs.Add(song);
                    }
                }
                if(searchedSongs.Count > 0)
                musicLibary.ItemsSource = searchedSongs;
            }
        }

        private void clearSearchBTN_Clicked(object sender, RoutedEventArgs e)
        {
            searchTXTBox.Text = "";
     
        }
    }
}

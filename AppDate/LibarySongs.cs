using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace simplymusic.AppDate
{

    public static class LibarySongs
    {
        private const string SettingsFileName = "songLibary.json";
        public static List<Song> songs { get; set; }
        public static List<string> songArtists { get; set; }
        public static List<string> songAlbums { get; set; }
        public static List<string> playlistKeys { get; set; }

        public static Dictionary<string, List<Song>> artistSongs = new Dictionary<string, List<Song>>();
        public static Dictionary<string, List<Song>> albumSongs = new Dictionary<string, List<Song>>();

        public static Dictionary<string, List<Song>> playlistSongs = new Dictionary<string, List<Song>>();


        public static HashSet<Song> songHashset = new HashSet<Song>() { };
       


        static LibarySongs()
        {
            LoadLibarySongs();
            updateArtistSongs();
            updateAlbumSongs();
        }

        private static void LoadLibarySongs()
        {
            // Get the full path of the settings file
            string settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFileName);

            try
            {




                // Check if the settings file exists
                if (File.Exists(settingsFilePath))
                {
                    // Read the contents of the file
                    string json = File.ReadAllText(settingsFilePath);

                    // Deserialize the JSON into a dynamic object
                    dynamic jsonData = JsonConvert.DeserializeObject(json);

                    // Deserialize the JSON into a MusicSettings object
                    songs = JsonConvert.DeserializeObject<List<Song>>(jsonData.songs.ToString()) ?? new List<Song>();
                    songArtists = JsonConvert.DeserializeObject<List<string>>(jsonData.songArtists.ToString()) ?? new List<string>();
                    songAlbums = JsonConvert.DeserializeObject<List<string>>(jsonData.songAlbums.ToString()) ?? new List<string>();

                    playlistKeys = JsonConvert.DeserializeObject<List<string>>(jsonData.playlistKeys.ToString()) ?? new List<string>();
                    playlistSongs = JsonConvert.DeserializeObject<Dictionary<string, List<Song>>>(jsonData.playlistSongs.ToString()) ?? new Dictionary<string, List<Song>>();
                    saveSong();

                }
                else
                {
                    // If the settings file doesn't exist, initialize SongFolders with an empty list
                    songs = new List<Song>();
                    songArtists = new List<string>();
                    songAlbums = new List<string>();

                    playlistKeys = new List<string>();
                    playlistSongs = new Dictionary<string, List<Song>>();

                    // Save the default settings to create the file
                    saveSong();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                fullRefresh();
            }
        }
   
        
        public static void syncLibary()
        {
            try
            {
                LoadLibarySongs();

                var folderList = SettingLog.SongFolders;
                string[] searchPatterns = { "*.jpeg", "*.jpg", "*.png", "*.gif", "*.bmp" };

                foreach (String songFolder in folderList)
                {
                    

                    var files = System.IO.Directory.GetFiles(songFolder, "*.mp3", System.IO.SearchOption.AllDirectories)
                                                                    .Concat(System.IO.Directory.GetFiles(songFolder, "*.wav", System.IO.SearchOption.AllDirectories))
                                                                     .Concat(System.IO.Directory.GetFiles(songFolder, "*.m4a", System.IO.SearchOption.AllDirectories))
                                                                      .Concat(System.IO.Directory.GetFiles(songFolder, "*.flac", System.IO.SearchOption.AllDirectories));
                    foreach (String file in files)
                    {
                        string folderpath = Path.GetDirectoryName(file);
                        string[] imageFiles = searchPatterns
                                                                .SelectMany(pattern => Directory.GetFiles(folderpath, pattern))
                                                                    .ToArray();
                       




                        TagLib.File tagFile = TagLib.File.Create(file);

                        if (!songs.Any(s => s.path == file))
                        {
                            TimeSpan duration = tagFile.Properties.Duration;
                            string formattedDuration = $"{(int)duration.TotalMinutes}:{duration.Seconds:D2}";
                            if (!songs.Any(s => s.title == tagFile.Tag.Title && s.duration == formattedDuration))
                            {
                                if (imageFiles.Length <= 0)
                                {
                                    //retrieve the track number of the tagfile
                                    //int trackNumber = (int)tagFile.Tag.Track;
                                    songs.Add(new Song(tagFile.Tag.Title ?? System.IO.Path.GetFileNameWithoutExtension(file), tagFile.Tag.Album, formattedDuration, tagFile.Tag.FirstPerformer, null, (int)tagFile.Tag.Track, file));
                                }
                                else songs.Add(new Song(tagFile.Tag.Title ?? System.IO.Path.GetFileNameWithoutExtension(file), tagFile.Tag.Album, formattedDuration, tagFile.Tag.FirstPerformer, imageFiles[0], (int)tagFile.Tag.Track, file));
                            }
                                if (!songArtists.Contains(tagFile.Tag.FirstPerformer))
                            {
                                songArtists.Add(tagFile.Tag.FirstPerformer);
                                
                            }
                            if (!songAlbums.Contains(tagFile.Tag.Album))
                            {
                                songAlbums.Add(tagFile.Tag.Album);
                            }
                        }
                    }
                }
                updateArtistSongs();
                updateAlbumSongs();
                saveSong();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public static void fullRefresh()
        {
            //if file exist delete it
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFileName)))
            {
                File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFileName));
            }
            LoadLibarySongs();
        }

        private static void saveSong()
        {
            // Get the full path of the settings file
            string settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFileName);


            // Create a new object to store the song list, artist list, and album list
            dynamic jsonData = new
            {
                songs = songs,
                songArtists = songArtists,
                songAlbums = songAlbums,
                playlistKeys = playlistKeys,
                playlistSongs = playlistSongs
            };

            // Serialize the MusicSettings object to JSON
            string json = JsonConvert.SerializeObject(jsonData);

            // Write the JSON to the settings file
            File.WriteAllText(settingsFilePath, json);
            
        }

        public static List<Song> RetrieveSongs()
        {
            // Return the current SongFolders list
            return songs;
        }

        public static List<string> RetrieveArtists()
        {
            // Return the current SongFolders list
            return songArtists;
        }

        public static List<string> RetrieveAlbums()
        {
            // Return the current SongFolders list
            return songAlbums;
        }

        public static List<string> RetrievePlaylists()
        {
            // Return the current SongFolders list
            return playlistKeys;
        }

        public static void deletedItem()
        {
            // If the settings file doesn't exist, initialize SongFolders with an empty list
            songs = new List<Song>();
            songArtists = new List<string>();
            songAlbums = new List<string>();

            // Save the default settings to create the file
            saveSong();

        }

        public static List<Song> GetSongsByArtist(string artist)
        {
            List<Song> songsByArtist = new List<Song>();
           
            if (artistSongs.ContainsKey(artist))
            {
           
                return artistSongs[artist];
            }
            else
            {
                return new List<Song>();
            }
        }

        public static List<Song> GetSongsByAlbum(string album)
        {
            List<Song> songsByAlbum = new List<Song>();
        
            if (albumSongs.ContainsKey(album))
            {
              
                return albumSongs[album];
            }
            else
            {
                return new List<Song>();
            }
        }

        public static List<Song> GetPlaylistByKey(string playlistKey)
        {
            List<Song> songsByPlaylist = new List<Song>();

            if (playlistSongs.ContainsKey(playlistKey))
            {
                return playlistSongs[playlistKey];
            }
            else
            {
                return new List<Song>();
            }

        }

        public static void addToPlaylist(string playlistName, Song song)
        {
            

            if (playlistKeys.Contains(playlistName))
            {
                playlistSongs[playlistName].Add(song);

            }
            else
            {
                playlistKeys.Add(playlistName);
                playlistSongs[playlistName] = new List<Song>
                {
                    song
                };
            }
            saveSong();
        }
        public static void setCurrentList(HashSet<Song> songs)
        {
            songHashset = songs;
        }
        public static HashSet<Song> getCurrentList()
        {
            return songHashset;
        }


        private static void updateArtistSongs()
        {
            // Clear the existing dictionary
            artistSongs.Clear();

            foreach (Song song in songs)
            {
                if(song.artist != null)
                {
                    // Check if the artist already exists in the dictionary
                    if (!artistSongs.ContainsKey(song.artist))
                    {
                        // If the artist does not exist, create a new list for the artist
                        artistSongs[song.artist] = new List<Song>();
                    }

                    // Add the song to the list of the corresponding artist
                    artistSongs[song.artist].Add(song);

                }
             
            }
        }

        private static void updateAlbumSongs()
        {
            // Clear the existing dictionary
            albumSongs.Clear();

            foreach (Song song in songs)
            {
                if (song.album != null)
                {
                    // Check if the artist already exists in the dictionary
                    if (!albumSongs.ContainsKey(song.album))
                    {
                        // If the artist does not exist, create a new list for the artist
                        albumSongs[song.album] = new List<Song>();
                    }

                    // Add the song to the list of the corresponding artist
                    albumSongs[song.album].Add(song);

                }

            }
        }

        public static void removePlaylist(string playlistkey)
        {
            if (playlistKeys.Contains(playlistkey))
            {
                playlistKeys.Remove(playlistkey);
                playlistSongs.Remove(playlistkey);
                saveSong();
            }

        }
    }
}

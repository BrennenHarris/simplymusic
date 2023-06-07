using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace simplymusic.AppDate
{
    public static class SettingLog
    {
        private const string SettingsFileName = "simplymusicsetting.json";
        public static List<string> SongFolders { get; set; }

        static SettingLog()
        {
            LoadSettings();
        }

        private static void LoadSettings()
        {
            // Get the full path of the settings file
            string settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFileName);

           
            // Check if the settings file exists
            if (File.Exists(settingsFilePath))
            {
                // Read the contents of the file
                string json = File.ReadAllText(settingsFilePath);

                // Deserialize the JSON into a MusicSettings object
                SongFolders = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();

            }
            else
            {
                // If the settings file doesn't exist, initialize SongFolders with an empty list
                SongFolders = new List<string>();

                // Save the default settings to create the file
                SaveSettings();
            }
        }

        private static void SaveSettings()
        {
            // Get the full path of the settings file
            string settingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFileName);

            // Serialize the MusicSettings object to JSON
            string json = JsonConvert.SerializeObject(SongFolders);

            // Write the JSON to the settings file
            File.WriteAllText(settingsFilePath, json);
        }

        public static void SaveSongFolders(List<string> newSongFolders)
        {
            // Update the SongFolders property with the provided list
            SongFolders = newSongFolders;

            // Save the updated settings to the file
            SaveSettings();
        }

        public static List<string> RetrieveSongFolders()
        {
            // Return the current SongFolders list
            return SongFolders;
        }

    }
}



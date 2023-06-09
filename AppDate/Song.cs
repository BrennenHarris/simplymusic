﻿using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace simplymusic.AppDate
{
    public class Song : IComparable<Song>
    {
        public Song(string title, string album, string time, string firstPerformer, string albumPath, int trackNumber, string file)
        {
            this.title = title;
            this.album = album;
            duration = time;
            artist = firstPerformer;
            albumCoverPath = albumPath;
            tracknumber = trackNumber;
            path = file;
        }

        public string title { get; set; }
        public string album { get; set; }
        public string duration { get; set; }
        public string artist { get; set; }

        public string albumCoverPath { get; set; }

        public int tracknumber { get; set; }

        public string path { get; set; }

        public int CompareTo(Song other)
        {
            // Compare the entire song objects
            int result = string.Compare(artist, other.artist);

            if (result == 0)
            {
                // If the artists are the same, compare the song titles
                result = string.Compare(title, other.title);
            }

            return result;
        }

        public string getFileExtension()
        {
            string pattern = @"\.([^.]+)$"; // Regular expression pattern to match the file extension
            Match match = Regex.Match(path, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value; // Extract the matched file extension
            }
            return string.Empty; // Return empty string if no file extension found
        }

    }
}

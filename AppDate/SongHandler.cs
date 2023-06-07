using NAudio.CoreAudioApi.Interfaces;
using NAudio.Dsp;
using NAudio.Flac;
using NAudio.Gui;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using TagLib;


namespace simplymusic.AppDate
{
    internal class SongHandler
    {
        private static WaveOutEvent outputDevice;
        private static WaveStream audioFile;
        
        public static  Song currSong {get; set;}

        public static long songPos;

        public static float volume = 0.5f;

        public  static bool newSong = false;

        public static bool loadNextSong = false;

        public static void loadSong(Song song)
        {
            newSong = true;
            currSong = song;
           
            resetAudio();
            
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
               

            }

            if (audioFile == null)
            {
                if (currSong.getFileExtension() == "flac")
                {
                    audioFile = new FlacReader(song.path);
                }
                else
                {
                    audioFile = new AudioFileReader(song.path);
                }
                outputDevice.Init(audioFile);
               

            }
            outputDevice.Stop();
            Thread.Sleep(800);

            outputDevice.Volume = volume;
            outputDevice.Play();
            
          
        }

        public static void loadSongNewPosition()
        {
            
            resetAudio();
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                

            }

            if (audioFile == null)
            {
                if (currSong.getFileExtension() == "flac")
                {
                    audioFile = new FlacReader(currSong.path);
                }
                else
                {
                    audioFile = new AudioFileReader(currSong.path);
                }
                outputDevice.Init(audioFile);
                audioFile.Position = songPos;


            }
      
            outputDevice.Volume = volume;
            Thread.Sleep(200); // Allow some time for the audio file to start playing
            outputDevice.Play();
            

        }


        public static void playSong()
        {
            if (outputDevice != null)
            {
                if (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    outputDevice.Pause();
                }
                else
                outputDevice.Play();
            }
         
        }

        public static void resetAudio()
        {
            if (audioFile != null)
            {
                outputDevice.Stop();
                outputDevice.Dispose();
                outputDevice = null;
                audioFile.Dispose();
                audioFile = null;
                
            }
        }


        public static void setVolume(float newVolume)
        {
            if (outputDevice != null)
            {
                volume = newVolume;
                outputDevice.Volume = volume;
            }
           
        }

        public static bool outputIsEmpty()
        {
            if(outputDevice == null)
            {
                return true;
            }
            else return false;
        }

     
       

        public static long getCurrentTime()
        {
            if(audioFile != null)
            {
                if (audioFile.Position >= audioFile.Length || audioFile.Length - audioFile.Position <= 50000)
                {
                    return -1;
                }
                else return audioFile.Position;

            }
            else return 0;
            
        }

        public static long getSongLength()
        {
            if (audioFile != null)
            {
                return audioFile.Length;
            }
            return -1;
        }

        
        
        public static void updateSongPosition(double position)
        {
            try
            {
                if (audioFile != null)
                {
                    outputDevice.Stop();
                    Thread.Sleep(100);
                    var newPosition = position;
                    var lengthinbytes = audioFile.Length;
                    var pos = (lengthinbytes / 100) * newPosition;
                    songPos = (long)pos;
                    loadSongNewPosition();

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
        
     


     

        public  static string getAlbumCover()
        {
            return currSong.albumCoverPath;
        }


        public static string getSongPositionAsString()
        {
            if (audioFile != null)
            {
                TimeSpan currentTime = audioFile.CurrentTime;
                TimeSpan songLength = audioFile.TotalTime;
                string formattedTime = string.Format("{0}:{1:00}", (int)currentTime.TotalMinutes, currentTime.Seconds);
                string formattedLength = string.Format("{0}:{1:00}", (int)songLength.TotalMinutes, songLength.Seconds);
                return formattedTime +  " / " + formattedLength;
            }
            return null;
        }

        public static void updatedSong()
        {
            newSong = false;
        }

    }
}

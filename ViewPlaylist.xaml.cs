using Microsoft.VisualBasic;
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
    /// Interaction logic for ViewPlaylist.xaml
    /// </summary>
    public partial class ViewPlaylist : Page
    {
     //Frame parentFrame;



        public ViewPlaylist()
        {
            InitializeComponent();
            //object parentContent = parentFrame.Content;



        }

        private void artistBTN_OnClick(object sender, RoutedEventArgs e)
        {
            // Find the parent window
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                // Find the frame within the parent window
                Frame playFrame = parentWindow.FindName("pageFrame") as Frame;

                if (playFrame != null)
                {
                    // Create a new instance of the page you want to navigate to
                   artistPlaylists newPage = new artistPlaylists();

                    // Set the content of the frame to the new page
                    playFrame.Content = newPage;
                }
            }

        }

        private void albumBTN_OnClick(object sender, RoutedEventArgs e)
        {
            // Find the parent window
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                // Find the frame within the parent window
                Frame playFrame = parentWindow.FindName("pageFrame") as Frame;

                if (playFrame != null)
                {
                    // Create a new instance of the page you want to navigate to
                    albumPlaylists newPage = new albumPlaylists();

                    // Set the content of the frame to the new page
                    playFrame.Content = newPage;
                }
            }

        }

        private void userPLaylistBTN_OnClick(object sender, RoutedEventArgs e)
        {
            // Find the parent window
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                // Find the frame within the parent window
                Frame playFrame = parentWindow.FindName("pageFrame") as Frame;

                if (playFrame != null)
                {
                    // Create a new instance of the page you want to navigate to
                    UserPlaylist newPage = new UserPlaylist();

                    // Set the content of the frame to the new page
                    playFrame.Content = newPage;
                }
            }

        }
    }
}

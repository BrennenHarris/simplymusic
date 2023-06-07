using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using simplymusic.AppDate;

namespace simplymusic
{
    /// <summary>
    /// Interaction logic for SetPage.xaml
    /// </summary>
    public partial class SetPage : Page
    {
        List<string> folderListItems = new List<string>();

        public SetPage()
        {
            InitializeComponent();
            updateFolderList();
        }

        private void AddFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            string path = folderBrowserDialog.SelectedPath;
            addToFolderList(path);
        }


        private void addToFolderList(string path)
        {
            //iterate threw the folder list and add all items to a list of strings called folderListItems
            folderListItems = new List<string>();
            foreach (var item in folderList.Items)
            {
                folderListItems.Add(item.ToString());
            }
            if (!folderListItems.Contains(path))
            {
                folderListItems.Add(path);
            }

           //Call the saveSongFolders function from SettingLog and pass threw the folderListItems list

            SettingLog.SaveSongFolders(folderListItems);
            updateFolderList();
        }

        private void updateFolderList()
        {
            try
            {
                //Call the retriveSongFolders function from SettingLog and set folderList.source to the returned list
                folderListItems = SettingLog.RetrieveSongFolders();
                folderList.ItemsSource = folderListItems;
                folderList.Items.Refresh();


            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("No song folders: " + e);
            }
          

        }

        //Delete items from the folder list
        private void delete_Click(object sender, RoutedEventArgs e)
        {
           //Get the item.text of the item where this button press occured
            string item = ((System.Windows.Controls.Button)sender).Tag.ToString();
       
            folderListItems.Remove(item);
            SettingLog.SaveSongFolders(folderListItems);
            updateFolderList();
            LibarySongs.deletedItem();
        }

       //Update Song Libary
        private void Sync_Click(object sender, RoutedEventArgs e)
        {
            LibarySongs.syncLibary();

        }

        private void fullRefresh_Click(object sender, RoutedEventArgs e)
        {
            LibarySongs.fullRefresh();
            LibarySongs.syncLibary();

        }
    }
}

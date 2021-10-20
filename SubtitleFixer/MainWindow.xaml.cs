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
using System.IO;
using Microsoft.Win32;
using SubtitleFixer.Business;
using System.Diagnostics;

namespace SubtitleFixer
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// This Is The Whole Extension + Zip File Convert By Application 
        /// </summary>
        private List<string> SupportExtension;
        /// <summary>
        /// list Of All Dropped File
        /// </summary>
        private List<string> DroppedFile;


        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// When File/Folder/Zip Droped Here This Event Occure And App Fix It
        /// </summary>
        private void FrmMainWindow_Drop(object sender, DragEventArgs e)
        {
            SubFile.SubtitleFilePaths.Clear();
            SubZip.ZipSubtitlePaths.Clear();
            SupportExtension = Setting.ExtensionList;
            DroppedFile = ((string[])e.Data.GetData(DataFormats.FileDrop)).ToList();

            foreach (string itemPath in DroppedFile)
            {
                FileAttributes attributes = File.GetAttributes(itemPath);

                if (attributes.HasFlag(FileAttributes.Directory)) //برای تشخیص پوشه یودن
                {
                    FindFileInDirectory(itemPath);
                }
                else
                {
                    MakeSubtitleFileList(itemPath);
                }
            }
            ChangeElementsPropertiesByDragDrop();
        }
        private bool CheckLists()
        {
            bool flag = false;
            if (SubFile.SubtitleFilePaths.Count != 0)
            {
                flag = true;
            }

            if (SubZip.ZipSubtitlePaths.Count != 0)
            {
                flag = true;
            }
            return flag;
        }
       
        private void ChangeElementsPropertiesByDragDrop()
        {
            
            if (CheckLists().Equals(true))
            {
                Btn_Start.IsEnabled = true;
                LblHelp.Text = "جهت اصلاح زیرنویس ها روی دکمه شروع اصلاح کلیک کنید";
            }
            else
            {
                Btn_Start.IsEnabled = false;
                LblHelp.Text = "فایل ها و پوشه های مربوط به زیرنویس را اینجا کشیده و رها کنید";
                LblHelp.Foreground = new SolidColorBrush(Color.FromRgb(207, 216, 213));
            }
        }
        /// <summary>
        /// if file path list or compressed path list is not empty start fixing proccess
        /// </summary>
        public void StartFixingProccess()
        {
            if (SubFile.SubtitleFilePaths.Count != 0)
            {
                SubFile.FixSubtitlesInList();
            }

            if (SubZip.ZipSubtitlePaths.Count != 0)
            {
                SubZip.FixSubtitles(SupportExtension);
            }
            DroppedFile.Clear();
        }
        /// <summary>
        /// Find All File In This Folder : Here Is Dropped Folder
        /// </summary>
        /// <param name="path">Path Of Folder</param>
        void FindFileInDirectory(string path)
        {
            string[] allfiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            foreach (string file in allfiles)
            {
                MakeSubtitleFileList(file);
            }
        }

        /// <summary>
        /// Detect File is one of subtitle or zip type and add it to list
        /// </summary>
        /// <param name="filePath">path of Dropped File</param>
        private void MakeSubtitleFileList(string filePath)
        {
            string fileExtension = Path.GetExtension(filePath).ToLower();
            if (SupportExtension.Contains(fileExtension))
            {
                SubFile.SubtitleFilePaths.Add(filePath);
                Btn_Start.IsEnabled = true;
            }
            else if (fileExtension.Equals(".zip"))
            {
                SubZip.ZipSubtitlePaths.Add(filePath);
                Btn_Start.IsEnabled = true;
            }
        }
        /// <summary>
        /// Move Main Window
        /// </summary>
        /// <param name="sender">The Object Who Called It</param>
        /// <param name="e">Arguments Of Event</param>
        private void WindowMove(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton.Equals(MouseButtonState.Pressed))
            {
                DragMove();
            }
        }
        /// <summary>
        /// Exit Application
        /// </summary>
        ///  <param name="sender">The Object Who Called It</param>
        /// <param name="e">Arguments Of Event</param>
        private void Exit(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown(0);
            Environment.Exit(0);
        }
        /// <summary>
        /// Show Setting Window
        /// </summary>
        ///  <param name="sender">The Object Who Called It</param>
        /// <param name="e">Arguments Of Event</param>
        private void BtnSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow
            {
                Owner = this
            };
            settingWindow.ShowDialog();
        }
        /// <summary>
        /// Show My GitHub Page In Browser
        /// </summary>
        ///  <param name="sender">The Object Who Called It</param>
        /// <param name="e">Arguments Of Event</param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            StartFixingProccess();
            MessageWindow messageWindow = new MessageWindow()
            {
                Owner = this
            };
            messageWindow.Show();
            ReadyForNewSubs();
        }
        private void ReadyForNewSubs()
        {
            DroppedFile.Clear();
            SubFile.SubtitleFilePaths.Clear();
            SubZip.ZipSubtitlePaths.Clear();
            ChangeElementsPropertiesByDragDrop();
        }
        private void Window_DragOver(object sender, DragEventArgs e)
        {
            LblHelp.Foreground = new SolidColorBrush(Color.FromRgb(245, 203, 92));
        }

        private void Window_DragLeave(object sender, DragEventArgs e)
        {
            LblHelp.Foreground = new SolidColorBrush(Color.FromRgb(207, 216, 213)); //CFDBD5
        }

        private void MainExp_Collapsed(object sender, RoutedEventArgs e)
        {
            TxtTitle.Opacity=1;
        }

        private void MainExp_Expanded(object sender, RoutedEventArgs e)
        {
            TxtTitle.Opacity = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SubtitleFixer
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        private static readonly Regex OnlyDotThenEnglish = new Regex(@"^(\.[a-z]+)( \.[a-z]+)*$");
        private static readonly Regex OnlyEnglishAndSpace = new Regex(@"^(([a-zA-Z]) ?)+$");
        private static string ExtensionString;
        private static string EditedTitleTemp;
        /// <summary>
        /// Construction Method For Initialize Components And Fill Form
        /// </summary>
        public SettingWindow()
        {
            InitializeComponent();
            FillFormFromSetting();
        }
        /// <summary>
        /// Save New Setting And Close Form When It Done
        /// </summary>
        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            SaveAppSetting();
            Close();
        }
        /// <summary>
        /// This Method Load Settings From Application Setting And Fill Object With Setting Data`s
        /// </summary>
        private void FillFormFromSetting()
        {
            TxtEditedTitle.Text = Business.Setting.EditedTitle;
            RbnReplace.IsChecked = Business.Setting.IsReplaceMode;
            RbnEdited.IsChecked = !RbnReplace.IsChecked;
            TxtExtensions.Text = string.Join(" ", Business.Setting.ExtensionList);
        }
        /// <summary>
        /// This Method Save New Setting In Application Setting To keep For Furture Application Run
        /// </summary>
        private void SaveAppSetting()
        {
            Business.Setting.EditedTitle = TxtEditedTitle.Text.Trim();
            Business.Setting.ExtensionList = ExtensionString.Split(' ').ToList();
            Business.Setting.IsReplaceMode = (bool)RbnReplace.IsChecked;
        }

        private void TxtExtensions_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionString = TxtExtensions.Text.ToLower().Trim();
            ExtensionString = Regex.Replace(ExtensionString, @"\s+", " ");
            if (OnlyDotThenEnglish.IsMatch(ExtensionString))
                btnDone.IsEnabled = true;
            else
                btnDone.IsEnabled = false;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton.Equals(MouseButtonState.Pressed))
            {
                DragMove();
            }
        }

        private void RbnReplace_Checked(object sender, RoutedEventArgs e)
        {
            PnlEditedTitle.IsEnabled = false;
            PnlEditedTitle.Visibility = Visibility.Collapsed;
        }

        private void RbnEdited_Checked(object sender, RoutedEventArgs e)
        {
            PnlEditedTitle.IsEnabled = true;
            PnlEditedTitle.Visibility = Visibility.Visible;
        }

        private void TxtEditedTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            EditedTitleTemp = TxtEditedTitle.Text.Trim();
            EditedTitleTemp = Regex.Replace(EditedTitleTemp, @"\s+", " ");
            if (OnlyEnglishAndSpace.IsMatch(EditedTitleTemp))
                btnDone.IsEnabled = true;
            else
                btnDone.IsEnabled = false;
        }
    }
}

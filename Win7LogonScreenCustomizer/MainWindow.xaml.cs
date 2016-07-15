using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32;

namespace Win7LogonScreenCustomizer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LogonScreenState state = new LogonScreenState();

            if (File.Exists(BackgroundHelper.OOBEBackgroundImageFullPath))
            {
                state.BackgroundInfo = new LogonBackgroundInfo(BackgroundHelper.OOBEBackgroundImageFullPath);
            }

            this.DataContext = state;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "Supported Image Files (*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff)|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff|Bitmap Image (*.bmp)|*.bmp|JPEG Image (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG Image (*.png)|*.png|TIFF Image (*.tif;*.tiff)|*.tif;*.tiff"
            };

            if (dlg.ShowDialog(this) == true)
            {
                LogonScreenState.Current.BackgroundInfo = new LogonBackgroundInfo(dlg.FileName);
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            EnableCustomLogonBackgroundCheckBox.GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();
            LogonButtonStyleComboBox.GetBindingExpression(ComboBox.SelectedIndexProperty).UpdateSource();

            LogonMessageCaptionTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            LogonMessageTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            DisplayLastLogonStatusCheckBox.GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource();

            if (LogonScreenState.Current.IsCustomBackgroundEnabled && LogonScreenState.Current.BackgroundInfo != null
                && !LogonScreenState.Current.BackgroundInfo.ImageFileInfo.FullName.Equals(BackgroundHelper.OOBEBackgroundImageFullPath))
            {
                BackgroundHelper.SetBackgroundImage(LogonScreenState.Current.BackgroundInfo);
            }

            MessageBox.Show(this, "New logon customizations have been saved." + Environment.NewLine + "Press Win+L to preview.",
                this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(this,
                String.Format("This operation will remove all logon customizations, including current custom background image.{0}{0}Continue?", Environment.NewLine),
                this.Title, MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                LogonScreenState.Current.IsCustomBackgroundEnabled = false;
                LogonScreenState.Current.BackgroundInfo = null;
                LogonScreenState.Current.ButtonStyle = 0;

                LogonScreenState.Current.MessageCaption = String.Empty;
                LogonScreenState.Current.MessageText = String.Empty;
                LogonScreenState.Current.DisplayLastLogonInfo = false;

                if (File.Exists(BackgroundHelper.OOBEBackgroundImageFullPath))
                {
                    File.Delete(BackgroundHelper.OOBEBackgroundImageFullPath);
                }

                MessageBox.Show(this, "All logon customizations have been reset to default.", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

using Microsoft.Win32;

namespace Win7LogonBackgroundChanger
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            fileNameLabel.Text = String.Format(fileNameLabel.Tag.ToString(), Screen.PrimaryScreen.Bounds.Width.ToString(), Screen.PrimaryScreen.Bounds.Height.ToString());

            isOEMBackgroundEnabled = (oemBackgroundRegistryKey.GetValue("UseOEMBackground", "0").ToString() == "1");
            enableCheckBox.Checked = isOEMBackgroundEnabled;

            backgroundGroupBox_VisibleChanged(sender, e);
        }

        private void enableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            backgroundGroupBox.Enabled = enableCheckBox.Checked;
            backgroundGroupBox.Visible = enableCheckBox.Checked;
        }

        private void enableCheckBox_Click(object sender, EventArgs e)
        {
            applyButton.Enabled = (isOEMBackgroundEnabled != enableCheckBox.Checked);
        }

        private void backgroundGroupBox_VisibleChanged(object sender, EventArgs e)
        {
            this.Size = backgroundGroupBox.Visible ? new Size(800, 600) : new Size(400, 100);
        }

        private void backgroundGroupBox_EnabledChanged(object sender, EventArgs e)
        {
            if (backgroundGroupBox.Enabled)
            {
                String tempBackgroundFileName = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), Path.GetTempFileName());

                File.Copy(File.Exists(oemBackgroundPath) ? oemBackgroundPath : defaultBackgroundPath, tempBackgroundFileName, true);

                backgroundPictureBox.Image = Image.FromFile(tempBackgroundFileName);
            }
            else
            {
                backgroundPictureBox.Image.Dispose();
                backgroundPictureBox.Image = null;
            }

            fileNameLabel.Text = String.Format(fileNameLabel.Tag.ToString(), Screen.PrimaryScreen.Bounds.Width.ToString(), Screen.PrimaryScreen.Bounds.Height.ToString());
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Supported Image Files (*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff)|*.bmp;*.jpg;*.png;*.tif;*.tiff|Bitmap (*.bmp)|*.bmp|JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png|TIFF (*.tif;*.tiff)|*.tif;*.tiff";
                dlg.CheckFileExists = true;

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (backgroundPictureBox.Image != null)
                    {
                        backgroundPictureBox.Image.Dispose();
                    }

                    backgroundPictureBox.Image = Image.FromFile(dlg.FileName);
                    backgroundPictureBox.Tag = dlg.FileName;

                    fileNameLabel.Text = Path.GetFileName(dlg.FileName);

                    applyButton.Enabled = true;
                }
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            if (isOEMBackgroundEnabled != enableCheckBox.Checked)
            {
                oemBackgroundRegistryKey.SetValue("UseOEMBackground", Convert.ToInt32(enableCheckBox.Checked).ToString(), RegistryValueKind.DWord);

                isOEMBackgroundEnabled = enableCheckBox.Checked;
            }

            if (enableCheckBox.Checked && backgroundPictureBox.Tag != null)
            {
                if (File.Exists(oemBackgroundPath))
                {
                    DialogResult result = MessageBox.Show("Do you want to overwrite the existing logon UI background?", Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (result != DialogResult.Yes)
                    {
                        return;
                    }
                }

                // Use "Any CPU" to compile, or the following codes cannot work

                if (!Directory.Exists(oemBackgroundDirectory))
                {
                    Directory.CreateDirectory(oemBackgroundDirectory);
                }

                FileInfo fileInfo = new FileInfo(backgroundPictureBox.Tag.ToString());

                if ((fileInfo.Extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase)
                    || fileInfo.Extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
                    && fileInfo.Length <= 250 * 1024)
                {
                    File.Copy(fileInfo.FullName, oemBackgroundPath, true);
                }
                else
                {
                    String tempFileName = Path.GetTempFileName();

                    using (Bitmap imageBitmap = new Bitmap(backgroundPictureBox.Image))
                    {
                        ImageCodecInfo imageCodecInfo = null;

                        foreach (ImageCodecInfo info in ImageCodecInfo.GetImageEncoders())
                        {
                            if (info.MimeType == "image/jpeg")
                            {
                                imageCodecInfo = info;

                                break;
                            }
                        }

                        if (imageCodecInfo != null)
                        {
                            using (EncoderParameters encoderParameters = new EncoderParameters())
                            {
                                Int64 qualityLevel = 100;

                                do
                                {
                                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityLevel);

                                    imageBitmap.Save(tempFileName, imageCodecInfo, encoderParameters);

                                    qualityLevel--;

                                    fileInfo = new FileInfo(tempFileName);
                                }
                                while (fileInfo.Length > 250 * 1024);
                            }
                        }
                        else
                        {
                            MessageBox.Show("JPEG Encoder is not found.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                            return;
                        }
                    }

                    File.Copy(tempFileName, oemBackgroundPath, true);
                    File.Delete(tempFileName);
                }
            }

            applyButton.Enabled = false;

            MessageBox.Show("New settings has been applied.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

            fileNameLabel.Text = String.Format(fileNameLabel.Tag.ToString(), Screen.PrimaryScreen.Bounds.Width.ToString(), Screen.PrimaryScreen.Bounds.Height.ToString());

            backgroundPictureBox.Tag = null;
        }

        private static Boolean isOEMBackgroundEnabled;

        private static readonly String oemBackgroundDirectory = Path.Combine(Environment.SystemDirectory, @"oobe\info\backgrounds");
        private static readonly String oemBackgroundPath = Path.Combine(oemBackgroundDirectory, @"backgroundDefault.jpg");
        private static readonly String defaultBackgroundPath = Path.Combine(Environment.SystemDirectory, @"oobe\background.bmp");

        private static readonly RegistryKey oemBackgroundRegistryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System", true);
    }
}
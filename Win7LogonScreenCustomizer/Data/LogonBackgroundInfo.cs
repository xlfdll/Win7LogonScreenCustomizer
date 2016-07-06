using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Win7LogonScreenCustomizer
{
    public class LogonBackgroundInfo
    {
        public LogonBackgroundInfo(String fileName)
        {
            _imageFileInfo = new FileInfo(fileName);
        }

        private FileInfo _imageFileInfo;
        private BitmapImage _image;

        public FileInfo ImageFileInfo
        {
            get { return _imageFileInfo; }
        }
        public String FileName
        {
            get { return _imageFileInfo.Name; }
        }
        public String Directory
        {
            get { return _imageFileInfo.DirectoryName; }
        }
        public String Size
        {
            get
            {
                return (_imageFileInfo.Length / Math.Pow(1024, 2)).ToString("F2") + " MB";
            }
        }

        public BitmapImage Image
        {
            get
            {
                if (_image == null)
                {
                    // Load image file without keeping it open
                    _image = new BitmapImage();

                    _image.BeginInit();
                    _image.UriSource = new Uri(_imageFileInfo.FullName);
                    _image.CacheOption = BitmapCacheOption.OnLoad;
                    _image.EndInit();
                }

                return _image;
            }
        }
        public String ImageDimensions
        {
            get
            {
                return this.Image.PixelWidth.ToString() + " * " + this.Image.PixelHeight.ToString();
            }
        }
    }
}
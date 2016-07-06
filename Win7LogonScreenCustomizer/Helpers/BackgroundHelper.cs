using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Win7LogonScreenCustomizer
{
    public static class BackgroundHelper
    {
        public static void SetBackgroundImage(LogonBackgroundInfo logonBackgroundInfo)
        {
            if (!Directory.Exists(OOBEBackgroundDirectory))
            {
                Directory.CreateDirectory(OOBEBackgroundDirectory);
            }
            else if (File.Exists(BackgroundHelper.OOBEBackgroundImageFullPath))
            {
                File.Delete(BackgroundHelper.OOBEBackgroundImageFullPath);
            }

            if ((logonBackgroundInfo.ImageFileInfo.Extension.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase)
                || logonBackgroundInfo.ImageFileInfo.Extension.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
                && logonBackgroundInfo.ImageFileInfo.Length <= BackgroundHelper.OOBEBackgroundImageFileSizeLimit)
            {
                File.Copy(logonBackgroundInfo.ImageFileInfo.FullName, BackgroundHelper.OOBEBackgroundImageFullPath);
            }
            else
            {
                String tempFileName = Path.GetTempFileName();

                for (Int32 q = 100; q > 0; q--)
                {
                    // BitmapEncoder object can be used to save only once

                    JpegBitmapEncoder bitmapEncoder = new JpegBitmapEncoder()
                    {
                        QualityLevel = q
                    };

                    bitmapEncoder.Frames.Add(BitmapFrame.Create(logonBackgroundInfo.Image));

                    using (FileStream stream = new FileStream(tempFileName, FileMode.Create))
                    {
                        bitmapEncoder.Save(stream);
                    }

                    FileInfo tempFileInfo = new FileInfo(tempFileName);

                    if (tempFileInfo.Length <= BackgroundHelper.OOBEBackgroundImageFileSizeLimit)
                    {
                        break;
                    }
                }

                File.Copy(tempFileName, BackgroundHelper.OOBEBackgroundImageFullPath);
                File.Delete(tempFileName);
            }
        }

        public static readonly String OOBEBackgroundDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"oobe\info\backgrounds");
        public static readonly String OOBEBackgroundImageFileName = "backgroundDefault.jpg";
        public static readonly String OOBEBackgroundImageFullPath = Path.Combine(BackgroundHelper.OOBEBackgroundDirectory, BackgroundHelper.OOBEBackgroundImageFileName);

        public const Int32 OOBEBackgroundImageFileSizeLimit = 256000;
    }
}
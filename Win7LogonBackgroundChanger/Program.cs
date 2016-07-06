using System;
using System.Windows.Forms;

namespace Win7LogonBackgroundChanger
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
            {
                Application.Run(new MainForm());
            }
            else
            {
                MessageBox.Show("This version of Windows is not supported. Program will exit.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Application.Exit();
            }
        }
    }
}
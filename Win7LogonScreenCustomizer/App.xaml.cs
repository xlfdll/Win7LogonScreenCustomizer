using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace Win7LogonScreenCustomizer
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (!Regex.IsMatch(Environment.OSVersion.VersionString, "Windows NT 6.1.760[01]"))
            {
                MessageBox.Show("This version of Windows is not supported. Program will exit.", "Windows 7 Logon Screen Customizer", MessageBoxButton.OK, MessageBoxImage.Warning);

                Application.Current.Shutdown();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            RegistryHelper.WindowsSystemPolicyRegistryKey.Close();
            RegistryHelper.WindowsSystemPolicyCurrentVersionRegistryKey.Close();
            RegistryHelper.WindowsLogonUIRegistryKey.Close();
        }
    }
}
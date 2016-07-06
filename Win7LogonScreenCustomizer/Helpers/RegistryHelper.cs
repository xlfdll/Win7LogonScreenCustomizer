using System;

using Microsoft.Win32;

namespace Win7LogonScreenCustomizer
{
    public static class RegistryHelper
    {
        static RegistryHelper()
        {
            RegistryHelper.WindowsSystemPolicyRegistryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System", true);
            RegistryHelper.WindowsSystemPolicyCurrentVersionRegistryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", true);
            RegistryHelper.WindowsLogonUIRegistryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI", true);
        }

        public static RegistryKey WindowsSystemPolicyRegistryKey { get; private set; }
        public static RegistryKey WindowsSystemPolicyCurrentVersionRegistryKey { get; private set; }
        public static RegistryKey WindowsLogonUIRegistryKey { get; private set; }

        #region Registry Key Names

        public static readonly String BackgroundRegistryKeyName = "UseOEMBackground";
        public static readonly String ButtonStyleRegistryKeyName = "ButtonSet";

        public static readonly String MessageCaptionRegistryKeyName = "legalnoticecaption";
        public static readonly String MessageTextRegistryKeyName = "legalnoticetext";
        public static readonly String DisplayLastLogonInfoRegistryKeyName = "DisplayLastLogonInfo";

        #endregion
    }
}
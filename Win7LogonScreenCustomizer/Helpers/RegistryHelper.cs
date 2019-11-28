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

        public const String BackgroundRegistryKeyName = "UseOEMBackground";
        public const String ButtonStyleRegistryKeyName = "ButtonSet";

        public const String MessageCaptionRegistryKeyName = "legalnoticecaption";
        public const String MessageTextRegistryKeyName = "legalnoticetext";
        public const String DisplayLastLogonInfoRegistryKeyName = "DisplayLastLogonInfo";

        #endregion
    }
}
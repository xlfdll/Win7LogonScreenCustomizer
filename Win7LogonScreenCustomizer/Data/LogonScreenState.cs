using System;
using System.Windows;

using Microsoft.Win32;

namespace Win7LogonScreenCustomizer
{
    public class LogonScreenState : ObservableObject
    {
        public LogonScreenState()
        {
            LogonScreenState.Current = this;
        }

        private LogonBackgroundInfo _backgroundInfo;

        public LogonBackgroundInfo BackgroundInfo
        {
            get
            {
                return _backgroundInfo;
            }
            set
            {
                _backgroundInfo = value;

                OnPropertyChanged("BackgroundInfo");
            }
        }

        public Boolean IsCustomBackgroundEnabled
        {
            get
            {
                return Convert.ToBoolean(RegistryHelper.WindowsSystemPolicyRegistryKey.GetValue(RegistryHelper.BackgroundRegistryKeyName, 0));
            }
            set
            {
                RegistryHelper.WindowsSystemPolicyRegistryKey.SetValue(RegistryHelper.BackgroundRegistryKeyName, Convert.ToInt32(value), RegistryValueKind.DWord);

                OnPropertyChanged("IsCustomBackgroundEnabled");
            }
        }
        public Int32 ButtonStyle
        {
            get
            {
                return (Int32)RegistryHelper.WindowsLogonUIRegistryKey.GetValue(RegistryHelper.ButtonStyleRegistryKeyName, 0);
            }
            set
            {
                RegistryHelper.WindowsLogonUIRegistryKey.SetValue(RegistryHelper.ButtonStyleRegistryKeyName, value, RegistryValueKind.DWord);

                OnPropertyChanged("ButtonStyle");
            }
        }
        public String MessageCaption
        {
            get
            {
                return RegistryHelper.WindowsSystemPolicyCurrentVersionRegistryKey.GetValue(RegistryHelper.MessageCaptionRegistryKeyName, String.Empty).ToString();
            }
            set
            {
                RegistryHelper.WindowsSystemPolicyCurrentVersionRegistryKey.SetValue(RegistryHelper.MessageCaptionRegistryKeyName, value, RegistryValueKind.String);

                OnPropertyChanged("MessageCaption");
            }
        }
        public String MessageText
        {
            get
            {
                return RegistryHelper.WindowsSystemPolicyCurrentVersionRegistryKey.GetValue(RegistryHelper.MessageTextRegistryKeyName, String.Empty).ToString();
            }
            set
            {
                RegistryHelper.WindowsSystemPolicyCurrentVersionRegistryKey.SetValue(RegistryHelper.MessageTextRegistryKeyName, value, RegistryValueKind.String);

                OnPropertyChanged("MessageText");
            }
        }
        public Boolean DisplayLastLogonInfo
        {
            get
            {
                return Convert.ToBoolean(RegistryHelper.WindowsSystemPolicyCurrentVersionRegistryKey.GetValue(RegistryHelper.DisplayLastLogonInfoRegistryKeyName, 0));
            }
            set
            {
                RegistryHelper.WindowsSystemPolicyCurrentVersionRegistryKey.SetValue(RegistryHelper.DisplayLastLogonInfoRegistryKeyName, Convert.ToInt32(value), RegistryValueKind.DWord);

                OnPropertyChanged("DisplayLastLogonInfo");
            }
        }

        public String ScreenResolution
        {
            get
            {
                PresentationSource presentationSource = PresentationSource.FromVisual(Application.Current.MainWindow);

                if (presentationSource != null)
                {
                    Int32 screenWidth = Convert.ToInt32(SystemParameters.PrimaryScreenWidth * presentationSource.CompositionTarget.TransformToDevice.M11);
                    Int32 screenHeight = Convert.ToInt32(SystemParameters.PrimaryScreenHeight * presentationSource.CompositionTarget.TransformToDevice.M22);

                    return screenWidth.ToString() + " * " + screenHeight.ToString();
                }

                return null;
            }
        }

        public static LogonScreenState Current { get; private set; }
    }
}
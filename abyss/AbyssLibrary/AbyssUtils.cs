using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbyssLibrary
{
    public static class AbyssUtils
    {
        public static string AbyssCommonDirectory
        {
            get
            {
                
                return Path.Combine(
                    System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
                    "Abyss");
            }
        }

        public static string AbyssSoundDirectory
        {
            get
            {
                return Path.Combine(AbyssCommonDirectory, "Sounds");
            }
        }

        private const string Browser64BitRegKey = @"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
        private const string Browser32BitRegKey = @"SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
        public static void CreateBrowserCompatibilityRegistryKey()
        {
            try
            {
                // https://msdn.microsoft.com/library/ee330730(v=vs.85).aspx
                Microsoft.Win32.RegistryKey key;
            
                key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(Browser32BitRegKey,
                    Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);
                string[] names = key.GetValueNames();

                key.SetValue("AbyssConsole.exe", 0x2AF9, Microsoft.Win32.RegistryValueKind.DWord);
                key.SetValue("devenv.exe", 0x2AF9, Microsoft.Win32.RegistryValueKind.DWord);
                key.Close();

                key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(Browser64BitRegKey, 
                    Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);
                key.SetValue("AbyssConsole.exe", 0x2AF9, Microsoft.Win32.RegistryValueKind.DWord);
                key.SetValue("devenv.exe", 0x2AF9, Microsoft.Win32.RegistryValueKind.DWord);
                key.Close();
            }
            catch
            {
                // ignore if this fails.
            }
        }

        public static string TruncateLongString(this string str, int maxLength)
        {
            return str.Substring(0, Math.Min(str.Length, maxLength));
        }
    }
}

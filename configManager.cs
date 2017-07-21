using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;

//            string loginName = configManager.ProfileReadValue("User", "loginname"); 
//                configManager.ProfileWriteValue("User", "loginname", Program.gUsername);
namespace CheckRenewalPkg
{
    public static class configManager
    {
        [System.Runtime.InteropServices.DllImport("kernel32")] // 写入配置文件的接口
        private static extern long WritePrivateProfileString(
        string section, string key, string val, string filePath);
        [System.Runtime.InteropServices.DllImport("kernel32")] // 读取配置文件的接口
        private static extern int GetPrivateProfileString(
        string section, string key, string def,
        StringBuilder retVal, int size, string filePath);
        // 向配置文件写入值
        public static void ProfileWriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, Program.configFilePath);
        }
        // 读取配置文件的值

        public static List<string> GetValueStrList(string section, string key)
        {
            List<string> result = new List<string> { };
            string value = ProfileReadValue(section, key);
            foreach( string s  in  value.Trim().Split(','))
            {
                result.Add(s.Trim());
            }
            return result;

        }


        public static string ProfileReadValue(string section, string key)
        {
            StringBuilder sb = new StringBuilder(1000);
            GetPrivateProfileString(section, key, "", sb, 1000, Program.configFilePath);
            return sb.ToString().Trim();
        }
        public static void SaveConfig(string key, string value)
        {
            // 写入参数设置
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save();

            // 重新读取参数
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}

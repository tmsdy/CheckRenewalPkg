﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Net;
using System.IO;
using System.Reflection;
using AutoUpdaterDotNET;
namespace CheckRenewalPkg
{
    static class Program
    {
        public static string sVer = Application.ProductVersion;
        public static string UserId;
        public static CookieContainer MLBCookie = new CookieContainer();
        public static string sGloableDomailUrl = "http://demo.ali-sim.com";  
        public static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.3; .NET4.0C; .NET4.0E";
        //public static readonly string DefaultUserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
        public static readonly string DefaultAccept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        public static string configFilePath = "";
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        { 

            Program.configFilePath = Application.StartupPath + @"\crp_config.ini";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string updateurl = "http://www.gpspax.com/apkdownload/AutoUpdater-crp.xml";
            if (!string.IsNullOrEmpty(updateurl))
            {

                AutoUpdater.Start(updateurl);
            }   



            Application.Run(new LoginForm());

            //Application.Run(new UpdateSims());
        }
    }
}

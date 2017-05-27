using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Net.Security;
using System.IO;


namespace CheckRenewalPkg
{
    public partial class UpdateSims : Form
    {
        public static Encoding RequestEncoding = _Encoding.UTF8;
        public static Encoding ResponseEncoding = _Encoding.UTF8;
        public UpdateSims()
        {
            InitializeComponent();
        }
        public struct _Encoding
        {
            public static Encoding UTF8;
            public static Encoding GB2312;
            public static Encoding ASCII;
            static _Encoding()
            {
                UTF8 = Encoding.UTF8;
                GB2312 = Encoding.GetEncoding("gb2312");
                ASCII = Encoding.ASCII;
            }
        }
        public static string CreatePostHttpResponse(byte[] data, string url, int? timeout, string userAgent, string cookies, WebProxy wp)
        {

            Stream responseStream;

            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException("url");
                }
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                request.KeepAlive = false;
                request.UserAgent = Program.DefaultUserAgent;
                request.CookieContainer = new CookieContainer();
                request.CookieContainer = Program.MLBCookie;
                request.Timeout = 300000;
                request.ReadWriteTimeout = 50000;
                if (!string.IsNullOrEmpty(userAgent))
                {
                    request.UserAgent = userAgent;
                }
                if (timeout.HasValue)
                {
                    request.Timeout = timeout.Value;
                }
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                try
                {
                    responseStream = request.GetResponse().GetResponseStream();
                }
                catch (Exception exception)
                {
                    return "";
                }
                string str = "";
                using (StreamReader reader = new StreamReader(responseStream, ResponseEncoding))
                {
                    str = reader.ReadToEnd();
                }
                responseStream.Close();
                return str;

            }
            catch (WebException webEx)
            {
                System.Diagnostics.Debug.WriteLine(webEx.Message.ToString() + " \r\n");
                return null;

            }

        }
        public static string PostDataToUrl(string data, string url)
        {
            return PostResponseSafe(RequestEncoding.GetBytes(data), url);
        }
        public static string PostResponseSafe(byte[] data, string url)
        {
           
            string result = CreatePostHttpResponse(  data, url, null, null, null, null);

 
            return result;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            string post = "{\"iccids\":[\"";
            for (int i = 0; i < richTextBox1.Lines.Count(); i++)
            {
                post += richTextBox1.Lines[i].Trim();
                if (i == richTextBox1.Lines.Count() - 1)
                    post += "\"]}";
                else
                    post += "\",\"";
            }
            richTextBox2.AppendText(PostDataToUrl(post, "http://demo.m-m10010.com/api/BatchUpdateTerminalUsageByICCIDs") + "\r\n");
            //this.button1.Enabled = false;
            //backgroundWorker1.RunWorkerAsync(); 
        }

        private void UpdateSims_Load(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string date = "";
            List< string> dit_status = new List< string>();

            for (int i = 0; i < 12; i++)
            {
                date = dt.AddMonths(-i).ToString("yyyy-MM");
                dit_status.Add(date+"-01");
            }
            this.comboBox1.DataSource = dit_status;
        }

        private void UpdateSims_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

 

        private void button2_Click(object sender, EventArgs e)
        {
            string post = "{\"iccids\":[\"";
            for (int i = 0; i < richTextBox1.Lines.Count(); i++)
            {
                post += richTextBox1.Lines[i].Trim();
                if (i == richTextBox1.Lines.Count() - 1)
                    post += "\"]";
                else
                    post += "\",\"";
            }
            post += ",\"statMonth\":\"" + this.comboBox1.Text + "\"}";
            //richTextBox2.AppendText(post + "\r\n");
            richTextBox2.AppendText(PostDataToUrl(post, "http://demo.m-m10010.com/api/UpdateTerminalsMonthUsage") + "\r\n");
        }

        //{"iccids":["123","234"]}

        //http://demo.m-m10010.com/api/BatchUpdateTerminalUsageByICCIDs
        //simIds[]=1110519&simIds[]=1110621&simIds[]=1110738&simIds[]=1110757&simIds[]=1110861&simIds[]=1110867&simIds[]=1110888&simIds[]=1110979&simIds[]=1110985
    }
}

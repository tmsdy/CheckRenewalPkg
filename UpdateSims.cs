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
using System.Diagnostics;


namespace CheckRenewalPkg
{
    public partial class UpdateSims : Form
    {
        Form1 parentForm;
        public static Encoding RequestEncoding = _Encoding.UTF8;
        public static Encoding ResponseEncoding = _Encoding.UTF8;
        public UpdateSims( )
        {
            //parentForm = f;
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
                request.CookieContainer = new CookieContainer();
                request.CookieContainer = Program.MLBCookie;
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

            //string post = "{\"iccids\":[\"";
            //for (int i = 0; i < richTextBox1.Lines.Count(); i++)
            //{
            //    post += richTextBox1.Lines[i].Trim();
            //    if (i == richTextBox1.Lines.Count() - 1)
            //        post += "\"]}";
            //    else
            //        post += "\",\"";
            //}
            //richTextBox2.AppendText(PostDataToUrl(post,,Program.sGloableDomailUrl + "/api/BatchUpdateTerminalUsageByICCIDs") + "\r\n");
            this.button1.Enabled = false;
            backgroundWorker1.RunWorkerAsync(); 
        }

        private void UpdateSims_Load(object sender, EventArgs e)
        {
            this.richTextBox1.MaxLength = 9999999;

            this.Text += Program.sVer;
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] str = InvokeHelper.Get(this.richTextBox1, "Text").ToString().Trim().Replace("\r\n\r\n", "\r\n").Replace("\r\n\r\n", "\r\n").Split('\n');
            string result = "";
            StringBuilder postdata = new StringBuilder();
            int count = str.Count();
            int updateRatePlanLimitCount = 50;
            int totalTimes = (int)Math.Ceiling((double)count / updateRatePlanLimitCount);
            int i = 0;
            for (int times = 0; times < totalTimes; times++)
            {

                InvokeHelper.Set(button1, "Text", (times + 1).ToString() + "/" + totalTimes.ToString()); 
                //richTextBox2.AppendText((times + 1).ToString() + "/" + totalTimes.ToString() + "\r\n");
                postdata.Clear();
                 postdata.Append("{\"iccids\":[\"" );
                for (i = 0; (i < updateRatePlanLimitCount) && (times * updateRatePlanLimitCount + i < count); i++)
                {
                    postdata.Append(str[times * updateRatePlanLimitCount + i].Trim());
                    if ((times * updateRatePlanLimitCount + i == count - 1) || (i == updateRatePlanLimitCount - 1))
                    {
                        postdata.Append("\"]}");
                    }
                    else
                    {
                        postdata.Append("\",\"");
                    }

                }
                try
                {
                    InvokeHelper.Set(richTextBox2, "Text", InvokeHelper.Get(this.richTextBox2, "Text").ToString() + PostDataToUrl(postdata.ToString(), Program.sGloableDomailUrl +"/api/BatchUpdateTerminalUsageByICCIDs") + "\r\n");

                }
                catch
                {
                    InvokeHelper.Set(richTextBox2, "Text", InvokeHelper.Get(this.richTextBox2, "Text").ToString() + "异常\r\n");

                }
            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button1.Enabled = true;
            InvokeHelper.Set(button1, "Text", "卡同步");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.button3.Enabled = false;
            backgroundWorker2.RunWorkerAsync(); 
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] str = InvokeHelper.Get(this.richTextBox1, "Text").ToString().Trim().Replace("\r\n\r\n", "\r\n").Replace("\r\n\r\n", "\r\n").Split('\n');
            string result = "";
            StringBuilder postdata = new StringBuilder();
            int count = str.Count();
            int updateRatePlanLimitCount = 100;
            int totalTimes = (int)Math.Ceiling((double)count / updateRatePlanLimitCount);
            int i = 0;

            Stopwatch sw = new Stopwatch();
            for (int times = 0; times < totalTimes; times++)
            {
                sw.Reset();
                sw.Start();
                InvokeHelper.Set(button3, "Text", (times + 1).ToString() + "/" + totalTimes.ToString());
                //richTextBox2.AppendText((times + 1).ToString() + "/" + totalTimes.ToString() + "\r\n");
                postdata.Clear();
                postdata.Append("{\"p\":1,\"pRowCount\":\"25\",\"storeState\":\"all\",\"loginHoldId\":\"89\",\"noChild\":0,\"key\":\"\",\"groupHoldId\":0,\"batchType\":\"2\",\"batchCardStr\":\"");
                for (i = 0; (i < updateRatePlanLimitCount) && (times * updateRatePlanLimitCount + i < count); i++)
                {
                    postdata.Append(str[times * updateRatePlanLimitCount + i].Trim());
                    postdata.Append("\n");
  

                }
                postdata.Append("\",\"batchStart\":\"\",\"batchEnd\":\"\",\"batchNumber\":0,\"date\":\"201704\",\"cost\":\"0\"}");
                try
                {                    
                    //结束计时  
                    sw.Stop();
                    //获取运行时间间隔  
                    TimeSpan ts = sw.Elapsed;
                    InvokeHelper.Set(richTextBox2, "Text", InvokeHelper.Get(this.richTextBox2, "Text").ToString() + PostDataToUrl(postdata.ToString(),Program.sGloableDomailUrl +  "/api/YDSimBill") );
                    InvokeHelper.Set(richTextBox2, "Text", InvokeHelper.Get(this.richTextBox2, "Text").ToString() + " 耗时" + ts.TotalSeconds + "秒\r\n");
                }
                catch
                {
                    InvokeHelper.Set(richTextBox2, "Text", InvokeHelper.Get(this.richTextBox2, "Text").ToString() + "异常\r\n");

                }
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button3.Enabled = true;
            InvokeHelper.Set(button3, "Text", "预扣费");
        }
         
    }
}

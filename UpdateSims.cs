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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;


namespace CheckRenewalPkg
{
    public partial class UpdateSims : Form
    {

        Dictionary<string, string> LTCustomerIdList = new Dictionary<string, string>();
        Dictionary<string, string> LTSimIdList = new Dictionary<string, string>();
        Form1 parentForm;
        public static Encoding RequestEncoding = _Encoding.UTF8;
        public static Encoding ResponseEncoding = _Encoding.UTF8;
        public UpdateSims(Form1 f)
        {
             parentForm = f;
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

 
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, string cookies, WebProxy wp)
        {



            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException("url");
                }
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
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
                request.CookieContainer = new CookieContainer();
                request.CookieContainer = Program.MLBCookie;

                if (wp != null)
                {
                    request.Proxy = wp;
                    //request.Proxy = new WebProxy("127.0.0.1:9666");
                }

                //request.Proxy = new WebProxy("127.0.0.1:9666");
                HttpWebResponse httpresponse = (HttpWebResponse)request.GetResponse();
                return httpresponse;

            }
            catch (WebException webEx)
            {
                System.Diagnostics.Debug.WriteLine(webEx.Message.ToString() + " \r\n");
                return null;

            }

        }
        public string GetResponseSafe(string url)
        {
            string result = "";
            HttpWebResponse webresponse_tmp = CreateGetHttpResponse(url, null, null, null, null);

            if (webresponse_tmp == null)
            {
                return result;
            }
            Stream sstream_tmp = webresponse_tmp.GetResponseStream();
            if (sstream_tmp == Stream.Null)
            {
                return result;
            }
            StreamReader reader = new StreamReader(sstream_tmp, Encoding.UTF8);

            try
            {
                result = reader.ReadToEnd();
            }
            catch (Exception e)
            {

                DisplayAndLog(e.Message.ToString() + " 失败\r\n", true);

            }
            webresponse_tmp.Dispose(); 
            sstream_tmp.Dispose(); 
            reader.Dispose(); 
            return result;
        }
        public static string CreatePostHttpResponse(byte[] data, string url, int? timeout, string userAgent, string cookies, string ContentType)
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
                if (string.IsNullOrEmpty(ContentType))
                {
                    request.ContentType = "application/json";
                }
                else
                {
                    request.ContentType = ContentType;
                }
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
        private void InitCustomerList()
        {
            LTCustomerIdList.Add("样机卡", "114");
            LTCustomerIdList.Add("PRE_1800M", "117");
            LTCustomerIdList.Add("5MB/月", "118");
            LTCustomerIdList.Add("30MB/月", "127");
            LTCustomerIdList.Add("麦谷测试卡", "149");
            LTCustomerIdList.Add("mg_test", "150");
            LTCustomerIdList.Add("S_50K", "313");
            LTCustomerIdList.Add("STOP", "119");
            LTCustomerIdList.Add("R_1M", "259");
            LTCustomerIdList.Add("R_2M", "260");
            LTCustomerIdList.Add("R_3M", "261");
            LTCustomerIdList.Add("R_4M", "262");
            LTCustomerIdList.Add("R_5M", "141");
            LTCustomerIdList.Add("R_6M", "263");
            LTCustomerIdList.Add("R_7M", "264");
            LTCustomerIdList.Add("R_8M", "265");
            LTCustomerIdList.Add("R_9M", "266");
            LTCustomerIdList.Add("R_10M", "98");
            LTCustomerIdList.Add("R_11M", "267");
            LTCustomerIdList.Add("R_12M", "96");
            LTCustomerIdList.Add("R_13M", "268");
            LTCustomerIdList.Add("R_14M", "269");
            LTCustomerIdList.Add("R_15M", "270");
            LTCustomerIdList.Add("R_16M", "271");
            LTCustomerIdList.Add("R_17M", "272");
            LTCustomerIdList.Add("R_18M", "273");
            LTCustomerIdList.Add("R_19M", "274");
            LTCustomerIdList.Add("R_20M", "240");
            LTCustomerIdList.Add("R_21M", "275");
            LTCustomerIdList.Add("R_22M", "276");
            LTCustomerIdList.Add("R_23M", "277");
            LTCustomerIdList.Add("R_24M", "278");
            LTCustomerIdList.Add("R_25M", "241");
            LTCustomerIdList.Add("R_26M", "279");
            LTCustomerIdList.Add("R_27M", "280");
            LTCustomerIdList.Add("R_28M", "281");
            LTCustomerIdList.Add("R_29M", "282");
            LTCustomerIdList.Add("R_30M", "109");
            LTCustomerIdList.Add("R_31M", "287");
            LTCustomerIdList.Add("R_32M", "288");
            LTCustomerIdList.Add("R_33M", "289");
            LTCustomerIdList.Add("R_34M", "290");
            LTCustomerIdList.Add("R_35M", "242");
            LTCustomerIdList.Add("R_36M", "291");
            LTCustomerIdList.Add("R_37M", "292");
            LTCustomerIdList.Add("R_38M", "293");
            LTCustomerIdList.Add("R_39M", "294");
            LTCustomerIdList.Add("R_40M", "243");
            LTCustomerIdList.Add("R_45M", "244");
            LTCustomerIdList.Add("R_50M", "232");
            LTCustomerIdList.Add("R_55M", "245");
            LTCustomerIdList.Add("R_60M", "246");
            LTCustomerIdList.Add("R_65M", "247");
            LTCustomerIdList.Add("R_70M", "233");
            LTCustomerIdList.Add("R_75M", "248");
            LTCustomerIdList.Add("R_80M", "249");
            LTCustomerIdList.Add("R_90M", "234");
            LTCustomerIdList.Add("S_100M", "153");
            LTCustomerIdList.Add("S_102M", "113");
            LTCustomerIdList.Add("R_120M", "318");
            LTCustomerIdList.Add("R_150M", "136");
            LTCustomerIdList.Add("R_170M", "95");
            LTCustomerIdList.Add("R_180M", "335");
            LTCustomerIdList.Add("R_200M", "183");
            LTCustomerIdList.Add("S_205M", "124");
            LTCustomerIdList.Add("R_250M", "151");
            LTCustomerIdList.Add("R_270M", "133");
            LTCustomerIdList.Add("R_300M", "137");
            LTCustomerIdList.Add("S_307M", "132");
            LTCustomerIdList.Add("R_330M", "336");
            LTCustomerIdList.Add("R_340M", "139");
            LTCustomerIdList.Add("R_370M", "144");
            LTCustomerIdList.Add("R_400M", "175");
            LTCustomerIdList.Add("S_410M", "123");
            LTCustomerIdList.Add("R_440M", "148");
            LTCustomerIdList.Add("R_470M", "138");
            LTCustomerIdList.Add("R_500M", "156");
            LTCustomerIdList.Add("S_500M", "120");
            LTCustomerIdList.Add("S_512M", "121");
            LTCustomerIdList.Add("R_540M", "147");
            LTCustomerIdList.Add("R_570M", "140");
            LTCustomerIdList.Add("R_600M", "176");
            LTCustomerIdList.Add("S_614M", "142");
            LTCustomerIdList.Add("R_700M", "181");
            LTCustomerIdList.Add("S_717M", "126");
            LTCustomerIdList.Add("R_800M", "180");
            LTCustomerIdList.Add("S_819M", "143");
            LTCustomerIdList.Add("R_900M", "182");
            LTCustomerIdList.Add("S_922M", "125");
            LTCustomerIdList.Add("R_1000M", "157");
            LTCustomerIdList.Add("S_1024M", "130");
            LTCustomerIdList.Add("R_1024M", "99");
            LTCustomerIdList.Add("R_1100M", "184");
            LTCustomerIdList.Add("S_1126M", "122");
            LTCustomerIdList.Add("R_1170M", "134");
            LTCustomerIdList.Add("R_1200M", "185");
            LTCustomerIdList.Add("S_1229M", "129");
            LTCustomerIdList.Add("R_1300M", "186");
            LTCustomerIdList.Add("S_1331M", "128");
            LTCustomerIdList.Add("R_1364M", "146");
            LTCustomerIdList.Add("R_1400M", "187");
            LTCustomerIdList.Add("S_1434M", "131");
            LTCustomerIdList.Add("R_1500M", "154");
            LTCustomerIdList.Add("S_1536M", "107");
            LTCustomerIdList.Add("R_1600M", "188");
            LTCustomerIdList.Add("S_1638M", "106");
            LTCustomerIdList.Add("R_1700M", "189");
            LTCustomerIdList.Add("S_1741M", "105");
            LTCustomerIdList.Add("R_1800M", "190");
            LTCustomerIdList.Add("S_1843M", "100");
            LTCustomerIdList.Add("R_1900M", "191");
            LTCustomerIdList.Add("S_1946M", "101");
            LTCustomerIdList.Add("R_2000M", "158");
            LTCustomerIdList.Add("R_2048M", "152");
            LTCustomerIdList.Add("S_2048M", "102");
            LTCustomerIdList.Add("R_2058M", "337");
            LTCustomerIdList.Add("R_2068M", "338");
            LTCustomerIdList.Add("R_2078M", "339");
            LTCustomerIdList.Add("R_2088M", "340");
            LTCustomerIdList.Add("R_2098M", "341");
            LTCustomerIdList.Add("R_2100M", "192");
            LTCustomerIdList.Add("R_2170M", "135");
            LTCustomerIdList.Add("R_2200M", "193");
            LTCustomerIdList.Add("R_2300M", "194");
            LTCustomerIdList.Add("R_2400M", "195");
            LTCustomerIdList.Add("R_2500M", "159");
            LTCustomerIdList.Add("R_2600M", "196");
            LTCustomerIdList.Add("R_2700M", "197");
            LTCustomerIdList.Add("R_2800M", "198");
            LTCustomerIdList.Add("R_2900M", "199");
            LTCustomerIdList.Add("R_3000M", "160");
            LTCustomerIdList.Add("R_3072M", "97");
            LTCustomerIdList.Add("R_3100M", "200");
            LTCustomerIdList.Add("R_3200M", "201");
            LTCustomerIdList.Add("R_3300M", "202");
            LTCustomerIdList.Add("R_3400M", "203");
            LTCustomerIdList.Add("R_3500M", "161");
            LTCustomerIdList.Add("R_3600M", "204");
            LTCustomerIdList.Add("R_3700M", "205");
            LTCustomerIdList.Add("R_3800M", "206");
            LTCustomerIdList.Add("R_3900M", "207");
            LTCustomerIdList.Add("R_4000M", "162");
            LTCustomerIdList.Add("R_4096M", "145");
            LTCustomerIdList.Add("R_4100M", "208");
            LTCustomerIdList.Add("R_4200M", "209");
            LTCustomerIdList.Add("R_4300M", "210");
            LTCustomerIdList.Add("R_4400M", "211");
            LTCustomerIdList.Add("R_4500M", "163");
            LTCustomerIdList.Add("R_4600M", "212");
            LTCustomerIdList.Add("R_4700M", "213");
            LTCustomerIdList.Add("R_4800M", "214");
            LTCustomerIdList.Add("R_4900M", "215");
            LTCustomerIdList.Add("R_5000M", "164");
            LTCustomerIdList.Add("R_5500M", "155");
            LTCustomerIdList.Add("R_6000M", "165");
            LTCustomerIdList.Add("R_6500M", "166");
            LTCustomerIdList.Add("R_7000M", "167");
            LTCustomerIdList.Add("R_7500M", "168");
            LTCustomerIdList.Add("R_8000M", "169");
            LTCustomerIdList.Add("R_8500M", "170");
            LTCustomerIdList.Add("R_9000M", "171");
            LTCustomerIdList.Add("R_9500M", "173");
            LTCustomerIdList.Add("R_10000M", "174");
            LTCustomerIdList.Add("R_11000M", "177");
            LTCustomerIdList.Add("R_12000M", "178");
            LTCustomerIdList.Add("R_13000M", "179");
            LTCustomerIdList.Add("R_14000M", "312");
            LTCustomerIdList.Add("R_15000M", "218");
            LTCustomerIdList.Add("R_16000M", "295");
            LTCustomerIdList.Add("R_17000M", "219");
            LTCustomerIdList.Add("R_18000M", "220");
            LTCustomerIdList.Add("R_19000M", "252");
            LTCustomerIdList.Add("R_20000M", "217");
            LTCustomerIdList.Add("R_21000M", "296");
            LTCustomerIdList.Add("R_22000M", "297");
            LTCustomerIdList.Add("R_23000M", "298");
            LTCustomerIdList.Add("R_24000M", "231");
            LTCustomerIdList.Add("R_24500M", "311");
            LTCustomerIdList.Add("R_25000M", "221");
            LTCustomerIdList.Add("R_26000M", "299");
            LTCustomerIdList.Add("R_27000M", "300");
            LTCustomerIdList.Add("R_28000M", "301");
            LTCustomerIdList.Add("R_29000M", "302");
            LTCustomerIdList.Add("R_30000M", "222");
            LTCustomerIdList.Add("R_31000M", "303");
            LTCustomerIdList.Add("R_32000M", "304");
            LTCustomerIdList.Add("R_33000M", "305");
            LTCustomerIdList.Add("R_34000M", "306");
            LTCustomerIdList.Add("R_35000M", "223");
            LTCustomerIdList.Add("R_36000M", "314");
            LTCustomerIdList.Add("R_37000M", "315");
            LTCustomerIdList.Add("R_38000M", "316");
            LTCustomerIdList.Add("R_39000M", "317");
            LTCustomerIdList.Add("R_40000M", "224");
            LTCustomerIdList.Add("R_41000M", "319");
            LTCustomerIdList.Add("R_42000M", "320");
            LTCustomerIdList.Add("R_43000M", "321");
            LTCustomerIdList.Add("R_44000M", "322");
            LTCustomerIdList.Add("R_45000M", "226");
            LTCustomerIdList.Add("R_46000M", "323");
            LTCustomerIdList.Add("R_47000M", "324");
            LTCustomerIdList.Add("R_48000M", "325");
            LTCustomerIdList.Add("R_49000M", "326");
            LTCustomerIdList.Add("R_50000M", "228");
            LTCustomerIdList.Add("R_51000M", "307");
            LTCustomerIdList.Add("R_52000M", "308");
            LTCustomerIdList.Add("R_53000M", "309");
            LTCustomerIdList.Add("R_54000M", "310");
            LTCustomerIdList.Add("R_55000M", "229");
            LTCustomerIdList.Add("R_56000M", "327");
            LTCustomerIdList.Add("R_57000M", "328");
            LTCustomerIdList.Add("R_58000M", "329");
            LTCustomerIdList.Add("R_59000M", "330");
            LTCustomerIdList.Add("R_60000M", "230");
            LTCustomerIdList.Add("R_61000M", "331");
            LTCustomerIdList.Add("R_62000M", "332");
            LTCustomerIdList.Add("R_63000M", "333");
            LTCustomerIdList.Add("R_64000M", "225");
            LTCustomerIdList.Add("R_70000M", "235");
            LTCustomerIdList.Add("R_80000M", "236");
            LTCustomerIdList.Add("R_85000M", "334");
            LTCustomerIdList.Add("R_90000M", "237");
            LTCustomerIdList.Add("R_100000M", "238");
            LTCustomerIdList.Add("R_110000M", "239");
            LTCustomerIdList.Add("R_115000M", "254");
            LTCustomerIdList.Add("R_120000M", "253");
            LTCustomerIdList.Add("R_125000M", "255");
            LTCustomerIdList.Add("R_130000M", "256");
            LTCustomerIdList.Add("R_135000M", "257");
            LTCustomerIdList.Add("R_140000M", "258");
            LTCustomerIdList.Add("R_160000M", "283");
            LTCustomerIdList.Add("R_180000M", "285");
            LTCustomerIdList.Add("R_190000M", "286");
            LTCustomerIdList.Add("R_200000M", "284");
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

            InitCustomerList();
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
            richTextBox2.AppendText(PostDataToUrl(post, Program.sGloableDomailUrl + "/api/UpdateTerminalsMonthUsage") + "\r\n");
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
                    InvokeHelper.Set(richTextBox2, "Text", InvokeHelper.Get(this.richTextBox2, "Text").ToString() + PostDataToUrl(postdata.ToString(), Program.sGloableDomailUrl + "/api/YDSimBill"));
                    //结束计时  
                    sw.Stop();
                    //获取运行时间间隔  
                    TimeSpan ts = sw.Elapsed;
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

        private void button4_Click(object sender, EventArgs e)
        {
            string str = this.richTextBox1.Text.Trim();
            if(str.Split(',').Count()<= str.Split('\n').Count())
            {
                MessageBox.Show("卡号和用量之间用英文逗号(,)隔开");
                return;
            }
     
            this.button4.Enabled = false;
            backgroundWorker3.RunWorkerAsync(); 
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            string iccid = "";
            string usage = "";
            string[] str = InvokeHelper.Get(this.richTextBox1, "Text").ToString().Trim().Replace("\r\n\r\n", "\r\n").Replace("\r\n\r\n", "\r\n").Split('\n');
            string result = "";
            StringBuilder postdata = new StringBuilder();
            int count = str.Count();
            for(int i=0;i<count;i++)
            {
                iccid = str[i].Split(',')[0].Trim();
                usage = str[i].Split(',')[1].Trim();
                if (Regex.Matches(usage, "[a-zA-Z]").Count > 0)
                {
                    DisplayAndLog(str[i] + "\t" +   "用量不要带字母\r\n", true);
                    continue;
                }
                postdata.Clear();
                postdata.Append("txtICCID=" + str[i].Split(',')[0].Trim() + "&txtAmountUsage=" + str[i].Split(',')[1].Trim());
                try
                {
                    InvokeHelper.Set(button4, "Text", i.ToString() + "/" + count.ToString());
                    result = CreatePostHttpResponse(RequestEncoding.GetBytes(postdata.ToString()), Program.sGloableDomailUrl + "/SysSetting/SetTerminalAmountUsage", null, null, null, "application/x-www-form-urlencoded");
                    DisplayAndLog(str[i] + "\t" + result.Substring(0, 20) + "\r\n", true);

                }
                catch
                {
                    DisplayAndLog(str[i] + "\t" +  "修改异常\r\n", true);

                }
            }
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button4.Enabled = true;
            this.button4.Text = "周期用量";
        }

        public int GetSimidFromSims(string[] simslist)
        {
            string simid = "";
            string sim = "";
            string url = Program.sGloableDomailUrl + "/api/YDSimListFire/Search";

            if (simslist == null || simslist.Count() <= 0)
                return -1;
            foreach (string a in simslist)
            {
                sim += a.Split(',')[0].Trim() + "%0A";
            }
            string postdata = "p=1&pRowCount=99999&loginHoldId=1&key=&noChild=0&groupHoldId=0&batchType=2&batchCardStr=" + sim;
            string response = CreatePostHttpResponse(RequestEncoding.GetBytes(postdata.ToString()), url, null, null, null, "application/x-www-form-urlencoded");
            if (response == "")
            {
                DisplayAndLog("查询卡失败\r\n", true);
                return -2;
            }
            try
            {

                //jo1是整个返回值
                JObject jo1 = (JObject)JsonConvert.DeserializeObject(response);
                //array是三个数组，分别是 汇总信息，卡列表，查询条件
                ////var listData = data.result[1];
                ////var page = data.result[0];
                ////var hid_querySqlwhereKey = data.result[2].card_query_sqlwhere;
                var strsimlist = jo1.GetValue("result")[1];
                string strsimlistjson = "{\"result\": " + strsimlist.ToString() + "}";
                ParamDefine.SearchSimListRoot ssldroot = JsonConvert.DeserializeObject<ParamDefine.SearchSimListRoot>(strsimlistjson);
                if (ssldroot == null || ssldroot.result == null || ssldroot.result.Count <= 0)
                    return -3;
                foreach (ParamDefine.SearchSimListDetail ssld in ssldroot.result)
                {
                    try
                    {
                        LTSimIdList.Add(ssld.sim, ssld.simId.Split('.')[0]);
                    }
                    catch
                    {
                        continue;
                    }
                }


                //JToken[] array = jo1.GetValue("result").ToArray();
                ////取第1个卡
                //string simlist = array[1].ToString();
                //ParamDefine.SearchSimListRoot sslds = JsonConvert.DeserializeObject<ParamDefine.SearchSimListRoot>(simlist);
                //if (sslds == null || sslds.result == null || sslds.result.Count <= 0)
                //    return -3;
                //foreach(ParamDefine.SearchSimListDetail ssld in sslds.result)
                //{
                //    LTSimIdList.Add(ssld.guid, ssld.simId.Split('.')[0]);
                //}

                return 0;
            }
            catch (Exception e)
            {
                DisplayAndLog(e.ToString() + "\r\n", true);
                return -4;
            }
        }
        public int GetSimidFromIccids( string[] iccidlist)
        {
            string simid = "";
            string iccid = "";
            string url = Program.sGloableDomailUrl + "/api/SimListFire/Search";

            if (iccidlist == null || iccidlist.Count() <= 0)
                return -1;
            foreach(string a in iccidlist)
            {
                iccid += a.Split(',')[0].Trim() + "%0A";
            }
            string postdata = "p=1&pRowCount=99999&loginHoldId=1&key=&noChild=0&groupHoldId=0&batchType=1&batchCardStr=" + iccid  ;
            string response =  CreatePostHttpResponse(RequestEncoding.GetBytes(postdata.ToString()), url , null, null, null, "application/x-www-form-urlencoded");
            if (response == "")
            {
                DisplayAndLog("查询卡失败\r\n", true);
                return -2;
            }
            try
            {

                //jo1是整个返回值
                JObject jo1 = (JObject)JsonConvert.DeserializeObject(response);
                //array是三个数组，分别是 汇总信息，卡列表，查询条件
                ////var listData = data.result[1];
                ////var page = data.result[0];
                ////var hid_querySqlwhereKey = data.result[2].card_query_sqlwhere;
                var strsimlist = jo1.GetValue("result")[1];
                string strsimlistjson = "{\"result\": " + strsimlist.ToString() + "}";
                ParamDefine.SearchSimListRoot ssldroot = JsonConvert.DeserializeObject<ParamDefine.SearchSimListRoot>(strsimlistjson);
                if (ssldroot == null || ssldroot.result == null || ssldroot.result.Count <= 0)
                    return -3;
                foreach (ParamDefine.SearchSimListDetail ssld in ssldroot.result)
                {
                    try
                    {
                        LTSimIdList.Add(ssld.guid, ssld.simId.Split('.')[0]);
                    }
                    catch
                    {
                        continue;
                    }
                }


                //JToken[] array = jo1.GetValue("result").ToArray();
                ////取第1个卡
                //string simlist = array[1].ToString();
                //ParamDefine.SearchSimListRoot sslds = JsonConvert.DeserializeObject<ParamDefine.SearchSimListRoot>(simlist);
                //if (sslds == null || sslds.result == null || sslds.result.Count <= 0)
                //    return -3;
                //foreach(ParamDefine.SearchSimListDetail ssld in sslds.result)
                //{
                //    LTSimIdList.Add(ssld.guid, ssld.simId.Split('.')[0]);
                //}

                return 0;
            }
            catch (Exception e)
            {
                DisplayAndLog(e.ToString() + "\r\n", true);
                return -4;
            }
        }
        private delegate void SetTextHandler(string text );
        public void SetText(string text)
        {
            if (richTextBox1.InvokeRequired == true)
            {
                SetTextHandler set = new SetTextHandler(SetText);//委托的方法参数应和SetText一致
                richTextBox2.Invoke(set, new object[] { text }); //此方法第二参数用于传入方法,代替形参text
            }
            else
            {
                richTextBox2.AppendText(text);
                this.richTextBox2.ScrollToCaret();
            }
        }
        public void DisplayAndLog(string s, bool isDisplay)
        {

            if (isDisplay)
            {

                SetText(s);
                Application.DoEvents();
            } 
            parentForm.WriteLogFile(s);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.button5.Enabled = false;
            this.backgroundWorker4.RunWorkerAsync();
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 1;
            string iccid = "";
            string customerid = "";
            int result = 0;
            string[] str = InvokeHelper.Get(this.richTextBox1, "Text").ToString().Trim().Replace("\r\n\r\n", "\r\n").Replace("\r\n\r\n", "\r\n").Split('\n');
            if (0 != GetSimidFromIccids(str))
            {
                DisplayAndLog("获取SIMID失败\r\n", true);
                return;
            };
            foreach(string a in str)
            {
                InvokeHelper.Set(this.button5, "Text", (i++).ToString() + "/" + str.Count().ToString());
                if (string.IsNullOrEmpty(a))
                    continue;
                iccid = GetSimID(a.Split(',')[0].Trim());
                customerid = GetCustomerID(a.Split(',')[1].Trim());
                result = SetCustomerID(iccid, customerid);
                if (0 != result)
                {
                    DisplayAndLog(a + "\t" + result.ToString() + "\t修改失败\r\n", true);
                   
                }
                else
                {
                    DisplayAndLog(a + "\t修改成功\r\n", true);
                 
                }


            }

        }
        private int SetCustomerID(string iccid,string customerid)
        {
            if (string.IsNullOrEmpty(iccid) || string.IsNullOrEmpty(customerid))
            {
                return -1;
            }
                   
            if ( customerid.Trim() == "OK")
            {
                return 0;
            }
            string url = Program.sGloableDomailUrl + "/api/MonitorTestSetSimGroup";
            string postdata = "simIds%5B%5D=" + iccid + "&groupId=" + customerid;

            string response = CreatePostHttpResponse(RequestEncoding.GetBytes(postdata.ToString()), url, null, null, null, "application/x-www-form-urlencoded");
     
            if (response == "")
            {
               // DisplayAndLog("修改失败\r\n", true);
                return -2;
            }
            else
            {
               // DisplayAndLog(iccid + "\t" + customerid + "修改成功\r\n", true);
                return 0;
            }
        }
        private string GetSimID(string iccid)
        {
            try
            { 
                return LTSimIdList[iccid];
            }
            catch
            {
                return "";
            }
        }
        private string GetCustomerID(string customerid)
        {
            try
            {
                return LTCustomerIdList[customerid];
            }
            catch
            {
                return "";
            }
        }
        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button5.Text = "组别";
            this.button5.Enabled = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.button6.Enabled = false;
            this.backgroundWorker5.RunWorkerAsync();
        }

        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 1;
            string iccid = "";
            string exptime = "";
            int result = 0;
            string[] str = InvokeHelper.Get(this.richTextBox1, "Text").ToString().Trim().Replace("\r\n\r\n", "\r\n").Replace("\r\n\r\n", "\r\n").Split('\n');
            if (0 != GetSimidFromIccids(str))
            {
                DisplayAndLog("获取SIMID失败\r\n", true);
                return;
            };
            foreach (string a in str)
            {
                InvokeHelper.Set(this.button6, "Text", (i++).ToString() + "/" + str.Count().ToString());
                if (string.IsNullOrEmpty(a))
                    continue;
                iccid = GetSimID(a.Split(',')[0].Trim());
                exptime =  (a.Split(',')[1].Trim());
               
                result = SetExpTime(iccid, exptime);
                if (0 != result)
                {
                    DisplayAndLog(a + "\t" + result.ToString() + "\t修改失败\r\n", true);

                }
                else
                {
                    DisplayAndLog(a + "\t修改成功\r\n", true);
                    
                }


            }
        }
        private int SetExpTime(string iccid, string exptime)
        {
            if(string.IsNullOrEmpty(iccid)||string.IsNullOrEmpty(exptime) )
            {
                return -1;
            }

            string url = Program.sGloableDomailUrl + "/api/SetVExpireTime";
            string postdata = "ids%5B%5D=" + iccid + "&vExpireTime=" + exptime;
            //string postdata = "ids%5B%5D=" + iccid + "&vExpireTime=" + exptime.Replace(" ", "+").Replace(":", "%3A");

            string response = CreatePostHttpResponse(RequestEncoding.GetBytes(postdata.ToString()), url, null, null, null, "application/x-www-form-urlencoded");

            if (response == "")
            {
                // DisplayAndLog("修改失败\r\n", true);
                return -2;
            }
            else
            {
                // DisplayAndLog(iccid + "\t" + customerid + "修改成功\r\n", true);
                return 0;
            }
        }
        private void backgroundWorker5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button6.Text = "到期";
            this.button6.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.button7.Enabled = false;
            this.backgroundWorker6.RunWorkerAsync();
        }

        private void backgroundWorker6_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 1;
            string iccid = "";
            string movetoHoldid = "";
            int result = 0;
            string[] str = InvokeHelper.Get(this.richTextBox1, "Text").ToString().Trim().Replace("\r\n\r\n", "\r\n").Replace("\r\n\r\n", "\r\n").Split('\n');
            if (0 != GetSimidFromIccids(str))
            {
                DisplayAndLog("获取SIMID失败\r\n", true);
                return;
            };
            foreach (string a in str)
            {
                InvokeHelper.Set(this.button7, "Text", (i++).ToString() + "/" + str.Count().ToString());
                if (string.IsNullOrEmpty(a))
                    continue;
                iccid = GetSimID(a.Split(',')[0].Trim());
                movetoHoldid =  (a.Split(',')[1].Trim());
                result = Distribute(iccid, movetoHoldid);
                if (0 != result)
                {
                    DisplayAndLog(a + "\t" + result.ToString() + "\t修改失败\r\n", true);

                }
                else
                {
                    DisplayAndLog(a + "\t修改成功\r\n", true);

                }


            }

        }
        private int Distribute(string iccid,string movetoHoldid)
        {

            if (string.IsNullOrEmpty(iccid) || string.IsNullOrEmpty(movetoHoldid))
            {
                return -1;
            }
            string url = Program.sGloableDomailUrl + "/api/SimHandle/Distribute";
            string postdata = "operateCmd=move&simId%5B%5D=" + iccid + "&toHoldId=" + movetoHoldid + "&loginHoldId=1";

            string response = CreatePostHttpResponse(RequestEncoding.GetBytes(postdata.ToString()), url, null, null, null, "application/x-www-form-urlencoded");

            if (response == "")
            {
                // DisplayAndLog("修改失败\r\n", true);
                return -2;
            }
            else
            {
                // DisplayAndLog(iccid + "\t" + customerid + "修改成功\r\n", true);
                return 0;
            }
        }

        private void backgroundWorker6_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button7.Text = "分配";
            this.button7.Enabled = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.button8.Enabled = false;
            this.backgroundWorker7.RunWorkerAsync();
        }
        private int SetActiveState(string iccid, string activeState)
        {
            if (string.IsNullOrEmpty(iccid) || string.IsNullOrEmpty(activeState))
            {
                return -1;
            }

            string url = Program.sGloableDomailUrl + "/api/SetSimActiveState";
            string postdata = "simIds%5B%5D=" + iccid + "&activeState=" + activeState;
            //string postdata = "ids%5B%5D=" + iccid + "&vExpireTime=" + exptime.Replace(" ", "+").Replace(":", "%3A");

            string response = CreatePostHttpResponse(RequestEncoding.GetBytes(postdata.ToString()), url, null, null, null, "application/x-www-form-urlencoded; charset=UTF-8");

            if (response == "")
            {
                // DisplayAndLog("修改失败\r\n", true);
                return -2;
            }
            else
            {
                // DisplayAndLog(iccid + "\t" + customerid + "修改成功\r\n", true);
                return 0;
            }
        }
        private void backgroundWorker7_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 1;
            string iccid = "";
            string activeState = "1";
            int result = 0;
            string[] str = InvokeHelper.Get(this.richTextBox1, "Text").ToString().Trim().Replace("\r\n\r\n", "\r\n").Replace("\r\n\r\n", "\r\n").Split('\n');
            if (0 != GetSimidFromIccids(str))
            {
                DisplayAndLog("获取SIMID失败\r\n", true);
                return;
            };
            foreach (string a in str)
            {
                InvokeHelper.Set(this.button8, "Text", (i++).ToString() + "/" + str.Count().ToString());
                if (string.IsNullOrEmpty(a))
                    continue;
                iccid = GetSimID(a.Split(',')[0].Trim());


                result = SetActiveState(iccid, activeState);
                if (0 != result)
                {
                    DisplayAndLog(a + "\t" + result.ToString() + "\t修改失败\r\n", true);

                }
                else
                {
                    DisplayAndLog(a + "\t修改成功\r\n", true);

                }


            }
        }

        private void backgroundWorker7_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button8.Text = "待开通";
            this.button8.Enabled = true;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.button9.Enabled = false;
            this.backgroundWorker9.RunWorkerAsync();
        }
        private string QueryCMCCbill(string simid)
        {
            string billTime = "";
            string result = "";
            if (string.IsNullOrEmpty(simid) )
            {
                return " Simid为空\r\n";
            }
            //http://demo.m-m10010.com/api/YDSimBill?simId=8823346
            string url = Program.sGloableDomailUrl + "/api/YDSimBill?simId=" + simid;

            int i = 0;
            string response = GetResponseSafe(url);

            if (response == "")
            {
                return ("修改失败\r\n" );
              
            }
            else
            {
                 
                ParamDefine.QueryCMCCBillRoot qcbr = JsonConvert.DeserializeObject<ParamDefine.QueryCMCCBillRoot>(response);
                if ((qcbr.error == 1) || (qcbr.result == null) || (qcbr.result.Count == 0))
                    return "没有账单\r\n";

                foreach (ParamDefine.QueryCMCCBillRootResultItem qcbrt in qcbr.result)
                {
                    if (qcbrt.operation.IndexOf("续") >= 0)
                        continue;

                    i = String.Compare(billTime, qcbrt.billTime);
                    if (billTime == "" || i < 0)
                    {
                        billTime = qcbrt.billTime;
                        result = "\t" + qcbrt.billTime + "\t" + qcbrt.operation + "\t消费\t" + qcbrt.cost + "\t余额\t" + qcbrt.balance + "\t时间\t" + qcbrt.createTime + "\r\n";

                    }
                }
                return result;
            }

        }
        private void backgroundWorker9_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 1;
            string simid = ""; 
            string result = "";
            string[] str = InvokeHelper.Get(this.richTextBox1, "Text").ToString().Trim().Replace("\r\n\r\n", "\r\n").Replace("\r\n\r\n", "\r\n").Split('\n');
            if (0 != GetSimidFromSims(str))
            {
                DisplayAndLog("获取SIMID失败\r\n", true);
                return;
            };
            foreach (string a in str)
            {
                InvokeHelper.Set(this.button9, "Text", (i++).ToString() + "/" + str.Count().ToString());
                if (string.IsNullOrEmpty(a))
                    continue;
                simid = GetSimID(a.Split(',')[0].Trim());
                
                result = QueryCMCCbill(simid);
                 
                DisplayAndLog(a + result, true);
 


            }
        }

        private void backgroundWorker9_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.button9.Enabled = true;
        }
         
         
    }
}

//POST http://demo.m-m10010.com/api/SimHandle/Distribute HTTP/1.1
//Host: demo.m-m10010.com
//Connection: keep-alive
//Content-Length: 82
//Accept: application/json, text/javascript, */*; q=0.01
//Origin: http://demo.m-m10010.com
//X-Requested-With: XMLHttpRequest
//User-Agent: Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36
//Content-Type: application/x-www-form-urlencoded; charset=UTF-8
//Referer: http://demo.m-m10010.com/sim/distribute?cardType=1&batch=undefined&sqlKey=undefined&nums=2
//Accept-Encoding: gzip, deflate
//Accept-Language: zh,zh-CN;q=0.9,en;q=0.8,zh-TW;q=0.7
//Cookie: ASP.NET_SessionId=vbcw43x3awlzgrsuxlu1ump3; UserCookie=UserID=1&UserName=admin&UserType=1&HoldID=1&HoldName=%e8%bf%90%e8%90%a5%e4%b8%ad%e5%bf%83&HoldLevel=1&HoldType=4&Token=F1X2BX1LI5V3SDQC1XPOFXW7C9HBHWTN&LoginFromType=1&OEMClient=

//operateCmd=move&simId%5B%5D=367006&toHoldId=3828&loginHoldId=1
 
  
//POST http://demo.m-m10010.com/api/MonitorTestSetSimGroup HTTP/1.1
//Host: demo.m-m10010.com
//Connection: keep-alive
//Content-Length: 31
//Accept: application/json, text/javascript, */*; q=0.01
//Origin: http://demo.m-m10010.com
//X-Requested-With: XMLHttpRequest
//User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36
//Content-Type: application/x-www-form-urlencoded; charset=UTF-8
//Referer: http://demo.m-m10010.com/sim/customer
//Accept-Encoding: gzip, deflate
//Accept-Language: zh,zh-CN;q=0.8,en;q=0.6,zh-TW;q=0.4
//Cookie: aliyungf_tc=AQAAAH3TPDes5gMAHB0Ot4yONBq6GaaA; ASP.NET_SessionId=hfok0ftocsfomjnbjftaakhx; UserCookie=UserID=1&UserName=admin&UserType=1&HoldID=1&HoldName=%e8%bf%90%e8%90%a5%e4%b8%ad%e5%bf%83&HoldLevel=1&HoldType=4&Token=IPIBMIWHVSNBYMDFJUKMK0QW5OTOCR5F&LoginFromType=1&OEMClient=

//simIds%5B%5D=408697&groupId=162


//POST http://demo.m-m10010.com/api/SetVExpireTime HTTP/1.1
//Host: demo.m-m10010.com
//Connection: keep-alive
//Content-Length: 53
//Accept: application/json, text/javascript, */*; q=0.01
//Origin: http://demo.m-m10010.com
//X-Requested-With: XMLHttpRequest
//User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36
//Content-Type: application/x-www-form-urlencoded; charset=UTF-8
//Referer: http://demo.m-m10010.com/sim/setTillDate?cardType=1
//Accept-Encoding: gzip, deflate
//Accept-Language: zh,zh-CN;q=0.8,en;q=0.6,zh-TW;q=0.4
//Cookie: aliyungf_tc=AQAAAH3TPDes5gMAHB0Ot4yONBq6GaaA; ASP.NET_SessionId=hfok0ftocsfomjnbjftaakhx; UserCookie=UserID=1&UserName=admin&UserType=1&HoldID=1&HoldName=%e8%bf%90%e8%90%a5%e4%b8%ad%e5%bf%83&HoldLevel=1&HoldType=4&Token=IPIBMIWHVSNBYMDFJUKMK0QW5OTOCR5F&LoginFromType=1&OEMClient=

//ids%5B%5D=4733596&vExpireTime=2018-10-27+23%3A59%3A59

   
 
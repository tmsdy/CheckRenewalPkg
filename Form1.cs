using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Data.SQLite;

namespace CheckRenewalPkg
{
    public partial class Form1 : Form
    {
        string sVer = "V1.1.7";
        string[] skipUserList = { "麦谷测试电信卡", "MG测试电信卡", "续费转仓", "0531081测试勿动", "娜姐", "接口调试(联通)", "麦谷内部人员", "ZYR_麦联宝测试", "ZYR_研发部调试卡" ,
                                "ZYR_客服体验", "ZYR_其他人员试用", "SDY_体验测试", "ZW_后视镜测试", "123", "123-01", "123-02", "实名奖励套餐测试", "ZYR_内部测试卡",
                                "ZYR_麦谷测试_YD", "ZYR_麦谷测试_DX", "ZYR_麦谷测试_LT","Jaffe_S85", "海如测试"};
        string sApiUrl = "http://demo.m-m10010.com/";
        string sLogFileName = "";
        string slogfilepath = "";
        private object filelocker = new object();
        public List<string> SearchResult;

        public static Encoding RequestEncoding = _Encoding.UTF8;
        public static Encoding ResponseEncoding = _Encoding.UTF8;


        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text += sVer;
            string sLogPath = Application.StartupPath + @"\logs\";
            if (!Directory.Exists(sLogPath))
            {
                Directory.CreateDirectory(sLogPath);
            }

            sLogFileName = "CRP_" + DateTime.Now.ToString("yyMMddHHmmss");
            slogfilepath = Application.StartupPath + @"\logs\" + sLogFileName + ".txt";

            FileStream fsLogFile = null;
            //int offset = 0;
            if (!File.Exists(slogfilepath))
            {
                //文件不存在
                fsLogFile = File.Create(slogfilepath);
                fsLogFile.Close();

            }
            GetUserTree(false);
            //RefreshUserTree(ParamDefine.UserTreeDefault);
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
                request.ContentType = "application/x-www-form-urlencoded";
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

            string result = CreatePostHttpResponse(data, url, null, null, null, null);


            return result;
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受     
        }
        public static HttpWebResponse CreateGetHttpResponse(string url, int? timeout, string userAgent, string cookies, WebProxy wp)
        {



            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException("url");
                }
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
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
            webresponse_tmp.Close();
            sstream_tmp.Dispose();
            sstream_tmp.Close();
            reader.Dispose();
            reader.Close();
            return result;
        }
        private TreeNode FindNodeById(TreeNode tnParent, string strValue)
        {
            if (tnParent == null) return null;

            if (tnParent.Tag.ToString() == strValue) return tnParent;
            else if (tnParent.Nodes.Count == 0) return null;

            TreeNode tnCurrent, tnCurrentPar;

            //Init node
            tnCurrentPar = tnParent;
            tnCurrent = tnCurrentPar.FirstNode;

            while (tnCurrent != null && tnCurrent != tnParent)
            {
                while (tnCurrent != null)
                {
                    if (tnCurrent.Tag.ToString() == strValue) return tnCurrent;
                    else if (tnCurrent.Nodes.Count > 0)
                    {
                        //Go into the deepest node in current sub-path
                        tnCurrentPar = tnCurrent;
                        tnCurrent = tnCurrent.FirstNode;
                    }
                    else if (tnCurrent != tnCurrentPar.LastNode)
                    {
                        //Goto next sible node
                        tnCurrent = tnCurrent.NextNode;
                    }
                    else
                        break;
                }

                //Go back to parent node till its has next sible node
                while (tnCurrent != tnParent && tnCurrent == tnCurrentPar.LastNode)
                {
                    tnCurrent = tnCurrentPar;
                    tnCurrentPar = tnCurrentPar.Parent;
                }

                //Goto next sible node
                if (tnCurrent != tnParent)
                    tnCurrent = tnCurrent.NextNode;
            }
            return null;
        }


        private TreeNode FindNodeByText(TreeNode tnParent, string strValue)
        {
            if (tnParent == null) return null;

            if (tnParent.Text == strValue) return tnParent;
            else if (tnParent.Nodes.Count == 0) return null;

            TreeNode tnCurrent, tnCurrentPar;

            //Init node
            tnCurrentPar = tnParent;
            tnCurrent = tnCurrentPar.FirstNode;

            while (tnCurrent != null && tnCurrent != tnParent)
            {
                while (tnCurrent != null)
                {
                    if (tnCurrent.Text == strValue) return tnCurrent;
                    else if (tnCurrent.Nodes.Count > 0)
                    {
                        //Go into the deepest node in current sub-path
                        tnCurrentPar = tnCurrent;
                        tnCurrent = tnCurrent.FirstNode;
                    }
                    else if (tnCurrent != tnCurrentPar.LastNode)
                    {
                        //Goto next sible node
                        tnCurrent = tnCurrent.NextNode;
                    }
                    else
                        break;
                }

                //Go back to parent node till its has next sible node
                while (tnCurrent != tnParent && tnCurrent == tnCurrentPar.LastNode)
                {
                    tnCurrent = tnCurrentPar;
                    tnCurrentPar = tnCurrentPar.Parent;
                }

                //Goto next sible node
                if (tnCurrent != tnParent)
                    tnCurrent = tnCurrent.NextNode;
            }
            return null;
        }

        public void GetUserTree(bool isDisplayGhostUser)
        {
            string result = "";
            result = GetResponseSafe(sApiUrl + "/api/allholdnodes?nodeListType=1&NJholdId=1&notIncludeCount=false&id=" + Program.UserId + "&parent=" + Program.UserId);
            RefreshUserTree(result, isDisplayGhostUser);

        }
        public void RefreshUserTree(string a, bool isDisplayGhostUser)
        {
            string usertree = "";
            string errormsg = "";
            if (a == "")
                return;
            TreeNode node1;
            TreeNode nodeParent;
            int id = 0;
            ParamDefine.UserTree userTree = JsonConvert.DeserializeObject<ParamDefine.UserTree>(a);


            if (userTree == null)
                return;
            if (userTree.result == null)
                return;
            for (int i = 0; i < userTree.result.Count; i++)
            {
                if (treeView1.Nodes.Count == 0)
                {
                    node1 = new TreeNode();
                    node1.Tag = userTree.result[i].id.ToString();
                    node1.Text = userTree.result[i].name.ToString();
                    node1.ToolTipText = userTree.result[i].id.ToString();
                    treeView1.Nodes.Add(node1);
                    usertree += node1.Text + "\t" + node1.ToolTipText+ "\r\n";
                    continue;
                }
                TreeNode root = treeView1.Nodes[0];
                root.Expand();
                nodeParent = FindNodeById(root, userTree.result[i].parentId.ToString());
                if (nodeParent != null)
                {
                    node1 = new TreeNode();
                    node1.Tag = userTree.result[i].id.ToString();
                    node1.Text = userTree.result[i].name.ToString();
                    node1.ToolTipText = userTree.result[i].id.ToString();
                    nodeParent.Nodes.Add(node1);
                    usertree += node1.Text + "\t" + node1.ToolTipText + "\r\n";
                }
                else
                {
                    if (isDisplayGhostUser)
                        errormsg += "parentId\t" + userTree.result[i].parentId.ToString() + "\tID\t" + userTree.result[i].id.ToString() + "\t" + userTree.result[i].name.ToString() + "\r\n";
                    //一堆异常的无父节点的用户
                    //node1 = new TreeNode();
                    //node1.Tag = userTree.result[i].id.ToString();
                    //node1.Text = userTree.result[i].name.ToString();
                    //treeView1.Nodes.Add(node1);
                }


            }
            //DisplayAndLog(usertree, true);
            DisplayAndLog(errormsg, true);

        }
        public void button1_Click(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();
            GetUserTree(false);
        }
        public string GetShowAllStr()
        {
            string result = "";
            if (this.checkBox1.Checked == true)
            {
                result = "?allShow=true";
            }
            else
            {

                result = "?allShow=false";
            }
            return result;
        }
        public string GetPkgRenewalPkg(string id)
        {
            string response = "";
            string result = "";

            bool isSpecialPkg = Convert.ToBoolean(InvokeHelper.Get(this.checkBox3, "Checked"));
            string specialPkg = InvokeHelper.Get(this.textBox1, "Text").ToString().Trim();

            if(string.IsNullOrEmpty(specialPkg))
                isSpecialPkg = false;
            
            if (id == "")
            {
                DisplayAndLog("ID不合法\r\n", true);
                return result;
            }
            string url = sApiUrl + "api/RenewalsPackage?id=" + id;
            response = GetResponseSafe(url);
            if (response == "")
            {
                response = GetResponseSafe(url);
                if (response == "")
                {
                    DisplayAndLog("套餐Id为" + id + "查不到啊亲\r\n", true);
                    return result;
                }
            }
            ParamDefine.RenewalsPackage rp = JsonConvert.DeserializeObject<ParamDefine.RenewalsPackage>(response);
            if (rp.result == null)
                return result;
            foreach (ParamDefine.RenewalsPackageItem rpi in rp.result)
            {
                if (isSpecialPkg == false)
                {
                    result += "@\t\t\t\t\t└--" + rpi.PackageName.PadRight(20) + "\t@R" + rpi.UnitPrice + "\t@R" + rpi.BackPrice + "\r\n";
                    if (rpi.UnitPrice == "0.01")
                    {
                        result += "\t@R!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\n"; 
                    }
                }
                else
                {
                    if(specialPkg==rpi.PackageName)
                    {
                        result += "@\t\t\t\t\t└--" + rpi.PackageName.PadRight(20) + "\t@R" + rpi.UnitPrice + "\t@R" + rpi.BackPrice + "\r\n";
                        if (rpi.UnitPrice == "0.01")
                        {
                            result += "\t@R!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\n";
                        }
                    }
                }
            
            }


            return result;
        }

        public bool isSkip(string username)
        {
            foreach(string u in skipUserList)
            {
                if (u == username)
                    return true;
            }
            return false;

        }
        public string GetHoldRenewalList(string id)
        {
            string response = "";
            string result = "";
            string tmp = "";
            string[] pkgDescArr = new string[10]; // 3-年，2-月，1-叠加，4-加油 0-电信移动
            bool isGetPkgRenelwalPkg = Convert.ToBoolean(InvokeHelper.Get(this.checkBox2, "Checked"));
            bool isVerySpeed = Convert.ToBoolean(InvokeHelper.Get(this.checkBox4, "Checked"));
            if (id == "")
            {
                DisplayAndLog("ID不合法\r\n", true);
                return result;
            }
            string url = sApiUrl + "api/HoldRenewalsList/" + id + GetShowAllStr();
            response = GetResponseSafe(url);
            if (response == "")
            {
                DisplayAndLog("holdId为" + id + "查不到啊亲\r\n", true);
                return result;
            }
            ParamDefine.HoldRenewalsList hrl = JsonConvert.DeserializeObject<ParamDefine.HoldRenewalsList>(response);
            if (hrl.result == null)
                return result;
            foreach (ParamDefine.HoldList user in hrl.result)
            {
                foreach (ParamDefine.PackageListItem pkg in user.PackageList)
                {
                    if ((isVerySpeed)&&(isSkip(user.HoldName)))
                        continue;

                    pkgDescArr[pkg.Type] += ("@" + user.HoldName.PadRight(20) + "\tCUCC\t@B" + pkg.PackageName.PadRight(20) + "\t@" + pkg.UnitPrice + "\t" + pkg.BackPrice + "\t" + (Convert.ToDouble(pkg.BackPrice) / Convert.ToDouble(pkg.UnitPrice)).ToString("0.00%") + "\t");
                    pkgDescArr[pkg.Type] += ((pkg.TopLevel == "0") || (pkg.TopLevel == "10")) ? "" : "@R荐" + pkg.TopLevel;
                    if (isGetPkgRenelwalPkg)
                    {
                        tmp = GetPkgRenewalPkg(pkg.ID);
                        pkgDescArr[pkg.Type] += "\t@B可续费套餐数量:" + (tmp.Split('\n').Count() - 1).ToString();
                        if (tmp.IndexOf(pkg.PackageName) >= 0)
                        {
                            pkgDescArr[pkg.Type] += "\t@B是" + "\r\n";
                        }
                        else
                        {
                            pkgDescArr[pkg.Type] += "\t@R否" + "\r\n";
                        }
                        pkgDescArr[pkg.Type] += tmp;

                    }
                    else
                    {
                        pkgDescArr[pkg.Type] += "\r\n";
                    }

                }
                for (int i = 0; i < pkgDescArr.Count(); i++)
                {
                    result += pkgDescArr[i];
                    pkgDescArr[i] = "";
                }
                foreach (ParamDefine.PackageListItem pkg in user.YDPackageList)
                {
                    if ((isVerySpeed) && (isSkip(user.HoldName)))
                        continue;
                    pkgDescArr[pkg.Type] += ("@" + user.HoldName.PadRight(20) + "\tCMCC\t@B" + pkg.PackageName.PadRight(20) + "\t@" + pkg.UnitPrice + "\t" + pkg.BackPrice + "\t" + (Convert.ToDouble(pkg.BackPrice) / Convert.ToDouble(pkg.UnitPrice)).ToString("0.00%") + "\t");
                    pkgDescArr[pkg.Type] += ((pkg.TopLevel == "0") || (pkg.TopLevel == "10")) ? "" : "@R" + pkg.TopLevel;
                    if (isGetPkgRenelwalPkg)
                    {
                        tmp = GetPkgRenewalPkg(pkg.ID);
                        pkgDescArr[pkg.Type] += "\t@B可续费套餐数量:" + (tmp.Split('\n').Count() - 1).ToString();
                        if (tmp.IndexOf(pkg.PackageName) >= 0)
                        {
                            pkgDescArr[pkg.Type] += "\t@B是" + "\r\n";
                        }
                        else
                        {
                            pkgDescArr[pkg.Type] += "\t@R否" + "\r\n";
                        }
                        pkgDescArr[pkg.Type] += tmp;
                    }
                    else
                    {
                        pkgDescArr[pkg.Type] += "\r\n";
                    }
                }
                for (int i = 0; i < pkgDescArr.Count(); i++)
                {
                    result += pkgDescArr[i];
                    pkgDescArr[i] = "";
                }
                foreach (ParamDefine.PackageListItem pkg in user.DXPackageList)
                {
                    if ((isVerySpeed) && (isSkip(user.HoldName)))
                        continue;
                    pkgDescArr[pkg.Type] += ("@" + user.HoldName.PadRight(20) + "\tCTCC\t@B" + pkg.PackageName.PadRight(20) + "\t@" + pkg.UnitPrice + "\t" + pkg.BackPrice + "\t" + (Convert.ToDouble(pkg.BackPrice) / Convert.ToDouble(pkg.UnitPrice)).ToString("0.00%") + "\t");
                    pkgDescArr[pkg.Type] += ((pkg.TopLevel == "0") || (pkg.TopLevel == "10")) ? "" : "@R" + pkg.TopLevel;
                    if (isGetPkgRenelwalPkg)
                    {
                        tmp = GetPkgRenewalPkg(pkg.ID);
                        pkgDescArr[pkg.Type] += "\t@B可续费套餐数量:" + (tmp.Split('\n').Count() - 1).ToString();
                        if (tmp.IndexOf(pkg.PackageName) >= 0)
                        {
                            pkgDescArr[pkg.Type] += "\t@B是" + "\r\n";
                        }
                        else
                        {
                            pkgDescArr[pkg.Type] += "\t@R否" + "\r\n";
                        }
                        pkgDescArr[pkg.Type] += tmp;
                    }
                    else
                    {
                        pkgDescArr[pkg.Type] += "\r\n";
                    }
                }
                for (int i = 0; i < pkgDescArr.Count(); i++)
                {
                    result += pkgDescArr[i];
                    pkgDescArr[i] = "";
                }
            }
            return result;

        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.button2.Text = "获取中";
            this.button2.Enabled = false;
            this.backgroundWorker1.RunWorkerAsync();

        }
        private delegate void SetTextHandler(string text, Color co);
        public void SetText(string text)
        {
            Color co = Color.Black;
            if (richTextBox1.InvokeRequired == true)
            {
                SetTextHandler set = new SetTextHandler(SetText);//委托的方法参数应和SetText一致
                richTextBox1.Invoke(set, new object[] { text, co }); //此方法第二参数用于传入方法,代替形参text
            }
            else
            {
                richTextBox1.SelectionColor = co;
                richTextBox1.AppendText(text);
                this.richTextBox1.ScrollToCaret();
            }
        }
        public void SetText(string text, Color co)
        {

            if (richTextBox1.InvokeRequired == true)
            {
                SetTextHandler set = new SetTextHandler(SetText);//委托的方法参数应和SetText一致
                richTextBox1.Invoke(set, new object[] { text, co }); //此方法第二参数用于传入方法,代替形参text
            }
            else
            {
                richTextBox1.SelectionColor = co;
                richTextBox1.AppendText(text);
                this.richTextBox1.ScrollToCaret();
            }
        }

        public void WriteLogFile(string input)
        {
            /**/
            ///定义文件信息对象

            FileInfo finfo = new FileInfo(slogfilepath);

            if (!finfo.Exists)
            {
                FileStream fs;
                fs = File.Create(slogfilepath);
                fs.Close();
                finfo = new FileInfo(slogfilepath);
            }

            try
            {
                lock (filelocker)
                {
                    using (StreamWriter writer = File.AppendText(slogfilepath))
                    {
                        //writer.WriteLine(input);
                        writer.Write(input);
                    }

                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("打印XXXX异常 " + input);
            }


        }
        public void DisplayAndLogBatch(string s, bool isDisplay)
        {
            string[] str = s.Split('@');
            int i = 0;
            foreach (string a in str)
            {
                i++;
                if (a == "")
                    continue;
                if (i == 1)
                {
                    DisplayAndLog(a, isDisplay, Color.Black);
                    continue;

                }
                //if (a.Substring(0, 1) == "@")
                //{
                switch (a.Substring(0, 1))
                {
                    case "B":
                        DisplayAndLog(a.Substring(1), isDisplay, Color.Blue);
                        break;
                    case "R":
                        DisplayAndLog(a.Substring(1), isDisplay, Color.Red);
                        break;
                    default:
                        DisplayAndLog(a.Substring(0), isDisplay, Color.Black);
                        break;

                }
                //}
                //else
                //{
                //    DisplayAndLog("\t" + a, isDisplay, Color.Black);
                //}

            }
        }
        public void DisplayAndLog(string s, bool isDisplay, Color color)
        {

            if (isDisplay)
            {
                if (color != null)
                {
                    SetText(s, color);
                }
                else
                {
                    SetText(s);
                }

                Application.DoEvents();
            }
            //richTextBox1.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + '\t' + s);
            WriteLogFile(s);
        }
        public void DisplayAndLog(string s, bool isDisplay)
        {

            if (isDisplay)
            {

                SetText(s);
                Application.DoEvents();
            }
            //richTextBox1.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + '\t' + s);
            WriteLogFile(s);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
        }

        private void LoopSearch(TreeNode tn, string key)
        {
            if (string.IsNullOrEmpty(key))
                return;
            //1.将当前节点显示到lable上
            if (tn.Text.IndexOf(key) >= 0)
                SearchResult.Add(tn.Text);
            foreach (TreeNode tnSub in tn.Nodes)
            {
                LoopSearch(tnSub, key);
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedItem == null)
                return;
            string key = this.comboBox1.SelectedItem.ToString();
            if (string.IsNullOrEmpty(key))
                return;
            treeView1.SelectedNode = FindNodeByText(treeView1.Nodes[0], key);
            treeView1.Select();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string key = this.comboBox1.Text;
            SearchResult = new List<string>();
            SearchResult.Clear();
            comboBox1.Items.Clear();
            LoopSearch(this.treeView1.Nodes[0], key);
            foreach (string a in SearchResult)
            {
                comboBox1.Items.Add(a);
                DisplayAndLog(a + "\r\n", false);
            }

            comboBox1.SelectionStart = comboBox1.Text.Trim().Length;
            comboBox1.DroppedDown = true;
        }

        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button4_Click(sender, null);
            }

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string id = "";
            e.Result = "";
            if (treeView1.Nodes.Count == 0)
            {
                DisplayAndLog("请先刷新用户列表\r\n", true);
                return;
            }

            if (treeView1.SelectedNode == null)
            {

                DisplayAndLog("请先选择用户\r\n", true);
                return;
            }
            id = treeView1.SelectedNode.Tag.ToString();
            e.Result = GetHoldRenewalList(id);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.button2.Text = "获取套餐列表";
            this.button2.Enabled = true;

            bool isVerySpeed = Convert.ToBoolean(InvokeHelper.Get(this.checkBox4, "Checked"));
           if(isVerySpeed)
           {
               DisplayAndLogBatch(e.Result.ToString(), false);
           }
            else
           {
               DisplayAndLogBatch(e.Result.ToString(), true);
           }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();
            GetUserTree(true);
        }

        private void button6_Click(object sender, EventArgs e)
        {

            this.button6.Text = "获取中";
            this.button6.Enabled = false;

            this.backgroundWorker2.RunWorkerAsync();
        }
        private string GetLastMonthBackMoney(string id, string period)
        {
            double backmoneySum = 0;
            double renewalsSum = 0;
            string result = "";
            string tmp = "";
            string url = "";
            switch (period)
            {
                case "lastmonth":

                    url = "http://open.m-m10010.com/api/GetHoldMonthAmountList?period=lastmonth&holdId=" + id;
                    break;

                default:
                    url = "http://open.m-m10010.com/api/GetHoldMonthAmountList?holdId=" + id;
                    break;

            }
            string response = GetResponseSafe(url);
            if (response == "")
            {
                DisplayAndLog("holdId为" + id + "查不到啊亲\r\n", true);
                return result;
            }
            ParamDefine.BackMoney bmlist = JsonConvert.DeserializeObject<ParamDefine.BackMoney>(response);
            if (bmlist.result == null)
                return result;
            foreach (ParamDefine.BackMoneyResultItem bmr in bmlist.result)
            {
                tmp += bmr.iccid + "\t" + bmr.packageName + "\t" + bmr.renewalsPrice + "\t" + bmr.backPrice + "\t" + bmr.renewalsTime + "\r\n";
                backmoneySum += (bmr.backPrice);
                renewalsSum += bmr.renewalsPrice;
            }

            result = "ID为" + id + "\t笔数为\t" + bmlist.result.Count().ToString() + "\t总续费为\t" + renewalsSum.ToString() + "\t总返利为\t" + backmoneySum.ToString() + "\r\n" + tmp;
            return result;
        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            string id = "";

            e.Result = "";
            if (treeView1.Nodes.Count == 0)
            {
                DisplayAndLog("请先刷新用户列表\r\n", true);
                return;
            }

            if (treeView1.SelectedNode == null)
            {

                DisplayAndLog("请先选择用户\r\n", true);
                return;
            }
            id = treeView1.SelectedNode.Tag.ToString();
            DisplayAndLogBatch(treeView1.SelectedNode.Text.ToString(), true);
            e.Result = GetLastMonthBackMoney(id, "lastmonth");
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button6.Text = "上月返利";
            this.button6.Enabled = true;
            DisplayAndLogBatch(e.Result.ToString(), true);
            DisplayAndLogBatch("------------------------------------------------------------------------\r\n", true);
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            string id = "";

            e.Result = "";
            if (treeView1.Nodes.Count == 0)
            {
                DisplayAndLog("请先刷新用户列表\r\n", true);
                return;
            }

            if (treeView1.SelectedNode == null)
            {

                DisplayAndLog("请先选择用户\r\n", true);
                return;
            }
            id = treeView1.SelectedNode.Tag.ToString();
            DisplayAndLogBatch(treeView1.SelectedNode.Text.ToString(), true);
            e.Result = GetLastMonthBackMoney(id, "");

        }

        private void button7_Click(object sender, EventArgs e)
        {

            this.button7.Text = "获取中";
            this.button7.Enabled = false;

            this.backgroundWorker3.RunWorkerAsync();
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button7.Text = "本月返利";
            this.button7.Enabled = true;
            DisplayAndLogBatch(e.Result.ToString(), true);
            DisplayAndLogBatch("------------------------------------------------------------------------\r\n", true);

        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.button8.Text = "获取中";
            this.button8.Enabled = false;

            this.backgroundWorker4.RunWorkerAsync("multi");
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            string id = "";
            string tmp = "";
            string whichway = e.Argument.ToString();

            e.Result = "";
            if (whichway == "single")
            {
                if (treeView1.Nodes.Count == 0)
                {
                    DisplayAndLog("请先刷新用户列表\r\n", true);
                    return;
                }

                if (treeView1.SelectedNode == null)
                {

                    DisplayAndLog("请先选择用户\r\n", true);
                    return;
                }
                tmp += "\t总卡数\t" +
                      "已激活卡\t" +
                      "已停用卡\t" +
                      "有效续费卡\t" +
                      "总续费金额\t" +
                      "总返利金额\t" +
                      "累计ARPU\t" +
                      "有效续费率\t" +
                      "续费率\t" +
                      "使用率\t" +
                      "脱网率\t" +
                      "复充率\r\n";
                DisplayAndLog(tmp, true);
                id = treeView1.SelectedNode.Tag.ToString();
                DisplayAndLog(treeView1.SelectedNode.Text.ToString() , true);
                DisplayAndLog(GetRenewalsTotal(id,true), true);


            }
            else
            {
                //MIFI AL VIP
                //高科,   燕遥,  宇荪,  羽嘉,   粤烨, 澳先,  酷比,盛世创展,蓝熊猫,秒通,星博睿,伊雷克,盛世    骏虎   朵墨
                //"6779","5287","5521","5288","5377","5108","4715","4716","5015","5013","5014","5116","7009","7987","7837",

                //MIFI AL 普通
                //宏软,东方睿智,亿晨,帝成,金德,函夏,莲腾,枚山,闪谷,悟虎,亿竖,私人,通恒伟创,微络斯,新丰慧,珠峰讯,致恒达,
                // "6367","6423","6947","6314","6975","5877","7242","6945","5905","5891","6362","5523","5926","6734","5411","5362","6317",

                //MIFI NEW
                //畅玩互娱,速叮通
                //"5519","6304",

                //MIFI普通
                //东方睿智,珠峰讯,高科,羽嘉,澳先,酷比,蓝熊猫,秒通,盛世创展,星博睿,沃德盛世,物联网基地,酷鱼OL 酷鱼线下
                //"6424","5361","6780","5293","5107","4933","5018","5019","4927","5020","7008","7269","6556","6889",

                //导航
                //润乐 爱影 奥车 魁途 三一 索菱 罗姆 永盛杰 永盛杰  卓兴威              福嘉太  浩谷  骏哥 车领航    拓步 途锐(易图)
                //"6980","6750","4988","6787","5426","4026","4382","4108","2201","4982","6791","7460","7868","8058","7329","7632", 

                //车联网
                //H_VIP50,H_VIP付费,H已续费,H未续费,H海南成杰汽车,H江圳_爱培科,H卡卡_凌度,安信翔,八零后,北斗传奇,丁威特,索行电子,途龙,维系欧,欣万和,征路者,卓派,东莞润禾车品电子C,天之眼_C,欣和先知C,众创伟业C,艾米,保速捷C,凌途C,四维星图C,泰瑞视C,3G绑带_L,威仕特麦联宝
                //"3410","2987","3411","3412","3369","1230","1272","2727","2330","5501","3395","1280","3164","3747","4001","3548","2199","6054","5236","6257","5341","3695","6710","6723","6729","6837","5129","4125",

                //车联网 AL
                //惠普AL,润禾车AL,天之眼_AL,创维汽车AL,欣和先知AL,众创伟业AL,智行至美_AL,威仕特_AL,威仕特_ALIC,
                //"6087","6052","4978","6212","6231","5342","4682","4717","6569",

                //POS
                //亿地,马克,富博,科伟,惠联,丰宁,紫仪,迦之南,欧尔,东颍,小宝贝,新瑞,盛世,易付,灯火
                //"3686","6063","2700","4087","3930","4817","6773","4035","3406","4923","3815","2467","3936","4897","4858"


                tmp += "\t总卡数\t" +
                          "已激活卡\t" +
                          "已停用卡\t" +
                          "有效续费卡\t" +
                          "总续费金额\t" +
                          "总返利金额\t" +
                          "累计ARPU\t" +
                          "有效续费率\t" +
                          "续费率\t" +
                          "使用率\t" +
                          "脱网率\t" +
                          "复充率\r\n";
                DisplayAndLog(tmp, true);
                string[] idlist = {
                                      "6779","5287","5521","5288","5377","5108","4715","4716","5015","5013","5014","5116","7009","7987","7837",
                                    "6367","6423","6947","6314","6975","5877","7242","6945","5905","5891","6362","5523","5926","6734","5411","5362","6317",
                                    "5519","6304",
                                    "6424","5361","6780","5293","5107","4933","5018","5019","4927","5020","7008","7269","6556","6889",
                                    "6980","6750","4988","6787","5426","4026","4382","4108","2201","4982","6791","7460","7868","8058","7329","7632",
                                    "3410","2987","3411","3412","3369","1230","1272","2727","2330","5501","3395","1280","3164","3747","4001","3548","2199","6054","5236","6257","5341","3695","6710","6723","6729","6837","5129","4125",
                                    "6087","6052","4978","6212","6231","5342","4682","4717","6569",
                                    "3686","6063","2700","4087","3930","4817","6773","4035","3406","4923","3815","2467","3936","4897","4858"  
                                  };
                foreach (string idid in idlist)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog("未知用户ID为" + idid  + GetRenewalsTotal(idid,false), true);
                        continue;
                    }

                    //treeView1.Select();

                    DisplayAndLog(treeView1.SelectedNode.Text.ToString() , true);
                    DisplayAndLog(GetRenewalsTotal(idid,false), true);
                }

            }

            e.Result = whichway;

        }

        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Result.ToString() == "single")
            {
                this.button9.Text = "单个续费汇总";
                this.button9.Enabled = true; 
            }
            else
            {
                this.button8.Text = "检查续费汇总";
                this.button8.Enabled = true; 
            }

            DisplayAndLogBatch("------------------------------------------------------------------------\r\n", true);
        }
        private string GetMonthRenewalsTotal(string id)
        {
            string result = "";
            string tmp = "";
            string url = "";

            url = sApiUrl + "/api/ReportStatusRenewalsTotal?cmd=2&holdId=" + id;

            string response = GetResponseSafe(url);
            if (response == "")
            {
                DisplayAndLog("holdId为" + id + "查不到啊亲\r\n", true);
                return result;
            }
            ParamDefine.MonthRenewalsTotal mrt = JsonConvert.DeserializeObject<ParamDefine.MonthRenewalsTotal>(response);
            if (mrt.result == null)
                return result + "\r\n";
            foreach (ParamDefine.MonthRenewalsTotalResultItem mrtresult in mrt.result)
            {
                tmp += mrtresult.date + "\t" +
                    mrtresult.amount.ToString("#0") + "\t" +
                    mrtresult.usage.ToString("#0") + "\t" +
                    (mrtresult.amount / mrtresult.usage * 1024.00D).ToString("#0.00") + "\t";
            }
            result = tmp + "\r\n";
            return result;

        }
        private string GetRenewalsTotal(string id,bool isDisplayTitle)
        {
            string result = "";
            string tmp = "";
            string url = "";

            url = sApiUrl + "/api/NewReportRenewalsTotalForHoldInfo?holdId=" + id;

            string response = GetResponseSafe(url);
            if (response == "")
            {
                DisplayAndLog("holdId为" + id + "查不到啊亲\r\n", true);
                return result;
            }
            ParamDefine.RenewalsTotal rt = JsonConvert.DeserializeObject<ParamDefine.RenewalsTotal>(response);
            ParamDefine.RenewalsTotalResult rtresult = rt.result;
            //if (isDisplayTitle)
            //{
            //    tmp += "总卡数\t" + rtresult.allCount +
            //          "\t已激活卡\t" + rtresult.ltActivatedCount +
            //          "\t已停用卡\t" + rtresult.ltStopCount +
            //          "\t有效续费卡\t" + rtresult.renewalsCount +
            //          "\t总续费金额\t" + rtresult.renewalsAmount +
            //          "\t总返利金额\t" + rtresult.backAmount +
            //          "\t累计ARPU\t" + (rtresult.renewalsAmount / rtresult.renewalsCount).ToString("#0.00") +
            //          "\t有效续费率\t" + string.Format("{0:0.00%}", ((double)rtresult.renewalsCount / (rtresult.ltActivatedCount + rtresult.ltStopCount))) +
            //          "\t续费率\t" + string.Format("{0:0.00%}", ((double)rtresult.renewalsCount / (rtresult.allCount))) +
            //          "\t使用率\t" + string.Format("{0:0.00%}", ((double)(rtresult.ltActivatedCount + rtresult.ltStopCount) / rtresult.ltAllCount)) +
            //          "\t脱网率\t" + string.Format("{0:0.00%}", ((double)rtresult.ltOutCount / (rtresult.ltActivatedCount + rtresult.ltStopCount))) +
            //          "\t复充率\t" + string.Format("{0:0.00%}", ((double)rtresult.twiceRenewalsCount / (rtresult.renewalsCount))) + "\t";

            //}
            //else
            //{ 

            if (rtresult == null)
                return result;
                tmp += "\t" + rtresult.allCount +
                       "\t" + rtresult.ltActivatedCount +
                       "\t" + rtresult.ltStopCount +
                       "\t" + rtresult.renewalsCount +
                       "\t" + rtresult.renewalsAmount.ToString("#0") +
                       "\t" + rtresult.backAmount.ToString("#0") +
                       "\t" + (rtresult.renewalsAmount / rtresult.renewalsCount).ToString("#0.00") +
                       "\t" + string.Format("{0:0.00%}", ((double)rtresult.renewalsCount / (rtresult.ltActivatedCount + rtresult.ltStopCount))) +
                       "\t" + string.Format("{0:0.00%}", ((double)rtresult.renewalsCount / (rtresult.allCount))) +
                       "\t" + string.Format("{0:0.00%}", ((double)(rtresult.ltActivatedCount + rtresult.ltStopCount) / rtresult.ltAllCount)) +
                       "\t" + string.Format("{0:0.00%}", ((double)rtresult.ltOutCount / (rtresult.ltActivatedCount + rtresult.ltStopCount))) +
                       "\t" + string.Format("{0:0.00%}", ((double)rtresult.twiceRenewalsCount / (rtresult.renewalsCount))) + "\t";
         
            //}

            tmp += GetMonthRenewalsTotal(id);
            result = tmp.Replace("非数字", "0");
            return result;


        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.button9.Text = "获取中";
            this.button9.Enabled = false;

            this.backgroundWorker4.RunWorkerAsync("single");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox2.Checked == false)
            {
                this.textBox1.Enabled = false;
                this.checkBox3.Enabled = false;
            }
            else
            {

                this.textBox1.Enabled = true;
                this.checkBox3.Enabled = true;
            }
            this.checkBox3.Checked = false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox3.Checked == false)
            {
                this.textBox1.Enabled = false; 
            }
            else
            {

                this.textBox1.Enabled = true; 
            }
        }

        private void PrintRecursive(TreeNode treeNode)
        {
            // Print the node. 

            DisplayAndLog(treeNode.Text + "\t" + treeNode.ToolTipText + "\r\n", false);
            // Print each node recursively.
            foreach (TreeNode tn in treeNode.Nodes)
            {
                PrintRecursive(tn);
            }
        }

 
        private void button10_Click(object sender, EventArgs e)
        {
            // Print each node recursively.
            TreeNodeCollection nodes = treeView1.Nodes;
            foreach (TreeNode n in nodes)
            {
                PrintRecursive(n);
            }
        }
        public string GetOldRenewals(string id, bool isDisplayTitle)
        {
            string result = "";
            string tmp = "";
            string url = "";

            url = sApiUrl + "/api/ReportRenewalsTotalForDay?payee=MAPGOO&packageId=0&holdId=" + id;

            string response = GetResponseSafe(url);
            if (response == "")
            {
                DisplayAndLog("holdId为" + id + "查不到啊亲\r\n", true);
                return result;
            }
            ParamDefine.OldRenewalsRoot orr = JsonConvert.DeserializeObject<ParamDefine.OldRenewalsRoot>(response);
  
            if (orr == null)
                return "查不到结果\r\n";

            foreach (ParamDefine.OldRenewalsRootResultItem orritem in orr.result)
            {
                tmp += orritem.totalDay + "\t" + orritem.renewalsTimes + "\t" + orritem.renewalsAmount.ToString("0.00") + "\t" +  orritem.MA5.ToString("0.00") + "\r\n";
            }

            tmp += "\r\n";
            return tmp;

        }
        public void CalcAverage(List<int> countlist,List<double> usagelist)
        {

            if((countlist == null)||(usagelist == null))
                return;

            int count1 = countlist.Count();
            int count2 = usagelist.Count() ;
            if ((count1 <= 0) || (count2 <= 0))
                return; 
            string result_count = "";
            string result_usage = "";
            string result_avarage = "";

            int count = (count1 >= count2) ? count2 : count1;
            int userCounts = count / 30;
            int totalSum = 0;
            double usageSum = 0;
            for (int i = 0; i < 30; i++)
            {
                totalSum = 0;
                usageSum = 0;
                for (int j = 0; j + i < count; j+=30)
                {
                    usageSum += usagelist[j + i];
                    totalSum += countlist[j + i];
                }
                result_usage += usageSum.ToString("0.00") + "\t";
                result_count += totalSum + "\t";
                result_avarage += (usageSum/totalSum).ToString("0.00") + "\t";

            }
            DisplayAndLog(result_usage + "\r\n" + result_count + "\r\n" + result_avarage + "\r\n --------------------- \r\n", true);
            countlist.Clear();
            usagelist.Clear();

        }
        public string GetUsageTotal(string id, bool isDisplayTitle)
        {
            string result = "";
            string tmp = "";
            string url = "";

            url = sApiUrl + "/api/ReportFlowHold?holdId=" + id;

            string response = GetResponseSafe(url);
            if (response == "")
            {
                DisplayAndLog("holdId为" + id + "查不到啊亲\r\n", true);
                return result;
            }
            ParamDefine.FlowReportRoot frr = JsonConvert.DeserializeObject<ParamDefine.FlowReportRoot>(response);
            ParamDefine.FlowReportResult frrResult = frr.result;
            if (frrResult == null || frrResult.dayStatisticsList == null)
                return "查不到结果\r\n";

            foreach (ParamDefine.DayStatisticsListItem daylist in frrResult.dayStatisticsList)
            {
                //tmp += daylist.statDay + "\t" + daylist.amountUsage.ToString("0.00") + "\t" + daylist.validCount + "\t" + daylist.validAvgUsage.ToString("0.00") + "\t";
                tmp += daylist.statDay + "\t" + daylist.amountUsage.ToString("0.00") + "\t" + daylist.validCount + "\t";
            }

            tmp += "\r\n";
            return tmp;


        }
        public string GetUsageTotal(string id,bool isDisplayTitle,List<int> countlist,List<double> usagelist)
        {
            string result = "";
            string tmp = "";
            string url = "";
          
            url = sApiUrl + "/api/ReportFlowHold?holdId=" + id;

            string response = GetResponseSafe(url);
            if (response == "")
            {
                DisplayAndLog("holdId为" + id + "查不到啊亲\r\n", true);
                return result;
            }
            ParamDefine.FlowReportRoot frr = JsonConvert.DeserializeObject<ParamDefine.FlowReportRoot>(response);
            ParamDefine.FlowReportResult frrResult = frr.result;
            if (frrResult == null || frrResult.dayStatisticsList == null)
                return "查不到结果\r\n";

             foreach (ParamDefine.DayStatisticsListItem daylist in frrResult.dayStatisticsList)
             {
                 //tmp += daylist.statDay + "\t" + daylist.amountUsage.ToString("0.00") + "\t" + daylist.validCount + "\t" + daylist.validAvgUsage.ToString("0.00") + "\t";
                 tmp += daylist.statDay + "\t" + daylist.amountUsage.ToString("0.00") + "\t" + daylist.validCount  + "\t";
                 countlist.Add(Convert.ToInt32(daylist.validCount));
                 usagelist.Add(daylist.amountUsage);
             }

             tmp += "\r\n";
             return tmp;

             
        }
        private void button11_Click(object sender, EventArgs e)
        {
            this.button11.Text = "获取中";
            this.button11.Enabled = false;

            this.backgroundWorker5.RunWorkerAsync("single");
        }

        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {

            string id = "";
            string tmp = "";
            string whichway = e.Argument.ToString();
            List<int> Countlist = new List<int> { };
            List<double> Usagelist = new List<double> { };
               
            

            e.Result = "";
            if (whichway == "single")
            {
                if (treeView1.Nodes.Count == 0)
                {
                    DisplayAndLog("请先刷新用户列表\r\n", true);
                    return;
                }

                if (treeView1.SelectedNode == null)
                {

                    DisplayAndLog("请先选择用户\r\n", true);
                    return;
                }

                DisplayAndLog(tmp, true);
                id = treeView1.SelectedNode.Tag.ToString();
                DisplayAndLog(treeView1.SelectedNode.Text.ToString() + "\t", true);
                DisplayAndLog(GetUsageTotal(id, true), true);


            }
            else
            {

                DisplayAndLog(tmp, true);
                //D导航,LB,后视镜V,艾米,3G绑带,WST_AL,威仕特麦联宝,M电商S,M电商V,小流量V,小流量体验,,,,,,,,,
                //string[] idlist = { "4123", "1323", "1281", "3695", "5129", "4717", "4125", "5467", "2332", "2898", "2673" };
                string[] idlistVec = { "4123", "1323", "1281", "3695", "5129", "4717", "4125"  };
                string[] idlistMifi = {   "5467", "2332"  };
                string[] idlistPos = { "2898", "2673" };
                foreach (string idid in idlistVec)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog("未知用户ID为" + idid +"\t" + GetRenewalsTotal(idid, false), true);
                        continue;
                    } 

                    DisplayAndLog(treeView1.SelectedNode.Text.ToString() + "\t", true);
                    DisplayAndLog(GetUsageTotal(idid, false, Countlist,Usagelist), true);
                }
                CalcAverage(Countlist, Usagelist);
                foreach (string idid in idlistMifi)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog("未知用户ID为" + idid + "\t" + GetRenewalsTotal(idid, false), true);
                        continue;
                    }

                    DisplayAndLog(treeView1.SelectedNode.Text.ToString() + "\t", true);
                    DisplayAndLog(GetUsageTotal(idid, false, Countlist, Usagelist), true);
                }
                CalcAverage(Countlist, Usagelist);
                foreach (string idid in idlistPos)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog("未知用户ID为" + idid + "\t" + GetRenewalsTotal(idid, false), true);
                        continue;
                    }

                    DisplayAndLog(treeView1.SelectedNode.Text.ToString() + "\t", true);
                    DisplayAndLog(GetUsageTotal(idid, false, Countlist, Usagelist), true);
                }
                CalcAverage(Countlist, Usagelist);
            }

            e.Result = whichway;
        }

        private void backgroundWorker5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result.ToString() == "single")
            {
                this.button11.Text = "单个用量汇总";
                this.button11.Enabled = true;
            }
            else
            {
                this.button12.Text = "检查用量汇总";
                this.button12.Enabled = true;
            }

            DisplayAndLogBatch("------------------------------------------------------------------------\r\n", true);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.button12.Text = "获取中";
            this.button12.Enabled = false;

            this.backgroundWorker5.RunWorkerAsync("multi");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.button13.Text = "获取中";
            this.button13.Enabled = false;

            this.backgroundWorker6.RunWorkerAsync("single");
        }

        private void backgroundWorker6_DoWork(object sender, DoWorkEventArgs e)
        {

            string id = "";
            string tmp = "";
            string whichway = e.Argument.ToString();

            e.Result = "";
            if (whichway == "single")
            {
                if (treeView1.Nodes.Count == 0)
                {
                    DisplayAndLog("请先刷新用户列表\r\n", true);
                    return;
                }

                if (treeView1.SelectedNode == null)
                {

                    DisplayAndLog("请先选择用户\r\n", true);
                    return;
                }

                DisplayAndLog(tmp, true);
                id = treeView1.SelectedNode.Tag.ToString();
                DisplayAndLog(treeView1.SelectedNode.Text.ToString() + "\t", true);
                DisplayAndLog(GetOldRenewals(id, true), true);


            }
            else
            {

                //DisplayAndLog(tmp, true);
                ////D导航,LB,后视镜V,艾米,3G绑带,WST_AL,WST_ALIC,威仕特麦联宝,M电商S,M电商V,M渠道,小流量V,小流量体验,,,,,,,,,
                //string[] idlist = { "4123", "1323", "1281", "3695", "5129", "4717", "6569", "4125", "5467", "2332", "6927", "2898", "2673" };
                //foreach (string idid in idlist)
                //{
                //    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                //    if (null == treeView1.SelectedNode)
                //    {
                //        DisplayAndLog("未知用户ID为" + idid + "\t" + GetRenewalsTotal(idid, false), true);
                //        continue;
                //    }

                //    //treeView1.Select();

                //    DisplayAndLog(treeView1.SelectedNode.Text.ToString(), true);
                //    DisplayAndLog(GetUsageTotal(idid, false), true);
                //}

            }

            e.Result = whichway;
        }

        private void backgroundWorker6_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result.ToString() == "single")
            {
                this.button13.Text = "老续费";
                this.button13.Enabled = true;
            }
            //else
            //{
            //    this.button12.Text = "检查用量汇总";
            //    this.button12.Enabled = true;
            //}

            DisplayAndLogBatch("------------------------------------------------------------------------\r\n", true);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string s = this.richTextBox1.Text;
            if((s.IndexOf('(')>0)&&(s.IndexOf(')')>0))
            {
                MessageBox.Show("用户名不允许带\"(\" \")\"\r\n");
                return;
            }

           DialogResult dr= MessageBox.Show("格式为：  用户名，登录名，用户类型，密码\r\n" +  this.richTextBox1.Text, "提示", MessageBoxButtons.OKCancel);
           if (dr == DialogResult.OK)
           {
               this.button14.Enabled = false;
               this.backgroundWorker7.RunWorkerAsync(); 
           }
           else if (dr == DialogResult.Cancel)
           {
                
           }

        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.button15.Enabled = false;
            this.backgroundWorker8.RunWorkerAsync(); 
        }


        private string CreateUser(string parentid,string displayname,string loginname,string usertype,string password)
        {
            string result = "";
            string post = "txtHoldName={1}&txtUserName={2}&txtUserPass={4}&txtReUserPass={5}&sltHoldType={3}&txtContacter=&txtContacterTel=&viewWXRenewals=1&sltProvince=0&txtAddress=&txtRemark=&hid_ParentHoldID={0}&hid_HoldID=&hid_Province=&hid_City=&hid_Region=&hid_GroupHoldIds=&hid_GroupHoldNames=";
            string postWithParam = string.Format(post, parentid, displayname, loginname, usertype, password, password);
           result = PostDataToUrl(postWithParam, "http://demo.m-m10010.com/hold/Info");
           if (string.IsNullOrEmpty(result))
               result = "失败";

           return result.Split(';')[0];
        }
        private void backgroundWorker7_DoWork(object sender, DoWorkEventArgs e)
        {
            string username = this.richTextBox1.Text;
            string[] usernamelist = username.Split('\n');
            int count = usernamelist.Count();
            string result = "";
            InvokeHelper.Set(this.richTextBox1, "Text", "");

            string selectedUserId = treeView1.SelectedNode.Tag.ToString();
            if(selectedUserId == "1")           
            {
                DisplayAndLog("不能在大账号下建子账号\r\n", true);
                return;
            }
            DisplayAndLog(treeView1.SelectedNode.Text.ToString() + "\r\n", true);
            string displayname = "";
            string loginname = "";
            string usertype = "6";
            string password = "8989123";
            for (int i=0; i < count; i++)
            {
                if(string.IsNullOrEmpty(usernamelist[i].Trim()))
                    continue;

                switch(usernamelist[i].Split(',').Length)
                    {
                    case 1:
                            displayname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[0]);
                            result = CreateUser(selectedUserId, displayname, displayname, usertype, password);
                            break;
                    case 2:
                            displayname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[0]);
                            loginname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[1]);
                            result = CreateUser(selectedUserId, displayname, loginname, usertype, password);
                            break;
                    case 3:
                            displayname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[0]);
                            loginname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[1]);
                            usertype = usernamelist[i].Trim().Split(',')[2];
                            result = CreateUser(selectedUserId, displayname, loginname, usertype, password);
                            break;
                    case 4:
                            displayname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[0]);
                            loginname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[1]);
                            usertype = usernamelist[i].Trim().Split(',')[2];
                            password = usernamelist[i].Trim().Split(',')[3];
                            result = CreateUser(selectedUserId, displayname, loginname, usertype, password);
                            break;

                }
                DisplayAndLog(usernamelist[i] + result+"\r\n", true);
            }
        }

        private void backgroundWorker7_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button14.Enabled = true;
        }

        private void backgroundWorker8_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button15.Enabled = true;
        }

        private string CreateUserRenewals(TreeNode tn)
        {
            string result = "";
            if (tn == null) 
                return result;



            foreach (TreeNode tns in tn.Nodes)
            {
                CreateUserRenewals(tns);
            }

            string post = "HoldName=aaaa&WXPayId=0&HoldId={0}";
            string postWithParam = string.Format(post, tn.Tag.ToString()    );
            result = PostDataToUrl(postWithParam, "http://demo.m-m10010.com/SysWxPay/Info");
            if (string.IsNullOrEmpty(result))
            {
                 DisplayAndLog( tn.Text + "失败\r\n",true);
                    
            }
            else
            {
                DisplayAndLog(tn.Text + result.Split(';')[0] + "\r\n", true);
            }
                       

            return result;
        }

        private void backgroundWorker8_DoWork(object sender, DoWorkEventArgs e)
        {
            string selectedUserId = treeView1.SelectedNode.Tag.ToString();
            if (selectedUserId == "1")
            {
                DisplayAndLog("怎么你选了大账号\r\n", true);
                return;
            }

            TreeNode selectednode = treeView1.SelectedNode;
            CreateUserRenewals(selectednode);

        }

        private void button16_Click(object sender, EventArgs e)
        {
            backgroundWorker9.RunWorkerAsync();

        }
        private string PringUserTree(TreeNode tn)
        {
            string result = "";
            if (tn == null)
                return result;



            foreach (TreeNode tns in tn.Nodes)
            {
                PringUserTree(tns);
            }


            DisplayAndLog(tn.Text + "\t" + tn.Tag    + "\r\n", true);
       

            return result;
        }
        private void backgroundWorker9_DoWork(object sender, DoWorkEventArgs e)
        {
            string selectedUserId = treeView1.SelectedNode.Tag.ToString();
 

            TreeNode selectednode = treeView1.SelectedNode;
            PringUserTree(selectednode);
        }
    }
}



/*
 * 平台登录 
 * http://demo.m-m10010.com/User/login 
 * userName=XXX&userPwd=XXXXX&remember=on
 * Set-Cookie: UserCookie=UserID=1&UserName=admin&UserType=1&HoldID=1&HoldName=%e8%bf%90%e8%90%a5%e4%b8%ad%e5%bf%83&HoldLevel=1&HoldType=4&Token=UJWGJQSGDVMXVX6MHT082WWNCIS9TM22&LoginFromType=1&OEMClient=; path=/
 * 获取用户列表
 * http://demo.m-m10010.com/api/allholdnodes?id=1&parent=1
 * http://demo.m-m10010.com/api/allholdnodes?id=1&parent=1&nodeListType=1&NJholdId=0&notIncludeCount=false
 * 
 * 用户的可续费套餐权限
 * http://demo.m-m10010.com/api/HoldRenewalsList/1496?allShow=true 
 * 
 * 套餐的可续费套餐
 * http://demo.m-m10010.com/api/RenewalsPackage?id=17547
 * 
 * 月用量报表
 * http://demo.m-m10010.com/api/ReportFlowHold?holdId=5877
 
 
 */
﻿using System;
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
        string sVer = "V1.0.6";
 
        string sApiUrl = "http://demo.m-m10010.com/";
        string sLogFileName = "";
        string slogfilepath = "";
        private object filelocker = new object();
        public List<string> SearchResult ;
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
            result = GetResponseSafe("http://demo.m-m10010.com/api/allholdnodes?nodeListType=1&NJholdId=0&notIncludeCount=false&id=" + Program.UserId + "&parent=" + Program.UserId);
            RefreshUserTree(result, isDisplayGhostUser);

        }
        public void RefreshUserTree(string a,bool isDisplayGhostUser)
        {
            string errormsg ="";
            if (a == "")
                return; 
            TreeNode node1;
            TreeNode nodeParent;
            int id = 0;
            ParamDefine.UserTree userTree = JsonConvert.DeserializeObject<ParamDefine.UserTree>(a);
            for (int i = 0; i < userTree.result.Count; i++)
            {
                if (treeView1.Nodes.Count == 0)
                {
                    node1 = new TreeNode();
                    node1.Tag = userTree.result[i].id.ToString();
                    node1.Text = userTree.result[i].name.ToString();
                    treeView1.Nodes.Add(node1);
                    
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
                    nodeParent.Nodes.Add(node1);
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
            DisplayAndLog(errormsg,true);

        }
        public void button1_Click(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Clear();
            GetUserTree(false);
        }
        public string GetShowAllStr( )
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
            
            if (id == "")
            {
                DisplayAndLog("ID不合法\r\n", true);
                return result;
            }
            string url = sApiUrl + "api/RenewalsPackage?id=" + id ;
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
            foreach (ParamDefine.RenewalsPackageItem rpi in rp.result)
            {
                result += "@\t\t\t\t\t└--" + rpi.PackageName.PadRight(20) + "\t@R" + rpi.UnitPrice + "\t@R" + rpi.BackPrice + "\r\n";
            }


            return result;
        }
 

        public string GetHoldRenewalList(string id)
        {
            string response = "";
            string result = "";
            string tmp = ""; 
            string[] pkgDescArr = new string[10]; // 3-年，2-月，1-叠加，4-加油 0-电信移动
            bool isGetPkgRenelwalPkg = Convert.ToBoolean(InvokeHelper.Get(this.checkBox2, "Checked"));  
            if (id == "")
            {
                DisplayAndLog("ID不合法\r\n",true);
                return result;
            }
            string url = sApiUrl + "api/HoldRenewalsList/" + id + GetShowAllStr();
            response = GetResponseSafe(url);
            if(response == "")
            {
                DisplayAndLog("holdId为"+ id + "查不到啊亲\r\n",true);
                return result;
            }
            ParamDefine.HoldRenewalsList hrl = JsonConvert.DeserializeObject<ParamDefine.HoldRenewalsList>(response);
            foreach(ParamDefine.HoldList user in hrl.result )
            {
                foreach (ParamDefine.PackageListItem pkg in user.PackageList)
                {
                    pkgDescArr[pkg.Type] += ("@" + user.HoldName.PadRight(20) + "\tCUCC\t@B" + pkg.PackageName.PadRight(20) + "\t@" + pkg.UnitPrice + "\t" + pkg.BackPrice + "\t" );
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
                for (int i = 0; i < pkgDescArr.Count();i++ )
                {
                    result += pkgDescArr[i];
                    pkgDescArr[i] = "";
                }
                foreach (ParamDefine.PackageListItem pkg in user.YDPackageList)
                {
                    pkgDescArr[pkg.Type] += ("@" + user.HoldName.PadRight(20) + "\tCMCC\t@B" + pkg.PackageName.PadRight(20) + "\t@" + pkg.UnitPrice + "\t" + pkg.BackPrice + "\t");
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
                    pkgDescArr[pkg.Type] += ("@" + user.HoldName.PadRight(20) + "\tCTCC\t@B" + pkg.PackageName.PadRight(20) + "\t@" + pkg.UnitPrice + "\t" + pkg.BackPrice + "\t");
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
        private delegate void SetTextHandler(string text,Color co);
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
        public void SetText(string text,Color co)
        {

            if (richTextBox1.InvokeRequired == true)
            {
                SetTextHandler set = new SetTextHandler(SetText);//委托的方法参数应和SetText一致
                richTextBox1.Invoke(set, new object[] { text ,co}); //此方法第二参数用于传入方法,代替形参text
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
            foreach(string a in str)
            {
                i++;
                if (a=="")
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
                            DisplayAndLog(  a.Substring(1), isDisplay, Color.Blue);
                            break;         
                        case "R":          
                            DisplayAndLog(  a.Substring(1), isDisplay, Color.Red);
                            break;         
                        default:           
                            DisplayAndLog(  a.Substring(0), isDisplay, Color.Black);
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
                    SetText(s,color);
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

        private void LoopSearch(TreeNode tn,string key)
        {
            if (string.IsNullOrEmpty(key))
                return;
            //1.将当前节点显示到lable上
            if (tn.Text.IndexOf(key) >= 0)
                SearchResult.Add(tn.Text); 
            foreach (TreeNode tnSub in tn.Nodes)
            {
                LoopSearch(tnSub,key);
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
                DisplayAndLog(a+"\r\n", false);
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
            DisplayAndLogBatch(e.Result.ToString(), true);

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
        private string GetLastMonthBackMoney(string id,string period)
        {
            double backmoneySum = 0;
            double renewalsSum = 0;
            string result = "";
            string tmp = "";
            string url = ""; 
            switch(period)
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
            foreach (ParamDefine.BackMoneyResultItem bmr in bmlist.result)
            {
                tmp += bmr.iccid + "\t" + bmr.packageName + "\t" + bmr.renewalsPrice + "\t" + bmr.backPrice + "\t" + bmr.renewalsTime + "\r\n";
                backmoneySum +=  (bmr.backPrice);
                renewalsSum += bmr.renewalsPrice;
            }

            result = "ID为" + id + "\t笔数为\t" + bmlist.result.Count().ToString() + "\t总续费为\t" + renewalsSum.ToString() + "\t总返利为\t" + backmoneySum.ToString() + "\r\n" + tmp;
            return result;
        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            string id = "";
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
            DisplayAndLogBatch(treeView1.SelectedNode.Text.ToString(),true);
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

            this.backgroundWorker4.RunWorkerAsync();
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            string id = "";
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
            DisplayAndLogBatch(treeView1.SelectedNode.Text.ToString()+"\t", true);
            e.Result = GetRenewalsTotal(id); 

        }

        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.button8.Text = "续费汇总";
            this.button8.Enabled = true;
            DisplayAndLogBatch(e.Result.ToString(), true);
            DisplayAndLogBatch("------------------------------------------------------------------------\r\n", true);
        }
        private string GetRenewalsTotal(string id)
        {
            double backmoneySum = 0;
            double renewalsSum = 0;
            string result = "";
            string tmp = "";
            string url = "";

            url = "http://demo.m-m10010.com/api/NewReportRenewalsTotalForHoldInfo?holdId=" + id;
                   
            string response = GetResponseSafe(url);
            if (response == "")
            {
                DisplayAndLog("holdId为" + id + "查不到啊亲\r\n", true);
                return result;
            }
            ParamDefine.RenewalsTotal rt = JsonConvert.DeserializeObject<ParamDefine.RenewalsTotal>(response);
            ParamDefine.RenewalsTotalResult rtresult = rt.result;

            tmp += "总卡数\t" + rtresult.allCount +
                "\t有效续费卡\t" + rtresult.renewalsCount +
                "\t总续费金额\t" + rtresult.renewalsAmount +
                "\t总返利金额\t" + rtresult.backAmount +
                "\t累计ARPU\t" + (rtresult.renewalsAmount / rtresult.renewalsCount).ToString("#0.00") +
                "\t有效续费率\t" + string.Format("{0:0.00%}", ((double)rtresult.renewalsCount / (rtresult.ltActivatedCount + rtresult.ltStopCount))) +
                "\t续费率\t"    +  string.Format("{0:0.00%}", ((double)rtresult.renewalsCount / (rtresult.allCount)))  +
                "\t使用率\t"    +  string.Format("{0:0.00%}", ((double)(rtresult.ltActivatedCount + rtresult.ltStopCount) / rtresult.ltAllCount))  +
                "\t脱网率\t"    +  string.Format("{0:0.00%}", ((double)rtresult.ltOutCount / (rtresult.ltActivatedCount + rtresult.ltStopCount)))  +
                "\t复充率\t"    +  string.Format("{0:0.00%}", ((double)rtresult.twiceRenewalsCount / (rtresult.renewalsCount))) + "\r\n";

            result =  tmp;
            return result;


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
 
 
 */
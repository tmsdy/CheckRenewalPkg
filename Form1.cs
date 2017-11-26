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
using Newtonsoft.Json.Linq;
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
        Dictionary<string, string> LTPkgIdList = new Dictionary<string, string>();
        string[] skipUserList = { "麦谷测试电信卡", "MG测试电信卡", "续费转仓", "0531081测试勿动", "娜姐", "接口调试(联通)", "麦谷内部人员", "ZYR_麦联宝测试", "ZYR_研发部调试卡" ,
                                "ZYR_客服体验", "ZYR_其他人员试用", "SDY_体验测试", "ZW_后视镜测试", "123", "123-01", "123-02", "实名奖励套餐测试", "ZYR_内部测试卡",
                                "ZYR_麦谷测试_YD", "ZYR_麦谷测试_DX", "ZYR_麦谷测试_LT","Jaffe_S85", "海如测试", "陈碧淼", "MG娜姐", "Telecom_S5"};
        public string sApiUrl = Program.sGloableDomailUrl;
        public string sOpenUrl = "http://open.m-m10010.com";
        public string sLogFileName = "";
        public string slogfilepath = "";
        private object filelocker = new object();
        public List<string> SearchResult;

        public static Encoding RequestEncoding = _Encoding.UTF8;
        public static Encoding ResponseEncoding = _Encoding.UTF8;

        public struct renewalsPeriod
        {
            public string whichway;
            public string stime;
            public string etime;
            public string desc;
            public double days;
        }
        public struct renewalsPeriodVS
        {
            public string whichway;
            public string stime;
            public string etime;
            public string desc;
            public double days;
            public string stime_vs  ;
            public string etime_vs  ; 
            public double days_vs  ;
            public string period;
        }
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }


        private void InitPayeeCombox()
        {
            List<KeyValuePair<object, string>> dit = new List<KeyValuePair<object, string>>();
            dit.Add(new KeyValuePair<object, string>("", "所有收款方"));
            dit.Add(new KeyValuePair<object, string>("MAPGOO", "MAPGOO"));
            dit.Add(new KeyValuePair<object, string>("HUAZ", "HUAZ"));
            dit.Add(new KeyValuePair<object, string>("WX_HUAZ2", "WX_HUAZ2"));
            dit.Add(new KeyValuePair<object, string>("MAPGOO_CDB", "MAPGOO_CDB"));
            dit.Add(new KeyValuePair<object, string>("SZBY", "SZBY"));
            dit.Add(new KeyValuePair<object, string>("WKTX", "WKTX"));
            dit.Add(new KeyValuePair<object, string>("DXRJ", "DXRJ"));
            dit.Add(new KeyValuePair<object, string>("TPZL", "TPZL"));
            dit.Add(new KeyValuePair<object, string>("WFDW", "WFDW"));
            dit.Add(new KeyValuePair<object, string>("BJQBK", "BJQBK"));
            dit.Add(new KeyValuePair<object, string>("SZPFD", "SZPFD"));
            dit.Add(new KeyValuePair<object, string>("QDLGWX", "QDLGWX"));
            dit.Add(new KeyValuePair<object, string>("APK", "APK"));
            dit.Add(new KeyValuePair<object, string>("HDSC888", "HDSC888"));
            dit.Add(new KeyValuePair<object, string>("ZXTX", "ZXTX"));
            dit.Add(new KeyValuePair<object, string>("SAIW", "SAIW"));
            dit.Add(new KeyValuePair<object, string>("HUAZ", "HUAZ"));
            dit.Add(new KeyValuePair<object, string>("GLW", "GLW"));
            dit.Add(new KeyValuePair<object, string>("WST", "WST"));
            dit.Add(new KeyValuePair<object, string>("TEST_N", "TEST_N"));
            dit.Add(new KeyValuePair<object, string>("JKTS", "JKTS"));
            dit.Add(new KeyValuePair<object, string>("JKTS_LT", "JKTS_LT"));
            dit.Add(new KeyValuePair<object, string>("BLHDZ", "BLHDZ"));
            dit.Add(new KeyValuePair<object, string>("KLD", "KLD"));
            dit.Add(new KeyValuePair<object, string>("TEST", "TEST"));
            dit.Add(new KeyValuePair<object, string>("FHXT", "FHXT"));
            dit.Add(new KeyValuePair<object, string>("SZTL", "SZTL"));
            dit.Add(new KeyValuePair<object, string>("JLXKDZ", "JLXKDZ"));
            dit.Add(new KeyValuePair<object, string>("XUFEIZHUANGCANG", "XUFEIZHUANGCANG"));
            dit.Add(new KeyValuePair<object, string>("TZY", "TZY"));
            dit.Add(new KeyValuePair<object, string>("JWJWJWJWJW", "JWJWJWJWJW"));
            dit.Add(new KeyValuePair<object, string>("AMTX", "AMTX"));
            dit.Add(new KeyValuePair<object, string>("JACLW", "JACLW"));
            dit.Add(new KeyValuePair<object, string>("RWY", "RWY"));
            dit.Add(new KeyValuePair<object, string>("FYD", "FYD"));
            dit.Add(new KeyValuePair<object, string>("HBKJ", "HBKJ"));
            dit.Add(new KeyValuePair<object, string>("ZXZM", "ZXZM"));
            dit.Add(new KeyValuePair<object, string>("HXQBTX", "HXQBTX"));
            dit.Add(new KeyValuePair<object, string>("WLZSH", "WLZSH"));
            dit.Add(new KeyValuePair<object, string>("ceshi", "ceshi"));
            dit.Add(new KeyValuePair<object, string>("SZSL", "SZSL"));
            dit.Add(new KeyValuePair<object, string>("TXCL", "TXCL"));
            dit.Add(new KeyValuePair<object, string>("BJAYC", "BJAYC"));
            dit.Add(new KeyValuePair<object, string>("CLB", "CLB"));
            dit.Add(new KeyValuePair<object, string>("SKR", "SKR"));
            dit.Add(new KeyValuePair<object, string>("XYCLW", "XYCLW"));
            dit.Add(new KeyValuePair<object, string>("RONGJM", "RONGJM"));
            dit.Add(new KeyValuePair<object, string>("CHUXIU", "CHUXIU"));
            dit.Add(new KeyValuePair<object, string>("WSTCS", "WSTCS"));
            dit.Add(new KeyValuePair<object, string>("DINGDING", "DINGDING"));
            dit.Add(new KeyValuePair<object, string>("GDTEST", "GDTEST"));
            dit.Add(new KeyValuePair<object, string>("MTB", "MTB"));
            dit.Add(new KeyValuePair<object, string>("GDDT", "GDDT"));
            dit.Add(new KeyValuePair<object, string>("JSKML", "JSKML"));
            dit.Add(new KeyValuePair<object, string>("QJJ", "QJJ"));
            dit.Add(new KeyValuePair<object, string>("水电费", "水电费"));
            dit.Add(new KeyValuePair<object, string>("KEKEHUD", "KEKEHUD"));
            dit.Add(new KeyValuePair<object, string>("KKSS", "KKSS"));
            dit.Add(new KeyValuePair<object, string>("CWHY", "CWHY"));
            dit.Add(new KeyValuePair<object, string>("SDT", "SDT"));
            dit.Add(new KeyValuePair<object, string>("XMLONG", "XMLONG"));
            dit.Add(new KeyValuePair<object, string>("ZZBJZZBJ", "ZZBJZZBJ"));
            dit.Add(new KeyValuePair<object, string>("KEKEYDHUD", "KEKEYDHUD"));
            dit.Add(new KeyValuePair<object, string>("SKYWORTH", "SKYWORTH"));
            dit.Add(new KeyValuePair<object, string>("SDT", "SDT"));
            dit.Add(new KeyValuePair<object, string>("LTT123", "LTT123"));
            dit.Add(new KeyValuePair<object, string>("TEST", "TEST"));
            dit.Add(new KeyValuePair<object, string>("HCQCDZ", "HCQCDZ"));
            dit.Add(new KeyValuePair<object, string>("TURUI", "TURUI"));
            dit.Add(new KeyValuePair<object, string>("HGYD", "HGYD"));
            dit.Add(new KeyValuePair<object, string>("FENGZY", "FENGZY"));
            dit.Add(new KeyValuePair<object, string>("HZHD", "HZHD"));
            dit.Add(new KeyValuePair<object, string>("WXCF", "WXCF"));
            dit.Add(new KeyValuePair<object, string>("CLBYD", "CLBYD"));
            dit.Add(new KeyValuePair<object, string>("SDZY", "SDZY"));
            dit.Add(new KeyValuePair<object, string>("YLTX", "YLTX"));
            dit.Add(new KeyValuePair<object, string>("ALITEST", "ALITEST"));
            dit.Add(new KeyValuePair<object, string>("MLRC", "MLRC"));
            dit.Add(new KeyValuePair<object, string>("YLK_OFFL", "YLK_OFFL"));
            dit.Add(new KeyValuePair<object, string>("GZYYXXKJ", "GZYYXXKJ"));
            dit.Add(new KeyValuePair<object, string>("SHND", "SHND"));
            dit.Add(new KeyValuePair<object, string>("SUOTIAN", "SUOTIAN"));
            dit.Add(new KeyValuePair<object, string>("CHELIZI", "CHELIZI"));
            dit.Add(new KeyValuePair<object, string>("yileike", "yileike"));
             
            this.comboBox2.DataSource = dit;
            this.comboBox2.DisplayMember = "Value";
            this.comboBox2.ValueMember = "Key"; ;
            this.comboBox2.SelectedIndex = 0;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text += Program.sVer;
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
            treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            InitPayeeCombox();
            GetUserTree(false);
            this.radioButton1.Checked = true;
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

            string id = treeView1.SelectedNode.Tag.ToString();
            this.treeView1.Nodes.Clear();
            GetUserTree(false);
            treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], id);
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
            string url = sApiUrl + "/api/RenewalsPackage?id=" + id;
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
        public string GetHoldRenewalList(string id, string method)
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
            string showallStr = GetShowAllStr();
            string url = sApiUrl + "/api/HoldRenewalsList/" + id + showallStr;
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
                    pkgDescArr[pkg.Type] += (pkg.ShowUnitPrice == "是") ? "\t@R显示单价" : "\t@隐藏单价";  
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
                    pkgDescArr[pkg.Type] += (pkg.ShowUnitPrice == "是") ? "\t@R显示单价" : "\t@隐藏单价";  
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
                    pkgDescArr[pkg.Type] += (pkg.ShowUnitPrice == "是") ? "\t@R显示单价" : "\t@隐藏单价";  
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
                //加上这个break则只显示第1个用户的
                if (showallStr.IndexOf("false") > 0  )
                {
                    break;
                }
            }
            return result;

        }
        private void button2_Click(object sender, EventArgs e)
        { 
            this.button2.Enabled = false;
            this.button24.Enabled = false;
            this.backgroundWorker1.RunWorkerAsync("singal");

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
            string whichway = e.Argument.ToString();
            if( whichway == "multi")
            {
                StringBuilder  tmp = new StringBuilder();

                List<string> idlist = configManager.GetValueStrList("Userlist", "pkglistCustomers");
                foreach (string idid in idlist)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        tmp.Append("未知用户ID为" + idid + "\t" + GetHoldRenewalList(idid, whichway));
                        continue;
                    }
                    tmp.Append(GetHoldRenewalList(idid, whichway));
                }


                e.Result = tmp.ToString();

            }
            else
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
                id = treeView1.SelectedNode.Tag.ToString();
                e.Result = GetHoldRenewalList(id, whichway);

            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
             
            this.button24.Enabled = true;
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
            this.button28.Enabled = false;
 
            this.button6.Enabled = false;

            this.backgroundWorker2.RunWorkerAsync("single");
        }
        private void button28_Click(object sender, EventArgs e)
        {
            this.button28.Enabled = false;

            this.button6.Enabled = false;

            this.backgroundWorker2.RunWorkerAsync("multi");
        }


        private string GetLastMonthBackMoney(string id, string period,bool isPrintDetail)
        {
            double backmoneySum = 0;
            double renewalsSum = 0;
            string result = "";
            string tmp = "\r\n";
            string url = "";
            switch (period)
            {
                case "lastmonth":

                    url = sOpenUrl + "/api/GetHoldMonthAmountList?period=lastmonth&holdId=" + id;
                    break;

                default:
                    url =sOpenUrl + "/api/GetHoldMonthAmountList?holdId=" + id;
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
            if (isPrintDetail)
            {
                result = tmp + "ID为" + id + "\t笔数为\t" + bmlist.result.Count().ToString() + "\t总续费为\t" + renewalsSum.ToString("0.00") + "\t总返利为\t" + backmoneySum.ToString("0.00") + "\t" + (backmoneySum / renewalsSum).ToString("0.00%") + "\r\n";
            }
            else
            {
                result =  "\tID为" + id + "\t笔数为\t" + bmlist.result.Count().ToString() + "\t总续费为\t" + renewalsSum.ToString("0.00") + "\t总返利为\t" + backmoneySum.ToString("0.00") + "\t" + (backmoneySum / renewalsSum).ToString("0.00%") + "\r\n";
            }
            return result;
        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            string id = "";
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
                id = treeView1.SelectedNode.Tag.ToString();
                DisplayAndLogBatch(GetUserName(treeView1.SelectedNode.Text.ToString()), true);
                DisplayAndLogBatch(GetLastMonthBackMoney(id, "lastmonth", true),true);


            }
            else
            {
                List<string> idlist = configManager.GetValueStrList("Userlist", "lastmonthbackmoney");
                foreach (string idid in idlist)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog(GetActivaCountDay("未知用户ID为" + idid, idid), true);
                        continue;
                    }

                    //treeView1.Select();
                    DisplayAndLogBatch(GetUserName(treeView1.SelectedNode.Text.ToString()), true);
                    DisplayAndLogBatch(GetLastMonthBackMoney(idid, "lastmonth", false), true);
                }

            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.button28.Enabled = true;
            this.button6.Enabled = true;
            //DisplayAndLogBatch(e.Result.ToString(), true);
            DisplayAndLogBatch("-----\t-----\t-----\t-----\t-----\t-----\t-----\t-----\r\n", true);
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
            DisplayAndLogBatch(GetUserName(treeView1.SelectedNode.Text.ToString()), true);
            e.Result = GetLastMonthBackMoney(id, "",true);

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
            DisplayAndLogBatch("-----\t-----\t-----\t-----\t-----\t-----\t-----\t-----\r\n", true);

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
                      "脱网卡\t" +
                      "总续费金额\t" +
                      "总返利金额\t" +
                      "单卡ARPU\t" +
                      "续费率\t" +
                      "有效续费率\t" +
                      "使用率\t" +
                      "脱网率\t" +
                      "复充率\r\n";
                DisplayAndLog(tmp, true);
                id = treeView1.SelectedNode.Tag.ToString();
                DisplayAndLog(GetUserName(treeView1.SelectedNode.Text.ToString()) , true);
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
                //H_VIP50,H_VIP付费,H已续费,H未续费,H海南成杰汽车,H江圳_爱培科,H卡卡_凌度,安信翔,八零后,北斗传奇,丁威特,索行电子,途龙,欣万和,卓派,东莞润禾车品电子C,天之眼_C,欣和先知C,众创伟业C,艾米,保速捷C,凌途C,四维星图C,泰瑞视C,3G绑带_L,威仕特麦联宝
                //"3410","2987","3411","3412","3369","1230","1272","2727","2330","5501","3395","1280","3164","4001","2199","6054","5236","6257","5341","3695","6710","6723","6729","6837","5129","4125",

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
                      "脱网卡\t" +
                          "总续费金额\t" +
                          "总返利金额\t" +
                          "单卡ARPU\t" +
                          "续费率\t" +
                          "有效续费率\t" +
                          "使用率\t" +
                          "脱网率\t" +
                          "复充率\r\n";
                DisplayAndLog(tmp, true);
                //string[] idlist = {
                //                      "6779","5287","5521","5288","5377","5108","4715","4716","5015","5013","5014","5116","7009","7987","7837",
                //                    "6367","6423","6947","6314","6975","5877","7242","6945","5905","5891","6362","5523","5926","6734","5411","5362","6317",
                //                    "5519","6304",
                //                    "6424","5361","6780","5293","5107","4933","5018","5019","4927","5020","7008","7269","6556","6889",
                //                    "6980","6750","4988","6787","5426","4026","4382","4108","2201","4982","6791","7460","7868","8058","7329","7632",
                //                   "3410","2987","3411","3412","3369","1230","1272","2727","2330","5501","3395","1280","3164","4001","2199","6054","5236","6257","5341","3695","6710","6723","6729","6837","5129","4125",
                //                    "6087","6052","4978","6212","6231","5342","4682","4717","6569",
                //                    "3686","6063","2700","4087","3930","4817","6773","4035","3406","4923","3815","2467","3936","4897","4858"  
                //                  };

                List<string> idlist = configManager.GetValueStrList("Userlist", "alluserlist");
                foreach (string idid in idlist)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog("未知用户ID为" + idid  + GetRenewalsTotal(idid,false), true);
                        continue;
                    }

                    //treeView1.Select();
                    
                    DisplayAndLog(GetUserName(treeView1.SelectedNode.Text.ToString()), true);
                    DisplayAndLog(GetRenewalsTotal(idid,false), true);
                }

            }

            e.Result = whichway;

        }

        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Result.ToString() == "single")
            {
                this.button9.Text = "单续费汇总";
                this.button9.Enabled = true; 
            }
            else
            {
                this.button8.Text = "*续费汇总";
                this.button8.Enabled = true; 
            }

            DisplayAndLogBatch("-----\t-----\t-----\t-----\t-----\t-----\t-----\t-----\r\n", true);
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
            //          "\t单卡ARPU\t" + (rtresult.renewalsAmount / rtresult.renewalsCount).ToString("#0.00") +
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
                       "\t" + rtresult.ltOutCount +
                       "\t" + rtresult.renewalsAmount.ToString("#0") +
                       "\t" + rtresult.backAmount.ToString("#0") +
                       "\t" + (rtresult.renewalsAmount / rtresult.renewalsCount).ToString("#0.00") +
                       "\t" + string.Format("{0:0.00%}", ((double)rtresult.renewalsCount / (rtresult.ltActivatedCount + rtresult.ltStopCount))) +
                       "\t" + string.Format("{0:0.00%}", ((double)rtresult.renewalsCount / (rtresult.renewalsCount+ rtresult.ltOutCount))) +
                       "\t" + string.Format("{0:0.00%}", ((double)(rtresult.ltActivatedCount + rtresult.ltStopCount) / rtresult.ltAllCount)) +
                       "\t" + string.Format("{0:0.00%}", ((double)rtresult.ltOutCount / (rtresult.allCount))) +
                       "\t" + string.Format("{0:0.00%}", ((double)rtresult.twiceRenewalsCount / (rtresult.renewalsCount))) + "\t";
         
            //}

            //临时屏蔽掉月度汇总 2017-09-29
            tmp += "\r\n";
            //tmp += GetMonthRenewalsTotal(id);
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
        public void  CalcAverage(List<int> countlist,List<double> usagelist)
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
        public string GetUsageTotal(string username,string id, bool isDisplayTitle)
        {
            string result = "";
            string tmp = "";
            string url = "";
            string dateStr = username + "\t日期\t";
            string amountUsageStr = username + "\t总用量\t";
            string validCountStr = username + "\t有效卡\t";
            string validAvgUsageStr = username + "\t平均用量\t";

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
                return username+"查不到结果\r\n";

            foreach (ParamDefine.DayStatisticsListItem daylist in frrResult.dayStatisticsList)
            {
                dateStr += daylist.statDay + "\t";
                amountUsageStr += daylist.amountUsage.ToString("0.00") + "\t";
                validCountStr += daylist.validCount + "\t";
                validAvgUsageStr += daylist.validAvgUsage.ToString("0.00") + "\t";
               //tmp += daylist.statDay + "\t" + daylist.amountUsage.ToString("0.00") + "\t" + daylist.validCount + "\t" + daylist.validAvgUsage.ToString("0.00") + "\t";
                //tmp += daylist.statDay + "\t" + daylist.amountUsage.ToString("0.00") + "\t" + daylist.validCount + "\t";
            }

            tmp += dateStr + "\r\n" + amountUsageStr + "\r\n" + validCountStr + "\r\n" + validAvgUsageStr + "\r\n";
            return tmp;


        }
        public string GetUsageTotal(string username,string id,bool isDisplayTitle,List<int> countlist,List<double> usagelist)
        {
            string result = "";
            string tmp = "";
            string url = "";
            string dateStr = username + "\t日期\t";
            string amountUsageStr = username + "\t总用量\t";
            string validCountStr = username + "\t有效卡\t";
            string validAvgUsageStr = username + "\t平均用量\t";
 


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
                return username+"查不到结果\r\n";

             foreach (ParamDefine.DayStatisticsListItem daylist in frrResult.dayStatisticsList)
             {
                 dateStr += daylist.statDay + "\t";
                 amountUsageStr += daylist.amountUsage.ToString("0.00") + "\t";
                 validCountStr += daylist.validCount + "\t";
                 validAvgUsageStr += daylist.validAvgUsage.ToString("0.00") + "\t";
                 //tmp += daylist.statDay + "\t" + daylist.amountUsage.ToString("0.00") + "\t" + daylist.validCount  + "\t";
                 countlist.Add(Convert.ToInt32(daylist.validCount));
                 usagelist.Add(daylist.amountUsage);
             }

             tmp += dateStr + "\r\n" + amountUsageStr + "\r\n" + validCountStr + "\r\n" + validAvgUsageStr + "\r\n";
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


            string username = "";
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
                username =GetUserName(treeView1.SelectedNode.Text.ToString()) ;

                DisplayAndLog(GetUsageTotal(username,id, true), true);


            }
            else
            {

                DisplayAndLog(tmp, true);
                //D导航,LB,后视镜V,艾米,3G绑带,WST_AL,威仕特麦联宝,M电商S,M电商V,小流量V,小流量体验,,,,,,,,, 
                //string[] idlistVec = { "4123", "1323", "1281", "3695", "5129", "4717", "4125"  };
                //string[] idlistMifi = {   "5467", "2332"  };
                //string[] idlistPos = { "2898", "2673" };


                List<string> idlistVec = configManager.GetValueStrList("Userlist", "UsageUserVec");
                List<string> idlistMifi = configManager.GetValueStrList("Userlist", "UsageUserMifi");
                List<string> idlistPos = configManager.GetValueStrList("Userlist", "UsageUserPos");


                foreach (string idid in idlistVec)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog("未知用户ID为" + idid +"\t"  , true);
                        continue;
                    }

                    //DisplayAndLog(GetUserName(treeView1.SelectedNode.Text.ToString()) + "\t", true);
                    username = GetUserName(treeView1.SelectedNode.Text.ToString());
                    DisplayAndLog(GetUsageTotal(username,idid, false, Countlist, Usagelist), true);
                }
                DisplayAndLog("-------车载-------\r\n", true);
                CalcAverage(Countlist, Usagelist);
                foreach (string idid in idlistMifi)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog("未知用户ID为" + idid + "\t"  , true);
                        continue;
                    }

                    //DisplayAndLog(GetUserName(treeView1.SelectedNode.Text.ToString()) + "\t", true);
                    username = GetUserName(treeView1.SelectedNode.Text.ToString());
                    DisplayAndLog(GetUsageTotal(username, idid, false, Countlist, Usagelist), true);
                }
                DisplayAndLog("-------MIFi-------\r\n", true);
                CalcAverage(Countlist, Usagelist);
                foreach (string idid in idlistPos)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog("未知用户ID为" + idid + "\t"  , true);
                        continue;
                    }

                    //DisplayAndLog(GetUserName(treeView1.SelectedNode.Text.ToString()) + "\t", true);
                    username = GetUserName(treeView1.SelectedNode.Text.ToString());
                    DisplayAndLog(GetUsageTotal(username, idid, false, Countlist, Usagelist), true);
                }
                DisplayAndLog("-------POS-------\r\n", true);
                CalcAverage(Countlist, Usagelist);
            }

            e.Result = whichway;
        }

        private void backgroundWorker5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result.ToString() == "single")
            {
                this.button11.Text = "单用量汇总";
                this.button11.Enabled = true;
            }
            else
            {
                this.button12.Text = "*用量汇总";
                this.button12.Enabled = true;
            }

            DisplayAndLogBatch("-----\t-----\t-----\t-----\t-----\t-----\t-----\t-----\r\n", true);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.button12.Text = "获取中";
            this.button12.Enabled = false;

            this.backgroundWorker5.RunWorkerAsync("multi");
        }
        #region 老续费
        private void button13_Click(object sender, EventArgs e)
        {

            this.button13.Enabled = false;
            this.button29.Enabled = false;

            this.backgroundWorker6.RunWorkerAsync("single");
        }
        private void button29_Click(object sender, EventArgs e)
        {
            this.button13.Enabled = false;
            this.button29.Enabled = false;

            this.backgroundWorker6.RunWorkerAsync("multi");

        }


        public string GetOldRenewals(string username,string id, bool isDisplayTitle)
        {
            string result = "";
            string tmp = "";
            string url = "";
            string payee = InvokeHelper.Get(this.comboBox2, "SelectedValue").ToString();
            url = sApiUrl + "/api/ReportRenewalsTotalForDay?payee=" + payee +"&packageId=0&holdId=" + id;

            string response = GetResponseSafe(url);
            if (response == "")
            {
                DisplayAndLog(username+ "holdId为" + id + "查不到啊亲\r\n", true);
                return result;
            }
            ParamDefine.OldRenewalsRoot orr = JsonConvert.DeserializeObject<ParamDefine.OldRenewalsRoot>(response);

            if (orr == null)
                return username+"查不到结果\r\n";

            foreach (ParamDefine.OldRenewalsRootResultItem orritem in orr.result)
            {
                tmp += username + "\t" + orritem.totalDay + "\t" + orritem.renewalsTimes + "\t" + orritem.renewalsAmount.ToString("0.00") + "\t" + orritem.MA5.ToString("0.00") + "\r\n";
            }

          
            return tmp;

        }
        private void backgroundWorker6_DoWork(object sender, DoWorkEventArgs e)
        {

            string id = "";
            string tmp = "";
            string whichway = e.Argument.ToString();
            string username = "";
            e.Result = "";
            DisplayAndLog("用户\t日期\t笔数\t续费金额\t5日均值\t收款方:" + InvokeHelper.Get(this.comboBox2, "Text").ToString() + "\r\n", true);
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
                username = GetUserName(treeView1.SelectedNode.Text.ToString());
                DisplayAndLog(GetOldRenewals(username, id, true), true);


            }
            else
            {

             //批量的老续费
                List<string> idlist = configManager.GetValueStrList("Userlist", "oldrenewalsuserlist");
               
                foreach (string idid in idlist)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog(GetRewnewalsUsage("未知用户ID为" + idid, idid), true);
                        continue;
                    }

                    //treeView1.Select();
                    username = GetUserName(treeView1.SelectedNode.Text.ToString());

                    DisplayAndLog(GetOldRenewals(username, idid,true), true);
                }

            }

            e.Result = whichway;
        }

        private void backgroundWorker6_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button13.Enabled = true;
            this.button29.Enabled = true;
            if (e.Result.ToString() == "single")
            {
              
       
            }
            else
            {
 
            }

            DisplayAndLogBatch("-----\t-----\t-----\t-----\t-----\t-----\t-----\t-----\r\n", true);
        }
        #endregion
        #region 批量新建下级用户
        private void button14_Click(object sender, EventArgs e)
        {
            string s = this.richTextBox1.Text;
            if((s.IndexOf('(')>0)&&(s.IndexOf(')')>0))
            {
                MessageBox.Show("用户名不允许带\"(\" \")\"\r\n");
                return;
            }
            int length = s.Length;
            if (length >= 50)
                length = 50;
            DialogResult dr = MessageBox.Show("在这个账号下新建： " + GetUserName(treeView1.SelectedNode.Text.ToString()) + "\r\n格式为：  用户名，登录名，用户类型，密码\r\n或者为： 待修改ID，用户名，登录名，用户类型，密码\r\n" + s.Substring(0,length), "提示", MessageBoxButtons.OKCancel);
           if (dr == DialogResult.OK)
           {
               this.button14.Enabled = false;
               this.backgroundWorker7.RunWorkerAsync(); 
           }
           else if (dr == DialogResult.Cancel)
           {
                
           }

        }


        private string CreateUser(string parentid,string displayname,string loginname,string usertype,string password, string currentid)
        {
            string result = "";
 
            string post = "txtHoldName={1}&txtUserName={2}&txtUserPass={4}&txtReUserPass={5}&sltHoldType={3}&txtContacter=&txtContacterTel=&viewWXRenewals=1&sltProvince=0&txtAddress=&txtRemark=&hid_ParentHoldID={0}&hid_HoldID={6}&hid_Province=&hid_City=&hid_Region=&hid_GroupHoldIds=&hid_GroupHoldNames=";
            string postWithParam = string.Format(post, parentid, displayname, loginname, usertype, password, password,currentid);
           result = PostDataToUrl(postWithParam, sApiUrl+"/hold/Info");
           if (string.IsNullOrEmpty(result))
               result = "失败";

           return result.Split(';')[0];
            //修改账号用这个格式
            //txtHoldName=SDY_TEST888&txtUserName=SDY_TES88&txtUserPass=&txtReUserPass=&sltHoldType=6&txtContacter=&txtContacterTel=&rdoOnlyOpenAPI=&viewWXRenewals=&sltProvince=&txtAddress=&txtRemark=&hid_ParentHoldID=&hid_HoldID=9560&hid_Province=&hid_City=&hid_Region=&hid_GroupHoldIds=&hid_GroupHoldNames=
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
            DisplayAndLog(GetUserName(treeView1.SelectedNode.Text.ToString()) + "\r\n", true);
            string displayname = "";
            string loginname = "";
            string usertype = "6";
            string password = "8989123";
            string currentid = "";
            for (int i=0; i < count; i++)
            {
                if(string.IsNullOrEmpty(usernamelist[i].Trim()))
                    continue;

                switch(usernamelist[i].Split(',').Length)
                    {
                    case 1:
                            displayname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[0].Trim().Replace("\t",""));
                            result = CreateUser(selectedUserId, displayname, displayname, usertype, password, currentid);
                            break;
                    case 2:
                            displayname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[0].Trim().Replace("\t", ""));
                            loginname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[1].Trim().Replace("\t", ""));
                            result = CreateUser(selectedUserId, displayname, loginname, usertype, password, currentid);
                            break;
                    case 3:
                            displayname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[0].Trim().Replace("\t", ""));
                            loginname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[1].Trim().Replace("\t", ""));
                            usertype = usernamelist[i].Trim().Split(',')[2].Trim();
                            result = CreateUser(selectedUserId, displayname, loginname, usertype, password, currentid);
                            break;
                    case 4:
                            displayname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[0].Trim().Replace("\t", ""));
                            loginname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[1].Trim().Replace("\t", ""));
                            usertype = usernamelist[i].Trim().Split(',')[2].Trim();
                            password = usernamelist[i].Trim().Split(',')[3].Trim().Replace("\t", "");
                            result = CreateUser(selectedUserId, displayname, loginname, usertype, password, currentid);
                            break;
                    case 5:
                            currentid = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[0].Trim().Replace("\t", ""));
                            displayname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[1].Trim().Replace("\t", ""));
                            loginname = System.Web.HttpUtility.UrlEncode(usernamelist[i].Trim().Split(',')[2].Trim().Replace("\t", ""));
                            usertype = usernamelist[i].Trim().Split(',')[3].Trim();
                            password = usernamelist[i].Trim().Split(',')[4].Trim().Replace("\t", "");
                            result = CreateUser(selectedUserId, displayname, loginname, usertype, password, currentid);
                            break;

                }
                DisplayAndLog(usernamelist[i] + result+"\r\n", true);
            }
        }

        private void backgroundWorker7_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button14.Enabled = true;

            button1_Click(sender, e);
        }
        #endregion

        #region 添加用户续费权限
        private void button15_Click(object sender, EventArgs e)
        {
            this.button15.Enabled = false;
            this.backgroundWorker8.RunWorkerAsync();
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
            result = PostDataToUrl(postWithParam, sApiUrl+"/SysWxPay/Info");
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

        private void backgroundWorker8_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button15.Enabled = true;
        }
        #endregion

        #region 打印下级用户
        private void button16_Click(object sender, EventArgs e)
        {
            PrintChildNodesWorker9.RunWorkerAsync();

        }
        private string GetUserName(string nodename)
        {
            string username = "";
            int index = nodename.LastIndexOf('(');

            int length = nodename.Length;
            if (index >= 0)
            {
                username = nodename.Substring(0, index);
            }
            else
            {
                username = nodename;
            }
            return username;

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
            int index = tn.Text.LastIndexOf('(');

            int length = tn.Text.Length;
            if (index >= 0)
            {
                DisplayAndLog(tn.Text.Substring(0, index) + "\t" + tn.Tag +"\t上级\t" +GetUserName( tn.Parent.Text)+"\t" + tn.Parent.Tag+  "\t卡量\t" + tn.Text.Substring(index + 1, length-index-2) + "\r\n", true);
            }
            else
            {
                DisplayAndLog(tn.Text + "\t" + tn.Tag + "\r\n", true);
            }

       

            return result;
        }
        private void backgroundWorker9_DoWork(object sender, DoWorkEventArgs e)
        {
            string selectedUserId = treeView1.SelectedNode.Tag.ToString();
 

            TreeNode selectednode = treeView1.SelectedNode;
            PringUserTree(selectednode);
        }
        #endregion

        #region 续费汇总 按卡源
        public string GetRenewalsOrderSum(string id, string  source,string stime,string etime,double days)
        {
            string result = "";
            string tmp = "";
            string url = "";

            int times = 0;
            double usage = 0;
            double amount = 0;
            double backPrice = 0;

            string sourceid = "";
            //转换卡源
            switch(source)
            {
                case "S1":
                case "s1":
                case  "1":
                    sourceid = "2";
                    break;
                case "S2":
                case "s2":
                case "2":
                    sourceid = "1";
                    break;
                case "S5":
                case "s5":
                case "5":
                    sourceid = "82";
                    break;
                case "S0":
                case "s0":
                case "0":
                case "all":
                    sourceid = "";
                    break;
            }
            string payee = InvokeHelper.Get(this.comboBox2, "SelectedValue").ToString();

            //http://demo.m-m10010.com/api/ReportRenewalsOrderTotal?holdid=1&ComeFrom=undefined&timeType=4&timeType=4&stime=2017-06-23%2000:00:00&etime=2017-06-23%2023:59:59&order=&id=&PayState=1&simState=-1&RenewalsState=&minamonth=&psize=60000&payee=&source=&access=&packageType=

            url = sApiUrl + "/api/ReportRenewalsOrderTotal?holdid=" + id + "&ComeFrom=undefined&timeType=4&timeType=4&stime=" + stime + "%2000:00:00&etime=" + etime + "%2023:59:59&order=&id=&PayState=1&simState=-1&RenewalsState=&minamonth=&psize=60000&payee=" + payee + "&source=" + sourceid + "&access=&packageType=";

            string response = GetResponseSafe(url);
            if (response == "")
            {
                DisplayAndLog("holdId为" + id + "查不到啊亲\r\n", true);
                return result;
            }
            ParamDefine.RenewalsOrderSum orr = JsonConvert.DeserializeObject<ParamDefine.RenewalsOrderSum>(response);

            if ((orr == null)|| (orr.result == null) || (orr.result.Total == null))
                return "查不到结果\r\n";

            foreach (ParamDefine.RenewalsOrderSumTotalItem orritem in orr.result.Total)
            {
                if (orritem.packageName.Contains("奖励套餐"))
                    continue;
                times += orritem.times;
                usage += orritem.usage;
                amount += orritem.amount;
                backPrice += orritem.backPrice;
            }

            //tmp += "卡源:" + source + "\t总续费次数:" + times + "\t总续费金额:" + amount + "\t总返利金额:" + backPrice + " \r\n";
            tmp += source + "\t" + times + "\t" + amount.ToString("0") + "\t" + usage + "\t" + (amount / times).ToString("0.00") + "\t" + (amount / usage * 1024).ToString("0.00") + "\t" + (amount / days).ToString("0.00") + " \r\n";
            return tmp;
        }

        private void backgroundWorker10_DoWork(object sender, DoWorkEventArgs e)
        {

            string id = "";
            string tmp = "";
            
            renewalsPeriodVS rp =( renewalsPeriodVS )e.Argument;
            string username ="";
            e.Result = "";

            string stime = rp.stime;
            string etime = rp.etime;
            double days = rp.days;
            string stime_vs = rp.stime_vs;
            string etime_vs = rp.etime_vs;
            double days_vs = rp.days_vs;
            int period = 1;

            e.Result = rp.whichway;
            if( string.IsNullOrEmpty( rp.period))
            {
                DisplayAndLog("请先输入对比周期\r\n", true);
                return ;
            }
            else
            {
                period = Convert.ToInt32(rp.period);
            }
            DateTime nowtime = DateTime.Now;
            //DisplayAndLog("数据对比时间段:" + stime_vs + "---" + etime_vs + " VS " + stime + "---" + etime + "\r\n\t\t\t" + rp.desc + "\t收款方:" + InvokeHelper.Get(this.comboBox2, "Text").ToString() + "\r\n", true);
            DisplayAndLog("时间段\t用户\t卡源\t续费次数\t续费金额\t总续费流量\t单笔ARPU\t每G售价\t日均续费\t收款方:" + InvokeHelper.Get(this.comboBox2, "Text").ToString()+"\r\n", true);
            if (rp.whichway == "single")
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
                 
                id = treeView1.SelectedNode.Tag.ToString();


                //username = treeView1.SelectedNode.Text.ToString();
                username = GetUserName(treeView1.SelectedNode.Text.ToString());

                for (int i = period; i >=1; i--)
                {
                    if(days==7.0)
                    {
                        stime = nowtime.AddDays(-i*days).ToString("yyyy-MM-dd");
                        etime = nowtime.AddDays(-(i-1)*days -1).ToString("yyyy-MM-dd");

                        DisplayAndLog(stime + "---" + etime + "\t" + username + "\t" + GetRenewalsOrderSum(id, "all", stime, etime, days), true);

                    }
                    else
                    {
                         stime = nowtime.AddMonths(-i+1).ToString("yyyy-MM") + "-01";
                         etime = Convert.ToDateTime(stime).AddMonths(1).AddHours(-1).ToString("yyyy-MM-dd");
                         //etime = nowtime.AddMonths(-i + 1).AddDays(-1).ToString("yyyy-MM-dd");
                         days = Convert.ToInt32(Convert.ToDateTime(etime).ToString("dd"));
                        //判断如果是月份是当前月，则结束日期和天数要修改下
                        if(string.Equals(stime.Substring(0,7),nowtime.ToString("yyyy-MM")))
                        {
                             etime = nowtime.ToString("yyyy-MM-dd");
                             days = nowtime.Day - 1 + nowtime.Hour / 24.0;
                        }
                        DisplayAndLog(stime + "---" + etime + "\t" + username + "\t" + GetRenewalsOrderSum(id, "all", stime, etime, days), true);
                    }

                }

               //DisplayAndLog(stime_vs + "---" + etime_vs + "\t" + username + "\t" + GetRenewalsOrderSum(id, "all", stime_vs, etime_vs, days_vs), true);
              //  DisplayAndLog("\t卡源\t续费次数\t续费金额\t总续费流量\t单笔ARPU\t每G售价\t日均续费\t续费金额环比\r\n", true);


            }
            //else if (rp.whichway == "singalimportant")
            //{
            //    if (treeView1.Nodes.Count == 0)
            //    {
            //        DisplayAndLog("请先刷新用户列表\r\n", true);
            //        return;
            //    }

            //    if (treeView1.SelectedNode == null)
            //    {

            //        DisplayAndLog("请先选择用户\r\n", true);
            //        return;
            //    }

            //    id = treeView1.SelectedNode.Tag.ToString();
            //    //username = treeView1.SelectedNode.Text.ToString();
            //    username = GetUserName(treeView1.SelectedNode.Text.ToString());

            //    DisplayAndLog(username + "\t" + GetRenewalsOrderSum(id, "all", stime_vs, etime_vs, days_vs), true);
            //    DisplayAndLog(username + "\t" + GetRenewalsOrderSum(id, "all", stime, etime, days), true);

            //}
            else if(rp.whichway == "important")
            {
                                       //  LB,    艾米,     WST   酷比,  澳先, 宇荪    本腾, 伊雷克,,,,,,,,,
                //string[] idlist = {   "1323",  "3695", "1756", "1496", "2803", "5521", "2302", "9317" };

                List<string> idlist = configManager.GetValueStrList("Userlist", "importantCustomers");

                foreach (string idid in idlist)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog(stime_vs + "---" + etime_vs + "\t" + "未知用户ID为" + idid + "\t" + GetRenewalsOrderSum(idid, "all", stime_vs, etime_vs, days_vs), true);
                        continue;
                    }
                    //username = treeView1.SelectedNode.Text.ToString();
                    username = GetUserName(treeView1.SelectedNode.Text.ToString());
                    DisplayAndLog(stime_vs + "---" + etime_vs + "\t" +username + "\t" + GetRenewalsOrderSum(idid, "all", stime_vs, etime_vs, days_vs), true);
                }

             //   DisplayAndLog("\t卡源\t续费次数\t续费金额\t总续费流量\t单笔ARPU\t每G售价\t日均续费\t续费金额环比\r\n", true);
                foreach (string idid in idlist)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog(stime + "---" + etime + "\t" + "未知用户ID为" + idid + "\t" + GetRenewalsOrderSum(idid, "all", stime, etime, days), true);
                        continue;
                    }
                    //username = treeView1.SelectedNode.Text.ToString();
                    username = GetUserName(treeView1.SelectedNode.Text.ToString());
                    DisplayAndLog(stime + "---" + etime + "\t" + username + "\t" + GetRenewalsOrderSum(idid, "all", stime, etime, days), true);
                }
            }
            //else
            //{
            //                       //D导航V,D导航T,   LB,   后视镜V, 艾米, WST_AL    M电商S,M电商V, M渠道  小流量V,小流量体验,,,,,,,,,
            //    //string[] idlist = { "4123", "8752", "1323", "1281", "3695", "1756", "5467", "2332", "6927", "2898", "2673" };

            //    List<string> idlist = configManager.GetValueStrList("Userlist", "renewalsbysource");
            //    foreach (string idid in idlist)
            //    {
            //        treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
            //        if (null == treeView1.SelectedNode)
            //        {
            //            DisplayAndLog("未知用户ID为" + idid + "\t" + GetRenewalsOrderSum(idid, "S1", stime, etime, days), true);
            //            continue;
            //        }
            //        username = GetUserName(treeView1.SelectedNode.Text.ToString());
            //        DisplayAndLog(username + "\t" + GetRenewalsOrderSum(idid, "S1", stime, etime, days), true);
            //    }
            //    foreach (string idid in idlist)
            //    {
            //        treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
            //        if (null == treeView1.SelectedNode)
            //        {
            //            DisplayAndLog("未知用户ID为" + idid + "\t" + GetRenewalsOrderSum(idid, "S2", stime, etime, days), true);
            //            continue;
            //        }
            //        username = GetUserName(treeView1.SelectedNode.Text.ToString());
            //        DisplayAndLog(username + "\t" + GetRenewalsOrderSum(idid, "S2", stime, etime, days), true);
            //    }
            //    foreach (string idid in idlist)
            //    {
            //        treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
            //        if (null == treeView1.SelectedNode)
            //        {
            //            DisplayAndLog("未知用户ID为" + idid + "\t" + GetRenewalsOrderSum(idid, "S5", stime, etime, days), true);
            //            continue;
            //        }
            //        username = GetUserName(treeView1.SelectedNode.Text.ToString());
            //        DisplayAndLog(username + "\t" + GetRenewalsOrderSum(idid, "S5", stime, etime, days), true);
            //    }

            //}


        }

        private void backgroundWorker10_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result.ToString() == "single")
            {
                this.button17.Text = "指定对比";
                this.button17.Enabled = true;

                DisplayAndLogBatch("-----\t-----\t-----\t-----\t-----\t-----\t-----\t-----\r\n", true);
            }
            //else if (e.Result.ToString() == "multi")
            //{
            //    this.button18.Text = "*续费按卡源";
            //    this.button18.Enabled = true;

            //   
            //    DisplayAndLogBatch("-----\t-----\t-----\t-----\t-----\t-----\t-----\t-----\r\n", true);
            //}
            else if (e.Result.ToString() == "important")
            {
                this.button22.Text = "*重要客户对比";
                this.button22.Enabled = true;
            }
            //else if (e.Result.ToString() == "singalimportant")
            //{
            //    this.button23.Text = "重要客户";
            //    this.button23.Enabled = true;
            //}
            
        }

  
        private void button17_Click(object sender, EventArgs e)
        {
            this.button17.Text = "获取中";
            this.button17.Enabled = false;
            renewalsPeriodVS rp;
            rp.whichway = "single";
             DateTime nowtime = DateTime.Now;
            //if (radioButton2.Checked)
            //{

             //rp.stime = nowtime.ToString("yyyy-MM") + "-01";
             //rp.etime = nowtime.ToString("yyyy-MM-dd");
             //rp.desc = "本月续费汇总";
             //rp.days = nowtime.Day - 1 + nowtime.Hour / 24.0;
            //}
            //else if (radioButton3.Checked)
             //{
                 //rp.stime = nowtime.AddMonths(-1).ToString("yyyy-MM") + "-01"; ;
                 //rp.etime = nowtime.AddDays(-(nowtime.Day)).ToString("yyyy-MM-dd");
                 //rp.desc = "上月续费汇总";
                 //rp.days = Convert.ToInt32(nowtime.AddDays(-(nowtime.Day)).ToString("dd"));
            //}
            //else
             if (radioButton4.Checked)
             {
                 rp.desc = "月对比";
                 rp.stime = nowtime.ToString("yyyy-MM") + "-01";
                 rp.etime = nowtime.ToString("yyyy-MM-dd");
                 rp.days = nowtime.Day - 1 + nowtime.Hour / 24.0;
                 rp.stime_vs = nowtime.AddMonths(-1).ToString("yyyy-MM") + "-01"; ;
                 rp.etime_vs = nowtime.AddDays(-(nowtime.Day)).ToString("yyyy-MM-dd");
                 rp.days_vs = Convert.ToInt32(nowtime.AddDays(-(nowtime.Day)).ToString("dd"));
             }
             else
             {
                 rp.stime = nowtime.AddDays(-7).ToString("yyyy-MM-dd");
                 rp.etime = nowtime.AddDays(-1).ToString("yyyy-MM-dd");
                 rp.desc = "周对比";
                 rp.days = 7.0;


                 rp.stime_vs = nowtime.AddDays(-14).ToString("yyyy-MM-dd");
                 rp.etime_vs = nowtime.AddDays(-8).ToString("yyyy-MM-dd");
                 rp.days_vs = 7.0;
             }
             rp.period =(this.textBox2.Text.Trim());
           this.backgroundWorker10.RunWorkerAsync(rp);
        }
         
        //private void button18_Click(object sender, EventArgs e)
        //{

        //    this.button18.Text = "获取中";
        //    this.button18.Enabled = false;
        //    renewalsPeriod rp;

        //    rp.whichway = "multi";
        //    DateTime nowtime = DateTime.Now;
        //    if (radioButton2.Checked)
        //    {

        //        rp.stime = nowtime.ToString("yyyy-MM") + "-01";
        //        rp.etime = nowtime.ToString("yyyy-MM-dd");
        //        rp.desc = "本月续费汇总";
        //        rp.days = nowtime.Day - 1 +  nowtime.Hour/24.0;;
        //    }
        //    else if (radioButton3.Checked)
        //    {
        //        rp.stime = nowtime.AddMonths(-1).ToString("yyyy-MM") + "-01"; ;
        //        rp.etime = nowtime.AddDays(-(nowtime.Day)).ToString("yyyy-MM-dd");
        //        rp.desc = "上月续费汇总";
        //        rp.days = Convert.ToInt32(nowtime.AddDays(-(nowtime.Day)).ToString("dd"));
        //    }
        //    else if (radioButton4.Checked)
        //    {
        //        rp.stime = nowtime.AddDays(-14).ToString("yyyy-MM-dd");
        //        rp.etime = nowtime.AddDays(-8).ToString("yyyy-MM-dd");
        //        rp.desc = "前前7天续费汇总";
        //        rp.days = 7.0;
        //    }
        //    else
        //    {
        //        rp.stime = nowtime.AddDays(-7).ToString("yyyy-MM-dd");
        //        rp.etime = nowtime.AddDays(-1).ToString("yyyy-MM-dd");
        //        rp.desc = "最近7天续费汇总";
        //        rp.days = 7.0;
        //    }
        //    //this.backgroundWorker10.RunWorkerAsync(rp);
        //}

        private void button22_Click(object sender, EventArgs e)
        {
            this.button22.Text = "获取中";
            this.button22.Enabled = false;
            renewalsPeriodVS rp;

            rp.whichway = "important";
            DateTime nowtime = DateTime.Now;
            if (radioButton4.Checked)
            {
                rp.desc = "月对比";
                rp.stime = nowtime.ToString("yyyy-MM") + "-01";
                rp.etime = nowtime.ToString("yyyy-MM-dd");
                rp.days = nowtime.Day - 1 + nowtime.Hour / 24.0;
                rp.stime_vs = nowtime.AddMonths(-1).ToString("yyyy-MM") + "-01"; ;
                rp.etime_vs = nowtime.AddDays(-(nowtime.Day)).ToString("yyyy-MM-dd");
                rp.days_vs = Convert.ToInt32(nowtime.AddDays(-(nowtime.Day)).ToString("dd"));
            }
            else
            {
                rp.stime = nowtime.AddDays(-7).ToString("yyyy-MM-dd");
                rp.etime = nowtime.AddDays(-1).ToString("yyyy-MM-dd");
                rp.desc = "周对比";
                rp.days = 7.0;


                rp.stime_vs = nowtime.AddDays(-14).ToString("yyyy-MM-dd");
                rp.etime_vs = nowtime.AddDays(-8).ToString("yyyy-MM-dd");
                rp.days_vs = 7.0;
            }
            rp.period =  (this.textBox2.Text.Trim());
             this.backgroundWorker10.RunWorkerAsync(rp);
        }
        //private void button23_Click(object sender, EventArgs e)
        //{
        //    this.button23.Text = "获取中";
        //    this.button23.Enabled = false;
        //    renewalsPeriod rp;

        //    rp.whichway = "singalimportant";
        //    DateTime nowtime = DateTime.Now;
        //    if (radioButton2.Checked)
        //    {

        //        rp.stime = nowtime.ToString("yyyy-MM") + "-01";
        //        rp.etime = nowtime.ToString("yyyy-MM-dd");
        //        rp.desc = "本月续费汇总";
        //        rp.days = nowtime.Day - 1 +  nowtime.Hour/24.0;;
        //    }
        //    else if (radioButton3.Checked)
        //    {
        //        rp.stime = nowtime.AddMonths(-1).ToString("yyyy-MM") + "-01"; ;
        //        rp.etime = nowtime.AddDays(-(nowtime.Day)).ToString("yyyy-MM-dd");
        //        rp.desc = "上月续费汇总";
        //        rp.days = Convert.ToInt32(nowtime.AddDays(-(nowtime.Day)).ToString("dd"));
        //    }
        //    else if (radioButton4.Checked)
        //    {
        //        rp.stime = nowtime.AddDays(-14).ToString("yyyy-MM-dd");
        //        rp.etime = nowtime.AddDays(-8).ToString("yyyy-MM-dd");
        //        rp.desc = "前前7天续费汇总";
        //        rp.days = 7.0;
        //    }
        //    else
        //    {
        //        rp.stime = nowtime.AddDays(-7).ToString("yyyy-MM-dd");
        //        rp.etime = nowtime.AddDays(-1).ToString("yyyy-MM-dd");
        //        rp.desc = "最近7天续费汇总";
        //        rp.days = 7.0;
        //    }
        //    //this.backgroundWorker10.RunWorkerAsync(rp);
        //}
        #endregion  

        #region 高德对账
        public string GetAMapRenewalsOrderSum(string id,   string stime, string etime)
        {
            string result = "";
            string tmp = "";
            string url = "";

            int times = 0;
            double usage = 0;
            double amount = 0;
           
            //http://demo.m-m10010.com/api/ReportYDRenewalsOrderTotal?serviceState=0&holdid=1756&sdate=2017-06-26%2012%3A32&edate=2017-06-28%2012%3A32&order=&id=&psize=50&payee=&comeFrom=&renewalsState=&sourceType=&packageType=558&p=1

            url = sApiUrl + "/api/ReportYDRenewalsOrderTotal?serviceState=0&holdid=" + id + "&sdate=" + stime + "%2000:00:00&edate=" + etime + "%2023:59:59&order=&id=&psize=50&payee=&comeFrom=&renewalsState=&sourceType=&packageType=558&p=1";

            string response = GetResponseSafe(url);
            if (response == "")
            {
                DisplayAndLog("holdId为" + id + "查不到啊亲\r\n", true);
                return result;
            }
            ParamDefine.RenewalsOrderSum orr = JsonConvert.DeserializeObject<ParamDefine.RenewalsOrderSum>(response);

            if ((orr == null) || (orr.result == null) || (orr.result.Total == null))
                return "查不到结果\r\n";

            foreach (ParamDefine.RenewalsOrderSumTotalItem orritem in orr.result.Total)
            {
                if (orritem.packageName==("系统赠送套餐(10M)"))
                {
                    times += orritem.times;
                    usage += orritem.usage;
                    amount += orritem.amount; 
                }
            }

            //\t续费笔数\t总用量\t单价\t总价
            tmp += times + "\t" + usage + "\t" + "23" + "\t" +( 23* usage / 1024).ToString("0.00") + " \r\n";
            return tmp;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            this.button19.Text = "获取中";
            this.button19.Enabled = false;
            renewalsPeriodVS rp;

            rp.whichway = "multi";
            DateTime nowtime = DateTime.Now;
            if (radioButton4.Checked)
            {
                rp.desc = "月对比";
                rp.stime = nowtime.ToString("yyyy-MM") + "-01";
                rp.etime = nowtime.ToString("yyyy-MM-dd");
                rp.days = nowtime.Day - 1 + nowtime.Hour / 24.0;
                rp.stime_vs = nowtime.AddMonths(-1).ToString("yyyy-MM") + "-01"; ;
                rp.etime_vs = nowtime.AddDays(-(nowtime.Day)).ToString("yyyy-MM-dd");
                rp.days_vs = Convert.ToInt32(nowtime.AddDays(-(nowtime.Day)).ToString("dd"));
            }
            else
            {
                rp.stime = nowtime.AddDays(-7).ToString("yyyy-MM-dd");
                rp.etime = nowtime.AddDays(-1).ToString("yyyy-MM-dd");
                rp.desc = "周对比";
                rp.days = 7.0;


                rp.stime_vs = nowtime.AddDays(-14).ToString("yyyy-MM-dd");
                rp.etime_vs = nowtime.AddDays(-8).ToString("yyyy-MM-dd");
                rp.days_vs = 7.0;
            }
            rp.period =  (this.textBox2.Text.Trim());
            this.backgroundWorker11.RunWorkerAsync(rp);
        }

        private void backgroundWorker11_DoWork(object sender, DoWorkEventArgs e)
        {
            string id = "";
            string tmp = "";

            renewalsPeriodVS rp = (renewalsPeriodVS)e.Argument;
            string username = "";
            e.Result = "";

            string stime = rp.stime;
            string etime = rp.etime;
            string stime_vs = rp.stime_vs;
            string etime_vs = rp.etime_vs;
            //string stime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            //string etime = DateTime.Now.ToString("yyyy-MM-dd");
             
            DisplayAndLog("数据采样时间段:" + stime_vs + "---" + etime_vs + " VS " + stime + "---" + etime + "\r\n\t\t" + rp.desc + "\r\n", true);
            DisplayAndLog("\t续费笔数\t总用量\t单价\t总价\r\n", true);
            if (rp.whichway == "single")
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

                id = treeView1.SelectedNode.Tag.ToString();


                username = GetUserName(treeView1.SelectedNode.Text.ToString());
                DisplayAndLog(username + "\t" + GetAMapRenewalsOrderSum(id, stime_vs, etime_vs), true); 
                DisplayAndLog(username + "\t" + GetAMapRenewalsOrderSum(id,   stime, etime), true); 


            }
            else
            {
                                    //WST    艾米     渠道  天之眼  欣和  大账号
                //string[] idlist = { "1756", "3695", "1323", "3628", "6258", "937"  };

                List<string> idlist = configManager.GetValueStrList("Userlist", "GDrenewals"); foreach (string idid in idlist)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog("未知用户ID为" + idid + "\t" + GetAMapRenewalsOrderSum(idid, stime_vs, etime_vs), true);
                        continue;
                    }
                    username = GetUserName(treeView1.SelectedNode.Text.ToString());
                    DisplayAndLog(username + "\t" + GetAMapRenewalsOrderSum(idid, stime_vs, etime_vs), true);
                }
             

                foreach (string idid in idlist)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog("未知用户ID为" + idid + "\t" + GetAMapRenewalsOrderSum(idid,   stime, etime), true);
                        continue;
                    }
                    username = GetUserName(treeView1.SelectedNode.Text.ToString());
                    DisplayAndLog(username + "\t" + GetAMapRenewalsOrderSum(idid,  stime, etime), true);
                }
             

            }

            e.Result = rp.whichway;

        }

        private void backgroundWorker11_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {          

            if (e.Result.ToString() == "single")
            {
                this.button20.Text = "高德续费";
                this.button20.Enabled = true;
            }
            else
            {
                this.button19.Text = "*高德续费";
                this.button19.Enabled = true;
            }

            DisplayAndLogBatch("-----\t-----\t-----\t-----\t-----\t-----\t-----\t-----\r\n", true);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            this.button20.Text = "获取中";
            this.button20.Enabled = false;
            renewalsPeriodVS rp;

            rp.whichway = "single";
            DateTime nowtime = DateTime.Now;
            if (radioButton4.Checked)
            {
                rp.desc = "月对比";
                rp.stime = nowtime.AddMonths(-1).ToString("yyyy-MM") + "-26";
                rp.etime = nowtime.ToString("yyyy-MM") + "-25";
                rp.days = nowtime.Day - 1 + nowtime.Hour / 24.0;
                rp.stime_vs = nowtime.AddMonths(-2).ToString("yyyy-MM") + "-26";
                rp.etime_vs = nowtime.AddMonths(-1).ToString("yyyy-MM") + "-25";
                rp.days_vs = Convert.ToInt32(nowtime.AddDays(-(nowtime.Day)).ToString("dd"));
            }
            else
            {
                rp.stime = nowtime.AddDays(-7).ToString("yyyy-MM-dd");
                rp.etime = nowtime.AddDays(-1).ToString("yyyy-MM-dd");
                rp.desc = "周对比";
                rp.days = 7.0;


                rp.stime_vs = nowtime.AddDays(-14).ToString("yyyy-MM-dd");
                rp.etime_vs = nowtime.AddDays(-8).ToString("yyyy-MM-dd");
                rp.days_vs = 7.0;
            }
            rp.period =  (this.textBox2.Text.Trim());
            this.backgroundWorker11.RunWorkerAsync(rp);
        }
        #endregion

        private void button21_Click(object sender, EventArgs e)
        {
            UpdateSims us = new UpdateSims(this);
            us.Show();
        }

        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
 

            if ((e.State & TreeNodeStates.Selected) != 0)
            {
                e.Graphics.FillRectangle(Brushes.Blue, e.Node.Bounds);
                Font nodeFont = e.Node.NodeFont;
                if (nodeFont == null) nodeFont = ((TreeView)sender).Font;
                e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White, Rectangle.Inflate(e.Bounds, 2, 0));
            }
            else
            {
                e.DrawDefault = true;
            }
  
             
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)//判断你点的是不是右键
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode CurrentNode = treeView1.GetNodeAt(ClickPoint);
                if (CurrentNode != null  )//判断你点的是不是一个节点
                {
                    treeView1.SelectedNode = CurrentNode;
                   DisplayAndLog(CurrentNode.Tag.ToString() + ",", true);
                     
                   
                }
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {

            this.button2.Enabled = false;
            this.button24.Enabled = false;
            this.backgroundWorker1.RunWorkerAsync("multi");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.button18.Enabled = false;

            this.CheckSimRenewalsWorker.RunWorkerAsync();
            
        }

        private void CheckSimRenewalsWorker_DoWork(object sender, DoWorkEventArgs e)
        {
             
            string id = "";
            string tmp = "";
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
            DisplayAndLog(GetUserName(treeView1.SelectedNode.Text.ToString()) + "\t卡的套餐分布如下：\r\n"  , true);
            DisplayAndLog(GetUserSimRenewalsPkgList(id, true), true);

        }
        public string GetUserSimRenewalsPkgList(string id, bool isDisplay)
        {
            string result = "";
            string simid = "";
            string pkgid = "";
            string url = sApiUrl + "/api/HoldPackageTotal?holdId=" + id + "&groupHoldId=0&simFromType=1" ;
            bool isPrintRenewals = Convert.ToBoolean(InvokeHelper.Get(this.checkBox2, "Checked"));
     
            string response = GetResponseSafe(url);
            if (response == "")
            {
                result = "holdId为" + id + "查不到啊亲\r\n" ;
                return result;
            }
            ParamDefine.PkgDistributionRoot pkgDisRoot = JsonConvert.DeserializeObject<ParamDefine.PkgDistributionRoot>(response);

            if (pkgDisRoot==null || pkgDisRoot.result == null)
            {
                result = "holdId为" + id + "查不到套餐分布呢\r\n";
                return result;
            }
            //查到了套餐分布
            foreach (ParamDefine.PkgDistributionResultItem pkgitem in pkgDisRoot.result)
            {
                DisplayAndLog(pkgitem.groupByName.PadRight(22) + "\t" + pkgitem.groupByValue + "张\t", true);
                //如果勾选了打印续费套餐列表  就打印
                if (isPrintRenewals)
                {
                    pkgid = GetPkgIdFromPkgName(pkgitem.groupByName);
                    if (string.IsNullOrEmpty(pkgid))
                    {
                        DisplayAndLog("套餐ID为空\r\n", true);
                        continue;
                    }
                    simid = GetSimidFromSearchPkg(id, pkgid);
                    if (string.IsNullOrEmpty(simid))
                    {
                        DisplayAndLog("SimID为空\r\n", true);
                        continue;
                    }
                    //返回的格式为 simid，ICCID
                    DisplayAndLog(simid.Split(',')[1] + "\r\n", true);

                    simid = simid.Split(',')[0];
                    DisplayAndLogBatch(GetSimRenewalsPkgList(simid), true);
                    

                }
                else
                {

                    DisplayAndLog(  "\r\n", true);
                }


            }
            

            return result;
        }

        public string GetSimRenewalsPkgList(string simid)
        {
            string result ="";
            if(string.IsNullOrEmpty(simid))
            {
                result = "\t@RSimId为空\r\n";
                return result;
            }

            string url = sApiUrl + "/api/GetSimRenewalsPackageList/" + simid;

            string response = GetResponseSafe(url);
            if (response == "")
            {
                result = "\t@R获取可续费套餐列表失败\r\n";
                return result;
            }
            ParamDefine.SimRenewalsPkgListRoot pkgListRoot = JsonConvert.DeserializeObject<ParamDefine.SimRenewalsPkgListRoot>(response);

            if (pkgListRoot == null || pkgListRoot.result == null )
            {
                result = "\t@R获取可续费套餐列表失败\r\n";
                return result;
            }

            foreach(ParamDefine.SimRenewalsPkgListResultItem pkg in pkgListRoot.result)
            {
                result += "\t@B└--" + pkg.packageName.PadRight(27) + "\t@R" + pkg.price + "\r\n";

            }


            return result;
        }
        public int RefreshPkg(bool isForceRefresh)
        {

            if ((LTPkgIdList.Keys.Count != 0) && isForceRefresh == false)
                return 0;

            string url = sApiUrl + "/api/SimHandle/ScreenSelectList?pageName=ltsim";

            string response = GetResponseSafe(url);
            if (response == "")
            {
                DisplayAndLog("刷新套餐失败\r\n",true);
                return -1;
            }
            ParamDefine.PkgListDetailRoot pkgListRoot = JsonConvert.DeserializeObject<ParamDefine.PkgListDetailRoot>(response);
            if (pkgListRoot == null || pkgListRoot.result == null || pkgListRoot.result.sltPackageType == null)
            {
                DisplayAndLog("获取套餐失败\r\n", true);
                return -2;
            }
            foreach (ParamDefine.SltPackageTypeItem pkgitem in pkgListRoot.result.sltPackageType)
            {
                LTPkgIdList.Add(pkgitem.Text, pkgitem.Value);
            }


            return 0;

        }
        public string GetPkgIdFromPkgName(string pkgname)
        {
            string pkgid = "";
            if (string.IsNullOrEmpty(pkgname))
            {
                DisplayAndLog("套餐名为空！" + pkgname + "\r\n", true);
                return pkgid;
            }
            RefreshPkg(false);

            pkgid = LTPkgIdList[pkgname];

            return pkgid;
        }
        public string GetSimidFromSearchPkg(string holdid, string pkgid)
        {
            string simid = "";

            string url = sApiUrl + "/api/SimListFire/Search";
            string postdata = "p=1&pRowCount=1&loginHoldId=" + holdid + "&key=&noChild=0&groupHoldId=0&packageType=" + pkgid;
            string response = PostDataToUrl(postdata,url);
            if (response == "")
            {
                DisplayAndLog("刷新套餐失败\r\n", true);
                return simid;
            }
            try
            {

                //jo1是整个返回值
                JObject jo1 = (JObject)JsonConvert.DeserializeObject(response);
                //array是三个数组，分别是 汇总信息，卡列表，查询条件
                ////var listData = data.result[1];
                ////var page = data.result[0];
                ////var hid_querySqlwhereKey = data.result[2].card_query_sqlwhere;
                //var strsimlist = jo1.GetValue("result")[1];
                //string strsimlistjson = "{\"result\": " + strsimlist.ToString() + "}";
                //ParamDefine.SearchSimListRoot ssld = JsonConvert.DeserializeObject<ParamDefine.SearchSimListRoot>(strsimlistjson);
                //foreach (ParamDefine.SearchSimListDetail a in ssld.result)
                //{
                //    simid = a.simId;
                //    break;
                //}


                JToken[] array = jo1.GetValue("result").ToArray();
                //取第1个卡
                string simlist = array[1].First.ToString();
                ParamDefine.SearchSimListDetail sslds = JsonConvert.DeserializeObject<ParamDefine.SearchSimListDetail>(simlist);
                simid = sslds.simId + "," + sslds.guid;
 
                return simid;
            }
            catch(Exception e)
            {
                DisplayAndLog(e.ToString() + "\r\n", true);
                return "";
            }
        }

        private void CheckSimRenewalsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button18.Enabled = true;
        }


        #region 激活数汇总
        private string GetActivaCountDay(string username,string id)
        {
            string result = "";
      
            string url = "";

            string dateStr = username + "\t日期\t";
            string activatedCountStr = username + "\t激活数\t";
            string deactivatedCountStr = username + "\t停用数\t";
            string firstActivatedCountStr = username + "\t首次激活\t";
            string lastDeactivatedStr = username + "\t最近停用\t";


            url = sApiUrl + "/api/ReportStatusRenewalsTotal?cmd=1&datetype=day&holdId=" + id;

            string response = GetResponseSafe(url);
            if (response == "")
            {
                DisplayAndLog("holdId为" + id + "查不到啊亲\r\n", true);
                return result;
            }
            ParamDefine.ActivateCountRoot mrt = JsonConvert.DeserializeObject<ParamDefine.ActivateCountRoot>(response);
            if (mrt.result == null)
                return result + "\r\n";
            foreach (ParamDefine.ActivateCountResultItem mrtresult in mrt.result)
            {
                dateStr += mrtresult.date + "\t";
                activatedCountStr += mrtresult.activatedCount + "\t";
                deactivatedCountStr += mrtresult.deactivatedCount + "\t";
                firstActivatedCountStr += mrtresult.firstActivatedCount + "\t";
                lastDeactivatedStr += mrtresult.lastDeactivatedCount + "\t";
            }
            result = dateStr + "\r\n" + activatedCountStr + "\r\n" + deactivatedCountStr + "\r\n" + firstActivatedCountStr + "\r\n" + lastDeactivatedStr + "\r\n";
          
            return result;

        }

        private void button25_Click(object sender, EventArgs e)
        {
            this.button25.Enabled = false;
            this.button23.Enabled = false;

            this.GetActiveCountWorker.RunWorkerAsync("single");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            this.button25.Enabled = false;
            this.button23.Enabled = false;

            this.GetActiveCountWorker.RunWorkerAsync("multi");
        }

        private void GetActiveCountWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string id = "";
            string tmp = "";
            string whichway = e.Argument.ToString();
            string username = "";
            e.Result = whichway;
 
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
                 
                id = treeView1.SelectedNode.Tag.ToString();
                username = GetUserName(treeView1.SelectedNode.Text.ToString());
                DisplayAndLog(GetActivaCountDay(username,id ), true);


            }
            else
            { 
                List<string> idlist = configManager.GetValueStrList("Userlist", "activeuserlist");
                foreach (string idid in idlist)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog(GetActivaCountDay("未知用户ID为" + idid, idid), true);
                        continue;
                    }

                    //treeView1.Select();
                    username = GetUserName(treeView1.SelectedNode.Text.ToString());
                    DisplayAndLog(GetActivaCountDay(username, idid), true);
                }

            }
             
        }

        private void GetActiveCountWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
 
 
                this.button25.Enabled = true;
    
                this.button23.Enabled = true;


                DisplayAndLogBatch("-----\t-----\t-----\t-----\t-----\t-----\t-----\t-----\r\n", true);
       

        }
        #endregion 

        private void button27_Click(object sender, EventArgs e)
        {
            this.button26.Enabled = false;
            this.button27.Enabled = false;

            this.GetRenewalsUsageWorker9.RunWorkerAsync("single");
        }

        private void button26_Click(object sender, EventArgs e)
        {
            this.button26.Enabled = false;
            this.button27.Enabled = false;
            this.GetRenewalsUsageWorker9.RunWorkerAsync("multi");
        }
        private string GetRewnewalsUsage(string username, string id)
        {
            string result = "";

            string url = "";
            string tmp = "\t日期\t续费金额\t续费流量\t续费笔数\t停机续费\t首次续费\r\n";
            //string dateStr = username + "\t日期\t";
            //string amountStr = username + "\t续费金额\t";
            //string usageStr = username + "\t续费流量\t";
            //string timesStr = username + "\t续费笔数\t";
            //string stopTimesStr = username + "\t停机续费\t";
            //string firstTimesStr = username + "\t首次续费\t";
        
            url = sApiUrl + "/api/ReportStatusRenewalsTotal?cmd=3&datetype=day&holdId=" + id;

            string response = GetResponseSafe(url);
            if (response == "")
            {
                DisplayAndLog("holdId为" + id + "查不到啊亲\r\n", true);
                return result;
            }
            ParamDefine.RenewalsUsageRoot mrt = JsonConvert.DeserializeObject<ParamDefine.RenewalsUsageRoot>(response);
            if (mrt.result == null)
                return result + "\r\n";
            foreach (ParamDefine.RenewalsUsageResultItem mrtresult in mrt.result)
            {
                //dateStr += mrtresult.date + "\t";
                //amountStr += mrtresult.amount + "\t";
                //usageStr += mrtresult.usage + "\t";
                //timesStr += mrtresult.times + "\t";
                //stopTimesStr += mrtresult.stopTimes + "\t";
                //firstTimesStr += mrtresult.firstTimes + "\t";
                tmp += username + "\t" + mrtresult.date + "\t" + mrtresult.amount + "\t" + mrtresult.usage + "\t" + mrtresult.times + "\t" + mrtresult.stopTimes + "\t" + mrtresult.firstTimes + "\r\n";
            }
            //result = dateStr + "\r\n" + amountStr + "\r\n" + usageStr + "\r\n" + timesStr + "\r\n" + stopTimesStr + "\r\n" + firstTimesStr + "\r\n";

            result = tmp;
            return result;

        }
        private void GetRenewalsUsageWorker9_DoWork(object sender, DoWorkEventArgs e)
        {
            string id = "";
            string tmp = "";
            string whichway = e.Argument.ToString();
            string username = "";
            e.Result = whichway;

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

                id = treeView1.SelectedNode.Tag.ToString();
                username = GetUserName(treeView1.SelectedNode.Text.ToString());
                DisplayAndLog(GetRewnewalsUsage(username, id), true);


            }
            else
            {
                List<string> idlist = configManager.GetValueStrList("Userlist", "renewalsusageuserlist");
                foreach (string idid in idlist)
                {
                    treeView1.SelectedNode = FindNodeById(treeView1.Nodes[0], idid);
                    if (null == treeView1.SelectedNode)
                    {
                        DisplayAndLog(GetRewnewalsUsage("未知用户ID为" + idid, idid), true);
                        continue;
                    }

                    //treeView1.Select();
                    username = GetUserName(treeView1.SelectedNode.Text.ToString());
                    DisplayAndLog(GetRewnewalsUsage(username, idid), true);
                }

            }
             
        }

        private void GetRenewalsUsageWorker9_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button26.Enabled = true;

            this.button27.Enabled = true;


            DisplayAndLogBatch("-----\t-----\t-----\t-----\t-----\t-----\t-----\t-----\r\n", true);
       
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
 * http://demo.m-m10010.com/api/allholdnodes?id=0&parent=0&nodeListType=3&NJholdId=0&notIncludeCount=false&UserLoadType=1
 * 
 * 用户的可续费套餐权限
 * http://demo.m-m10010.com/api/HoldRenewalsList/1496?allShow=true 
 * 
 * 套餐的可续费套餐
 * http://demo.m-m10010.com/api/RenewalsPackage?id=17547
 * 
 * 月用量报表
 * http://demo.m-m10010.com/api/ReportFlowHold?holdId=5877
 
 * 
 * 套餐列表-分组列表
 * GET http://demo.m-m10010.com/api/SimHandle/ScreenSelectList?pageName=ltsim 
 
 * 获取账号的所有卡的套餐分布
 * GET http://demo.m-m10010.com/api/HoldPackageTotal?holdId=4984&groupHoldId=0&simFromType=1 

 * 查询SIM卡
 * POST http://demo.m-m10010.com/api/SimListFire/Search
 * p=1&pRowCount=2&loginHoldId=1&key=&noChild=0&groupHoldId=0&packageType=578
 * 
 * 卡的可续费套餐
 * GET http://demo.m-m10010.com/api/GetSimRenewalsPackageList/430932  
 */
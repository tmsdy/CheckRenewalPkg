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
using System.IO;
using System.Data.SQLite;

namespace CheckRenewalPkg
{
    public partial class LoginForm : Form
    {
        public int clickcount = 0;
        public LoginForm()
        {
            InitializeComponent();

        }
        public static void SetUserId(string cookie)
        {
            if(String.IsNullOrEmpty(cookie))
                return ;
            string[] paramlist = cookie.Split('&');
            for (int i = 0; i < paramlist.Count();i++ )
            {

                if (paramlist[i].Split('=')[0] == "HoldID")
             {
                 Program.UserId = paramlist[i].Split('=')[1];
                 break;
             }

            }
        }
        public static bool PosttoLogin(string postdata)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(postdata);
                string cookie;
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Program.sGloableDomailUrl + "/User/login");
                myRequest.Method = "POST";
                myRequest.UserAgent = Program.DefaultUserAgent;
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = data.Length;
                myRequest.CookieContainer = new CookieContainer();
                myRequest.CookieContainer = Program.MLBCookie;
                //如果允许重定向 就获取不到cookie和302的状态码
                myRequest.AllowAutoRedirect = false;
                //MessageBox.Show(Program.JasperCookie.GetCookieHeader(new Uri(Program.sCookHeader)).Trim('\n').Trim('\r'));
                //myRequest.CookieContainer.SetCookies(new Uri(Program.sCookHeader), Program.JasperCookie.GetCookieHeader(new Uri(Program.sCookHeader)));
                Stream newStream = myRequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();


                ///**/
                //StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                //string ttttt = reader.ReadToEnd();
                //System.Diagnostics.Debug.WriteLine(ttttt + "\r\n");
                ///**/
                     
                if (myResponse.StatusCode == HttpStatusCode.Found)
                {  
                    string a = myResponse.GetResponseHeader("Set-Cookie");
                    cookie = myResponse.Headers.Get("Set-Cookie");
                    SetUserId(cookie);
                    Program.MLBCookie.SetCookies(new Uri(Program.sGloableDomailUrl), cookie);
                    myResponse.Dispose();
                    myResponse.Close(); 

                    return true;
                }
                else
                {
                    myResponse.Dispose();
                    myResponse.Close();
                    return false;
                }

    
            }
            catch(Exception e)
            {
                string err = e.Message.ToString();
                return false;
            }


        }
        private void button1_Click(object sender, EventArgs e)
        {
            //userName=admin&userPwd=www.u12580.com.allah
            string postData = "userName=" + this.textBox1.Text.Trim() + "&userPwd=" + this.textBox2.Text.Trim();

            if (PosttoLogin(postData))
            {
                //登录成功，则将cookie保存起来
               
                this.Hide();
                Form1 form1 = new Form1();
                form1.ShowDialog();
                //UpdateSims form2 = new UpdateSims();
                //form2.Show();

            }
            else
            {
                MessageBox.Show("错了");
            }
        }
        void createNewDatabase()
        {
            SQLiteConnection.CreateFile("MyDatabase.sqlite");
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

            this.Text += Program.sVer;
            //string response = "{\"error\":0,\"reason\":\"\",\"result\":{\"Total\":[{\"packageName\":\"1GB(一年有效)\",\"times\":36,\"usage\":36864.0,\"amount\":1079.64,\"backPrice\":216.0},{\"packageName\":\"4G(月叠加包)限时特惠\",\"times\":15,\"usage\":61440.0,\"amount\":585.0,\"backPrice\":117.0},{\"packageName\":\"12GB(半年有效)限时特惠\",\"times\":12,\"usage\":147456.0,\"amount\":1668.0,\"backPrice\":333.6},{\"packageName\":\"100M一年(体验套餐)\",\"times\":7,\"usage\":700.0,\"amount\":0.07,\"backPrice\":0.0},{\"packageName\":\"4GB(一年有效)\",\"times\":7,\"usage\":28672.0,\"amount\":699.93,\"backPrice\":140.0},{\"packageName\":\"2GB(一年有效)\",\"times\":6,\"usage\":12288.0,\"amount\":347.94,\"backPrice\":69.6},{\"packageName\":\"50GB(一年有效)限时钜惠\",\"times\":4,\"usage\":204800.0,\"amount\":1596.0,\"backPrice\":319.2},{\"packageName\":\"15GB(月叠加包)限时钜惠\",\"times\":3,\"usage\":46080.0,\"amount\":297.0,\"backPrice\":59.4},{\"packageName\":\"30GB(一年有效)限时钜惠\",\"times\":3,\"usage\":92160.0,\"amount\":747.0,\"backPrice\":149.4},{\"packageName\":\"12G 12个月(每月1G)\",\"times\":2,\"usage\":24576.0,\"amount\":399.98,\"backPrice\":80.0},{\"packageName\":\"48G 12个月(每月4G)\",\"times\":2,\"usage\":98304.0,\"amount\":799.98,\"backPrice\":160.0},{\"packageName\":\"1G(月叠加包)\",\"times\":1,\"usage\":1024.0,\"amount\":29.99,\"backPrice\":6.0},{\"packageName\":\"12GB(一年有效)\",\"times\":1,\"usage\":12288.0,\"amount\":259.99,\"backPrice\":52.0},{\"packageName\":\"60G 12个月(每月5G)\",\"times\":1,\"usage\":61440.0,\"amount\":499.99,\"backPrice\":100.0},{\"packageName\":\"8GB(一年有效)\",\"times\":1,\"usage\":8192.0,\"amount\":189.99,\"backPrice\":38.0},{\"packageName\":\"60GB 6个月(每月10G)限时钜惠\",\"times\":1,\"usage\":61440.0,\"amount\":399.0,\"backPrice\":79.8}],\"Order\":[]}}";
            //ParamDefine.RenewalsOrderSum orr = Newtonsoft.Json.JsonConvert.DeserializeObject<ParamDefine.RenewalsOrderSum>(response);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            if(clickcount++ >=4)
            {
                clickcount = 0;
                UpdateSims us = new UpdateSims();
                us.Show();
                this.Hide();

            }

        }
    }
}

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
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Program.sLoginUrl);
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
                    Program.MLBCookie.SetCookies(new Uri(Program.sCookHeader), cookie);
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
             
        }
    }
}

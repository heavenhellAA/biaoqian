using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace BarCodePrintSys.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Login()
        {
            HttpCookie usercookie = Request.Cookies["bcp_user"];
            HttpCookie pwdcookie = Request.Cookies["bcp_pwd"];
            if (usercookie != null && MD5Helper.MD5Decrypt(usercookie["flag"]) == "Yes")
            {
                ViewData["UserName"] = Server.UrlDecode(usercookie["UserName"].ToString());
            }
            if (pwdcookie != null && MD5Helper.MD5Decrypt(pwdcookie["flag"]) == "Yes")
            {
                string pwd =Server.UrlDecode(pwdcookie["UserPassword"].ToString());
                ViewData["UserPassword"] = MD5Helper.MD5Decrypt(pwd);
            }
            return View();
        }
        public ActionResult RoleWarm()
        {
            return View();
        }
        public int Loging(string loginUsername, string loginPassword, bool saveuser, bool savepwd)
        {
            int data = 0;
            string name = Func.Zhuru(loginUsername.Trim());
            string pwd = Func.Zhuru(loginPassword.Trim());
            pwd = MD5Helper.MD5Encrypt(pwd);
            string sql = "select * from tbUser where s_UserName='" + name.Trim() + "' and b_IsDeleted = 0";
            //创建连接数据库Connection对象
            SqlConnection con = DB.Con();
            //创建连接数据库SqlCommand对象，执行sql语句
            SqlCommand cmd = new SqlCommand(sql, con);
            //打开数据库
            con.Open();
            //读取数据集
            SqlDataReader rd = cmd.ExecuteReader();
            //验证登陆密码
            if (rd.Read() && Func.Zhuru(loginPassword) != "")
            {
                //密码正确
                if (rd["s_UserPassword"].ToString() == pwd && pwd != "")
                {
                    //保存Cookies值
                    string flagYes = MD5Helper.MD5Encrypt("Yes");
                    string flagNo = MD5Helper.MD5Encrypt("No");
                    HttpCookie cookie = new HttpCookie("bcp_userInfo");
                    HttpCookie usercookie = new HttpCookie("bcp_user");
                    HttpCookie pwdcookie = new HttpCookie("bcp_pwd");
                    cookie.Values["UserID"] = Server.UrlEncode(rd["s_UserID"].ToString());
                    cookie.Values["RoleID"] = Server.HtmlEncode(rd["s_RoleID"].ToString());
                    cookie.Expires = DateTime.Now.AddDays(1);
                    usercookie.Values["UserName"] = Server.UrlEncode(rd["s_UserName"].ToString());
                    pwdcookie.Values["UserPassword"] = Server.UrlEncode(rd["s_UserPassword"].ToString());
                    Response.Cookies.Add(cookie);//保存Cookies值
                    //记住账号
                    if (saveuser)
                    {
                        usercookie.Values["flag"] = flagYes;
                        usercookie.Expires = DateTime.Now.AddDays(30);
                    }
                    else
                    {
                        usercookie.Values["flag"] = flagNo;
                        usercookie.Expires = DateTime.Now.AddDays(1);
                    }
                    //记住密码
                    if (savepwd)
                    {
                        pwdcookie.Values["flag"] = flagYes;
                        pwdcookie.Expires = DateTime.Now.AddDays(30);
                    }
                    else
                    {
                        pwdcookie.Values["flag"] = flagNo;
                        pwdcookie.Expires = DateTime.Now.AddDays(1);
                    }
                    Response.Cookies.Add(usercookie);//保存Cookies值
                    Response.Cookies.Add(pwdcookie);//保存Cookies值
                    con.Close(); //关闭数据库连接
                    data = 2;
                }
                else
                {
                    con.Close();
                    data = 1;
                }
            }
            else
            {
                con.Close();
            }
            return data;
        }
    }
}

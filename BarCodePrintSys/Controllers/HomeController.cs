using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BarCodePrintSys.Controllers
{
    [MemberValidation]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            ViewData["UserName"] = Server.UrlDecode(Request.Cookies["bcp_user"]["UserName"].ToString());
            return View();
        }
        public ActionResult Amain()
        {
            return View();
        }
        public JsonResult GetmenuDt()
        {
            string sql;
            var res = new JsonResult();
            int role = DBHelper.getRoleNo(Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString()));
            sql = "select * from tbMenu where b_IsDeleted = 0 and n_RoleNO >=" + role + " order by n_Sort";
            DataSet ds = DBHelper.getDateSet(sql);
            var datas = ds.Tables[0].Rows;
            List<Menu> list = new List<Menu>();
            var total = datas.Count;
            foreach (DataRow item in datas)
            {
                var act = new Menu()
                {
                    Node = Convert.ToInt32(item["n_Node"]),
                    ParentNode = Convert.ToInt32(item["n_ParentNode"]),
                    MenuName = item["s_MenuName"].ToString(),
                    Url = item["s_Url"].ToString(),
                };
                list.Add(act);
            }
            ViewBag.menu = list;
            res.Data = list;
            res.JsonRequestBehavior = JsonRequestBehavior.AllowGet;//允许使用GET方式获取，否则用GET获取是会报错。
            return res;
        }
        public int ReomveCookie()
        {
            int flag = 0;
            HttpCookie cookie = Request.Cookies["bcp_userInfo"];
            HttpCookie usercookie = Request.Cookies["bcp_user"];
            HttpCookie pwdcookie = Request.Cookies["bcp_pwd"];
            //if (filterContext.HttpContext.Session["username"] == null)
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
                flag = 1;
            }
            //记住账号或者密码usercookie["flag"]=Yes，不是记住密码退出系统就移除cookie值
            if (usercookie != null && MD5Helper.MD5Decrypt(usercookie["flag"]) != "Yes")
            {
                usercookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(usercookie);
            }
            if (pwdcookie != null && MD5Helper.MD5Decrypt(pwdcookie["flag"]) != "Yes")
            {
                pwdcookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(pwdcookie);
            }
            return flag;
        }
        public int EditPwd(string OldPassword, string NewPassword, string NewPassword2)
        {
            int res = 0;
            string sql="";
            string UserName = Server.HtmlDecode(Request.Cookies["bcp_user"]["UserName"].ToString());
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string OldUserPassword = MD5Helper.MD5Encrypt(OldPassword);
            string NewUserPassword = MD5Helper.MD5Encrypt(NewPassword);
            string NewUserPassword2 = MD5Helper.MD5Encrypt(NewPassword2);
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "select * from tbUser where s_UserName ='" + UserName + "' AND s_UserID='" + UserID + "'";
            //创建连接数据库Connection对象
            SqlConnection con = DB.Con();
            //创建连接数据库SqlCommand对象，执行sql语句
            SqlCommand cmd = new SqlCommand(sql, con);
            //打开数据库
            con.Open();
            //读取数据集
            SqlDataReader rd = cmd.ExecuteReader();
            //验证登陆密码
            if (rd.Read() && Func.Zhuru(OldUserPassword) != "")
            {
                //密码正确
                if (rd["s_UserPassword"].ToString() == OldUserPassword)
                {
                    con.Close(); //关闭数据库连接
                    sql = "update tbUser set s_UserName='" + UserName + "',s_UserPassword='" + NewUserPassword + "'";
                    sql += ",s_UpdateUserID='" + UserID + "',d_UpdateTime='" + nowtime + "' where s_UserID = '" + UserID + "'";
                    res = DBHelper.excuteNoQuery(sql);
                }
                else
                {
                    con.Close(); //关闭数据库连接
                    res = 2;
                }
            }
            else
            {
                con.Close(); //关闭数据库连接
            }
            return res;
        }
    }
}

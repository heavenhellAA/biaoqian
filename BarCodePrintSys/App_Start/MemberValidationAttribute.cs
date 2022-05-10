using BarCodePrintSys.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BarCodePrintSys
{
  // public class AuthenticationAttribute
   public class MemberValidationAttribute:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            Boolean flag = false;
            //HttpCookie name, pwd, role;
            //name = HttpContext.Current.Request.Cookies["userInfo"]["role"];
            //pwd = HttpContext.Current.Request.Cookies["userInfo"]["pwd"];
            //role
            HttpCookie cookie = HttpContext.Current.Request.Cookies["bcp_userInfo"];
            HttpCookie pwdcookie = HttpContext.Current.Request.Cookies["bcp_pwd"];
            if (cookie != null && pwdcookie!=null)
            {

                string UserID = Func.Zhuru(cookie["UserID"].ToString()) + "";
                string Pwd = Func.Zhuru(pwdcookie["UserPassword"].ToString()) + "";
                string RoleID = Func.Zhuru(cookie["RoleID"].ToString()) + "";
                //name = HttpContext.Current.Request.Cookies["name"];
                //pwd = HttpContext.Current.Request.Cookies["pwd"];
                //role= HttpContext.Current.Request.Cookies["role"];
                //name = HttpContext.Current.Server.UrlDecode(name);//确保恶意用户没有向 Cookie 中添加可执行脚本

                if (UserID.Length > 0 && Pwd.Length > 0 && RoleID.Length > 0)
                {
                    string sql = "select * from tbUser where s_UserID='" + UserID + "' and s_UserPassword='" + Pwd + "' and s_RoleID='" + RoleID + "'";
                    SqlConnection con = DB.Con();
                    SqlCommand cmd = new SqlCommand(sql, con);
                    con.Open();
                    SqlDataReader rd = cmd.ExecuteReader();
                    //验证cookie
                    if (rd.Read())
                    {
                        flag = true;
                    }
                    con.Close();
                }
                else
                {
                 
                    filterContext.Result = new RedirectResult("../Default/RoleWarm");
                }
                if (!flag)
                {
                    filterContext.Result = new RedirectResult("../Default/RoleWarm");
                }
            }
            else
            //filterContext.Result = new RedirectToRouteResult("Login", new RouteValueDictionary { { "from", Request.Url.ToString() } });
            {
                //filterContext.Result = new RedirectToRouteResult("Login", new RouteValueDictionary(new { controller = "Default", action = "Login" }),false);
                filterContext.Result = new RedirectResult("/");
            }
           // base.OnActionExecuting(filterContext);
        }
    }
}
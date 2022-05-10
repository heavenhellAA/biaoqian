using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
namespace BarCodePrintSys.Controllers
{
     [MemberValidation]
    public class UserController : Controller
    {
        //
        // GET: /User/
        public ActionResult Index()
        {
            int role = DBHelper.getRoleNo(Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString()))+1;
            ViewBag.role = DBHelper.getRoles(role);//获取权限组
            ViewBag.group = DBHelper.getGroups();//获取分组
            string s_none = "";
            if (role > 20)
            {
                s_none = "none";
            }
            ViewData["none"] = s_none;
            return View();
        }
        public string GetUser()
        {
            string sql;
            string SKey;
            int dcode = -1;
            int dcount = 0;
            string dmsg = "此查询无数据！";
            string mName = Request["mName"];
            int nPages = Convert.ToInt32(Request["page"]);
            int nPageSize = Convert.ToInt32(Request["limit"]);
            int start = nPageSize * nPages - nPageSize + 1;//分页数据的开始序号
            int end = nPageSize * nPages;//分页数据的终止序号
            int role = DBHelper.getRoleNo(Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString()));
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string group = DBHelper.getuserGroup(UserID);
            Dictionary<String, Object> rsMap = new Dictionary<String, Object>();
            if (role == 0)
            {
                SKey = "tbUser.n_RoleNO >=" + role;
            }
            else {
                SKey = "tbUser.n_RoleNO >" + role;
            }
            if (role > 5)
            {
                SKey += " AND tbUser.s_GroupID='" + group + "'";
            }
            if (mName != "")
            {
                SKey += " AND tbUser.s_UserName like '%" + mName + "%'";
            }
             sql = "SELECT * FROM (SELECT ROW_NUMBER() over(ORDER BY tbUser.n_RoleNO ASC) AS num,";
             sql+=" (SELECT  count(*)  FROM tbUser  where  "+ SKey+" AND tbUser.b_IsDeleted = 0 AND tbRole.b_IsDeleted = 0 ) AS tcount, " ;
             sql += " tbUser.s_UserID,tbUser.s_UserName,tbUser.s_Name,tbUser.s_UserPassword,tbRole.n_RoleNO,tbRole.s_RoleName,tbGroup.s_GroupName,tbGroup.s_GroupID  ";
             sql += " FROM tbUser JOIN tbRole ON tbUser.s_RoleID=tbRole.s_RoleID  JOIN tbGroup ON tbUser.s_GroupID=tbGroup.s_GroupID  where " + SKey;
             sql+=" AND tbUser.b_IsDeleted = 0 AND tbRole.b_IsDeleted = 0 ) tbUser WHERE (num BETWEEN " + start + " AND " + end + ") ";
            DataSet ds = DBHelper.getDateSet(sql);
            var datas = ds.Tables[0].Rows;
            List<TUser> list = new List<TUser>();
            var total = datas.Count;
            if (total > 0)
            {
                dcode = 0;
                dmsg = "S";
                dcount =  Convert.ToInt32(datas[0]["tcount"]);
                foreach (DataRow item in datas)
                {
                    var act = new TUser()
                    {
                        UserID = item["s_UserID"].ToString(),
                        UserName = item["s_UserName"].ToString(),
                        Name = item["s_Name"].ToString(),
                        UserPassword = MD5Helper.MD5Decrypt(item["s_UserPassword"].ToString()),
                        GroupID = item["s_GroupID"].ToString(),
                        RoleName = item["s_RoleName"].ToString(),
                        RoleNO = Convert.ToInt32(item["n_RoleNO"]),
                        GroupName = item["s_GroupName"].ToString(),
                    };
                    list.Add(act);
                }
            }
            rsMap["count"] = dcount;
            rsMap["code"] = dcode;
            rsMap["msg"] = dmsg;
            rsMap["data"] = list;
            string res = JsonConvert.SerializeObject(rsMap);
            return res;
        }
        public int AddData()
        {
            int res = 0;
            int RoleNO = Convert.ToInt32(Request["sRole"]);
            string RoleID = DBHelper.getRoleID(RoleNO);
            string GroupID = Request["sGroup"];
            string UserName = Request["UserName"];
            string Name = Request["Name"];
            string UserPassword = MD5Helper.MD5Encrypt(Request["Password"]);
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SqlConnection con = DB.Con(); ;//创建连接数据库Connection对象
            string sql = "select * from tbUser where b_IsDeleted = 0 and s_UserName='" + UserName + "' AND s_GroupID='" + GroupID + "'";
            SqlCommand cmd = new SqlCommand(sql, con);//创建连接数据库SqlCommand对象，执行sql语句
            con.Open();//打开数据库
            SqlDataReader rd = cmd.ExecuteReader();//读取数据集
            if (rd.Read())
            {
                con.Close();
            }
            else
            {
                con.Close();// NEWID(),
                sql = "Insert Into tbUser (s_UserID,s_UserName,s_Name,s_UserPassword,s_GroupID,s_RoleID,n_RoleNO,b_IsDeleted, s_CreateUserID,d_CreateTime,s_UpdateUserID,d_UpdateTime)";
                sql += " values (NEWID(),'" + UserName + "','" + Name + "','" + UserPassword + "','" + GroupID + "','" + RoleID + "'," + RoleNO + ",0,'" + UserID + "','" + nowtime + "','" + UserID + "','" + nowtime + "')";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public int EditData()
        {
            int res = 0;
            int RoleNO = Convert.ToInt32(Request["sRoleE"]);
            string RoleID = DBHelper.getRoleID(RoleNO);
            string GroupID = Request["sGroupE"];
            string UserName = Request["UserNameE"];
            string Name = Request["NameE"];
            string UserPassword = MD5Helper.MD5Encrypt(Request["PasswordE"]);
            string sUserID = Request["tUserIDE"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "select * from tbUser where b_IsDeleted = 0 and s_UserName='" + UserName + "' and s_UserID<>'" + sUserID + "' AND s_GroupID='" + GroupID + "' ";
            SqlConnection con = DB.Con(); ;//创建连接数据库Connection对象
            SqlCommand cmd = new SqlCommand(sql, con);//创建连接数据库SqlCommand对象，执行sql语句
            con.Open();//打开数据库
            SqlDataReader rd = cmd.ExecuteReader();//读取数据集
            if (rd.Read())
            {
                con.Close();
            }
            else
            {
                con.Close();// NEWID(),
                sql = "update tbUser set s_UserName='" + UserName + "',s_Name = '" + Name + "',s_UserPassword='" + UserPassword + "',s_GroupID='" + GroupID + "',s_RoleID='" + RoleID + "',n_RoleNO=" + RoleNO;
                sql += ",s_UpdateUserID='" + UserID + "',d_UpdateTime='" + nowtime + "' where s_UserID = '" + sUserID + "'";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res ;
        }
        public int DelData(string delstr)
        {
            int res = 0;
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (delstr != "")
            {
                delstr = Func.LLeft(delstr, delstr.Length - 1);//去除字符串最后一个字符","
                string sql = "update tbUser set b_IsDeleted = 1 ,s_UpdateUserID='" + UserID + "',d_UpdateTime='" + nowtime + "' where s_UserID in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
    }
}

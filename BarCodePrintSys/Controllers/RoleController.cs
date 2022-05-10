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
    public class RoleController : Controller
    {
        //
        // GET: /Role/
        public ActionResult Index()
        {
            return View();
        }
        public string GetRole()
        {
            string sql;
            string SKey;
            int dcode = -1;
            int dcount = 0;
            string dmsg = "此查询无数据！";
            int nPages = Convert.ToInt32(Request["page"]);
            int nPageSize = Convert.ToInt32(Request["limit"]);
            int start = nPageSize * nPages - nPageSize + 1;//分页数据的开始序号
            int end = nPageSize * nPages;//分页数据的终止序号
            int role = DBHelper.getRoleNo(Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString()));
            Dictionary<String, Object> rsMap = new Dictionary<String, Object>();
            SKey = " b_IsDeleted = 0 AND n_RoleNO >" + role;
           // sql = "select * from tbRole where " + SKey + " order by n_Sort";
            sql = "SELECT * FROM (SELECT ROW_NUMBER() over(ORDER BY n_Sort ASC) AS num,";
            sql += "(SELECT  count(*)  FROM tbRole where  " + SKey + " ) AS tcount,";
            sql += "* FROM tbRole where " + SKey + " ) tbRole WHERE (num BETWEEN " + start + " AND " + end + ") ";
            DataSet ds = DBHelper.getDateSet(sql);
            var datas = ds.Tables[0].Rows;
            List<TRole> list = new List<TRole>();
            var total = datas.Count;
            if (total > 0)
            {
                dcode = 0;
                dmsg = "S";
                dcount = Convert.ToInt32(datas[0]["tcount"]);
                foreach (DataRow item in datas)
                {
                    var act = new TRole()
                    {
                        RoleID = item["s_RoleID"].ToString(),
                        RoleNO = Convert.ToInt32(item["n_RoleNO"]),
                        RoleName = item["s_RoleName"].ToString(),
                        Sort = Convert.ToInt32(item["n_Sort"]),
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
            int Sort = Convert.ToInt32(Request["nSort"]);
            int RoleNO = Convert.ToInt32(Request["nRole"]);
            string RoleName = Request["tRoleName"];
            int res = 0;
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "select * from tbRole where b_IsDeleted = 0 and s_RoleName='" + RoleName + "'";
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
                sql = "Insert Into tbRole (s_RoleID,s_RoleName,n_RoleNO,n_Sort,b_IsDeleted, s_CreateUserID,d_CreateTime,s_UpdateUserID,d_UpdateTime)";
                sql += " values (NEWID(),'" + RoleName + "'," + RoleNO + "," + Sort + ",0,'" + UserID + "','" + nowtime + "','" + UserID + "','" + nowtime + "')";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public int EditData()
        {
            int Sort = Convert.ToInt32(Request["nSortE"]);
            int RoleNO = Convert.ToInt32(Request["nRoleE"]);
            string RoleName = Request["tRoleNameE"];
            string RoleID = Request["tRoleIDE"];
            int res = 0;
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "select * from tbRole where b_IsDeleted = 0 and s_RoleName='" + RoleName + "'and s_RoleID<>'" + RoleID + "'";
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
                sql = "update tbRole set s_RoleName='" + RoleName + "',n_RoleNO=" + RoleNO + ",n_Sort=" + Sort;
                sql += ",s_UpdateUserID='" + UserID + "',d_UpdateTime='" + nowtime + "' where s_RoleID = '" + RoleID + "'";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public int DelData(string delstr)
        {
            int res = 0;
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (delstr != "")
            {
                delstr = Func.LLeft(delstr, delstr.Length - 1);//去除字符串最后一个字符","
                string sql = "update tbRole set b_IsDeleted = 1 ,s_UpdateUserID='" + UserID + "',d_UpdateTime='" + nowtime + "' where s_RoleID in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
    }
}

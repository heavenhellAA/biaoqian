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
    public class GroupController : Controller
    {
        //
        // GET: /Group/
        public ActionResult Index()
        {
            return View();
        }
        public string GetGroup()
        {
            string sql;
            int dcode = -1;
            int dcount = 0;
            string dmsg = "此查询无数据！";
            int nPages = Convert.ToInt32(Request["page"]);
            int nPageSize = Convert.ToInt32(Request["limit"]);
            int start = nPageSize * nPages - nPageSize + 1;//分页数据的开始序号
            int end = nPageSize * nPages;//分页数据的终止序号
            int role = DBHelper.getRoleNo(Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString()));

            Dictionary<String, Object> rsMap = new Dictionary<String, Object>();
            sql = "SELECT * FROM (SELECT ROW_NUMBER() over(ORDER BY n_Sort ASC) AS num,";
            sql += "(SELECT  count(*)  FROM tbGroup where b_IsDeleted = 0  ) AS tcount,";
            sql += "* FROM tbGroup where b_IsDeleted = 0  ) tbGroup WHERE (num BETWEEN " + start + " AND " + end + ") ";
            DataSet ds = DBHelper.getDateSet(sql);
            var datas = ds.Tables[0].Rows;
            List<TGroup> list = new List<TGroup>();
            var total = datas.Count;
            if (total > 0)
            {
                dcode = 0;
                dmsg = "S";
                dcount = Convert.ToInt32(datas[0]["tcount"]);
                foreach (DataRow item in datas)
                {
                    var act = new TGroup()
                    {
                        GroupID = item["s_GroupID"].ToString(),
                        GroupName = item["s_GroupName"].ToString(),
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
            int GroupNO = Convert.ToInt32(Request["nGroup"]);
            string GroupName = Request["tGroupName"];
            int res = 0;
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "select * from tbGroup where b_IsDeleted = 0 and s_GroupName='" + GroupName + "'";
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
                sql = "Insert Into tbGroup (s_GroupID,s_GroupName,n_Sort,b_IsDeleted, s_CreateUserID,d_CreateTime,s_UpdateUserID,d_UpdateTime)";
                sql += " values (NEWID(),'" + GroupName + "'," + Sort + ",0,'" + UserID + "','" + nowtime + "','" + UserID + "','" + nowtime + "')";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public int EditData()
        {
            int Sort = Convert.ToInt32(Request["nSortE"]);
            int GroupNO = Convert.ToInt32(Request["nGroupE"]);
            string GroupName = Request["tGroupNameE"];
            string GroupID = Request["tGroupIDE"];
            int res = 0;
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "select * from tbGroup where b_IsDeleted = 0 and s_GroupName='" + GroupName + "'and s_GroupID<>'" + GroupID + "'";
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
                sql = "update tbGroup set s_GroupName='" + GroupName + "',n_Sort=" + Sort;
                sql += ",s_UpdateUserID='" + UserID + "',d_UpdateTime='" + nowtime + "' where s_GroupID = '" + GroupID + "'";
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
                string sql = "update tbGroup set b_IsDeleted = 1 ,s_UpdateUserID='" + UserID + "',d_UpdateTime='" + nowtime + "' where s_GroupID in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
    }
}

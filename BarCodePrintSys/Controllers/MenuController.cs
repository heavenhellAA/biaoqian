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
    public class MenuController : Controller
    {
        // GET: /SubMenu/
        public ActionResult Index()
        {
            int role = DBHelper.getRoleNo(Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString()));
            ViewBag.role = DBHelper.getRoles(role);//获取权限组
            ViewBag.menu = DBHelper.getMenus(role);//获取菜单组
            return View();
          
        }
        public string GetMenus(string mName)
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
            SKey = " b_IsDeleted = 0 AND n_RoleNO >=" + role;
            //if (mName != "")
            //{
            //    SKey = "AND s_MenuName LIKE '%'+" + mName + "+'%'";
            //}
            sql = "select * from tbMenu where " + SKey + " order by n_Sort";
            //sql = "SELECT * FROM (SELECT ROW_NUMBER() over(ORDER BY n_Sort ASC) AS num,";
            //sql += "(SELECT  count(*)  FROM tbMenu where  " + SKey + " ) AS tcount,";
            //sql += "* FROM tbMenu where " + SKey + " ) tbMenu WHERE (num BETWEEN 1 AND 10) ";
            DataSet ds = DBHelper.getDateSet(sql);
            var datas = ds.Tables[0].Rows;
            List<Menu> list = new List<Menu>();
            var total = datas.Count;
            if (total > 0)
            {
                dcode = 0;
                dmsg = "S";
                dcount = total;
                foreach (DataRow item in datas)
                {
                    var act = new Menu()
                    {
                        Node = Convert.ToInt32(item["n_Node"]),
                        ParentNode = Convert.ToInt32(item["n_ParentNode"]),
                        MenuID = item["s_MenuID"].ToString(),
                        MenuName = item["s_MenuName"].ToString(),
                        Url = item["s_Url"].ToString(),
                        RoleNO = Convert.ToInt32(item["n_RoleNO"]),
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

            int ParentNode = Convert.ToInt32(Request["sMenu"]);
            int RoleNO = Convert.ToInt32(Request["sRole"]);
            int Sort = Convert.ToInt32(Request["nSort"]);
            string MenuName = Request["tMenuName"];
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string Url = Request["tUrl"];
            int res = 0;
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string sql = "select * from tbMenu where b_IsDeleted = 0 and s_MenuName='" + MenuName + "'";
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
                sql = "Insert Into tbMenu (s_MenuID,n_ParentNode,s_MenuName,s_Url,n_RoleNO,n_Sort,b_IsDeleted, s_CreateUserID,d_CreateTime,s_UpdateUserID,d_UpdateTime)";
                sql += " values (NEWID(),'" + ParentNode + "','" + MenuName + "','" + Url + "'," + RoleNO + "," + Sort + ",0,'" + UserID + "','" + nowtime + "','" + UserID + "','" + nowtime + "')";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public int EditData()
        {
            int ParentNode = Convert.ToInt32(Request["sMenuE"]);
            int RoleNO = Convert.ToInt32(Request["sRoleE"]);
            int Sort = Convert.ToInt32(Request["nSortE"]);
            string MenuID = Request["tMenuIDE"];
            string MenuName = Request["tMenuNameE"];
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string Url = Request["tUrlE"];
            int res = 0;
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string sql = "select * from tbMenu where b_IsDeleted = 0 and s_MenuName='" + MenuName + "'and s_MenuID<>'" + MenuID + "'";
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
                sql = "update tbMenu set n_ParentNode=" + ParentNode + ",s_MenuName='" + MenuName + "',s_Url='" + Url + "',n_RoleNO=" + RoleNO;
                sql += ",n_Sort=" + Sort + ",s_UpdateUserID='" + UserID + "',d_UpdateTime='" + nowtime + "' where s_MenuID = '" + MenuID + "'";
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
                string sql = "update tbMenu set b_IsDeleted = 1 ,s_UpdateUserID='" + UserID + "',d_UpdateTime='" + nowtime + "' where s_MenuID in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
    }
}

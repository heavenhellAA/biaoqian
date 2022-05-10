using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class BoyieltController : Controller
    {
        //
        // GET: /Boyielt/

        public ActionResult Index()
        {
            return View();
        }
        public string searchprint()
        {
            string sql;
            var page = Request["page"];
            var limit = Request["limit"];
            int page_sql = Convert.ToInt32(page);
            int limit_sql = Convert.ToInt32(limit);

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbBoyieltPrint where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbBoyieltPrint where n_state = 0";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows;
            //获取DS长度
            var total = datas.Count;
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
        //墨西哥飞利浦外箱
        public int AddBoyieltPrint()
        {
            var code = 0;
            string sql;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string gysmc = Func.Zhuru(Request["gysmc"]);
            string sczq = Func.Zhuru(Request["sczq"]);
            string yxq = Func.Zhuru(Request["yxq"]);
            string sl = Func.Zhuru(Request["sl"]);
            string scph = Func.Zhuru(Request["scph"]);
            string aslh = Func.Zhuru(Request["aslh"]);
            string ylbd = Func.Zhuru(Request["ylbd"]);
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            sql = "declare @count varchar(100)    set @count=1 While(convert(int,@count) <= '" + num_print + "') begin Insert Into tbBoyieltPrint(s_id,s_cnbqywm,s_gysmc,s_khlh,s_sczq,s_yxq,s_sl,s_scph,s_aslh,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + gysmc + "','" + khlh + "','" + sczq + "','" + yxq + "','" + sl + "','" + scph + "','" + aslh + "'";
            sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "',@count+'/" + num_print + "',0,'" + ylbd + "') set @count= @count + 1 end ";
            var warndata = sql.IndexOf("warning");
            if (warndata != -1)
            {
                code = -1;
                return code;
            }
            else
            {
                code = DBHelper.excuteNoQuery(sql);
                if (code == -1)
                {
                    code = 0;
                }
                return code;
            }
        }
        public int UpdatetbBoyieltPrint_BD()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbBoyieltPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
            code = DBHelper.excuteNoQuery(sql);
            return code;
        }
        public int Zuofei(string delstr)
        {
            int res = 0;
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            if (delstr != "")
            {
                delstr = Func.LLeft(delstr, delstr.Length - 1);//去除字符串最后一个字符","
                string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = "update tbBoyieltPrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
    }
}

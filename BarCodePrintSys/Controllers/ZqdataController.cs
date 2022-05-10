using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class ZqdataController : Controller
    {
        //
        // GET: /Zqdata/

        public ActionResult Index()
        {
            return View();
        }
        public string searchdata()
        {
            string sql;
            var page = Request["page"];
            var limit = Request["limit"];
            int page_sql = Convert.ToInt32(page);
            int limit_sql = Convert.ToInt32(limit);
            sql = "select tu.s_UserName,updatorname=ts.s_UserName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbSetZqdata where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbuser ts  on ts.s_UserID =a.s_updator  where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbSetZqdata where n_state = 0";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows;
            //获取DS长度
            var total = datas.Count;
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
        public int AddZqData()
        {
            var code = 0;
            string sql;
            string zqm = Func.Zhuru(Request["zqm"]);
            string scrq = Func.Zhuru(Request["scrq"]);
            string yxq = Func.Zhuru(Request["yxq"]);
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            sql = "If Not EXISTS(select * from tbSetZqData where s_zqm='" + zqm + "' and n_state = '0') Insert Into tbSetZqData(s_id,s_zqm,s_scrq,s_yxq,s_creator,s_createtime,n_state) ";
            sql += "values(NEWID(),'" + zqm + "','" + scrq + "','" + yxq + "','" + creatorid + "','" + nowtime + "',0 )";
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
        public int EditZqdata()
        {
            var code = 0;
            string sql;
            string id = Func.Zhuru(Request["id_e"]);
            string zqm = Func.Zhuru(Request["zqm_e"]);
            string scrq = Func.Zhuru(Request["scrq_e"]);
            string yxq = Func.Zhuru(Request["yxq_e"]);
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbSetZqData set s_zqm='" + zqm + "',s_scrq='" + scrq + "',s_yxq='" + yxq + "',s_updator='" + UserID + "',s_updatetime='" + nowtime + "'";
            sql += "where n_id ='" + id + "'";
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
        public int ZuofeiData(string delstr)
        {
            int res = 0;
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (delstr != "")
            {
                delstr = Func.LLeft(delstr, delstr.Length - 1);//去除字符串最后一个字符","
                string sql = "update tbSetZqData set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }


    }
}

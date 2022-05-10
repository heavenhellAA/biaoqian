using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class JsxinanstlController : Controller
    {
        //
        // GET: /Jsxinanstl/

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

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbJsxinanstlPrint where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbJsxinanstlPrint where n_state = 0";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows;
            //获取DS长度
            var total = datas.Count;
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
        public int Zuofei(string delstr)
        {
            int res = 0;
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            if (delstr != "")
            {
                delstr = Func.LLeft(delstr, delstr.Length - 1);//去除字符串最后一个字符","
                string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = "update tbJsxinanstlPrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public string AddJsxinanstlPrint()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string wllh = Func.Zhuru(Request["wllh"]);
            string jlh = Func.Zhuru(Request["jlh"]);
            string mpn = Func.Zhuru(Request["mpn"]);
            string wlmc = Func.Zhuru(Request["wlmc"]);
            string pp = Func.Zhuru(Request["pp"]);
            string sl = Func.Zhuru(Request["sl"]);
            string wlz = Func.Zhuru(Request["wlz"]);
            string wlgg = Func.Zhuru(Request["wlgg"]);
            string scsj = Func.Zhuru(Request["scsj"]);
            string dyry = Func.Zhuru(Request["dyry"]);
            string dysj = Func.Zhuru(Request["dysj"]);
            string gysdm = Func.Zhuru(Request["gysdm"]);
            string lsh = Getlsnum();
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string codemsg = gysdm+wllh+sl+scsj+lsh;
            string onecodemsg = codemsg;
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbJsxinanstlPrint(s_id,s_cnbqywm,s_wllh,s_jlh,s_mpn,s_wlmc,s_pp,s_sl,s_wlz,s_wlgg,s_scsj,s_dyry,s_dysj,s_gysdm,s_lsh,s_onecodemsg,s_codemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + wllh + "','" + jlh + "','" + mpn + "','" + wlmc + "','" + pp + "','" + sl + "','" + wlz + "','" + wlgg + "','" + scsj + "','" + dyry + "','" + dysj + "','" + gysdm + "','" + lsh + "','" + onecodemsg + "','" + codemsg + "'";
            sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "') ";
            var warndata = sql.IndexOf("warning");
            if (warndata != -1)
            {
                code = -1;
                string data = code + "," + lsnum;
                return data;
            }
            else
            {
                while (id <= num_print)
                {
                    lsh = Getlsnum();
                    codemsg = gysdm + wllh + sl + scsj + lsh;
                    onecodemsg = codemsg;
                    sql = "Insert Into tbJsxinanstlPrint(s_id,s_cnbqywm,s_wllh,s_jlh,s_mpn,s_wlmc,s_pp,s_sl,s_wlz,s_wlgg,s_scsj,s_dyry,s_dysj,s_gysdm,s_lsh,s_onecodemsg,s_codemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + cnbqywm + "','" + wllh + "','" + jlh + "','" + mpn + "','" + wlmc + "','" + pp + "','" + sl + "','" + wlz + "','" + wlgg + "','" + scsj + "','" + dyry + "','" + dysj + "','" + gysdm + "','" + lsh + "','" + onecodemsg + "','" + codemsg + "'";
                    sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "') ";
                    lsary = lsary + "," + Getlsnum();
                    code = DBHelper.excuteNoQuery(sql);
                    if (code == -1)
                    {
                        code = 0;
                    }
                    id += 1;
                }
                string data = code + "," + lsary;
                return data;
            }
        }
        //流水号获取
        public string Getlsnum()
        {

            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbJsxinanstlPrint where n_state = 0 )  set @lsnum = '00001' ";
            sql += "else  if exists (select n_id from tbJsxinanstlPrint where n_state = 0 ) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_lsh,5) as lsnum from tbJsxinanstlPrint where n_state=0  order by n_id DESC )  = '99999' then  '00001' ";
            sql += "else substring(convert(varchar,convert(int,'00001')+('1'+(select top 1 right(s_lsh,5) as lsnum from tbJsxinanstlPrint where n_state=0  order by n_id DESC ))),2,5) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        public int UpdatetbJsxinanstlPrint_BD()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbJsxinanstlPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
            code = DBHelper.excuteNoQuery(sql);
            return code;
        }
    }
}

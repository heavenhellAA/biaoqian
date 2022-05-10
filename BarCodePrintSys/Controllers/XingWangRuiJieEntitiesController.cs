using BarCodePrintSys.Models.Prints;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class XingWangRuiJieEntitiesController : Controller
    {
        //XingWangRuiJieEntityDBContext xingWangRuiJieEntityDB = new XingWangRuiJieEntityDBContext();
        // GET: XingWangRuiJie
        public ActionResult Index()
        {
            //ViewData["XingWangRuiJieEntities"] =  ;xingWangRuiJieEntityDB.XingWangRuiJie.Find()
            return View();
        }

        public int UpdatePrint_BD()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbXingWangRuiJiePrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
            code = DBHelper.excuteNoQuery(sql);
            return code;
        }
        public string Getlsnum(string chrq)
        {
            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbXingWangRuiJiePrint where n_state = 0 and s_chrq = '" + chrq + "')  set @lsnum = '001' ";
            sql += "else  if exists (select n_id from tbXingWangRuiJiePrint where n_state = 0 and s_chrq = '" + chrq + "') set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_lsh,3) as lsnum from tbXingWangRuiJiePrint where n_state=0 and s_chrq = '" + chrq + "' order by n_id DESC )  = '999' then  '001' ";
            sql += "else substring(convert(varchar,convert(int,'001')+('1'+(select top 1 right(s_lsh,3) as lsnum from tbXingWangRuiJiePrint where n_state=0 and s_chrq = '" + chrq + "' order by n_id DESC ))),2,3) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
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
                string sql = "update tbXingWangRuiJiePrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public string searchprint()
        {
            string sql;
            var page = Request["page"];
            var limit = Request["limit"];
            int page_sql = Convert.ToInt32(page);
            int limit_sql = Convert.ToInt32(limit);

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* " +
                "from (select *,row_number() over (order by n_id DESC) as id from " +
                "tbXingWangRuiJiePrint where n_state = 0) a " +
                "left join tbuser tu on  tu.s_UserID = a.s_creator " +
                "left join tbGroup tg  on tg.s_GroupID = a.s_Groupid " +
                "left join tbRole tr  on tr.s_RoleID = a.s_Roleid " +
                "where id between('" + limit_sql + "' * ('" + page_sql + "' - 1) + 1) " +
                "and '" + limit_sql + "' * ('" + page_sql + "' - 1) + '" + limit_sql + "' " +
                "order by id";

            sql+= " select COUNT(n_id)  as zongshu " +
                "from tbXingWangRuiJiePrint where n_state = 0";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows;
            //获取DS长度
            var total = datas.Count;
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
    }
}
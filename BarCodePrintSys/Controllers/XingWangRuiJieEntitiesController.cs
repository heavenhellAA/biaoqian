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
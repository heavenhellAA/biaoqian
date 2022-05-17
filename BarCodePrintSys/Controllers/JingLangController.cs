using System;
using System.Data;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class JingLangController : Controller
    {
        // GET: JingLang
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

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbJingLangWuLiaoPrint where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbJingLangWuLiaoPrint where n_state = 0";
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
                string sql = "update tbJingLangWuLiaoPrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public string Getlsnum()
        {

            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbJingLangWuLiaoPrint where n_state = 0 )  set @lsnum = '00001' ";
            sql += "else  if exists (select n_id from tbJingLangWuLiaoPrint where n_state = 0 ) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_lsh,5) as lsnum from tbJingLangWuLiaoPrint where n_state=0  order by n_id DESC )  = '99999' then  '00001' ";
            sql += "else substring(convert(varchar,convert(int,'00001')+('1'+(select top 1 right(s_lsh,5) as lsnum from tbJingLangWuLiaoPrint where n_state=0  order by n_id DESC ))),2,5) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        public int UpdatePrint_BD()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbJingLangWuLiaoPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
            code = DBHelper.excuteNoQuery(sql);
            return code;
        }
        public string AddPrint()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;

            string s_id = Request["s_id"];
            string s_cnbqywm = Request["s_cnbqywm"];
            string s_gysdm = Request["s_gysdm"];
            string s_wlbm = Request["s_wlbm"];
            string s_wlname = Request["s_wlname"];
            string s_wlgg = Request["s_wlgg"];
            string DC = Request["DC"];
            string LtNo = Request["LtNo"];
            string s_sl = Request["s_sl"];
            string s_amount = Request["s_amount"];
            string ylbd = Request["ylbd"];

            //string lsh = Getlsnum();
            int n_state = 0;

            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string s_creator = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string s_Groupid = DBHelper.getuserGroup(s_creator);
            string s_Roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());

            string s_lsh = "";


            sql = "Insert Into tbJingLangWuLiaoPrint(s_id,s_cnbqywm,s_gysdm,s_wlbm,s_wlname,s_wlgg,DC,LtNo,s_Groupid,s_Roleid,s_lsh,s_sl,n_state,n_bdprint,s_amount,s_creator,s_updator,s_createtime,s_updatetime)";
            sql += "values( '"+s_id + "','" + s_cnbqywm + "','" + s_gysdm + "','" + s_wlbm + "','" + s_wlname + "','" + s_wlgg + "','" + DC + "','" + LtNo + "','" + s_Groupid + "','" + s_Roleid + "','" + s_lsh + "','" + s_sl + "','" + n_state + "','" + ylbd + "','" + s_amount + "','" + s_creator + "','" + null + "','" + nowtime + "','" + null + "') ";

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
                    s_lsh = Getlsnum();
                    if (s_lsh.Length == 0)
                    {
                        s_lsh = "1";
                    }
                    sql = "Insert Into tbJingLangWuLiaoPrint(s_id,s_cnbqywm,s_gysdm,s_wlbm,s_wlname,s_wlgg,DC,LtNo,s_Groupid,s_Roleid,s_lsh,s_sl,n_state,n_bdprint,s_amount,s_creator,s_updator,s_createtime,s_updatetime)";
                    sql += "values('" + s_id + "','" + s_cnbqywm + "','" + s_gysdm + "','" + s_wlbm + "','" + s_wlname + "','" + s_wlgg + "','" + DC + "','" + LtNo + "','" + s_Groupid + "','" + s_Roleid + "','" + s_lsh + "','" + s_sl + "','" + n_state + "','" + ylbd + "','" + s_amount + "','" + s_creator + "','" + null + "','" + nowtime + "','" + null + "') ";

                    //lsary = lsary + "," + Getlsnum();
                    code = DBHelper.excuteNoQuery(sql);
                    if (code == -1)
                    {
                        code = 0;
                    }
                   
                    id += 1;
                }

                string data = code.ToString();

                return data;
            }

        }
    }
}


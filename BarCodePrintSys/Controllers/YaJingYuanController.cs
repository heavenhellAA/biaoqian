using System;
using System.Data;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class YaJingYuanController : Controller
    {
        // GET: YaJingYuan
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

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbYaJingYuanChanPinPrint where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbYaJingYuanChanPinPrint where n_state = 0";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows;
            //获取DS长度
            var total = datas.Count;
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }

        public string AddYaJingYuanPrint()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;

            var n_state = "0";
            var s_cnbqywm = Request["s_cnbqywm"];
            var s_packageType = Request["s_packageType"];
            var s_AMCwlNum = Request["s_AMCwlNum"];
            var s_cpgg = Request["s_cpgg"];
            var s_bzAmount = Request["s_bzAmount"];
            var s_PoOrder = Request["s_PoOrder"];
            var s_jhDate = Request["s_jhDate"];
            var s_scDate = Request["s_scDate"];
            var s_scNum = Request["s_scNum"];
            var s_Dc = Request["s_Dc"];
            var s_mhDc = Request["s_mhDc"];
            var s_scMsg = Request["s_scMsg"];
            var EWMmsg = Request["EWMmsg"];
            var n_bdprint = Request["n_bdprint"];
            //var num_print = Request["num_print"];
            var ylbd = Request["ylbd"];
            var s_id = "liushui";
            var s_waternum = s_id;


            int num_print = int.Parse(Request["num_print"]);
            string s_creator = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string s_Groupid = DBHelper.getuserGroup(s_creator);
            string s_Roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());

            //string a = Tools(i,o,d);
            //Tools tools = new Tools();
            sql = "INSERT INTO HNAH_BarCodePrintSys.dbo.tbYaJingYuanChanPinPrint ";
            sql += "(s_id, s_cnbqywm, s_packageType, s_AMCwlNum, s_cpgg, s_bzAmount, s_PoOrder, s_jhDate, s_scDate, s_scNum, s_Dc, s_mhDc, EWMmsg, s_creator, s_updator, s_createtime, s_updatetime, s_Groupid, s_Roleid, s_waternum, n_state, n_bdprint, s_scMsg) ";
            sql += "VALUES(" + "'" + s_id + "','" + s_cnbqywm + "','" + s_packageType + "','" + s_AMCwlNum + "','" + s_cpgg + "','" + s_bzAmount + "','" + s_PoOrder + "','" + s_jhDate + "','" + s_scDate + "','" + s_scNum + "','" + s_Dc + "','" + s_mhDc + "','" + EWMmsg + "','" + s_creator + "','" + null + "','" + nowtime + "','" + null + "','" + s_Groupid + "','" + s_Roleid + "','" + s_waternum + "','" + n_state + "','" + n_bdprint + "','" + s_scMsg + "');";
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
                    //s_lsh = Getlsnum();
                    //if (s_lsh.Length == 0)
                    //{
                    //    s_lsh = "1";
                    //}
                    sql = "INSERT INTO HNAH_BarCodePrintSys.dbo.tbYaJingYuanChanPinPrint ";
                    sql += "(s_id, s_cnbqywm, s_packageType, s_AMCwlNum, s_cpgg, s_bzAmount, s_PoOrder, s_jhDate, s_scDate, s_scNum, s_Dc, s_mhDc, EWMmsg, s_creator, s_updator, s_createtime, s_updatetime, s_Groupid, s_Roleid, s_waternum, n_state, n_bdprint, s_scMsg) ";
                    sql += "VALUES(" + "'" + s_id + "','" + s_cnbqywm + "','" + s_packageType + "','" + s_AMCwlNum + "','" + s_cpgg + "','" + s_bzAmount + "','" + s_PoOrder + "','" + s_jhDate + "','" + s_scDate + "','" + s_scNum + "','" + s_Dc + "','" + s_mhDc + "','" + EWMmsg + "','" + s_creator + "','" + null + "','" + nowtime + "','" + null + "','" + s_Groupid + "','" + s_Roleid + "','" + s_waternum + "','" + n_state + "','" + n_bdprint + "','" + s_scMsg + "');";

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
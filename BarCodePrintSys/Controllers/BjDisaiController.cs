using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class BjDisaiController : Controller
    {
        //
        // GET: /BjDisai/

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

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbBjDisaiPrint where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbBjDisaiPrint where n_state = 0";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows;
            //获取DS长度
            var total = datas.Count;
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
        public string Getlsnum(string scrq)
        {

            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbBjDisaiPrint where n_state = 0 and '" + scrq + "' = s_scrq)  set @lsnum = '00001' ";
            sql += "else  if exists (select n_id from tbBjDisaiPrint where n_state = 0 and '" + scrq + "' = s_scrq) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_lsh,5) as lsnum from tbBjDisaiPrint where n_state=0 and '" + scrq + "' = s_scrq order by n_id DESC )  = '99999' then  '00001' ";
            sql += "else substring(convert(varchar,convert(int,'00001')+('1'+(select top 1 right(s_lsh,5) as lsnum from tbBjDisaiPrint where n_state=0 and '" + scrq + "' = s_scrq order by n_id DESC ))),2,5) end )";
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
                string sql = "update tbBjDisaiPrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public string AddBjDisaiPrint()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string packageType = Func.Zhuru(Request["packageType"]);
            string wldm = Func.Zhuru(Request["wldm"]);
            string sl = Func.Zhuru(Request["sl"]);
            string scpc = Func.Zhuru(Request["scpc"]);
            string sczq = Func.Zhuru(Request["sczq"]);
            string ggxx = Func.Zhuru(Request["ggxx"]);
            string gys = Func.Zhuru(Request["gys"]);
            string dd = Func.Zhuru(Request["dd"]);
            string pp = Func.Zhuru(Request["pp"]);
            string scgc = Func.Zhuru(Request["scgc"]);
            string lh = Func.Zhuru(Request["lh"]);
            string gysbm = Func.Zhuru(Request["gysbm"]);
            string scrq = Func.Zhuru(Request["scrq"]);
            string ip = Func.Zhuru(Request["ip"]);
            string lsh = Getlsnum(scrq);
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string wym = lh + ip + gysbm + scrq + lsh;
            string codemsg = wldm + '|' + sl + '|' + scpc + '|' + sczq + '|' + ggxx + '|' + gys + '|' + dd + '|' + pp + '|' + scgc + '|' + wym;
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbBjDisaiPrint(s_id,s_cnbqywm,s_packageType,s_wldm,s_sl,s_scpc,s_sczq,s_ggxx,s_gys,s_dd,s_pp,s_scgc,s_wym,s_lh,s_ip,s_gysbm,s_scrq,s_lsh,s_codemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + packageType + "','" + wldm + "','" + sl + "','" + scpc + "','" + sczq + "','" + ggxx + "','" + gys + "','" + dd + "','" + pp + "','" + scgc + "','" + wym + "','" + lh + "','" + ip + "','" + gysbm + "','" + scrq + "','" + lsh + "','" + codemsg + "'";
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
                    lsh = Getlsnum(scrq);
                    wym = lh + ip + gysbm + scrq + lsh;
                    codemsg = wldm + '|' + sl + '|' + scpc + '|' + sczq + '|' + ggxx + '|' + gys + '|' + dd + '|' + pp + '|' + scgc + '|' + wym;
                    sql = "Insert Into tbBjDisaiPrint(s_id,s_cnbqywm,s_packageType,s_wldm,s_sl,s_scpc,s_sczq,s_ggxx,s_gys,s_dd,s_pp,s_scgc,s_wym,s_lh,s_ip,s_gysbm,s_scrq,s_lsh,s_codemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + cnbqywm + "','" + packageType + "','" + wldm + "','" + sl + "','" + scpc + "','" + sczq + "','" + ggxx + "','" + gys + "','" + dd + "','" + pp + "','" + scgc + "','" + wym + "','" + lh + "','" + ip + "','" + gysbm + "','" + scrq + "','" + lsh + "','" + codemsg + "'";
                    sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "') ";
                    lsary = lsary + "," + lsh;
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

        public int UpdatetbBjDisaiPrint_BD()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbBjDisaiPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
            code = DBHelper.excuteNoQuery(sql);
            return code;
        }
    }
}

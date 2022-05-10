using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class KsrenbaoController : Controller
    {
        //
        // GET: /Ksrenbao/

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

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbKsrenbaoPrint where n_state = 0 ) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbKsrenbaoPrint where n_state = 0";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows;
            //获取DS长度
            var total = datas.Count;
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
        //流水号获取
        public string GetlsnumZX(string scrq)
        {

            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbKsrenbaoPrint where n_state = 0 and s_packageType = 0 and '" + scrq + "' = s_scrq)  set @lsnum = '001' ";
            sql += "else  if exists (select n_id from tbKsrenbaoPrint where n_state = 0 and s_packageType =0 and '" + scrq + "' = s_scrq) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_lsh,3) as lsnum from tbKsrenbaoPrint where n_state=0 and s_packageType =0 and '" + scrq + "' = s_scrq order by n_id DESC )  = '999' then  '001' ";
            sql += "else substring(convert(varchar,convert(int,'001')+('1'+(select top 1 right(s_lsh,3) as lsnum from tbKsrenbaoPrint where n_state=0 and s_packageType =0 and '" + scrq + "' = s_scrq order by n_id DESC ))),2,3) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        public int AddKsrenbaoPrint()
        {
            var code = 0;
            string sql;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string packageType = Func.Zhuru(Request["packageType"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string sczq = Func.Zhuru(Request["sczq"]);
            string scph = Func.Zhuru(Request["scph"]);
            string gysdm = Func.Zhuru(Request["gysdm"]);
            string sl = Func.Zhuru(Request["sl"]);
            string aslh = Func.Zhuru(Request["aslh"]);
            string ylbd = Func.Zhuru(Request["ylbd"]);
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            string codemsg = khlh + ';' + sczq + ';' + scph + ';' + gysdm + ';' + sl + ';' + aslh;
            sql = "declare @count varchar(100)    set @count=1 While(convert(int,@count) <= '" + num_print + "') begin Insert Into tbKsrenbaoPrint(s_id,s_cnbqywm,s_packageType,s_khlh,s_sczq,s_scph,s_gysdm,s_sl,s_aslh,s_codemsg1,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + packageType + "','" + khlh + "','" + sczq + "','" + scph + "','" + gysdm + "','" + sl + "','" + aslh + "','" + codemsg + "'";
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
        public string AddKsrenbaoPrintZX()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string packageType = Func.Zhuru(Request["packageType"]);
            string scrq = Func.Zhuru(Request["scrq"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string sczq = Func.Zhuru(Request["sczq"]);
            string scph = Func.Zhuru(Request["scph"]);
            string gysdm = Func.Zhuru(Request["gysdm"]);
            string sl = Func.Zhuru(Request["sl"]);
            string aslh = Func.Zhuru(Request["aslh"]);
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string lsh = GetlsnumZX(scrq);
            string codemsg = gysdm + khlh + scrq + lsh + ';' + khlh + ';' + sczq + ';' + scph + ';' + gysdm + ';' + sl + ';' + aslh;
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbKsrenbaoPrint(s_id,s_cnbqywm,s_packageType,s_khlh,s_sczq,s_scph,s_gysdm,s_sl,s_aslh,s_scrq,s_lsh,s_codemsg2,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + packageType + "','" + khlh + "','" + sczq + "','" + scph + "','" + gysdm + "','" + sl + "','" + aslh + "','" + scrq + "','" + lsh + "','" + codemsg + "'";
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
                    lsh = GetlsnumZX(scrq);
                    codemsg = gysdm + khlh + scrq + lsh + ';' + khlh + ';' + sczq + ';' + scph + ';' + gysdm + ';' + sl + ';' + aslh;
                    sql = "Insert Into tbKsrenbaoPrint(s_id,s_cnbqywm,s_packageType,s_khlh,s_sczq,s_scph,s_gysdm,s_sl,s_aslh,s_scrq,s_lsh,s_codemsg2,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + cnbqywm + "','" + packageType + "','" + khlh + "','" + sczq + "','" + scph + "','" + gysdm + "','" + sl + "','" + aslh + "','" + scrq + "','" + lsh + "','" + codemsg + "'";
                    sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "') ";
                    lsary = lsary + "," + GetlsnumZX(scrq);
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
        public int UpdateKsrenbaoPrint_BD()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbKsrenbaoPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
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
                string sql = "update tbKsrenbaoPrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
    }
}

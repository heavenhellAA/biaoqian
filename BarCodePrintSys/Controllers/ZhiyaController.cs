using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class ZhiyaController : Controller
    {
        //
        // GET: /Zhiya/

        public ActionResult Index()
        {
            return View();
        }
        public string Getlsnum()
        {
            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbZhiyaPrint where n_state = 0 and s_packageType = 2)  set @lsnum = '000001' ";
            sql += "else  if exists (select n_id from tbZhiyaPrint where n_state = 0 and s_packageType = 2) set @lsnum = ( ";
            sql += "case when  (select top 1 SUBSTRING(s_rid,8,6) as lsnum from tbZhiyaPrint where n_state=0 and s_packageType = 2 order by n_id DESC )  = '999999' then  '000001' ";
            sql += "else substring(convert(varchar,convert(int,'000001')+('1'+(select top 1 right(s_rid,6) as lsnum from tbZhiyaPrint where n_state=0 and s_packageType = 2 order by n_id DESC ))),2,6) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        public string searchprint()
        {
            string sql;
            var page = Request["page"];
            var limit = Request["limit"];
            int page_sql = Convert.ToInt32(page);
            int limit_sql = Convert.ToInt32(limit);

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbZhiyaPrint where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbZhiyaPrint where n_state = 0";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows;
            //获取DS长度
            var total = datas.Count;
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
        public string AddZhiyaPrint()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string packtype = Func.Zhuru(Request["packtype"]);
            string wlh = Func.Zhuru(Request["wlh"]);
            string pch = Func.Zhuru(Request["pch"]);
            string pn = Func.Zhuru(Request["pn"]);
            string sl = Func.Zhuru(Request["sl"]);
            string rid = Func.Zhuru(Request["rid"]) + Getlsnum();
            string ylbd = Func.Zhuru(Request["ylbd"]);
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbZhiyaPrint(s_id,s_packageType,s_rid,s_wlh,s_pch,s_pn,s_sl,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + packtype + "','" + rid + "','" + wlh + "','" + pch + "','" + pn + "','" + sl + "'";
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
                    rid = Func.Zhuru(Request["rid"]) + Getlsnum();
                    string codemsg = "RID:" + rid + ',' +"MID:"+ wlh + ',' +"QTY:"+ sl + ',' +"DC:"+ pch + ',' + "PN:"+ pn ;
                    sql = "Insert Into tbZhiyaPrint(s_id,s_packageType,s_rid,s_wlh,s_pch,s_pn,s_sl,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,s_cnbqywm,s_codemsg,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + packtype + "','" + rid + "','" + wlh + "','" + pch + "','" + pn + "','" + sl + "'";
                    sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "','" + cnbqywm + "','" + codemsg + "',0,'" + ylbd + "') ";
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
        public int AddZhiyaPrint_zx()
        {
            var code = 0;
            string sql;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string packtype = Func.Zhuru(Request["packtype"]);
            string wlh = Func.Zhuru(Request["wlh"]);
            string pch = Func.Zhuru(Request["pch"]);
            string pn = Func.Zhuru(Request["pn"]);
            string sl = Func.Zhuru(Request["sl"]);
            string ylbd = Func.Zhuru(Request["ylbd"]);
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            sql = "declare @count varchar(100)    set @count=1 While(convert(int,@count) <= '" + num_print + "') begin Insert Into tbZhiyaPrint(s_id,s_packageType,s_wlh,s_pch,s_pn,s_sl,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,s_cnbqywm,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + packtype + "','" + wlh + "','" + pch + "','" + pn + "','" + sl + "'";
            sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "',@count+'/" + num_print + "','" + cnbqywm + "',0,'" + ylbd + "') set @count= @count + 1 end ";
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

        public int UpdateZhiyaPrint_BD()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbZhiyaPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
            code = DBHelper.excuteNoQuery(sql);
            return code;
        }

        public int UpdateZhiyaPrint_BDZX()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbZhiyaPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
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
                string sql = "update tbZhiyaPrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }

    }
}

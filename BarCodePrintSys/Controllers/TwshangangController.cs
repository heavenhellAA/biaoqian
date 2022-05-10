using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class TwshangangController : Controller
    {
        //
        // GET: /Twshangang/

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

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbTwshangangPrint where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbTwshangangPrint where n_state = 0";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows;
            //获取DS长度
            var total = datas.Count;
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
        public string Getlsnum()
        {
            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbTwshangangPrint where n_state = 0 and s_khmc = 0)  set @lsnum = '000001' ";
            sql += "else  if exists (select n_id from tbTwshangangPrint where n_state = 0 and s_khmc = 0) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_lsh,6) as lsnum from tbTwshangangPrint where n_state=0 and s_khmc = 0 order by n_id DESC )  = '999999' then  '000001' ";
            sql += "else substring(convert(varchar,convert(int,'000001')+('1'+(select top 1 right(s_lsh,6) as lsnum from tbTwshangangPrint where n_state=0 and s_khmc = 0 order by n_id DESC ))),2,6) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        public string GetlsnumZX()
        {
            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbTwshangangPrint where n_state = 0 and s_khmc = 1)  set @lsnum = '00001' ";
            sql += "else  if exists (select n_id from tbTwshangangPrint where n_state = 0 and s_khmc = 1) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_lsh,5) as lsnum from tbTwshangangPrint where n_state=0 and s_khmc = 1 order by n_id DESC )  = '99999' then  '00001' ";
            sql += "else substring(convert(varchar,convert(int,'00001')+('1'+(select top 1 right(s_lsh,5) as lsnum from tbTwshangangPrint where n_state=0 and s_khmc = 1 order by n_id DESC ))),2,5) end )";
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
                string sql = "update tbTwshangangPrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public string AddTwshangangPrint()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string khmc = Func.Zhuru(Request["khmc"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string gysdm = Func.Zhuru(Request["gysdm"]);
            string sl = Func.Zhuru(Request["sl"]);
            string zq = Func.Zhuru(Request["zq"]);
            string lsh = Getlsnum();
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string onecodemsg = khlh + '-' + gysdm + '-' + sl + '-' + zq + '-' + lsh;
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbTwshangangPrint(s_id,s_cnbqywm,s_khmc,s_khlh,s_gysdm,s_sl,s_zq,s_lsh,s_onecodemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + khmc + "','" + khlh + "','" + gysdm + "','" + sl + "','" + zq + "','" + lsh + "','" + onecodemsg + "'";
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
                    onecodemsg = khlh + '-' + gysdm + '-' + sl + '-' + zq + '-' + lsh;
                    sql = "Insert Into tbTwshangangPrint(s_id,s_cnbqywm,s_khmc,s_khlh,s_gysdm,s_sl,s_zq,s_lsh,s_onecodemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + cnbqywm + "','" + khmc + "','" + khlh + "','" + gysdm + "','" + sl + "','" + zq + "','" + lsh + "','" + onecodemsg + "'";
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
        public string AddTwshangangPrintZX()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string khmc = Func.Zhuru(Request["khmc"]);
            string gysdm = Func.Zhuru(Request["gysdm"]);
            string scrq = Func.Zhuru(Request["scrq"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string cpms = Func.Zhuru(Request["cpms"]);
            string zq = Func.Zhuru(Request["zq"]);
            string scph = Func.Zhuru(Request["scph"]);
            string sl = Func.Zhuru(Request["sl"]);
            string aslh = Func.Zhuru(Request["aslh"]);
            string lsh = GetlsnumZX();
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string codemsg = "RLC" + gysdm + scrq + lsh + ',' + khlh + ',' + zq + "-AISHI," + scph + ',' + sl;
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbTwshangangPrint(s_id,s_cnbqywm,s_khmc,s_gysdm,s_khlh,s_sl,s_zq,s_lsh,s_scrq,s_cpms,s_scph,s_aslh,s_codemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + khmc + "','" + gysdm + "','" + khlh + "','" + sl + "','" + zq + "','" + lsh + "','" + scrq + "','" + cpms + "','" + scph + "','" + aslh + "','" + codemsg + "'";
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
                    lsh = GetlsnumZX();
                    codemsg = "RLC" + gysdm + scrq + lsh + ',' + khlh + ',' + zq + "-AISHI," + scph + ',' + sl;
                    sql = "Insert Into tbTwshangangPrint(s_id,s_cnbqywm,s_khmc,s_gysdm,s_khlh,s_sl,s_zq,s_lsh,s_scrq,s_cpms,s_scph,s_aslh,s_codemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + cnbqywm + "','" + khmc + "','" + gysdm + "','" + khlh + "','" + sl + "','" + zq + "','" + lsh + "','" + scrq + "','" + cpms + "','" + scph + "','" + aslh + "','" + codemsg + "'";
                    sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "') ";
                    lsary = lsary + "," + GetlsnumZX();
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

        public int UpdatetbTwshangangPrint_BD()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbTwshangangPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
            code = DBHelper.excuteNoQuery(sql);
            return code;
        }
    }
}

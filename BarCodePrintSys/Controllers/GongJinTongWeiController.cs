using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class GongJinTongWeiController : Controller
    {
        //
        // GET: /GongJinTongWei/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DataSet()
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
            sql = "select tu.s_UserName,updatorname=ts.s_UserName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbGongJinTongWeiData where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbuser ts  on ts.s_UserID =a.s_updator  where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbGongJinTongWeiData where n_state = 0";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows;
            //获取DS长度
            var total = datas.Count;
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
        public int ZuofeiData(string delstr)
        {
            int res = 0;
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (delstr != "")
            {
                delstr = Func.LLeft(delstr, delstr.Length - 1);//去除字符串最后一个字符","
                string sql = "update tbGongJinTongWeiData set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public int AddGongJinTongWeiData()
        {
            var code = 0;
            string sql;
            string khlh = Func.Zhuru(Request["d_khlh"]);
            string nw = Func.Zhuru(Request["d_nw"]);
            string gw = Func.Zhuru(Request["d_gw"]);
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            sql = "If Not EXISTS(select * from tbGongJinTongWeiData where s_khlh='" + khlh + "' and n_state = '0') Insert Into tbGongJinTongWeiData(s_id,s_khlh,s_nw,s_gw,s_creator,s_createtime,n_state) ";
            sql += "values(NEWID(),'" + khlh + "','" + nw + "','" + gw + "','" + creatorid + "','" + nowtime + "',0 )";
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
        public int AddGongJinTongWeiDataE()
        {
            var code = 0;
            string sql;
            string id = Func.Zhuru(Request["d_id_e"]);
            string khlh = Func.Zhuru(Request["d_khlh_e"]);
            string nw = Func.Zhuru(Request["d_nw_e"]);
            string gw = Func.Zhuru(Request["d_gw_e"]);
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbGongJinTongWeiData set s_khlh='" + khlh + "',s_nw='" + nw + "',s_gw='" + gw + "',s_updator='" + UserID + "',s_updatetime='" + nowtime + "'";
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

        //标签打印
        public string gtdata()
        {
            string sql;
            string khlh = Request["khlh"];
            sql = "select * from tbGongJinTongWeiData where n_state = 0 and s_khlh = '" + khlh + "'";
            DataSet ds = DBHelper.getDateSet(sql);
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
        public string searchprint()
        {
            string sql;
            var page = Request["page"];
            var limit = Request["limit"];
            int page_sql = Convert.ToInt32(page);
            int limit_sql = Convert.ToInt32(limit);

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbGongJinTongWeiPrint where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbGongJinTongWeiPrint where n_state = 0";
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
                string sql = "update tbGongJinTongWeiPrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public string Getlsnum()
        {

            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbGongJinTongWeiPrint where n_state = 0 )  set @lsnum = '00001' ";
            sql += "else  if exists (select n_id from tbGongJinTongWeiPrint where n_state = 0 ) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_rid,5) as lsnum from tbGongJinTongWeiPrint where n_state=0  order by n_id DESC )  = '99999' then  '00001' ";
            sql += "else substring(convert(varchar,convert(int,'00001')+('1'+(select top 1 right(s_rid,5) as lsnum from tbGongJinTongWeiPrint where n_state=0  order by n_id DESC ))),2,5) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        public string AddGongJinTongWeiPrint()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string packtype = Func.Zhuru(Request["packtype"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string name = Func.Zhuru(Request["name"]);
            string aslh = Func.Zhuru(Request["aslh"]);
            string ggxh = Func.Zhuru(Request["ggxh"]);
            string sl = Func.Zhuru(Request["sl"]);
            string sczq = Func.Zhuru(Request["sczq"]);
            string nw = Func.Zhuru(Request["nw"]);
            string gw = Func.Zhuru(Request["gw"]);
            string scph = Func.Zhuru(Request["scph"]);
            string scphlist = Func.Zhuru(Request["scphlist"]);
            string partType = Func.Zhuru(Request["partType"]);
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string lsh = Getlsnum();
            string rid = Func.Zhuru(Request["rid"])+lsh;
            string codemsg = rid + '&' + khlh + '&' + sl + '&' + "A772#" + sczq + '#' + scph;
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbGongJinTongWeiPrint(s_id,s_cnbqywm,s_packageType,s_partType,s_rid,s_khlh,s_name,s_aslh,s_ggxh,s_sl,s_sczq,s_nw,s_gw,s_scphlist,s_codemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + packtype + "','" + partType + "','" + rid + "','" + khlh + "','" + name + "','" + aslh + "','" + ggxh + "','" + sl + "','" + sczq + "','" + nw + "','" + gw + "','" + scphlist + "','" + codemsg + "'";
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
                    rid = Func.Zhuru(Request["rid"]) + lsh;
                    codemsg = rid + '&' + khlh + '&' + sl + '&' + "A772#" + sczq + '#' + scph;
                    sql = "Insert Into tbGongJinTongWeiPrint(s_id,s_cnbqywm,s_packageType,s_partType,s_rid,s_khlh,s_name,s_aslh,s_ggxh,s_sl,s_sczq,s_nw,s_gw,s_scphlist,s_codemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + cnbqywm + "','" + packtype + "','" + partType + "','" + rid + "','" + khlh + "','" + name + "','" + aslh + "','" + ggxh + "','" + sl + "','" + sczq + "','" + nw + "','" + gw + "','" + scphlist + "','" + codemsg + "'";
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
        public int UpdatetbGongJinTongWeiPrint_BD()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbGongJinTongWeiPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
            code = DBHelper.excuteNoQuery(sql);
            return code;
        }
    }
}

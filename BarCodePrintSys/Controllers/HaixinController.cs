using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class HaixinController : Controller
    {
        //
        // GET: /Haixin/

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

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbHaixinPrint where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbHaixinPrint where n_state = 0";
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
                string sql = "update tbHaixinPrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public string AddHaixinPrint()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string haixinType = Func.Zhuru(Request["haixinType"]);
            string gysdm = Func.Zhuru(Request["gysdm"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string wlms = Func.Zhuru(Request["wlms"]);
            string scrq = Func.Zhuru(Request["scrq"]);
            var arr = scrq.Split('/');
            string month_32 = Func.int10Convert36(Convert.ToInt32(arr[1])); 
            string dat_32 = Func.int10Convert36(Convert.ToInt32(arr[2]));
            string nian_32 = scrq.Substring(2, 2);
            string scrq_32 = nian_32 + month_32 + dat_32;
            string sl = Func.Zhuru(Request["sl"]);
            string qc = Func.Zhuru(Request["qc"]);
            string scph = Func.Zhuru(Request["scph"]);
            string bz = Func.Zhuru(Request["bz"]);
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string sld = Func.Zhuru(Request["sld"]);
            string onecodemsg = gysdm + '-' + khlh + '-' + scrq + Getlsnum();
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbHaixinPrint(s_id,s_gysdm,s_cnbqywm,s_haixinType,s_khlh,s_wlms,s_sl,s_scrq,s_qc,s_scph,s_bz,s_onecodemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint,s_sld) ";
            sql += "values(NEWID(),'" + gysdm + "','" + cnbqywm + "','" + haixinType + "','" + khlh + "','" + wlms + "','" + sl + "','" + scrq + "','" + qc + "','" + scph + "','" + bz + "','" + onecodemsg + "'";
            sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "','" + sld + "') ";
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
                    onecodemsg = gysdm + '-' + khlh + '-' + scrq_32 + '-' + Getlsnum();
                    sql = "Insert Into tbHaixinPrint(s_id,s_gysdm,s_cnbqywm,s_haixinType,s_khlh,s_wlms,s_sl,s_scrq,s_qc,s_scph,s_bz,s_onecodemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint,s_sld) ";
                    sql += "values(NEWID(),'" + gysdm + "','" + cnbqywm + "','" + haixinType + "','" + khlh + "','" + wlms + "','" + sl + "','" + scrq + "','" + qc + "','" + scph + "','" + bz + "','" + onecodemsg + "'";
                    sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "','" + sld + "') ";
                    lsary = lsary + "," + Getlsnum();
                    code = DBHelper.excuteNoQuery(sql);
                    if (code == -1)
                    {
                        code = 0;
                    }
                    id += 1;
                }
                string data = code + "," + scrq_32  + lsary;
                return data;
            }
        }

        public string AddHaixinPrint_zx()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string haixinType = Func.Zhuru(Request["haixinType"]);
            string gysdm = Func.Zhuru(Request["gysdm"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string scrq = Func.Zhuru(Request["scrq"]);
            var arr = scrq.Split('/');
            string month_32 = Func.int10Convert36(Convert.ToInt32(arr[1]));
            string dat_32 = Func.int10Convert36(Convert.ToInt32(arr[2]));
            string nian_32 = scrq.Substring(2, 2);
            string scrq_32 = nian_32 + month_32 + dat_32;
            string sl = Func.Zhuru(Request["sl"]);
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string hbbs = Func.Zhuru(Request["hbbs"]);
            //string sld = Func.Zhuru(Request["sld"]);
            string onecodemsg = gysdm + '-' + khlh + '-' + scrq + Getlsnum_zx();
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbHaixinPrint(s_id,s_gysdm,s_cnbqywm,s_haixinType,s_khlh,s_sl,s_scrq,s_onecodemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint,s_hbbs) ";
            sql += "values(NEWID(),'" + gysdm + "','" + cnbqywm + "','" + haixinType + "','" + khlh + "','" + sl + "','" + scrq + "','" + onecodemsg + "'";
            sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "','" + hbbs + "') ";
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
                    onecodemsg = gysdm + '-' + khlh + '-' + scrq_32 + '-' + Getlsnum_zx();
                    sql = "Insert Into tbHaixinPrint(s_id,s_gysdm,s_cnbqywm,s_haixinType,s_khlh,s_sl,s_scrq,s_onecodemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint,s_hbbs) ";
                    sql += "values(NEWID(),'" + gysdm + "','" + cnbqywm + "','" + haixinType + "','" + khlh + "','" + sl + "','" + scrq + "','" + onecodemsg + "'";
                    sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "','" + hbbs + "') ";
                    lsary = lsary + "," + Getlsnum_zx();
                    code = DBHelper.excuteNoQuery(sql);
                    if (code == -1)
                    {
                        code = 0;
                    }
                    id += 1;
                }
                string data = code + "," + scrq_32 + lsary;
                return data;
            }
        }
        public string AddHaixinPrintALL()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string haixinType = Func.Zhuru(Request["haixinType"]);
            string gysdm = Func.Zhuru(Request["gysdm"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string wlms = Func.Zhuru(Request["wlms"]);
            string scrq = Func.Zhuru(Request["scrq"]);
            var arr = scrq.Split('/');
            string month_32 = Func.int10Convert36(Convert.ToInt32(arr[1])); 
            string dat_32 = Func.int10Convert36(Convert.ToInt32(arr[2]));
            string nian_32 = scrq.Substring(2, 2);
            string scrq_32 = nian_32 + month_32 + dat_32;
            string sl = Func.Zhuru(Request["sl"]);
            string qc = Func.Zhuru(Request["qc"]);
            string scph = Func.Zhuru(Request["scph"]);
            string bz = Func.Zhuru(Request["bz"]);
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string sld = Func.Zhuru(Request["sld"]);
            string onecodemsg = gysdm + '-' + khlh + '-' + scrq + Getlsnumall();
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbHaixinPrint(s_id,s_gysdm,s_cnbqywm,s_haixinType,s_khlh,s_wlms,s_sl,s_scrq,s_qc,s_scph,s_bz,s_onecodemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint,s_sld) ";
            sql += "values(NEWID(),'" + gysdm + "','" + cnbqywm + "','" + haixinType + "','" + khlh + "','" + wlms + "','" + sl + "','" + scrq + "','" + qc + "','" + scph + "','" + bz + "','" + onecodemsg + "'";
            sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "','" + sld + "') ";
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
                    onecodemsg = gysdm + '-' + khlh + '-' + scrq_32 + '-' + Getlsnumall();
                    sql = "Insert Into tbHaixinPrint(s_id,s_gysdm,s_cnbqywm,s_haixinType,s_khlh,s_wlms,s_sl,s_scrq,s_qc,s_scph,s_bz,s_onecodemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint,s_sld) ";
                    sql += "values(NEWID(),'" + gysdm + "','" + cnbqywm + "','" + haixinType + "','" + khlh + "','" + wlms + "','" + sl + "','" + scrq + "','" + qc + "','" + scph + "','" + bz + "','" + onecodemsg + "'";
                    sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "','" + sld + "') ";
                    lsary = lsary + "," + Getlsnumall();
                    code = DBHelper.excuteNoQuery(sql);
                    if (code == -1)
                    {
                        code = 0;
                    }
                    id += 1;
                }
                string data = code + "," + scrq_32  + lsary;
                return data;
            }
        }

        public string Getlsnum()
        {
            
            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbHaixinPrint where n_state = 0 and s_haixinType =0)  set @lsnum = '00001' ";
            sql += "else  if exists (select n_id from tbHaixinPrint where n_state = 0 and s_haixinType =0) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_onecodemsg,5) as lsnum from tbHaixinPrint where n_state=0 and s_haixinType =0 order by n_id DESC )  = '99999' then  '00001' ";
            sql += "else substring(convert(varchar,convert(int,'00001')+('1'+(select top 1 right(s_onecodemsg,5) as lsnum from tbHaixinPrint where n_state=0 and s_haixinType =0 order by n_id DESC ))),2,5) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        public string Getlsnum_zx()
        {

            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbHaixinPrint where n_state = 0 and s_haixinType =1)  set @lsnum = '00001' ";
            sql += "else  if exists (select n_id from tbHaixinPrint where n_state = 0 and s_haixinType =1) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_onecodemsg,5) as lsnum from tbHaixinPrint where n_state=0 and s_haixinType =1 order by n_id DESC )  = '99999' then  '00001' ";
            sql += "else substring(convert(varchar,convert(int,'00001')+('1'+(select top 1 right(s_onecodemsg,5) as lsnum from tbHaixinPrint where n_state=0 and s_haixinType =1 order by n_id DESC ))),2,5) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        public string Getlsnumall()
        {

            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbHaixinPrint where n_state = 0 and s_haixinType =2)  set @lsnum = '00001' ";
            sql += "else  if exists (select n_id from tbHaixinPrint where n_state = 0 and s_haixinType =2) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_onecodemsg,5) as lsnum from tbHaixinPrint where n_state=0 and s_haixinType = 2 order by n_id DESC )  = '99999' then  '00001' ";
            sql += "else substring(convert(varchar,convert(int,'00001')+('1'+(select top 1 right(s_onecodemsg,5) as lsnum from tbHaixinPrint where n_state=0 and s_haixinType = 2 order by n_id DESC ))),2,5) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        public int UpdateHaixinPrint_BD()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbHaixinPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
            code = DBHelper.excuteNoQuery(sql);
            return code;
        }
        public int UpdateHaixinPrint_BDZX()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbHaixinPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
            code = DBHelper.excuteNoQuery(sql);
            return code;
        }
    }
}

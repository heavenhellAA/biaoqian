﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class LianheController : Controller
    {
        //
        // GET: /Lianhe/

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

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbLianhePrint where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbLianhePrint where n_state = 0";
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
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbLianhePrint where n_state = 0 and s_packageType = 1)  set @lsnum = '000000000001' ";
            sql += "else  if exists (select n_id from tbLianhePrint where n_state = 0 and s_packageType = 1) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_lsh,12) as lsnum from tbLianhePrint where n_state=0 and s_packageType = 1 order by n_id DESC )  = '999999999999' then  '000000000001' ";
            sql += "else substring(cast(cast(('1'+(select top 1 right(s_lsh,12) as lsnum from tbLianhePrint where n_state=0 and s_packageType =1 order by n_id DESC )) as bigint) + 1 as varchar),2,12) end )";
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
                string sql = "update tbLianhePrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public string AddLianhePrint()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string packageType = Func.Zhuru(Request["packageType"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string scrq = Func.Zhuru(Request["scrq"]);
            string sl = Func.Zhuru(Request["sl"]);
            string bbh = Func.Zhuru(Request["bbh"]);
            string bcwlms = Func.Zhuru(Request["bcwlms"]);
            string yxq = Func.Zhuru(Request["yxq"]);
            string wlms = Func.Zhuru(Request["wlms"]);
            string smd = Func.Zhuru(Request["smd"]);
            string aslhbcsm = Func.Zhuru(Request["aslhbcsm"]);
            string gysbm = Func.Zhuru(Request["gysbm"]);
            string scph1 = Func.Zhuru(Request["scph1"]);
            string scph2 = Func.Zhuru(Request["scph2"]);
            string cgddbh = Func.Zhuru(Request["cgddbh"]);
            string tyd = Func.Zhuru(Request["tyd"]);
            string aslh = Func.Zhuru(Request["aslh"]);
            string onecode1msg = Func.Zhuru(Request["onecode1msg"]);
            string onecode2msg = Func.Zhuru(Request["onecode2msg"]);
            string gyswz = Func.Zhuru(Request["gyswz"]);
            string gys = Func.Zhuru(Request["gys"]);
            string gyssj = Func.Zhuru(Request["gyssj"]);
            string lsh = "S" + Getlsnum();
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string codemsg = "[)>@06@12S0002@P" + khlh + "@1P" + aslh + "@31P" + aslhbcsm + "@12V" + gysbm + "@10V" + gyswz + "@2P" + bbh +"@20P" +"NA"+ "@6D" +scrq+"@14D"+yxq+"@30P"+ 'Y'+"@Z"+smd+"@K"+cgddbh+"@16K"+tyd+"@V"+gysbm+"@3S"+lsh +"@Q"+sl + "@20T"+'1' + "@1T"+scph1+"@2T"+scph2+"@1Z"+gyssj+"@@";
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbLianhePrint(s_id,s_cnbqywm,s_packageType,s_khlh,s_scrq,s_sl,s_bbh,s_bcwlms,s_yxq,s_wlms,s_smd,s_aslhbcsm,s_gysdm,s_lsh,s_scph1,s_scph2,s_cgddbh,s_tyd,s_aslh,s_onecode1msg,s_onecode2msg,s_gyswz,s_gys,s_gyssj,s_codemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + packageType + "','" + khlh + "','" + scrq + "','" + sl + "','" + bbh + "','" + bcwlms + "','" + yxq + "','" + wlms + "','" + smd + "','" + aslhbcsm + "','" + gysbm + "','" + lsh + "','" + scph1 + "','" + scph2 + "','" + cgddbh + "','" + tyd + "','" + aslh + "','" + onecode1msg + "','" + onecode2msg + "','" + gyswz + "','" + gys + "','" + gyssj + "','" + codemsg + "'";
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
                    lsh = "S" + Getlsnum();
                    codemsg = "[)>@06@12S0002@P" + khlh + "@1P" + aslh + "@31P" + aslhbcsm + "@12V" + gysbm + "@10V" + gyswz + "@2P" + bbh + "@20P" + "NA" + "@6D" + scrq + "@14D" + yxq + "@30P" + 'Y' + "@Z" + smd + "@K" + cgddbh + "@16K" + tyd + "@V" + gysbm + "@3S" + lsh + "@Q" + sl + "@20T" + '1' + "@1T" + scph1 + "@2T" + scph2 + "@1Z" + gyssj + "@@";
                    sql = "Insert Into tbLianhePrint(s_id,s_cnbqywm,s_packageType,s_khlh,s_scrq,s_sl,s_bbh,s_bcwlms,s_yxq,s_wlms,s_smd,s_aslhbcsm,s_gysdm,s_lsh,s_scph1,s_scph2,s_cgddbh,s_tyd,s_aslh,s_onecode1msg,s_onecode2msg,s_gyswz,s_gys,s_gyssj,s_codemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + cnbqywm + "','" + packageType + "','" + khlh + "','" + scrq + "','" + sl + "','" + bbh + "','" + bcwlms + "','" + yxq + "','" + wlms + "','" + smd + "','" + aslhbcsm + "','" + gysbm + "','" + lsh + "','" + scph1 + "','" + scph2 + "','" + cgddbh + "','" + tyd + "','" + aslh + "','" + onecode1msg + "','" + onecode2msg + "','" + gyswz + "','" + gys + "','" + gyssj + "','" + codemsg + "'";
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

        public int UpdatetbLianhePrint_BD()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbLianhePrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
            code = DBHelper.excuteNoQuery(sql);
            return code;
        }
    }
}

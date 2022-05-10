using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class LiantaoController : Controller
    {
        //
        // GET: /Liantao/

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

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbLiantaoPrint where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbLiantaoPrint where n_state = 0";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows;
            //获取DS长度
            var total = datas.Count;
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
        //流水号获取
        public string Getlsnum()
        {

            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbLiantaoPrint where n_state = 0 and s_packageType =1)  set @lsnum = '0000000001' ";
            sql += "else  if exists (select n_id from tbLiantaoPrint where n_state = 0 and s_packageType =1) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_lsh,10) as lsnum from tbLiantaoPrint where n_state=0 and s_packageType =1 order by n_id DESC )  = '9999999999' then  '0000000001' ";
            sql += "else substring(cast(cast(('1'+(select top 1 right(s_lsh,10) as lsnum from tbLiantaoPrint where n_state=0 and s_packageType =1 order by n_id DESC )) as bigint) + 1 as varchar),2,10) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        //流水号获取
        public string GetlsnumZX()
        {

            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbLiantaoPrint where n_state = 0 and s_packageType =0)  set @lsnum = '000001' ";
            sql += "else  if exists (select n_id from tbLiantaoPrint where n_state = 0 and s_packageType =0) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_lsh,6) as lsnum from tbLiantaoPrint where n_state=0 and s_packageType =0 order by n_id DESC )  = '999999' then  '000001' ";
            sql += "else substring(convert(varchar,convert(int,'000001')+('1'+(select top 1 right(s_lsh,6) as lsnum from tbLiantaoPrint where n_state=0 and s_packageType =0 order by n_id DESC ))),2,6) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        public int UpdateLiantaoPrint_BD()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbLiantaoPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
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
                string sql = "update tbLiantaoPrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public string AddLiantaoPrint()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string packageType = Func.Zhuru(Request["packageType"]);
            string cpxm = Func.Zhuru(Request["cpxm"]);
            string scjd = Func.Zhuru(Request["scjd"]);
            string bbh = Func.Zhuru(Request["bbh"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string sl = Func.Zhuru(Request["sl"]);
            string scrq = Func.Zhuru(Request["scrq"]);
            string scph = Func.Zhuru(Request["scph"]);
            string ljms = Func.Zhuru(Request["ljms"]);
            string config = Func.Zhuru(Request["config"]);
            string smd = Func.Zhuru(Request["smd"]);
            string gysdm = Func.Zhuru(Request["gysdm"]);
            string gysmc = Func.Zhuru(Request["gysmc"]);
            string cbsl = Func.Zhuru(Request["cbsl"]);
            string binh = Func.Zhuru(Request["binh"]);
            string mxh = Func.Zhuru(Request["mxh"]);
            string ycd = Func.Zhuru(Request["ycd"]);
            string xh = Func.Zhuru(Request["xh"]);
            string pp = Func.Zhuru(Request["pp"]);
            string ejgys = Func.Zhuru(Request["ejgys"]);
            string lsh = Getlsnum();
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string codemsg = gysdm + lsh + '$' + gysdm + '$' + gysmc + '$' + khlh + '$' + config + '$' + scph + '$' + scrq + '$' + sl + '$' + mxh + '$' + binh + '$' + cbsl + '$' + ejgys + '$'+smd;
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbLiantaoPrint(s_id,s_cnbqywm,s_packageType,s_cpxm,s_scjd,s_bbh,s_khlh,s_sl,s_scrq,s_scph,s_ljms,s_config,s_smd,s_gysdm,s_gysmc,s_lsh,s_cbsl,s_binh,s_mxh,s_ycd,s_xh,s_pp,s_ejgys,s_wcodemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + packageType + "','" + cpxm + "','" + scjd + "','" + bbh + "','" + khlh + "','" + sl + "','" + scrq + "','" + scph + "','" + ljms + "','" + config + "','" + smd + "','" + gysdm + "','" + gysmc + "','" + lsh + "','" + cbsl + "','" + binh + "','" + mxh + "','" + ycd + "','" + xh + "','" + pp + "','" + ejgys + "','" + codemsg + "'";
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
                    codemsg = gysdm + lsh + '$' + gysdm + '$' + gysmc + '$' + khlh + '$' + config + '$' + scph + '$' + scrq + '$' + sl + '$' + mxh + '$' + binh + '$' + cbsl + '$' + ejgys + '$' + smd;
                    sql = "Insert Into tbLiantaoPrint(s_id,s_cnbqywm,s_packageType,s_cpxm,s_scjd,s_bbh,s_khlh,s_sl,s_scrq,s_scph,s_ljms,s_config,s_smd,s_gysdm,s_gysmc,s_lsh,s_cbsl,s_binh,s_mxh,s_ycd,s_xh,s_pp,s_ejgys,s_wcodemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + cnbqywm + "','" + packageType + "','" + cpxm + "','" + scjd + "','" + bbh + "','" + khlh + "','" + sl + "','" + scrq + "','" + scph + "','" + ljms + "','" + config + "','" + smd + "','" + gysdm + "','" + gysmc + "','" + lsh + "','" + cbsl + "','" + binh + "','" + mxh + "','" + ycd + "','" + xh + "','" + pp + "','" + ejgys + "','" + codemsg + "'";
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
        public string AddLiantaoPrintZX()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string packageType = Func.Zhuru(Request["packageType"]);
            string vendor = Func.Zhuru(Request["vendor"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string sl = Func.Zhuru(Request["sl"]);
            string scrq = Func.Zhuru(Request["scrq"]);
            string gysdm = Func.Zhuru(Request["gysdm"]);
            string ddh = Func.Zhuru(Request["ddh"]);
            string scph = Func.Zhuru(Request["scph"]);
            string lsh2 = Func.Zhuru(Request["lsh2"]);
            string msd = Func.Zhuru(Request["msd"]);
            string cblx = Func.Zhuru(Request["cblx"]);
            string cbsl = Func.Zhuru(Request["cbsl"]);
            string bing = Func.Zhuru(Request["bing"]);
            string mxh = Func.Zhuru(Request["mxh"]);
            string lsh = GetlsnumZX();
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string QRCodes1 = khlh + '|' + scrq + '|' + sl + '|' + gysdm + '|' + ddh + '|' + scph + '|' + khlh + lsh2 + lsh + '|' + mxh + '|' + bing + '|' + cblx + '|' + cbsl + '|' + msd;
            string QRCodes2 = khlh + lsh2 + lsh;
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbLiantaoPrint(s_id,s_cnbqywm,s_packageType,s_vendor,s_khlh,s_sl,s_scrq,s_gysdm,s_ddh,s_scph,s_lsh2,s_msd,s_cblx,s_cbsl,s_bing,s_mxh,s_lsh,s_zxcodemsg1,s_zxcodemsg2,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + packageType + "','" + vendor + "','" + khlh + "','" + sl + "','" + scrq + "','" + gysdm + "','" + ddh + "','" + scph + "','" + lsh2 + "','" + msd + "','" + cblx + "','" + cbsl + "','" + bing + "','" + mxh + "','" + lsh + "','" + QRCodes1 + "','" + QRCodes2 + "'";
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
                    QRCodes1 = khlh + '|' + scrq + '|' + sl + '|' + gysdm + '|' + ddh + '|' + scph + '|' + khlh + lsh2 + lsh + '|' + mxh + '|' + bing + '|' + cblx + '|' + cbsl + '|' + msd;
                    QRCodes2 = khlh + lsh2 + lsh;
                    sql = "Insert Into tbLiantaoPrint(s_id,s_cnbqywm,s_packageType,s_vendor,s_khlh,s_sl,s_scrq,s_gysdm,s_ddh,s_scph,s_lsh2,s_msd,s_cblx,s_cbsl,s_bing,s_mxh,s_lsh,s_zxcodemsg1,s_zxcodemsg2,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + cnbqywm + "','" + packageType + "','" + vendor + "','" + khlh + "','" + sl + "','" + scrq + "','" + gysdm + "','" + ddh + "','" + scph + "','" + lsh2 + "','" + msd + "','" + cblx + "','" + cbsl + "','" + bing + "','" + mxh + "','" + lsh + "','" + QRCodes1 + "','" + QRCodes2 + "'";
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

    }
}

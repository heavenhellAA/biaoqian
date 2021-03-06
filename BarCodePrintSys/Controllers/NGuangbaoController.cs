using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class NGuangbaoController : Controller
    {
        //
        // GET: /NGuangbao/

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

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbNGuangbaoPrint where n_state = 0 ) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbNGuangbaoPrint where n_state = 0";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows;
            //获取DS长度
            var total = datas.Count;
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
        //流水号获取 三位26进制PPP转化十进制为17575
        public string Getlsnum(string scrq)
        {

            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbNGuangbaoPrint where n_state = 0 and s_scrq='" + scrq + "' )  set @lsnum = '00001' ";
            sql += "else  if exists (select n_id from tbNGuangbaoPrint where n_state = 0 and s_scrq='" + scrq + "' ) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_lsh,3) as lsnum from tbNGuangbaoPrint where n_state=0 and s_scrq='" + scrq + "'  order by n_id DESC )  = '17575' then  '00001' ";
            sql += "else substring(convert(varchar,convert(int,'00001')+('1'+(select top 1 right(s_lsh,5) as lsnum from tbNGuangbaoPrint where n_state=0 and s_scrq='" + scrq + "'  order by n_id DESC ))),2,5) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        public string AddNGuangbaoPrint()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string okhlh = Func.Zhuru(Request["okhlh"]);
            string gysdm = Func.Zhuru(Request["gysdm"]);
            string scrq = Func.Zhuru(Request["scrq"]);
            string sl = Func.Zhuru(Request["sl"]);
            string bb = Func.Zhuru(Request["bb"]);
            string mx = Func.Zhuru(Request["mx"]);
            string lsh = Getlsnum(scrq);
            string lsh2 = Func.createNum(int.Parse(lsh), 26, 3);
            string scph = Func.Zhuru(Request["scph"]);
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string onecodemsg = khlh + gysdm + scrq + sl + lsh2;
            string codemsg = "P" + khlh + ";V" + gysdm + ";D" + scrq + ";Q" + int.Parse(sl) + ";S" + onecodemsg + ";R" + bb + ";C" + mx + ";L" + scph;
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbNGuangbaoPrint(s_id,s_cnbqywm,s_khlh,s_okhlh,s_gysdm,s_scrq,s_sl,s_lsh,s_lsnum,s_bb,s_mx,s_scph,s_onecodemsg,s_codemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + khlh + "','" + okhlh + "','" + gysdm + "','" + scrq + "','" + sl + "','" + lsh + "','" + lsh2 + "','" + bb + "','" + mx + "','" + scph + "','" + onecodemsg + "','" + codemsg + "'";
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
                    lsh2 = Func.createNum(int.Parse(lsh), 26, 3);
                    onecodemsg = khlh + gysdm + scrq + sl + lsh2;
                    codemsg = "P" + khlh + ";V" + gysdm + ";D" + scrq + ";Q" + int.Parse(sl) + ";S" + onecodemsg + ";R" + bb + ";C" + mx + ";L" + scph;
                    sql = "Insert Into tbNGuangbaoPrint(s_id,s_cnbqywm,s_khlh,s_okhlh,s_gysdm,s_scrq,s_sl,s_lsh,s_lsnum,s_bb,s_mx,s_scph,s_onecodemsg,s_codemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + cnbqywm + "','" + khlh + "','" + okhlh + "','" + gysdm + "','" + scrq + "','" + sl + "','" + lsh + "','" + lsh2 + "','" + bb + "','" + mx + "','" + scph + "','" + onecodemsg + "','" + codemsg + "'";
                    sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "') ";
                    lsary = lsary + "," + lsh2;
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
        public int UpdateNGuangbaoPrint_BD()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbNGuangbaoPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
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
                string sql = "update tbNGuangbaoPrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
    }
}

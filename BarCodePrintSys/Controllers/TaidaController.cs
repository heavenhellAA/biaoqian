using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class TaidaController : Controller
    {
        //
        // GET: /Taida/

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

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbTaidaPrint where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbTaidaPrint where n_state = 0";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows;
            //获取DS长度
            var total = datas.Count;
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
        //流水号获取
        public string Getlsnum(string scrq)
        {

            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbTaidaPrint where n_state = 0 and '" + scrq + "' = s_lsh1)  set @lsnum = '000001' ";
            sql += "else  if exists (select n_id from tbTaidaPrint where n_state = 0 and '" + scrq + "' = s_lsh1) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_lsh,6) as lsnum from tbTaidaPrint where n_state=0 and '" + scrq + "' = s_lsh1 order by n_id DESC )  = '999999' then  '000001' ";
            sql += "else substring(convert(varchar,convert(int,'000001')+('1'+(select top 1 right(s_lsh,6) as lsnum from tbTaidaPrint where n_state=0 and '" + scrq + "' = s_lsh1 order by n_id DESC ))),2,6) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        public string AddTaidaPrint()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string packageType = Func.Zhuru(Request["packageType"]);
            string cb = Func.Zhuru(Request["cb"]);
            string gysdm = Func.Zhuru(Request["gysdm"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string sl = Func.Zhuru(Request["sl"]);
            string dw = Func.Zhuru(Request["dw"]);
            string lsh1 = Func.Zhuru(Request["lsh"]);
            string lsh = lsh1 + Getlsnum(lsh1);
            string zq = Func.Zhuru(Request["zq"]);
            string mydm = Func.Zhuru(Request["mydm"]);
            string scph = Func.Zhuru(Request["scph"]);
            string yxq = Func.Zhuru(Request["yxq"]);
            string ddh = Func.Zhuru(Request["ddh"]);
            string fph = Func.Zhuru(Request["fph"]);
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string codemsg = khlh + " {" + sl + " {" + dw + " {" + gysdm + " {" + zq + ' ' + ' ' + ' ' + ' ' + scph + " {" + mydm + " {" + ddh + " {" + fph + " {" + lsh;
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //下面的SQL是不完整的只是单纯检查下SQL注入。
            sql = "Insert Into tbTaidaPrint(s_id,s_cnbqywm,s_packageType,s_khlh,s_sl,s_dw,s_gysdm,s_zq,s_scph,s_mydm,s_ddh,s_fph,s_lsh1,s_lsh,s_cb,s_yxq,s_codemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + packageType + "','" + khlh + "','" + sl + "','" + dw + "','" + gysdm + "','" + zq + "','" + scph + "','" + mydm + "','" + ddh + "','" + fph + "','" + lsh1 + "','" + lsh + "','" + cb + "','" + yxq + "','" + codemsg + "'";
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
                    lsh = lsh1 + Getlsnum(lsh1);
                    codemsg = khlh + " {" + sl + " {" + dw + " {" + gysdm + " {" + zq + ' ' + ' ' + ' ' + ' ' + scph + " {" + mydm + " {" + ddh + " {" + fph + " {" + lsh;
                    sql = "Insert Into tbTaidaPrint(s_id,s_cnbqywm,s_packageType,s_khlh,s_sl,s_dw,s_gysdm,s_zq,s_scph,s_mydm,s_ddh,s_fph,s_lsh1,s_lsh,s_cb,s_yxq,s_codemsg,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + cnbqywm + "','" + packageType + "','" + khlh + "','" + sl + "','" + dw + "','" + gysdm + "','" + zq + "','" + scph + "','" + mydm + "','" + ddh + "','" + fph + "','" + lsh1 + "','" + lsh + "','" + cb + "','" + yxq + "','" + codemsg + "'";
                    sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "') ";
                    lsary = lsary + "," + Getlsnum(lsh1);
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
        public int UpdateTaidaPrint()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbTaidaPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
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
                string sql = "update tbTaidaPrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
    }
}

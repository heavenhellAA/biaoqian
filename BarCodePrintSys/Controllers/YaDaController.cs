using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class YaDaController : Controller
    {
        //
        // GET: /YaDa/

        public ActionResult Index()
        {
            return View();
        }
        public string GetGroup(string userid)
        {
            string data = DBHelper.getuserGroup(userid);
            return data;
        }
        public string searchprint()
        {
            string sql;
            var page = Request["page"];
            var limit = Request["limit"];
            int page_sql = Convert.ToInt32(page);
            int limit_sql = Convert.ToInt32(limit);

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbYaDaPrint where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbYaDaPrint where n_state = 0";
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
                string sql = "update tbYaDaPrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        //内箱最小包装流水号获取
        public string Getlsnum()
        {

            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbYaDaPrint where n_state = 0 and s_packageType =0)  set @lsnum = '00001' ";
            sql += "else  if exists (select n_id from tbYaDaPrint where n_state = 0 and s_packageType =0) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_clidbm,5) as lsnum from tbYaDaPrint where n_state=0 and s_packageType =0 order by n_id DESC )  = '99999' then  '00001' ";
            sql += "else substring(convert(varchar,convert(int,'00001')+('1'+(select top 1 right(s_clidbm,5) as lsnum from tbYaDaPrint where n_state=0 and s_packageType =0 order by n_id DESC ))),2,5) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        //外箱流水号获取
        public string Getlsnum_w()
        {

            string sql;
            sql = " declare @lsnum nvarchar(20)   if not exists (select n_id from tbYaDaPrint where n_state = 0 and s_packageType =1)  set @lsnum = 'W0001' ";
            sql += "else  if exists (select n_id from tbYaDaPrint where n_state = 0 and s_packageType =1) set @lsnum = ( ";
            sql += "case when  (select top 1 right(s_clidbm,5) as lsnum from tbYaDaPrint where n_state=0 and s_packageType =1 order by n_id DESC )  = 'W9999' then  'W0001' ";
            sql += "else 'W' + substring(convert(varchar,convert(int,'0001')+('1'+(select top 1 right(s_clidbm,4) as lsnum from tbYaDaPrint where n_state=0 and s_packageType =1 order by n_id DESC ))),2,4) end )";
            sql += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        public string AddYaDaPrint()
        {
            var code = 0;
            string lsnum = "";
            string sql;
            string sql1;
            string lsary = "";
            int id = 1;
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string packageType = Func.Zhuru(Request["packageType"]);
            string zzdm = Func.Zhuru(Request["zzdm"]);
            string clidbm_d = Func.Zhuru(Request["clidbm"]);
            string khlh = Func.Zhuru(Request["khlh"]);
            string pch = Func.Zhuru(Request["pch"]);
            string sl = Func.Zhuru(Request["sl"]);
            string yxrq = Func.Zhuru(Request["yxrq"]);
            string wslh = Func.Zhuru(Request["wslh"]);
            string zq = Func.Zhuru(Request["zq"]);
            string ylbd = Func.Zhuru(Request["ylbd"]);
            string clidbm = clidbm_d + Getlsnum();
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            sql = "declare @count varchar(100)   set @count=1 While(convert(int,@count) <= '" + num_print + "') begin Insert Into tbYaDaPrint(s_id,s_cnbqywm,s_zzdm,s_packageType,s_clidbm,s_khlh,s_pch,s_sl,s_yxrq,s_wslh,s_zq,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + zzdm + "','" + packageType + "','" + clidbm + "','" + khlh + "','" + pch + "','" + sl + "','" + yxrq + "','" + wslh + "','" + zq + "'";
            sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "',@count+'/" + num_print + "','" + cnbqywm + "',0,'" + ylbd + "') set @count= @count + 1 end ";
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
                    clidbm = clidbm_d + Getlsnum();
                    sql = "Insert Into tbYaDaPrint(s_id,s_cnbqywm,s_zzdm,s_packageType,s_clidbm,s_khlh,s_pch,s_sl,s_yxrq,s_wslh,s_zq,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + cnbqywm + "','" + zzdm + "','" + packageType + "','" + clidbm + "','" + khlh + "','" + pch + "','" + sl + "','" + yxrq + "','" + wslh + "','" + zq + "'";
                    sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "') ";
                    lsary = lsary + "," + Getlsnum();
                    code = DBHelper.excuteNoQuery(sql);
                    if (code == -1)
                    {
                        code = 0;
                    }
                    id += 1;
                }
                string lsnum_w = Getlsnum_w();
                clidbm = clidbm_d + Getlsnum_w();
                int sl_w = int.Parse(sl) * num_print;
                sql1 = "Insert Into tbYaDaPrint(s_id,s_cnbqywm,s_zzdm,s_packageType,s_clidbm,s_khlh,s_pch,s_sl,s_yxrq,s_wslh,s_zq,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                sql1 += "values(NEWID(),'" + cnbqywm + "','" + zzdm + "',1,'" + clidbm + "','" + khlh + "','" + pch + "','" + sl_w + "','" + yxrq + "','" + wslh + "','" + zq + "'";
                sql1 += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','1/1',0,'" + ylbd + "') ";
                var code1 = DBHelper.excuteNoQuery(sql1);
                string data = code + "," + lsnum_w + lsary;
                return data;
            }
        }

        public int UpdateYaDaPrint_BD()
        {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbYaDaPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
            code = DBHelper.excuteNoQuery(sql);
            return code;
        }

    }
}

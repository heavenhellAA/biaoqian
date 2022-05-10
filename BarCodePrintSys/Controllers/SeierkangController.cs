using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BarCodePrintSys.Controllers
{
    [MemberValidation]
    public class SeierkangController : Controller
    {
        /// <summary>
        /// //连接SAP ZPP036合格证数据维护 ZPPIP009 查询合格证打印AISHI料号数据 接口
        /// </summary>

        //private RFCConfig rfc = new RFCConfig()
        //{
        //    //配置
        //    Name = "艾华系统-router",
        //    AppServerHost = "10.7.7.2",
        //    SystemID = "S4Q",
        //    SystemNumber = "00",
        //    Client = "600",
        //    User = "AH-ABAP03",
        //    Password = "1234567abc",
        //    Language = "ZH"
        //};
        public string SAPBaojiaData(RFCConfig rfc)
        {
            #region 配置
            //输出参数
            List<List<string>> outputParameter;
            //输入参数
            Dictionary<string, object> inputParameter;

            RfcDestination dest;
            RfcRepository rfcrep;
            IRfcFunction myfun = null;
            IRfcStructure import = null;
            IRfcTable mytable = null;
            #endregion

            outputParameter = new List<List<string>>();
            inputParameter = new Dictionary<string, object>();
            inputParameter.Add("KDMAT", Request["wlbh"]); //物料编号 

            //RFC方法
            rfc.RFCCreateFunction = new RFCCreateFunction()
            {
                OutPutList = outputParameter,
                InPutList = inputParameter,
                FunctionName = "ZPPIP009",  //函数名
                TableNameList = new List<string>() { "EV_MSGTY", "EV_MESSAGE" }
            };
            dest = rfc.ConfigurationFunction();
            rfcrep = dest.Repository;

            myfun = rfcrep.CreateFunction("ZPPIP009");
            myfun.SetValue("IV_KDMAT", Request["wlbh"]);//SAP里面的传入参数
            myfun.Invoke(dest);
            mytable = myfun.GetTable("ET_OUTPUT");
             //提前实例化一个空的表结构出来
             DataTable dt = new DataTable();
             dt.Columns.Add("ZZMATNR"); //承认书料号
             dt.Columns.Add("KUNNR");  //客户编号
             dt.Columns.Add("MATNR");  //物料编号
             dt.Columns.Add("KDMAT");  //客户物料编号
             dt.Columns.Add("BISMT");  //SAP规格码
             //循环把IRfcTable里面的数据放入Table里面，因为类型不同，不可直接使用。
             for (int i = 0; i < mytable.Count; i++)
             {
                 mytable.CurrentIndex = i;
                 DataRow dr = dt.NewRow();
                 dr["ZZMATNR"] = mytable.GetString("ZZMATNR");
                 dr["KUNNR"] = mytable.GetString("KUNNR");
                 dr["MATNR"] = mytable.GetString("MATNR");
                 dr["KDMAT"] = mytable.GetString("KDMAT");
                 dr["BISMT"] = mytable.GetString("BISMT");
                 dt.Rows.Add(dr);
             }
             return DBHelper.DataTableToJson(dt);
            ////打开RFC方法 （RFC方法由客户SAP维护人员提供，每个方法都有自己的功能，比如读数据，写数据，这里的这个RFC是典型的读数据RFC）
            //myfun = rfcrep.CreateFunction(rfc.RFCCreateFunction.FunctionName);

            //import = rfcrep.GetStructureMetadata("ZSPPIP009").CreateStructure(); //结构
            //mytable = rfcrep.GetStructureMetadata("ZSPPIP009").CreateTable(); //表
            ////给方法传入参数 （执行RFC需事先知道这个RFC的结构，传入什么，传出什么，需要客户事先沟通知会，否则我们无法开发）
            //if (rfc.RFCCreateFunction.InPutList != null && rfc.RFCCreateFunction.InPutList.Count > 0)
            //{
            //    foreach (string key in rfc.RFCCreateFunction.InPutList.Keys)
            //    {
            //        import.SetValue(key, rfc.RFCCreateFunction.InPutList[key]);
            //    }
            //}
            //mytable.Insert(import);
            //myfun.SetValue("ET_OUTPUT", mytable); //输出表
            ////执行方法 这个时候SAP系统会把参数传过去并且返回的东西装到myfun里面
            //myfun.Invoke(dest);

            ////返回值
            //string message = myfun.GetValue("IV_ZZMATNR").ToString();
            //return message;

        }
        //
        // GET: /Seierkang/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DataSet()
        {
            return View();
        }
        public string AddSererkangPrint()
        {
            //result.OutPut.SerialNnumber
            int code= 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            string sql2;
            string gruop;
            int id = 1;
                        if (GetGroup(Request["bcp_userInfo"]) == "A99900A0-879B-4BA6-97FE-ED7B4E93B8F7") {
                            gruop = "110";
                        }
                        else if (GetGroup(Request["bcp_userInfo"]) == "2ABE14FC-4E9F-4039-A868-E9FDAC4D7EEE")
                        {
                            gruop = "220";
                        }
                        else if (GetGroup(Request["bcp_userInfo"]) == "F61D8679-D34C-4ABE-A7A3-258BC5A914D9") {
                            gruop = "220";
                        }
                        else if (GetGroup(Request["bcp_userInfo"]) == "27144168-6010-43D8-A70A-3F7F328BFC20")
                        {
                            gruop = "330";
                        }
                        else if (GetGroup(Request["bcp_userInfo"]) == "C3933AE1-0148-4AD1-BE79-AA2C67BBEDE0")
                        {
                            gruop = "550";
                        }
                        else
                        {
                            gruop = "660";
                        }
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string  packtype = Func.Zhuru(Request["packtype"]);
            string  cgdd = Func.Zhuru(Request["cgdd"]);
            string  wlbh = Func.Zhuru(Request["wlbh"]);
            string  pm = Func.Zhuru(Request["pm"]);
            string  scrq = Func.Zhuru(Request["scrq"]);
            string  yxq = Func.Zhuru(Request["yxq"]);
            string khpch4 = Func.Zhuru(Request["khpch4"]);
            string khpch = Func.Zhuru(Request["khpch"]) + gruop + khpch4;
            string gyspch = Func.Zhuru(Request["gyspch"]);
            string  slmax = Func.Zhuru(Request["slmax"]);
            string  slmin = Func.Zhuru(Request["slmin"]);
            string  gyslh = Func.Zhuru(Request["gyslh"]);
            string ylbd = Func.Zhuru(Request["ylbd"]);
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //sql = "declare @count varchar(100)    set @count=1 While(convert(int,@count) <= '" + num_print + "') begin Insert Into tbSeierkangPrint(s_id,s_packageType,s_cgdd,s_wlbh,s_pm,s_scrq,s_yxq,s_khpch,n_slmax,n_slmin,s_gyslh,s_gyspch,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            //sql += "values(NEWID(),'" + packtype + "','" + cgdd + "','" + wlbh + "','" + pm + "','" + scrq + "','" + yxq + "','" + khpch  + "','" + slmax + "','" + slmin + "','" + gyslh + "','" + gyspch + "'";
            //sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "',@count+'/" + num_print + "',0,0 ) set @count= @count + 1 end ";
            sql = "Insert Into tbSeierkangPrint(s_id,s_cnbqywm,s_packageType,s_cgdd,s_wlbh,s_pm,s_scrq,s_yxq,s_khpch,n_slmax,n_slmin,s_gyslh,s_gyspch,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + packtype + "','" + cgdd + "','" + wlbh + "','" + pm + "','" + scrq + "','" + yxq + "','" + khpch + "','" + slmax + "','" + slmin + "','" + gyslh + "','" + gyspch + "'";
            sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "' ) ";
            //sql2 = "select 'SerialNnumber' = (select s_liushuihao from tbSeierkangLsnum where n_id=(select n_id from tbSeierkangLsnum where s_liushuihao= (select top 1 SUBSTRING(s_khpch,11,3) as lsnum from tbSeierkangPrint where n_state=0  and s_packageType=0 order by n_id DESC ))+1 )";
            var warndata = sql.IndexOf("warning");
            if (warndata != -1)
            {
                code = -1;
                lsnum = "warning";
                string data = code + "," + lsnum;
                return data;
            }
            else
            {
                //lsnum = DBHelper.DatasetToJson((DBHelper.getDateSet(sql2)));
                while (id <= num_print) {
                    khpch = Func.Zhuru(Request["khpch"]) + gruop + khpch4;
                    sql = "Insert Into tbSeierkangPrint(s_id,s_cnbqywm,s_packageType,s_cgdd,s_wlbh,s_pm,s_scrq,s_yxq,s_khpch,n_slmax,n_slmin,s_gyslh,s_gyspch,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + cnbqywm + "','" + packtype + "','" + cgdd + "','" + wlbh + "','" + pm + "','" + scrq + "','" + yxq + "','" + khpch + "','" + slmax + "','" + slmin + "','" + gyslh + "','" + gyspch + "'";
                    sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "'  ) ";
                    //lsary = lsary + "," + Getlsnum();
                    code = DBHelper.excuteNoQuery(sql);
                    id += 1;
                }
                string data = code + "," + gruop ;
                return data;
            }
        }
        public string AddSererkangPrint_N()
        {
            var code = 0;
            string lsnum = "";
            string lsary = "";
            string sql;
            string sql2;
            string gruop;
            int id = 1;
            if (GetGroup(Request["bcp_userInfo"]) == "A99900A0-879B-4BA6-97FE-ED7B4E93B8F7")
            {
                gruop = "110";
            }
            else if (GetGroup(Request["bcp_userInfo"]) == "2ABE14FC-4E9F-4039-A868-E9FDAC4D7EEE")
            {
                gruop = "220";
            }
            else if (GetGroup(Request["bcp_userInfo"]) == "F61D8679-D34C-4ABE-A7A3-258BC5A914D9")
            {
                gruop = "220";
            }
            else if (GetGroup(Request["bcp_userInfo"]) == "27144168-6010-43D8-A70A-3F7F328BFC20")
            {
                gruop = "330";
            }
            else if (GetGroup(Request["bcp_userInfo"]) == "C3933AE1-0148-4AD1-BE79-AA2C67BBEDE0")
            {
                gruop = "550";
            }
            else
            {
                gruop = "660";
            }
            string cnbqywm = Func.Zhuru(Request["cnbqywm"]);
            string packtype = Func.Zhuru(Request["packtype"]);
            string wlbh = Func.Zhuru(Request["wlbh"]);
            string pm = Func.Zhuru(Request["pm"]);
            string khpch = Func.Zhuru(Request["khpch"]) + gruop + Getlsnum(wlbh);
            string gyspch = Func.Zhuru(Request["gyspch"]);
            string slmax = Func.Zhuru(Request["slmax"]);
            string gyslh = Func.Zhuru(Request["gyslh"]);
            string ylbd = Func.Zhuru(Request["ylbd"]);
            int num_print = int.Parse(Func.Zhuru(Request["num_print"]));
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string groupid = DBHelper.getuserGroup(creatorid);
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            //sql = "declare @count varchar(100)    set @count=1 While(convert(int,@count) <= '" + num_print + "') begin Insert Into tbSeierkangPrint(s_id,s_packageType,s_wlbh,s_pm,s_khpch,n_slmax,s_gyslh,s_gyspch,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            //sql += "values(NEWID(),'" + packtype + "','" + wlbh + "','" + pm + "','" + khpch + "','" + slmax + "','" + gyslh + "','" + gyspch + "'";
            //sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "',@count+'/" + num_print + "',0,0 ) set @count= @count + 1 end";
            sql = "Insert Into tbSeierkangPrint(s_id,s_cnbqywm,s_packageType,s_wlbh,s_pm,s_khpch,n_slmax,s_gyslh,s_gyspch,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
            sql += "values(NEWID(),'" + cnbqywm + "','" + packtype + "','" + wlbh + "','" + pm + "','" + khpch + "','" + slmax + "','" + gyslh + "','" + gyspch + "'";
            sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "' )";
            sql2 = "select 'SerialNnumber' = (select s_liushuihao from tbSeierkangLsnum where n_id=(select n_id from tbSeierkangLsnum where s_liushuihao= (select top 1 SUBSTRING(s_khpch,11,3) as lsnum from tbSeierkangPrint where n_state=0  and s_packageType=0 order by n_id DESC ))+1 )";
            var warndata = sql.IndexOf("warning");
            if (warndata != -1)
            {
                code = -1;
                string data = code + "," + lsnum;
                return data;
            }
            else
            {
                lsnum = DBHelper.DatasetToJson((DBHelper.getDateSet(sql2)));
                while (id <= num_print)
                {
                    khpch = Func.Zhuru(Request["khpch"]) + gruop + Getlsnum(wlbh);
                    sql = "Insert Into tbSeierkangPrint(s_id,s_cnbqywm,s_packageType,s_wlbh,s_pm,s_khpch,n_slmax,s_gyslh,s_gyspch,s_creator,s_createtime,s_Groupid,s_Roleid,s_waternum,n_state,n_bdprint) ";
                    sql += "values(NEWID(),'" + cnbqywm + "','" + packtype + "','" + wlbh + "','" + pm + "','" + khpch + "','" + slmax + "','" + gyslh + "','" + gyspch + "'";
                    sql += ",'" + creatorid + "','" + nowtime + "','" + groupid + "','" + roleid + "','" + id + "'+'/" + num_print + "',0,'" + ylbd + "' )";
                    lsary = lsary + "," + Getlsnum(wlbh);
                    code = DBHelper.excuteNoQuery(sql);
                    id += 1;
                }
                string data = code + "," + gruop + "," + lsary;
                return data;


            }
        }
        public string searchprint()
        {
            string sql;
            var page = Request["page"];
            var limit = Request["limit"];
            int page_sql = Convert.ToInt32(page);
            int limit_sql = Convert.ToInt32(limit);

            sql = "select tu.s_UserName,tg.s_GroupName,tr.s_RoleName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbSeierkangPrint where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbGroup tg  on  tg.s_GroupID=a.s_Groupid left join tbRole tr  on  tr.s_RoleID=a.s_Roleid where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbSeierkangPrint where n_state = 0";
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
                string sql = "update tbSeierkangPrint set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public int UpdateSererkangPrint_BD() {
            int code = 0;
            string sql;
            string id = Request["id"];
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbSeierkangPrint set n_bdprint = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id = '" + id + "'";
            code = DBHelper.excuteNoQuery(sql);
            return code;
        }
        public string searchdata() {
            string sql;
            var page = Request["page"];
            var limit = Request["limit"];
            int page_sql = Convert.ToInt32(page);
            int limit_sql = Convert.ToInt32(limit);
            sql = "select tu.s_UserName,updatorname=ts.s_UserName,a.* from (select *,row_number() over (order by n_id DESC) as id from tbSeierkangData where n_state = 0) a left join tbuser tu  on  tu.s_UserID=a.s_creator left join tbuser ts  on ts.s_UserID =a.s_updator  where id between ('" + limit_sql + "'*('" + page_sql + "'-1)+1) and '" + limit_sql + "'*('" + page_sql + "'-1)+'" + limit_sql + "' order by id  ";
            sql += "select COUNT(n_id)  as zongshu from tbSeierkangData where n_state = 0";
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
                string sql = "update tbSeierkangData set n_state = 1 ,s_updator='" + UserID + "',s_updatetime='" + nowtime + "' where n_id in (" + delstr + ")";
                res = DBHelper.excuteNoQuery(sql);
            }
            return res;
        }
        public int AddSererkangData()
        {
            var code = 0;
            string sql;
            string wlbh = Func.Zhuru(Request["d_wlbh"]);
            string pm = Func.Zhuru(Request["d_pm"]);
            string slmax = Func.Zhuru(Request["d_slmax"]);
            string slmin = Func.Zhuru(Request["d_slmin"]);
            string gyslh = Func.Zhuru(Request["d_gyslh"]);
            string creatorid = Server.UrlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string roleid = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["RoleID"].ToString());
            sql = "If Not EXISTS(select * from tbSeierkangData where s_wlbh='"+wlbh+"' and n_state = '0') Insert Into tbSeierkangData(s_id,s_wlbh,s_pm,n_slmax,n_slmin,s_gyslh,s_creator,s_createtime,n_state) ";
            sql += "values(NEWID(),'" + wlbh + "','" + pm + "','" + slmax + "','" + slmin + "','" + gyslh + "','" + creatorid + "','" + nowtime + "',0 )";
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
        public int AddSererkangDataE()
        {
            var code = 0;
            string sql;
            string id = Func.Zhuru(Request["d_id_e"]);
            string wlbh = Func.Zhuru(Request["d_wlbh_e"]);
            string pm = Func.Zhuru(Request["d_pm_e"]);
            string slmax = Func.Zhuru(Request["d_slmax_e"]);
            string slmin = Func.Zhuru(Request["d_slmin_e"]);
            string gyslh = Func.Zhuru(Request["d_gyslh_e"]);
            string UserID = Server.HtmlDecode(Request.Cookies["bcp_userInfo"]["UserID"].ToString());
            string nowtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = "update tbSeierkangData set s_wlbh='" + wlbh + "',s_pm='" + pm + "',n_slmax='" + slmax + "',n_slmin='" + slmin + "',s_gyslh='" + gyslh + "',s_updator='" + UserID + "',s_updatetime='" + nowtime + "'";
            sql += "where n_id ='"+id+"'";
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
        public string GetGroup(string userid) {
            string data = DBHelper.getuserGroup(userid);
            return data;
        }
        public string Getlsnum(string wlbh)
        {
            string sql;
            sql = "if not exists (select n_id from tbSeierkangPrint where n_state = 0 and s_packageType = 0 and '" + wlbh + "' = s_wlbh)  select s_liushuihao from tbSeierkangLsnum where n_id =1 ";
            sql += "else  if exists (select n_id from tbSeierkangPrint where n_state = 0 and s_packageType = 0 and '" + wlbh + "' = s_wlbh) select s_liushuihao from tbSeierkangLsnum where n_id =  ";
            sql += "case when  (select n_id from tbSeierkangLsnum  where s_liushuihao= (select top 1 right(s_khpch,3) as lsnum from tbSeierkangPrint where n_state=0 and s_packageType = 0 and '" + wlbh + "' = s_wlbh order by n_id DESC ))  = 11655 then  1 ";
            sql += "else (select n_id from tbSeierkangLsnum  where s_liushuihao= (select top 1 right(s_khpch,3) as lsnum from tbSeierkangPrint where n_state=0 and s_packageType = 0 and '" + wlbh + "' = s_wlbh order by n_id DESC )) + 1 end ";
            DataSet ds = DBHelper.getDateSet(sql);
            //将DataSet转化为DataTable,这里实际上是转list用但没用到
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }
        public string Getwlbh() {
            string sql;
            string wlbh = Request["data"]; 
            sql = "select s_wlbh from  tbSeierkangData where n_state = 0 and 1 = (case when ISNULL('" + wlbh + "','') = '' then 1 when s_wlbh like '%'+'" + wlbh + "'+'%' then 1 else 0 end)";
            DataSet ds = DBHelper.getDateSet(sql);
            string data = DBHelper.DatasetToJson(ds);
            return data;      
        }
        public string gtdata() {
            string sql;
            string wlbh = Request["wlbh"];  
            sql = "select * from tbSeierkangData where n_state = 0 and s_wlbh = '"+wlbh+"'";
            DataSet ds = DBHelper.getDateSet(sql);
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }

    }
}


using System;
using System.Data;

namespace BarCodePrintSys.APP_Code.Tools
{
    public class Tools
    {
        public string Liushui(int i , string tableName)
        {
            string lsh = "";
            string sqlSelect ;
            sqlSelect = " declare @lsnum nvarchar(20)   if not exists (select n_id from "+ tableName + " where n_state = 0 and s_packageType = 0)  set @lsnum = '00001' ";
            sqlSelect += "else  if exists (select n_id from " + tableName + " where n_state = 0 and s_packageType = 0) set @lsnum = ( ";
            sqlSelect += "case when  (select top 1 SUBSTRING(s_pkgid,12,5) as lsnum from " + tableName + " where n_state=0 and s_packageType = 0 order by n_id DESC )  = '99999' then  '00001' ";
            sqlSelect += "else substring(convert(varchar,convert(int,'0001')+('1'+(select top 1 right(s_pkgid,5) as lsnum from " + tableName + " where n_state=0 and s_packageType = 0 order by n_id DESC ))),2,5) end )";
            sqlSelect += "select convert(nvarchar,@lsnum)";
            DataSet ds = DBHelper.getDateSet(sqlSelect);
            return lsh;
        }
    }
}
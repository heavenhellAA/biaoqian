using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.Controllers
{
    public class DatainselfController : Controller
    {
        public string Getdata()
        {
            string[] sql = new string[6];
            string data = "";
            string scph = Func.Zhuru(Request["scph"]);
            var j = 1;
            sql[1] = "select * from yl_ws_hgz_big where   id+order_type+CONVERT(varchar,seq_id) = '" + scph + "' and dbo.get_number_beach( '" + scph + "')=id and dbo.get_number_seq( '" + scph + "') = seq_id ";
            sql[2] = "select * from yl_ws_hgz_midd where   id+order_type+CONVERT(varchar,seq_id) = '" + scph + "' and dbo.get_number_beach( '" + scph + "')=id and dbo.get_number_seq( '" + scph + "')= seq_id ";
            sql[3] = "select * from yl_ws_hgz_small where   id+order_type+CONVERT(varchar,seq_id) = '" + scph + "' and dbo.get_number_beach( '" + scph + "')=id and dbo.get_number_seq( '" + scph + "')= seq_id ";
            for (var i = 1; i <= 3; i++)
            {
                DataSet ds = DBHelper.getData51(sql[i]);
                //获取DS长度
                var datas = ds.Tables[0].Rows;
                var total = datas.Count;
                if (total > 0)
                {
                    data = DBHelper.DatasetToJson(ds);
                    break;
                }
                else
                {
                    data = DBHelper.DatasetToJson(ds);
                }
                j++;
            }
            if (j > 3 && j < 7)
            {
                for (var i = 1; i <= 3; i++)
                {
                    DataSet ds = DBHelper.getData52(sql[i]);
                    //获取DS长度
                    var datas = ds.Tables[0].Rows;
                    var total = datas.Count;
                    if (total > 0)
                    {
                        data = DBHelper.DatasetToJson(ds);
                        break;
                    }
                    else
                    {
                        data = DBHelper.DatasetToJson(ds);
                    }
                    j++;
                }
            } if (j > 6 && j < 10)
            {
                for (var i = 1; i <= 3; i++)
                {
                    DataSet ds = DBHelper.getData2D(sql[i]);
                    //获取DS长度
                    var datas = ds.Tables[0].Rows;
                    var total = datas.Count;
                    if (total > 0)
                    {
                        data = DBHelper.DatasetToJson(ds);
                        break;
                    }
                    else
                    {
                        data = DBHelper.DatasetToJson(ds);
                    }
                    j++;
                }
            }
            if (j > 9 && j < 13)
            {
                for (var i = 1; i <= 3; i++)
                {
                    DataSet ds = DBHelper.getData2(sql[i]);
                    //获取DS长度
                    var datas = ds.Tables[0].Rows;
                    var total = datas.Count;
                    if (total > 0)
                    {
                        data = DBHelper.DatasetToJson(ds);
                        break;
                    }
                    else
                    {
                        data = DBHelper.DatasetToJson(ds);
                    }
                    j++;
                }
            }
            if (j > 12 && j < 16)
            {
                for (var i = 1; i <= 3; i++)
                {
                    DataSet ds = DBHelper.getData1(sql[i]);
                    //获取DS长度
                    var datas = ds.Tables[0].Rows;
                    var total = datas.Count;
                    if (total > 0)
                    {
                        data = DBHelper.DatasetToJson(ds);
                        break;
                    }
                    else
                    {
                        data = DBHelper.DatasetToJson(ds);
                    }
                    j++;
                }
            }
            if (j > 15 && j < 19)
            {
                for (var i = 1; i <= 3; i++)
                {
                    DataSet ds = DBHelper.getData3(sql[i]);
                    //获取DS长度
                    var datas = ds.Tables[0].Rows;
                    var total = datas.Count;
                    if (total > 0)
                    {
                        data = DBHelper.DatasetToJson(ds);
                        break;
                    }
                    else
                    {
                        data = DBHelper.DatasetToJson(ds);
                    }
                    j++;
                }
            }
            if (j > 18 && j < 22)
            {
                for (var i = 1; i <= 3; i++)
                {
                    DataSet ds = DBHelper.getData6(sql[i]);
                    //获取DS长度
                    var datas = ds.Tables[0].Rows;
                    var total = datas.Count;
                    if (total > 0)
                    {
                        data = DBHelper.DatasetToJson(ds);
                        break;
                    }
                    else
                    {
                        data = DBHelper.DatasetToJson(ds);
                    }
                    j++;
                }
            }
            if (j >= 22) {
                string sqlstr = "exec p_mes_pack_lot_get_print_data '" + scph + "'";
                DataSet ds = DBHelper.getDataMES(sqlstr);
                ds.Tables[0].Columns.Add("goods_name",typeof(string));
                ds.Tables[0].Columns.Add("size_id", typeof(string)); 
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[0][66] = ds.Tables[0].Rows[0][8] + " " + ds.Tables[0].Rows[0][10] + "V " + ds.Tables[0].Rows[0][11] + "μF";
                    ds.Tables[0].Rows[0][67] = ds.Tables[0].Rows[0][12] + "*" + ds.Tables[0].Rows[0][13];
                }
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    if (dc.ColumnName == "客户订单号")
                    {
                        dc.ColumnName = "customer_order_id";//可以将列名改变
                    }
                    if (dc.ColumnName == "客户料号")
                    {
                        dc.ColumnName = "customer_goods_id";//可以将列名改变
                    }
                    if (dc.ColumnName == "合格证料号")
                    {
                        dc.ColumnName = "company_materials_id";//可以将列名改变
                    }
                    if (dc.ColumnName == "数量")
                    {
                        dc.ColumnName = "qty";//可以将列名改变
                    }
                    if (dc.ColumnName == "工单号")
                    {
                        dc.ColumnName = "number";//可以将列名改变
                    }
                    if (dc.ColumnName == "生产任务")
                    {
                        dc.ColumnName = "id";//可以将列名改变
                    }
                    if (dc.ColumnName == "周号")
                    {
                        dc.ColumnName = "dc";//可以将列名改变
                    }
                    if (dc.ColumnName == "生产日期")
                    {
                        dc.ColumnName = "datetime_source";//可以将列名改变
                    }
                }
                //获取DS长度
                var datas = ds.Tables[0].Rows;
                var total = datas.Count;
                if (total > 0)
                {
                    data = DBHelper.DatasetToJson(ds);
                }
                else
                {
                    data = DBHelper.DatasetToJson(ds);
                }
            }
            return data;
        }
        //周期码模糊查询
        public string GetZqdata() {
            string sql;
            string zqm = Func.Zhuru(Request["zqm"]);
            sql = "select s_zqm from tbSetZqData where n_state = 0 and 1 = (case when ISNULL('" + zqm + "','') = '' then 1 when s_zqm like '%'+'" + zqm + "'+'%' then 1 else 0 end )";
            DataSet ds = DBHelper.getDateSet(sql);
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
        //周期码带出生产日期和有效日期
        public string GetRqdata(){
            string sql;
            string zqm = Func.Zhuru(Request["zqm"]);
            sql = "select s_scrq,s_yxq from tbSetZqData where n_state = 0 and  s_zqm = '" + zqm + "'";
            DataSet ds = DBHelper.getDateSet(sql);
            string data = DBHelper.DatasetToJson(ds);
            return data;
        }
 

    }
}

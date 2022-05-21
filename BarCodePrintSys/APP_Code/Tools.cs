
using System;
using System.Data;

namespace BarCodePrintSys
{
   public class Tools
    {
        /// <summary>
        /// 流水号生成工具
        /// </summary>
        /// <param name="i">记录在当批次序列的位置</param>
        /// <param name="tableName">记录指向的表名称</param>
        /// <param name="num">当批次总数</param>
        /// <param name="columnName">序列字段名称</param>
        /// <returns></returns>
        public static string LiushuiTool(int i, string tableName, int num, string columnName)
        {
            //流水规则：3位 批次数量 3位 在批次中的序列流水号 5位在库中的序列号
            // 批次数量：本次打印的张数
            // 批次中的序列流水号：循环的位置
            string liushuihao;
            string picishuliang;
            if (num > 99999)
            {
                throw new Exception("超出长度！");
            }
            else
            {
                picishuliang = num.ToString().PadLeft(3, '0');
            }
            string picixuliehao;
            if (i > num)
            {
                throw new Exception("超出本批次数量极限！");
            }
            else
            {
                picixuliehao = i.ToString().PadLeft(3, '0');
            }
            string dangrizaikuxulie;

            //方案一：取当日时间段中最大的记录
            string sql = "select  top 1 ";
            sql += columnName + "from ";
            sql += tableName;
            sql += " where s_createtime  >= convert(varchar(10), Getdate(), 120)";
            sql += " and s_createtime<convert(varchar(10),dateadd(d, 1, Getdate()),120)";
            DataSet ds = DBHelper.getDateSet(sql);

            dangrizaikuxulie=ds.ToString();

            liushuihao = picishuliang + picixuliehao + dangrizaikuxulie;

            //====自动生成编号====//
            //日期格式化

            //取最新数据编号

            return liushuihao;
        }
    }
}
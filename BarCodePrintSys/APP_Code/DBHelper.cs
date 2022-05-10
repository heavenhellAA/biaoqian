using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace BarCodePrintSys
{
    public class DBHelper
    {

        /// <summary>
        ///SQL Server 通用的查询方法
        /// </summary>
        public static DataSet getDateSet(string sql)
        {
            //建立连接对象
            SqlConnection con = DB.Con();
            con.Open();
            try
            {
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                con.Close();
                //throw e;  //抛异常==，感觉暂时用不到
                string log = e.Message + sql;
                string logName = "错误信息日志文件";
                string path = System.Configuration.ConfigurationManager.AppSettings["Errornote_Save_URL"].ToString(); ;
                Func.save_emessage(log, logName, path);
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                return ds;
            }
        }
        /// <summary>
        ///SQL Server 通用的增删改
        /// </summary>
        public static int excuteNoQuery(string sql)
        {
            //建立连接对象
            SqlConnection con = DB.Con();
            con.Open();
            try
            {
                //创建命令对象
                SqlCommand cmd = new SqlCommand(sql, con);
                int count = cmd.ExecuteNonQuery();
                con.Close();
                return count;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                con.Close();
                //throw e;  //抛异常==，感觉暂时用不到
                string log = e.Message + sql;
                string logName = "错误信息日志文件";
                string path = System.Configuration.ConfigurationManager.AppSettings["Errornote_Save_URL"].ToString(); ;
                Func.save_emessage(log, logName, path);
                return 0;
            }
            //SqlConnection con = DB.Con();
            //con.Open();
            ////创建命令对象
            //SqlCommand cmd = new SqlCommand(sql, con);
            //int count = cmd.ExecuteNonQuery();
            //con.Close();
            //return count;

        }
        /// <summary>
        /// SQL Server 查询权限ID
        /// </summary>
        public static int getRoleNo(string RoleID)
        {
            int RoleNO = 1000;
            string sql = "select * from tbRole where s_RoleID='" + RoleID + "'";
            //创建连接数据库Connection对象
            SqlConnection con = DB.Con();
            //创建连接数据库SqlCommand对象，执行sql语句
            SqlCommand cmd = new SqlCommand(sql, con);
            //打开数据库
            con.Open();
            //读取数据集
            SqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                RoleNO = Convert.ToInt32(rd["n_RoleNO"]);
            }
            con.Close();
            return RoleNO;
        }
        /// <summary>
        /// SQL Server 查询权限NO
        /// </summary>
        public static string getRoleID(int RoleNO)
        {
            string RoleID = "";
            string sql = "select * from tbRole where n_RoleNO='" + RoleNO + "'";
            //创建连接数据库Connection对象
            SqlConnection con = DB.Con();
            //创建连接数据库SqlCommand对象，执行sql语句
            SqlCommand cmd = new SqlCommand(sql, con);
            //打开数据库
            con.Open();
            //读取数据集
            SqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                RoleID = rd["s_RoleID"].ToString();
            }
            con.Close();
            return RoleID;
        }
        /// <summary>
        /// SQL Server 查询权限组
        /// </summary>
        public static List<TRole> getRoles(int RoleNO)
        {
            string sql = "select * from tbRole where b_IsDeleted = 0 and n_RoleNO >=" + RoleNO + " order by n_Sort";
            DataSet ds = DBHelper.getDateSet(sql);
            var datas = ds.Tables[0].Rows;
            List<TRole> list = new List<TRole>();
            foreach (DataRow item in datas)
            {
                var act = new TRole()
                {
                    RoleNO = Convert.ToInt32(item["n_RoleNO"]),
                    RoleName = item["s_RoleName"].ToString(),
                };
                list.Add(act);
            }
            return list;
        }

        /// <summary>
        /// SQL Server 查询分组
        /// </summary>
        public static List<TGroup> getGroups()
        {
            string sql = "select * from tbGroup  where b_IsDeleted = 0 order by n_Sort";
            DataSet ds = DBHelper.getDateSet(sql);
            var datas = ds.Tables[0].Rows;
            List<TGroup> list = new List<TGroup>();
            foreach (DataRow item in datas)
            {
                var act = new TGroup()
                {
                    GroupID = item["s_GroupID"].ToString(),
                    GroupName = item["s_GroupName"].ToString(),
                };
                list.Add(act);
            }
            return list;
        }

        /// <summary>
        /// SQL Server 查询对应账户分厂
        /// </summary>
        public static string getuserGroup(string userid)
        {
            string sql = "select s_GroupID from tbUser  where b_IsDeleted = 0 and s_UserID ='" + userid + "'";
            DataSet ds = DBHelper.getDateSet(sql);
            var datas = ds.Tables[0].Rows[0][0];
            string data = datas.ToString();
            return data;
        }

        /// <summary>
        ///SQL Server 查询菜单
        /// </summary>
        public static List<TMenu> getMenus(int RoleNO)
        {
            string sql = "select * from tbMenu where b_IsDeleted = 0 and n_RoleNO >=" + RoleNO + " order by n_Sort";
            DataSet ds = DBHelper.getDateSet(sql);
            var datas = ds.Tables[0].Rows;
            List<TMenu> list = new List<TMenu>();
            foreach (DataRow item in datas)
            {
                var act = new TMenu()
                {
                    Node = Convert.ToInt32(item["n_Node"]),
                    MenuName = item["s_MenuName"].ToString(),
                };
                list.Add(act);
            }
           return list;
        }
        /// <summary>
        ///SQL Server 通用的查询方法 第二个数据库连接 普联数据 五厂一部
        /// </summary>
        public static DataSet getData51(string sql)
        {
            //建立连接对象
            SqlConnection con = DB.Con_51();
            con.Open();
            try
            {
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                con.Close();
                //throw e;  //抛异常==，感觉暂时用不到
                string log = e.Message + sql;
                string logName = "错误信息日志文件";
                string path = System.Configuration.ConfigurationManager.AppSettings["Errornote_Save_URL"].ToString(); ;
                Func.save_emessage(log, logName, path);
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                return ds;
            }
        }
        /// <summary>
        ///SQL Server 通用的查询方法 第三个数据库连接 普联数据 五厂二部
        /// </summary>
        public static DataSet getData52(string sql)
        {
            //建立连接对象
            SqlConnection con = DB.Con_52();
            con.Open();
            try
            {
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                con.Close();
                //throw e;  //抛异常==，感觉暂时用不到
                string log = e.Message + sql;
                string logName = "错误信息日志文件";
                string path = System.Configuration.ConfigurationManager.AppSettings["Errornote_Save_URL"].ToString(); ;
                Func.save_emessage(log, logName, path);
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                return ds;
            }
        }
        /// <summary>
        ///SQL Server 通用的查询方法 第四个数据库连接 普联数据 二厂大壳号
        /// </summary>
        public static DataSet getData2D(string sql)
        {
            //建立连接对象
            SqlConnection con = DB.Con_2D();
            con.Open();
            try
            {
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                con.Close();
                //throw e;  //抛异常==，感觉暂时用不到
                string log = e.Message + sql;
                string logName = "错误信息日志文件";
                string path = System.Configuration.ConfigurationManager.AppSettings["Errornote_Save_URL"].ToString(); ;
                Func.save_emessage(log, logName, path);
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                return ds;
            }
        }
        /// <summary>
        ///SQL Server 通用的查询方法 第五个数据库连接 普联数据 二厂小壳号
        /// </summary>
        public static DataSet getData2(string sql)
        {
            //建立连接对象
            SqlConnection con = DB.Con_2();
            con.Open();
            try
            {
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                con.Close();
                //throw e;  //抛异常==，感觉暂时用不到
                string log = e.Message + sql;
                string logName = "错误信息日志文件";
                string path = System.Configuration.ConfigurationManager.AppSettings["Errornote_Save_URL"].ToString(); ;
                Func.save_emessage(log, logName, path);
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                return ds;
            }
        }
        /// <summary>
        ///SQL Server 通用的查询方法 第六个数据库连接 普联数据 一厂
        /// </summary>
        public static DataSet getData1(string sql)
        {
            //建立连接对象
            SqlConnection con = DB.Con_1();
            con.Open();
            try
            {
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                con.Close();
                //throw e;  //抛异常==，感觉暂时用不到
                string log = e.Message + sql;
                string logName = "错误信息日志文件";
                string path = System.Configuration.ConfigurationManager.AppSettings["Errornote_Save_URL"].ToString(); ;
                Func.save_emessage(log, logName, path);
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                return ds;
            }
        }
        /// <summary>
        ///SQL Server 通用的查询方法 第七个数据库连接 普联数据 三厂
        /// </summary>
        public static DataSet getData3(string sql)
        {
            //建立连接对象
            SqlConnection con = DB.Con_3();
            con.Open();
            try
            {
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                con.Close();
                //throw e;  //抛异常==，感觉暂时用不到
                string log = e.Message + sql;
                string logName = "错误信息日志文件";
                string path = System.Configuration.ConfigurationManager.AppSettings["Errornote_Save_URL"].ToString(); ;
                Func.save_emessage(log, logName, path);
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                return ds;
            }
        }
        /// <summary>
        ///SQL Server 通用的查询方法 第八个数据库连接 数据 绵阳资江
        /// </summary>
        public static DataSet getData6(string sql)
        {
            //建立连接对象
            SqlConnection con = DB.Con_6();
            con.Open();
            try
            {
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                con.Close();
                //throw e;  //抛异常==，感觉暂时用不到
                string log = e.Message + sql;
                string logName = "错误信息日志文件";
                string path = System.Configuration.ConfigurationManager.AppSettings["Errornote_Save_URL"].ToString(); ;
                Func.save_emessage(log, logName, path);
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                return ds;
            }
        }
        /// <summary>
        ///SQL Server 通用的查询方法 第九个数据库连接 MES数据
        /// </summary>
        public static DataSet getDataMES(string sql)
        {
            //建立连接对象
            SqlConnection con = DB.MES();
            con.Open();
            try
            {
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                con.Close();
                return ds;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                con.Close();
                //throw e;  //抛异常==，感觉暂时用不到
                string log = e.Message + sql;
                string logName = "错误信息日志文件";
                string path = System.Configuration.ConfigurationManager.AppSettings["Errornote_Save_URL"].ToString(); ;
                Func.save_emessage(log, logName, path);
                DataSet ds = new DataSet();//此时没有数据
                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                //通过da把数据库中数据填充到ds中
                da.Fill(ds);
                return ds;
            }
        }
        /// <summary>
        /// dataset转Json
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static string DatasetToJson(System.Data.DataSet ds)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{\"Tables\":");
            json.Append("[");
            foreach (System.Data.DataTable dt in ds.Tables)
            {
                json.Append(DataTableToJson(dt));
                json.Append(",");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]");
            json.Append("}");
            return json.ToString();
        }
        /// <summary>
        /// table转json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            ///要是查询无数据，拼接方法有所改变
            if (dt.Rows.Count == 0)
            {
                jsonBuilder.Append("{\"Name\":\"" + dt.TableName + "\",\"Code\":\"" + "400" + "\",\"Count\":\"" + dt.Rows.Count + "\",\"Rows");
                jsonBuilder.Append("\":");
                jsonBuilder.Append(0);
                jsonBuilder.Append("},");
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            }
            else
            {
                jsonBuilder.Append("{\"Name\":\"" + dt.TableName + "\",\"Code\":\"" + "200" + "\",\"Count\":\"" + dt.Rows.Count + "\",\"Rows");
                jsonBuilder.Append("\":[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    jsonBuilder.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        jsonBuilder.Append("\"");
                        jsonBuilder.Append(dt.Columns[j].ColumnName);
                        jsonBuilder.Append("\":\"");
                        jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("\\", "\\\\").Replace("	 ", " "));//.Replace("\n", "").Replace("\t", " ").Replace("\r", "")
                        jsonBuilder.Append("\",");
                    }
                    jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                    jsonBuilder.Append("},");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("]");
                jsonBuilder.Append("}");
            }
            return jsonBuilder.ToString();
        }
    }
}

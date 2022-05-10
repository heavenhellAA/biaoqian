using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;

namespace BarCodePrintSys
{
    public class DB
    {
        public static SqlConnection Con()
        {
            string a = Machine.GetHostName();
            if (Machine.GetHostName() == "xieyongbing")
            {
                string cnStr = ConfigurationManager.ConnectionStrings["INF"].ConnectionString;
                SqlConnection Con = new SqlConnection(cnStr);
                return Con;
            }
            else if (Machine.GetHostName() == "heavenhell")
            {
                string cnStr = ConfigurationManager.ConnectionStrings["SEE"].ConnectionString;
                SqlConnection Con = new SqlConnection(cnStr);
                return Con;
            }
            else
            {
                string cnStr = ConfigurationManager.ConnectionStrings["INF"].ConnectionString;
                SqlConnection Con = new SqlConnection(cnStr);
                return Con;
            }   
        }
        public static SqlConnection Con_51()
        {
            //string cnStr = ConfigurationManager.AppSettings["connstr"];
            //string cnStr = ConfigurationManager.ConnectionStrings["blueghost"].ConnectionString;
            string cnStr = ConfigurationManager.ConnectionStrings["PulianData_51"].ConnectionString;
            SqlConnection Con = new SqlConnection(cnStr);
            return Con;
        }
        public static SqlConnection Con_52()
        {
            //string cnStr = ConfigurationManager.AppSettings["connstr"];
            //string cnStr = ConfigurationManager.ConnectionStrings["blueghost"].ConnectionString;
            string cnStr = ConfigurationManager.ConnectionStrings["PulianData_52"].ConnectionString;
            SqlConnection Con = new SqlConnection(cnStr);
            return Con;
        }
        public static SqlConnection Con_2D()
        {
            //string cnStr = ConfigurationManager.AppSettings["connstr"];
            //string cnStr = ConfigurationManager.ConnectionStrings["blueghost"].ConnectionString;
            string cnStr = ConfigurationManager.ConnectionStrings["PulianData_2D"].ConnectionString;
            SqlConnection Con = new SqlConnection(cnStr);
            return Con;
        }
        public static SqlConnection Con_2()
        {
            //string cnStr = ConfigurationManager.AppSettings["connstr"];
            //string cnStr = ConfigurationManager.ConnectionStrings["blueghost"].ConnectionString;
            string cnStr = ConfigurationManager.ConnectionStrings["PulianData_2"].ConnectionString;
            SqlConnection Con = new SqlConnection(cnStr);
            return Con;
        }
        public static SqlConnection Con_1()
        {
            //string cnStr = ConfigurationManager.AppSettings["connstr"];
            //string cnStr = ConfigurationManager.ConnectionStrings["blueghost"].ConnectionString;
            string cnStr = ConfigurationManager.ConnectionStrings["PulianData_1"].ConnectionString;
            SqlConnection Con = new SqlConnection(cnStr);
            return Con;
        }
        public static SqlConnection Con_3()
        {
            //string cnStr = ConfigurationManager.AppSettings["connstr"];
            //string cnStr = ConfigurationManager.ConnectionStrings["blueghost"].ConnectionString;
            string cnStr = ConfigurationManager.ConnectionStrings["PulianData_3"].ConnectionString;
            SqlConnection Con = new SqlConnection(cnStr);
            return Con;
        }
        public static SqlConnection Con_6()
        {
            //string cnStr = ConfigurationManager.AppSettings["connstr"];
            //string cnStr = ConfigurationManager.ConnectionStrings["blueghost"].ConnectionString;
            string cnStr = ConfigurationManager.ConnectionStrings["PulianData_6"].ConnectionString;
            SqlConnection Con = new SqlConnection(cnStr);
            return Con;
        }
        public static SqlConnection MES()
        {
            //string cnStr = ConfigurationManager.AppSettings["connstr"];
            //string cnStr = ConfigurationManager.ConnectionStrings["blueghost"].ConnectionString;
            string cnStr = ConfigurationManager.ConnectionStrings["MES"].ConnectionString;
            SqlConnection Con = new SqlConnection(cnStr);
            return Con;
        }
    }
}

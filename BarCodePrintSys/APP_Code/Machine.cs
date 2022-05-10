using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.InteropServices;
namespace BarCodePrintSys
{
    public class Machine
    {
        //
        // GET: /Machine/

        //取机器名
        public static string GetHostName()
        {
            return System.Net.Dns.GetHostName();
        }

    }
}
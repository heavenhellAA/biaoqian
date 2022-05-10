using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BarCodePrintSys.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //将自定义异常过滤器的优先级提高，防止异常被默认的HandleError处理（也可以自定义类重写HandleErrorAttribute 实现错误处理）
            //filters.Add(new SystemIExceptionFilter(), 1);
            //控制器过滤器
            //filters.Add(new SystemIActionFilter());
            //filters.Add(new AuthenticationAttribute());
            //授权过滤器
            //filters.Add(new SystemIAuthorizationFilter(), 3);
        }
    }
}
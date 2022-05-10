using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarCodePrintSys.Controllers
{
    /// <summary>
    /// RFC配置类
    /// </summary>
    public class RFCConfig
    {
        /// <summary>
        /// //服务器名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 系统IP
        /// </summary>
        public string AppServerHost { get; set; }
        /// <summary>
        /// 客户端
        /// </summary>
        public string Client { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 系统ID
        /// </summary>
        public string SystemID { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 系统编号
        /// </summary>
        public string SystemNumber { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// RFC方法
        /// </summary>
        public RFCCreateFunction RFCCreateFunction { get; set; }



        public RFCConfig()
        {
            this.Name = "艾华系统-router";
            this.AppServerHost = "192.168.0.25";
            this.Client = "800";
            this.User = "AH-ABAP03";
            this.SystemID = "S4P";
            this.Password = "1234567abc";
            this.SystemNumber = "00";
            this.Language = "ZH";
            this.RFCCreateFunction = RFCCreateFunction;
        }

        /// <summary>
        /// 配置RFC链接函数
        /// </summary>
        public RfcDestination ConfigurationFunction()
        {
            //下面开始建立连接
            RfcConfigParameters RCFParam = new RfcConfigParameters();
            RCFParam.Add(RfcConfigParameters.Name, this.Name);//服务器名称
            RCFParam.Add(RfcConfigParameters.AppServerHost, this.AppServerHost);//系统IP
            RCFParam.Add(RfcConfigParameters.Client, this.Client);//客户端
            RCFParam.Add(RfcConfigParameters.User, this.User);//用户名
            RCFParam.Add(RfcConfigParameters.SystemID, this.SystemID);//系统ID
            RCFParam.Add(RfcConfigParameters.Password, this.Password);//密码
            RCFParam.Add(RfcConfigParameters.SystemNumber, this.SystemNumber);//系统编号
            RCFParam.Add(RfcConfigParameters.Language, this.Language);//语言
            RCFParam.Add(RfcConfigParameters.PoolSize, "5");//连接池大小
            RCFParam.Add(RfcConfigParameters.MaxPoolSize, "10");//连接池最大限制
            RCFParam.Add(RfcConfigParameters.IdleTimeout, "500");//超时时间


            //完成设置
            RfcDestination dest = RfcDestinationManager.GetDestination(RCFParam);

            return dest;
        }
    }


    /// <summary>
    /// RFC方法类
    /// </summary>
    public class RFCCreateFunction
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public string FunctionName { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        public List<string> TableNameList { get; set; }
        /// <summary>
        /// 输出参数数组
        /// </summary>
        public List<List<string>> OutPutList { get; set; }
        /// <summary>
        /// 输入参数数组
        /// </summary>
        public Dictionary<string, object> InPutList { get; set; }
    }



    }


namespace BarCodePrintSys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    //菜单表格数据
    public partial class JData
    {
        public int code { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public List<TMenu> data { get; set; }
    }
}
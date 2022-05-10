using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace BarCodePrintSys.Models.Prints
{
    [Table("tbXingWangRuiJiePrint")]
    public class XingWangRuiJieEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Key]
        public int ID { get; set; }

        public string SId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Khlh { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Csdm { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? Cgqd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Nkzz { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Wlbbh { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Creator { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Updator { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? Createtime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DateTime? Updatetime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Groupid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Roleid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Waternum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int? N_state { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool? N_bdprint { get; set; }
    }
    //public List<XingWangRuiJieEntity> XingWangRuiJieEntities { get; set; }
    public class XingWangRuiJieEntityDBContext : DbContext
    {
        public DbSet<XingWangRuiJieEntity> XingWangRuiJie { get; set; }
    }
}
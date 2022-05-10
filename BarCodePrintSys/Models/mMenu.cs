namespace BarCodePrintSys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tbMenu")]//菜单表
    public partial class Menu
    {
        [Key]
        [StringLength(50)]
        public string MenuID { get; set; }//菜单ID

        public int ParentNode { get; set; }//父节点

        public int Node { get; set; }//本级节点
        
        [StringLength(50)]
        public string MenuName { get; set; }//菜单名称

        [StringLength(50)]
        public string Url { get; set; }//访问地址

        public int RoleNO { get; set; }//权限级别

        public int Sort { get; set; }//排序

        public bool IsDeleted { get; set; }//是否删除

        [StringLength(50)]
        public string CreateUserID { get; set; }//创建人ID

        public DateTime CreateTime { get; set; }//创建时间

        [StringLength(50)]
        public string UpdateUserID { get; set; }//最后一次修改人ID

        public DateTime UpdateTime { get; set; }//最后修改时间
    }
    public partial class TMenu
    {
        public int ParentNode { get; set; }//父节点

        public int Node { get; set; }//本级节点

        [StringLength(50)]
        public string MenuName { get; set; }//菜单名称

        [StringLength(50)]
        public string Url { get; set; }//访问地址
    }
}
namespace BarCodePrintSys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tbRole")]//权限表
    public partial class Role
    {
        [Key]
        [StringLength(50)]
        public string RoleID { get; set; }//权限ID

        public int RoleNO { get; set; }//权限级别

        [StringLength(50)]
        public string RoleName { get; set; }//权限名称

        public int Sort { get; set; }//排序

        public bool IsDeleted { get; set; }//是否删除

        [StringLength(50)]
        public string CreateUserID { get; set; }//创建人ID

        public DateTime CreateTime { get; set; }//创建时间

        [StringLength(50)]
        public string UpdateUserID { get; set; }//最后一次修改人ID

        public DateTime UpdateTime { get; set; }//最后修改时间
    }

    public partial class TRole
    {
        [StringLength(50)]
        public string RoleID { get; set; }//权限ID

        public int RoleNO { get; set; }//权限级别

        [StringLength(50)]
        public string RoleName { get; set; }//权限名称

        public int Sort { get; set; }//排序
    }
}
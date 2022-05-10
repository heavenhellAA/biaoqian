namespace BarCodePrintSys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tbUser")]//用户表
    public partial class User
    {
        [Key]
        [StringLength(50)]
        public string UserID { get; set; }//用户ID

        [StringLength(50)]
        public string UserName { get; set; }//用户名称

        [StringLength(50)]
        public string Name { get; set; }//姓名

        [StringLength(50)]
        public string UserPassword { get; set; }//用户密码

        [StringLength(50)]
        public string GroupID { get; set; }//用户分组ID

        [StringLength(50)]
        public string RoleID { get; set; }//用户权限ID

        public int RoleNO { get; set; }//权限级别

        public bool IsDeleted { get; set; }//是否删除

        [StringLength(50)]
        public string CreateUserID { get; set; }//创建人ID

        public DateTime CreateTime { get; set; }//创建时间

        [StringLength(50)]
        public string UpdateUserID { get; set; }//最后一次修改人ID

        public DateTime UpdateTime { get; set; }//最后修改时间
    }
    public partial class TUser
    {
        [StringLength(50)]
        public string UserID { get; set; }//用户ID

        [StringLength(50)]
        public string UserName { get; set; }//用户名称

        [StringLength(50)]
        public string Name { get; set; }//姓名

        [StringLength(50)]
        public string UserPassword { get; set; }//用户密码

        public int RoleNO { get; set; }//权限级别

        public string GroupID { get; set; }//分组ID

        [StringLength(50)]
        public string RoleName { get; set; }//权限名称

        [StringLength(50)]
        public string GroupName { get; set; }//分组名称
    }
}
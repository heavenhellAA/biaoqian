namespace BarCodePrintSys
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tbGroup")]//权限表
    public partial class Group
    {
        [Key]
        [StringLength(50)]
        public string GroupID { get; set; }//权限ID

        [StringLength(50)]
        public string GroupName { get; set; }//权限名称

        public int Sort { get; set; }//排序

        public bool IsDeleted { get; set; }//是否删除

        [StringLength(50)]
        public string CreateUserID { get; set; }//创建人ID

        public DateTime CreateTime { get; set; }//创建时间

        [StringLength(50)]
        public string UpdateUserID { get; set; }//最后一次修改人ID

        public DateTime UpdateTime { get; set; }//最后修改时间
    }

    public partial class TGroup
    {
        [StringLength(50)]
        public string GroupID { get; set; }//分组ID

        [StringLength(50)]
        public string GroupName { get; set; }//分组名称

        public int Sort { get; set; }//排序
    }
}
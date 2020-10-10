using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    /// <summary>
    /// 实体基础类
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Description("编号")]
        public int Id { get; set; }
        /// <summary>
        /// Guid
        /// </summary>
        [Description("Guid")]
        public Guid Guid { get; set; }
        /// <summary>
        /// 状态(-1:逻辑删除;0:锁定;1:正常)
        /// </summary>
        [Range(-1, 1), Required]
        [DefaultValue(1)]
        [Description("状态")]
        public int Status { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [DefaultValue(100)]
        [Description("排序")]
        public int SortId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [DataType(DataType.DateTime)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Description("更新时间")]
        public DateTime UpdateTime { get; set; }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lenceas.Core.Model
{
    /// <summary>
    /// 基础Model
    /// </summary>
    public class BaseEntity
    {
        public BaseEntity()
        {
            CDate = DateTime.Now;
            MDate = DateTime.Now;
            Remark = string.Empty;
        }

        /// <summary>
        /// 主键
        /// </summary>
        [Description("主键")]
        public int Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(TypeName = "datetime")]
        [Description("创建时间")]
        [Required]
        public DateTime CDate { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column(TypeName = "datetime")]
        [Description("更新时间")]
        [Required]
        public DateTime MDate { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        [Required]
        public int Sort { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        [Description("备注")]
        public string? Remark { get; set; }
    }
}
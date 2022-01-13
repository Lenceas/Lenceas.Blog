using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lenceas.Core.Model
{
    /// <summary>
    /// 角色表
    /// </summary>
    public class Role : BaseEntity
    {
        #region 构造函数
        public Role()
        {
        }
        #endregion

        /// <summary>
        /// 角色名称
        /// </summary>
        [Column(TypeName = "varchar(20)")]
        [Description("角色名称")]
        [Required]
        public string RoleName { get; set; }
    }
}

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lenceas.Core.Model
{
    /// <summary>
    /// 角色菜单表
    /// </summary>
    public class RoleMenu : BaseEntity
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Description("角色ID")]
        [Required]
        public int RoleID { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        [Description("菜单ID")]
        [Required]
        public int MenuID { get; set; }
    }
}

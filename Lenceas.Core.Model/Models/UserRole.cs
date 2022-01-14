using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lenceas.Core.Model
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    public class UserRole : BaseEntity
    {
        #region 构造函数
        public UserRole()
        {
            CDate = DateTime.Now;
            MDate = DateTime.Now;
            Remark = string.Empty;
        }
        #endregion

        /// <summary>
        /// 用户ID
        /// </summary>
        [Description("用户ID")]
        [Required]
        public int UserID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [Description("角色ID")]
        [Required]
        public int RoleID { get; set; }
    }
}

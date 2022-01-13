using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lenceas.Core.Model
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class User : BaseEntity
    {
        #region 构造函数
        public User()
        {
            CDate = DateTime.Now;
            MDate = DateTime.Now;
        }

        public User(string account, string md5pwd)
        {
            UserName = account;
            Password = md5pwd;
            Email = string.Empty;
            CDate = DateTime.Now;
            MDate = DateTime.Now;
        }
        #endregion

        /// <summary>
        /// 账号
        /// </summary>
        [Column(TypeName = "varchar(20)")]
        [Description("账号")]
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        [Description("密码")]
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        [Description("邮箱")]
        public string Email { get; set; }
    }
}

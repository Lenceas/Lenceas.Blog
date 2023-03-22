using System.ComponentModel.DataAnnotations;

namespace Lenceas.Core.Model
{
    /// <summary>
    /// 登录入参 WebModel
    /// </summary>
    public class LoginWebModel
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
namespace Lenceas.Core.Model
{
    /// <summary>
    /// 用户 EditWebModel
    /// </summary>
    public class UserEditWebModel : BaseEditWebModel
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
    }
}

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
        public string UserName { get; set; } = default!;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = default!;

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = default!;
    }
}
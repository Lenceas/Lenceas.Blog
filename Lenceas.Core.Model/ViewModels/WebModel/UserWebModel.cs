namespace Lenceas.Core.Model
{
    /// <summary>
    /// 用户 WebModel
    /// </summary>
    public class UserWebModel : BaseWebModel
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; } = default!;

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; } = default!;
    }
}
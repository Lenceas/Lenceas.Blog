namespace Lenceas.Core.Model
{
    public class TokenInfoViewModel
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; } = default!;

        /// <summary>
        /// 过期时间 单位:秒 默认15分钟
        /// </summary>
        public int Expires_In { get; set; }

        /// <summary>
        /// Token类型
        /// </summary>
        public string Token_Type { get; set; } = default!;
    }
}
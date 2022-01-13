namespace Lenceas.Core.Model
{
    public class TokenInfoViewModel
    {
        /// <summary>
        /// Token
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 过期时间 单位:秒 默认15分钟
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// Token类型
        /// </summary>
        public string token_type { get; set; }
    }
}

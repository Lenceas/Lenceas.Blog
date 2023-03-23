namespace Lenceas.Core.Model
{
    /// <summary>
    /// Api通用返回信息类
    /// </summary>
    public class ApiResult<T>
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; } = 200;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get { return Status == 200; } }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; } = "服务器异常";

        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data { get; set; } = default!;
    }

    public class ApiResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Status { get; set; } = 200;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get { return Status == 200; } }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; } = "服务器异常";

        /// <summary>
        /// 响应数据
        /// </summary>
        public string Data { get; set; } = default!;
    }
}
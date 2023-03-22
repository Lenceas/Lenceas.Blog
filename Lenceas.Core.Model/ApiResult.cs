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
        public int status { get; set; } = 200;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get { return status == 200; } }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; } = "服务器异常";

        /// <summary>
        /// 响应数据
        /// </summary>
        public T data { get; set; } = default!;
    }

    public class ApiResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int status { get; set; } = 200;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get { return status == 200; } }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; } = "服务器异常";

        /// <summary>
        /// 响应数据
        /// </summary>
        public string data { get; set; } = default!;
    }
}
using Lenceas.Core.Model;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Newtonsoft.Json;

namespace Lenceas.Core.Extensions
{
    /// <summary>
    /// 当前用户信息
    /// </summary>
    public class UserContext
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IRedisBaseRepository _redis;
        public UserContext(IHttpContextAccessor accessor, IRedisBaseRepository redis)
        {
            _accessor = (HttpContextAccessor)accessor;
            _redis = (RedisBaseRepository)redis;
        }

        /// <summary>
        /// 获取当前登录的TokenData
        /// </summary>
        public AuthModel CurrentTokenData
        {
            get
            {
                var current = _accessor.HttpContext;
                if (current == null || current.Request == null)
                {
                    return null;
                }

                return GetAuthenticatedTokenData();
            }
        }

        /// <summary>
        /// 获取当前已登录的用户缓存对象
        /// </summary>
        /// <returns></returns>
        public AuthModel GetAuthenticatedTokenData()
        {
            // 1. 从header/QueryString/Form中获取token数据
            AuthModel userTokenData = null;
            string tokenKey = GetHttpTokenKey();
            if (!string.IsNullOrEmpty(tokenKey))
            {
                // 1. header中找到token标识，根据TokenKey去缓存找AuthModel
                var cacheTokenData = _redis.GetValue($"Token:{tokenKey}");
                if (cacheTokenData != null)
                {
                    userTokenData = JsonConvert.DeserializeObject<AuthModel>(cacheTokenData);
                }
            }

            return userTokenData;
        }

        /// <summary>
        /// 根据http请求头获取的tokeKey数据
        /// </summary>
        /// <returns></returns>
        private string GetHttpTokenKey()
        {
            // 1. http会话为空时返回NULL
            var current = _accessor.HttpContext;
            if (current == null || current.Request == null)
            {
                return string.Empty;
            }

            // 2. 从header/QueryString/Form中获取token数据

            // a.从headers中找
            string tokenKey = current.Request.Headers["Authorization"];
            // b.从querystring中找
            if (string.IsNullOrEmpty(tokenKey))
                tokenKey = current.Request.Query["token"].FirstOrDefault();

            return tokenKey.Replace("Bearer ", "");
        }
    }
}
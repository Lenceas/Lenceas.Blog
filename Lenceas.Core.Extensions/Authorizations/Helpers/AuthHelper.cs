using Lenceas.Core.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace Lenceas.Core.Extensions
{
    public class AuthHelper
    {
        public AuthModel _userRole;
        private readonly IHttpContextAccessor _accessor;
        private readonly IRedisBaseRepository _redis;
        public AuthHelper(IHttpContextAccessor accessor, IRedisBaseRepository redis)
        {
            _accessor = accessor;
            _redis = redis;
        }

        /// <summary>
        /// 当前的用户身份缓存对象
        /// </summary>
        public AuthModel UserRoleCache { get { return GetCurUserRole(); } }

        /// <summary>
        /// 获取当前的用户身份缓存对象
        /// </summary>
        /// <returns></returns>
        public AuthModel GetCurUserRole()
        {
            if (_userRole != null)
                return _userRole;
            var tokenData = new UserContext(_accessor, _redis).CurrentTokenData;
            if (tokenData != null)
            {
                _userRole = tokenData;
                return _userRole;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 保存当前用户身份缓存对象
        /// </summary>
        /// <param name="model"></param>
        public void SaveCurrSessionAndUserRole(TokenInfoViewModel tokenInfo, AuthModel model)
        {
            try
            {
                _redis.SetValue($"Token:{tokenInfo.token}", model, tokenInfo.expires_in);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
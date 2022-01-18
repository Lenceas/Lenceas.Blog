using Lenceas.Core.Common;
using Lenceas.Core.Extensions;
using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static Lenceas.Core.Extensions.CustomApiVersion;

namespace Lenceas.Core.Api.Controllers
{
    /// <summary>
    /// 权限相关接口
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [CustomRoute(ApiVersions.v1)]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        #region 构造函数
        private readonly IHttpContextAccessor _accessor;
        private readonly IRedisBaseRepository _redis;
        private readonly IUserServices _userServices;
        private readonly IUserRoleServices _roleServices;
        public AuthController(IHttpContextAccessor accessor, IRedisBaseRepository redis, IUserServices userServices, IUserRoleServices roleServices)
        {
            _accessor = accessor;
            _redis = redis;
            _userServices = userServices;
            _roleServices = roleServices;
        }
        #endregion

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="name">账号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public async Task<ApiResult<TokenInfoViewModel>> Login(string name = "", string pwd = "")
        {
            var r = new ApiResult<TokenInfoViewModel>();
            try
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
                {
                    r.status = 400;
                    r.msg = "账号或密码不能为空！";
                    return r;
                }
                var user = await _userServices.GetEntity(t => t.UserName.Equals(name) && t.Password.Equals(MD5Helper.MD5Encrypt32(pwd)));
                if (user != null)
                {
                    var userRole = await _roleServices.GetList(_ => _.UserID == user.Id);
                    var role = string.Empty;
                    if (userRole != null && userRole.Any())
                    {
                        role = string.Join(",", userRole.Select(_ => _.RoleID).Distinct());
                    }
                    //创建声明数组
                    var claims = new Claim[] {
                        new Claim("uid",user.Id.ToString()),
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.Role,role),
                        new Claim(JwtRegisteredClaimNames.Email,user.Email)
                    };
                    var responseJson = JwtToken.BuildJwtToken(claims);
                    if (responseJson != null)
                    {
                        r.status = 200;
                        r.msg = "登录成功！";
                        r.data = responseJson;
                        new AuthHelper(_accessor, _redis).SaveCurrSessionAndUserRole(responseJson, new AuthModel() { UserID = user.Id, UserName = user.UserName, RoleIDs = userRole?.Select(_ => _.Id).Distinct().ToList() ?? new List<int>() });
                    }
                }
                else
                {
                    r.status = 401;
                    r.msg = "账号或密码错误！";
                }
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="name">账号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public async Task<ApiResult<User>> Register(string name, string pwd)
        {
            var r = new ApiResult<User>();
            try
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
                {
                    r.status = 400;
                    r.msg = "账号或密码不能为空！";
                    return r;
                }
                var isExist = await _userServices.ExistName(name);
                if (isExist)
                {
                    r.status = 400;
                    r.msg = "账号已存在！";
                    return r;
                }
                var user = await _userServices.Register(name, pwd);
                if (user == null)
                {
                    r.status = 400;
                    r.msg = "注册失败！";
                    return r;
                }
                r.status = 200;
                r.msg = "注册成功！";
                r.data = user;
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 请求刷新Token（以旧换新），不安全即将遗弃，新方法开发中:通过redis中间层来控制，这样还可以实现单点登录，或者退出登录，取消token权限的问题
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RefreshToken")]
        [Obsolete]
        public async Task<ApiResult<TokenInfoViewModel>> RefreshToken(string token)
        {
            var r = new ApiResult<TokenInfoViewModel>();
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    r.status = 400;
                    r.msg = "token无效，请重新登录！";
                    return r;
                }
                var tokenModel = JwtHelper.SerializeToken(token);
                if (tokenModel != null && JwtHelper.CustomSafeVerify(token) && tokenModel.Uid > 0)
                {
                    var user = await _userServices.GetById(tokenModel.Uid);
                    if (user != null)
                    {
                        var userRole = await _roleServices.GetList(_ => _.UserID == user.Id);
                        var role = string.Empty;
                        if (userRole != null && userRole.Any())
                        {
                            role = string.Join(",", userRole.Select(_ => _.RoleID).Distinct());
                        }
                        //创建声明数组
                        var claims = new Claim[] {
                            new Claim("uid",user.Id.ToString()),
                            new Claim(ClaimTypes.Name,user.UserName),
                            new Claim(ClaimTypes.Role,role),
                            new Claim(JwtRegisteredClaimNames.Email,user.Email)
                        };
                        var responseJson = JwtToken.BuildJwtToken(claims);
                        if (responseJson != null)
                        {
                            r.status = 200;
                            r.msg = "刷新Token成功！";
                            r.data = responseJson;
                            new AuthHelper(_accessor, _redis).SaveCurrSessionAndUserRole(responseJson, new AuthModel() { UserID = user.Id, UserName = user.UserName, RoleIDs = userRole?.Select(_ => _.Id).Distinct().ToList() ?? new List<int>() });
                            return r;
                        }
                    }
                }
                r.status = 400;
                r.msg = "刷新token失败请重新登录！";
                return r;
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }
    }
}

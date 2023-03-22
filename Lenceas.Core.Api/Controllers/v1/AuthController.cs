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
        /// <param name="model">登录入参 WebModel</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public async Task<ApiResult<TokenInfoViewModel>> Login(LoginWebModel model)
        {
            var r = new ApiResult<TokenInfoViewModel>();
            try
            {
                if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                {
                    r.status = 400;
                    r.msg = "账号或密码不能为空！";
                    return r;
                }
                var user = await _userServices.GetEntity(t => t.UserName.Equals(model.UserName) && t.Password.Equals(MD5Helper.MD5Encrypt32(model.Password)));
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
                        new Claim(JwtRegisteredClaimNames.Email,user?.Email??string.Empty)
                    };
                    var responseJson = JwtToken.BuildJwtToken(claims);
                    if (responseJson != null)
                    {
                        r.status = 200;
                        r.msg = "登录成功！";
                        r.data = responseJson;
                        new AuthHelper(_accessor, _redis).SaveCurrSessionAndUserRole(responseJson, new AuthModel() { UserID = user?.Id ?? 0, UserName = user?.UserName ?? string.Empty, RoleIDs = userRole?.Select(_ => _.Id).Distinct().ToList() ?? new List<int>() });
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
        /// <param UserName="name">账号</param>
        /// <param UserName="Password">密码</param>
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
        /// 请求刷新Token
        /// </summary>
        /// <param UserName="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RefreshToken")]
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
                            new Claim(JwtRegisteredClaimNames.Email,user?.Email??string.Empty)
                        };
                        var responseJson = JwtToken.BuildJwtToken(claims);
                        if (responseJson != null)
                        {
                            r.status = 200;
                            r.msg = "刷新Token成功！";
                            r.data = responseJson;
                            new AuthHelper(_accessor, _redis).SaveCurrSessionAndUserRole(responseJson, new AuthModel() { UserID = user?.Id ?? 0, UserName = user?.UserName ?? string.Empty, RoleIDs = userRole?.Select(_ => _.Id).Distinct().ToList() ?? new List<int>() });
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

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param UserName="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Logout")]
        public async Task<ApiResult> Logout(string token)
        {
            var r = new ApiResult();
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    var tokenModel = JwtHelper.SerializeToken(token);
                    if (tokenModel != null && JwtHelper.CustomSafeVerify(token) && tokenModel.Uid > 0)
                    {
                        if (_redis.Exist($"Token:{token}"))
                        {
                            await _redis.RemoveAsync($"Token:{token}");
                        }
                    }
                }
                r.msg = "退出登录成功";
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

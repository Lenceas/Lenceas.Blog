using Lenceas.Core.Common;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lenceas.Core.Extensions
{
    public class JwtHelper
    {
        /// <summary>
        /// Token解析
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static TokenModelJwt SerializeToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            TokenModelJwt tokenModelJwt = new TokenModelJwt();
            // token校验
            if (token.IsNotEmptyOrNull() && jwtHandler.CanReadToken(token))
            {
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(token);
                jwtToken.Payload.TryGetValue("uid", out object? uid);
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out object? role);
                tokenModelJwt = new TokenModelJwt
                {
                    Uid = uid?.ObjToInt() ?? 0,
                    Role = role != null ? role.ObjToString() : "",
                };
            }
            return tokenModelJwt;
        }

        /// <summary>
        /// 验证Token安全性
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool CustomSafeVerify(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var symmetricKeyAsBase64 = AppSecretConfig.Audience_Secret_String;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = jwtHandler.ReadJwtToken(token);
            return jwt.RawSignature == Microsoft.IdentityModel.JsonWebTokens.JwtTokenUtilities.CreateEncodedSignature(jwt.RawHeader + "." + jwt.RawPayload, signingCredentials);
        }
    }

    /// <summary>
    /// 令牌
    /// </summary>
    public class TokenModelJwt
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int Uid { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; } = default!;
    }
}

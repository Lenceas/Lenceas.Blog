using Lenceas.Core.Common;
using Lenceas.Core.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lenceas.Core.Extensions
{
    /// <summary>
    /// JwtToken生成类
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// 获取基于Jwt的Token
        /// </summary>
        /// <param name="claims">需要在登陆的时候配置</param>
        /// <returns></returns>
        public static TokenInfoViewModel BuildJwtToken(Claim[] claims)
        {
            //读取配置文件
            var symmetricKeyAsBase64 = ConfigHelper.JwtSecretString;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var Issuer = AppSettings.App(new string[] { "Audience", "Issuer" });
            var Audience = AppSettings.App(new string[] { "Audience", "Audience" });
            var Expires = Convert.ToInt32(AppSettings.App(new string[] { "Audience", "Expires" }) ?? "15");
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Expires),
                signingCredentials: signingCredentials
            );
            //生成token
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);
            //打包返回前台
            var responseJson = new TokenInfoViewModel
            {
                Token = encodedJwt,
                Expires_In = (int)TimeSpan.FromMinutes(Expires).TotalSeconds,
                Token_Type = "Bearer"
            };
            return responseJson;
        }
    }
}

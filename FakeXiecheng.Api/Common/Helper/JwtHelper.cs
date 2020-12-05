using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace FakeXiecheng.Api.Common.Helper
{
    public class JwtHelper
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        // public static string IssueToken(Manage manage)
        public static string IssueToken(string userId)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name,"JWT"),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim(JwtRegisteredClaimNames.Sub,userId),
                // new Claim(nameof(Manage.Id),Convert.ToString(manage.Id)),
                // new Claim(nameof(Manage.UserName),manage.UserName),
                // new Claim(nameof(Manage.NickName),manage.NickName),
                // new Claim(nameof(Manage.Level),Convert.ToString(manage.Level))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfigs.Key));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               issuer: JwtConfigs.Issuer,
               audience: JwtConfigs.Issuer,
               claims: claims,
               notBefore: DateTime.UtcNow,
               expires: DateTime.UtcNow.AddMinutes(JwtConfigs.Expires),
               signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 从token获取信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        // public static Manage ParseToken(string token)
        public static string ParseToken(string token)
        {
            if (token is null)
                return null;

            var tokenStr = token.Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();

            var payload = handler.ReadJwtToken(tokenStr).Payload;

            var claims = payload.Claims.ToList();

            // var manage = new Manage()
            // {
            //     Id = Convert.ToInt32(claims.FirstOrDefault(claim => claim.Type == nameof(Manage.Id))?.Value),
            //     UserName = claims.FirstOrDefault(claim => claim.Type == nameof(Manage.UserName))?.Value,
            //     NickName = claims.FirstOrDefault(claim => claim.Type == nameof(Manage.NickName))?.Value,
            //     Level = Convert.ToInt32(claims.FirstOrDefault(claim => claim.Type == nameof(Manage.Level))?.Value)
            // };
            //
            // return manage;

            return claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
        }
    }
}

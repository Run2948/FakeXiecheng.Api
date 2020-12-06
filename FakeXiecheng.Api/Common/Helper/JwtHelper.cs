using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FakeXiecheng.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FakeXiecheng.Api.Common.Helper
{
    public class JwtHelper
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public static string IssueToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name,"JWT"),
                // new Claim(ClaimTypes.Role,"Admin"),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(nameof(ApplicationUser.UserName),user.UserName),
                new Claim(nameof(ApplicationUser.Email),user.Email),
                new Claim(nameof(ApplicationUser.PhoneNumber),Convert.ToString(user.PhoneNumber))
            };

            if (roles != null && roles.Any())
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

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
        public static ApplicationUser ParseToken(string token)
        {
            if (token is null)
                return null;

            var tokenStr = token.Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();

            var payload = handler.ReadJwtToken(tokenStr).Payload;

            var claims = payload.Claims.ToList();

            var user = new ApplicationUser()
            {
                Id = claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value,
                UserName = claims.FirstOrDefault(claim => claim.Type == nameof(ApplicationUser.UserName))?.Value,
                Email = claims.FirstOrDefault(claim => claim.Type == nameof(ApplicationUser.Email))?.Value,
                PhoneNumber = claims.FirstOrDefault(claim => claim.Type == nameof(ApplicationUser.PhoneNumber))?.Value,
                // UserRoles = claims.Where(claim => claim.Type == ClaimTypes.Role).Select(c => c.Value).ToList()
            };

            return user;
        }
    }
}

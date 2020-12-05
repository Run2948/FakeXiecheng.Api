using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FakeXiecheng.Api.Common;
using FakeXiecheng.Api.Common.Helper;
using FakeXiecheng.Api.Models.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FakeXiecheng.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] LoginDto loginDto)
        {
            // // 1. 验证用户名密码
            // var loginResult = await _signInManager.PasswordSignInAsync(
            //     loginDto.Email,
            //     loginDto.Password,
            //     false,
            //     false
            // );
            // if (!loginResult.Succeeded)
            // {
            //     return BadRequest();
            // }
            // var user = await _userManager.FindByNameAsync(loginDto.Email);
            //
            var tokenStr = JwtHelper.IssueToken("fake_user_id");
            // // 3. return 200 OK + jwt
            return Ok(tokenStr);
        }
    }
}

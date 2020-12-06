using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeXiecheng.Api.Common.Helper;
using FakeXiecheng.Api.Models;
using FakeXiecheng.Api.Models.Dtos;
using FakeXiecheng.Api.Repository;
using Microsoft.AspNetCore.Identity;

namespace FakeXiecheng.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticateController(ITouristRouteRepository touristRouteRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _touristRouteRepository = touristRouteRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            // 1. 验证用户名密码
            var loginResult = await _signInManager.PasswordSignInAsync(
                loginDto.Email,
                loginDto.Password,
                false,
                false
            );
            if (!loginResult.Succeeded)
                return BadRequest();
            var user = await _userManager.FindByNameAsync(loginDto.Email);
            var userRoles = await _userManager.GetRolesAsync(user);
            var tokenStr = JwtHelper.IssueToken(user, userRoles);
            return Ok(tokenStr);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            //1. 使用用户名创建用户对象
            var user = new ApplicationUser()
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };
            //2. Hash密码，保存用户
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest();
            }
            //3.给新用户初始化购物车
            // var shoppingCart = new ShoppingCart()
            // {
            //     Id = Guid.NewGuid(),
            //     UserId = user.Id
            // };
            // await _touristRouteRepository.CreateShoppingCart(shoppingCart);
            await _touristRouteRepository.SaveAsync();

            return Ok();
        }
    }
}

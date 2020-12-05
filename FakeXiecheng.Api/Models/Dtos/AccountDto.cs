using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.Api.Models.Dtos
{
    public abstract class AccountDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginDto : AccountDto
    {

    }

    public class RegisterDto : AccountDto
    {
        [Required]
        [Compare(nameof(Password), ErrorMessage = "密码输入不一致")]
        public string ConfirmPassword { get; set; }
    }
}

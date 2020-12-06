using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FakeXiecheng.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }

        public virtual ShoppingCart ShoppingCart { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        // 用户的角色信息
        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        // 用户的权限信息
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        // 第三方登录信息
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        // 用户登录的Token信息
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FakeXiecheng.Api.Repository
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> //DbContext
    {
        private readonly ILogger<AppDbContext> _logger;

        public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        public virtual DbSet<TouristRoute> TouristRoutes { get; set; }
        public virtual DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<LineItem> LineItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.LogInformation(Path.Join(Directory.GetCurrentDirectory(), @"\Models", @"\touristRoutesMockData.json"));
            _logger.LogInformation(Path.Join(Directory.GetCurrentDirectory(), @"\Models", @"\touristRoutePicturesMockData.json"));

            modelBuilder.Entity<TouristRoute>().HasData(JsonConvert.DeserializeObject<List<TouristRoute>>(File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), @"\Models", @"\touristRoutesMockData.json"))));
            modelBuilder.Entity<TouristRoutePicture>().HasData(JsonConvert.DeserializeObject<List<TouristRoutePicture>>(File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), @"\Models", @"\touristRoutePicturesMockData.json"))));

            // 初始化用户与角色的种子数据
            // 1. 更新用户与角色的外键关系
            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.HasMany(x => x.UserRoles)
                    .WithOne()
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            // 2. 添加角色
            var adminRoleId = "308660dc-ae51-480f-824d-7dca6714c3e2"; // guid 
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                }
            );

            // 3. 添加用户
            var adminUserId = "90184155-dee0-40c9-bb1e-b5ed07afc04e";
            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@fakexiecheng.com",
                NormalizedUserName = "admin@fakexiecheng.com".ToUpper(),
                Email = "admin@fakexiecheng.com",
                NormalizedEmail = "admin@fakexiecheng.com".ToUpper(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = "123456789",
                PhoneNumberConfirmed = false
            };
            var ph = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = ph.HashPassword(adminUser, "Fake123$");
            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // 4. 给用户加入管理员权限
            // 通过使用 linking table：IdentityUserRole
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasData(new IdentityUserRole<string>()
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                });


            base.OnModelCreating(modelBuilder);
        }
    }
}

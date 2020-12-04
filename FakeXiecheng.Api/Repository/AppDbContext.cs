using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FakeXiecheng.Api.Repository
{
    public class AppDbContext : DbContext
    {
        private readonly ILogger<AppDbContext> _logger;

        public AppDbContext(DbContextOptions<AppDbContext> options, ILogger<AppDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        public virtual DbSet<TouristRoute> TouristRoutes { get; set; }
        public virtual DbSet<TouristRoutePicture> TouristRoutePictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.LogInformation(Path.Join(Directory.GetCurrentDirectory(), @"\Models", @"\touristRoutesMockData.json"));
            _logger.LogInformation(Path.Join(Directory.GetCurrentDirectory(), @"\Models", @"\touristRoutePicturesMockData.json"));

            modelBuilder.Entity<TouristRoute>().HasData(JsonConvert.DeserializeObject<List<TouristRoute>>(File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), @"\Models", @"\touristRoutesMockData.json"))));
            modelBuilder.Entity<TouristRoutePicture>().HasData(JsonConvert.DeserializeObject<List<TouristRoutePicture>>(File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), @"\Models", @"\touristRoutePicturesMockData.json"))));

            base.OnModelCreating(modelBuilder);
        }
    }
}

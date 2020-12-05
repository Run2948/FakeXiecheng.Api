using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FakeXiecheng.Api.Repository.Impl
{
    public class TouristRouteRepository : ITouristRouteRepository
    {
        private readonly AppDbContext _context;

        public TouristRouteRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TouristRoute> GetTouristRoutes()
        {
            return _context.TouristRoutes.Include(t => t.TouristRoutePictures);
        }

        public TouristRoute GetTouristRoute(Guid touristRouteId)
        {
            // return _context.TouristRoutes.Find(touristRouteId);
            return _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefault(t =>  t.Id == touristRouteId);
        }

        public bool TouristRouteExists(Guid touristRouteId)
        {
            return _context.TouristRoutes.Any(t => t.Id == touristRouteId);
        }

        public IEnumerable<TouristRoutePicture> GetTouristRoutePictures(Guid touristRouteId)
        {
            return _context.TouristRoutePictures.Where(p => p.TouristRouteId == touristRouteId).AsEnumerable();
        }

        public TouristRoutePicture GetTouristRoutePicture(int pictureId)
        {
            return _context.TouristRoutePictures.Find(pictureId);
        }
    }
}

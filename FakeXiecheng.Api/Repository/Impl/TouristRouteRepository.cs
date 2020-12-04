using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.Api.Models;

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
            return _context.TouristRoutes;
        }

        public TouristRoute GetTouristRoute(Guid touristRouteId)
        {
            return _context.TouristRoutes.Find(touristRouteId);
        }
    }
}

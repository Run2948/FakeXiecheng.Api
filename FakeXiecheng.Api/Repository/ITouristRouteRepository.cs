using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.Api.Models;

namespace FakeXiecheng.Api.Repository
{
    public interface ITouristRouteRepository
    {
        IEnumerable<TouristRoute> GetTouristRoutes();

        TouristRoute GetTouristRoute(Guid touristRouteId);

        bool TouristRouteExists(Guid touristRouteId);

        IEnumerable<TouristRoutePicture> GetTouristRoutePictures(Guid touristRouteId);

        TouristRoutePicture GetTouristRoutePicture(int pictureId);
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.Api.Dtos;
using FakeXiecheng.Api.Repository;

namespace FakeXiecheng.Api.Controllers
{
    [Route("api/TouristRoutes/{touristRouteId}/Pictures")]
    [ApiController]
    public class TouristRoutePicturesController : ControllerBase
    {
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutePicturesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(Guid touristRouteId)
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
                return NotFound($"找不到{touristRouteId}旅游路线");
            var pictures = _touristRouteRepository.GetTouristRoutePictures(touristRouteId);
            if (!pictures.Any()) return NotFound($"没有找到旅游路线{touristRouteId}的照片");
            return Ok(_mapper.Map<List<TouristRoutePictureDto>>(pictures));
        }

        [HttpGet("{pictureId:int}")]
        public IActionResult Get(Guid touristRouteId, int pictureId)
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
                return NotFound($"找不到{touristRouteId}旅游路线");
            var picture = _touristRouteRepository.GetTouristRoutePicture(pictureId);
            if (picture == null) return NotFound($"找到不到{pictureId}照片");
            return Ok(_mapper.Map<TouristRoutePictureDto>(picture));
        }
    }
}

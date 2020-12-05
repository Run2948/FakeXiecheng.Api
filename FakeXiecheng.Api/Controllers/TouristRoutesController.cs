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
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public TouristRoutesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        public IActionResult Get()
        {
            var routes = _touristRouteRepository.GetTouristRoutes();
            if (!routes.Any()) return NotFound("没有找到旅游路线");
            return Ok(_mapper.Map<List<TouristRouteDto>>(routes));
        }

        [HttpGet("{touristRouteId:Guid}")]
        [HttpHead("{touristRouteId:Guid}")]
        public IActionResult Get(Guid touristRouteId)
        {
            var route = _touristRouteRepository.GetTouristRoute(touristRouteId);
            if (route == null) return NotFound($"找不到{touristRouteId}旅游路线");
            return Ok(_mapper.Map<TouristRouteDto>(route));
        }
    }
}

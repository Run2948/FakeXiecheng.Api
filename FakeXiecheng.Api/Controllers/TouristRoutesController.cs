using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.Api.Dtos;
using FakeXiecheng.Api.Models;
using FakeXiecheng.Api.Repository;
using FakeXiecheng.Api.ResourceParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

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
        public async Task<IActionResult> Get([FromQuery] TouristRouteResourceParameters parameters)
        {
            var routes = await _touristRouteRepository.GetTouristRoutesAsync(parameters.Keyword, parameters.RatingOperator, parameters.RatingValue, parameters.PageNumber, parameters.PageSize);
            if (!routes.Any()) return NotFound("没有找到旅游路线");
            return Ok(_mapper.Map<List<TouristRouteDto>>(routes));
        }

        [HttpGet("{touristRouteId:Guid}", Name = "GetTouristRouteById")]
        // [HttpHead("{touristRouteId:Guid}")]
        public async Task<IActionResult> Get(Guid touristRouteId)
        {
            var route = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            if (route == null) return NotFound($"找不到{touristRouteId}旅游路线");
            return Ok(_mapper.Map<TouristRouteDto>(route));
        }

        [HttpPost]
        // [Authorize(AuthenticationSchemes = "Bearer")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTouristRoute([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
        {
            var touristRoute = _mapper.Map<TouristRoute>(touristRouteForCreationDto);
            _touristRouteRepository.AddTouristRoute(touristRoute);
            await _touristRouteRepository.SaveAsync();
            var touristRouteToReturn = _mapper.Map<TouristRouteDto>(touristRoute);
            return CreatedAtRoute(
                "GetTouristRouteById",
                new { touristRouteId = touristRouteToReturn.Id },
                touristRouteToReturn
            );
        }

        // [HttpPut("{touristRouteId}")]
        // [Authorize(AuthenticationSchemes = "Bearer")]
        // [Authorize(Roles = "Admin")]
        // public async Task<IActionResult> UpdateTouristRoute(
        //     [FromRoute] Guid touristRouteId,
        //     [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto
        // )
        // {
        //     if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
        //     {
        //         return NotFound("旅游路线找不到");
        //     }
        //
        //     var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
        //     // 1. 映射dto
        //     // 2. 更新dto
        //     // 3. 映射model
        //     _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);
        //
        //     await _touristRouteRepository.SaveAsync();
        //
        //     return NoContent();
        // }

        // [HttpPatch("{touristRouteId}")]
        // [Authorize(AuthenticationSchemes = "Bearer")]
        // [Authorize(Roles = "Admin")]
        // public async Task<IActionResult> PartiallyUpdateTouristRoute(
        //     [FromRoute] Guid touristRouteId,
        //     [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> patchDocument
        // )
        // {
        //     if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
        //     {
        //         return NotFound("旅游路线找不到");
        //     }
        //
        //     var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
        //     var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRouteFromRepo);
        //     patchDocument.ApplyTo(touristRouteToPatch, ModelState);
        //     if (!TryValidateModel(touristRouteToPatch))
        //     {
        //         return ValidationProblem(ModelState);
        //     }
        //     _mapper.Map(touristRouteToPatch, touristRouteFromRepo);
        //     await _touristRouteRepository.SaveAsync();
        //
        //     return NoContent();
        // }
        //
        // [HttpDelete("{touristRouteId}")]
        // [Authorize(AuthenticationSchemes = "Bearer")]
        // [Authorize(Roles = "Admin")]
        // public async Task<IActionResult> DeleteTouristRoute([FromRoute] Guid touristRouteId)
        // {
        //     if (!(await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId)))
        //     {
        //         return NotFound("旅游路线找不到");
        //     }
        //
        //     var touristRoute = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
        //     _touristRouteRepository.DeleteTouristRoute(touristRoute);
        //     await _touristRouteRepository.SaveAsync();
        //
        //     return NoContent();
        // }
        //
        // [HttpDelete("({touristIDs})")]
        // [Authorize(AuthenticationSchemes = "Bearer")]
        // [Authorize(Roles = "Admin")]
        // public async Task<IActionResult> DeleteByIDs(
        //     [ModelBinder(BinderType = typeof(ArrayModelBinder<>))][FromRoute] IEnumerable<Guid> touristIDs)
        // {
        //     if (touristIDs == null)
        //     {
        //         return BadRequest();
        //     }
        //
        //     var touristRoutesFromRepo = await _touristRouteRepository.GetTouristRoutesByIdListAsync(touristIDs);
        //     _touristRouteRepository.DeleteTouristRoutes(touristRoutesFromRepo);
        //     await _touristRouteRepository.SaveAsync();
        //
        //     return NoContent();
        // }
    }
}

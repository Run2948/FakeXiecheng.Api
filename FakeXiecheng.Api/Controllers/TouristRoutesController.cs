﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.Api.Common.Extensions;
using FakeXiecheng.Api.Models.Dtos;
using FakeXiecheng.Api.Common.Helper;
using FakeXiecheng.Api.Models;
using FakeXiecheng.Api.Repository;
using FakeXiecheng.Api.Models.Requests;
using FakeXiecheng.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;

namespace FakeXiecheng.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TouristRoutesController : ControllerBase
    {
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IUrlHelper _urlHelper;

        public TouristRoutesController(ITouristRouteRepository touristRouteRepository, IMapper mapper, IUrlHelperFactory urlHelperFactory, IActionContextAccessor actionContextAccessor, IPropertyMappingService propertyMappingService)
        {
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
            _propertyMappingService = propertyMappingService;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        [HttpGet(Name = "GetTouristRoutes")]
        [HttpHead]
        public async Task<IActionResult> Get([FromQuery] TouristRouteRequest request)
        {
            if (_propertyMappingService.PropertyMappingExists<TouristRouteDto, TouristRoute>(request.OrderBy))
                return BadRequest($"排序参数{request.OrderBy}存在未识别的属性");
            if (_propertyMappingService.PropertyExists<TouristRouteDto>(request.Fields))
                return BadRequest($"塑性参数{request.Fields}存在未识别属性");
            var routes = await _touristRouteRepository.GetTouristRoutesAsync(request.Keyword, request.RatingOperator, request.RatingValue, request.PageNumber, request.PageSize, request.OrderBy);
            if (!routes.Any()) return NotFound("没有找到旅游路线");
            Response.Headers.Add(_urlHelper.GeneratePaginationHeader(routes, "GetTouristRoutes", request));
            return Ok(_mapper.Map<List<TouristRouteDto>>(routes).ShapeData(request.Fields));
        }

        [HttpGet("{touristRouteId:Guid}", Name = "GetTouristRouteById")]
        // [HttpHead("{touristRouteId:Guid}")]
        public async Task<IActionResult> Get(Guid touristRouteId, string fields)
        {
            if (_propertyMappingService.PropertyExists<TouristRouteDto>(fields))
                return BadRequest($"塑性参数{fields}存在未识别属性");
            var route = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
            if (route == null) return NotFound($"找不到{touristRouteId}旅游路线");
            return Ok(_mapper.Map<TouristRouteDto>(route).ShapeData(fields));
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] TouristRouteForCreationDto touristRouteForCreationDto)
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

        [HttpPut("{touristRouteId:Guid}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> Put([FromRoute] Guid touristRouteId, [FromBody] TouristRouteForUpdateDto touristRouteForUpdateDto)
        {
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
                return NotFound($"找不到{touristRouteId}旅游路线");

            var touristRoute = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);

            _mapper.Map(touristRouteForUpdateDto, touristRoute);

            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{touristRouteId:Guid}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> Patch([FromRoute] Guid touristRouteId, [FromBody] JsonPatchDocument<TouristRouteForUpdateDto> patchDocument)
        {
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
                return NotFound($"找不到{touristRouteId}旅游路线");

            var touristRoute = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);

            var touristRouteToPatch = _mapper.Map<TouristRouteForUpdateDto>(touristRoute);

            // patchDocument.ApplyTo(touristRouteToPatch);
            patchDocument.ApplyTo(touristRouteToPatch, ModelState);
            if (!TryValidateModel(touristRouteToPatch))
                return ValidationProblem(ModelState);

            _mapper.Map(touristRouteToPatch, touristRoute);

            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{touristRouteId:Guid}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid touristRouteId)
        {
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
                return NotFound($"找不到{touristRouteId}旅游路线");

            var touristRoute = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);

            _touristRouteRepository.DeleteTouristRoute(touristRoute);

            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }

        [HttpDelete("({touristRouteIds})")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> Delete([ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute] IEnumerable<Guid> touristRouteIds)
        {
            if (touristRouteIds == null)
                return BadRequest();

            var touristRoutes = await _touristRouteRepository.GetTouristRoutesAsync(touristRouteIds);
            _touristRouteRepository.DeleteTouristRoutes(touristRoutes);
            await _touristRouteRepository.SaveAsync();

            return NoContent();
        }
    }
}

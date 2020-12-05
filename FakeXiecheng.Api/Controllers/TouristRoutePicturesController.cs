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
        public async Task<IActionResult> Get([FromRoute] Guid touristRouteId)
        {
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
                return NotFound($"找不到{touristRouteId}旅游路线");
            var pictures = await _touristRouteRepository.GetTouristRoutePicturesAsync(touristRouteId);
            if (!pictures.Any()) return NotFound($"没有找到旅游路线{touristRouteId}的照片");
            return Ok(_mapper.Map<List<TouristRoutePictureDto>>(pictures));
        }

        [HttpGet("{pictureId:int}", Name = "GetPictureById")]
        public async Task<IActionResult> Get([FromRoute] Guid touristRouteId, [FromRoute] int pictureId)
        {
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
                return NotFound($"找不到{touristRouteId}旅游路线");
            var picture = await _touristRouteRepository.GetTouristRoutePictureAsync(pictureId);
            if (picture == null) return NotFound($"找到不到{pictureId}照片");
            return Ok(_mapper.Map<TouristRoutePictureDto>(picture));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTouristRoutePicture([FromRoute] Guid touristRouteId, [FromBody] TouristRoutePictureForCreationDto touristRoutePictureForCreationDto)
        {
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
                return NotFound($"找不到{touristRouteId}旅游路线");
            var picture = _mapper.Map<TouristRoutePicture>(touristRoutePictureForCreationDto);
            _touristRouteRepository.AddTouristRoutePicture(touristRouteId, picture);
            await _touristRouteRepository.SaveAsync();
            var pictureToReturn = _mapper.Map<TouristRoutePictureDto>(picture);
            return CreatedAtRoute(
                "GetPictureById",
                new { touristRouteId = picture.TouristRouteId, pictureId = picture.Id },
                pictureToReturn
            );
        }

        [HttpDelete("{pictureId}")]
        public async Task<IActionResult> DeletePicture([FromRoute] Guid touristRouteId, [FromRoute] int pictureId)
        {
            if (!await _touristRouteRepository.TouristRouteExistsAsync(touristRouteId))
                return NotFound($"找不到{touristRouteId}旅游路线");
            var picture = await _touristRouteRepository.GetTouristRoutePictureAsync(pictureId);
            _touristRouteRepository.DeleteTouristRoutePicture(picture);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }
    }
}

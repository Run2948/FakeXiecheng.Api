using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.Api.Dtos;
using FakeXiecheng.Api.Models;

namespace FakeXiecheng.Api
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<TouristRoute, TouristRouteDto>()
                .ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(src => src.OriginalPrice * (decimal)(src.DiscountPresent ?? 1))
                    )
                .ForMember(
                    dest => dest.TravelDays,
                    opt => opt.MapFrom(src => src.TravelDays.ToString())
                )
                .ForMember(
                    dest => dest.TripType,
                    opt => opt.MapFrom(src => src.TripType.ToString())
                )
                .ForMember(
                    dest => dest.DepartureCity,
                    opt => opt.MapFrom(src => src.DepartureCity.ToString())
                )
                // .ForMember(
                //     dest => dest.TouristRoutePictures,
                //     opt => opt.MapFrom((src, _, __, ctx) => ctx.Mapper.Map<List<TouristRoutePictureDto>>(src.TouristRoutePictures))
                //     )
                ;

            CreateMap<TouristRouteForCreationDto, TouristRoute>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => Guid.NewGuid()))
                // .ForMember(dest => dest.TouristRoutePictures,
                //     opt => opt.MapFrom((src, _, __, ctx) => ctx.Mapper.Map<List<TouristRoutePicture>>(src.TouristRoutePictures)))
                ;
            CreateMap<TouristRouteForUpdateDto, TouristRoute>();
            CreateMap<TouristRoute, TouristRouteForUpdateDto>();



            CreateMap<TouristRoutePicture, TouristRoutePictureDto>();
            CreateMap<TouristRoutePictureForCreationDto, TouristRoutePicture>();
            CreateMap<TouristRoutePictureDto, TouristRoutePictureForCreationDto>();

        }
    }
}

using AutoMapper;
using Hotels.Commands;
using Hotels.Domain.Models;
using Hotels.Dtos;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotels.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Hotel, HotelDto>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new LocationDto
                {
                    Longitude = src.Location.X,
                    Latitude = src.Location.Y
                }))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => decimal.Round(src.Price, 2)));
   
               CreateMap<HotelCommand, Hotel>()
                .ForMember(dest => dest.Location, opt => opt.Ignore());
        }
    }
}

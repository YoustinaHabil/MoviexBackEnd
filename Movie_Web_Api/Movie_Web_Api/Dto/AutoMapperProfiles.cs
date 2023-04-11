using System.Linq;
using AutoMapper;
using Movie_Web_Api.Models;

namespace Movie_Web_Api.Dto
{
    public class AutoMapperProfiles :Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<Actor, ActorDto>().ReverseMap();
            CreateMap<Cinema, CenimaDto>().ReverseMap();
            CreateMap<Movie, MoviesDto>().ReverseMap();
            CreateMap<Producer, ProducerDto>().ReverseMap();
            CreateMap<CartItem, CartItemRequestDTO>().ReverseMap();
            //CreateMap<City, CityUpdateDto>().ReverseMap();

            //CreateMap<Property, PropertyDto>().ReverseMap();

            //CreateMap<Photo, PhotoDto>().ReverseMap();

            //CreateMap<Property, PropertyListDto>()
            //    .ForMember(d => d.City, opt => opt.MapFrom(src => src.City.Name))
            //    .ForMember(d => d.Country, opt => opt.MapFrom(src => src.City.Country))
            //    .ForMember(d => d.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name))
            //    .ForMember(d => d.FurnishingType, opt => opt.MapFrom(src => src.FurnishingType.Name))
            //    .ForMember(d => d.Photo, opt => opt.MapFrom(src => src.Photos
            //                    .FirstOrDefault(p => p.IsPrimary).ImageUrl)); 


            //CreateMap<Property, PropertyDetailDto>()
            //    .ForMember(d => d.City, opt => opt.MapFrom(src => src.City.Name))
            //    .ForMember(d => d.Country, opt => opt.MapFrom(src => src.City.Country))
            //    .ForMember(d => d.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name))
            //    .ForMember(d => d.FurnishingType, opt => opt.MapFrom(src => src.FurnishingType.Name));


            //CreateMap<FurnishingType, KeyValuePairDto>();            

            //CreateMap<PropertyType, KeyValuePairDto>();            

        }
        
    }
}
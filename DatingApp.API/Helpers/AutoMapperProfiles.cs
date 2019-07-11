using System.Linq;
using AutoMapper;
using DatingApp.API.DTO.Photo;
using DatingApp.API.DTO.User;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDetailedDTO>()
                .ForMember(dest => dest.PhotoURL, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(src => src.DateBirth.CalculateAge());
                });
            CreateMap<User, UserForListDTO>()
                .ForMember(dest => dest.PhotoURL, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.MapFrom(src => src.DateBirth.CalculateAge());
                });
            ;
            CreateMap<Photo, PhotoForDetailedDTO>();
            CreateMap<UserForUpdateDTO, User>();
        }
    }
}
using AutoMapper;
using system_backend.Models;
using system_backend.Models.Dtos;

namespace system_backend
{
    public class MapConfig : Profile
    {
        public MapConfig() {

            CreateMap<RegisterModel, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, AdminDTO>().ReverseMap();
        }
    }
}

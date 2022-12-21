using AutoMapper;
using system_backend.Models;

namespace system_backend
{
    public class MapConfig : Profile
    {
        public MapConfig() {

            CreateMap<RegisterModel, ApplicationUser>().ReverseMap();
        }
    }
}

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
            CreateMap<RegisterAgentModel, ApplicationUser>().ReverseMap();
            CreateMap<RegisterModel, RegisterAgentModel>().ReverseMap();
            CreateMap<RegisterAgentModel, Agent>().ReverseMap();
            CreateMap<AgentServicePlaces, ServicePlacesDTO>().ReverseMap();
            CreateMap<ServicePlaces, ServicePlacesCreateDTO>().ReverseMap();
            CreateMap<ServicePlaces, ServicePlacesUpdateDTO>().ReverseMap();
            CreateMap<Agent, AgentDTO>().ReverseMap();
            CreateMap<Agent, AgentModel>().ReverseMap();
            CreateMap<Agent, AgentUpdateDTO>().ReverseMap();
            CreateMap<CouponsPayments, CouponDTO>().ReverseMap();
            CreateMap<ExpensesPayments, ExpenseDTO>().ReverseMap();

        }
    }
}

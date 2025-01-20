using AutoMapper;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO;

namespace Warehouse_Management.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequestDTO, ApplicationUser>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));
        }

    }
}

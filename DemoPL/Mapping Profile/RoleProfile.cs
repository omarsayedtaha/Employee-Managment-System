using AutoMapper;
using DemoPL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace DemoPL.Mapping_Profile
{
    public class RoleProfile : Profile 
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModel, IdentityRole>()
                .ForMember(d => d.Name, O => O.MapFrom(s=>s.RoleName)).ReverseMap();
        }

    }
}

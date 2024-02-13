using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            CreateMap<UserViewModel, ApplicationUser>().ReverseMap();
            CreateMap<RoleViewModel, IdentityRole>()
                .ForMember(d=>d.Name,O=>O.MapFrom(S=>S.RoleName))
                .ReverseMap();
        }
    }
}

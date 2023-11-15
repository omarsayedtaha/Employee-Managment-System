using AutoMapper;
using Demo.DAL.Model;
using DemoPL.ViewModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;

namespace DemoPL.Mapping_Profile
{
    public class UserProfile:Profile
    {

        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}

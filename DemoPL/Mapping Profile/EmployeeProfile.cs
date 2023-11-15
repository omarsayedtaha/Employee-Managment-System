using AutoMapper;
using Demo.DAL.Model;
using DemoPL.ViewModels;

namespace DemoPL.Mapping_Profile
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap(); 
        }
    }
}

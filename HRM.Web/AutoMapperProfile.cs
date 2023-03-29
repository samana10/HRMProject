using AutoMapper;
using HRM.DAL.Models;
using HRM.Web.ViewModel;

namespace HRM.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<Employee, EmployeeVM>().ForMember(des => des.EmployeeName, opt => opt.MapFrom(src => src.EmployeeName)).ReverseMap();
            CreateMap<EmployeeVM, Employee>().ReverseMap(); 
        }
    }
}

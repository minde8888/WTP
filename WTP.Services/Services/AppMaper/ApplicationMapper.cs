using AutoMapper;
using WTP.Domain.Dtos;
using WTP.Domain.Entities;
using WTP.Services.Services.Dtos;

namespace WTP.Data.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Manager, ManagerDto> ().ReverseMap();
            CreateMap<Manager, EmployeeInformationDto>().ReverseMap();
            CreateMap<Employee, EmployeeInformationDto>().ReverseMap();
            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Post, PostDto>().ReverseMap();
        }
    }
}

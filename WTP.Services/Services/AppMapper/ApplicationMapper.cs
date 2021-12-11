﻿using AutoMapper;
using WTP.Domain.Dtos;
using WTP.Domain.Dtos.Requests;
using WTP.Domain.Dtos.UpdateDto;
using WTP.Domain.Entities;
using WTP.Services.Services.Dtos;

namespace WTP.Services.Services.AppMapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Manager, ManagerDto>().ReverseMap();
            CreateMap<Manager, RequestManagerDto>().ReverseMap();
            CreateMap<Manager, UserRegistrationDto>().ReverseMap();
            CreateMap<RequestManagerDto, ReturnUserDto>().ReverseMap()
            .ForMember(m => m.Address, opt =>
            opt.MapFrom(m => m.Address));
            CreateMap<Manager, EmployeeInformationDto>().ReverseMap();

            CreateMap<Employee, EmployeeInformationDto>().ReverseMap();
            CreateMap<UserRegistrationDto, Employee>().ReverseMap();
            CreateMap<RequestEmployeeDto, ReturnUserDto>().ReverseMap()
            .ForMember(m => m.Address, opt =>
            opt.MapFrom(m => m.Address)); ;

            CreateMap<AddressDto, Address>().ReverseMap();

            CreateMap<Employee, EmployeeDto>().ReverseMap();

            CreateMap<Post, PostDto>().ReverseMap();
        }
    }
}
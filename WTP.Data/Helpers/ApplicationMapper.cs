using AutoMapper;
using WTP.Domain.Dtos;
using WTP.Domain.Entities;

namespace WTP.Data.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Manager, ManagerDto> ().ReverseMap();
            CreateMap<AddressDto, Address>().ReverseMap();
        }
    }
}

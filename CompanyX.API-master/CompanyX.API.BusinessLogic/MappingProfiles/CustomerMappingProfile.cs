using AutoMapper;
using CompanyX.API.DataAccess.Entities;
using CompanyX.API.DataAccess.Models;

namespace CompanyX.API.BusinessLogic.MappingProfiles
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<CreateCustomerDto, Customer>()
                .ForMember(dest => dest.HomeAddress, opt => opt.MapFrom(src => new CustomerHomeAddress
                {
                    StreetName = src.StreetName,
                    HouseNumber = src.HouseNumber.ToString(),
                    City = src.City,
                    PostalCode = src.PostalCode
                }));
        }
    }
}

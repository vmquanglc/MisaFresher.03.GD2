using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.BL.Automapper
{
    /// <summary>
    /// Class Customer dành cho automapper 
    /// </summary>
    /// Author: LeDucTiep (07/06/2023)
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            // Map Customer sang CustomerDto
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();
            // Map CustomerCreateDto sang Customer
            CreateMap<BasePage<Customer>, BasePage<CustomerDto>>();
            CreateMap<CustomerCreateDto, Customer>();
            // Map CustomerUpdateDto sang Customer
            CreateMap<CustomerUpdateDto, Customer>();

            CreateMap<CustomerUpdateDto, Customer>();

            CreateMap<CustomerCreateDto, CustomerDto>();

            CreateMap<CustomerUpdateDto, CustomerDto>();
        }
    }
}

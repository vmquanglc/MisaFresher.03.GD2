using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;
using static Dapper.SqlMapper;

namespace MISA.WebFresher2023.Demo.BL.Automapper
{
    /// <summary>
    /// Class CustomerGroup dành cho automapper 
    /// </summary>
    /// Author: LeDucTiep (07/06/2023)
    public class CustomerGroupProfile : Profile
    {
        public CustomerGroupProfile()
        {
            // Map CustomerGroup sang CustomerGroupDto
            CreateMap<CustomerGroup, CustomerGroupDto>();
            CreateMap<CustomerGroupDto, CustomerGroup>();
            // Map CustomerGroupCreateDto sang CustomerGroup
            CreateMap<CustomerGroupCreateDto, CustomerGroup>();
            // Map CustomerGroupUpdateDto sang CustomerGroup
            CreateMap<CustomerGroupUpdateDto, CustomerGroup>();

            CreateMap<CustomerGroupUpdateDto, CustomerGroup>();

            CreateMap<CustomerGroupCreateDto, CustomerGroupDto>();
            CreateMap<BasePage<CustomerGroup>, BasePage<CustomerGroupDto>>();

            CreateMap<CustomerGroupUpdateDto, CustomerGroupDto>();
        }
    }
}

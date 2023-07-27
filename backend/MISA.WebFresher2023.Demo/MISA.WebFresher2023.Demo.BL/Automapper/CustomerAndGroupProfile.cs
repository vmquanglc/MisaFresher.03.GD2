using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;
using static Dapper.SqlMapper;

namespace MISA.WebFresher2023.Demo.BL.Automapper
{
    /// <summary>
    /// Class CustomerAndGroup dành cho automapper 
    /// </summary>
    /// Author: LeDucTiep (07/06/2023)
    public class CustomerAndGroupProfile : Profile
    {
        public CustomerAndGroupProfile()
        {
            // Map CustomerAndGroup sang CustomerAndGroupDto
            CreateMap<CustomerAndGroup, CustomerAndGroupDto>();
            CreateMap<CustomerAndGroupDto, CustomerAndGroup>();
            // Map CustomerAndGroupCreateDto sang CustomerAndGroup
            CreateMap<CustomerAndGroupCreateDto, CustomerAndGroup>();
            // Map CustomerAndGroupUpdateDto sang CustomerAndGroup
            CreateMap<CustomerAndGroupUpdateDto, CustomerAndGroup>();

            CreateMap<CustomerAndGroupUpdateDto, CustomerAndGroup>();

            CreateMap<CustomerAndGroupCreateDto, CustomerAndGroupDto>();
            CreateMap<BasePage<CustomerAndGroup>, BasePage<CustomerAndGroupDto>>();

            CreateMap<CustomerAndGroupUpdateDto, CustomerAndGroupDto>();
        }
    }
}

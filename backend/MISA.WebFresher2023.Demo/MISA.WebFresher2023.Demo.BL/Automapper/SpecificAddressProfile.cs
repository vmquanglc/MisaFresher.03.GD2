using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.BL.Automapper
{
    /// <summary>
    /// Class SpecificAddress dành cho automapper 
    /// </summary>
    /// Author: LeDucTiep (07/06/2023)
    public class SpecificAddressProfile : Profile
    {
        public SpecificAddressProfile()
        {
            // Map SpecificAddress sang SpecificAddressDto
            CreateMap<SpecificAddress, SpecificAddressDto>();
            CreateMap<SpecificAddressDto, SpecificAddress>();

            // Map SpecificAddressCreateDto sang SpecificAddress
            CreateMap<SpecificAddressCreateDto, SpecificAddress>();
            // Map SpecificAddressUpdateDto sang SpecificAddress
            CreateMap<SpecificAddressUpdateDto, SpecificAddress>();

            CreateMap<BasePage<SpecificAddress>, BasePage<SpecificAddressDto>>();
            CreateMap<SpecificAddressUpdateDto, SpecificAddress>();

            CreateMap<SpecificAddressCreateDto, SpecificAddressDto>();

            CreateMap<SpecificAddressUpdateDto, SpecificAddressDto>();


        }
    }
}

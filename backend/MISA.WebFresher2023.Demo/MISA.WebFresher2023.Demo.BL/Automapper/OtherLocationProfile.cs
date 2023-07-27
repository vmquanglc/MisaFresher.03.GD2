using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.BL.Automapper
{
    /// <summary>
    /// Class OtherLocation dành cho automapper 
    /// </summary>
    /// Author: LeDucTiep (07/06/2023)
    public class OtherLocationProfile : Profile
    {
        public OtherLocationProfile()
        {
            // Map OtherLocation sang OtherLocationDto
            CreateMap<OtherLocation, OtherLocationDto>();
            
            CreateMap<OtherLocationDto, OtherLocation>();
            // Map OtherLocationCreateDto sang OtherLocation
            CreateMap<OtherLocationCreateDto, OtherLocation>();
            // Map OtherLocationUpdateDto sang OtherLocation
            CreateMap<OtherLocationUpdateDto, OtherLocation>();

            CreateMap<BasePage<OtherLocation>, BasePage<OtherLocationDto>>();
            CreateMap<OtherLocationUpdateDto, OtherLocation>();

            CreateMap<OtherLocationCreateDto, OtherLocationDto>();

            CreateMap<OtherLocationUpdateDto, OtherLocationDto>();
        }
    }
}

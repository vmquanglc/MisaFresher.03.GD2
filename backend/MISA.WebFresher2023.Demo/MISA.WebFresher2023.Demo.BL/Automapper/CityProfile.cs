using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.BL.Automapper
{
    /// <summary>
    /// Class dành cho automapper 
    /// </summary>
    /// Author: LeDucTiep (07/06/2023)
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            // Map City sang CityDto
            CreateMap<City, CityDto>();
            CreateMap<CityDto, City>();
            // Map CityCreateDto sang City
            CreateMap<CityCreateDto, City>();
            // Map CityUpdateDto sang City
            CreateMap<CityUpdateDto, City>();

            CreateMap<CityUpdateDto, City>();

            CreateMap<BasePage<City>, BasePage<CityDto>>();
            CreateMap<CityCreateDto, CityDto>();

            CreateMap<CityUpdateDto, CityDto>();
        }
    }
}

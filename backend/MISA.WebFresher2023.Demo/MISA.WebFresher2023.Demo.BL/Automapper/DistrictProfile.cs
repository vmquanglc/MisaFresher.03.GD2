using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;
using static Dapper.SqlMapper;

namespace MISA.WebFresher2023.Demo.BL.Automapper
{
    /// <summary>
    /// Class District dành cho automapper 
    /// </summary>
    /// Author: LeDucTiep (07/06/2023)
    public class DistrictProfile : Profile
    {
        public DistrictProfile()
        {
            // Map District sang DistrictDto
            CreateMap<District, DistrictDto>();
            CreateMap<DistrictDto, District>();
            // Map DistrictCreateDto sang District
            CreateMap<DistrictCreateDto, District>();
            // Map DistrictUpdateDto sang District
            CreateMap<DistrictUpdateDto, District>();
            CreateMap<BasePage<District>, BasePage<DistrictDto>>();

            CreateMap<DistrictUpdateDto, District>();

            CreateMap<DistrictCreateDto, DistrictDto>();

            CreateMap<DistrictUpdateDto, DistrictDto>();
        }
    }
}

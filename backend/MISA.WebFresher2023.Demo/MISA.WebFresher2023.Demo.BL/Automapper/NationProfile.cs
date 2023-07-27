using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.BL.Automapper
{
    /// <summary>
    /// Class Nation dành cho automapper 
    /// </summary>
    /// Author: LeDucTiep (07/06/2023)
    public class NationProfile : Profile
    {
        public NationProfile()
        {
            // Map Nation sang NationDto
            CreateMap<Nation, NationDto>();
            CreateMap<NationDto, Nation>();
            // Map NationCreateDto sang Nation
            CreateMap<NationCreateDto, Nation>();
            // Map NationUpdateDto sang Nation
            CreateMap<NationUpdateDto, Nation>();

            CreateMap<NationUpdateDto, Nation>();

            CreateMap<BasePage<Nation>, BasePage<NationDto>>();
            CreateMap<NationCreateDto, NationDto>();

            CreateMap<NationUpdateDto, NationDto>();
        }
    }
}

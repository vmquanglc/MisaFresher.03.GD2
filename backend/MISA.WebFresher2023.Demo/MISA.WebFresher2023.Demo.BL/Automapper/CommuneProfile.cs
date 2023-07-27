using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;
using static Dapper.SqlMapper;

namespace MISA.WebFresher2023.Demo.BL.Automapper
{
    /// <summary>
    /// Class dành cho automapper 
    /// </summary>
    /// Author: LeDucTiep (07/06/2023)
    public class CommuneProfile : Profile
    {
        public CommuneProfile()
        {
            // Map Commune sang CommuneDto
            CreateMap<Commune, CommuneDto>();
            CreateMap<CommuneDto, Commune>();
            // Map CommuneCreateDto sang Commune
            CreateMap<CommuneCreateDto, Commune>();
            // Map CommuneUpdateDto sang Commune
            CreateMap<CommuneUpdateDto, Commune>();

            CreateMap<CommuneUpdateDto, Commune>();

            CreateMap<CommuneCreateDto, CommuneDto>();
            CreateMap<BasePage<Commune>, BasePage<CommuneDto>>();

            CreateMap<CommuneUpdateDto, CommuneDto>();
        }
    }
}

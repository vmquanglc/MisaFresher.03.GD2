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
    public class AccountPropertyProfile : Profile
    {
        public AccountPropertyProfile()
        {
            // Map AccountProperty sang AccountPropertyDto
            CreateMap<AccountProperty, AccountPropertyDto>();
            CreateMap<AccountPropertyDto, AccountProperty>();
            // Map AccountPropertyCreateDto sang AccountProperty
            CreateMap<AccountPropertyCreateDto, AccountProperty>();
            // Map AccountPropertyUpdateDto sang AccountProperty
            CreateMap<AccountPropertyUpdateDto, AccountProperty>();

            CreateMap<AccountPropertyUpdateDto, AccountProperty>();

            CreateMap<AccountPropertyCreateDto, AccountPropertyDto>();

            CreateMap<AccountPropertyUpdateDto, AccountPropertyDto>();
            CreateMap<BasePage<AccountProperty>, BasePage<AccountPropertyDto>>();
        }
    }
}

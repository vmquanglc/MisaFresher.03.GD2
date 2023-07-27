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
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            // Map Account sang AccountDto
            CreateMap<Account, AccountDto>();
            CreateMap<AccountDto, Account>();
            // Map AccountCreateDto sang Account
            CreateMap<AccountCreateDto, Account>();
            // Map AccountUpdateDto sang Account
            CreateMap<AccountUpdateDto, Account>();

            CreateMap<AccountUpdateDto, Account>();

            CreateMap<AccountCreateDto, AccountDto>();

            CreateMap<BasePage<Account>, BasePage<AccountDto>>();

            CreateMap<AccountUpdateDto, AccountDto>();
        }
    }
}

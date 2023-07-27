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
    public class BankAccountProfile : Profile
    {
        public BankAccountProfile()
        {
            // Map BankAccount sang BankAccountDto
            CreateMap<BankAccount, BankAccountDto>();
            CreateMap<BankAccountDto, BankAccount>();
            // Map BankAccountCreateDto sang BankAccount
            CreateMap<BankAccountCreateDto, BankAccount>();
            // Map BankAccountUpdateDto sang BankAccount
            CreateMap<BankAccountUpdateDto, BankAccount>();

            CreateMap<BankAccountUpdateDto, BankAccount>();

            CreateMap<BasePage<BankAccount>, BasePage<BankAccountDto>>();
            CreateMap<BankAccountCreateDto, BankAccountDto>();

            CreateMap<BankAccountUpdateDto, BankAccountDto>();
        }
    }
}

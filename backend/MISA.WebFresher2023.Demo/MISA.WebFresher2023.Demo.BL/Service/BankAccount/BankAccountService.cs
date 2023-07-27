using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Repository;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public class BankAccountService : BaseService<BankAccount, BankAccountDto, BankAccountCreateDto, BankAccountUpdateDto>, IBankAccountService
    {
        public BankAccountService(IBankAccountRepository bankAccountRepository,
            IMSDatabase msDatabase,
            IMapper mapper) : base(msDatabase, bankAccountRepository, mapper)
        {
        }
    }
}

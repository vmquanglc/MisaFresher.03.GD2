using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.BL.Service;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public interface IBankAccountService : IBaseService<BankAccountDto, BankAccountCreateDto, BankAccountUpdateDto>
    {
    }
}

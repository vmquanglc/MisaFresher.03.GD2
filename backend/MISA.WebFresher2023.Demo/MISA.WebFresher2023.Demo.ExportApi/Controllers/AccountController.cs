using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.BL.Service;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.WebFresher2023.Demo.Controllers
{
    [Route("api/v1/[controller]s")]
    public class AccountController : BaseController<Account, AccountDto, AccountCreateDto, AccountUpdateDto>
    {
        protected readonly IAccountService _accountService;

        public AccountController(IAccountService accountService
            ) : base(accountService)
        {
            _accountService = accountService;
        }
    }
}

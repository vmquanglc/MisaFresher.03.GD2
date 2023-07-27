using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Repository;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public class AccountPropertyService : BaseService<AccountProperty, AccountPropertyDto, AccountPropertyCreateDto, AccountPropertyUpdateDto>, IAccountPropertyService
    {
        public AccountPropertyService(IAccountPropertyRepository accountPropertyRepository, 
            IMSDatabase msDatabase,
            IMapper mapper) : base(msDatabase, accountPropertyRepository, mapper)
        {
        }
    }
}

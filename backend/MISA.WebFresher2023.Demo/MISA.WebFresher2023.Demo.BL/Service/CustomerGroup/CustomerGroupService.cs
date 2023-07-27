using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Repository;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public class CustomerGroupService : BaseService<CustomerGroup, CustomerGroupDto, CustomerGroupCreateDto, CustomerGroupUpdateDto>, ICustomerGroupService
    {
        public CustomerGroupService(ICustomerGroupRepository customerGroupRepository, 
            IMSDatabase msDatabase,
            IMapper mapper) : base(msDatabase, customerGroupRepository, mapper)
        {
        }
    }
}

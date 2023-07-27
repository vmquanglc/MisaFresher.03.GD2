using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Repository;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public class CustomerAndGroupService : BaseService<CustomerAndGroup, CustomerAndGroupDto, CustomerAndGroupCreateDto, CustomerAndGroupUpdateDto>, ICustomerAndGroupService
    {
        public CustomerAndGroupService(ICustomerAndGroupRepository customerAndGroupRepository,
            IMSDatabase msDatabase,
            IMapper mapper) : base(msDatabase, customerAndGroupRepository, mapper)
        {
        }

        public async Task<List<Guid>> GetCustomerGroupIdByCustomerIdAsync(Guid id)
        {
            try
            {
                return await ((CustomerAndGroupRepository)_baseRepository).GetCustomerGroupIdByCustomerIdAsync(id);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }
    }
}

using MISA.WebFresher2023.Demo.DL.Entity;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public interface ICustomerAndGroupRepository : IBaseRepository<CustomerAndGroup>
    {
        /// <summary>
        /// Hàm lấy danh sách nhóm khách hàng theo id khách hàng
        /// </summary>
        /// <param name="customerId">Id khách hàng</param>
        /// <returns>Danh sách Id nhóm khách hàng</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<List<Guid>> GetCustomerGroupIdByCustomerIdAsync(Guid customerId);

        /// <summary>
        /// Hàm kiểm tra bản ghi có tồn tại không
        /// </summary>
        /// <param name="customerGroupId">Id của nhóm khách hàng</param>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<bool> CheckExistedAsync(Guid customerGroupId, Guid customerId);

        /// <summary>
        /// Hàm xóa các bản ghi không nằm trong danh sách 
        /// </summary>
        /// <param name="customerGroupList">Danh sách id các customerGroup</param>
        /// <param name="customerId">id của khách hàng</param>
        /// <returns>Task</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task DeleteNotInAsync(string customerGroupList, Guid customerId);
    }

}

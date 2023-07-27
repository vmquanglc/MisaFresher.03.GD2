using MISA.WebFresher2023.Demo.DL.Entity;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public interface IOtherLocationRepository : IBaseRepository<OtherLocation>
    {
        /// <summary>
        /// Hàm lấy danh sách địa chỉ theo id của khách hàng
        /// </summary>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns>OtherLocation</returns>
        Task<OtherLocation> GetByCustomerIdAsync(Guid customerId);
    }
}

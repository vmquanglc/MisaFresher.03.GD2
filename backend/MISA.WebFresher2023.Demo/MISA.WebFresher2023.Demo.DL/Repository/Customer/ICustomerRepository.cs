using MISA.WebFresher2023.Demo.DL.Entity;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        /// <summary>
        /// Hàm kiểm tra mã đang chỉnh sửa có bị trùng không
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <param name="itsCode">Mã khách hàng trước khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<bool> CheckEditCodeDuplicatedAsync(string customerCode, string itsCode);

        /// <summary>
        /// Hàm kiểm tra mã khách hàng đã tồn tại chưa
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<bool> CheckExistedCodeAsync(string customerCode);

        /// <summary>
        /// Hàm lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<string> GetNewCustomerCodeAsync();
    }
}

using MISA.WebFresher2023.Demo.DL.Entity;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public interface IBankAccountRepository : IBaseRepository<BankAccount>
    {
        /// <summary>
        /// Hàm xóa các bản ghi không có id trong danh sách
        /// </summary>
        /// <param name="bankAccountNumberList">Danh sách id của các bản ghi không cần xóa </param>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns>Task</returns>
        /// Author: LeDucTiep (08/05/2023)
        Task DeleteNotInAsync(string bankAccountNumberList, Guid customerId);

        /// <summary>
        /// Hàm lấy danh sách tài khoản ngân hàng theo id của khách hàng
        /// </summary>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns>Danh sách tài khoản ngân hàng</returns>
        /// Author: LeDucTiep (08/05/2023)
        Task<List<BankAccount>> GetByCustomerIdAsync(Guid customerId);
    }
}

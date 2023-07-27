using MISA.WebFresher2023.Demo.BL.Dto;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public interface ICustomerService : IBaseService<CustomerDto, CustomerCreateDto, CustomerUpdateDto>
    {
        /// <summary>
        /// Hàm kiểm tra mã code đã tồn tại chưa
        /// </summary>
        /// <param name="code">Mã code cần kiểm tra</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<bool> CheckCodeDuplicatedAsync(string code);

        /// <summary>
        /// Hàm kiểm tra mã code đang sửa có bị trùng không
        /// </summary>
        /// <param name="customerCode">Mã code đang sửa</param>
        /// <param name="itsCode">Mã trước khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<bool> CheckEditCodeDuplicatedAsync(string customerCode, string itsCode);

        /// <summary>
        /// Hàm lấy mã mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<string> GetNewCustomerCodeAsync();
    }
}

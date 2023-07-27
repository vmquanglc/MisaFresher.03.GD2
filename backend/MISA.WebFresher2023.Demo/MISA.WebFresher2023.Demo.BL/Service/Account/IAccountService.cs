using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.BL.Service;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public interface IAccountService : IBaseService<AccountDto, AccountCreateDto, AccountUpdateDto>
    {
        /// <summary>
        /// Hàm kiểm tra số tài khoản đã tồn tại chưa
        /// </summary>
        /// <param name="code">Mã code cần kiểm tra</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<bool> CheckCodeDuplicatedAsync(string code);

        /// <summary>
        /// Hàm kiểm tra số tải khoản đang sửa có bị trùng không
        /// </summary>
        /// <param name="accountCode">Số tài khoản đang sửa</param>
        /// <param name="itsCode">Số tải khoản trước khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<bool> CheckEditCodeDuplicatedAsync(string accountCode, string itsCode);

        /// <summary>
        /// Hàm lấy một trang tài khoản 
        /// </summary>
        /// <param name="accountPagingArgument">Tham số để phân trang</param>
        /// <returns>Trang tài khoản</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<BasePage<AccountDto>> GetPageAsync(AccountPagingArgument accountPagingArgument);
    }
}

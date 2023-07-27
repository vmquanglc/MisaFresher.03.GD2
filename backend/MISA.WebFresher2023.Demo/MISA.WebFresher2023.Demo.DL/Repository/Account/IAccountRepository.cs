using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        /// <summary>
        /// Hàm kiểm tra account này có phải là account cha hay không
        /// </summary>
        /// <param name="id">Id của account</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<bool> CheckIsParentAsync(Guid id);

        /// <summary>
        /// Hàm kiểm tra account này có phải là account ông không 
        /// </summary>
        /// <param name="id">Id của account</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<bool> CheckIsGrandpaAsync(Guid id);

        /// <summary>
        /// Hàm kiểm tra số tài khoản đã thay đổi chưa
        /// </summary>
        /// <param name="id">Id của account</param>
        /// <param name="accountCode">Số tài khoản</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<bool> CheckIsChangedAccountCodeAsync(Guid id, string accountCode);

        /// <summary>
        /// Hàm kiểm tra khóa ngoại đến account cha đã thay đổi
        /// </summary>
        /// <param name="id">Id của của bản ghi</param>
        /// <param name="parentId">Id khóa ngoại trước khi thay đổi</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<bool> CheckIsChangedParentIdAsync(Guid id, Guid? parentId);

        /// <summary>
        /// Hàm Cập nhật lại MisaCode
        /// </summary>
        /// <param name="oldMisaCode">MisaCode cũ </param>
        /// <param name="newMisaCode">MisaCode mới</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<bool> UpdateMisaCodeAsync(string oldMisaCode, string newMisaCode);

        /// <summary>
        /// Hàm tính toán lại MisaCode
        /// </summary>
        /// <param name="id">Id của bản ghi cần tính toán lại</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<bool> RecalculateMisaCodeAsync(Guid id);

        /// <summary>
        /// Hàm kiểm tra số tài khoản cập nhật sẽ làm trùng với số tài khoản đã tồn tại
        /// </summary>
        /// <param name="id">Id của tài khoản</param>
        /// <param name="accountCode">Số tài khoản sau khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<bool> CheckUpdateCodeBeDublicatedAsync(Guid id, string accountCode);

        /// <summary>
        /// Hàm lấy trang tài khoản
        /// </summary>
        /// <typeparam name="TEntityInPage"></typeparam>
        /// <param name="accountPagingArgument">Tham số để lọc</param>
        /// <returns>BasePage<TEntityInPage></returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<BasePage<TEntityInPage>> GetPageAsync<TEntityInPage>(AccountPagingArgument accountPagingArgument);

        /// <summary>
        /// Hàm kiểm tra sửa mã code có bị trùng không 
        /// </summary>
        /// <param name="accountCode">Mã sau khi sửa</param>
        /// <param name="itsCode">Mã trước khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<bool> CheckEditCodeDuplicatedAsync(string accountCode, string itsCode);

        /// <summary>
        /// Hàm kiểm tra số tài khoản cần thêm có bị trùng không 
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<bool> CheckExistedCodeAsync(string accountCode);
    }
}

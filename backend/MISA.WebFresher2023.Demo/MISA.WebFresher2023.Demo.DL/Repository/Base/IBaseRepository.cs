using MISA.WebFresher2023.Demo.DL.Model;
using System.Data.Common;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public interface IBaseRepository<TEntity>
    {
        /// <summary>
        /// Hàm lấy một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>TEntity</returns>
        /// Created by: LeDucTiep (21/05/2023)
        Task<TEntity?> GetAsync(Guid id);

        /// <summary>
        /// Hàm cập nhật một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="entity">Giá trị của bản ghi</param>
        /// Created by: LeDucTiep (21/05/2023)
        Task<int> UpdateAsync(Guid id, TEntity entity);

        /// <summary>
        /// Hàm xóa một bàn ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// Created by: LeDucTiep (21/05/2023)
        Task<int> DeleteAsync(Guid id);

        /// <summary>
        /// Hàm xóa các bàn ghi
        /// </summary>
        /// <param name="arrayId">Id của các bản ghi</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// Created by: LeDucTiep (21/05/2023)
        Task<int> DeleteManyAsync(Guid[] arrayId);

        /// <summary>
        /// Hàm tạo một bàn ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// Created by: LeDucTiep (22/05/2023)
        Task<int> PostAsync(TEntity entity, Guid newId);

        /// <summary>
        /// Hàm lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Author: LeDucTiep (27/05/2023)
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Hàm kiểm tra có tồn tại không
        /// </summary>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (08/05/2023)
        Task<bool> CheckExistedAsync(Guid id, string table = "");

        /// <summary>
        /// Hàm lấy trang bản ghi
        /// </summary>
        /// <typeparam name="TEntityInPage">Loại bản ghi trong trang</typeparam>
        /// <param name="basePagingArgument">Tham số để phân trang</param>
        /// <returns>BasePage<TEntityInPage></returns>
        /// Author: LeDucTiep (08/05/2023)
        Task<BasePage<TEntityInPage>> GetPageAsync<TEntityInPage>(BasePagingArgument basePagingArgument);

        /// <summary>
        /// Hàm xuất excel
        /// </summary>
        /// <typeparam name="TEntityExport">Loại bản ghi để xuất</typeparam>
        /// <returns>Danh sách bản ghi cần xuất</returns>
        /// Author: LeDucTiep (08/05/2023)
        Task<IEnumerable<TEntityExport>> GetExportAsync<TEntityExport>();
    }
}

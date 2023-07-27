using ClosedXML.Excel;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public interface IBaseService<TEntityDto, TEntityCreateDto, TEntityUpdateDto>
    {
        /// <summary>
        /// Hàm thêm một bản ghi 
        /// </summary>
        /// <param name="entityCreateDto"></param>
        /// <returns>TEntity</returns>
        /// Author: LeDucTiep (23/05/2023)
        Task<TEntityDto> PostAsync(TEntityCreateDto entityCreateDto);

        /// <summary>
        /// Lấy một bản ghi theo Id 
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>Bản ghi</returns>
        /// Author: LeDucTiep (23/05/2023)
        Task<TEntityDto?> GetAsync(Guid id);

        /// <summary>
        /// Sửa một bản ghi 
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="updateDto">Nội dung sửa</param>
        /// <returns></returns>
        /// Author: LeDucTiep (23/05/2023)
        Task<int> UpdateAsync(Guid id, TEntityUpdateDto updateDto);

        /// <summary>
        /// Hàm xóa một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi cần xóa</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// Author: LeDucTiep (23/05/2023)
        Task<int> DeleteAsync(Guid id);

        /// <summary>
        /// Hàm xóa nhiều bản ghi
        /// </summary>
        /// <param name="arrayId">Id của các bản ghi cần xóa</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// Author: LeDucTiep (23/05/2023)
        Task<int> DeleteManyAsync(Guid[] arrayId);

        /// <summary>
        /// Hàm lấy tất cả bản ghi 
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Author: LeDucTiep (23/05/2023)
        Task<IEnumerable<TEntityDto>> GetAllAsync();

        /// <summary>
        /// Hàm kiểm tra bản ghi cần thêm 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        Task<List<int>> PostValidate(TEntityDto entity);

        /// <summary>
        /// Hàm kiểm tra thông tin sửa 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        Task<List<int>> UpdateValidate(Guid id, TEntityDto entity);

        /// <summary>
        /// Hàm kiểm tra Id xóa 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        Task<List<int>> DeleteValidate(Guid id);

        /// <summary>
        /// hàm lấy trang bản ghi
        /// </summary>
        /// <param name="basePagingArgument">Tham số để phân trang</param>
        /// <returns>Trang bản ghi</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<BasePage<TEntityDto>> GetPageAsync(BasePagingArgument basePagingArgument);

        /// <summary>
        /// Hàm xuất khẩu dữ liệu thành file excel
        /// </summary>
        /// <param name="baseExportArgument">Tham số để xuất khẩu</param>
        /// <returns>Excel</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<XLWorkbook> ExportAsync(BaseExportArgument baseExportArgument);
    }
}

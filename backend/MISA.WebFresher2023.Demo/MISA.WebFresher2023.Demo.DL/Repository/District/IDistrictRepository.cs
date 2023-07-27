using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public interface IDistrictRepository : IBaseRepository<District>
    {
        /// <summary>
        /// Hàm lấy trang
        /// </summary>
        /// <typeparam name="TEntityInPage">Loại bản ghi trong trang</typeparam>
        /// <param name="districtPagingArgument">Tham số để phân trang</param>
        /// <returns>BasePage<TEntityInPage></returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<BasePage<TEntityInPage>> GetPageAsync<TEntityInPage>(DistrictPagingArgument districtPagingArgument);

        /// <summary>
        /// Hàm lấy một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="cityId">Mã thành phố</param>
        /// <returns>District?</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<District?> GetAsync(Guid id, Guid? cityId = null);
    }
}

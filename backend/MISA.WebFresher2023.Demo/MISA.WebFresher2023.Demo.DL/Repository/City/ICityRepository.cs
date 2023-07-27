using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public interface ICityRepository : IBaseRepository<City>
    {
        /// <summary>
        /// Hàm lấy trang bản ghi
        /// </summary>
        /// <typeparam name="TEntityInPage">Loại bản ghi trong trang</typeparam>
        /// <param name="cityPagingArgument">Tham số để phân trang</param>
        /// <returns>Trang bản ghi</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<BasePage<TEntityInPage>> GetPageAsync<TEntityInPage>(CityPagingArgument cityPagingArgument);

        /// <summary>
        /// Hàm lấy một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="nationId">Id quốc gia</param>
        /// <returns>Thành phố</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<City?> GetAsync(Guid id, Guid? nationId = null);
    }
}

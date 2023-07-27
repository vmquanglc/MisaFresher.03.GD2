using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public interface ICommuneRepository : IBaseRepository<Commune>
    {
        /// <summary>
        /// Hàm lấy trang bản ghi 
        /// </summary>
        /// <typeparam name="TEntityInPage">Loại bản ghi trong trang </typeparam>
        /// <param name="communePagingArgument">Tham số để phân trang</param>
        /// <returns>Trang bản ghi</returns>
        /// Author: LeDucTiep (12/04/2023)
        Task<BasePage<TEntityInPage>> GetPageAsync<TEntityInPage>(CommunePagingArgument communePagingArgument);

        /// <summary>
        /// Hàm lấy một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="districtId">Id của huyện</param>
        /// <returns>Commune?</returns>
        /// Author: LeDucTiep (12/04/2023)
        Task<Commune?> GetAsync(Guid id, Guid? districtId = null);
    }
}

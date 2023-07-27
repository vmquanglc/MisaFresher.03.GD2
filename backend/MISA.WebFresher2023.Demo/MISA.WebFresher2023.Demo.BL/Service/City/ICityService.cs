using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.BL.Service;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public interface ICityService : IBaseService<CityDto, CityCreateDto, CityUpdateDto>
    {
        /// <summary>
        /// Hàm lấy trang bản ghi
        /// </summary>
        /// <param name="basePagingArgument">Tham số để phân trang</param>
        /// <returns>Trang thành phố</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<BasePage<CityDto>> GetPageAsync(CityPagingArgument basePagingArgument);

        /// <summary>
        /// Hàm lấy một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="nationId">Id quốc gia</param>
        /// <returns>Thành phố</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<CityDto?> GetAsync(Guid id, Guid? nationId = null);
    }
}

using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public interface IDistrictService : IBaseService<DistrictDto, DistrictCreateDto, DistrictUpdateDto>
    {
        /// <summary>
        /// Hàm lấy một trang
        /// </summary>
        /// <param name="districtPagingArgument">Tham số để phân trang</param>
        /// <returns>Trang bản ghi</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<BasePage<DistrictDto>> GetPageAsync(DistrictPagingArgument districtPagingArgument);

        /// <summary>
        /// Hàm lấy một bản ghi 
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="cityId">Mã thành phố</param>
        /// <returns>Bản ghi</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<DistrictDto?> GetAsync(Guid id, Guid? cityId = null);
    }
}

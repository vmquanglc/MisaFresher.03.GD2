using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Model;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public interface ICommuneService : IBaseService<CommuneDto, CommuneCreateDto, CommuneUpdateDto>
    {
        /// <summary>
        /// Hàm lấy trang bản ghi
        /// </summary>
        /// <param name="communePagingArgument">Tham số để phân trang</param>
        /// <returns>Trang bản ghi</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<BasePage<CommuneDto>> GetPageAsync(CommunePagingArgument communePagingArgument);

        /// <summary>
        /// Hàm lấy một bản ghi 
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="districtId">Id của Huyện</param>
        /// <returns>Xã</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<CommuneDto?> GetAsync(Guid id, Guid? districtId = null);
    }
}

using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.BL.Service;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.WebFresher2023.Demo.Controllers
{
    [Route("api/v1/[controller]s")]
    public class DistrictController : BaseController<District, DistrictDto, DistrictCreateDto, DistrictUpdateDto>
    {
        public IDistrictService _districtService;
        public DistrictController(IDistrictService districtService
            ) : base(districtService)
        {
            _districtService = districtService;
        }

        /// <summary>
        /// API lấy tất cả danh sách chức vụ
        /// </summary>
        /// <returns>Danh sách chức vụ</returns>
        /// Author: LeDucTiep (23/05/2023)
        [HttpGet]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<DistrictDto> list = await _baseService.GetAllAsync();

            if (!list.Any())
                return NoContent();

            return Ok(list);
        }

        /// <summary>
        /// API phân trang theo id của thành phố 
        /// </summary>
        /// <param name="basePagingArgument">Tham số để phân trang </param>
        /// <param name="id">Id thành phố</param>
        /// <returns>Trang huyện</returns>
        /// Author: LeDucTiep (12/07/2023)
        [Route("paging/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPageAsync([FromQuery] BasePagingArgument basePagingArgument, Guid id)
        {
            DistrictPagingArgument districtPagingArgument = new();
            districtPagingArgument.PageSize = basePagingArgument.PageSize;
            districtPagingArgument.PageNumber = basePagingArgument.PageNumber;
            districtPagingArgument.SearchTerm = basePagingArgument.SearchTerm;
            districtPagingArgument.CityId = id;

            BasePage<DistrictDto> page = await _districtService.GetPageAsync(districtPagingArgument);

            return Ok(page);
        }

        /// <summary>
        /// API lấy thông tin một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi </param>
        /// <param name="cityId">Id thành phố</param>
        /// <returns>Huyện</returns>
        /// Author: LeDucTiep (12/07/2023)
        [HttpGet("{cityId}/{id}")]
        public virtual async Task<IActionResult> GetAsync(Guid id, Guid cityId)
        {
            DistrictDto? entityDto = await _districtService.GetAsync(id, cityId);

            if (entityDto == null)
                return NoContent();

            return Ok(entityDto);
        }

        /// <summary>
        /// API Lấy thông tin huyện theo id
        /// </summary>
        /// <param name="id">Id huyện</param>
        /// <returns>Bản ghi</returns>
        /// Author: LeDucTiep (12/07/2023)
        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(Guid id)
        {
            DistrictDto? entityDto = await _districtService.GetAsync(id);

            if (entityDto == null)
                return NoContent();

            return Ok(entityDto);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.BL.Service;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.WebFresher2023.Demo.Controllers
{
    [Route("api/v1/[controller]s")]
    public class CityController : BaseController<City, CityDto, CityCreateDto, CityUpdateDto>
    {
        public ICityService _cityService;
        public CityController(ICityService cityService
            ) : base(cityService)
        {
            _cityService = cityService;
        }

        /// <summary>
        /// API lấy tất cả danh sách chức vụ
        /// </summary>
        /// <returns>Danh sách chức vụ</returns>
        /// Author: LeDucTiep (23/05/2023)
        [HttpGet]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<CityDto> list = await _baseService.GetAllAsync();

            if (!list.Any())
                return NoContent();

            return Ok(list);
        }

        /// <summary>
        /// API phân trang thành phố
        /// </summary>
        /// <param name="basePagingArgument">Tham số để phân trang</param>
        /// <param name="id">Id của quốc gia</param>
        /// <returns>Trang thành phố</returns>
        /// Author: LeDucTiep (12/07/2023)
        [Route("paging/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPageAsync([FromQuery] BasePagingArgument basePagingArgument, Guid id)
        {
            CityPagingArgument cityPagingArgument = new();
            cityPagingArgument.PageSize = basePagingArgument.PageSize;
            cityPagingArgument.PageNumber = basePagingArgument.PageNumber;
            cityPagingArgument.SearchTerm = basePagingArgument.SearchTerm;
            cityPagingArgument.NationId = id;

            BasePage<CityDto> page = await _cityService.GetPageAsync(cityPagingArgument);

            return Ok(page);
        }

        /// <summary>
        /// API lấy thông tin của thành phố
        /// </summary>
        /// <param name="id">Id của thành phố</param>
        /// <param name="nationId">Id của quốc gia</param>
        /// <returns>Thành phố</returns>
        [HttpGet("{nationId}/{id}")]
        public async Task<IActionResult> GetAsync(Guid id, Guid nationId = default)
        {
            CityDto? entityDto = await _cityService.GetAsync(id, nationId);

            if (entityDto == null)
                return NoContent();

            return Ok(entityDto);
        }

        /// <summary>
        /// API lấy thông tin thành phố
        /// </summary>
        /// <param name="id">Id thành phố</param>
        /// <returns>Thông tin thành phố</returns>
        /// Author: LeDucTiep (12/07/2023)
        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(Guid id)
        {
            CityDto? entityDto = await _cityService.GetAsync(id);

            if (entityDto == null)
                return NoContent();

            return Ok(entityDto);
        }
    }
}

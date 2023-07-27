using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.BL.Service;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.WebFresher2023.Demo.Controllers
{
    [Route("api/v1/[controller]s")]
    public class CommuneController : BaseController<Commune, CommuneDto, CommuneCreateDto, CommuneUpdateDto>
    {
        public ICommuneService _communeService;
        public CommuneController(ICommuneService communeService
            ) : base(communeService)
        {
            _communeService = communeService;
        }

        /// <summary>
        /// API lấy tất cả danh sách chức vụ
        /// </summary>
        /// <returns>Danh sách chức vụ</returns>
        /// Author: LeDucTiep (23/05/2023)
        [HttpGet]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<CommuneDto> list = await _baseService.GetAllAsync();

            if (!list.Any())
                return NoContent();

            return Ok(list);
        }

        /// <summary>
        /// API phân trang Xã bằng id Huyện
        /// </summary>
        /// <param name="basePagingArgument">Tham số để phân trang</param>
        /// <param name="id">Id của Huyện</param>
        /// <returns>Trang xã</returns>
        /// Author: LeDucTiep (12/07/2023)
        [Route("paging/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetPageAsync([FromQuery] BasePagingArgument basePagingArgument, Guid id)
        {
            CommunePagingArgument communePagingArgument = new();
            communePagingArgument.PageSize = basePagingArgument.PageSize;
            communePagingArgument.PageNumber = basePagingArgument.PageNumber;
            communePagingArgument.SearchTerm = basePagingArgument.SearchTerm;
            communePagingArgument.DistrictId = id;

            BasePage<CommuneDto> page = await _communeService.GetPageAsync(communePagingArgument);

            return Ok(page);
        }

        /// <summary>
        /// API lấy thông tin của xã
        /// </summary>
        /// <param name="id">Id của xã</param>
        /// <param name="districtId">Id của huyện</param>
        /// <returns>Xã</returns>
        /// Author: LeDucTiep (12/07/2023)
        [HttpGet("{districtId}/{id}")]
        public virtual async Task<IActionResult> GetAsync(Guid id, Guid districtId)
        {
            CommuneDto? entityDto = await _communeService.GetAsync( id, districtId);

            if (entityDto == null)
                return NoContent();

            return Ok(entityDto);
        }

        /// <summary>
        /// API lấy thông tin của xã
        /// </summary>
        /// <param name="id">Id của xã</param>
        /// <returns>Xã</returns>
        /// Author: LeDucTiep (12/07/2023)
        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(Guid id)
        {
            CommuneDto? entityDto = await _communeService.GetAsync(id);

            if (entityDto == null)
                return NoContent();

            return Ok(entityDto);
        }
    }
}

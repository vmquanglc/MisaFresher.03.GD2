using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher2023.Demo.BL.Service;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL.Model;
using System.Net.Http.Headers;

namespace MISA.WebFresher2023.Demo.Controllers
{
    [ApiController]
    public abstract class BaseController<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto> : ControllerBase
    {
        #region Field
        protected readonly IBaseService<TEntityDto, TEntityCreateDto, TEntityUpdateDto> _baseService;
        #endregion

        #region Contructor
        public BaseController(IBaseService<TEntityDto, TEntityCreateDto, TEntityUpdateDto> baseService)
        {
            _baseService = baseService;
        }
        #endregion

        #region Method
        /// <summary>
        /// API xuất dữ liệu
        /// </summary>
        /// <param name="baseExportArgument">Tham số để xuất dữ liệu</param>
        /// <returns>File excel</returns>
        /// Author: LeDucTiep (23/05/2023)
        [Route("exporting")]
        [HttpGet]
        public virtual async Task<IActionResult> GetExportAsync([FromQuery]BaseExportArgument baseExportArgument)
        {
            // Tạo file excel 
            XLWorkbook xlWorkbook = await _baseService.ExportAsync(baseExportArgument);

            using MemoryStream ms = new();

            // Lưu file vào MemoryStream
            xlWorkbook.SaveAs(ms);

            var table = typeof(TEntity).Name;

            HttpContext.Response.Headers.Add("FileName", ExportExcelResource.FileName(table));

            // Gửi file
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ExportExcelResource.FileName(table));
        }
        #endregion
    }
}

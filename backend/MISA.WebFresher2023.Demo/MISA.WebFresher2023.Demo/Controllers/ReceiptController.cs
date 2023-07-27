using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.BL.Service;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.DL.Entity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.WebFresher2023.Demo.Controllers
{
    [Route("api/v1/[controller]s")]
    public class ReceiptController : BaseController<Receipt, ReceiptDto, ReceiptCreateDto, ReceiptUpdateDto>
    {
        protected readonly IReceiptService _receiptService;
        public ReceiptController(IReceiptService receiptService
            ) : base(receiptService)
        {
            _receiptService = receiptService;
        }

        /// <summary>
        /// API lấy tất cả danh sách chức vụ
        /// </summary>
        /// <returns>Danh sách chức vụ</returns>
        /// Author: LeDucTiep (23/05/2023)
        [HttpGet]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<ReceiptDto> list = await _baseService.GetAllAsync();

            if (!list.Any())
                return NoContent();

            return Ok(list);
        }

        /// <summary>
        /// API thêm một bản ghi
        /// </summary>
        /// <param name="receiptCreateDto">Thông tin cần thêm</param>
        /// <returns>Id của bản ghi vừa thêm</returns>
        /// <exception cref="BadRequestException">Lỗi</exception>
        /// Author: LeDucTiep (12/07/2023)
        [HttpPost]
        public async Task<IActionResult> PostAsync(ReceiptCreateDto receiptCreateDto)
        {
            if (receiptCreateDto == null)
            {
                throw new BadRequestException();
            }
            ReceiptDto receiptDto = await _receiptService.PostAsync(receiptCreateDto);

            return StatusCode(201, receiptDto.ReceiptId);
        }

        /// <summary>
        /// API lấy mã mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// Author: LeDucTiep (12/07/2023)
        [Route("new-receipt-code")]
        [HttpGet]
        public async Task<IActionResult> GetNewCodeAsync()
        {
            return Ok(await _receiptService.GetNewCodeAsync());
        }

        /// <summary>
        /// API kiểm tra mã code đã tồn tại chưa
        /// </summary>
        /// <param name="code">Mã cần kiểm tra</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        [Route("is-code-duplicated")]
        [HttpGet]
        public async Task<IActionResult> CheckIsExistedCodeAsync(string code)
        {
            // Tạo connection
            return Ok(await _receiptService.CheckCodeDuplicatedAsync(code));
        }

        /// <summary>
        /// API kiểm tra mã code đang chỉnh sửa có bị trùng không
        /// </summary>
        /// <param name="receiptCode">Mã đang sửa</param>
        /// <param name="itsCode">Mã trước khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        [Route("is-edit-code-duplicated")]
        [HttpGet]
        public async Task<IActionResult> CheckEditCodeDuplicatedAsync(string receiptCode, string itsCode)
        {
            // Tạo connection
            return Ok(await _receiptService.CheckEditCodeDuplicatedAsync(receiptCode, itsCode));
        }

           /// <summary>
        /// API kiểm tra mã code đang chỉnh sửa có bị trùng không
        /// </summary>
        /// <param name="employeeCode">Mã đang sửa</param>
        /// <param name="itsCode">Mã trước khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        [Route("setting-recorded/{id}")]
        [HttpPut]
        public async Task<IActionResult> SetRecordedAsync(Guid id, bool isRecorded)
        {
            // Tạo connection
            return Ok(await _receiptService.SetRecordedAsync(id, isRecorded));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.BL.Service;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.WebFresher2023.Demo.Controllers
{
    [Route("api/v1/[controller]s")]
    public class AccountController : BaseController<Account, AccountDto, AccountCreateDto, AccountUpdateDto>
    {
        protected readonly IAccountService _accountService;

        public AccountController(IAccountService accountService
            ) : base(accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// API lấy tất cả danh sách chức vụ
        /// </summary>
        /// <returns>Danh sách chức vụ</returns>
        /// Author: LeDucTiep (23/05/2023)
        [HttpGet]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<AccountDto> list = await _baseService.GetAllAsync();

            if (!list.Any())
                return NoContent();

            return Ok(list);
        }

        /// <summary>
        /// API thêm một bản ghi
        /// </summary>
        /// <param name="accountCreateDto">Thông tin tài khoản cần thêm</param>
        /// <exception cref="BadRequestException">Lỗi thông tin bản ghi</exception>
        /// <returns>Id bản ghi vừa thêm</returns>
        /// Author: LeDucTiep (23/05/2023)
        [HttpPost]
        public async Task<IActionResult> PostAsync(AccountCreateDto accountCreateDto)
        {
            if (accountCreateDto == null)
            {
                throw new BadRequestException();
            }
            AccountDto accountDto = await _accountService.PostAsync(accountCreateDto);

            return StatusCode(201, accountDto.AccountId);
        }

        /// <summary>
        /// API lấy trang tài khoản và có lọc theo MisaCode
        /// </summary>
        /// <param name="accountPagingArgument">Các điều kiện để phân trang</param>
        /// <returns>Trang tai khoan</returns>
        /// Author: LeDucTiep (14/07/2023)
        [Route("ms-paging")]
        [HttpGet]
        public async Task<IActionResult> GetPageAsync([FromQuery] AccountPagingArgument accountPagingArgument)
        {
            BasePage<AccountDto> page = await _accountService.GetPageAsync(accountPagingArgument);

            return Ok(page);
        }

        /// <summary>
        /// API kiểm tra số tài khoản đã tồn tại chưa
        /// </summary>
        /// <param name="code">Số tài khoản</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (23/05/2023)
        [Route("is-code-duplicated")]
        [HttpGet]
        public async Task<IActionResult> CheckIsExistedCodeAsync(string code)
        {
            // Tạo connection
            return Ok(await _accountService.CheckCodeDuplicatedAsync(code));
        }

        /// <summary>
        /// API kiểm tra mã code đang sửa có bị trùng không
        /// </summary>
        /// <param name="accountCode">Số tài khoản</param>
        /// <param name="itsCode">Số tài khoản trước khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (23/05/2023)
        [Route("is-edit-code-duplicated")]
        [HttpGet]
        public async Task<IActionResult> CheckEditCodeDuplicatedAsync(string accountCode, string itsCode)
        {
            // Tạo connection
            return Ok(await _accountService.CheckEditCodeDuplicatedAsync(accountCode, itsCode));
        }
    }
}

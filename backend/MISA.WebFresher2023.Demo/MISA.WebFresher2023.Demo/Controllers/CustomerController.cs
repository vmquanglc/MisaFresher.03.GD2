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
    public class CustomerController : BaseController<Customer, CustomerDto, CustomerCreateDto, CustomerUpdateDto>
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService
            ) : base(customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// API lấy tất cả danh sách chức vụ
        /// </summary>
        /// <returns>Danh sách chức vụ</returns>
        /// Author: LeDucTiep (23/05/2023)
        [HttpGet]
        public virtual async Task<IActionResult> GetAllAsync()
        {
            IEnumerable<CustomerDto> list = await _baseService.GetAllAsync();

            if (!list.Any())
                return NoContent();

            return Ok(list);
        }

        /// <summary>
        /// API lấy mã mới
        /// </summary>
        /// <returns>Mã mới</returns>
        /// Author: LeDucTiep (12/07/2023)
        [Route("new-customer-code")]
        [HttpGet]
        public async Task<IActionResult> GetNewCodeAsync()
        {
            return Ok(await _customerService.GetNewCustomerCodeAsync());
        }

        /// <summary>
        /// API thêm một bản ghi
        /// </summary>
        /// <param name="customerCreateDto">Thông tin của bản ghi cần thêm</param>
        /// <returns>Id của bản ghi đã thêm</returns>
        /// <exception cref="BadRequestException"></exception>
        /// Author: LeDucTiep (12/07/2023)
        [HttpPost]
        public async Task<IActionResult> PostAsync(CustomerCreateDto customerCreateDto)
        {
            if (customerCreateDto == null)
            {
                throw new BadRequestException();
            }
            CustomerDto customerDto = await _customerService.PostAsync(customerCreateDto);

            return StatusCode(201, customerDto.CustomerId);
        }

        /// <summary>
        /// API kiểm tra mã code có bị trùng không
        /// </summary>
        /// <param name="code">Mã code cần kiểm tra</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        [Route("is-code-duplicated")]
        [HttpGet]
        public async Task<IActionResult> CheckIsExistedCodeAsync(string code)
        {
            // Tạo connection
            return Ok(await _customerService.CheckCodeDuplicatedAsync(code));
        }

        /// <summary>
        /// API kiểm tra mã code đang sửa có bị trùng không
        /// </summary>
        /// <param name="employeeCode">Mã code sau khi sửa</param>
        /// <param name="itsCode">Mã code trước khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        [Route("is-edit-code-duplicated")]
        [HttpGet]
        public async Task<IActionResult> CheckEditCodeDuplicatedAsync(string employeeCode, string itsCode)
        {
            // Tạo connection
            return Ok(await _customerService.CheckEditCodeDuplicatedAsync(employeeCode, itsCode));
        }
    }
}

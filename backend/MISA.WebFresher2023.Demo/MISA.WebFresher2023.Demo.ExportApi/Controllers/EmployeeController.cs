using Microsoft.AspNetCore.Mvc;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;
using ClosedXML.Excel;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.BL.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.WebFresher2023.Demo.Controllers
{
    [Route("api/v1/[controller]s")]
    public class EmployeeController : BaseController<Employee, EmployeeDto, EmployeeCreateDto, EmployeeUpdateDto>
    {
        #region Field
        /// <summary>
        /// EmployeeService
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        protected readonly IEmployeeService _employeeService;
        #endregion

        #region Contructor
        public EmployeeController(
            IEmployeeService employeeService
            ) : base(employeeService)
        {
            _employeeService = employeeService;
        }
        #endregion
    }
}

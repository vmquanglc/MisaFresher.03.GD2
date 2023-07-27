using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.BL.Service;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Repository;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    /// <summary>
    /// Class dịch vụ phòng ban
    /// </summary>
    /// Author: LeDucTiep (23/05/2023)
    public class DepartmentService : BaseService<Department, DepartmentDto, DepartmentCreateDto, DepartmentUpdateDto>, IDepartmentService
    {
        public DepartmentService(IDepartmentRepository departmentRepository, 
            IMSDatabase msDatabase,
            IMapper mapper) : base(msDatabase, departmentRepository, mapper)
        {
        }
    }
}

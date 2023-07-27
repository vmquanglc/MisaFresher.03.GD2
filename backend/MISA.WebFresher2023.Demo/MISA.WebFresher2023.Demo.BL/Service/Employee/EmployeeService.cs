using AutoMapper;
using MISA.WebFresher2023.Demo.Common.Constant;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;
using MISA.WebFresher2023.Demo.Common.Attribute;
using System.Reflection;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.DL.Repository;
using MISA.WebFresher2023.Demo.DL;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    /// <summary>
    /// Class dịch vụ nhân viên 
    /// </summary>
    /// Author: LeDucTiep (23/05/2023)
    public class EmployeeService : BaseService<Employee, EmployeeDto, EmployeeCreateDto, EmployeeUpdateDto>, IEmployeeService
    {
        #region Fieldz
        public IEmployeeRepository _employeeRepository;
        #endregion

        #region Contructor
        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IMSDatabase msDatabase,
            IMapper mapper) : base(msDatabase, employeeRepository, mapper)
        {
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Method
        /// <summary>
        /// Kiểm tra bản ghi cần thêm 
        /// </summary>
        /// <param name="entity">Bản ghi</param>
        /// <exception cref="BadRequestException">Thông tin không đúng</exception>
        /// Author: LeDucTiep (23/05/2023)
        public override async Task<List<int>> PostValidate(EmployeeDto entity)
        {
            List<int> errorList = new();

            // Kiểm tra tồn tại phòng ban 
            bool isExisted = await _employeeRepository.CheckExistedAsync(entity.DepartmentId, "Department");
            if (!isExisted)
                errorList.Add((int)DepartmentErrorCode.IdNotFound);

            // Kiểm tra tồn tại chức vụ
            if (entity.PositionId != null)
            {
                isExisted = await _employeeRepository.CheckExistedAsync((Guid)entity.PositionId, "Position");

                if (!isExisted)
                    errorList.Add((int)PositionErrorCode.IdNotFound);
            }

            // Kiểm tra phải chưa tồn tại mã nhân viên 
            isExisted = await _employeeRepository.CheckExistedEmployeeCodeAsync(entity.EmployeeCode);
            if (isExisted)
                errorList.Add((int)EmployeeErrorCode.CodeDuplicated);

            return errorList;
        }

        /// <summary>
        /// Validate update một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="entity">Giá trị bản ghi</param>
        /// Author: LeDucTiep (08/06/2023)
        public override async Task<List<int>> UpdateValidate(Guid id, EmployeeDto entity)
        {
            List<int> errorList = new();

            // Kiểm tra tồn tại phòng ban 
            bool isExisted = await _employeeRepository.CheckExistedAsync(entity.DepartmentId, "Department");
            if (!isExisted)
                // Xử lý lỗi
                errorList.Add((int)DepartmentErrorCode.IdNotFound);


            // Kiểm tra tồn tại chức vụ
            if (entity.PositionId != null)
            {
                isExisted = await _employeeRepository.CheckExistedAsync((Guid)entity.PositionId, "Position");
                if (!isExisted)
                    errorList.Add((int)PositionErrorCode.IdNotFound);
            }

            // Kiểm tra phải chưa tồn tại mã nhân viên, ngoại trừ mã trước khi sửa
            isExisted = await _employeeRepository.CheckDuplicatedEmployeeEditCodeAsync(entity.EmployeeCode, id);
            if (isExisted)
                errorList.Add((int)EmployeeErrorCode.CodeDuplicated);

            // Kiểm tra có Id nhân viên cần sửa không
            isExisted = await _baseRepository.CheckExistedAsync(id);
            if (!isExisted)
                errorList.Add((int)EmployeeErrorCode.IdNotFound);

            return errorList;
        }

        /// <summary>
        /// Xóa một bản ghi theo id 
        /// </summary>
        /// <param name="id">Id của bản ghi </param>
        /// <exception cref="BadRequestException">Lỗi không tìm thấy </exception>
        /// Author: LeDucTiep (23/05/2023)
        public override async Task<List<int>> DeleteValidate(Guid id)
        {
            List<int> errorList = new();

            // Kiểm tra có tồn tại bản ghi không 
            bool isExistedCode = await _baseRepository.CheckExistedAsync(id);

            // Nếu có lỗi xảy ra thì ném lỗi 
            if (!isExistedCode)
            {
                errorList.Add((int)EmployeeErrorCode.IdNotFound);
            }

            return errorList;
        }

        /// <summary>
        /// Hàm kiểm tra mã nhân viên đã tồn tại chưa
        /// </summary>
        /// <param name="code">Mã nhân viên</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (23/05/2023)
        public async Task<bool> CheckEmployeeCodeAsync(string code)
        {
            try
            {
                PropertyInfo? property = typeof(EmployeeDto).GetProperty("EmployeeCode");

                if (property != null)
                {

                    // Xét độ dài 
                    var attributeMaxLength = (MSMaxLengthAttribute?)property.GetCustomAttributes(typeof(MSMaxLengthAttribute), false).FirstOrDefault();

                    if (attributeMaxLength != null && code != null)
                    {
                        int valueLength = code.Length;
                        int maxLength = attributeMaxLength.Length;
                        bool isTooLong = valueLength > maxLength;
                        if (isTooLong)
                        {
                            List<int> errorList = new() { (int)EmployeeErrorCode.EmployeeCodeTooLong };
                            throw new BadRequestException(errorList);
                        }
                    }
                }

                return await _employeeRepository.CheckExistedEmployeeCodeAsync(code);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Hàm kiểm tra mã EmployeeCode muốn sửa đã tồn tại chưa
        /// </summary>
        /// <param name="employeeCode">EmployeeCode</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (23/05/2023)
        public async Task<bool> CheckDuplicatedEmployeeEditCodeAsync(string employeeCode, string itsCode)
        {
            try
            {
                PropertyInfo? property = typeof(EmployeeDto).GetProperty("EmployeeCode");

                if (property != null)
                {

                    // Xét độ dài 
                    var attributeMaxLength = (MSMaxLengthAttribute?)property.GetCustomAttributes(typeof(MSMaxLengthAttribute), false).FirstOrDefault();

                    if (attributeMaxLength != null && employeeCode != null)
                    {
                        int valueLength = employeeCode.Length;
                        int maxLength = attributeMaxLength.Length;
                        bool isTooLong = valueLength > maxLength;
                        if (isTooLong)
                        {
                            List<int> errorList = new() { (int)EmployeeErrorCode.EmployeeCodeTooLong };
                            throw new BadRequestException(errorList);
                        }
                    }
                }

                return await _employeeRepository.CheckDuplicatedEmployeeEditCodeAsync(employeeCode, itsCode);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Hàm lấy mã nhân viên mới 
        /// </summary>
        /// <returns>Mã nhân viên mới </returns>
        /// Author: LeDucTiep (23/05/2023)
        public async Task<string> GetNewEmployeeCodeAsync()
        {
            try
            {
                return await _employeeRepository.GetNewEmployeeCodeAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Hàm lấy tất cả employee để xuất file 
        /// </summary>
        /// <returns>Danh sách employee</returns>
        /// Author: LeDucTiep (07/06/2023)
        public async Task<List<EmployeeExport>> ExportJsonAsync()
        {
            return (List<EmployeeExport>)await _employeeRepository.GetEmployeeExportAsync();
        }
        #endregion
    }
}

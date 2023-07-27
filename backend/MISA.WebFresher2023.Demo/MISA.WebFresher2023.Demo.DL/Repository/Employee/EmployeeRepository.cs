using MISA.WebFresher2023.Demo.DL.Entity;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using MISA.WebFresher2023.Demo.DL.Model;
using MISA.WebFresher2023.Demo.Common.Resource;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        #region Contructor
        public EmployeeRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }
        #endregion

        #region Method
        /// <summary>
        /// Hàm kiểm tra mã EmployeeCode đã tồn tại chưa
        /// </summary>
        /// <param name="employeeCode">EmployeeCode</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (23/05/2023)
        public async Task<bool> CheckExistedEmployeeCodeAsync(string employeeCode)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Tên procedure
            string procedure = ProcedureResource.EmployeeCheckDuplicatedCode;

            // Tạo các tham số 
            var parameters = new DynamicParameters();
            parameters.Add("employeeCode", employeeCode);

            // Gọi đến procedure
            bool result = await connection.QueryFirstAsync<bool>(
                procedure,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        /// <summary>
        /// Hàm kiểm tra mã EmployeeCode muốn sửa đã tồn tại chưa
        /// </summary>
        /// <param name="employeeCode">EmployeeCode</param>
        /// <param name="itsCode">EmployeeCode trước khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (23/05/2023)
        public async Task<bool> CheckDuplicatedEmployeeEditCodeAsync(string employeeCode, string itsCode)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Tên procedure
            string procedure = ProcedureResource.EmployeeCheckDuplicatedCodeExceptItsCode;


            // Tạo các tham số 
            var parameters = new DynamicParameters();
            parameters.Add("employeeCode", employeeCode);
            parameters.Add("itsCode", itsCode);

            // Gọi đến procedure
            bool result = await connection.QueryFirstAsync<bool>(
                procedure,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        /// <summary>
        /// Hàm kiểm tra mã EmployeeCode muốn sửa đã tồn tại chưa
        /// </summary>
        /// <param name="employeeCode">EmployeeCode</param>
        /// <param name="itsId">Id của bản ghi</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (23/05/2023)
        public async Task<bool> CheckDuplicatedEmployeeEditCodeAsync(string employeeCode, Guid itsId)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Tên procedure
            string procedure = ProcedureResource.EmployeeCheckDuplicatedCodeExceptItsId;

            // Tạo các tham số 
            var parameters = new DynamicParameters();
            parameters.Add("employeeCode", employeeCode);
            parameters.Add("itsId", itsId);

            // Gọi đến procedure
            bool result = await connection.QueryFirstAsync<bool>(
                procedure,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        /// <summary>
        /// Hàm chuyển chuỗi sang số 
        /// </summary>
        /// <param name="input">Chuỗi kí tự</param>
        /// <returns>Số int</returns>
        /// Author: LeDucTiep (27/05/2023)
        static long GetNumbers(string input)
        {
            try
            {
                return long.Parse(new string(input.Where(c => char.IsDigit(c)).ToArray()));
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Hàm tạo ra mã employeeCode
        /// </summary>
        /// <returns>Mã employeeCode mới</returns>
        /// Author: LeDucTiep (23/05/2023)
        public async Task<string> GetNewEmployeeCodeAsync()
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Tên procedure
            string procedure = ProcedureResource.EmployeeCheckDuplicatedCode;

            int numberUp = 1;

            // Tạo mã mới
            string sql = "SELECT MAX(e.EmployeeCode) FROM employee e;";
            IEnumerable<string> enumerable = await connection.QueryAsync<string>(sql);
            string maxCode = enumerable.First();

            string preCode = "NV-";

            int index = maxCode.IndexOf("-"); // Tìm vị trí của dấu "-"
            if (index != -1) // Nếu tìm thấy dấu "-"
            {
                preCode = maxCode[..(index + 1)]; // Lấy ra chuỗi "PT-"
            }

            long code = GetNumbers(maxCode);

            while (true)
            {
                code += numberUp;
                string newEmployeeCode = $"{preCode}{code.ToString().PadLeft(4, '0')}";

                // Kiểm tra mã mới có bị trùng không
                // Tạo các tham số 
                var parameters = new DynamicParameters();
                parameters.Add("employeeCode", newEmployeeCode);

                // Gọi đến procedure
                bool isExitsted = await connection.QueryFirstAsync<bool>(
                    procedure,
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (!isExitsted)
                {
                    return newEmployeeCode;
                }
                numberUp++;
            }
        }

        /// <summary>
        /// Hàm lấy dữ liệu để xuất employee 
        /// </summary>
        /// <returns>EmployeeExport</returns>
        /// Author: LeDucTiep (07/06/2023)
        public async Task<IEnumerable<EmployeeExport>> GetEmployeeExportAsync()
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            string procedure = ProcedureResource.EmployeeExport;

            // Gọi procedure 
            var res = await connection.QueryAsync<EmployeeExport>(
                procedure,
                commandType: CommandType.StoredProcedure
            );

            // trả về kết quả
            return res;
        }

        #endregion
    }
}

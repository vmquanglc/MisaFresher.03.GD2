using Dapper;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL.Entity;
using System.Data;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }

        /// <summary>
        /// Hàm kiểm tra mã đang chỉnh sửa có bị trùng không
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <param name="itsCode">Mã khách hàng trước khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<bool> CheckEditCodeDuplicatedAsync(string customerCode, string itsCode)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Tên procedure
            string procedure = ProcedureResource.CustomerCheckDuplicatedCodeExceptItsCode;


            // Tạo các tham số 
            var parameters = new DynamicParameters();
            parameters.Add("customerCode", customerCode);
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
        /// Hàm kiểm tra mã khách hàng đã tồn tại chưa
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<bool> CheckExistedCodeAsync(string customerCode)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Tên procedure
            string procedure = ProcedureResource.CustomerCheckDuplicatedCode;

            // Tạo các tham số 
            var parameters = new DynamicParameters();
            parameters.Add("customerCode", customerCode);

            // Gọi đến procedure
            bool result = await connection.QueryFirstAsync<bool>(
                procedure,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        /// <summary>
        /// Hàm lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<string> GetNewCustomerCodeAsync()
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Tên procedure
            //string procedure = ProcedureResource.CheckDuplicatedCode;

            int numberUp = 1;

            // Tạo mã mới
            string sql = "SELECT MAX(c.CustomerCode) FROM customer c;";
            IEnumerable<string> enumerable = await connection.QueryAsync<string>(sql);
            string maxCode = enumerable.First();

            string preCode = "KH-";

            int index = maxCode.IndexOf("-"); // Tìm vị trí của dấu "-"
            if (index != -1) // Nếu tìm thấy dấu "-"
            {
                preCode = maxCode.Substring(0, index + 1); // Lấy ra chuỗi "PT-"
            }

            long code = GetNumbers(maxCode);

            while (true)
            {
                code += numberUp;
                string newEmployeeCode = $"{preCode}{code.ToString().PadLeft(4, '0')}";

                return newEmployeeCode;
            }
        }

        /// <summary>
        /// Hàm lấy các số từ chuỗi string 
        /// </summary>
        /// <param name="input">Chuỗi</param>
        /// <returns>Số</returns>
        /// Author: LeDucTiep (12/04/2023)
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
    }
}

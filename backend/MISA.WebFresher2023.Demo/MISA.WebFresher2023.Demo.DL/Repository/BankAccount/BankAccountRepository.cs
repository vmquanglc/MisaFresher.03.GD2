using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL.Entity;
using System.Data;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class BankAccountRepository : BaseRepository<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }

        /// <summary>
        /// Hàm lấy danh sách tài khoản ngân hàng theo id của khách hàng
        /// </summary>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns>Danh sách tài khoản ngân hàng</returns>
        /// Author: LeDucTiep (08/05/2023)
        public async Task<List<BankAccount>> GetByCustomerIdAsync(Guid customerId)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            string procedure = ProcedureResource.BankAccountGetByCustomerId;

            var parameters = new DynamicParameters();
            parameters.Add("customerId", customerId);

            // Gọi procedure 
            List<BankAccount> res = (List<BankAccount>)await connection.QueryAsync<BankAccount>(
                procedure,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

            // trả về kết quả
            return res;
        }

        /// <summary>
        /// Hàm xóa các bản ghi không có id trong danh sách
        /// </summary>
        /// <param name="bankAccountNumberList">Danh sách id của các bản ghi không cần xóa </param>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns>Task</returns>
        /// Author: LeDucTiep (08/05/2023)
        public async Task DeleteNotInAsync(string bankAccountNumberList, Guid customerId)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            string procedure = ProcedureResource.BankAccountDeleteNotIn;

            var parameters = new DynamicParameters();
            parameters.Add("bankAccountNumberList", bankAccountNumberList);
            parameters.Add("customerId", customerId);

            // Gọi procedure 
            await connection.ExecuteAsync(
                procedure,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

        }
    }
}

using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL.Entity;
using System.Data;
using static Dapper.SqlMapper;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class CustomerAndGroupRepository : BaseRepository<CustomerAndGroup>, ICustomerAndGroupRepository
    {
        public CustomerAndGroupRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }

        /// <summary>
        /// Hàm kiểm tra bản ghi có tồn tại không
        /// </summary>
        /// <param name="customerGroupId">Id của nhóm khách hàng</param>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<bool> CheckExistedAsync(Guid customerGroupId, Guid customerId)
        {
            // Tên bảng
            string table = typeof(CustomerAndGroup).Name;

            // Kết nối với database
            var connection = await _msDatabase.GetOpenConnectionAsync();
            // Tên procedure
            string procedure = ProcedureResource.CheckExistedById(table);

            // Tham số 
            var dynamicParams = new DynamicParameters();
            dynamicParams.Add($"customerGroupId", customerGroupId);
            dynamicParams.Add($"customerId", customerId);

            // Bản ghi trả về 
            bool isExists = await connection.QueryFirstAsync<bool>(
                    procedure,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

            return isExists;

        }

        /// <summary>
        /// Hàm lấy danh sách nhóm khách hàng theo id khách hàng
        /// </summary>
        /// <param name="customerId">Id khách hàng</param>
        /// <returns>Danh sách Id nhóm khách hàng</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<List<Guid>> GetCustomerGroupIdByCustomerIdAsync(Guid customerId)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            string procedure = ProcedureResource.CustomerAndGroupGetCustomerGroupIdByCustomerId;

            var parameters = new DynamicParameters();
            parameters.Add("customerId", customerId);

            // Gọi procedure 
            List<Guid> res = (List<Guid>)await connection.QueryAsync<Guid>(
                procedure,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

            // trả về kết quả
            return res;
        }

        /// <summary>
        /// Hàm xóa các bản ghi không nằm trong danh sách 
        /// </summary>
        /// <param name="customerGroupList">Danh sách id các customerGroup</param>
        /// <param name="customerId">id của khách hàng</param>
        /// <returns>Task</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task DeleteNotInAsync(string customerGroupList, Guid customerId)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            string procedure = ProcedureResource.CustomerAndGroupDeleteNotIn;

            var parameters = new DynamicParameters();
            parameters.Add("customerGroupList", customerGroupList);
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

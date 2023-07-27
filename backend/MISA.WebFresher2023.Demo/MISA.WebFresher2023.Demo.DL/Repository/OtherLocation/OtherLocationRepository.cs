using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL.Entity;
using System.Data;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class OtherLocationRepository : BaseRepository<OtherLocation>, IOtherLocationRepository
    {
        public OtherLocationRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }

        /// <summary>
        /// Hàm lấy danh sách địa chỉ theo id của khách hàng
        /// </summary>
        /// <param name="customerId">Id của khách hàng</param>
        /// <returns>OtherLocation</returns>
        public async Task<OtherLocation> GetByCustomerIdAsync(Guid customerId)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            string procedure = ProcedureResource.OtherLocationGetByCustomerId;

            var parameters = new DynamicParameters();
            parameters.Add("customerId", customerId);

            // Gọi procedure 
            OtherLocation res = await connection.QueryFirstOrDefaultAsync<OtherLocation>(
                procedure,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

            // trả về kết quả
            return res;
        }
    }
}

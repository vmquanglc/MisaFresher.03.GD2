using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL.Entity;
using System.Collections.Generic;
using System.Data;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class SpecificAddressRepository : BaseRepository<SpecificAddress>, ISpecificAddressRepository
    {
        public SpecificAddressRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }

        /// <summary>
        /// Hàm lấy danh sách địa chỉ theo OtherLocationId
        /// </summary>
        /// <param name="otherLocationId">Id của bảng địa chỉ khác</param>
        /// <returns>Danh sách địa chỉ</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<List<SpecificAddress>> GetByOtherLocationIdAsync(Guid otherLocationId)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            string procedure = ProcedureResource.SpecificAddressGetByOtherLocationId;

            var parameters = new DynamicParameters();
            parameters.Add("otherLocationId", $"{otherLocationId}");

            // Gọi procedure 
            List<SpecificAddress> res = (List<SpecificAddress>)await connection.QueryAsync<SpecificAddress>(
                procedure,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

            // trả về kết quả
            return res;
        }

        /// <summary>
        /// Hàm xóa các địa chỉ không có danh sách
        /// </summary>
        /// <param name="addressList">Danh sách id các bản ghi ko xóa, các id nằm trong dấu ' và cách nhau dấu ,</param>
        /// <param name="otherLocationId">Id của khóa ngoại đến bảng cha</param>
        /// <returns>void</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task DeleteNotInAsync(string addressList, Guid otherLocationId)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            string procedure = ProcedureResource.SpecificAddressDeleteNotIn;

            var parameters = new DynamicParameters();
            parameters.Add("addressList", addressList);
            parameters.Add("otherLocationId", otherLocationId);

            // Gọi procedure 
            await connection.ExecuteAsync(
                procedure,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}

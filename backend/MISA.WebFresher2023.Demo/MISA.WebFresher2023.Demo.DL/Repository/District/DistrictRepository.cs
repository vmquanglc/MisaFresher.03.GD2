using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;
using System.Data;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class DistrictRepository : BaseRepository<District>, IDistrictRepository
    {
        public DistrictRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }

        /// <summary>
        /// Hàm lấy trang
        /// </summary>
        /// <typeparam name="TEntityInPage">Loại bản ghi trong trang</typeparam>
        /// <param name="districtPagingArgument">Tham số để phân trang</param>
        /// <returns>BasePage<TEntityInPage></returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<BasePage<TEntityInPage>> GetPageAsync<TEntityInPage>(DistrictPagingArgument districtPagingArgument)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            string tableName = typeof(District).Name;

            string procedure = ProcedureResource.GetPaging(tableName);

            // Tạo tham số đầu vào 
            // IN _offset: Số bản ghi bị bỏ qua
            // IN _limit: Số bản ghi được lấy
            // IN employeeSearchTerm: Từ khóa tìm kiếm, theo employeeCode hoặc FullName
            // OUT TotalRecord: Tổng số bản ghi tìm thấy
            var parameters = new DynamicParameters();
            int offset = (districtPagingArgument.PageNumber - 1) * districtPagingArgument.PageSize;
            parameters.Add("cityId", districtPagingArgument.CityId);
            parameters.Add("_offset", offset);
            parameters.Add("_limit", districtPagingArgument.PageSize);
            parameters.Add("searchTerm", districtPagingArgument.SearchTerm ?? "");
            parameters.Add("totalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try
            {

                // Gọi procedure 
                IEnumerable<TEntityInPage> res = await connection.QueryAsync<TEntityInPage>(
                    procedure,
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                // Lấy tổng số trang 
                var totalRecord = parameters.Get<int>("totalRecord");

                // trả về kết quả
                return new BasePage<TEntityInPage>(totalRecord, res);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Hàm lấy một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="cityId">Mã thành phố</param>
        /// <returns>District?</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<District?> GetAsync(Guid id, Guid? cityId = null)
        {
            // Tên bảng
            var table = typeof(District).Name;

            // Tên procedure
            string procedure = ProcedureResource.Get(table);

            // Kết nối với database
            var connection = await _msDatabase.GetOpenConnectionAsync();
            try
            {
                // Tham số 
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add($"{table}Id", id);
                dynamicParams.Add($"cityId", cityId);

                // Bản ghi trả về 
                var entity = await connection.QueryFirstOrDefaultAsync<District>(
                    procedure,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

                return entity;
            }
            catch (Exception ex)
            {
                throw new InternalException();
            }
        }
    }
}

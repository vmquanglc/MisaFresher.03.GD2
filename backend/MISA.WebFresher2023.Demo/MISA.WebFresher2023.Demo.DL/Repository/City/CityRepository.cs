using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;
using static Dapper.SqlMapper;
using System.Data;
using MISA.WebFresher2023.Demo.Common.MyException;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class CityRepository : BaseRepository<City>, ICityRepository
    {
        public CityRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }

        /// <summary>
        /// Hàm lấy trang bản ghi
        /// </summary>
        /// <typeparam name="TEntityInPage">Loại bản ghi trong trang</typeparam>
        /// <param name="cityPagingArgument">Tham số để phân trang</param>
        /// <returns>Trang bản ghi</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<BasePage<TEntityInPage>> GetPageAsync<TEntityInPage>(CityPagingArgument cityPagingArgument)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            string tableName = typeof(City).Name;

            string procedure = ProcedureResource.GetPaging(tableName);

            // Tạo tham số đầu vào 
            // IN _offset: Số bản ghi bị bỏ qua
            // IN _limit: Số bản ghi được lấy
            // IN employeeSearchTerm: Từ khóa tìm kiếm, theo employeeCode hoặc FullName
            // OUT TotalRecord: Tổng số bản ghi tìm thấy
            var parameters = new DynamicParameters();
            int offset = (cityPagingArgument.PageNumber - 1) * cityPagingArgument.PageSize;
            parameters.Add("nationId", cityPagingArgument.NationId);
            parameters.Add("_offset", offset);
            parameters.Add("_limit", cityPagingArgument.PageSize);
            parameters.Add("searchTerm", cityPagingArgument.SearchTerm ?? "");
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
        /// <param name="nationId">Id quốc gia</param>
        /// <returns>Thành phố</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<City?> GetAsync(Guid id, Guid? nationId = null)
        {
            // Tên bảng
            var table = typeof(City).Name;

            // Tên procedure
            string procedure = ProcedureResource.Get(table);

            // Kết nối với database
            var connection = await _msDatabase.GetOpenConnectionAsync();
            try
            {
                // Tham số 
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add($"{table}Id", id);
                dynamicParams.Add($"nationId", nationId);

                // Bản ghi trả về 
                var entity = await connection.QueryFirstOrDefaultAsync<City>(
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

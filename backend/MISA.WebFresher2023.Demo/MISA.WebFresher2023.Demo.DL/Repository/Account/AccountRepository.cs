using Dapper;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;
using System.Data;
using static Dapper.SqlMapper;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }

        /// <summary>
        /// Hàm kiểm tra sửa mã code có bị trùng không 
        /// </summary>
        /// <param name="accountCode">Mã sau khi sửa</param>
        /// <param name="itsCode">Mã trước khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> CheckEditCodeDuplicatedAsync(string accountCode, string itsCode)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Tên procedure
            string procedure = ProcedureResource.AccountCheckDuplicatedCodeExceptItsCode;


            // Tạo các tham số 
            var parameters = new DynamicParameters();
            parameters.Add("accountCode", accountCode);
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
        /// Hàm kiểm tra số tài khoản cần thêm có bị trùng không 
        /// </summary>
        /// <param name="accountCode"></param>
        /// <returns></returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> CheckExistedCodeAsync(string accountCode)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Tên procedure
            string procedure = ProcedureResource.AccountCheckDuplicatedCode;

            // Tạo các tham số 
            var parameters = new DynamicParameters();
            parameters.Add("accountCode", accountCode);

            // Gọi đến procedure
            bool result = await connection.QueryFirstAsync<bool>(
                procedure,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        /// <summary>
        /// Hàm lấy trang tài khoản
        /// </summary>
        /// <typeparam name="TEntityInPage">Kiểu dữ liệu của danh sách</typeparam>
        /// <param name="accountPagingArgument">Tham số để lọc</param>
        /// <returns>BasePage<TEntityInPage></returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<BasePage<TEntityInPage>> GetPageAsync<TEntityInPage>(AccountPagingArgument accountPagingArgument)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            string procedure = ProcedureResource.AccountPagingByRank;

            // Tạo tham số đầu vào 
            // IN _offset: Số bản ghi bị bỏ qua
            // IN _limit: Số bản ghi được lấy
            // IN employeeSearchTerm: Từ khóa tìm kiếm, theo employeeCode hoặc FullName
            // OUT TotalRecord: Tổng số bản ghi tìm thấy
            var parameters = new DynamicParameters();
            int offset = (accountPagingArgument.PageNumber - 1) * accountPagingArgument.PageSize;
            parameters.Add("misaCode", accountPagingArgument.MisaCode);
            parameters.Add("_offset", offset);
            parameters.Add("_limit", accountPagingArgument.PageSize);
            parameters.Add("searchTerm", accountPagingArgument.SearchTerm ?? "");
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
        /// Hàm kiểm tra account này có phải là account cha hay không
        /// </summary>
        /// <param name="id">Id của account</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> CheckIsParentAsync(Guid id)
        {
            // Tên bảng
            string table = typeof(Account).Name;

            // Kết nối với database
            var connection = await _msDatabase.GetOpenConnectionAsync();
            try
            {
                // Tên procedure
                string procedure = ProcedureResource.AccountCheckIsParent;

                // Tham số 
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add($"{table}Id", id);

                // Bản ghi trả về 
                bool isExists = await connection.QueryFirstAsync<bool>(
                        procedure,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );

                return isExists;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Hàm kiểm tra account này có phải là account ông không 
        /// </summary>
        /// <param name="id">Id của account</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> CheckIsGrandpaAsync(Guid id)
        {
            // Tên bảng
            string table = typeof(Account).Name;

            // Kết nối với database
            var connection = await _msDatabase.GetOpenConnectionAsync();
            try
            {
                // Tên procedure
                string procedure = ProcedureResource.AccountCheckIsGrandpa;

                // Tham số 
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add($"{table}Id", id);

                // Bản ghi trả về 
                bool isExists = await connection.QueryFirstAsync<bool>(
                        procedure,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );

                return isExists;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Hàm kiểm tra số tài khoản đã thay đổi chưa
        /// </summary>
        /// <param name="id">Id của account</param>
        /// <param name="accountCode">Số tài khoản</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> CheckIsChangedAccountCodeAsync(Guid id, string accountCode)
        {
            // Tên bảng
            string table = typeof(Account).Name;

            // Kết nối với database
            var connection = await _msDatabase.GetOpenConnectionAsync();
            try
            {
                // Tên procedure
                string procedure = ProcedureResource.AccountCheckIsChangedAccountCode;

                // Tham số 
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add($"{table}Id", id);
                dynamicParams.Add($"accountCode", accountCode);

                // Bản ghi trả về 
                bool isChanged = await connection.QueryFirstAsync<bool>(
                        procedure,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );

                return isChanged;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Hàm Cập nhật lại MisaCode
        /// </summary>
        /// <param name="oldMisaCode">MisaCode cũ </param>
        /// <param name="newMisaCode">MisaCode mới</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> UpdateMisaCodeAsync(string oldMisaCode, string newMisaCode)
        {
            // Kết nối với database
            var connection = await _msDatabase.GetOpenConnectionAsync();

            try
            {
                // Tên procedure
                string procedure = ProcedureResource.AccountUpdateMisaCode;

                // Tham số 
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add($"oldMisaCode", oldMisaCode);
                dynamicParams.Add($"newMisaCode", newMisaCode);

                // Bản ghi trả về 
                bool isChanged = await connection.QueryFirstAsync<bool>(
                        procedure,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );

                return isChanged;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Hàm tính toán lại MisaCode
        /// </summary>
        /// <param name="id">Id của bản ghi cần tính toán lại</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> RecalculateMisaCodeAsync(Guid id)
        {
            // Kết nối với database
            var connection = await _msDatabase.GetOpenConnectionAsync();

            try
            {
                // Tên procedure
                string procedure = ProcedureResource.AccountRecalculateMisaCode;

                // Tham số 
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add($"accountId", id);

                // Bản ghi trả về 
                int changedCount = await connection.ExecuteAsync(
                        procedure,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );

                return changedCount > 0;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Hàm kiểm tra khóa ngoại đến account cha đã thay đổi
        /// </summary>
        /// <param name="id">Id của của bản ghi</param>
        /// <param name="parentId">Id khóa ngoại trước khi thay đổi</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> CheckIsChangedParentIdAsync(Guid id, Guid? parentId)
        {
            if (parentId.Equals(Guid.Empty))
                parentId = null;
            // Kết nối với database
            var connection = await _msDatabase.GetOpenConnectionAsync();
            try
            {
                // Tên procedure
                string procedure = ProcedureResource.AccountCheckIsChangedParentId;

                // Tham số 
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add($"accountId", id);
                dynamicParams.Add($"parentId", parentId);

                // Bản ghi trả về 
                bool isChanged = await connection.QueryFirstAsync<bool>(
                        procedure,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );

                return isChanged;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Hàm kiểm tra số tài khoản cập nhật sẽ làm trùng với số tài khoản đã tồn tại
        /// </summary>
        /// <param name="id">Id của tài khoản</param>
        /// <param name="accountCode">Số tài khoản sau khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> CheckUpdateCodeBeDublicatedAsync(Guid id, string accountCode)
        {
            // Kết nối với database
            var connection = await _msDatabase.GetOpenConnectionAsync();
            try
            {
                // Tên procedure
                string procedure = ProcedureResource.AccountCheckUpdateCodeBeDublicated;

                // Tham số 
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add($"accountId", id);
                dynamicParams.Add($"accountCode", accountCode);

                // Bản ghi trả về 
                bool isChanged = await connection.QueryFirstAsync<bool>(
                        procedure,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );

                return isChanged;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
        }
    }
}

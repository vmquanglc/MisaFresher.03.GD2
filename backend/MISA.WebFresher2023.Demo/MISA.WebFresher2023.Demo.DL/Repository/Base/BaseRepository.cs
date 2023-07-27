using Dapper;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL.Model;
using System.Data;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    {
        #region Field
        /// <summary>
        /// Chuỗi kết nối 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        protected readonly IMSDatabase _msDatabase;
        #endregion

        #region Contructor
        public BaseRepository(IMSDatabase msDatabase)
        {
            _msDatabase = msDatabase;
        }
        #endregion

        #region Method
        /// <summary>
        /// Hàm xuất excel
        /// </summary>
        /// <typeparam name="TEntityExport">Loại bản ghi để xuất</typeparam>
        /// <returns>Danh sách bản ghi cần xuất</returns>
        /// Author: LeDucTiep (08/05/2023)
        public async Task<IEnumerable<TEntityExport>> GetExportAsync<TEntityExport>()
        {
            var table = typeof(TEntity).Name;

            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            string procedure = ProcedureResource.Export(table);

            // Gọi procedure 
            var res = await connection.QueryAsync<TEntityExport>(
                procedure,
                commandType: CommandType.StoredProcedure
            );

            // trả về kết quả
            return res;
        }

        /// <summary>
        /// Hàm xóa một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// Author: LeDucTiep (23/05/2023)
        public virtual async Task<int> DeleteAsync(Guid id)
        {
            // Tên bảng 
            var table = typeof(TEntity).Name;

            // Tên procedure
            string procedure = ProcedureResource.Delete(table);

            // Connection với database 
            var connection = await _msDatabase.GetOpenConnectionAsync();

            try
            {
                // Khởi tạo các tham số 
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add($"{table}Id", id);

                var countChanged = await connection.ExecuteAsync(
                    procedure,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

                return countChanged;
            }
            catch (Exception ex)
            {
                throw new InternalException();
            }

        }

        /// <summary>
        /// Hàm xóa nhiều bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// Author: LeDucTiep (23/05/2023)
        public virtual async Task<int> DeleteManyAsync(Guid[] arrayId)
        {
            // Số lượng bản ghi bị xóa
            int countChanged = 0;

            // Tên bảng 
            var table = typeof(TEntity).Name;

            // Tên procedure
            string procedure = ProcedureResource.DeleteMany(table);

            // Connection với database 
            var connection = await _msDatabase.GetOpenConnectionAsync();


            try
            {
                // Chuyển thành dạng danh sách
                List<Guid> listId = arrayId.ToList();

                // Còn thở thì còn xóa
                while (listId.Count > 0)
                {
                    string param = "";
                    int counterFlag = 0;

                    // Xóa mỗi lần 10 bản ghi
                    while (listId.Count > 0 && counterFlag < 10)
                    {
                        Guid guid = listId[0];

                        param += $"'{guid}'";

                        listId.RemoveAt(0);

                        counterFlag++;

                        // Nếu là phần tử cuối cùng thì không cần dấu ,
                        if (listId.Count > 0 && counterFlag < 10)
                        {
                            param += ",";
                        }
                    }

                    // Khởi tạo các tham số 
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add($"arrayId", param);

                    countChanged += await connection.ExecuteAsync(
                        procedure,
                        param: dynamicParams,
                        commandType: CommandType.StoredProcedure
                    );

                }
                return countChanged;
            }
            catch (Exception ex)
            {
                throw new InternalException();
            }
        }

        /// <summary>
        /// Hàm lấy một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>Giá trị của bản ghi</returns>
        /// Author: LeDucTiep (23/05/2023)
        public virtual async Task<TEntity?> GetAsync(Guid id)
        {
            // Tên bảng
            var table = typeof(TEntity).Name;

            // Tên procedure
            string procedure = ProcedureResource.Get(table);

            // Kết nối với database
            var connection = await _msDatabase.GetOpenConnectionAsync();

            try
            {
                // Tham số 
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add($"{table}Id", id);

                // Bản ghi trả về 
                var entity = await connection.QueryFirstOrDefaultAsync<TEntity>(
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

        /// <summary>
        /// Hàm thêm một bản ghi
        /// </summary>
        /// <param name="entity">Giá trị của bản ghi</param>
        /// <returns>Mã lỗi</returns>
        /// Author: LeDucTiep (23/05/2023)
        public virtual async Task<int> PostAsync(TEntity entity, Guid newId)
        {
            // Tên bảng
            var table = typeof(TEntity).Name;

            // Procedure
            string procedure = ProcedureResource.Add(table);

            // Mở kết nối tới database
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Các tham số
            var dynamicParams = new DynamicParameters();

            //// Tạo id mới
            //Guid newId = Guid.NewGuid();

            var properties = entity.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                // Tên thuộc tính
                var name = property.Name;

                // Gán id mới
                if (name == $"{table}Id")
                {
                    property.SetValue(entity, newId, null);

                    dynamicParams.Add($"{table}Id", newId);
                }

                // Bỏ qua ngày sửa và người sửa 
                if (name == "ModifiedBy" || name == "ModifiedDate")
                    continue;

                // Giá trị của tham số 
                object? value = property.GetValue(entity);

                if(property.PropertyType == typeof(Guid) || property.PropertyType == typeof(Guid?))
                {
                    if (value != null && value.Equals(Guid.Empty))
                    {
                        value = null;
                    }
                }

                // Thêm tham số 
                dynamicParams.Add(name, value);
            }

            // Thêm người tạo 
            dynamicParams.Add("CreatedBy", UserResource.Name);
            // Thêm ngày tạo 
            dynamicParams.Add("CreatedDate", DateTime.Now);


            try
            {
                //Gọi procedure
                int changedCount = await connection.ExecuteAsync(
                    procedure,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

                return changedCount;
            }
            catch (Exception ex)
            {
                throw new InternalException();
            }
        }

        /// <summary>
        /// Hàm update một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="entity">Giá trị của bản ghi</param>
        /// <returns>Mã lỗi</returns>
        /// Author: LeDucTiep (23/05/2023)
        public virtual async Task<int> UpdateAsync(Guid id, TEntity entity)
        {
            // Tên bảng 
            var table = typeof(TEntity).Name;

            // Tên procedure
            string procedure = ProcedureResource.Update(table);

            // Mở kết nối tới database
            var connection = await _msDatabase.GetOpenConnectionAsync();
            // Khởi tạo tham số 
            var dynamicParams = new DynamicParameters();

            // Duyệt qua tất cả thuộc tính của entity
            System.Reflection.PropertyInfo[] properties = entity.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                // Tên tham số
                var name = property.Name;

                // Gán id truyền vào 
                if (name == $"{table}Id")
                {
                    dynamicParams.Add($"{table}Id", id);
                    continue;
                }

                // Bỏ qua người tạo và ngày tạo
                if (name == "CreatedBy" || name == "CreatedDate")
                    continue;

                // Giá trị của tham số 
                var value = property.GetValue(entity);

                if (value != null && value.Equals(Guid.Empty))
                {
                    dynamicParams.Add(name, null);
                    continue;
                }

                // Thêm tham số
                dynamicParams.Add(name, value);
            }

            // Thêm người sửa 
            dynamicParams.Add("ModifiedBy", UserResource.Name);

            // Thêm ngày sửa 
            dynamicParams.Add("ModifiedDate", DateTime.Now);

            try
            {
                // Gọi procedure
                int changedCount = await connection.ExecuteAsync(
                    procedure,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

                return changedCount;
            }
            catch (Exception ex)
            {
                throw new InternalException();
            }
        }

        /// <summary>
        /// Hàm lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Author: LeDucTiep (27/05/2023)
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            // Tên bảng
            var table = typeof(TEntity).Name;

            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();
            try
            {
                // Tên procedure
                string procedure = ProcedureResource.GetAll(table);

                // Gọi đến procedure
                return await connection.QueryAsync<TEntity>(
                    procedure,
                    commandType: CommandType.StoredProcedure
                );
            }
            catch
            {
                throw new InternalException();
            }
        }

        /// <summary>
        /// Hàm kiểm tra bản ghi có tồn tại không
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="table">Tên table</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<bool> CheckExistedAsync(Guid id, string table = "")
        {
            // Tên bảng
            if (table.Equals(string.Empty))
                table = typeof(TEntity).Name;

            // Kết nối với database
            var connection = await _msDatabase.GetOpenConnectionAsync();

            try
            {
                // Tên procedure
                string procedure = ProcedureResource.CheckExistedById(table);

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
        /// Hàm lấy trang bản ghi
        /// </summary>
        /// <typeparam name="TEntityInPage">Loại bản ghi trong trang</typeparam>
        /// <param name="basePagingArgument">Tham số để phân trang</param>
        /// <returns>BasePage<TEntityInPage></returns>
        /// Author: LeDucTiep (08/05/2023)
        public virtual async Task<BasePage<TEntityInPage>> GetPageAsync<TEntityInPage>(BasePagingArgument basePagingArgument)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            string tableName = typeof(TEntity).Name;

            string procedure = ProcedureResource.GetPaging(tableName);

            // Tạo tham số đầu vào 
            // IN _offset: Số bản ghi bị bỏ qua
            // IN _limit: Số bản ghi được lấy
            // IN employeeSearchTerm: Từ khóa tìm kiếm, theo employeeCode hoặc FullName
            // OUT TotalRecord: Tổng số bản ghi tìm thấy
            var parameters = new DynamicParameters();
            int offset = (basePagingArgument.PageNumber - 1) * basePagingArgument.PageSize;
            parameters.Add("_offset", offset);
            parameters.Add("_limit", basePagingArgument.PageSize);
            parameters.Add("searchTerm", basePagingArgument.SearchTerm ?? "");
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
        #endregion
    }
}

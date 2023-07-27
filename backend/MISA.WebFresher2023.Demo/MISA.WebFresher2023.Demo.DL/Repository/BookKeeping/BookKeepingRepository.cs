using Dapper;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL.Entity;
using System.Data;
using static Dapper.SqlMapper;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class BookKeepingRepository : BaseRepository<BookKeeping>, IBookKeepingRepository
    {
        public BookKeepingRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }

        /// <summary>
        /// Hàm thêm nhiều bản ghi
        /// </summary>
        /// <param name="values">Danh sách bản ghi cần thêm</param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<int> AddManyAsync(string values)
        {
            // Tên procedure
            string procedure = ProcedureResource.BookKeepingAddMany;

            // Khởi tạo tham số 
            var dynamicParams = new DynamicParameters();

            // Thêm tham số 
            dynamicParams.Add("data", values);

            // Mở kết nối tới database
            var connection = await _msDatabase.GetOpenConnectionAsync();

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
        /// Hàm xóa các bản ghi không có trong danh sách
        /// </summary>
        /// <param name="receiptId">Id của phiếu thu</param>
        /// <param name="bookKeepingIds">Danh sách bookkeeping</param>
        /// <returns>Số bản ghi đã thay đối</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<int> DeleteNotInAsync(Guid receiptId, string bookKeepingIds)
        {
            // Tên procedure
            string procedure = ProcedureResource.BookKeepingDeleteNotIn;

            // Mở kết nối tới database
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Khởi tạo tham số 
            var dynamicParams = new DynamicParameters();

            // Thêm ...
            dynamicParams.Add("receiptId", receiptId);
            dynamicParams.Add("bookKeepingIds", bookKeepingIds);

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
        /// Hàm lấy danh sách bookkeeping bằng receiptid
        /// </summary>
        /// <param name="id">Id của phiếu thu</param>
        /// <returns>Danh sách bookkeeping</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<List<BookKeeping>> GetByReceiptIdAsync(Guid id)
        {
            // Tên procedure
            string procedure = ProcedureResource.BookKeepingGetByReceiptId;

            // Kết nối với database
            var connection = await _msDatabase.GetOpenConnectionAsync();
            try
            {
                // Tham số 
                var dynamicParams = new DynamicParameters();
                dynamicParams.Add($"receiptId", id);

                // Bản ghi trả về 
                var bookKeepings = (List<BookKeeping>)await connection.QueryAsync<BookKeeping>(
                    procedure,
                    param: dynamicParams,
                    commandType: CommandType.StoredProcedure
                );

                return bookKeepings;
            }
            catch (Exception ex)
            {
                throw new InternalException();
            }
        }
    }
}

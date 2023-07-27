using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public class ReceiptRepository : BaseRepository<Receipt>, IReceiptRepository
    {
        public ReceiptRepository(IMSDatabase msDatabase) : base(msDatabase)
        {
        }

        /// <summary>
        /// Hàm Kiểm tra mã code đang sửa có trùng với mã code trước khi sửa không
        /// </summary>
        /// <param name="employeeCode">Mã code đang sửa</param>
        /// <param name="itsCode">Mã code trước khi sửa</param>
        /// <returns>Bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> CheckEditCodeDuplicatedAsync(string receiptCode, string itsCode)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Tên procedure
            string procedure = ProcedureResource.ReceiptCheckDuplicatedCodeExceptItsCode;


            // Tạo các tham số 
            var parameters = new DynamicParameters();
            parameters.Add("receiptCode", receiptCode);
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
        /// Hàm kiểm tra mã code đã tồn tại chưa
        /// </summary>
        /// <param name="receiptCode">Mã code cần kiểm tra</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> CheckExistedCodeAsync(string receiptCode)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Tên procedure
            string procedure = ProcedureResource.ReceiptCheckDuplicatedCode;

            // Tạo các tham số 
            var parameters = new DynamicParameters();
            parameters.Add("receiptCode", receiptCode);

            // Gọi đến procedure
            bool result = await connection.QueryFirstAsync<bool>(
                procedure,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        /// <summary>
        /// Hàm thiết lập lại trạng thái ghi sổ
        /// </summary>
        /// <param name="isRecored">Trạng thái ghi sổ</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<int> SetRecordedAsync(Guid id, bool isRecorded)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Tên procedure
            string procedure = ProcedureResource.ReceiptSetRecorded;

            // Tạo các tham số 
            var parameters = new DynamicParameters();
            parameters.Add("receiptId", id);
            parameters.Add("isRecorded", isRecorded);

            // Gọi đến procedure
            int result = await connection.ExecuteAsync(
                procedure,
                param: parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        /// <summary>
        /// Hàm kiểm tra bản ghi đã ghi sổ chưa
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns></returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> CheckRecordedAsync(Guid id)
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            // Tên procedure
            string sql = "SELECT r.IsRecorded FROM receipt r WHERE r.ReceiptId = @receiptId";


            // Tạo các tham số 
            var parameters = new DynamicParameters();
            parameters.Add("receiptId", id);
         

            // Gọi đến procedure
            bool result = await connection.QueryFirstAsync(
                sql,
                param: parameters
            );

            return result;
        }

        /// <summary>
        /// Hàm lấy mã code mới
        /// </summary>
        /// <returns>Mã code mới</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<string> GetNewCodeAsync()
        {
            // Tạo connection
            var connection = await _msDatabase.GetOpenConnectionAsync();

            int numberUp = 1;

            // Tạo mã mới
            string sql = "SELECT MAX(r.ReceiptCode) FROM receipt r;";
            IEnumerable<string> enumerable = await connection.QueryAsync<string>(sql);
            string maxCode = enumerable.First();

            string preCode = "PT-";

            int index = maxCode.IndexOf("-"); // Tìm vị trí của dấu "-"
            if (index != -1) // Nếu tìm thấy dấu "-"
            {
                preCode = maxCode.Substring(0, index + 1); // Lấy ra chuỗi "PT-"
            }

            long code = GetNumbers(maxCode);

            while (true)
            {
                code += numberUp;
                string newReceiptCode = $"{preCode}{code.ToString().PadLeft(4, '0')}";

                // Kiểm tra mã mới có bị trùng không

                bool isExitsted = await CheckExistedCodeAsync(newReceiptCode);

                if (!isExitsted)
                {
                    return newReceiptCode;
                }
                numberUp++;
            }
        }




        /// <summary>
        /// Hàm chuyển chuỗi thành số
        /// </summary>
        /// <param name="input">Chuỗi cần chuyển</param>
        /// <returns>Số trong chuỗi</returns>
        /// Author: LeDucTiep (14/07/2023)
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

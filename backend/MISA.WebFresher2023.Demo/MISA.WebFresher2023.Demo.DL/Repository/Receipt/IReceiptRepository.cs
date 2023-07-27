using MISA.WebFresher2023.Demo.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public interface IReceiptRepository : IBaseRepository<Receipt>
    {
        /// <summary>
        /// Hàm Kiểm tra mã code đang sửa có trùng với mã code trước khi sửa không
        /// </summary>
        /// <param name="employeeCode">Mã code đang sửa</param>
        /// <param name="itsCode">Mã code trước khi sửa</param>
        /// <returns>Bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<bool> CheckEditCodeDuplicatedAsync(string employeeCode, string itsCode);

        /// <summary>
        /// Hàm kiểm tra mã code đã tồn tại chưa
        /// </summary>
        /// <param name="receiptCode">Mã code cần kiểm tra</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<bool> CheckExistedCodeAsync(string receiptCode);
        Task<bool> CheckRecordedAsync(Guid id);

        /// <summary>
        /// Hàm lấy mã code mới
        /// </summary>
        /// <returns>Mã code mới</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<string> GetNewCodeAsync();

        /// <summary>
        /// Hàm ghi sổ một bản ghi
        /// </summary>
        /// <param name="receipt"></param>
        /// <returns>int</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<int> SetRecordedAsync(Guid id, bool isRecorded);
    }
}

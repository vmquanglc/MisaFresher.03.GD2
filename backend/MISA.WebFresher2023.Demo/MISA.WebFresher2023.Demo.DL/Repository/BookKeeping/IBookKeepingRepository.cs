using MISA.WebFresher2023.Demo.DL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public interface IBookKeepingRepository : IBaseRepository<BookKeeping>
    {
        /// <summary>
        /// Hàm thêm nhiều bản ghi
        /// </summary>
        /// <param name="values">Danh sách bản ghi cần thêm</param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<int> AddManyAsync(string values);

        /// <summary>
        /// Hàm xóa các bản ghi không có trong danh sách
        /// </summary>
        /// <param name="receiptId">Id của phiếu thu</param>
        /// <param name="bookKeepingIds">Danh sách bookkeeping</param>
        /// <returns>Số bản ghi đã thay đối</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<int> DeleteNotInAsync(Guid receiptId, string bookKeepingIds);

        /// <summary>
        /// Hàm lấy danh sách bookkeeping bằng receiptid
        /// </summary>
        /// <param name="id">Id của phiếu thu</param>
        /// <returns>Danh sách bookkeeping</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<List<BookKeeping>> GetByReceiptIdAsync(Guid id);
    }
}

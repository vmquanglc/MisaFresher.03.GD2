using MISA.WebFresher2023.Demo.BL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public interface IReceiptService : IBaseService<ReceiptDto, ReceiptCreateDto, ReceiptUpdateDto>
    {
        /// <summary>
        /// Hàm kiểm tra mã code đã tồn tại chưa
        /// </summary>
        /// <param name="code">Mã code cần kiểm tra</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<bool> CheckCodeDuplicatedAsync(string code);

        /// <summary>
        /// Hàm kiểm tra mã code đang sửa có bị trùng
        /// </summary>
        /// <param name="employeeCode">Mã code đang sửa</param>
        /// <param name="itsCode">Mã code trước khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<bool> CheckEditCodeDuplicatedAsync(string employeeCode, string itsCode);

        /// <summary>
        /// Hàm lấy mã phiếu thu mới
        /// </summary>
        /// <returns>Mã phiếu thu mới</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task<string> GetNewCodeAsync();

        /// <summary>
        /// Hàm thiết lập lại trạng thái ghi sổ
        /// </summary>
        /// <param name="isRecored">Trạng thái ghi sổ</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<bool> SetRecordedAsync(Guid id, bool isRecorded);
    }
}

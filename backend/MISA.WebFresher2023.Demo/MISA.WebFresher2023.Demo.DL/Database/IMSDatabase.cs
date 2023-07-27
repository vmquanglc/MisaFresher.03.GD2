using MySqlConnector;
using System.Data.Common;
using System.Transactions;

namespace MISA.WebFresher2023.Demo.DL
{
    public interface IMSDatabase
    {
        /// <summary>
        /// Hàm bắt đầu transaction
        /// </summary>
        /// Author: LeDucTiep (12/07/2023)
        void BeginTransaction();

        /// <summary>
        /// Hàm đóng connection
        /// </summary>
        /// <returns>Task</returns>
        /// Author: LeDucTiep (12/07/2023)
        Task CloseConnectionAsync();

        /// <summary>
        /// Hàm commit
        /// </summary>
        /// Author: LeDucTiep (12/07/2023)
        void Commit();

        /// <summary>
        /// Hàm lấy connection đang mở
        /// </summary>
        /// <returns>Connection</returns>
        /// <exception cref="InternalException"></exception>
        /// Author: LeDucTiep (12/07/2023)
        Task<DbConnection> GetOpenConnectionAsync();

        /// <summary>
        /// Hàm rollback
        /// </summary>
        /// Author: LeDucTiep (12/07/2023)
        void Rollback();
    }
}

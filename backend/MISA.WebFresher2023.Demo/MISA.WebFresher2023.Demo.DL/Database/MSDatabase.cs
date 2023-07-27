using MISA.WebFresher2023.Demo.Common;
using MISA.WebFresher2023.Demo.Common.Constant;
using MISA.WebFresher2023.Demo.Common.MyException;
using MySqlConnector;
using System.Data;
using System.Data.Common;
using System.Transactions;

namespace MISA.WebFresher2023.Demo.DL
{
    /// <summary>
    /// Cơ sở dữ liệu
    /// </summary>
    /// Author: LeDucTiep (12/07/2023)
    public class MSDatabase : IMSDatabase
    {
        /// <summary>
        /// Chuỗi kết nối
        /// </summary>
        protected readonly string _connectionString;

        /// <summary>
        /// Kết nối
        /// </summary>
        public MySqlConnection _connection = null;

        /// <summary>
        /// Transaction của kết nôi
        /// </summary>
        public CommittableTransaction myTransaction = null;

        public MSDatabase(AppConfig appConfig)
        {
            _connectionString = appConfig.ConnectionString;
            _connection = new MySqlConnection(_connectionString);
        }

        ~MSDatabase()
        {
            CloseConnectionAsync().Wait();
        }

        /// <summary>
        /// Hàm bắt đầu transaction
        /// </summary>
        /// Author: LeDucTiep (12/07/2023)
        public void BeginTransaction()
        {
            GetOpenConnectionAsync().Wait();

            myTransaction = new CommittableTransaction();

            EnlistTransaction();
        }

        /// <summary>
        /// Hàm gắn transaction vào connection
        /// </summary>
        /// Author: LeDucTiep (12/07/2023)
        private void EnlistTransaction()
        {
            _connection.EnlistTransaction(myTransaction);
        }

        /// <summary>
        /// Hàm commit
        /// </summary>
        /// Author: LeDucTiep (12/07/2023)
        public void Commit()
        {
            if (myTransaction != null)
            {
                myTransaction.Commit();
                myTransaction.Dispose();
                myTransaction = null;
            }
        }

        /// <summary>
        /// Hàm rollback
        /// </summary>
        /// Author: LeDucTiep (12/07/2023)
        public void Rollback()
        {
            if (myTransaction != null)
            {
                myTransaction.Rollback();
                myTransaction.Dispose();
                myTransaction = null;
            }
        }

        /// <summary>
        /// Hàm lấy connection đang mở
        /// </summary>
        /// <returns>Connection</returns>
        /// <exception cref="InternalException"></exception>
        /// Author: LeDucTiep (12/07/2023)
        public virtual async Task<DbConnection> GetOpenConnectionAsync()
        {
            try
            {
                if (_connection == null || _connection.State == ConnectionState.Closed)
                {
                    // Mở kết nối 
                    await _connection.OpenAsync();
                }

                // Trả về kết nối
                return _connection;
            }
            catch
            {
                throw new InternalException(new() { (int)InternalErrorCode.ConnectDbError });
            }
        }

        /// <summary>
        /// Hàm đóng connection
        /// </summary>
        /// <returns>Task</returns>
        /// Author: LeDucTiep (12/07/2023)
        public virtual async Task CloseConnectionAsync()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
                await _connection.CloseAsync();
        }
    }
}

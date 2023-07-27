using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.WebFresher2023.Demo.DL.Entity;

namespace MISA.WebFresher2023.Demo.DL.Model
{
    /// <summary>
    /// Class trang nhân viên
    /// </summary>
    /// Author: LeDucTiep (23/05/2023)
    public class BasePage<TEntity>
    {
        #region Contructor
        public BasePage() { }
        public BasePage(int totalRecord, IEnumerable<TEntity>? entity)
        {
            TotalRecord = totalRecord;
            Data = entity;
        }
        #endregion

        #region Method
        /// <summary>
        /// Tổng số bản ghi 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        public int TotalRecord { get; set; }

        /// <summary>
        /// Danh sách nhân viên trả về
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        public IEnumerable<TEntity> Data { get; set; }
        #endregion
    }
}

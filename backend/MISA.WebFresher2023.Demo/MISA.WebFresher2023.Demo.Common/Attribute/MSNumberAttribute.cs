using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.WebFresher2023.Demo.Common.Attribute
{
    /// <summary>
    /// Attribute trường bắt buộc 
    /// </summary>
    /// Author: LeDucTiep (11/06/2023)
    [AttributeUsage(AttributeTargets.All)]
    public class MSNumberAttribute : System.Attribute
    {
        /// <summary>
        /// Mã lỗi nội bộ 
        /// </summary>
        /// Author: LeDucTiep (11/06/2023)
        public int ErrorCode { get; set; }

        /// <summary>
        /// Là hợp lệ 
        /// </summary>
        /// <param name="value">Chuỗi number</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (11/06/2023)
        public static bool IsValid(object value)
        {
            if (value == null) return true;

            try
            {
                string numbers = (string)value;

                Regex regex = new("^[0-9]*$");

                return regex.IsMatch(numbers);
            }
            catch { return true; }
        }
    }
}

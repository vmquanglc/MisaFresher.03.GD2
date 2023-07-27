namespace MISA.WebFresher2023.Demo.DL.Model
{
    /// <summary>
    /// Tham số để phân trang xã
    /// </summary>
    /// Author: LeDucTiep (12/07/2023)
    public class CommunePagingArgument : BasePagingArgument
    {
        /// <summary>
        /// Id của Huyện
        /// </summary>
        public Guid DistrictId { get; set; }
    }
}

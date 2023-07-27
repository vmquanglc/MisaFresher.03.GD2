namespace MISA.WebFresher2023.Demo.DL.Model
{
    /// <summary>
    /// Tham số để phân trang huyện
    /// </summary>
    /// Author: LeDucTiep (12/07/2023)
    public class DistrictPagingArgument : BasePagingArgument
    {
        /// <summary>
        /// Id của thành phố
        /// </summary>
        public Guid CityId { get; set; }
    }
}

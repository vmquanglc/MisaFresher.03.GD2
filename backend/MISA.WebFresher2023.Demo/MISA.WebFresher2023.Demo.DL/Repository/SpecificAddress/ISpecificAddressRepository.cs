using MISA.WebFresher2023.Demo.DL.Entity;

namespace MISA.WebFresher2023.Demo.DL.Repository
{
    public interface ISpecificAddressRepository : IBaseRepository<SpecificAddress>
    {
        /// <summary>
        /// Hàm lấy danh sách địa chỉ theo OtherLocationId
        /// </summary>
        /// <param name="otherLocationId">Id của bảng địa chỉ khác</param>
        /// <returns>Danh sách địa chỉ</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task<List<SpecificAddress>> GetByOtherLocationIdAsync(Guid otherLocationId);

        /// <summary>
        /// Hàm xóa các địa chỉ không có danh sách
        /// </summary>
        /// <param name="addressList">Danh sách id các bản ghi ko xóa, các id nằm trong dấu ' và cách nhau dấu ,</param>
        /// <param name="otherLocationId">Id của khóa ngoại đến bảng cha</param>
        /// <returns>void</returns>
        /// Author: LeDucTiep (14/07/2023)
        Task DeleteNotInAsync(string addressList, Guid otherLocationId);
    }
}

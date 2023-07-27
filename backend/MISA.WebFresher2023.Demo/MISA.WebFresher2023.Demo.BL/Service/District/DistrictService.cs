using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.Common.Attribute;
using MISA.WebFresher2023.Demo.Common.Constant;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Model;
using MISA.WebFresher2023.Demo.DL.Repository;
using System.Reflection;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public class DistrictService : BaseService<District, DistrictDto, DistrictCreateDto, DistrictUpdateDto>, IDistrictService
    {
        public IDistrictRepository _districtRepository;
        public DistrictService(IDistrictRepository districtRepository,
            IMSDatabase msDatabase,
            IMapper mapper) : base(msDatabase, districtRepository, mapper)
        {
            _districtRepository = districtRepository;
        }

        /// <summary>
        /// Hàm lấy một trang
        /// </summary>
        /// <param name="districtPagingArgument">Tham số để phân trang</param>
        /// <returns>Trang bản ghi</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<BasePage<DistrictDto>> GetPageAsync(DistrictPagingArgument districtPagingArgument)
        {
            try
            {
                List<int> errorCodes = new();

                // Lỗi kích thước trang 
                if (districtPagingArgument.PageSize <= 0)
                {
                    errorCodes.Add((int)PagingErrorCode.InvalidPageSize);
                }

                // Lỗi số thứ tự trang 
                if (districtPagingArgument.PageNumber <= 0)
                {
                    errorCodes.Add((int)PagingErrorCode.InvalidPageNumber);
                }

                // Kiểm tra độ dài chuỗi tìm kiếm 
                PropertyInfo[] properties = typeof(DistrictPagingArgument).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var value = property.GetValue(districtPagingArgument, null);

                    // Lỗi độ dài từ khóa tìm kiếm
                    var attributeMaxLength = (MSMaxLengthAttribute?)property.GetCustomAttributes(typeof(MSMaxLengthAttribute), false).FirstOrDefault();

                    if (attributeMaxLength != null && value != null && value.ToString().Length > attributeMaxLength.Length)
                    {
                        errorCodes.Add(attributeMaxLength.ErrorCode);
                    }
                }

                // Nếu có lỗi 
                if (errorCodes.Count > 0)
                    throw new BadRequestException(errorCodes);

                BasePage<DistrictDto> page = await _districtRepository.GetPageAsync<DistrictDto>(districtPagingArgument);

                return page;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Hàm lấy một bản ghi 
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="cityId">Mã thành phố</param>
        /// <returns>Bản ghi</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<DistrictDto?> GetAsync(Guid id, Guid? cityId = null)
        {
            try
            {

                // Kiểm tra lỗi
                if (id.Equals(Guid.Empty))
                {
                    List<int> errorList = new() { (int)RequestErrorCode.GuidInvalid };
                    throw new BadRequestException(errorList);
                }

                // Thực hiện
                var entity = await _districtRepository.GetAsync(id, cityId);

                // Trả về
                var entityDto = _mapper.Map<DistrictDto>(entity);

                return entityDto;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }

    }
}

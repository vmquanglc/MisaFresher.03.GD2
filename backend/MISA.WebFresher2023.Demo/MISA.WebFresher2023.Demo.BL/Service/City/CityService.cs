using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.BL.Service;
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
    public class CityService : BaseService<City, CityDto, CityCreateDto, CityUpdateDto>, ICityService
    {
        ICityRepository _cityRepository;
        public CityService(ICityRepository cityRepository,
            IMSDatabase msDatabase,
            IMapper mapper) : base(msDatabase, cityRepository, mapper)
        {
            _cityRepository = cityRepository;
        }

        /// <summary>
        /// Hàm lấy trang bản ghi
        /// </summary>
        /// <param name="basePagingArgument">Tham số để phân trang</param>
        /// <returns>Trang thành phố</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<BasePage<CityDto>> GetPageAsync(CityPagingArgument cityPagingArgument)
        {
            try
            {
                List<int> errorCodes = new();

                // Lỗi kích thước trang 
                if (cityPagingArgument.PageSize <= 0)
                {
                    errorCodes.Add((int)PagingErrorCode.InvalidPageSize);
                }

                // Lỗi số thứ tự trang 
                if (cityPagingArgument.PageNumber <= 0)
                {
                    errorCodes.Add((int)PagingErrorCode.InvalidPageNumber);
                }

                // Kiểm tra độ dài chuỗi tìm kiếm 
                PropertyInfo[] properties = typeof(CityPagingArgument).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var value = property.GetValue(cityPagingArgument, null);

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

                BasePage<CityDto> page = await _cityRepository.GetPageAsync<CityDto>(cityPagingArgument);

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
        /// <param name="nationId">Id quốc gia</param>
        /// <returns>Thành phố</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<CityDto?> GetAsync(Guid id, Guid? nationId = null)
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
                var entity = await _cityRepository.GetAsync(id, nationId);

                // Trả về
                var entityDto = _mapper.Map<CityDto>(entity);

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

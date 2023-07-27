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
    public class CommuneService : BaseService<Commune, CommuneDto, CommuneCreateDto, CommuneUpdateDto>, ICommuneService
    {
        public ICommuneRepository _communeRepository;
        public CommuneService(ICommuneRepository communeRepository,
            IMSDatabase msDatabase,
            IMapper mapper) : base(msDatabase, communeRepository, mapper)
        {
            _communeRepository = communeRepository;
        }

        /// <summary>
        /// Hàm lấy trang bản ghi
        /// </summary>
        /// <param name="communePagingArgument">Tham số để phân trang</param>
        /// <returns>Trang bản ghi</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<BasePage<CommuneDto>> GetPageAsync(CommunePagingArgument communePagingArgument)
        {
            try
            {
                List<int> errorCodes = new();

                // Lỗi kích thước trang 
                if (communePagingArgument.PageSize <= 0)
                {
                    errorCodes.Add((int)PagingErrorCode.InvalidPageSize);
                }

                // Lỗi số thứ tự trang 
                if (communePagingArgument.PageNumber <= 0)
                {
                    errorCodes.Add((int)PagingErrorCode.InvalidPageNumber);
                }

                // Kiểm tra độ dài chuỗi tìm kiếm 
                PropertyInfo[] properties = typeof(CommunePagingArgument).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var value = property.GetValue(communePagingArgument, null);

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

                BasePage<CommuneDto> page = await _communeRepository.GetPageAsync<CommuneDto>(communePagingArgument);

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
        /// <param name="districtId">Id của Huyện</param>
        /// <returns>Xã</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<CommuneDto?> GetAsync(Guid id, Guid? districtId = null)
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
                var entity = await _communeRepository.GetAsync(id, districtId);

                // Trả về
                var entityDto = _mapper.Map<CommuneDto>(entity);

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

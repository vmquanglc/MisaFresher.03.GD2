using AutoMapper;
using MISA.WebFresher2023.Demo.BL.Dto;
using MISA.WebFresher2023.Demo.Common.Attribute;
using MISA.WebFresher2023.Demo.Common.Constant;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Entity;
using MISA.WebFresher2023.Demo.DL.Repository;
using System.Reflection;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    public class ReceiptService : BaseService<Receipt, ReceiptDto, ReceiptCreateDto, ReceiptUpdateDto>, IReceiptService
    {
        IBookKeepingRepository _bookKeepingRepository;
        IReceiptRepository _receiptRepository;

        public ReceiptService(
            IReceiptRepository receiptRepository,
            IMSDatabase msDatabase,
            IBookKeepingRepository bookKeepingRepository,
            IMapper mapper) : base(msDatabase, receiptRepository, mapper)
        {
            _bookKeepingRepository = bookKeepingRepository;
            _receiptRepository = receiptRepository;
        }

        /// <summary>
        /// Hàm kiểm tra mã code đã tồn tại chưa
        /// </summary>
        /// <param name="code">Mã code cần kiểm tra</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<bool> CheckCodeDuplicatedAsync(string code)
        {
            try
            {
                PropertyInfo? property = typeof(ReceiptDto).GetProperty("ReceiptCode");

                if (property != null)
                {

                    // Xét độ dài 
                    var attributeMaxLength = (MSMaxLengthAttribute?)property.GetCustomAttributes(typeof(MSMaxLengthAttribute), false).FirstOrDefault();

                    if (attributeMaxLength != null && code != null)
                    {
                        int valueLength = code.Length;
                        int maxLength = attributeMaxLength.Length;
                        bool isTooLong = valueLength > maxLength;
                        if (isTooLong)
                        {
                            List<int> errorList = new() { (int)ReceiptErrorCode.CodeTooLong };
                            throw new BadRequestException(errorList);
                        }
                    }
                }

                return await _receiptRepository.CheckExistedCodeAsync(code);
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
        /// Hàm kiểm tra mã code đang sửa có bị trùng
        /// </summary>
        /// <param name="employeeCode">Mã code đang sửa</param>
        /// <param name="itsCode">Mã code trước khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<bool> CheckEditCodeDuplicatedAsync(string receiptCode, string itsCode)
        {
            try
            {
                PropertyInfo? property = typeof(ReceiptDto).GetProperty("ReceiptCode");

                if (property != null)
                {

                    // Xét độ dài 
                    var attributeMaxLength = (MSMaxLengthAttribute?)property.GetCustomAttributes(typeof(MSMaxLengthAttribute), false).FirstOrDefault();

                    if (attributeMaxLength != null && receiptCode != null)
                    {
                        int valueLength = receiptCode.Length;
                        int maxLength = attributeMaxLength.Length;
                        bool isTooLong = valueLength > maxLength;
                        if (isTooLong)
                        {
                            List<int> errorList = new() { (int)ReceiptErrorCode.CodeTooLong };
                            throw new BadRequestException(errorList);
                        }
                    }
                }

                return await _receiptRepository.CheckEditCodeDuplicatedAsync(receiptCode, itsCode);
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
        /// Hàm lấy mã phiếu thu mới
        /// </summary>
        /// <returns>Mã phiếu thu mới</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<string> GetNewCodeAsync()
        {
            try
            {
                return await _receiptRepository.GetNewCodeAsync();
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
        /// Lấy một bản ghi theo Id 
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>Bản ghi</returns>
        /// Author: LeDucTiep (23/05/2023)
        public override async Task<ReceiptDto?> GetAsync(Guid id)
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
                var receipt = await _baseRepository.GetAsync(id);

                ReceiptDto receiptDto = _mapper.Map<ReceiptDto>(receipt);

                if (receiptDto != null)
                {
                    List<BookKeeping> bookKeepings = await _bookKeepingRepository.GetByReceiptIdAsync(id);
                    receiptDto.BookKeepings = bookKeepings;
                }

                return receiptDto;
            }
            catch
            {
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Sửa một bản ghi 
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="updateDto">Nội dung sửa</param>
        /// <returns></returns>
        /// Author: LeDucTiep (23/05/2023)
        public override async Task<int> UpdateAsync(Guid id, ReceiptUpdateDto receiptUpdateDto)
        {
            _msDatabase.BeginTransaction();
            try
            {
                ReceiptDto receiptDto = _mapper.Map<ReceiptDto>(receiptUpdateDto);

                List<int> errorCodes = new();

                errorCodes.AddRange(Validate(receiptDto));

                try
                {
                    errorCodes.AddRange(await UpdateValidate(id, receiptDto));
                }
                catch { }

                if (errorCodes.Any())
                    throw new BadRequestException(errorCodes);

                string dataToAddBookKeeping = "";
                string bookKeepingIds = "";

                if (receiptUpdateDto != null && receiptUpdateDto.BookKeepings != null)
                {
                    BookKeepingUpdateDto[] bookKeepingUpdateDtos = receiptUpdateDto.BookKeepings;

                    // Duyệt qua các hạch toán 
                    foreach (var bookKeepingUpdateDto in bookKeepingUpdateDtos)
                    {
                        // Nếu dữ liệu ko thay đổi thì bỏ qua
                        if (bookKeepingUpdateDto.isNotChanged == true)
                            continue;

                        // Gắn khóa ngoại
                        bookKeepingUpdateDto.ReceiptId = id;

                        // Kiểm tra tồn tại chưa
                        bool isNewBookKeeping = true;

                        BookKeeping bookKeeping = _mapper.Map<BookKeeping>(bookKeepingUpdateDto);

                        if (bookKeepingUpdateDto.BookKeepingId != null)
                        {
                            bool isExisted = await _bookKeepingRepository.CheckExistedAsync((Guid)bookKeepingUpdateDto.BookKeepingId);

                            // Nếu tồn tại thì update 
                            if (isExisted)
                            {
                                await _bookKeepingRepository.UpdateAsync((Guid)bookKeepingUpdateDto.BookKeepingId, bookKeeping);
                                isNewBookKeeping = false;
                            }
                        }

                        // Nếu chưa tồn tại
                        if (isNewBookKeeping)
                        {
                            bookKeeping.BookKeepingId = Guid.NewGuid();

                            string row = $"('{bookKeeping.ReceiptId}','{bookKeeping.BookKeepingId}','{bookKeeping.Note}','{bookKeeping.DebitAccountId}','{bookKeeping.CollectAccountId}',{bookKeeping.AmountOfMoney},'{UserResource.Name}',NULL)";

                            row = row.Replace("''", "NULL");
                            row = row.Replace("'00000000-0000-0000-0000-000000000000'", "NULL");

                            dataToAddBookKeeping += row + ",";
                        }

                        bookKeepingIds += $"'{bookKeeping.BookKeepingId}',";
                    }
                }

                if (dataToAddBookKeeping.Length > 1)
                {
                    dataToAddBookKeeping = dataToAddBookKeeping[..^1];

                    await _bookKeepingRepository.AddManyAsync(dataToAddBookKeeping);
                }

                if (bookKeepingIds.Length > 1)
                {
                    bookKeepingIds = bookKeepingIds[..^1];

                    await _bookKeepingRepository.DeleteNotInAsync(id, bookKeepingIds);
                }

                // Thêm trường id để trả về
                Receipt _receipt = _mapper.Map<Receipt>(receiptUpdateDto);

                // Update
                int changedCount = await _baseRepository.UpdateAsync(id, _receipt);

                _msDatabase.Commit();

                return changedCount;
            }
            catch (Exception ex)
            {
                _msDatabase.Rollback();
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Hàm thêm một bản ghi 
        /// </summary>
        /// <param name="entityCreateDto"></param>
        /// <returns>TEntity</returns>
        /// Author: LeDucTiep (23/05/2023)
        public override async Task<ReceiptDto> PostAsync(ReceiptCreateDto receiptCreateDto)
        {
            _msDatabase.BeginTransaction();

            Guid receiptId = Guid.NewGuid();
            try
            {
                ReceiptDto receiptDto = _mapper.Map<ReceiptDto>(receiptCreateDto);

                List<int> errorCodes = new();

                errorCodes.AddRange(Validate(receiptDto));

                try
                {
                    List<int> ints = await PostValidate(receiptDto);

                    errorCodes.AddRange(ints);
                }
                catch { }


                if (errorCodes.Any())
                    throw new BadRequestException(errorCodes);

                Receipt receipt1 = _mapper.Map<Receipt>(receiptCreateDto);

                int changedCount = await _baseRepository.PostAsync(receipt1, receiptId);

                if (changedCount == 0)
                    throw new InternalException();

                ReceiptDto receipt2 = _mapper.Map<ReceiptDto>(receipt1);

                if (receiptCreateDto != null && receiptCreateDto.BookKeepings != null)
                {
                    BookKeepingCreateDto[] bookKeepingCreateDtos = receiptCreateDto.BookKeepings;

                    string dataToAddBookKeeping = "";

                    // Duyệt qua các hạch toán 
                    foreach (var bookKeeping in bookKeepingCreateDtos)
                    {
                        bookKeeping.ReceiptId = receiptId;

                        bookKeeping.BookKeepingId = Guid.NewGuid();

                        string row = $"('{bookKeeping.ReceiptId}','{bookKeeping.BookKeepingId}','{bookKeeping.Note}','{bookKeeping.DebitAccountId}','{bookKeeping.CollectAccountId}',{bookKeeping.AmountOfMoney},'{UserResource.Name}',NULL)";

                        row = row.Replace("''", "NULL");
                        row = row.Replace("'00000000-0000-0000-0000-000000000000'", "NULL");

                        dataToAddBookKeeping += row + ",";
                    }

                    if (dataToAddBookKeeping.Length > 1)
                    {
                        dataToAddBookKeeping = dataToAddBookKeeping[..^1];

                        await _bookKeepingRepository.AddManyAsync(dataToAddBookKeeping);
                    }
                }

                _msDatabase.Commit();

                return receipt2;
            }
            catch (Exception ex)
            {
                _msDatabase.Rollback();
                throw;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Hàm thiết lập lại trạng thái ghi sổ
        /// </summary>
        /// <param name="isRecored">Trạng thái ghi sổ</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<bool> SetRecordedAsync(Guid id, bool isRecorded)
        {
            try
            {
                if (id.Equals(Guid.Empty))
                {
                    throw new BadRequestException();
                }

                return await _receiptRepository.SetRecordedAsync(id, isRecorded) != 0;
            }
            finally
            {
                await _msDatabase.CloseConnectionAsync();
            }
        }

        /// <summary>
        /// Hàm validate theo từng loại service
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// Author: LeDucTiep (09/06/2023)
        public virtual async Task<List<int>> DeleteValidate(Guid id)
        {
            List<int> errorCodes = new();

            bool isRecorded = await _receiptRepository.CheckRecordedAsync(id);

            if (isRecorded)
            {
                errorCodes.Add((int)ReceiptErrorCode.DeleteBooked);
            }

            // Trả về danh sách rỗng
            return errorCodes;
        }
    }
}

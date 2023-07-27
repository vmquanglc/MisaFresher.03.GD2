using AutoMapper;
using ClosedXML.Excel;
using MISA.WebFresher2023.Demo.Common.Attribute;
using MISA.WebFresher2023.Demo.Common.Constant;
using MISA.WebFresher2023.Demo.Common.MyException;
using MISA.WebFresher2023.Demo.Common.Resource;
using MISA.WebFresher2023.Demo.DL;
using MISA.WebFresher2023.Demo.DL.Model;
using MISA.WebFresher2023.Demo.DL.Repository;
using MISA.WebFresher2023.Demo.Enum;
using System.Globalization;
using System.Reflection;

namespace MISA.WebFresher2023.Demo.BL.Service
{
    /// <summary>
    /// Class dịch vụ cơ bản
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TEntityCreateDto"></typeparam>
    /// <typeparam name="TEntityUpdateDto"></typeparam>
    /// Author: LeDucTiep (23/05/2023)
    public class BaseService<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto> : IBaseService<TEntityDto, TEntityCreateDto, TEntityUpdateDto>
    {
        #region Field
        protected readonly IBaseRepository<TEntity> _baseRepository;

        protected IMSDatabase _msDatabase;

        protected readonly IMapper _mapper;
        #endregion

        #region Contructor 
        public BaseService(
            IMSDatabase msDatabase,
            IBaseRepository<TEntity> baseRepository,
            IMapper mapper)
        {
            _baseRepository = baseRepository;
            _msDatabase = msDatabase;
            _mapper = mapper;
        }
        #endregion

        #region Method

        /// <summary>
        /// Hàm xuất khẩu dữ liệu thành file excel
        /// </summary>
        /// <param name="baseExportArgument">Tham số để xuất khẩu</param>
        /// <returns>Excel</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<XLWorkbook> ExportAsync(BaseExportArgument baseExportArgument)
        {
            if (baseExportArgument == null)
            {
                throw new BadRequestException();
            }

            var table = typeof(TEntity).Name;

            try
            {
                // Tạo ra workbook 
                XLWorkbook xlWorkbook = new();

                // Tạo sheet 
                IXLWorksheet sheet1 = xlWorkbook.Worksheets.Add(ExportExcelResource.SheetName(table));

                // Nạp dữ liêu cho table 
                // Lấy danh sách các 

                PropertyInfo[] properties = typeof(TEntityDto).GetProperties();

                List<string> columns = new();

                foreach (string col in baseExportArgument.Fields)
                {
                    bool isContains = false;
                    foreach (PropertyInfo property in properties)
                    {
                        // Tên thuộc tính 
                        var field = property.Name;

                        if (col == field)
                        {
                            isContains = true;
                        }
                    }

                    if (isContains)
                    {
                        columns.Add(col);
                    }
                }

                baseExportArgument.Fields = columns.ToArray();

                IEnumerable<TEntityDto> entityList = await _baseRepository.GetExportAsync<TEntityDto>();

                // Ghi thông tin lên sheet 
                BaseService<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto>.LoadExportData(sheet1, entityList, baseExportArgument);

                //// Thêm style
                BaseService<TEntity, TEntityDto, TEntityCreateDto, TEntityUpdateDto>.StyleSheet(sheet1, entityList, baseExportArgument);

                // Trả về workbook 
                return xlWorkbook;
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
        /// Hàm style cho excel
        /// </summary>
        /// <param name="sheet">Trang excel</param>
        /// <param name="entityList">Danh sách dữ liệu xuất</param>
        /// <param name="baseExportArgument">Tham số để xuất</param>
        /// Author: LeDucTiep (12/07/2023)
        private static void StyleSheet(IXLWorksheet sheet, IEnumerable<TEntityDto> entityList, BaseExportArgument baseExportArgument)
        {
            int rowNumber = entityList.Count() + 3;

            int columnNumber = baseExportArgument.Fields.Length + 1;
            if (baseExportArgument.Fields.Length == 0)
            {
                columnNumber = typeof(TEntityDto).GetProperties().Length + 1;
            }

            // Định dạng lại sheet 
            // Định dạng border 2 dòng đầu 
            sheet.Range(1, 1, 1, columnNumber).Merge();
            sheet.Range(2, 1, 2, columnNumber).Merge();

            // Độ cao của dòng 
            sheet.Row(1).Height = 20.5;
            sheet.Row(2).Height = 22;
            sheet.Rows(3, rowNumber).Height = 15;

            // Định dạng cho toàn trang
            var tempRangeStyle = sheet.Range(1, 1, rowNumber, columnNumber).Style;
            tempRangeStyle.Alignment.WrapText = true;
            tempRangeStyle.Border.InsideBorder = XLBorderStyleValues.Thin;
            tempRangeStyle.Border.OutsideBorder = XLBorderStyleValues.Thin;

            // Định dạng cho 3 dòng đầu
            tempRangeStyle = sheet.Range(1, 1, 3, columnNumber).Style;
            tempRangeStyle.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            tempRangeStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            tempRangeStyle.Font.FontName = "Arial";
            tempRangeStyle.Font.Bold = true;

            // Định dạng màu border 2 dòng đầu 
            sheet.Range(1, 1, 1, columnNumber).Style.Border.OutsideBorderColor = XLColor.LightGray;
            sheet.Range(2, 1, 2, columnNumber).Style.Border.OutsideBorderColor = XLColor.LightGray;

            // Định dạng table có border màu đen 
            tempRangeStyle = sheet.Range(3, 1, rowNumber, columnNumber).Style;
            tempRangeStyle.Border.InsideBorderColor = XLColor.Black;
            tempRangeStyle.Border.OutsideBorderColor = XLColor.Black;

            // Định dạng Font cho dòng đầu 
            tempRangeStyle = sheet.Cell("A1").Style;
            tempRangeStyle.Font.FontSize = 16;

            // Định dạng Font cho tên cột  
            tempRangeStyle = sheet.Range(3, 1, 3, columnNumber).Style;
            tempRangeStyle.Font.FontSize = 10;
            tempRangeStyle.Fill.BackgroundColor = XLColor.FromArgb(255, 216, 216, 216);

            // Định dạng Text cho dữ liệu trong bảng 
            tempRangeStyle = sheet.Range(4, 1, rowNumber, columnNumber).Style;
            tempRangeStyle.Font.FontName = "Times New Roman";
            tempRangeStyle.Font.FontSize = 11;
            tempRangeStyle.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            tempRangeStyle.Alignment.Vertical = XLAlignmentVerticalValues.Bottom;

            // Căn giữa ngày tháng

            var properties = typeof(TEntityDto).GetProperties();
            if (baseExportArgument.Fields.Length == 0)
            {
                int i = 1;
                foreach (var field in properties)
                {
                    i++;
                    if (field.PropertyType.Equals(typeof(DateTime)) || field.PropertyType.Equals(typeof(DateTime?)))
                    {
                        sheet.Range(4, i, rowNumber, i).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }
                }
            }
            else
            {
                int i = 1;
                foreach (string col in baseExportArgument.Fields)
                {
                    i++;
                    bool isDateTime = false;
                    foreach (var field in properties)
                    {
                        if (col == field.Name && (field.PropertyType.Equals(typeof(DateTime)) || field.PropertyType.Equals(typeof(DateTime?))))
                        {
                            isDateTime = true;
                        }
                    }
                    if (isDateTime)
                    {
                        sheet.Range(4, i, rowNumber, i).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }
                }
            }
        }

        /// <summary>
        /// Hàm ghi dữ liệu xuất vào trang excel
        /// </summary>
        /// <param name="sheet">Trang excel</param>
        /// <param name="entityList">Danh sách dữ liệu xuất</param>
        /// <param name="baseExportArgument">Tham số đê xuất</param>
        /// Author: LeDucTiep (12/07/2023)
        private static void LoadExportData(IXLWorksheet sheet, IEnumerable<TEntityDto> entityList, BaseExportArgument baseExportArgument)
        {
            var table = typeof(TEntity).Name;

            PropertyInfo[] properties = typeof(TEntityDto).GetProperties();

            // Tiêu đề của bảng
            sheet.Cell("A1").Value = ExportExcelResource.SheetTitle(table);

            // Danh sách tên các cột 
            List<string> headers = new();
            List<string> fields = new();

            headers.Add(ExportExcelResource.NumericalOrder);

            if (baseExportArgument.Fields.Length > 0)
            {
                foreach (var field in baseExportArgument.Fields)
                {
                    headers.Add(ExportExcelResource.Header(field));
                    fields.Add(field);
                }
            }
            else
            {
                foreach (PropertyInfo property in properties)
                {
                    // Tên thuộc tính 
                    var field = property.Name;
                    headers.Add(ExportExcelResource.Header(field));
                    fields.Add(field);
                }
            }

            // Gán tên cho các cột 
            for (int i = 0; i < headers.Count; i++)
                sheet.Cell(3, i + 1).Value = headers[i];


            // Danh sách độ rộng của các cột 
            int[] arrayColumnWidth = new int[headers.Count];

            arrayColumnWidth[0] = 3;

            // Số thứ tự 
            int rowCount = 0;

            // Duyệt qua các bản ghi
            foreach (var entity in entityList)
            {
                rowCount++;

                // Số thứ tự cột 
                int columnCount = 1;

                // Ghi giá trị cho cột số thứ tự 
                sheet.Cell(3 + rowCount, columnCount).Value = rowCount;

                // Tìm ra độ rộng lớn nhất của cột số thứ tự
                arrayColumnWidth[0] = Math.Max(arrayColumnWidth[0], $"{rowCount}".Length);

                // Duyệt qua các thuộc tính
                foreach (string col in fields)

                    foreach (PropertyInfo property in properties)
                    {
                        // Tên thuộc tính 
                        var field = property.Name;

                        if (col != field)
                        {
                            continue;
                        }

                        columnCount++;


                        // Giá trị ghi lên excel 
                        string value = "";

                        // Mỗi kiểu dữ liệu có một cách ghi riêng
                        Type type = property.PropertyType;

                        // Kiểu giới tính 
                        if (type == typeof(Gender?))
                        {
                            Gender? gender = (Gender?)property.GetValue(entity);

                            if (gender == Gender.Nu)
                                value = ExportExcelResource.Female;
                            else if (gender == Gender.Nam)
                                value = ExportExcelResource.Male;
                        }
                        // Kiểu ngày tháng
                        else if (type == typeof(DateTime?))
                        {
                            DateTime? dateTime = (DateTime?)property.GetValue(entity);

                            if (dateTime != null)
                            {
                                value = ((DateTime)dateTime).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            }
                        }
                        // Kiểu số hoặc chuỗi 
                        else
                        {
                            value = property.GetValue(entity)?.ToString() ?? "";
                        }

                        // Ghi dữ liệu cho 1 ô excel
                        sheet.Cell(3 + rowCount, columnCount).Value = value;

                        // Tìm ra ô dài nhất 
                        arrayColumnWidth[columnCount - 1] = Math.Max(arrayColumnWidth[columnCount - 1], value.Length);
                    }
            }

            // So sánh ô dài nhất với ô tiêu đề
            for (int index = 0; index < arrayColumnWidth.Length; index++)
            {
                int headerLength = headers[index].Length;
                arrayColumnWidth[index] = Math.Max(arrayColumnWidth[index], headerLength);
            }

            // Tìm ra độ rộng hợp lý của cột 
            for (int index = 0; index < arrayColumnWidth.Length; index++)
            {
                    sheet.Column(index + 1).Width = arrayColumnWidth[index] > 30 ? 30 : arrayColumnWidth[index] * 1.5;
            }
        }

        /// <summary>
        /// Xóa một bản ghi theo id 
        /// </summary>
        /// <param name="id">Id của bản ghi </param>
        /// <returns>Số bản ghi đã xóa</returns>
        /// Author: LeDucTiep (23/05/2023)
        public virtual async Task<int> DeleteAsync(Guid id)
        {
            _msDatabase.BeginTransaction();

            try
            {
                // Kiểm tra lỗi
                List<int> errorCodes = await DeleteValidate(id);
                if (errorCodes.Any())
                    throw new BadRequestException(errorCodes);

                /// Xóa 
                int changedCount = await _baseRepository.DeleteAsync(id);

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
        /// Xóa một bản ghi theo id 
        /// </summary>
        /// <param name="id">Id của bản ghi </param>
        /// <returns>Task</returns>
        /// <exception cref="NotFoundException">Lỗi không tìm thấy </exception>
        /// Author: LeDucTiep (23/05/2023)
        public virtual async Task<int> DeleteManyAsync(Guid[] arrayId)
        {
            _msDatabase.BeginTransaction();

            try
            {
                /// Xóa và nhận về mã lỗi 
                int changedCount = await _baseRepository.DeleteManyAsync(arrayId);

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
        /// Hàm lấy một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi cần lấy </param>
        /// <returns>TEntityDto?</returns>
        /// Author: LeDucTiep (23/05/2023)
        public virtual async Task<TEntityDto?> GetAsync(Guid id)
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
                var entity = await _baseRepository.GetAsync(id);

                // Trả về
                var entityDto = _mapper.Map<TEntityDto>(entity);

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

        /// <summary>
        /// Thêm một bản ghi
        /// </summary>
        /// <param name="entity">Loại bản ghi </param>
        /// <returns>TEntity</returns>
        /// Author: LeDucTiep (23/05/2023)
        public virtual async Task<TEntityDto> PostAsync(TEntityCreateDto entity)
        {
            _msDatabase.BeginTransaction();

            try
            {
                TEntityDto entityDto = _mapper.Map<TEntityDto>(entity);

                List<int> errorCodes = new();

                errorCodes.AddRange(Validate(entityDto));

                try
                {
                    List<int> ints = await PostValidate(entityDto);

                    errorCodes.AddRange(ints);
                }
                catch { }


                if (errorCodes.Any())
                    throw new BadRequestException(errorCodes);

                TEntity entity1 = _mapper.Map<TEntity>(entity);

                int changedCount = await _baseRepository.PostAsync(entity1, Guid.NewGuid());

                if (changedCount == 0)
                    throw new InternalException();

                TEntityDto entity2 = _mapper.Map<TEntityDto>(entity1);

                _msDatabase.Commit();

                return entity2;
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
        /// Hàm update một bản ghi
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="entity">Giá trị bản ghi</param>
        /// Author: LeDucTiep (23/05/2023)
        public virtual async Task<int> UpdateAsync(Guid id, TEntityUpdateDto entity)
        {
            _msDatabase.BeginTransaction();

            try
            {
                TEntityDto entityDto = _mapper.Map<TEntityDto>(entity);

                List<int> errorCodes = new();

                errorCodes.AddRange(Validate(entityDto));

                try
                {
                    errorCodes.AddRange(await UpdateValidate(id, entityDto));
                }
                catch { }

                if (errorCodes.Any())
                    throw new BadRequestException(errorCodes);

                // Thêm trường id để trả về
                TEntity _entity = _mapper.Map<TEntity>(entity);

                // Update
                int changedCount = await _baseRepository.UpdateAsync(id, _entity);

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
        /// Hàm lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Author: LeDucTiep (23/05/2023)
        public virtual async Task<IEnumerable<TEntityDto>> GetAllAsync()
        {
            try
            {
                // Gọi đến procedure
                IEnumerable<TEntity> myList = await _baseRepository.GetAllAsync();

                return _mapper.Map<IEnumerable<TEntityDto>>(myList);
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
        /// Hàm validate
        /// </summary>
        /// Author: LeDucTiep (09/06/2023)
        public virtual List<int> Validate(TEntityDto entity)
        {
            List<int> errorCodes = new();

            if(entity == null)
            {
                return errorCodes;
            }

            System.Reflection.PropertyInfo[] properties = typeof(TEntityDto).GetProperties();

            foreach (System.Reflection.PropertyInfo property in properties)
            {
                var value = property.GetValue(entity, null);


                // Xét bắt buộc 
                var attributeRequired = (MSRequiredAttribute?)property.GetCustomAttributes(typeof(MSRequiredAttribute), false).FirstOrDefault();

                if (attributeRequired != null)
                {
                    if (value == null)
                        errorCodes.Add(attributeRequired.ErrorCode);
                    else if (property.PropertyType == typeof(Guid) && (Guid)value == Guid.Empty)
                        errorCodes.Add(attributeRequired.ErrorCode);
                    else if (property.PropertyType == typeof(string) && (string)value == string.Empty)
                        errorCodes.Add(attributeRequired.ErrorCode);

                }

                // Xét thông tin chỉ số 
                var attributeNumber = (MSNumberAttribute?)property.GetCustomAttributes(typeof(MSNumberAttribute), false).FirstOrDefault();

                if (attributeNumber != null && value != null && !MSNumberAttribute.IsValid(value))
                {
                    errorCodes.Add(attributeNumber.ErrorCode);
                }


                // Xét độ dài 
                var attributeMaxLength = (MSMaxLengthAttribute?)property.GetCustomAttributes(typeof(MSMaxLengthAttribute), false).FirstOrDefault();

                if (attributeMaxLength != null && value != null)
                {
                    int valueLength = value.ToString().Length;
                    int maxLength = attributeMaxLength.Length;
                    bool isTooLong = valueLength > maxLength;
                    if (isTooLong)
                    {
                        errorCodes.Add(attributeMaxLength.ErrorCode);
                    }
                }


                // Xét ngày tháng
                var validDateInThePast = (MSValidDateInThePastAttribute?)property.GetCustomAttributes(typeof(MSValidDateInThePastAttribute), false).FirstOrDefault();

                if (validDateInThePast != null && value != null && !MSValidDateInThePastAttribute.IsValid(value))
                {
                    errorCodes.Add(validDateInThePast.ErrorCode);
                }


                // Xét email
                var emailAttribute = (MSEmailAttribute?)property.GetCustomAttributes(typeof(MSEmailAttribute), false).FirstOrDefault();

                if (emailAttribute != null && !string.IsNullOrEmpty((string)value) && !MSEmailAttribute.IsValid(value))
                {
                    errorCodes.Add(emailAttribute.ErrorCode);
                }
            }

            return errorCodes;
        }

        /// <summary>
        /// Hàm validate theo từng loại service
        /// </summary>
        /// <typeparam name="T">Thực thể</typeparam>
        /// <param name="entity">Thực thể</param>
        /// Author: LeDucTiep (09/06/2023)
        public virtual async Task<List<int>> PostValidate(TEntityDto entity)
        {
            Task<List<int>> task = new(
                    () => new List<int>()
                );
            task.Start();

            // Trả về danh sách rỗng
            return await task;
        }

        /// <summary>
        /// Hàm validate theo từng loại service
        /// </summary>
        /// <typeparam name="T">Thực thể</typeparam>
        /// <param name="entity">Thực thể</param>
        /// <param name="id">Id của bản ghi</param>
        /// Author: LeDucTiep (09/06/2023)
        public virtual async Task<List<int>> UpdateValidate(Guid id, TEntityDto entity)
        {
            Task<List<int>> task = new(
                    () => new List<int>()
                );
            task.Start();

            // Trả về danh sách rỗng
            return await task;
        }

        /// <summary>
        /// Hàm validate theo từng loại service
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// Author: LeDucTiep (09/06/2023)
        public virtual async Task<List<int>> DeleteValidate(Guid id)
        {
            Task<List<int>> task = new(
                    () => new List<int>()
                );
            task.Start();

            // Trả về danh sách rỗng
            return await task;
        }

        /// <summary>
        /// hàm lấy trang bản ghi
        /// </summary>
        /// <param name="basePagingArgument">Tham số để phân trang</param>
        /// <returns>Trang bản ghi</returns>
        /// Author: LeDucTiep (12/07/2023)
        public virtual async Task<BasePage<TEntityDto>> GetPageAsync(BasePagingArgument basePagingArgument)
        {
            try
            {
                List<int> errorCodes = new();

                // Lỗi kích thước trang 
                if (basePagingArgument.PageSize <= 0)
                {
                    errorCodes.Add((int)PagingErrorCode.InvalidPageSize);
                }

                // Lỗi số thứ tự trang 
                if (basePagingArgument.PageNumber <= 0)
                {
                    errorCodes.Add((int)PagingErrorCode.InvalidPageNumber);
                }

                // Kiểm tra độ dài chuỗi tìm kiếm 
                PropertyInfo[] properties = typeof(BasePagingArgument).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var value = property.GetValue(basePagingArgument, null);

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

                BasePage<TEntityDto> page = await _baseRepository.GetPageAsync<TEntityDto>(basePagingArgument);

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
        #endregion
    }
}

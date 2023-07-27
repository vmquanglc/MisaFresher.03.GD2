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
    public class AccountService : BaseService<Account, AccountDto, AccountCreateDto, AccountUpdateDto>, IAccountService
    {
        IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository,
            IMSDatabase msDatabase,
            IMapper mapper) : base(msDatabase, accountRepository, mapper)
        {
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// Hàm kiểm tra số tài khoản đã tồn tại chưa
        /// </summary>
        /// <param name="code">Mã code cần kiểm tra</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<bool> CheckCodeDuplicatedAsync(string code)
        {
            try
            {
                PropertyInfo? property = typeof(AccountDto).GetProperty("AccountCode");

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
                            List<int> errorList = new() { (int)AccountErrorCode.CodeTooLong };
                            throw new BadRequestException(errorList);
                        }
                    }
                }

                return await _accountRepository.CheckExistedCodeAsync(code);
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
        /// Hàm kiểm tra số tải khoản đang sửa có bị trùng không
        /// </summary>
        /// <param name="accountCode">Số tài khoản đang sửa</param>
        /// <param name="itsCode">Số tải khoản trước khi sửa</param>
        /// <returns>bool</returns>
        /// Author: LeDucTiep (12/07/2023)
        public async Task<bool> CheckEditCodeDuplicatedAsync(string accountCode, string itsCode)
        {
            try
            {
                PropertyInfo? property = typeof(AccountDto).GetProperty("AccountCode");

                if (property != null)
                {

                    // Xét độ dài 
                    var attributeMaxLength = (MSMaxLengthAttribute?)property.GetCustomAttributes(typeof(MSMaxLengthAttribute), false).FirstOrDefault();

                    if (attributeMaxLength != null && accountCode != null)
                    {
                        int valueLength = accountCode.Length;
                        int maxLength = attributeMaxLength.Length;
                        bool isTooLong = valueLength > maxLength;
                        if (isTooLong)
                        {
                            List<int> errorList = new() { (int)AccountErrorCode.CodeTooLong };
                            throw new BadRequestException(errorList);
                        }
                    }
                }

                return await _accountRepository.CheckEditCodeDuplicatedAsync(accountCode, itsCode);
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
        /// Hàm lấy danh sách tài khoản, có misaCode
        /// </summary>
        /// <param name="accountPagingArgument">Tham số để lọc</param>
        /// <returns>BasePage<AccountDto></returns>
        /// <exception cref="BadRequestException"></exception>
        /// Author: LeDucTiep (14/07/2023)
        public async Task<BasePage<AccountDto>> GetPageAsync(AccountPagingArgument accountPagingArgument)
        {
            try
            {
                List<int> errorCodes = new();

                // Lỗi kích thước trang 
                if (accountPagingArgument.PageSize <= 0)
                {
                    errorCodes.Add((int)PagingErrorCode.InvalidPageSize);
                }

                // Lỗi số thứ tự trang 
                if (accountPagingArgument.PageNumber <= 0)
                {
                    errorCodes.Add((int)PagingErrorCode.InvalidPageNumber);
                }

                // Kiểm tra độ dài chuỗi tìm kiếm 
                PropertyInfo[] properties = typeof(AccountPagingArgument).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var value = property.GetValue(accountPagingArgument, null);

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

                BasePage<AccountDto> page = await _accountRepository.GetPageAsync<AccountDto>(accountPagingArgument);

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
        /// Hàm thêm một bản ghi 
        /// </summary>
        /// <param name="entityCreateDto"></param>
        /// <returns>TEntity</returns>
        /// Author: LeDucTiep (23/05/2023)
        public override async Task<AccountDto> PostAsync(AccountCreateDto account)
        {
            _msDatabase.BeginTransaction();

            try
            {
                // Lấy thông tin từ cha
                if (account.AccountSyntheticId != null && !account.AccountSyntheticId.Equals(Guid.Empty))
                {
                    Account? parentAcc = await _accountRepository.GetAsync((Guid)account.AccountSyntheticId);

                    account.MisaCode = parentAcc.MisaCode + account.AccountCode + "/";

                    account.Rank = parentAcc.Rank + 1;
                }
                else
                {
                    account.MisaCode = account.AccountCode + "/";

                    account.Rank = 1;
                }

                AccountDto accountDto = _mapper.Map<AccountDto>(account);

                List<int> errorCodes = new();

                errorCodes.AddRange(Validate(accountDto));

                try
                {
                    List<int> ints = await PostValidate(accountDto);

                    errorCodes.AddRange(ints);
                }
                catch { }


                if (errorCodes.Any())
                    throw new BadRequestException(errorCodes);
                Account account1 = _mapper.Map<Account>(account);

                int changedCount = await _baseRepository.PostAsync(account1, Guid.NewGuid());

                if (changedCount == 0)
                    throw new InternalException();

                AccountDto account2 = _mapper.Map<AccountDto>(account1);

                _msDatabase.Commit();

                return account2;
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
        /// Sửa một bản ghi 
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <param name="updateDto">Nội dung sửa</param>
        /// <returns></returns>
        /// Author: LeDucTiep (23/05/2023)
        public override async Task<int> UpdateAsync(Guid id, AccountUpdateDto account)
        {
            _msDatabase.BeginTransaction();

            try
            {
                bool isChangedAccountCode = await _accountRepository.CheckIsChangedAccountCodeAsync(id, account.AccountCode);
                bool isChangedParentId = await _accountRepository.CheckIsChangedParentIdAsync(id, account.AccountSyntheticId);

                // validate và sửa

                AccountDto accountDto = _mapper.Map<AccountDto>(account);

                List<int> errorCodes = new();

                errorCodes.AddRange(Validate(accountDto));

                try
                {
                    errorCodes.AddRange(await UpdateValidate(id, accountDto));
                }
                catch { }

                if (errorCodes.Any())
                    throw new BadRequestException(errorCodes);

                // Thêm trường id để trả về
                Account _account = _mapper.Map<Account>(account);

                // Update
                int changedCount = await _baseRepository.UpdateAsync(id, _account);


                // Nếu mà thay đổi thì ta sẽ sửa lại misa code
                if (isChangedAccountCode)
                {
                    // Nắn lại misaCode của nó và các con của nó
                    string oldMisaCode = account.MisaCode;

                    int index = oldMisaCode[..^1].LastIndexOf('/');
                    string newMisaCode = oldMisaCode[..(index + 1)] + account.AccountCode + "/";


                    // Sửa cha
                    await _accountRepository.UpdateMisaCodeAsync(oldMisaCode, newMisaCode);

                }

                if (isChangedParentId)
                {
                    // Nếu thay đổi thì sửa lại misacode của bản thân account này
                    await _accountRepository.RecalculateMisaCodeAsync(id);
                }

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
        /// Hàm kiểm tra thông tin sửa 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        public override async Task<List<int>> UpdateValidate(Guid id, AccountDto account)
        {
            List<int> errorList = new();

            // Kiểm tra sửa trùng mã 
            bool isDuplicated = await _accountRepository.CheckUpdateCodeBeDublicatedAsync(id, account.AccountCode);
            if (isDuplicated)
                // Xử lý lỗi
                errorList.Add((int)AccountErrorCode.DuplicatedCode);

            return errorList;
        }

        /// <summary>
        /// Hàm kiểm tra Id xóa 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        public override async Task<List<int>> DeleteValidate(Guid id)
        {
            List<int> errors = new();

            bool isParent = await _accountRepository.CheckIsParentAsync(id);

            if (isParent)
            {
                errors.Add((int)AccountErrorCode.CantDeleteParent);
            }

            return errors;
        }
    }
}

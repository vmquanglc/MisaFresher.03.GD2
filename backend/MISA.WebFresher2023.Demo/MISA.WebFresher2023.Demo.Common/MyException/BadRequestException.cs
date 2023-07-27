using MISA.WebFresher2023.Demo.Common.Constant;
using MISA.WebFresher2023.Demo.Common.Resource;
using System.Net;

namespace MISA.WebFresher2023.Demo.Common.MyException
{
    /// <summary>
    /// Class Exception
    /// </summary>
    /// Author: LeDucTiep (23/05/2023)
    public class BadRequestException : BaseException
    {
        #region Field 
        /// <summary>
        /// Http code
        /// </summary>
        /// Author: LeDucTiep (09/06/2023)
        public readonly int StatusCode = (int)HttpStatusCode.BadRequest;
        #endregion

        #region Contructor
        public BadRequestException() : base()
        {
        }

        public BadRequestException(List<int> errorCode) : base(errorCode)
        {
            List<string> userMessage = base.UserMessage;
            List<string> devMessage = base.DevMessage;

            foreach (int code in errorCode)
            {
                switch (code)
                {
                    // Receipt
                    case (int)ReceiptErrorCode.CodeTooLong:
                        userMessage.Add(ReceiptUserMessage.CodeTooLong);
                        devMessage.Add(ReceiptDevMessage.CodeTooLong);
                        break;
                    case (int)ReceiptErrorCode.DeleteBooked:
                        userMessage.Add(ReceiptUserMessage.DeleteBooked);
                        devMessage.Add(ReceiptDevMessage.DeleteBooked);
                        break;



                    // Account
                    case (int)AccountErrorCode.CodeTooLong:
                        userMessage.Add(AccountUserMessage.CodeTooLong);
                        devMessage.Add(AccountDevMessage.CodeTooLong);
                        break;
                    case (int)AccountErrorCode.CodeIsRequired:
                        userMessage.Add(AccountUserMessage.CodeIsRequired);
                        devMessage.Add(AccountDevMessage.CodeIsRequired);
                        break;
                    case (int)AccountErrorCode.NameIsRequired:
                        userMessage.Add(AccountUserMessage.NameIsRequired);
                        devMessage.Add(AccountDevMessage.NameIsRequired);
                        break;
                    case (int)AccountErrorCode.AccountPropertyIsRequired:
                        userMessage.Add(AccountUserMessage.AccountPropertyIsRequired);
                        devMessage.Add(AccountDevMessage.AccountPropertyIsRequired);
                        break;
                    case (int)AccountErrorCode.NameViTooLong:
                        userMessage.Add(AccountUserMessage.NameViTooLong);
                        devMessage.Add(AccountDevMessage.NameViTooLong);
                        break;
                    case (int)AccountErrorCode.NameEnTooLong:
                        userMessage.Add(AccountUserMessage.NameEnTooLong);
                        devMessage.Add(AccountDevMessage.NameEnTooLong);
                        break;

                    case (int)AccountErrorCode.NoteTooLong:
                        userMessage.Add(AccountUserMessage.NoteTooLong);
                        devMessage.Add(AccountDevMessage.NoteTooLong);
                        break;

                    case (int)AccountErrorCode.CantDeleteParent:
                        userMessage.Add(AccountUserMessage.CantDeleteParent);
                        devMessage.Add(AccountDevMessage.CantDeleteParent);
                        break;
                    case (int)AccountErrorCode.DuplicatedCode:
                        userMessage.Add(AccountUserMessage.DuplicatedCode);
                        devMessage.Add(AccountDevMessage.DuplicatedCode);
                        break;


                    // customer
                    case (int)CustomerErrorCode.TaxCodeTooLong:
                        userMessage.Add(CustomerUserMessage.TaxCodeTooLong);
                        devMessage.Add(CustomerDevMessage.TaxCodeTooLong);
                        break;
                    case (int)CustomerErrorCode.CodeIsRequired:
                        userMessage.Add(CustomerUserMessage.CodeIsRequired);
                        devMessage.Add(CustomerDevMessage.CodeIsRequired);
                        break;
                    case (int)CustomerErrorCode.FullNameIsRequired:
                        userMessage.Add(CustomerUserMessage.FullNameIsRequired);
                        devMessage.Add(CustomerDevMessage.FullNameIsRequired);
                        break;
                    case (int)CustomerErrorCode.FullNameTooLong:
                        userMessage.Add(CustomerUserMessage.FullNameTooLong);
                        devMessage.Add(CustomerDevMessage.FullNameTooLong);
                        break;
                    case (int)CustomerErrorCode.AddressTooLong:
                        userMessage.Add(CustomerUserMessage.AddressTooLong);
                        devMessage.Add(CustomerDevMessage.AddressTooLong);
                        break;
                    case (int)CustomerErrorCode.PhoneNumberTooLong:
                        userMessage.Add(CustomerUserMessage.PhoneNumberTooLong);
                        devMessage.Add(CustomerDevMessage.PhoneNumberTooLong);
                        break;
                    case (int)CustomerErrorCode.WebsiteTooLong:
                        userMessage.Add(CustomerUserMessage.WebsiteTooLong);
                        devMessage.Add(CustomerDevMessage.WebsiteTooLong);
                        break;
                    case (int)CustomerErrorCode.NoteTooLong:
                        userMessage.Add(CustomerUserMessage.NoteTooLong);
                        devMessage.Add(CustomerDevMessage.NoteTooLong);
                        break;
                    case (int)CustomerErrorCode.IdentityNumberTooLong:
                        userMessage.Add(CustomerUserMessage.IdentityNumberTooLong);
                        devMessage.Add(CustomerDevMessage.IdentityNumberTooLong);
                        break;
                    case (int)CustomerErrorCode.IdentityPlaceTooLong:
                        userMessage.Add(CustomerUserMessage.IdentityPlaceTooLong);
                        devMessage.Add(CustomerDevMessage.IdentityPlaceTooLong);
                        break;
                    case (int)CustomerErrorCode.CustomerCodeTooLong:
                        userMessage.Add(CustomerUserMessage.CustomerCodeTooLong);
                        devMessage.Add(CustomerDevMessage.CustomerCodeTooLong);
                        break;


                    case (int)EmployeeErrorCode.GuidInvalid:
                        userMessage.Add(EmployeeUserMessage.GuidInvalid);
                        devMessage.Add(EmployeeDevMessage.GuidInvalid);
                        break;

                    case (int)RequestErrorCode.GuidInvalid:
                        userMessage.Add(RequestUserMessage.GuidInvalid);
                        devMessage.Add(RequestDevMessage.GuidInvalid);
                        break;

                    // Nhân viên 
                    case (int)EmployeeErrorCode.CodeDuplicated:
                        userMessage.Add(EmployeeUserMessage.CodeDuplicated);
                        devMessage.Add(EmployeeDevMessage.CodeDuplicated);
                        break;

                    case (int)EmployeeErrorCode.IdNotFound:
                        userMessage.Add(EmployeeUserMessage.IdNotFound);
                        devMessage.Add(EmployeeDevMessage.IdNotFound);
                        break;

                    case (int)EmployeeErrorCode.CodeIsRequired:
                        userMessage.Add(EmployeeUserMessage.CodeIsRequired);
                        devMessage.Add(EmployeeDevMessage.CodeIsRequired);
                        break;

                    case (int)EmployeeErrorCode.FullNameIsRequired:
                        userMessage.Add(EmployeeUserMessage.FullNameIsRequired);
                        devMessage.Add(EmployeeDevMessage.FullNameIsRequired);
                        break;

                    case (int)EmployeeErrorCode.EmployeeCodeTooLong:
                        userMessage.Add(EmployeeUserMessage.EmployeeCodeTooLong);
                        devMessage.Add(EmployeeDevMessage.EmployeeCodeTooLong);
                        break;

                    case (int)EmployeeErrorCode.FullNameTooLong:
                        userMessage.Add(EmployeeUserMessage.FullNameTooLong);
                        devMessage.Add(EmployeeDevMessage.FullNameTooLong);
                        break;

                    case (int)EmployeeErrorCode.EmailTooLong:
                        userMessage.Add(EmployeeUserMessage.EmailTooLong);
                        devMessage.Add(EmployeeDevMessage.EmailTooLong);

                        break;
                    case (int)EmployeeErrorCode.AddressTooLong:
                        userMessage.Add(EmployeeUserMessage.AddressTooLong);
                        devMessage.Add(EmployeeDevMessage.AddressTooLong);
                        break;

                    case (int)EmployeeErrorCode.PhoneNumberTooLong:
                        userMessage.Add(EmployeeUserMessage.PhoneNumberTooLong);
                        devMessage.Add(EmployeeDevMessage.PhoneNumberTooLong);
                        break;

                    case (int)EmployeeErrorCode.IdentityNumberTooLong:
                        userMessage.Add(EmployeeUserMessage.IdentityNumberTooLong);
                        devMessage.Add(EmployeeDevMessage.IdentityNumberTooLong);
                        break;

                    case (int)EmployeeErrorCode.IdentityNumberInvalid:
                        userMessage.Add(EmployeeUserMessage.IdentityNumberInvalid);
                        devMessage.Add(EmployeeDevMessage.IdentityNumberInvalid);
                        break;

                    case (int)EmployeeErrorCode.IdentityPlaceTooLong:
                        userMessage.Add(EmployeeUserMessage.IdentityPlaceTooLong);
                        devMessage.Add(EmployeeDevMessage.IdentityPlaceTooLong);
                        break;

                    case (int)EmployeeErrorCode.BankAccountNumberTooLong:
                        userMessage.Add(EmployeeUserMessage.BankAccountNumberTooLong);
                        devMessage.Add(EmployeeDevMessage.BankAccountNumberTooLong);
                        break;

                    case (int)EmployeeErrorCode.NameOfBankTooLong:
                        userMessage.Add(EmployeeUserMessage.NameOfBankTooLong);
                        devMessage.Add(EmployeeDevMessage.NameOfBankTooLong);
                        break;

                    case (int)EmployeeErrorCode.BankAccountBranchTooLong:
                        userMessage.Add(EmployeeUserMessage.BankAccountBranchTooLong);
                        devMessage.Add(EmployeeDevMessage.BankAccountBranchTooLong);
                        break;

                    case (int)EmployeeErrorCode.DateOfBirthInvalidTime:
                        userMessage.Add(EmployeeUserMessage.DateOfBirthInvalidTime);
                        devMessage.Add(EmployeeDevMessage.DateOfBirthInvalidTime);
                        break;

                    case (int)EmployeeErrorCode.IdentityDateInvalidTime:
                        userMessage.Add(EmployeeUserMessage.IdentityDateInvalidTime);
                        devMessage.Add(EmployeeDevMessage.IdentityDateInvalidTime);
                        break;

                    case (int)EmployeeErrorCode.EmailInvalid:
                        userMessage.Add(EmployeeUserMessage.EmailInvalid);
                        devMessage.Add(EmployeeDevMessage.EmailInvalid);
                        break;

                    // Phòng ban 
                    case (int)DepartmentErrorCode.IdNotFound:
                        userMessage.Add(DepartmentUserMessage.IdNotFound);
                        devMessage.Add(DepartmentDevMessage.IdNotFound);
                        break;

                    // Chức vụ
                    case (int)PositionErrorCode.IdNotFound:
                        userMessage.Add(PositionUserMessage.IdNotFound);
                        devMessage.Add(PositionDevMessage.IdNotFound);
                        break;

                    // Phân trang
                    case (int)PagingErrorCode.InvalidPageSize:
                        userMessage.Add(PagingUserMessage.InvalidPageSize);
                        devMessage.Add(PagingDevMessage.InvalidPageSize);
                        break;

                    case (int)PagingErrorCode.InvalidPageNumber:
                        userMessage.Add(PagingUserMessage.InvalidPageNumber);
                        devMessage.Add(PagingDevMessage.InvalidPageNumber);
                        break;

                    case (int)PagingErrorCode.EmployeeSearchTermTooLong:
                        userMessage.Add(PagingUserMessage.EmployeeSearchTermTooLong);
                        devMessage.Add(PagingDevMessage.EmployeeSearchTermTooLong);
                        break;
                }
            }

            ErrorCode = errorCode;
            UserMessage = userMessage;
            DevMessage = devMessage;
        }
        #endregion
    }
}

namespace MISA.WebFresher2023.Demo.Common.Constant
{

    /// <summary>
    /// Request mã lỗi
    /// </summary>
    /// Author: LeDucTiep (23/05/2023)
    public enum RequestErrorCode
    {
        /// <summary>
        /// Lỗi không đúng Guid
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        GuidInvalid = 6001,
    }
    /// <summary>
    /// Internal mã lỗi
    /// </summary>
    /// Author: LeDucTiep (23/05/2023)
    public enum InternalErrorCode
    {
        /// <summary>
        /// Lỗi không xác định
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        Unknown = 5001,

        /// <summary>
        /// Lỗi không kết nối được database
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        ConnectDbError = 5002,
    }

    /// <summary>
    /// Department mã lỗi
    /// </summary>
    /// Author: LeDucTiep (23/05/2023)
    public enum DepartmentErrorCode
    {
        /// <summary>
        /// Lỗi không tìm thấy Id phòng ban 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        IdNotFound = 4001,
    }

    /// <summary>
    /// Position mã lỗi 
    /// </summary>
    /// Author: LeDucTiep (23/05/2023)
    public enum PositionErrorCode
    {
        /// <summary>
        /// Lỗi không tìm thấy Id chức vụ
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        IdNotFound = 3001,
    }

    /// <summary>
    /// Paging mã lỗi 
    /// </summary>
    /// Author: LeDucTiep (23/05/2023)
    public enum PagingErrorCode
    {
        /// <summary>
        /// Lỗi kích thước trang không hợp lệ 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        InvalidPageSize = 2001,

        /// <summary>
        /// Lỗi số thứ tự trang không hợp lệ 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        InvalidPageNumber = 2002,

        /// <summary>
        /// Lỗi độ dài từ khóa tìm kiếm
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        EmployeeSearchTermTooLong = 2003,
    }

    /// <summary>
    /// Employee mã lỗi 
    /// </summary>
    /// Author: LeDucTiep (23/05/2023)
    public enum EmployeeErrorCode
    {
        /// <summary>
        /// Lỗi không tìm thấy Id nhân viên
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        IdNotFound = 1001,

        /// <summary>
        /// Lỗi trùng mã nhân viên 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        CodeDuplicated = 1002,

        /// <summary>
        /// Không để trống mã nhân viên 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        CodeIsRequired = 1003,

        /// <summary>
        /// Không để trống tên nhân viên 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        FullNameIsRequired = 1004,

        /// <summary>
        /// Mã nhân viên quá dài 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        EmployeeCodeTooLong = 1005,

        /// <summary>
        /// Họ tên quá dài 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        FullNameTooLong = 1006,

        /// <summary>
        /// Email quá dài 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        EmailTooLong = 1007,

        /// <summary>
        /// Địa chỉ quá dài
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        AddressTooLong = 1008,

        /// <summary>
        /// Số điện thoại quá dài
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        PhoneNumberTooLong = 1009,

        /// <summary>
        /// Số chứng minh nhân dân quá dài 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        IdentityNumberTooLong = 1010,

        /// <summary>
        /// Nơi cấp chứng minh nhân dân quá dài 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        IdentityPlaceTooLong = 1011,

        /// <summary>
        /// Tài khoản ngân hàng quá dài 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        BankAccountNumberTooLong = 1012,

        /// <summary>
        /// Tên ngân hàng quá dài 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        NameOfBankTooLong = 1013,

        /// <summary>
        /// Chi nhánh ngân hàng quá dài 
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        BankAccountBranchTooLong = 1014,

        /// <summary>
        /// Ngày sinh không được phép lớn hơn ngày hiện tại
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        DateOfBirthInvalidTime = 1015,

        /// <summary>
        /// Ngày cấp không được phép lớn hơn ngày hiện tại
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        IdentityDateInvalidTime = 1016,

        /// <summary>
        /// Email không đúng
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        EmailInvalid = 1017,

        /// <summary>
        /// Guid không đúng
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        GuidInvalid = 1018,

        /// <summary>
        /// Số chứng minh thư không đúng
        /// </summary>
        /// Author: LeDucTiep (23/05/2023)
        IdentityNumberInvalid = 1019,
    }

    /// <summary>
    /// Mã lỗi khách hàng
    /// </summary>
    /// Author: LeDucTiep (12/07/2023)
    public enum CustomerErrorCode
    {
        // Id không tìm thấy
        IdNotFound = 7001,
        // Mã khách hàng không được để trống
        CodeIsRequired = 7002,
        // Họ tên không được để trống
        FullNameIsRequired = 7003,
        // Họ tên quá dài
        FullNameTooLong = 7004,
        // Mã số thuế quá dài
        TaxCodeTooLong = 7005,
        // Địa chỉ quá dài
        AddressTooLong = 7006,
        // Số điện thoại quá dài 
        PhoneNumberTooLong = 7007,
        // Website quá dài
        WebsiteTooLong = 7008,
        // Ghi chú quá dài
        NoteTooLong = 7009,
        // Số chứng minh nhân dân quá dài
        IdentityNumberTooLong = 7010,
        // Nơi cấp quá dài
        IdentityPlaceTooLong = 7011,
        // Mã khách hàng quá dài
        CustomerCodeTooLong = 7012,
    }

    /// <summary>
    /// Mã lỗi tài khoản
    /// </summary>
    /// Author: LeDucTiep (12/07/2023)
    public enum AccountErrorCode
    {
        // Số tài khoản quá dài
        CodeTooLong = 8001,
        // Số tài khoản không được để trống
        CodeIsRequired = 8002,
        // Tên tài khoản không được để trống
        NameIsRequired = 8003,
        // Tính chất tài khoản không được để trống
        AccountPropertyIsRequired = 8004,
        // Tên tài khoản quá dài 
        NameViTooLong = 8005,
        // Tên tiếng anh quá dài
        NameEnTooLong = 8006,
        // Ghi chú quá dài
        NoteTooLong = 8009,
        // Không thể xóa tài khoản cha
        CantDeleteParent = 8010,
        // Trùng số tài khoản
        DuplicatedCode = 8011,
    }

    /// <summary>
    /// Mã lỗi phiếu thu
    /// </summary>
    /// Author: LeDucTiep(12/07/2023)
    public enum ReceiptErrorCode
    {
        // Số phiếu thu quá dài 
        CodeTooLong = 9001,
        DeleteBooked = 9002,
    }
}

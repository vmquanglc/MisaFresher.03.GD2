DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Employee_Paging;


CREATE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Employee_Paging (IN _offset int,
IN _limit int,
IN searchTerm varchar(255),
OUT totalRecord int)
COMMENT '
  Param: _limit: Số bản ghi cần lấy
         _offset: Số bản ghi bỏ qua
         employeeSearchTerm: FullName, số điện thoại hoặc EmployeeCode cần tìm
  Output: employeeId: Id của nhân viên cần thêm
          employeeCode: Mã nhân viên
          lastName: Họ 
          firstName: Tên 
          fullName: Họ và tên 
          gender: Giới tính (0: Nam, 1: Nữ, 2: Khác)
          dateOfBirth: Ngày sinh 
          email: mail
          address: Địa chỉ 
          phoneNumber: Số điện thoại
          identityNumber: số chứng minh nhân dân 
          identityDate: Ngày cấp chứng minh nhân dân 
          identityPlace: Nơi cấp chứng minh nhân dân 
          positionId: Mã chức vụ 
          departmentId: Mã phòng ban 
          bankAccountNumber: Tài khoản ngân hàng 
          nameOfBank: Tên ngân hàng 
          bankAccountBranch: Chi nhánh
          createdBy: Người tạo
          createdDate: Ngày tạo
          modifiedDate: Ngày sửa
          modifiedBy: Người sửa 
          totalRecord: Tổng số bản ghi
  Author: LeDucTiep (06/05/2023)
  Test: CALL Proc_Employee_Paging(0, 10, "Đan", @totalRecord);
        SELECT
          @totalRecord;
'
BEGIN
  -- Lấy ra dữ liệu 
  WITH SearchedList
  AS
  (SELECT
        e.EmployeeId,
        e.EmployeeCode,
        e.FullName,
        e.Gender,
        e.DateOfBirth,
        e.Email,
        e.Address,
        e.PhoneNumber,
        e.IdentityNumber,
        e.IdentityDate,
        e.IdentityPlace,
        e.PositionId,
        e.DepartmentId,
        d.DepartmentName,
        e.CreatedDate,
        p.PositionName,
        e.BankAccountNumber,
        e.NameOfBank,
        e.BankAccountBranch,
        e.CreatedBy,
        e.ModifiedDate,
        e.ModifiedBy
      FROM employee e
        LEFT JOIN department d
          ON e.DepartmentId = d.DepartmentId
        LEFT JOIN position_ p
          ON e.PositionId = p.PositionId
      WHERE FullName LIKE concat('%', searchTerm, '%')
      OR EmployeeCode LIKE concat('%', searchTerm, '%')
      OR e.PhoneNumber LIKE concat('%', searchTerm, '%'))

  SELECT
    EmployeeId,
    EmployeeCode,
    FullName,
    Gender,
    DateOfBirth,
    Email,
    Address,
    PhoneNumber,
    IdentityNumber,
    IdentityDate,
    IdentityPlace,
    PositionId,
    DepartmentId,
    DepartmentName,
    CreatedDate,
    PositionName,
    BankAccountNumber,
    NameOfBank,
    BankAccountBranch,
    CreatedBy,
    ModifiedDate,
    ModifiedBy
  FROM SearchedList
  ORDER BY EmployeeCode DESC
  LIMIT _limit
  OFFSET _offset;

  -- Lấy ra số lượng bản ghi
  WITH SearchedList
  AS
  (SELECT
        e.EmployeeId,
        e.EmployeeCode,
        e.FullName,
        e.Gender,
        e.DateOfBirth,
        e.Email,
        e.Address,
        e.PhoneNumber,
        e.IdentityNumber,
        e.IdentityDate,
        e.IdentityPlace,
        e.PositionId,
        e.DepartmentId,
        e.CreatedDate,
        e.BankAccountNumber,
        e.NameOfBank,
        e.BankAccountBranch,
        e.CreatedBy,
        e.ModifiedDate,
        e.ModifiedBy
      FROM employee e
      WHERE FullName LIKE concat('%', searchTerm, '%')
      OR EmployeeCode LIKE concat('%', searchTerm, '%')
      OR e.PhoneNumber LIKE concat('%', searchTerm, '%'))
  SELECT
    COUNT(*) INTO totalRecord
  FROM SearchedList;


END;

-- Test
CALL Proc_Employee_Paging(0, 10, "Đan", @totalRecord);
SELECT
  @totalRecord;
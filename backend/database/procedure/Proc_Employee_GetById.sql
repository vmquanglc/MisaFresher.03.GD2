DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Employee_GetById;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Employee_GetById (IN employeeId char(36))
COMMENT '
  Output: Thông tin nhân viên 
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_Employee_GetById("1117fccb-2220-33ff-667b-d9ebc53aac3d");
'
BEGIN
  SELECT
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
    e.BankAccountNumber,
    e.NameOfBank,
    e.BankAccountBranch,
    e.CreatedDate,
    e.CreatedBy,
    e.ModifiedDate,
    e.ModifiedBy
  FROM employee e
  WHERE e.employeeId = employeeId;
END;

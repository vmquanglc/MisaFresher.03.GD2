DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Employee_Export;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Employee_Export ()
COMMENT '
  Output: Danh sách t?t employee, tên phòng ban, tên ch?c v?
  Author: LeDucTiep (07/06/2023)
  Test: CALL `misa.web202303_mf1609_leductiep`.Proc_Employee_Export ()
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
    e.ModifiedBy,
    p.PositionName,
    d.DepartmentName
  FROM employee e

    LEFT JOIN department d
      ON e.DepartmentId = d.DepartmentId
    LEFT JOIN position_ p
      ON e.PositionId = p.PositionId
  ;

END;
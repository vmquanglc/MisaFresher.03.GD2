CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Employee_CheckDuplicatedCodeExceptItsId (IN employeeCode varchar(20), IN itsId char(36))
COMMENT '
  Param: employeeCode: Mã của nhân viên cần lấy
         itsId: Id của nhân viên trước khi sửa
  Output: Nhân viên cần lấy nếu tìm thấy
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_Employee_CheckDuplicatedCodeExceptItsId("NV-0061", "1c81ee21-5452-337b-3c77-d9ebc53aac3d");
'

BEGIN
  SELECT
    EXISTS (SELECT
        *
      FROM employee e
      WHERE e.employeeCode = employeeCode
      AND e.employeeCode NOT IN (SELECT
          e1.employeeCode
        FROM employee e1
        WHERE e1.EmployeeId = itsId));
END;
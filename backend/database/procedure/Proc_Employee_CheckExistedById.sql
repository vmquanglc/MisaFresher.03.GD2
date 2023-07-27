CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Employee_CheckExistedById (IN employeeId char(36))
COMMENT '
  Param: employeeId: Id c?a nhân viên
  Output: true: có
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_Employee_CheckExistedById("1c81ee21-5452-337b-3c77-d9ebc53aac3d");
'
BEGIN
  SELECT
    EXISTS (SELECT
        *
      FROM employee e
      WHERE e.EmployeeId = employeeId);
END;
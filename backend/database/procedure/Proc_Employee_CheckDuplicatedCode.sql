CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Employee_CheckDuplicatedCode (IN employeeCode varchar(20))
COMMENT '
  Param: employeeCode: Mã của nhân viên cần lấy
  Output: Nhân viên cần lấy nếu tìm thấy
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_Employee_CheckDuplicatedCode("NV-1648398084");
      CALL Proc_Employee_CheckDuplicatedCode("NV-1648398084abc");
'
BEGIN
  SELECT
    EXISTS (SELECT
        *
      FROM employee e
      WHERE employeeCode = e.employeeCode);
END;
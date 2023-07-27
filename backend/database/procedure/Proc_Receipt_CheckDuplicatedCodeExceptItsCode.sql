CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Receipt_CheckDuplicatedCodeExceptItsCode (IN receiptCode varchar(20),
IN itsCode varchar(20))
COMMENT '
  Param: employeeCode: Mã của nhân viên cần lấy
         itsCode: Mã của nhân viên trước khi sửa
  Output: Nhân viên cần lấy nếu tìm thấy
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_Employee_CheckDuplicatedCodeExceptItsCode("NV-0872", "NV-0874");
      CALL Proc_Employee_CheckDuplicatedCodeExceptItsCode("NV-1648398084abc", "");
'
BEGIN
  SELECT
    EXISTS (SELECT
        *
      FROM receipt r
      WHERE r.receiptCode != itsCode
      AND r.receiptCode = receiptCode);
END;
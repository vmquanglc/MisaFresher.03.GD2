CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_CheckDuplicatedCodeExceptItsId (IN accountCode varchar(20),
IN itsId char(36))
COMMENT '
  Param: employeeCode: Mã của nhân viên cần lấy
         itsId: Id của nhân viên trước khi sửa
  Output: Nhân viên cần lấy nếu tìm thấy
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_Receipt_CheckDuplicatedCodeExceptItsId("NV-0061", "1c81ee21-5452-337b-3c77-d9ebc53aac3d");
'

BEGIN
  SELECT
    EXISTS (SELECT
        *
      FROM account a
      WHERE a.AccountCode = accountCode
      AND a.AccountCode NOT IN (SELECT
          a1.AccountCode
        FROM account a1
        WHERE a1.AccountId = itsId));
END;
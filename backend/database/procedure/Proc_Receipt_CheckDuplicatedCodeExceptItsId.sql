CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Receipt_CheckDuplicatedCodeExceptItsId (IN receiptCode varchar(20),
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
      FROM receipt r
      WHERE r.receiptCode = receiptCode
      AND r.receiptCode NOT IN (SELECT
          r1.receiptCode
        FROM receipt r1
        WHERE r1.ReceiptId = itsId));
END;
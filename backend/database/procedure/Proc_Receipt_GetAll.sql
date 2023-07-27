CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Receipt_GetAll ()
COMMENT '
  Output: Danh sách tất cả
  Author: LeDucTiep (16/05/2023)
  Test: CALL `misa.web202303_mf1609_leductiep`.Proc_City_GetAll ()
'
BEGIN
  SELECT
    r.ReceiptId,
    r.IsRecorded,
    r.ReceiptCode,
    r.ReceiptType,
    r.CustomerId,
    r.CustomerName,
    r.Payer,
    r.Address,
    r.EmployeeId,
    r.Reason,
    r.Amount,
    r.AccountingDate,
    r.ReceiptDate,
    r.CreatedBy,
    r.CreatedDate,
    r.ModifiedBy,
    r.ModifiedDate
  FROM receipt r;

END;
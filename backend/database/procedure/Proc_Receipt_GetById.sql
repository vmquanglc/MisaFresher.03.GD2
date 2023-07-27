CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Receipt_GetById (IN receiptId char(36))
COMMENT '
  Output: Thông tin
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_City_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    r.ReceiptId,
    r.ReceiptCode,
    r.IsRecorded,
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
  FROM receipt r
  WHERE r.receiptId = receiptId
  ;
END;

CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_BookKeeping_GetById (IN bookKeepingId char(36))
COMMENT '
  Output: Thông tin
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_City_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    b.ReceiptId,
    b.bookKeepingId,
    b.Note,
    b.DebitAccountId,
    b.CollectAccountId,
    b.AmountOfMoney,
    b.CreatedBy,
    b.CreatedDate,
    b.ModifiedBy,
    b.ModifiedDate
  FROM bookkeeping b
  WHERE b.bookKeepingId = bookKeepingId
  ;
END;

CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_BookKeeping_GetAll ()
COMMENT '
  Output: Danh sách tất cả
  Author: LeDucTiep (16/05/2023)
  Test: CALL `misa.web202303_mf1609_leductiep`.Proc_City_GetAll ()
'
BEGIN
  SELECT
    b.ReceiptId,
    b.BookKeepingId,
    b.Note,
    b.DebitAccountId,
    b.CollectAccountId,
    b.AmountOfMoney,
    b.CreatedBy,
    b.CreatedDate,
    b.ModifiedBy,
    b.ModifiedDate
  FROM bookkeeping b;

END;
CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_BankAccount_GetAll ()
COMMENT '
  Output: Danh sách tất cả
  Author: LeDucTiep (16/05/2023)
  Test: CALL `misa.web202303_mf1609_leductiep`.Proc_BankAccount_GetAll ()
'
BEGIN
  SELECT
    b.BankAccountId,
    b.CustomerId,
    b.BankAccountNumber,
    b.Name,
    b.Branch,
    b.BankCity,
    b.CreatedDate,
    b.CreatedBy,
    b.ModifiedDate,
    b.ModifiedBy
  FROM BankAccount b;

END;
CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_BankAccount_GetById (IN bankAccountId char(36))
COMMENT '
  Output: Thông tin chức vụ
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_BankAccount_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    b.bankAccountId,
    b.CustomerId,
    b.BankAccountNumber,
    b.Name,
    b.Branch,
    b.BankCity,
    b.CreatedDate,
    b.CreatedBy,
    b.ModifiedDate,
    b.ModifiedBy
  FROM BankAccount b
  WHERE b.bankAccountId = bankAccountId;
END;

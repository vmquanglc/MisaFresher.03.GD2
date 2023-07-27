CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_BankAccount_GetByCustomerId (IN customerId char(36))
COMMENT '
  Param: customerId: Id khách hàng
       
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL Proc_BankAccount_GetByCustomerId("26e3e78c-254b-288b-5825-73f60cb6c8c3");
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
  FROM BankAccount b
  WHERE b.customerId = customerId;
END
;

CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_BankAccount_DeleteNotIn (IN bankAccountNumberList text, IN customerId char(36))
COMMENT '
  Param: 
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL `misa.web202303_mf1609_leductiep`.Proc_BankAccount_DeleteNotIn("06306268-f3b5-11ed-a084-d8bbc1b19103");
'
BEGIN

  SET @sql = concat('DELETE from bankaccount
  WHERE bankaccount.BankAccountNumber NOT IN (', bankAccountNumberList, ')
    AND bankaccount.customerId = \'', customerId, '\';');
  PREPARE stmt FROM @sql;
  EXECUTE stmt;
  DEALLOCATE PREPARE stmt;

END; 

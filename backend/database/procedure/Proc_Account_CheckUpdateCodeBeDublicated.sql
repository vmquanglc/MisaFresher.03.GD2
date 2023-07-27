CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_CheckUpdateCodeBeDublicated (IN accountId char(36),
IN accountCode varchar(20))
COMMENT '
  Param: 
  Output: true: b? trùng
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_Account_CheckUpdateCodeBeDublicated("4fc10e46-e431-427b-a6bc-9ca5b02db654", "25451");
'
BEGIN
  SELECT
    EXISTS (SELECT
        *
      FROM account a
      WHERE a.accountCode = accountCode
      AND a.accountId != accountId);
END;
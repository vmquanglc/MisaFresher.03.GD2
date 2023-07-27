CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_CheckIsChangedAccountCode (IN accountId char(36), IN accountCode varchar(20))
COMMENT '
  Param: positionId: Id ch?c v? 
  Output: true: có
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_Account_CheckIsChangedAccountCode("142cb08f-7c31-21fa-8e90-67245e8b283e", "02967");
'
BEGIN
  SELECT
    NOT EXISTS (SELECT
        *
      FROM account a
      WHERE a.accountId = accountId
      AND a.accountCode = accountCode);
END;
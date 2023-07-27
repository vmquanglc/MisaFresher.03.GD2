CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_CheckIsParent (IN accountId char(36))
COMMENT '
  Param: positionId: Id ch?c v? 
  Output: true: có
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_Account_CheckIsParent("bd9048d8-9582-4e5e-b1e5-dbd1db427c13");
'
BEGIN
  SELECT
    EXISTS (SELECT
        *
      FROM account a
      WHERE a.AccountSyntheticId = accountId);
END;
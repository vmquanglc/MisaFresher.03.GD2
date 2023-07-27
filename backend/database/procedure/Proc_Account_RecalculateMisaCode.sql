CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_RecalculateMisaCode (IN accountId char(36))
COMMENT '
  Param: accountId
  Output: true: có
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_Account_RecalculateMisaCode("2f5c122e-c70f-408a-aa86-ef068124216f");
'
BEGIN
  -- Update misacode cho các con
  SET @parentId = (SELECT
      a.AccountSyntheticId
    FROM account a
    WHERE a.accountId = accountId
    LIMIT 1);

  SET @parentMSCode = (SELECT
      a.MisaCode
    FROM account a
    WHERE a.accountId = @parentId);
  SET @parentRank = (SELECT
      a.Rank
    FROM account a
    WHERE a.accountId = @parentId);

  IF (NOT EXISTS (SELECT
        a.Rank
      FROM account a
      WHERE a.accountId = @parentId)) THEN
    SET @parentRank = 0;
  END IF;

  UPDATE account a

  SET a.MisaCode = CONCAT(COALESCE(@parentMSCode, ''), a.AccountCode, '/'),
      a.Rank = @parentRank + 1
  WHERE a.accountId = accountId;

END;

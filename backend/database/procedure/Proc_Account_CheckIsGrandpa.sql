CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_CheckIsGrandpa (IN accountId char(36))
COMMENT '
  Param: accountId: Id tai khoan
  Output: true: có
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_Account_CheckIsGrandpa("2f5c122e-c70f-408a-aa86-ef068124216f");
'
BEGIN

  SET @grandRank = (SELECT
      a.Rank
    FROM account a
    WHERE a.accountId = accountId
    LIMIT 1);

  SET @misaCode = (SELECT
      a.MisaCode
    FROM account a
    WHERE a.accountId = accountId
    LIMIT 1);

  SELECT
    EXISTS (SELECT
        *
      FROM account a
      WHERE a.MisaCode LIKE CONCAT(@misaCode, '_%')
      AND a.Rank = @grandRank + 2);
END;

CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_CheckDuplicatedCode (IN accountCode varchar(20))
COMMENT '
  Param: positionId: Id ch?c v? 
  Output: true: c�
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_Position_CheckExistedById("1c81ee21-5452-337b-3c77-d9ebc53aac3d");
'
BEGIN
  SELECT
    EXISTS (SELECT
        *
      FROM account a
      WHERE a.AccountCode = accountCode);
END;
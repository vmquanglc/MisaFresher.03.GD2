CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_UpdateMisaCode (IN oldMisaCode varchar(255),
IN newMisaCode varchar(255))
COMMENT '
  Param: positionId: Id ch?c v? 
  Output: true: có
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_Account_UpdateMisaCode("", "");
'
BEGIN

  -- Update misacode cho cha
  UPDATE account a
  SET a.MisaCode = CONCAT(newMisaCode, SUBSTRING(a.MisaCode, LENGTH(oldMisaCode) + 1, LENGTH(a.MisaCode)))
  WHERE a.MisaCode LIKE CONCAT(oldMisaCode, '%');

--   -- Update misacode cho các con
--   SET @parentCode = (SELECT
--       a.AccountCode
--     FROM account a
--     WHERE a.MisaCode = misaCode
--     LIMIT 1);
-- 
--   UPDATE account a
-- 
--   SET a.MisaCode = CONCAT(a.MisaCode, @parentCode, SUBSTRING(a.AccountCode, LENGTH(@parentCode) + 1, 21), '/'),
--   a.AccountCode = CONCAT(@parentCode, SUBSTRING(a.AccountCode , LENGTH(@parentCode) + 1, 21))
-- 
--   WHERE a.MisaCode LIKE CONCAT(misaCode, '_%');
END;
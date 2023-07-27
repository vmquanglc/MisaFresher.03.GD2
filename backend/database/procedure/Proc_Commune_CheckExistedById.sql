CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Commune_CheckExistedById (IN communeId char(36))
COMMENT '
  Param: positionId: Id ch?c v? 
  Output: true: có
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_Commune_CheckExistedById("1c81ee21-5452-337b-3c77-d9ebc53aac3d");
'
BEGIN
  SELECT
    EXISTS (SELECT
        *
      FROM commune c
      WHERE c.communeId = communeId);
END;
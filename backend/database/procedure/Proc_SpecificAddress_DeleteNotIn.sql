CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_SpecificAddress_DeleteNotIn (IN addressList text, IN otherLocationId char(36))
COMMENT '
  Param: 
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL `misa.web202303_mf1609_leductiep`.Proc_SpecificAddress_DeleteNotIn
'
BEGIN

  SET @sql = concat('DELETE from specificaddress
  WHERE specificaddress.Address NOT IN (', addressList, ')
    AND specificaddress.otherLocationId = \'', otherLocationId, '\';');
  PREPARE stmt FROM @sql;
  EXECUTE stmt;
  DEALLOCATE PREPARE stmt;

END; 

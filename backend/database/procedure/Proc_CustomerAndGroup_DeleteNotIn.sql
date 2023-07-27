CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_CustomerAndGroup_DeleteNotIn (IN customerGroupList text, IN customerId char(36))
COMMENT '
  Param: 
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL `misa.web202303_mf1609_leductiep`.Proc_CustomerAndGroup_DeleteNotIn("06306268-f3b5-11ed-a084-d8bbc1b19103");
'
BEGIN

  SET @sql = CONCAT('DELETE FROM customerandgroup 
    WHERE customerandgroup.customerGroupId NOT IN (', customerGroupList, ')
    AND customerandgroup.customerId = \'', customerId, '\';');
  PREPARE stmt FROM @sql;
  EXECUTE stmt;
  DEALLOCATE PREPARE stmt;

END; 

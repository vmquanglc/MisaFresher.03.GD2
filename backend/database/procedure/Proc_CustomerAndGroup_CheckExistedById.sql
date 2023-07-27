CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_CustomerAndGroup_CheckExistedById (IN customerGroupId char(36), IN customerId char(36))
COMMENT '
  Param: positionId: Id ch?c v? 
  Output: true: c�
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_CustomerAndGroup_CheckExistedById("1c81ee21-5452-337b-3c77-d9ebc53aac3d", "1c81ee21-5452-337b-3c77-d9ebc53aac3d", );
'
BEGIN
  SELECT
    EXISTS (SELECT
        *
      FROM customerandgroup c
      WHERE c.customerGroupId = customerGroupId
      AND c.customerId = customerId);
END;
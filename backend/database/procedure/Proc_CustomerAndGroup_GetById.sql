DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_CustomerAndGroup_GetById;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_CustomerAndGroup_GetById (IN customerGroupId char(36), IN customerId char(36))
COMMENT '
  Output: Thông tin
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_CustomerAndGroup_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    c.customerGroupId,
    c.customerId,
    c.CreatedDate,
    c.CreatedBy,
    c.ModifiedDate,
    c.ModifiedBy
  FROM customerandgroup c
  WHERE c.customerGroupId = customerGroupId
  AND c.customerId = customerId;
END;

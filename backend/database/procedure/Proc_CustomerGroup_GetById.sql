CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_CustomerGroup_GetById (IN customerGroupId char(36))
COMMENT '
  Output: Thông tin
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_CustomerGroup_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    c.CustomerGroupId,
    c.CustomerGroupCode,
    c.Name,
    c.Note,
    c.CreatedDate,
    c.CreatedBy,
    c.ModifiedDate,
    c.ModifiedBy
  FROM customergroup c
  WHERE c.customerGroupId = customerGroupId;
END;

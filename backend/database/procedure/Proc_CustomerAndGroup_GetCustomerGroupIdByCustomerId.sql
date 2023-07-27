CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_CustomerAndGroup_GetCustomerGroupIdByCustomerId (IN CustomerId char(36))
COMMENT '
  Output: Thông tin
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_CustomerAndGroup_GetCustomerGroupIdByCustomerId("7b485eca-3cd0-3a1f-5b5a-6dec7329b7db");
'
BEGIN
  SELECT
    c.CustomerGroupId
  FROM customerandgroup c
  WHERE  c.CustomerId = CustomerId;
END;

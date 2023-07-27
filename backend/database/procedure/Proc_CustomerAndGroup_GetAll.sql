CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_CustomerAndGroup_GetAll ()
COMMENT '
  Output: Danh sách tất cả chức vụ
  Author: LeDucTiep (16/05/2023)
  Test: CALL `misa.web202303_mf1609_leductiep`.Proc_Position_GetAll ()
'
BEGIN
  SELECT
    c.CustomerGroupId,
    c.CustomerId,
    c.CreatedDate,
    c.CreatedBy,
    c.ModifiedDate,
    c.ModifiedBy
  FROM customerandgroup c;

END;
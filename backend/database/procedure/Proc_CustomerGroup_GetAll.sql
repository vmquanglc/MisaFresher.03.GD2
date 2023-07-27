CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_CustomerGroup_GetAll ()
COMMENT '
  Output: Danh sách tất cả
  Author: LeDucTiep (16/05/2023)
  Test: CALL `misa.web202303_mf1609_leductiep`.Proc_CustomerGroup_GetAll ()
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
  FROM customergroup c;
END;
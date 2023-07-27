CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_AccountProperty_GetAll ()
COMMENT '
  Output: Danh sách tất cả
  Author: LeDucTiep (16/05/2023)
  Test: CALL `misa.web202303_mf1609_leductiep`.Proc_AccountProperty_GetAll ()
'
BEGIN
  SELECT
    a.AccountPropertyId,
    a.Name,
    a.CreatedDate,
    a.CreatedBy,
    a.ModifiedDate,
    a.ModifiedBy
  FROM accountproperty a;

END;
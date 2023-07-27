CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Nation_GetAll ()
COMMENT '
  Output: Danh sách tất cả
  Author: LeDucTiep (16/05/2023)
  Test: CALL `misa.web202303_mf1609_leductiep`.Proc_Nation_GetAll ()
'
BEGIN
  SELECT
    n.NationId,
    n.Name,
    n.CreatedDate,
    n.CreatedBy,
    n.ModifiedDate,
    n.ModifiedBy
  FROM nation n;

END;
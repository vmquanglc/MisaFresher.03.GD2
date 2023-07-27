DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Position_GetAll;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Position_GetAll ()
COMMENT '
  Output: Danh sách tất cả chức vụ
  Author: LeDucTiep (16/05/2023)
  Test: CALL `misa.web202303_mf1609_leductiep`.Proc_Position_GetAll ()
'
BEGIN
  SELECT
    p.PositionId,
    p.PositionName,
    p.CreatedDate,
    p.CreatedBy,
    p.ModifiedDate,
    p.ModifiedBy
  FROM position_ p;

END;
CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Position_GetById (IN positionId char(36))
COMMENT '
  Output: Thông tin chức vụ
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_Position_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    p.PositionId,
    p.PositionName,
    p.CreatedDate,
    p.CreatedBy,
    p.ModifiedDate,
    p.ModifiedBy
  FROM position_ p
  WHERE p.PositionId = positionId;
END;

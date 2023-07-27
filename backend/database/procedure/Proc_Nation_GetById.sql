﻿CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Nation_GetById (IN nationId char(36))
COMMENT '
  Output: Thông tin 
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_Nation_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    n.NationId,
    n.Name,
    n.CreatedDate,
    n.CreatedBy,
    n.ModifiedDate,
    n.ModifiedBy
  FROM nation n
  WHERE n.NationId = nationId;
END;

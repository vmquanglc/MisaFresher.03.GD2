CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_AccountProperty_GetById (IN accountPropertyId char(36))
COMMENT '
  Output: Thông tin
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_AccountProperty_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    a.accountPropertyId,
    a.Name,
    a.CreatedDate,
    a.CreatedBy,
    a.ModifiedDate,
    a.ModifiedBy
  FROM accountproperty a
  WHERE a.accountPropertyId = accountPropertyId;
END;

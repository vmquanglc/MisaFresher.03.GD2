CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_City_GetById (IN cityId char(36), IN nationId char(36))
COMMENT '
  Output: Thông tin
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_City_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    c.cityId,
    c.Name,
    c.nationId,
    c.CreatedDate,
    c.CreatedBy,
    c.ModifiedDate,
    c.ModifiedBy
  FROM city c
  WHERE c.cityId = cityId
  AND c.nationId LIKE concat('%', nationId, '%')
  ;
END;

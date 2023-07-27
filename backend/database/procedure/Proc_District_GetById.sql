CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_District_GetById (IN districtId char(36), IN cityId char(36))
COMMENT '
  Output: Thông tin chức vụ
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_District_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    d.districtId,
    d.Name,
    d.cityId,
    d.CreatedDate,
    d.CreatedBy,
    d.ModifiedDate,
    d.ModifiedBy
  FROM district d
  WHERE d.districtId = districtId
  AND d.cityId LIKE concat('%', cityId, '%')
  ;
END;

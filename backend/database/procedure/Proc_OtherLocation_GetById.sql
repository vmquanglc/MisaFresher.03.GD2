CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_OtherLocation_GetById (IN otherLocationId char(36))
COMMENT '
  Output: Tất cả trường của 1 row 
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_OtherLocation_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    o.OtherLocationId,
    o.CustomerId,
    o.NationId,
    o.CityId,
    o.DistrictId,
    o.CommuneId,
    o.IsSameCustomerAddress,
    o.CreatedDate,
    o.CreatedBy,
    o.ModifiedDate,
    o.ModifiedBy
  FROM otherlocation o
  WHERE o.OtherLocationId = otherLocationId;
END;

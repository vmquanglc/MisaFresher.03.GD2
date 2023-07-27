﻿CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_OtherLocation_GetByCustomerId (IN customerId char(36))
COMMENT '
  Param: customerId: Id khach hang
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL `misa.web202303_mf1609_leductiep`.Proc_OtherLocation_GetByCustomerId("06306268-f3b5-11ed-a084-d8bbc1b19103");
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
  WHERE o.customerId = customerId;
END; 




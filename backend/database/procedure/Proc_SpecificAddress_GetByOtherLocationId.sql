CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_SpecificAddress_GetByOtherLocationId (IN otherLocationId char(36))
COMMENT '
  Param: otherLocationId: Id other location
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL `misa.web202303_mf1609_leductiep`.Proc_SpecificAddress_GetByOtherLocationId("2e85742d-44d7-57df-faf7-7841d6caa989");
'
BEGIN
  SELECT
    s.SpecificAddressId,
    s.OtherLocationId,
    s.Address,
    s.CreatedDate,
    s.CreatedBy,
    s.ModifiedDate,
    s.ModifiedBy
  FROM specificaddress s
  WHERE s.otherLocationId = otherLocationId;
END; 




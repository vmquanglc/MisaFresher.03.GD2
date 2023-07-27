CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_SpecificAddress_GetById (IN specificAddressId char(36))
COMMENT '
  Output: Thông tin chức vụ
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_SpecificAddress_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    s.specificAddressId,
    s.OtherLocationId,
    s.Address,
    s.CreatedDate,
    s.CreatedBy,
    s.ModifiedDate,
    s.ModifiedBy
  FROM specificaddress s
  WHERE s.specificAddressId = specificAddressId;
END;

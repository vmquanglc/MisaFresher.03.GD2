DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_SpecificAddress_GetAll;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_SpecificAddress_GetAll ()
COMMENT '
  Output: Danh sách tất cả
  Author: LeDucTiep (16/05/2023)
  Test: CALL `misa.web202303_mf1609_leductiep`.Proc_SpecificAddress_GetAll ()
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
  FROM specificaddress s;

END;
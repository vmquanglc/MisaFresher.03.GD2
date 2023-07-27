DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_ContactInfor_GetAll;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_ContactInfor_GetAll ()
COMMENT '
  Output: Danh sách tất cả
  Author: LeDucTiep (16/05/2023)
  Test: CALL `misa.web202303_mf1609_leductiep`.Proc_ContactInfor_GetAll ()
'
BEGIN
  SELECT
    c.ContactInforId,
    c.Vocative,
    c.FullName,
    c.Email,
    c.PhoneNumber,
    c.LandlineNumber,
    c.RecipientFullName,
    c.RecipientEmail,
    c.RecipientPhoneNumber,
    c.LegalRepresentative,
    c.CreatedDate,
    c.CreatedBy,
    c.ModifiedDate,
    c.ModifiedBy
  FROM contactinfor c;

END;
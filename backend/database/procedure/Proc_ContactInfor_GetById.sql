DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_ContactInfor_GetById;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_ContactInfor_GetById (IN contactInforId char(36))
COMMENT '
  Output: Thông tin 
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_ContactInfor_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    c.contactInforId,
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
  FROM contactinfor c
  WHERE c.contactInforId = contactInforId;
END;

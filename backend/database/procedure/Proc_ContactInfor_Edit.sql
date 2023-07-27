CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_ContactInfor_Edit (IN contactInforId char(36),
IN vocative varchar(100),
IN fullName varchar(100),
IN email varchar(100),
IN phoneNumber varchar(50),
IN landlineNumber varchar(50),
IN recipientFullName varchar(100),
IN recipientEmail varchar(100),
IN recipientPhoneNumber varchar(50),
IN legalRepresentative varchar(100),
IN modifiedDate datetime,
IN modifiedBy varchar(100))
COMMENT '
    Param: positionId: Id chức vụ
           positionName: Tên chức vụ 
           modifiedBy: Người sửa
           modifiedDate: Ngày sửa 
    Test:
        CALL Proc_Position_Edit("11123110-7667-44cf-8cd7-89b22d10517e", "Nhân viên", "2012-04-19 13:08:22", "Le Duc Tiep");
'
BEGIN

  UPDATE contactinfor c
  SET c.vocative = vocative,
      c.fullName = fullName,
      c.email = email,
      c.phoneNumber = phoneNumber,
      c.landlineNumber = landlineNumber,
      c.recipientFullName = recipientFullName,
      c.recipientEmail = recipientEmail,
      c.recipientPhoneNumber = recipientPhoneNumber,
      c.legalRepresentative = legalRepresentative,
      c.modifiedDate = modifiedDate,
      c.modifiedBy = modifiedBy
  WHERE c.contactInforId = contactInforId;

END;
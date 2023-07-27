CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_ContactInfor_Add (IN contactInforId char(36),
IN vocative varchar(100),
IN fullName varchar(100),
IN email varchar(100),
IN phoneNumber varchar(50),
IN landlineNumber varchar(50),
IN recipientFullName varchar(100),
IN recipientEmail varchar(100),
IN recipientPhoneNumber varchar(50),
IN legalRepresentative varchar(100),
IN createdDate datetime,
IN createdBy varchar(100))
COMMENT '
  Param: positionId: Mã chức vụ
         positionName: Tên chức vụ
         createdBy: Người tạo,
         createdDate: Ngày tạo
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL Proc_Position_Add(UUID(), "Intern", "2012-04-19 13:08:22","Le Duc Tiep");
'
BEGIN
  INSERT INTO contactinfor (contactInforId,
  vocative,
  fullName,
  email,
  phoneNumber,
  landlineNumber,
  recipientFullName,
  recipientEmail,
  recipientPhoneNumber,
  legalRepresentative,
  createdDate,
  createdBy)
    VALUES (contactInforId, vocative, fullName, email, phoneNumber, landlineNumber, recipientFullName, recipientEmail, recipientPhoneNumber, legalRepresentative, createdDate, createdBy);
END
;

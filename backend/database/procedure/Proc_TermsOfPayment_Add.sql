CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_TermsOfPayment_Add (IN termsOfPaymentId char(36),
IN termsOfPaymentCode varchar(20),
IN name varchar(255),
IN maxDaysOwed int(5),
IN discountPeriod int(5),
IN discountRate int(3),
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

  INSERT INTO termsofpayment (termsOfPaymentId,
  termsOfPaymentCode,
  name,
  maxDaysOwed,
  discountPeriod,
  discountRate,
  createdDate,
  createdBy)
    VALUES (termsOfPaymentId, termsOfPaymentCode, name, maxDaysOwed, discountPeriod, discountRate, createdDate, createdBy);

END
;

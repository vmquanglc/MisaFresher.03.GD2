CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_TermsOfPayment_Edit (IN termsOfPaymentId char(36),
IN termsOfPaymentCode varchar(20),
IN name varchar(255),
IN maxDaysOwed int(5),
IN discountPeriod int(5),
IN discountRate int(3),
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

  UPDATE termsofpayment t
  SET t.name = name,
      t.maxDaysOwed = maxDaysOwed,
      t.discountPeriod = discountPeriod,
      t.discountRate = discountRate,
      t.modifiedDate = modifiedDate,
      t.modifiedBy = modifiedBy
  WHERE t.termsOfPaymentId = termsOfPaymentId;

END;

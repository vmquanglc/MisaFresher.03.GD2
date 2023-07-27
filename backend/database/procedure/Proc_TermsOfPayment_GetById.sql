CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_TermsOfPayment_GetById (IN termsOfPaymentId char(36))
COMMENT '
  Output: Thông tin chức vụ
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_TermsOfPayment_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    t.termsOfPaymentId,
    t.TermsOfPaymentCode,
    t.Name,
    t.MaxDaysOwed,
    t.DiscountPeriod,
    t.DiscountRate,
    t.CreatedDate,
    t.CreatedBy,
    t.ModifiedDate,
    t.ModifiedBy
  FROM termsofpayment t
  WHERE t.termsOfPaymentId = termsOfPaymentId;
END;

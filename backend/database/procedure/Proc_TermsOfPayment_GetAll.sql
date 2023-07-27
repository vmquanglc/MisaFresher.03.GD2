CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_TermsOfPayment_GetAll ()
COMMENT '
  Output: Danh sách tất cả chức vụ
  Author: LeDucTiep (16/05/2023)
  Test: CALL `misa.web202303_mf1609_leductiep`.Proc_TermsOfPayment_GetAll ()
'
BEGIN
  SELECT
    t.TermsOfPaymentId,
    t.TermsOfPaymentCode,
    t.Name,
    t.MaxDaysOwed,
    t.DiscountPeriod,
    t.DiscountRate,
    t.CreatedDate,
    t.CreatedBy,
    t.ModifiedDate,
    t.ModifiedBy
  FROM termsofpayment t;

END;
DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Customer_GetAll;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Customer_GetAll ()
COMMENT '
  Output: Danh sách tất cả
  Author: LeDucTiep (16/05/2023)
  Test: CALL `misa.web202303_mf1609_leductiep`.Proc_Customer_GetAll ()
'
BEGIN
  SELECT
    c.CustomerId,
    c.CustomerType,
    c.IsSupplier,
    c.TaxCode,
    c.CustomerCode,
    c.FullName,
    c.Address,
    c.PhoneNumber,
    c.Website,
    c.EmployeeId,
    c.ContactInforId,
    c.TermsOfPaymentId,
    c.MaxDaysOwed,
    c.MaxAmountOfDebt,
    c.AccountCollectId,
    c.AccountPayId,
    c.Note,
    c.IdentityNumber,
    c.IdentityDate,
    c.IdentityPlace,
    c.CreatedDate,
    c.CreatedBy,
    c.ModifiedDate,
    c.ModifiedBy
  FROM customer c;

END;
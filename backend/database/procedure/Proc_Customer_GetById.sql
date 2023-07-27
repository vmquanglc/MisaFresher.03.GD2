DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Customer_GetById;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Customer_GetById (IN customerId char(36))
COMMENT '
  Output: Thông tin
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_Customer_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
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
  FROM customer c
  WHERE c.customerId = customerId;
END;

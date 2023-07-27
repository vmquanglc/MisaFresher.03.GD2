CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_GetById (IN accountId char(36))
COMMENT '
  Output: Thông tin
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_Account_GetById("3f8e6896-4c7d-15f5-a018-75d8bd200d7c");
'
BEGIN
  SELECT
    a.accountId,
    a.AccountCode,
    a.Rank,
    a.MisaCode,
    a.NameVi,
    a.NameEn,
    a.AccountSyntheticId,
    a.AccountPropertyId,
    a.Note,
    a.HasForeignCurrencyAccounting,
    a.ObjectType,
    a.CostAggregationObject,
    a.TheOrder,
    a.PurchaseContract,
    a.Unit,
    a.BankAccount,
    a.Construction,
    a.ContractOfSale,
    a.ExpenseItem,
    a.StatisticalCode,
    a.CreatedDate,
    a.CreatedBy,
    a.ModifiedDate,
    a.ModifiedBy
  FROM account a
  WHERE a.accountId = accountId;
END;

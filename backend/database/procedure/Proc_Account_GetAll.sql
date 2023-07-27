CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_GetAll ()
COMMENT '
  Output: Danh sách tất cả chức vụ
  Author: LeDucTiep (16/05/2023)
  Test: CALL `misa.web202303_mf1609_leductiep`.Proc_Account_GetAll ()
'
BEGIN
  SELECT
    a.AccountId,
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
  FROM account a;

END;
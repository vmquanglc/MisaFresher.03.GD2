CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_Export()
COMMENT '
  Param: _limit: Số bản ghi cần lấy
         _offset: Số bản ghi bỏ qua
         searchTerm: FullName, số điện thoại hoặc EmployeeCode cần tìm
  Output: customer.*
          totalRecord: Tổng số bản ghi
  Author: LeDucTiep (06/05/2023)
  Test: CALL Proc_Employee_Paging(0, 10, "Đan", @totalRecord);
        SELECT
          @totalRecord;
'
BEGIN
  -- Lấy ra dữ liệu 
  SELECT
    a.AccountId,
    a.AccountCode,
    a.Rank,
    a.MisaCode,
    a.NameVi,
    a.NameEn,
    a.AccountSyntheticId,
    a.AccountPropertyId,
    a1.Name AS AccountPropertyName,
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
    LEFT JOIN accountproperty a1
      ON a.AccountPropertyId = a1.AccountPropertyId
  ;
END;
CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_Paging (IN _offset int,
IN _limit int,
IN searchTerm varchar(255),
OUT totalRecord int)
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

  WHERE a.AccountCode LIKE concat('%', searchTerm, '%')
  OR a.NameVi LIKE concat('%', searchTerm, '%')
  OR a.NameEn LIKE concat('%', searchTerm, '%')
  OR coalesce(a1.Name, '') LIKE concat('%', searchTerm, '%')
  OR a.MisaCode LIKE concat('%', searchTerm, '%')

  LIMIT _limit

  OFFSET _offset
  ;

  -- Lấy ra số lượng bản ghi

  SELECT
    COUNT(1) INTO totalRecord
  FROM account a
    LEFT JOIN accountproperty a1
      ON a.AccountPropertyId = a1.AccountPropertyId
  WHERE a.AccountCode LIKE concat('%', searchTerm, '%')
  OR a.NameVi LIKE concat('%', searchTerm, '%')
  OR a.NameEn LIKE concat('%', searchTerm, '%')
  OR coalesce(a1.Name, '') LIKE concat('%', searchTerm, '%')
  OR a.MisaCode LIKE concat('%', searchTerm, '%')
  ;
END;


CALL Proc_Account_Paging(0, 99, 'Dư nợ', @totalRecord);

SELECT
  @totalRecord;

CALL Proc_Account_Paging(0, 99, '', @totalRecord);

SELECT
  @totalRecord;



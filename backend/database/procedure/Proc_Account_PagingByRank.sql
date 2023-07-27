CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_PagingByRank (IN _offset int,
IN _limit int,
IN searchTerm varchar(255),
IN misaCode varchar(255),
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
    IF(COUNT(a2.AccountId) > 0, TRUE, FALSE) AS IsParent,
    a.AccountCode,
    a.Rank,
    a.misaCode,
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
    LEFT JOIN account a2
      ON a.AccountId = a2.AccountSyntheticId

  WHERE IF(misaCode != ''
  AND searchTerm = '', (SELECT
      a.Rank = (SELECT
          MIN(Rank)
        FROM account
        WHERE account.misaCode LIKE CONCAT(misaCode, '_%'))), (
  TRUE))

  AND a.misaCode LIKE CONCAT(misaCode, '_%')


  AND (a.AccountCode LIKE CONCAT('%', searchTerm, '%')
  OR a.NameVi LIKE CONCAT('%', searchTerm, '%')
  OR a.NameEn LIKE CONCAT('%', searchTerm, '%')
  OR a.misaCode LIKE CONCAT('%', searchTerm, '%')
  )


  GROUP BY a.AccountCode,
           a.Rank



  LIMIT _limit

  OFFSET _offset
  ;

  -- Lấy ra số lượng bản ghi

  SELECT
    COUNT(1) INTO totalRecord
  FROM account a

  WHERE IF(misaCode != ''
  AND searchTerm = '', (SELECT
      a.Rank = (SELECT
          MIN(Rank)
        FROM account
        WHERE account.misaCode LIKE CONCAT(misaCode, '_%'))), (
  TRUE))

  AND a.misaCode LIKE CONCAT(misaCode, '_%')

  AND (a.AccountCode LIKE CONCAT('%', searchTerm, '%')
  OR a.NameVi LIKE CONCAT('%', searchTerm, '%')
  OR a.NameEn LIKE CONCAT('%', searchTerm, '%')
  OR a.misaCode LIKE CONCAT('%', searchTerm, '%')
  )
  ;
END;


CALL Proc_Account_PagingByRank(0, 99, '0296112', '', @totalRecord);

SELECT
  @totalRecord;

CALL Proc_Account_PagingByRank(0, 99, '', '02961/', @totalRecord);

SELECT
  @totalRecord;



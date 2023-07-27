DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Account_Add;

CREATE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_Add (IN accountId char(36),
IN accountCode varchar(20),
IN rank int(2),
IN misaCode varchar(255),
IN nameVi varchar(100),
IN nameEn varchar(100),
IN accountSyntheticId char(36),
IN accountPropertyId char(36),
IN note varchar(255),
IN hasForeignCurrencyAccounting tinyint(1),
IN objectType int(1),
IN costAggregationObject int(1),
IN theOrder int(1),
IN purchaseContract int(1),
IN unit int(1),
IN bankAccount int(1),
IN construction int(1),
IN contractOfSale int(1),
IN expenseItem int(1),
IN statisticalCode int(1),
IN createdDate datetime,
IN createdBy varchar(100))
COMMENT '
  Param: positionId: Mã chức vụ
         positionName: Tên chức vụ
         createdBy: Người tạo,
         createdDate: Ngày tạo
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL Proc_Account_Add(UUID(), "Intern", "2012-04-19 13:08:22","Le Duc Tiep");
'
BEGIN

  INSERT INTO account (accountId,
  accountCode,
  rank,
  misaCode,
  nameVi, nameEn,
  accountSyntheticId,
  accountPropertyId,
  note,
  hasForeignCurrencyAccounting,
  objectType,
  costAggregationObject,
  theOrder,
  purchaseContract,
  unit,
  bankAccount,
  construction,
  contractOfSale,
  expenseItem,
  statisticalCode,
  createdDate,
  createdBy)
    VALUES (accountId, accountCode, rank, misaCode, nameVi, nameEn, accountSyntheticId, accountPropertyId, note, hasForeignCurrencyAccounting, objectType, costAggregationObject, theOrder, purchaseContract, unit, bankAccount, construction, contractOfSale, expenseItem, statisticalCode, createdDate, createdBy);

END
;

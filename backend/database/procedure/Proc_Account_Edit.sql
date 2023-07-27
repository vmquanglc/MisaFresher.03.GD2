CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Account_Edit (IN accountId char(36),
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
IN modifiedDate datetime,
IN modifiedBy varchar(100))
COMMENT '
    Param: positionId: Id chức vụ
           positionName: Tên chức vụ 
           modifiedBy: Người sửa
           modifiedDate: Ngày sửa 
    Test:
        CALL Proc_Position_Edit("11123110-7667-44cf-8cd7-89b22d10517e", "Nhân viên", "2012-04-19 13:08:22", "Le Duc Tiep");
'
BEGIN

  UPDATE account a
  SET a.accountCode = accountCode,
      a.rank = rank,
      a.misaCode = misaCode,
      a.nameVi = nameVi,
      a.nameEn = nameEn,
      a.accountSyntheticId = accountSyntheticId,
      a.accountPropertyId = accountPropertyId,
      a.note = note,
      a.hasForeignCurrencyAccounting = hasForeignCurrencyAccounting,
      a.objectType = objectType,
      a.costAggregationObject = costAggregationObject,
      a.theOrder = theOrder,
      a.purchaseContract = purchaseContract,
      a.unit = unit,
      a.bankAccount = bankAccount,
      a.construction = construction,
      a.contractOfSale = contractOfSale,
      a.expenseItem = expenseItem,
      a.statisticalCode = statisticalCode,
      a.modifiedDate = modifiedDate,
      a.modifiedBy = modifiedBy
  WHERE a.accountId = accountId;

END;
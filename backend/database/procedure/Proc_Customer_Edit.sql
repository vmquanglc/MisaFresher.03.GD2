CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Customer_Edit (IN customerId char(36),
IN customerType int(1),
IN isSupplier int(1),
IN taxCode varchar(50),
IN customerCode varchar(20),
IN fullName varchar(100),
IN address varchar(255),
IN phoneNumber varchar(50),
IN website varchar(255),
IN employeeId char(36),
IN contactInforId char(36),
IN termsOfPaymentId char(36),
IN maxDaysOwed int(5),
IN maxAmountOfDebt decimal(18, 4),
IN accountCollectId char(36),
IN accountPayId char(36),
IN note varchar(255),
IN identityNumber varchar(25),
IN identityDate datetime,
IN identityPlace varchar(255),
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


  UPDATE customer c
  SET c.customerType = customerType,
      c.isSupplier = isSupplier,
      c.taxCode = taxCode,
      c.customerCode = customerCode,
      c.fullName = fullName,
      c.address = address,
      c.phoneNumber = phoneNumber,
      c.website = website,
      c.employeeId = employeeId,
      c.contactInforId = contactInforId,
      c.termsOfPaymentId = termsOfPaymentId,
      c.maxDaysOwed = maxDaysOwed,
      c.maxAmountOfDebt = maxAmountOfDebt,
      c.accountCollectId = accountCollectId,
      c.accountPayId = accountPayId,
      c.note = note,
      c.identityNumber = identityNumber,
      c.identityDate = identityDate,
      c.identityPlace = identityPlace,
      c.modifiedDate = modifiedDate,
      c.modifiedBy = modifiedBy
  WHERE c.customerId = customerId;

END;
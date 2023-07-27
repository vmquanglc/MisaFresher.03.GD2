CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Customer_Add (IN customerId char(36),
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
IN createdDate datetime,
IN createdBy varchar(100))
COMMENT '
  Param: positionId: Mã chức vụ
         positionName: Tên chức vụ
         createdBy: Người tạo,
         createdDate: Ngày tạo
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL Proc_Position_Add(UUID(), "Intern", "2012-04-19 13:08:22","Le Duc Tiep");
'
BEGIN

  INSERT INTO customer (customerId,
  customerType,
  isSupplier,
  taxCode,
  customerCode,
  fullName,
  address,
  phoneNumber,
  website,
  employeeId,
  contactInforId,
  termsOfPaymentId,
  maxDaysOwed,
  maxAmountOfDebt,
  accountCollectId,
  accountPayId,
  note,
  identityNumber,
  identityDate,
  identityPlace,
  createdDate,
  createdBy)
    VALUES (customerId, customerType, isSupplier, taxCode, customerCode, fullName, address, phoneNumber, website, employeeId, contactInforId, termsOfPaymentId, maxDaysOwed, maxAmountOfDebt, accountCollectId, accountPayId, note, identityNumber, identityDate, identityPlace, createdDate, createdBy);

END
;

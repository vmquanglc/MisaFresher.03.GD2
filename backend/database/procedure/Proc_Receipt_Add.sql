CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Receipt_Add (IN receiptId char(36),
IN receiptCode varchar(20),
IN IsRecorded tinyint(1),
IN receiptType int(2),
IN customerId char(36),
IN customerName varchar(100),
IN payer varchar(100),
IN address varchar(255),
IN employeeId char(36),
IN reason varchar(255),
IN amount int(11),
IN accountingDate datetime,
IN receiptDate datetime,
IN createdBy varchar(100),
IN createdDate datetime)
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

  INSERT INTO receipt (receiptId, receiptCode, IsRecorded, receiptType, customerId, customerName, payer, address, employeeId, reason, amount, accountingDate, receiptDate, createdBy, createdDate)
    VALUES (receiptId, receiptCode, IsRecorded, receiptType, customerId, customerName, payer, address, employeeId, reason, amount, accountingDate, receiptDate, createdBy, createdDate);

END
;

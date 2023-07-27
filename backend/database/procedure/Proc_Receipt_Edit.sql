CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Receipt_Edit (IN receiptId char(36),
IN receiptCode varchar(20),
IN isRecorded tinyint(1),
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
IN modifiedBy varchar(100),
IN modifiedDate datetime)
COMMENT '
    Param: positionId: Id chức vụ
           positionName: Tên chức vụ 
           modifiedBy: Người sửa
           modifiedDate: Ngày sửa 
    Test:
        CALL Proc_Position_Edit("11123110-7667-44cf-8cd7-89b22d10517e", "Nhân viên", "2012-04-19 13:08:22", "Le Duc Tiep");
'
BEGIN

  UPDATE receipt r
  SET r.receiptCode = receiptCode,
      r.isRecorded = isRecorded,
      r.receiptType = receiptType,
      r.customerId = customerId,
      r.customerName = customerName,
      r.payer = payer,
      r.employeeId = employeeId,
      r.reason = reason,
      r.amount = amount,
      r.accountingDate = accountingDate,
      r.receiptDate = receiptDate,
      r.address = address,
      r.modifiedDate = modifiedDate,
      r.modifiedBy = modifiedBy
  WHERE r.receiptId = receiptId;

END;
CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_BookKeeping_Add (IN receiptId char(36),
IN bookKeepingId char(36),
IN note varchar(255),
IN debitAccountId char(36),
IN collectAccountId char(36),
IN amountOfMoney decimal(18, 4),
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

  INSERT INTO bookkeeping (receiptId, bookKeepingId, note, debitAccountId, collectAccountId, amountOfMoney, createdBy, createdDate)
    VALUES (receiptId, bookKeepingId, note, debitAccountId, collectAccountId, amountOfMoney, createdBy, createdDate);
END
;

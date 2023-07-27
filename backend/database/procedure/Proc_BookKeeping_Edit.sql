CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_BookKeeping_Edit (IN receiptId char(36),
IN bookKeepingId char(36),
IN note varchar(255),
IN debitAccountId char(36),
IN collectAccountId char(36),
IN amountOfMoney decimal(18, 4),
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

  UPDATE bookkeeping b
  SET b.receiptId = receiptId,
      b.note = note,
      b.debitAccountId = debitAccountId,
      b.collectAccountId = collectAccountId,
      b.amountOfMoney = amountOfMoney,
      b.modifiedDate = modifiedDate,
      b.modifiedBy = modifiedBy
  WHERE b.bookKeepingId = bookKeepingId;

END;
CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Receipt_Export ()
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
    r.ReceiptId,
    r.IsRecorded,
    r.ReceiptCode,
    r.ReceiptType,
    r.CustomerId,
    r.CustomerName,
    r.Payer,
    r.Address,
    r.EmployeeId,
    r.Reason,
    r.Amount,
    r.AccountingDate,
    r.ReceiptDate,
    (SELECT
        SUM(b.AmountOfMoney)
      FROM bookkeeping b
      WHERE b.ReceiptId = r.ReceiptId) AS TotalMoney,
    (SELECT
        c.CustomerCode
      FROM customer c
      WHERE c.CustomerId = r.CustomerId) AS CustomerCode
  FROM receipt r
  ;
END;

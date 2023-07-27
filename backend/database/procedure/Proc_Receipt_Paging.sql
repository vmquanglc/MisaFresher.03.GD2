CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Receipt_Paging (IN _offset int,
IN _limit int,
IN searchTerm varchar(255),
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

  WHERE coalesce(r.ReceiptCode, '') LIKE concat('%', searchTerm, '%')
  OR coalesce(r.Payer, '') LIKE concat('%', searchTerm, '%')
  OR coalesce(r.CustomerName, '') LIKE concat('%', searchTerm, '%')

  ORDER BY r.ReceiptCode DESC

  LIMIT _limit

  OFFSET _offset;

  -- Lấy ra số lượng bản ghi

  SELECT
    COUNT(1) INTO totalRecord
  FROM receipt r
  WHERE coalesce(r.ReceiptCode, '') LIKE concat('%', searchTerm, '%')
  OR coalesce(r.Payer, '') LIKE concat('%', searchTerm, '%')
  OR coalesce(r.CustomerName, '') LIKE concat('%', searchTerm, '%')
  ;
END;


CALL Proc_Receipt_Paging(0, 20, '', @totalRecord);

SELECT
  @totalRecord;
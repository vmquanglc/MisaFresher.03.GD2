CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_BookKeeping_Paging (IN _offset int,
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
    b.ReceiptId,
    b.BookKeepingId,
    b.Note,
    b.DebitAccountId,
    b.CollectAccountId,
    b.AmountOfMoney,
    b.CreatedBy,
    b.CreatedDate,
    b.ModifiedBy,
    b.ModifiedDate
  FROM bookkeeping b

  WHERE COALESCE(b.Note, '')  LIKE concat('%', searchTerm, '%')

  LIMIT _limit

  OFFSET _offset;

  -- Lấy ra số lượng bản ghi

  SELECT
    COUNT(1) INTO totalRecord

  FROM bookkeeping b

  WHERE b.Note LIKE concat('%', searchTerm, '%')
  ;
END;


CALL Proc_BookKeeping_Paging(0, 20, '', @totalRecord);

SELECT
  @totalRecord;
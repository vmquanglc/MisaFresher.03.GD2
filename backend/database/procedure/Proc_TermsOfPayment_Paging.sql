CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_TermsOfPayment_Paging (IN _offset int,
IN _limit int,
IN searchTerm varchar(255),
OUT totalRecord int)
COMMENT '
  Param: _limit: Số bản ghi cần lấy
         _offset: Số bản ghi bỏ qua
         searchTerm: FullName, số điện thoại hoặc EmployeeCode cần tìm
  Output: .*
          totalRecord: Tổng số bản ghi
  Author: LeDucTiep (06/05/2023)
  Test: CALL Proc_Employee_Paging(0, 10, "Đan", @totalRecord);
        SELECT
          @totalRecord;
'
BEGIN
  -- Lấy ra dữ liệu 
  SELECT
    t.TermsOfPaymentId,
    t.TermsOfPaymentCode,
    t.Name,
    t.MaxDaysOwed,
    t.DiscountPeriod,
    t.DiscountRate,
    t.CreatedDate,
    t.CreatedBy,
    t.ModifiedDate,
    t.ModifiedBy
  FROM termsofpayment t

  WHERE t.Name LIKE CONCAT('%', searchTerm, '%') 
  OR t.TermsOfPaymentCode LIKE CONCAT('%', searchTerm, '%') 
  
  LIMIT _limit

  OFFSET _offset;

  -- Lấy ra số lượng bản ghi

  SELECT
    COUNT(t.TermsOfPaymentId) INTO totalRecord
  FROM termsofpayment t
 WHERE t.Name LIKE CONCAT('%', searchTerm, '%') 
  OR t.TermsOfPaymentCode LIKE CONCAT('%', searchTerm, '%') 
  ;
END;


CALL Proc_TermsOfPayment_Paging(1, 1, '', @totalRecord);

SELECT
  @totalRecord;
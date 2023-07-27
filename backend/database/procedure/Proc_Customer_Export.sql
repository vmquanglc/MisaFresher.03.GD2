CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Customer_Export ()
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
    c.CustomerId,
    c.CustomerType,
    c.IsSupplier,
    c.TaxCode,
    c.CustomerCode,
    c.FullName,
    c.Address,
    c.PhoneNumber,
    c.Website,
    c.EmployeeId,
    c.ContactInforId,
    c.TermsOfPaymentId,
    c.MaxDaysOwed,
    c.MaxAmountOfDebt,
    c.AccountCollectId,
    c.AccountPayId,
    c.Note,
    c.IdentityNumber,
    c.IdentityDate,
    c.IdentityPlace,
    c.CreatedDate,
    c.CreatedBy,
    c.ModifiedDate,
    c.ModifiedBy
  FROM customer c
;
END;

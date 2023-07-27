CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Customer_Delete (IN customerId char(36))
COMMENT '
  Param: employeeId: Id chức vụ cần xóa
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL `misa.web202303_mf1609_leductiep`.Proc_Customer_Delete("114c10d1-3839-47e4-45da-f444281e2faa");
'
BEGIN
    DELETE
      FROM customer
    WHERE customer.customerId = customerId;
END; 

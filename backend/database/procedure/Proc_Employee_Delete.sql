DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Employee_Delete;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Employee_Delete (IN employeeId char(36))
COMMENT '
  Param: employeeId: Mã nhân viên cần xóa
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL `misa.web202303_mf1609_leductiep`.Proc_Employee_Delete("06306268-f3b5-11ed-a084-d8bbc1b19103");
'
BEGIN
    DELETE
      FROM employee
    WHERE employee.EmployeeId = employeeId;
END; 




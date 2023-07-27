DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Employee_DeleteMany;

CREATE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Employee_DeleteMany (IN arrayId varchar(400))
COMMENT "
  Param: arrayId: Danh sách các id của employee
  Author: LeDucTiep (04/06/2023)
  Test: Call `misa.web202303_mf1609_leductiep`.Proc_Employee_DeleteMany('\'14a7a012-4c25-4c58-dc41-c1ec321fe728\', \'1826501f-5391-6712-1f42-c1ec321fe728\'');
"

BEGIN

 SET @sql = CONCAT('DELETE FROM employee WHERE employee.EmployeeId  IN (', arrayId, ')');
  PREPARE stmt FROM @sql;
  EXECUTE stmt;
  DEALLOCATE PREPARE stmt;

END
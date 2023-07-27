DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Department_GetAll;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Department_GetAll ()
COMMENT '
  Output: Danh sách tất cả phòng ban
  Author: LeDucTiep (16/05/2023)
  Test: Call `misa.web202303_mf1609_leductiep`.Proc_Department_GetAll();
'
BEGIN
  SELECT
    d.DepartmentId,
    d.DepartmentName,
    d.CreatedDate,
    d.CreatedBy,
    d.ModifiedDate,
    d.ModifiedBy
  FROM department d;
END;
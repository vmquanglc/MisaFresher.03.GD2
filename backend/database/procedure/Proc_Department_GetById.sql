DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Department_GetById;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Department_GetById (IN departmentId char(36))
COMMENT '
  Output: Thông tin phòng ban
  Author: LeDucTiep (17/05/2023)
  Test: CALL Proc_Department_GetById("142cb08f-7c31-21fa-8e90-67245e8b283e");
'
BEGIN
  SELECT
    d.DepartmentId,
    d.DepartmentName,
    d.CreatedDate,
    d.CreatedBy,
    d.ModifiedDate,
    d.ModifiedBy
  FROM department d
  WHERE d.DepartmentId = departmentId;
END;

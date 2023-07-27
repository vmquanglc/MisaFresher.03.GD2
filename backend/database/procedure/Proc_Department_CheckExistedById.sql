CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Department_CheckExistedById (IN departmentId char(36))
COMMENT '
  Param: employeeId: Id của phòng ban
  Output: true: có
  Author: LeDucTiep (17/05/2023)
  Test: 
      CALL Proc_Department_CheckExistedById("142cb08f-7c31-21fa-8e90-67245e8b283e");
'
BEGIN
  SELECT
    EXISTS (SELECT
        *
      FROM department d
      WHERE d.DepartmentId = departmentId);
END;
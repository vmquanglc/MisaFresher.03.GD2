DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Department_Edit;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Department_Edit (IN departmentId char(36),
IN departmentName varchar(255),
IN modifiedDate datetime,
IN modifiedBy varchar(100))
COMMENT '
    Param: departmentId: Id phòng ban
           departmentName: Tên phòng ban
           modifiedBy: Người sửa,
           modifiedDate: Ngày sửa 
    Test:
        CALL Proc_Department_Edit("11123110-7667-44cf-8cd7-89b22d10517e", "Phòng hành chính", "2012-04-19 13:08:22", "Le Duc Tiep");
'
BEGIN

  UPDATE department d
  SET d.departmentName = departmentName,
      d.modifiedDate = modifiedDate,
      d.modifiedBy = modifiedBy
  WHERE d.departmentId = departmentId;

END;
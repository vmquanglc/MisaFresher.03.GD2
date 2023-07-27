DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Department_Add;

CREATE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Department_Add (IN departmentId char(36),
IN departmentName varchar(255),
IN createdDate datetime,
IN createdBy varchar(100))

COMMENT '
  Param: departmentId: Id phòng ban
         departmentName: Tên phòng ban
         createdBy: Người tạo,
         createdDate: Ngày tạo
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL Proc_Department_Add(UUID(), "Kiểm toán", "Le Duc Tiep", "2012-04-19 13:08:22");
'
BEGIN
  INSERT INTO department (departmentId,
  departmentName,
  createdBy,
  createdDate)
    VALUES (departmentId, departmentName, createdBy, createdDate);
END
;

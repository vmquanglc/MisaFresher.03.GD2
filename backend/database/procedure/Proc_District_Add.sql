CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_District_Add (IN districtId char(36),
IN name varchar(255),
IN cityId char(36),
IN createdDate datetime,
IN createdBy varchar(100))
COMMENT '
  Param: positionId: Mã chức vụ
         positionName: Tên chức vụ
         createdBy: Người tạo,
         createdDate: Ngày tạo
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL Proc_Position_Add(UUID(), "Intern", "2012-04-19 13:08:22","Le Duc Tiep");
'
BEGIN

  INSERT INTO district (districtId, name, cityId, createdDate, createdBy)
    VALUES (districtId, name, cityId, createdDate, createdBy);
END
;

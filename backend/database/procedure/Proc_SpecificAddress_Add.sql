CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_SpecificAddress_Add (IN specificAddressId char(36),
IN otherLocationId char(36),
IN address varchar(255),
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

  INSERT INTO specificaddress (specificAddressId, otherLocationId, address, createdDate, createdBy)
    VALUES (specificAddressId, otherLocationId, address, createdDate, createdBy);

END
;

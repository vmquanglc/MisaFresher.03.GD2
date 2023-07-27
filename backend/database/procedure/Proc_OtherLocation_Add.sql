CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_OtherLocation_Add (IN otherLocationId char(36),
IN customerId char(36),
IN nationId char(36),
IN cityId char(36),
IN districtId char(36),
IN communeId char(36),
IN isSameCustomerAddress tinyint(1),
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

  INSERT INTO otherlocation (otherLocationId,
  customerId,
  nationId,
  cityId,
  districtId,
  communeId,
  isSameCustomerAddress,
  createdDate,
  createdBy)
    VALUES (otherLocationId, customerId, nationId, cityId, districtId, communeId, isSameCustomerAddress, createdDate, createdBy);

END
;

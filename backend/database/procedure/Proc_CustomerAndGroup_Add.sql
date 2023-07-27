CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_CustomerAndGroup_Add (IN customerGroupId char(36),
IN customerId char(36),
IN createdDate datetime,
IN createdBy varchar(100))
COMMENT '
  Param: positionId: Mã chức vụ
         positionName: Tên chức vụ
         createdBy: Người tạo,
         createdDate: Ngày tạo
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL Proc_CustomerAndGroup_Add(UUID(), "Intern", "2012-04-19 13:08:22","Le Duc Tiep");
'
BEGIN

  INSERT INTO customerandgroup (customerGroupId, customerId, createdDate, createdBy)
    VALUES (customerGroupId, customerId, createdDate, createdBy);
END
;

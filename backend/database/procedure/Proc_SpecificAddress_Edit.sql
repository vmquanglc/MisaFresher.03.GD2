CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_SpecificAddress_Edit (IN specificAddressId char(36),
IN otherLocationId char(36),
IN address varchar(255),
IN modifiedDate datetime,
IN modifiedBy varchar(100))
COMMENT '
    Param: positionId: Id chức vụ
           positionName: Tên chức vụ 
           modifiedBy: Người sửa
           modifiedDate: Ngày sửa 
    Test:
        CALL Proc_Position_Edit("11123110-7667-44cf-8cd7-89b22d10517e", "Nhân viên", "2012-04-19 13:08:22", "Le Duc Tiep");
'
BEGIN

  UPDATE specificaddress s
  SET s.address = address,
      s.modifiedDate = modifiedDate,
      s.modifiedBy = modifiedBy
  WHERE s.specificAddressId = specificAddressId;

END;
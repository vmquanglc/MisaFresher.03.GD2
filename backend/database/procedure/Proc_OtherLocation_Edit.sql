CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_OtherLocation_Edit (IN otherLocationId char(36),
IN customerId char(36),
IN nationId char(36),
IN cityId char(36),
IN districtId char(36),
IN communeId char(36),
IN isSameCustomerAddress tinyint(1),
IN modifiedDate datetime,
IN modifiedBy varchar(100))
COMMENT '
    Param: positionId: Id chức vụ
           positionName: Tên chức vụ 
           modifiedBy: Người sửa
           modifiedDate: Ngày sửa 
    Test:
        CALL Proc_OtherLocation_Edit("11123110-7667-44cf-8cd7-89b22d10517e", "Nhân viên", "2012-04-19 13:08:22", "Le Duc Tiep");
'
BEGIN

  UPDATE otherlocation o
  SET o.customerId = customerId,
      o.nationId = nationId,
      o.cityId = cityId,
      o.districtId = districtId,
      o.communeId = communeId,
      o.isSameCustomerAddress = isSameCustomerAddress,
      o.modifiedDate = modifiedDate,
      o.modifiedBy = modifiedBy
  WHERE o.otherLocationId = otherLocationId;

END;
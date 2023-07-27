CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Commune_Edit (IN communeId char(36),
IN name varchar(255),
IN districtId char(36),
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

  UPDATE commune c
  SET c.name = name,
      c.districtId = districtId,
      c.modifiedDate = modifiedDate,
      c.modifiedBy = modifiedBy
  WHERE c.communeId = communeId;

END;
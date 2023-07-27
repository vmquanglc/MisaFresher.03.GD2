DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Position_Edit;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Position_Edit (IN positionId char(36),
IN positionName varchar(100),
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

  UPDATE position_ p
  SET p.positionName = positionName,
      p.modifiedDate = modifiedDate,
      p.modifiedBy = modifiedBy
  WHERE p.positionId = positionId;

END;
CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_AccountProperty_Edit (IN accountPropertyId char(36),
IN name varchar(100),
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

  UPDATE accountproperty a
  SET a.name = name,
      a.modifiedDate = modifiedDate,
      a.modifiedBy = modifiedBy
  WHERE a.accountPropertyId = accountPropertyId;

END;
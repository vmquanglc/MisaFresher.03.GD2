CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Nation_Edit (IN nationId char(36),
IN name varchar(50),
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

  UPDATE nation n
  SET n.name = name,
      n.modifiedDate = modifiedDate,
      n.modifiedBy = modifiedBy
  WHERE n.nationId = nationId;

END;
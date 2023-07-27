CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Receipt_SetRecorded (IN receiptId char(36),
IN isRecorded tinyint(1))
COMMENT '
    Param: positionId: Id chức vụ
           positionName: Tên chức vụ 
           modifiedBy: Người sửa
           modifiedDate: Ngày sửa 
    Test:
        CALL Proc_Position_Edit("11123110-7667-44cf-8cd7-89b22d10517e", "Nhân viên", "2012-04-19 13:08:22", "Le Duc Tiep");
'
BEGIN

  UPDATE receipt r
  SET r.isRecorded = isRecorded
  WHERE r.receiptId = receiptId;

END;
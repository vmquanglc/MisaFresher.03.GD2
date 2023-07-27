DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_CustomerGroup_Edit;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_CustomerGroup_Edit (IN customerGroupId char(36),
IN name varchar(255),
IN note varchar(255),
IN modifiedDate datetime,
IN modifiedBy varchar(100))
COMMENT '
    Param: positionId: Id chức vụ
           positionName: Tên chức vụ 
           modifiedBy: Người sửa
           modifiedDate: Ngày sửa 
    Test:
        CALL Proc_CustomerGroup_Edit("11123110-7667-44cf-8cd7-89b22d10517e", "Nhân viên", "2012-04-19 13:08:22", "Le Duc Tiep");
'
BEGIN

  UPDATE customergroup c
  SET c.name = name,
      c.CustomerGroupCode = CustomerGroupCode,
      c.note = note,
      c.modifiedDate = modifiedDate,
      c.modifiedBy = modifiedBy
  WHERE c.customerGroupId = customerGroupId;

END;
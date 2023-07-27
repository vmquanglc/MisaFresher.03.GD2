CREATE OR REPLACE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_BankAccount_Edit (IN bankAccountId char(36),
IN customerId char(36),
IN bankAccountNumber varchar(25),
IN name varchar(50),
IN branch varchar(255),
IN bankCity varchar(255),
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

  UPDATE BankAccount b
  SET b.customerId = customerId,
      b.bankAccountNumber = bankAccountNumber,
      b.name = name,
      b.branch = branch,
      b.bankCity = bankCity,
      b.modifiedDate = modifiedDate,
      b.modifiedBy = modifiedBy
  WHERE b.bankAccountId = bankAccountId;

END;
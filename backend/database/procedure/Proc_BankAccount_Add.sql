CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_BankAccount_Add (IN bankAccountId char(36),
IN customerId char(36),
IN bankAccountNumber varchar(25),
IN name varchar(50),
IN branch varchar(255),
IN bankCity varchar(255),
IN createdDate datetime,
IN createdBy varchar(100))
COMMENT '
  Param: positionId: Mã chức vụ
         positionName: Tên chức vụ
         createdBy: Người tạo,
         createdDate: Ngày tạo
  Author: LeDucTiep (16/05/2023)
  Test: 
        CALL Proc_BankAccount_Add(UUID(), "Intern", "2012-04-19 13:08:22","Le Duc Tiep");
'
BEGIN

  INSERT INTO BankAccount (bankAccountId,
  customerId,
  bankAccountNumber,
  name,
  branch,
  bankCity,
  createdDate,
  createdBy)
    VALUES (bankAccountId, customerId, bankAccountNumber, name, branch, bankCity, createdDate, createdBy);

END
;

DROP PROCEDURE IF EXISTS `misa.web202303_mf1609_leductiep`.Proc_Employee_Edit;

CREATE PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Employee_Edit (IN employeeId char(36),
IN employeeCode varchar(20),
IN fullName varchar(100),
IN gender int,
IN dateOfBirth datetime,
IN email varchar(50),
IN address varchar(255),
IN phoneNumber varchar(50),
IN identityNumber varchar(25),
IN identityDate datetime,
IN identityPlace varchar(255),
IN positionId char(36),
IN departmentId char(36),
IN bankAccountNumber varchar(25),
IN nameOfBank varchar(255),
IN bankAccountBranch varchar(255),
IN modifiedDate datetime,
IN modifiedBy varchar(100))
COMMENT '
    Param: employeeId: Id của nhân viên cần thêm
           employeeCode: Mã nhân viên
           fullName: Họ và tên 
           gender: Giới tính (0: Nam, 1: Nữ, 2: Khác)
           dateOfBirth: Ngày sinh 
           email: mail
           address: Địa chỉ 
           phoneNumber: Số điện thoại
           identityNumber: số chứng minh nhân dân 
           identityDate: Ngày cấp chứng minh nhân dân 
           identityPlace: Nơi cấp chứng minh nhân dân 
           positionId: Mã chức vụ 
           departmentId: Mã phòng ban 
           bankAccountNumber: Tài khoản ngân hàng 
           nameOfBank: Tên ngân hàng 
           bankAccountBranch: Chi nhánh
           modifiedBy: Người sửa,
           modifiedDate: Ngày sửa 
    Test:
        CALL Proc_Employee_Edit("11123110-7667-44cf-8cd7-89b22d10517e", "NV43521", "Le Tiep", 0, "2012-04-19 13:08:22",
         "tiep@gmail.com", "B?c T? Liêm, Hà N?i", "0362111444","036202011212",
         "2012-04-19 13:08:22","C?c c?nh sát","35e773ea-5d44-2dda-26a8-6d513e380bde",
         "11452b0c-768e-5ff7-0d63-eeb1d8ed8cef", "", "", "", "2012-04-19 13:08:22","Le Duc Tiep");
'
BEGIN

  UPDATE employee e
  SET e.employeeCode = employeeCode,
      e.fullName = fullName,
      e.gender = gender,
      e.dateOfBirth = dateOfBirth,
      e.email = email,
      e.address = address,
      e.phoneNumber = phoneNumber,
      e.identityNumber = identityNumber,
      e.identityDate = identityDate,
      e.identityPlace = identityPlace,
      e.positionId = positionId,
      e.departmentId = departmentId,
      e.bankAccountNumber = bankAccountNumber,
      e.nameOfBank = nameOfBank,
      e.bankAccountBranch = bankAccountBranch,
      e.modifiedDate = modifiedDate,
      e.modifiedBy = modifiedBy
  WHERE e.employeeId = employeeId;

END;
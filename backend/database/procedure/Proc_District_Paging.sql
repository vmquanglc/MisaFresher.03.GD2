CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_District_Paging (IN cityId char(36),IN _offset int,
IN _limit int,
IN searchTerm varchar(255),
OUT totalRecord int)
COMMENT '
  Param: _limit: Số bản ghi cần lấy
         _offset: Số bản ghi bỏ qua
         searchTerm: FullName, số điện thoại hoặc EmployeeCode cần tìm
  Output: customer.*
          totalRecord: Tổng số bản ghi
  Author: LeDucTiep (06/05/2023)
  Test: CALL Proc_Employee_Paging(0, 10, "Đan", @totalRecord);
        SELECT
          @totalRecord;
'
BEGIN
  -- Lấy ra dữ liệu 
  SELECT
    d.DistrictId,
    d.Name,
    d.CityId

  FROM district d

  WHERE d.Name LIKE concat('%', searchTerm, '%')
  AND d.CityId LIKE concat('%', cityId, '%')

  ORDER BY d.Name

  LIMIT _limit

  OFFSET _offset;

  -- Lấy ra số lượng bản ghi

  SELECT
    COUNT(1) INTO totalRecord
  FROM district d
  WHERE d.Name LIKE concat('%', searchTerm, '%')
  AND d.CityId LIKE concat('%', cityId, '%')
  ;
END;


CALL Proc_District_Paging('14bf4313-486c-46be-d7a7-99e7765afd72', 1, 20, '', @totalRecord);

SELECT
  @totalRecord;
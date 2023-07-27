CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_City_Paging (IN nationId char(36),IN _offset int,
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
    c.CityId,
    c.Name,
    c.NationId
  FROM city c

  WHERE c.Name LIKE concat('%', searchTerm, '%')
  AND c.NationId LIKE concat('%', nationId, '%')

  ORDER BY c.Name

  LIMIT _limit

  OFFSET _offset;

  -- Lấy ra số lượng bản ghi

  SELECT
    COUNT(1) INTO totalRecord
  FROM city c
  WHERE c.Name LIKE concat('%', searchTerm, '%')
  AND c.NationId LIKE concat('%', nationId, '%')
  ;
END;


CALL Proc_City_Paging('1371fd31-2358-5e47-1a23-282384b72b34', 1, 20, '', @totalRecord);

SELECT
  @totalRecord;
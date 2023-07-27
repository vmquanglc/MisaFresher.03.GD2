CREATE OR REPLACE DEFINER = 'root'@'localhost'
PROCEDURE `misa.web202303_mf1609_leductiep`.Proc_Commune_Paging (IN districtId char(36),IN _offset int,
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
    c.CommuneId,
    c.Name,
    c.DistrictId

  FROM commune c

  WHERE c.Name LIKE concat('%', searchTerm, '%')
  AND c.DistrictId LIKE concat('%', districtId, '%')

  ORDER BY c.Name

  LIMIT _limit

  OFFSET _offset;

  -- Lấy ra số lượng bản ghi

  SELECT
    COUNT(1) INTO totalRecord

  FROM commune c

  WHERE c.Name LIKE concat('%', searchTerm, '%')
  AND c.DistrictId LIKE concat('%', districtId, '%')
  ;
END;


CALL Proc_Commune_Paging('63cc2ce9-3251-3faf-5f40-c5efa1df0570', 1, 20, '', @totalRecord);

SELECT
  @totalRecord;
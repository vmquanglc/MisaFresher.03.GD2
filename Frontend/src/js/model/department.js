export class Department {
  departmentCode;
  departmentId;
  departmentName;

  /**
   * Hàm khởi tạo cho Department
   *
   * Author: Dũng (12/05/2023)
   */
  constructor(e) {
    this.departmentCode = e.departmentCode;
    this.departmentId = e.departmentId;
    this.departmentName = e.departmentName;
  }

  /**
   * Trả về Object với định dạng là Body của request
   *
   * Author: Dũng (12/05/2023)
   */
  convertToApiFormat() {
    let obj = {};
    return obj;
  }
}

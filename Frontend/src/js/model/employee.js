import $formatter from "../common/formater";

export class Employee {
  address;
  dateOfBirth;
  departmentCode;
  departmentId;
  departmentName;
  email;
  employeeCode;
  employeeId;
  employeeFullName;
  gender;
  identityDate;
  identityNumber;
  identityPlace;
  phoneNumber;
  landlineNumber;
  positionName;
  positionId;
  positionCode;
  bankAccount;
  bankName;
  bankBranch;

  /**
   * Hàm khởi tạo cho Employee
   *
   * Author: Dũng (11/05/2023)
   */
  constructor(e) {
    this.address = e.address ?? "";
    this.dateOfBirth = $formatter.changeFormat(e.dateOfBirth);
    this.departmentCode = e.departmentCode ?? "";
    this.departmentId = e.departmentId ?? "";
    this.departmentName = e.departmentName ?? "";
    this.email = e.email ?? "";
    this.employeeCode = e.employeeCode ?? "";
    this.employeeId = e.employeeId ?? "";
    this.employeeFullName = e.employeeFullName ?? "";
    this.gender = e.gender ?? -1;
    this.identityDate = $formatter.changeFormat(e.identityDate);
    this.identityNumber = e.identityNumber ?? "";
    this.identityPlace = e.identityPlace ?? "";
    this.phoneNumber = e.phoneNumber ?? "";
    this.landlineNumber = e.landlineNumber ?? "";
    this.positionName = e.positionName ?? "";
    this.positionId = e.positionId ?? "";
    this.positionCode = e.positionCode ?? "";
    this.bankAccount = e.bankAccount ?? "";
    this.bankName = e.bankName ?? "";
    this.bankBranch = e.bankBranch ?? "";
  }

  /**
   * Copy dữ liệu từ employee khác vào object hiện tại
   *
   * Author: Dũng (04/06/2023)
   */
  cloneFromOtherEmployee(e) {
    this.address = e.address ?? "";
    this.dateOfBirth = e.dateOfBirth ?? "";
    this.departmentCode = e.departmentCode ?? "";
    this.departmentId = e.departmentId ?? "";
    this.departmentName = e.departmentName ?? "";
    this.email = e.email ?? "";
    this.employeeCode = e.employeeCode ?? "";
    this.employeeId = e.employeeId ?? "";
    this.employeeFullName = e.employeeFullName ?? "";
    this.gender = e.gender ?? -1;
    this.identityDate = e.identityDate ?? "";
    this.identityNumber = e.identityNumber ?? "";
    this.identityPlace = e.identityPlace ?? "";
    this.phoneNumber = e.phoneNumber ?? "";
    this.landlineNumber = e.landlineNumber ?? "";
    this.positionName = e.positionName ?? "";
    this.positionId = e.positionId ?? "";
    this.positionCode = e.positionCode ?? "";
    this.bankAccount = e.bankAccount ?? "";
    this.bankName = e.bankName ?? "";
    this.bankBranch = e.bankBranch ?? "";
  }

  /**
   * Trả về Object với định dạng là Body của request
   * @param {Boolean} isEditApi là api sửa thông tin nhân viên hay không
   *
   * Author: Dũng (11/05/2023)
   */
  convertToApiFormat() {
    let obj = {
      employeeCode: this.employeeCode,
      employeeFullName: this.employeeFullName,
      departmentId: this.departmentId,
      departmentName: this.departmentName,
      positionName: this.positionName,
      dateOfBirth: $formatter.formatDateToApiDate(this.dateOfBirth),
      gender: this.gender != -1 ? this.gender : null,
      identityNumber: this.identityNumber,
      identityDate: $formatter.formatDateToApiDate(this.identityDate),
      identityPlace: this.identityPlace,
      address: this.address,
      bankAccount: this.bankAccount,
      bankBranch: this.bankBranch,
      bankName: this.bankName,
      phoneNumber: this.phoneNumber,
      landlineNumber: this.landlineNumber,
      email: this.email,
    };
    return obj;
  }
}

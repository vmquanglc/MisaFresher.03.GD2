const DateFormater = {
  /**
   * Chuyển đổi chuỗi ngày tháng từ của api sang hiển thị table (2023-05-23T00:00:00 -> dd/mm/yyyy)
   * @param {Day of bird} dob chuỗi ngày tháng dạng 2023-05-23T00:00:00
   * @return {Day of bird} Chuỗi ngày tháng dạng dd/mm/yyyy
   * Author: LeDucTiep (04/05/2023)
   */
  convertIsoToDDMMYYY(dob) {
    const pattern = /\d{1,2}\/\d{1,2}\/\d{4}/;
    if (pattern.test(dob)) {
      return dob;
    }
    let dateOfBirth = null;
    if (dob) {
      const dateTime = new Date(dob);
      dateOfBirth = `${dateTime.getDate().toString().padStart(2, "0")}/${(
        dateTime.getMonth() + 1
      )
        .toString()
        .padStart(2, "0")}/${dateTime
        .getFullYear()
        .toString()
        .padStart(4, "0")}`;
    }
    return dateOfBirth;
  },

  /**
   * Chuyển đổi chuỗi ngày tháng có dạng dd/mm/yyyy sang kiểu dữ liệu Date()
   * @param {*} dateString Chuỗi ngày tháng, ví dụ: 15/03/2023
   * @return Kiểu ngày tháng hoặc null nếu giá trị truyền vào không đúng định dạng
   */
  stringToDate(dateString) {
    try {
      const dt = dateString.split("/");
      return new Date(dt.slice(0, 3).reverse().join("-"));
    } catch (error) {
      return null;
    }
  },
};

export default DateFormater;

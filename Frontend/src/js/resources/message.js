const $message = {
  employeeDeleteConfirm: (code) => `Bạn có muốn xóa nhân viên <${code}>?`,
  customerDeleteConfirm: (code) => `Bạn có muốn xóa khách hàng <${code}>?`,
  employeeDeleted: "Nhân viên đã bị xóa khỏi hệ thống",
  customerDeleted: "Khách hàng đã bị xóa khỏi hệ thống",
  employeeMultipleDeleteConfirm: (amount) =>
    `Bạn có chắc chắn muốn xóa ${amount} nhân viên?`,
  customerMultipleDeleteConfirm: (amount) =>
    `Bạn có chắc chắn muốn xóa ${amount} khách hàng?`,
  employeeMultipeDeleted: (amount) => `Xóa thành công ${amount} nhân viên`,
  customerMultipeDeleted: (amount) => `Xóa thành công ${amount} khách hàng`,
  employeeCreated: "Thêm mới nhân viên thành công",
  customerCreated: "Thêm mới khách hàng thành công",
  employeeUpdated: "Sửa thông tin nhân viên thành công",
  customerUpdated: "Sửa thông tin khách hàng thành công",
};
export default $message;

const cat_account = {
  pageTitle: "Hệ thống tài khoản",
  button: {
    addAccount: "Thêm mới tài khoản",
    goBack: "Tất cả danh mục",
    reload: "Tải lại dữ liệu",
    export: "Xuất dữ liệu",
  },
  form: {
    title: {
      edit: "Sửa tài khoản",
      add: "Thêm tài khoản",
    },
  },
  text: {
    searchAccount: "Tìm kiếm tài khoản",
    expand: "Mở rộng",
    collapse: "Thu gọn",
    detailBy: "Theo dõi chi tiết theo",
  },
  label: {
    accNumber: "Số tài khoản",
    accName: "Tên tài khoản",
    accNameVi: "Tên tiếng Anh",
    parentNumber: "Tài khoản tổng hợp",
    categoryKind: "Tính chất",
    description: "Diễn giải",
    foreignCurrency: "Có hạch toán ngoại tệ",
    object: "Đối tượng",
    bankAcc: "Tài khoản ngân hàng",
    objectCollectionCost: "Đối tượng THCP",
    construction: "Công trình",
    order: "Đơn đặt hàng",
    sellContract: "Hợp đồng bán",
    buyContract: "Hợp đồng mua",
    expenseItem: "Khoản mục CP",
    unit: "Đơn vị",
    statisticCode: "Mã thống kê",
  },
  tooltip: {
    closeBtn: "Đóng (ESC)",
    objectCollectionCost: "Đối tượng tập hợp chi phí",
    expenseItem: "Khoản mục chi phí",
  },
  message: {
    confirmDeleteTitle: "Xác nhận xóa tài khoản",
    confirmDeleteMessage: (accNameVi) =>
      `Bạn có muốn xóa tài khoản ${accNameVi}`,
    deleteSuccess: "Tài khoản đã bị xóa",
    deleteParentWarnning:
      "Không thể xóa danh mục cha nếu chưa xóa danh mục con",
    createSuccess: "Tạo mới tài khoản thành công",
    updateSuccess: "Cập nhật tài khoản thành công",
  },
  tableStructure: {
    accNumber: "SỐ TÀI KHOẢN",
    accNameVi: "TÊN TÀI KHOẢN",
    accNameEn: "TÊN TIẾNG ANH",
    categoryKind: "TÍNH CHẤT",
    description: "DIỄN GIẢI",
    usingStatus: "TRẠNG THÁI",
  },
  accountObjectOption: {
    provider: "Nhà cung cấp",
    customer: "Khách hàng",
    employee: "Nhân viên",
  },
  categoryKind: {
    debitBalance: "Dư nợ",
    creditBalance: "Dư có",
    biAccount: "Lưỡng tính",
    noBalance: "Không có số dư",
  },
};
export default cat_account;

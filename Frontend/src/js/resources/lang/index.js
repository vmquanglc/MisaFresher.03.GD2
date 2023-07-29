import category from "./category-page";
import cat_customer from "./category-customer";
import cat_account from "./category-account";
import cash_page from "./cash-page";
import cash_process from "./cash-process";
import cash_receipt from "./cash-receipt";
import lang_common from "./lang-common";

const lang = {
  cat_customer: cat_customer,
  cat_account: cat_account,
  cash_page: cash_page,
  cash_process: cash_process,
  cash_receipt: cash_receipt,
  common: lang_common,
  form: {
    title: {
      add: "Thêm mới nhân viên",
      edit: "Sửa thông tin nhân viên",
    },
  },
  checkboxLabel: {
    isCustomer: "Là khách hàng",
    isProvider: "Là nhà cung cấp",
  },
  textfield: {
    empCode: {
      label: "Mã",
      autofillMessage: "Shift + F8 để tự tạo mã",
    },
    empName: {
      label: "Tên",
    },
    empPos: {
      label: "Chức danh",
    },
    empIdentity: {
      label: "Số CMND",
      tooltip: "Số chứng minh nhân dân",
    },
    empIdentityPlace: {
      label: "Nơi cấp",
    },
    empAddress: {
      label: "Địa chỉ",
    },
    empPhone: {
      label: "ĐT di động",
      tooltip: "Số điện thoại di động",
    },
    empLandline: {
      label: "ĐT cố định",
      tooltip: "Số điện thoại cố định",
    },
    empEmail: {
      label: "Email",
    },
    empBankAcc: {
      label: "Tài khoản ngân hàng",
    },
    empBankName: {
      label: "Tên ngân hàng",
    },
    empBankBranch: {
      label: "Chi nhánh",
    },
    searchBar: {
      pholder: "Tìm kiếm nhân viên",
    },
  },
  combobox: {
    notFound: "Không có dữ liệu",
    empDepart: {
      label: "Đơn vị",
    },
  },
  radioGroup: {
    empGender: {
      label: "Giới tính",
      male: "Nam",
      female: "Nữ",
      other: "Khác",
    },
  },
  datepicker: {
    thead: {
      mon: "T2",
      tue: "T3",
      wed: "T4",
      thu: "T5",
      fri: "T6",
      sat: "T7",
      sun: "CN",
    },
    monthText: "Thg",
    todayBtn: "Hôm nay",
    cancelBtn: "Hủy bỏ",
    empIdentityDate: {
      label: "Ngày cấp",
    },
    empDob: {
      label: "Ngày sinh",
    },
  },
  button: {
    agree: "Đồng ý",
    cancel: "Hủy",
    save: "Cất",
    saveAndAdd: "Cất và Thêm",
    addEmployee: "Thêm mới nhân viên",
    batchOperaton: "Thực hiện hàng loạt",
    reload: "Tải lại dữ liệu",
    export: "Xuất dữ liệu",
    cancelSelect: "Bỏ chọn",
    batchDelete: "Xóa hàng loạt",
    addLine: "Thêm dòng",
    deleteAllLine: "Xóa hết dòng",
  },
  employeeList: {
    title: "Nhân viên",
  },
  table_items: {
    edit: "Sửa",
    batch: "Gộp",
    delete: "Xóa",
    dupplicate: "Nhân bản",
    stop: "Ngừng sử dụng",
    startUsing: "Sử dụng",
    view: "Xem",
  },
  tableHeader: {
    empCode: "MÃ NHÂN VIÊN",
    empName: "TÊN NHÂN VIÊN",
    empGender: "GIỚI TÍNH",
    empDob: "NGÀY SINH",
    empIdentity: "SỐ CMND",
    empIdentityTooltip: "Số chứng minh nhân dân",
    empPos: "CHỨC DANH",
    empDepart: "TÊN ĐƠN VỊ",
    empBankAcc: "SỐ TÀI KHOẢN",
    empBankName: "TÊN NGÂN HÀNG",
    empBankBranch: "CHI NHÁNH TK NGÂN HÀNG",
    empBankBranchTooltip: "Chi nhánh tài khoản ngân hàng",
    tool: "CHỨC NĂNG",
  },
  tablePag: {
    total: {
      left: "Tổng số:",
      right: "bản ghi",
    },
    recordAmount: "bản ghi trên 1 trang",
    recordPerPage: "Số bản ghi / trang: ",
    record: "bản ghi",
  },
  tableNoti: {
    dataNotFound: "Không có dữ liệu",
  },
  gender: {
    male: "Nam",
    female: "Nữ",
    other: "Khác",
  },
  tooltip: {
    saveAndAdd: "Cất và thêm (Ctr + Shift + S)",
    save: "Cất (Ctr + S)",
    closeForm: "Đóng (ESC)",
    help: "Giúp",
  },
  dataNotFound: "Không có dữ liệu",
  category: category,
};
export default lang;

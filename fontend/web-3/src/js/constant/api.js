// const BaseApi = "https://cukcuk.manhnv.net/api/v1";
const BaseApi = "https://localhost:44356/api/v1";
const BaseExportApi = "https://localhost:7191/api/v1";

const MSApi = {
  EmployeeApi: {
    Post: `${BaseApi}/Employees`,
    Get: (id) => `${BaseApi}/Employees/${id}`,
    Put: (id) => `${BaseApi}/Employees/${id}`,
    Delete: (id) => `${BaseApi}/Employees/${id}`,
    DeleteMany: `${BaseApi}/Employees`,
    GetNewCode: `${BaseApi}/Employees/new-employee-code`,
    CheckCodeDuplicated: `${BaseApi}/Employees/is-code-duplicated`,
    CheckEditCodeDuplicated: `${BaseApi}/Employees/is-edit-code-duplicated`,
    Paging: `${BaseApi}/Employees/paging`,
    ExportExcel: `${BaseExportApi}/Employees/exporting`,
    GetAll: `${BaseApi}/Employees/exporting-json`,
  },
  DepartmentApi: {
    ExportExcel: `${BaseExportApi}/Departments/exporting`,
    GetAll: `${BaseApi}/Departments`,
  },
  PositionApi: {
    ExportExcel: `${BaseExportApi}/Positions/exporting`,
    GetAll: `${BaseApi}/Positions`,
  },
  CustomerApi: {
    GetAll: `${BaseApi}/Customers`,
    Post: `${BaseApi}/Customers`,
    Get: (id) => `${BaseApi}/Customers/${id}`,
    Put: (id) => `${BaseApi}/Customers/${id}`,
    Delete: (id) => `${BaseApi}/Customers/${id}`,
    DeleteMany: `${BaseApi}/Customers`,
    Paging: `${BaseApi}/Customers/paging`,
    GetNewCode: `${BaseApi}/Customers/new-customer-code`,
    CheckCodeDuplicated: `${BaseApi}/Customers/is-code-duplicated`,
    CheckEditCodeDuplicated: `${BaseApi}/Customers/is-edit-code-duplicated`,
    ExportExcel: `${BaseExportApi}/Customers/exporting`,
  },
  CustomerGroupApi: {
    GetAll: `${BaseApi}/CustomerGroups`,
    ExportExcel: `${BaseExportApi}/CustomerGroups/exporting`,
  },
  TermsOfPaymentApi: {
    Get: (id) => `${BaseApi}/TermsOfPayments/${id}`,
    Paging: `${BaseApi}/TermsOfPayments/paging`,
    ExportExcel: `${BaseExportApi}/TermsOfPayments/exporting`,
  },
  AccountApi: {
    Post: `${BaseApi}/Accounts`,
    Get: (id) => `${BaseApi}/Accounts/${id}`,
    Put: (id) => `${BaseApi}/Accounts/${id}`,
    Delete: (id) => `${BaseApi}/Accounts/${id}`,
    DeleteMany: `${BaseApi}/Accounts`,
    GetNewCode: `${BaseApi}/Accounts/new-account-code`,
    CheckCodeDuplicated: `${BaseApi}/Accounts/is-code-duplicated`,
    CheckEditCodeDuplicated: `${BaseApi}/Accounts/is-edit-code-duplicated`,
    Paging: `${BaseApi}/Accounts/ms-paging`,
    ExportExcel: `${BaseExportApi}/Accounts/exporting`,
    GetAll: `${BaseApi}/Accounts`,
    PagingCombobox: `${BaseApi}/Accounts/paging`,
  },

  AccountPropertyApi: {
    GetAll: `${BaseApi}/AccountPropertys`,
    Get: (id) => `${BaseApi}/AccountPropertys/${id}`,
    Paging: `${BaseApi}/AccountPropertys/paging`,
    ExportExcel: `${BaseExportApi}/AccountPropertys/exporting`,
  },

  NationApi: {
    GetAll: `${BaseApi}/Nations`,
    Get: (id) => `${BaseApi}/Nations/${id}`,
    Paging: `${BaseApi}/Nations/paging`,
    ExportExcel: `${BaseExportApi}/Nations/exporting`,
  },

  CityApi: {
    GetAll: `${BaseApi}/Citys`,
    Get: (id) => `${BaseApi}/Citys/${id}`,
    Paging: `${BaseApi}/Citys/paging`,
    ExportExcel: `${BaseExportApi}/Citys/exporting`,
  },

  DistrictApi: {
    GetAll: `${BaseApi}/Districts`,
    Get: (id) => `${BaseApi}/Districts/${id}`,
    Paging: `${BaseApi}/Districts/paging`,
    ExportExcel: `${BaseExportApi}/Districts/exporting`,
  },

  CommuneApi: {
    GetAll: `${BaseApi}/Communes`,
    Get: (id) => `${BaseApi}/Communes/${id}`,
    Paging: `${BaseApi}/Communes/paging`,
    ExportExcel: `${BaseExportApi}/Communes/exporting`,
  },

  ReceiptApi: {
    Post: `${BaseApi}/Receipts`,
    Get: (id) => `${BaseApi}/Receipts/${id}`,
    Put: (id) => `${BaseApi}/Receipts/${id}`,
    Delete: (id) => `${BaseApi}/Receipts/${id}`,
    DeleteMany: `${BaseApi}/Receipts`,
    GetNewCode: `${BaseApi}/Receipts/new-receipt-code`,
    CheckCodeDuplicated: `${BaseApi}/Receipts/is-code-duplicated`,
    CheckEditCodeDuplicated: `${BaseApi}/Receipts/is-edit-code-duplicated`,
    Paging: `${BaseApi}/Receipts/paging`,
    ExportExcel: `${BaseExportApi}/Receipts/exporting`,
    GetAll: `${BaseApi}/Receipts`,
    PagingCombobox: `${BaseApi}/Receipts/paging`,
    SettingRecorded: `${BaseApi}/Receipts/setting-recorded`,
  },
  TaxCodeApi: {
    CheckExist: (taxCode) => `https://api.vietqr.io/v2/business/${taxCode}`,
  },
};

export default MSApi;

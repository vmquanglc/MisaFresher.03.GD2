import employeeApi from "./employee";
import departmentApi from "./department";
import customerApi from "./customer";
import groupApi from "./group";
import accountApi from "./account";
import receiptApi from "./receipt";
import locationApi from "./location";
const baseUrl = "https://localhost:44381/api/v1";
const $api = {
  employee: employeeApi(baseUrl),
  department: departmentApi(baseUrl),
  customer: customerApi(baseUrl),
  group: groupApi(baseUrl),
  account: accountApi(baseUrl),
  receipt: receiptApi(baseUrl),
  location: locationApi(baseUrl),
};
export default $api;

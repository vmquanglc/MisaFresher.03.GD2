import UnderDevelopment from "../components/views/under-development/UnderDevelopment";
import CategoryPage from "../components/views/category/CategoryPage";
import EmployeeList from "../components/views/category/employee/EmployeeList";
import EmployeeForm from "../components/views/category/employee/EmployeeForm";
import CustomerList from "../components/views/category/customer/CustomerList";
import CustomerForm from "../components/views/category/customer/CustomerForm";
import AccountForm from "../components/views/category/account/AccountForm";
import AccountList from "../components/views/category/account/AccountList";
import CashPage from "../components/views/cash/CashPage";
import CashProcess from "../components/views/cash/process/CashProcess";
import ReceiptList from "../components/views/cash/receipt/ReceiptList";
import ReceiptForm from "../components/views/cash/receipt/ReceiptForm";
import { createRouter, createWebHistory } from "vue-router";

const routes = [
  {
    path: "/DI",
    component: CategoryPage,
    children: [
      {
        path: "DIEmployee",
        component: EmployeeList,
        children: [
          {
            path: "create",
            components: {
              default: EmployeeList,
              EmployeeForm: EmployeeForm,
            },
          },
          {
            path: ":id",
            components: {
              default: EmployeeList,
              EmployeeForm: EmployeeForm,
            },
          },
        ],
      },
      {
        path: "DICustomer",
        component: CustomerList,
        children: [
          {
            path: "create",
            components: {
              default: CustomerList,
              CustomerForm: CustomerForm,
            },
          },
          {
            path: ":id",
            components: {
              default: CustomerList,
              CustomerForm: CustomerForm,
            },
          },
        ],
      },
      {
        path: "DIAccount",
        component: AccountList,
        children: [
          {
            path: "create",
            components: {
              default: AccountList,
              CustomerForm: AccountForm,
            },
          },
          {
            path: ":id",
            components: {
              default: AccountList,
              CustomerForm: AccountForm,
            },
          },
        ],
      },
    ],
  },
  { path: "/under-development/:id", component: UnderDevelopment },
  {
    path: "/CA",
    component: CashPage,
    children: [
      {
        path: "",
        redirect: "/CA/CAProcess",
      },
      {
        path: "CAProcess",
        component: CashProcess,
      },
      {
        path: "CAReceipt",
        component: ReceiptList,
        children: [
          {
            path: "create",
            components: {
              default: ReceiptList,
              ReceiptForm: ReceiptForm,
            },
          },
          {
            path: ":id",
            components: {
              default: ReceiptList,
              ReceiptForm: ReceiptForm,
            },
          },
        ],
      },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;

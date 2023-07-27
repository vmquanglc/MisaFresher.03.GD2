import { createRouter, createWebHistory } from "vue-router";

import EmployeeHome from "@/views/employee/EmployeeHome.vue";

import EmployeeRoute from "./employee.js";
import CashRoute from "./cash.js";
import DepositsRoute from "./deposits.js";
import PurchaseRoute from "./purchase.js";
import SellRoute from "./sell.js";
import BillManagementRoute from "./billmanagement.js";
import WarehouseRoute from "./warehouse.js";
import ServiceToolsRoute from "./servicetools.js";
import FixedAssetsRoute from "./fixedassets.js";
import TaxRoute from "./tax.js";
import PriceRoute from "./price.js";
import OverviewRoute from "./overview.js";
import SyntheticRoute from "./synthetic.js";
import BudgetRoute from "./budget.js";
import ReportRoute from "./report.js";
import FinancialAnalysisRoute from "./financialanalysis.js";
import AccountRoute from "./account.js";
import CategoryRoute from "./category.js";
import CustomerRoute from "./customer.js";

const routers = [
  { path: "/", component: EmployeeHome, name: "Home" },
  ...EmployeeRoute,
  ...CashRoute,
  ...DepositsRoute,
  ...PurchaseRoute,
  ...SellRoute,
  ...BillManagementRoute,
  ...WarehouseRoute,
  ...ServiceToolsRoute,
  ...FixedAssetsRoute,
  ...TaxRoute,
  ...PriceRoute,
  ...SyntheticRoute,
  ...BudgetRoute,
  ...ReportRoute,
  ...FinancialAnalysisRoute,
  ...AccountRoute,
  ...CategoryRoute,
  ...CustomerRoute,
  ...OverviewRoute,
];

// Khởi tạo router
const router = createRouter({
  history: createWebHistory(),
  routes: routers,
});

export default router;

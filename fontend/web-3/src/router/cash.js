import CashHome from "@/views/cash/CashHome.vue";
import CashList from "@/views/cash/ReceiptList.vue";
import CashBusinessProcess from "@/views/cash/CashBusinessProcess";
import CashCollectMoneyForm from "@/views/cash/CashCollectMoneyForm.vue";

const CashRoute = [
  {
    path: "/cash",
    component: CashHome,
    name: "CashHomeRouter",
    children: [
      {
        path: "business-process",
        components: {
          default: CashHome,
          CashRouterView: CashBusinessProcess,
        },
        name: "business-process",
      },
      {
        path: "create",
        components: {
          default: CashHome,
          CashRouterView: CashCollectMoneyForm,
        },
        name: "receipt-add",
      },
      {
        path: "edit/:id",
        components: {
          default: CashHome,
          CashRouterView: CashCollectMoneyForm,
        },
        name: "receipt-edit",
        props: true,
      },
      {
        path: "view/:id",
        components: {
          default: CashHome,
          CashRouterView: CashCollectMoneyForm,
        },
        name: "receipt-view",
        props: true,
      },
      {
        path: "receipt",
        components: {
          default: CashHome,
          CashRouterView: CashList,
        },
        name: "receipt-list",
        props: true,
      },
    ],
  },
];

export default CashRoute;

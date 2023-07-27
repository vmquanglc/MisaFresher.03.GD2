import AccountHome from "@/views/account/AccountHome.vue";
import AccountForm from "@/views/account/AccountForm.vue";

const AccountRoute = [
  {
    path: "/account",
    component: AccountHome,
    name: "AccountHomeRouter",
    children: [
      {
        path: "edit/:id",
        components: {
          default: AccountHome,
          AccountRouterView: AccountForm,
        },
        name: "AccountEditRouter",
        props: true,
      },
      {
        path: "view/:id",
        components: {
          default: AccountHome,
          AccountRouterView: AccountForm,
        },
        name: "AccountViewRouter",
        props: true,
      },
      {
        path: "create",
        components: {
          default: AccountHome,
          AccountRouterView: AccountForm,
        },
        name: "AccountCreateRouter",
      },
      {
        path: "create-child/:id",
        components: {
          default: AccountHome,
          AccountRouterView: AccountForm,
        },
        name: "AccountCreateChildRouter",
        props: true,
      },
    ],
  },
];

export default AccountRoute;

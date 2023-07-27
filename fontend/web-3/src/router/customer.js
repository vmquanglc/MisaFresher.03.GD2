import CustomerHome from "@/views/customer/CustomerHome.vue";
import CustomerForm from "@/views/customer/CustomerForm.vue";

const CustomerRoute = [
  {
    path: "/customer",
    component: CustomerHome,
    name: "CustomerHomeRouter",
    children: [
      {
        path: "edit/:id",
        components: {
          default: CustomerHome,
          CustomerRouterView: CustomerForm,
        },
        name: "CustomerEditRouter",
        props: true,
      },
      {
        path: "view/:id",
        components: {
          default: CustomerHome,
          CustomerRouterView: CustomerForm,
        },
        name: "CustomerViewRouter",
        props: true,
      },
      {
        path: "create",
        components: {
          default: CustomerHome,
          CustomerRouterView: CustomerForm,
        },
        name: "CustomerCreateRouter",
      },
    ],
  },
];

export default CustomerRoute;

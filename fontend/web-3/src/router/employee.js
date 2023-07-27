import EmployeeHome from "@/views/employee/EmployeeHome.vue";
import EmployeeForm from "@/views/employee/EmployeeForm.vue";

const EmployeeRoute = [
  {
    path: "/employee",
    component: EmployeeHome,
    name: "EmployeeHomeRouter",
    children: [
      {
        path: "edit/:id",
        components: {
          default: EmployeeHome,
          EmployeeRouterView: EmployeeForm,
        },
        name: "EmployeeEditRouter",
        props: true,
      },
      {
        path: "duplicate/:id",
        components: {
          default: EmployeeHome,
          EmployeeRouterView: EmployeeForm,
        },
        name: "EmployeeDuplicateRouter",
        props: true,
      },

      {
        path: "create",
        components: {
          default: EmployeeHome,
          EmployeeRouterView: EmployeeForm,
        },
        name: "EmployeeCreateRouter",
      },
    ],
  },
  {
    path: "/employee/pageSize=:pageSize/pageNumber=:pageNumber",
    components: {
      default: EmployeeHome,
      EmployeeRouterView: EmployeeHome,
    },
    name: "EmployeePagingRouter",
  },
];

export default EmployeeRoute;

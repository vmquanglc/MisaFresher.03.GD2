const employeeApi = (baseUrl) => {
  const controller = "/Employees";
  return {
    index: baseUrl + controller,
    newCode: baseUrl + controller + "/NewEmployeeCode",
    one: (id) => baseUrl + controller + `/${id}`,
    filter: baseUrl + controller + "/Filter",
    deleteMultiple: baseUrl + controller + "/DeleteMultiple",
    checkCodeExist: baseUrl + controller + "/CheckCodeExist",
    exportExcel: baseUrl + controller + "/ExportEmployeesData",
  };
};

export default employeeApi;

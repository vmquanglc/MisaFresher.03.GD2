const customerApi = (baseUrl) => {
  const controller = "/Customers";
  return {
    index: baseUrl + controller,
    filter: baseUrl + controller + "/Filter",
    one: (id) => baseUrl + controller + `/${id}`,
    deleteMultiple: baseUrl + controller + "/DeleteMultiple",
    newCode: baseUrl + controller + "/NewCustomerCode",
    checkCodeExist: baseUrl + controller + "/CheckCodeExist",
    exportExcel: baseUrl + controller + "/ExportData",
  };
};

export default customerApi;

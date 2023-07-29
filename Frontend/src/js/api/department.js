const departmentApi = (baseUrl) => {
  const controller = "/Departments";
  return {
    index: baseUrl + controller,
    filter: baseUrl + controller + "/Filter",
  };
};

export default departmentApi;

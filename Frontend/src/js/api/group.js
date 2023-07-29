const groupApi = (baseUrl) => {
  const controller = "/Groups";
  return {
    filter: baseUrl + controller + "/Filter",
  };
};

export default groupApi;

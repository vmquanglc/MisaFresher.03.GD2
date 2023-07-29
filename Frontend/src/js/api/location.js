const locationApi = (baseUrl) => {
  const controller = "/Locations";
  return {
    province: (id) => baseUrl + controller + `/country/${id}`,
    district: (id) => baseUrl + controller + `/province/${id}`,
    ward: (id) => baseUrl + controller + `/district/${id}`,
  };
};

export default locationApi;

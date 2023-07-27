import axios from "axios";
import processError from "./axiosError";

const msAxios = async (method, api, data) => {
  let response = null;
  try {
    switch (method.toLowerCase()) {
      case "post":
        response = await axios.post(api, data);
        break;
      case "get":
        response = await axios.get(api, data);
        break;
      case "put":
        response = await axios.put(api, data);
        break;
      case "delete":
        response = await axios.delete(api, data);
        break;
    }
  } catch (error) {
    // Xử lý lỗi
    try {
      processError(error.response);
    } catch (error) {
      console.log("processError: ", error);
    }
  }
  return response;
};

export default msAxios;

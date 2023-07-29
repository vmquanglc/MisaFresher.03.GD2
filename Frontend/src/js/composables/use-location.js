import { ref, watch } from "vue";
import api from "../api/index";
const axios = require("axios");

export function useLocation(customer) {
  const countryList = ref([]);
  const provinceList = ref([]);
  const districtList = ref([]);
  const wardList = ref([]);
  const provinceCache = [];
  const districtCache = [];
  const wardCache = [];

  countryList.value = [
    {
      countryId: "1",
      name: "Việt Nam",
    },
  ];

  watch(
    () => customer.value.countryId,
    async (newId) => {
      if (newId == "undefined" || newId == "null") return;
      if (provinceCache[newId] == null) {
        const response = await axios.get(api.location.province(1));
        provinceCache[newId] = response.data;
      }
      provinceList.value = provinceCache[newId];
    }
  );
  watch(
    () => customer.value.provinceOrCityId,
    async (newId) => {
      if (newId == "undefined" || newId == "null") return;
      if (districtCache[newId] == null && newId != null && newId.length > 0) {
        const response = await axios.get(api.location.district(newId));
        districtCache[newId] = response.data;
      }
      districtList.value = districtCache[newId];
    }
  );
  watch(
    () => customer.value.districtId,
    async (newId) => {
      if (newId == "undefined" || newId == "null") return;
      if (wardCache[newId] == null && newId != null && newId.length > 0) {
        const response = await axios.get(api.location.ward(newId));
        wardCache[newId] = response.data;
      }
      wardList.value = wardCache[newId];
    }
  );
  return {
    countryList,
    provinceList,
    districtList,
    wardList,
  };
}

<template>
  <div class="scb">
    <div class="scb__label">Nhân viên bán hàng</div>
    <div class="scb__main" :class="[isTableOpen ? 'active' : '']">
      <div class="main__selected">
        <div class="selected__input">
          <input
            :disabled="blocked"
            type="text"
            ref="refInput"
            :value="selectedEmployeeName"
            @input="$emit('update:selectedEmployeeName', $event.target.value)"
            @keyup="inputKeyupHandler"
            @keypress="inputKeyPressHandler"
          />
        </div>
      </div>
      <div class="main__action">
        <div class="btn__add">
          <div class="btn__add__icon mi mi-12 mi-add--green"></div>
        </div>
        <button
          class="btn__dropdown"
          @click="btnDropDownOnClick"
          :disabled="blocked"
        >
          <i class="fas fa-chevron-down"></i>
        </button>
      </div>
    </div>
    <div class="scb__table" v-show="isTableOpen">
      <div class="table__header">
        <table class="t__header">
          <thead>
            <tr>
              <th
                v-for="header in tableStructure"
                :key="header.name"
                :style="{
                  width: header.width != 0 ? header.width + 'px' : 'auto',
                }"
              >
                <div :class="header.align">
                  {{ header.name }}
                </div>
              </th>
            </tr>
          </thead>
        </table>
      </div>
      <div
        class="table__content"
        @scroll="tableContentOnScroll($event)"
        ref="talbeContentRef"
      >
        <div class="t__loader" v-show="isLoadingData">
          <BaseLoader />
        </div>
        <table class="t__content" v-show="!isLoadingData">
          <tbody>
            <tr
              v-for="(entity, index) in entityList"
              :key="index"
              @click="trOnClick(index)"
              :class="[
                entity[entityStructure.id] == selectedEmployeeId
                  ? 'selected'
                  : '',
              ]"
            >
              <td
                v-for="header in tableStructure"
                :key="header.prop"
                :style="{
                  width: header.width != 0 ? header.width + 'px' : 'auto',
                }"
              >
                <div class="td__text">{{ entity[header.prop] }}</div>
              </td>
            </tr>
            <tr v-show="entityList.length == 0">
              <div class="no__data">{{ lang.dataNotFound }}</div>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, inject, nextTick } from "vue";
import $api from "@/js/api";
const refInput = ref(null);
const isTableOpen = ref(false);
const $axios = inject("$axios");
const lang = inject("$lang");
const entityList = ref([]);
const typingTimers = [];
const timeoutVal = 500;
const isLoadingData = ref(false);
const talbeContentRef = ref(null);
const selectedEmployeeIndex = ref(0);

const props = defineProps({
  blocked: Boolean,
  selectedEmployeeId: String,
  selectedEmployeeName: String,
  tableStructure: Array,
});
const emits = defineEmits([
  "update:selectedEmployeeId",
  "update:selectedEmployeeName",
]);

var totalRecord = 0;

const entityStructure = {
  id: "employeeId",
  code: "employeeCode",
  name: "employeeFullName",
};

async function fetchNewEmployee(skip, take, keySearch, reload) {
  const response = await $axios.get($api.employee.filter, {
    params: {
      skip: skip,
      take: take,
      keySearch: keySearch,
    },
  });
  if (reload) entityList.value = [];
  for (const entity of response.data.filteredList) {
    entityList.value.push(entity);
  }
  totalRecord = response.data.totalRecord;
}

async function tableContentOnScroll(e) {
  let {
    target: { scrollTop, clientHeight, scrollHeight },
  } = e;
  if (scrollTop + clientHeight >= scrollHeight) {
    if (entityList.value.length < totalRecord) {
      await fetchNewEmployee(
        entityList.value.length,
        10,
        props.selectedEmployeeName,
        false
      );
    }
  }
}

async function btnDropDownOnClick() {
  if (!isTableOpen.value && entityList.value.length == 0) {
    await fetchNewEmployee(
      entityList.value.length,
      10,
      props.selectedEmployeeName,
      false
    );
  }
  isTableOpen.value = !isTableOpen.value;
  if (isTableOpen.value && props.selectedEmployeeId != "") {
    await nextTick();
    talbeContentRef.value.scrollTo({
      top: 38 * selectedEmployeeIndex.value,
      behavior: "smooth",
    });
  }
}

function inputKeyupHandler($event) {
  if (!isNormalCharacterKey($event.key)) return;

  // 500ms sau khi Typing tự động mở optionBox
  if (isTableOpen.value == false && $event.key != "Tab") {
    setTimeout(() => {
      isTableOpen.value = true;
    }, 500);
  }
  // Xóa các timeout trước trong khi typing
  while (typingTimers.length > 0) {
    clearTimeout(typingTimers[0]);
    typingTimers.splice(0, 1);
  }

  typingTimers.push(
    setTimeout(() => {
      // Display loading
      isLoadingData.value = true;
      emits("update:selectedEmployeeId", "");
      fetchNewEmployee(0, 20, props.selectedEmployeeName, true);
      isLoadingData.value = false;
    }, timeoutVal)
  );
}

/**
 * Sự kiện Typing vào ô input
 * Author: Dũng (08/05/2023)
 */
function inputKeyPressHandler($event) {
  if (!isNormalCharacterKey($event.key)) return;
  isLoadingData.value = true;
  // Xóa setTimeout đã tạo từ tước
  while (typingTimers.length > 0) {
    clearTimeout(typingTimers[0]);
    typingTimers.splice(0, 1);
  }
}

//#region function
/**
 * Kiểm tra keypress có là ký tự text bình thường không
 * @param {String} key là $event.key
 *
 * Author: Dũng(12/05/2023)
 */
function isNormalCharacterKey(key) {
  return key.length == 1 || key == "Backspace" ? true : false;
}

function trOnClick(index) {
  emits(
    "update:selectedEmployeeName",
    entityList.value[index][entityStructure.name]
  );
  emits(
    "update:selectedEmployeeId",
    entityList.value[index][entityStructure.id]
  );
  selectedEmployeeIndex.value = index;
  isTableOpen.value = false;
}
</script>

<style
  scoped
  lang="css"
  src="../../../../css/components/views/category/customer/employee-combobox.css"
></style>

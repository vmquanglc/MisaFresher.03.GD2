<template>
  <div class="scb">
    <div class="scb__label">Mã khách hàng</div>
    <div class="scb__main" :class="[isTableOpen ? 'active' : '']">
      <div class="main__selected">
        <div class="selected__input">
          <input
            :disabled="blocked"
            type="text"
            ref="refInput"
            :value="selectedEntityCode"
            @input="$emit('update:selectedEntityCode', $event.target.value)"
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
                entity[entityStructure.id] == selectedEntityId
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
const selectedEntityIndex = ref(0);
var totalRecord = 0;

const props = defineProps({
  blocked: Boolean,
  selectedEntityId: String,
  selectedEntityCode: String,
  selectedEntityName: String,
  selectedEntityContact: String,
  selectedEntityAddress: String,
});
const emits = defineEmits([
  "update:selectedEntityId",
  "update:selectedEntityCode",
  "update:selectedEntityName",
  "update:selectedEntityContact",
  "update:selectedEntityAddress",
]);

const entityStructure = {
  id: "customerId",
  code: "customerCode",
  name: "customerFullName",
  address: "address",
  contact: "contactName",
};

const tableStructure = [
  {
    name: "Đối tượng",
    prop: "customerCode",
    align: "text-left",
    width: 100,
  },
  {
    name: "Tên đối tượng",
    prop: "customerFullName",
    align: "text-left",
    width: 300,
  },
  {
    name: "Mã số thuế",
    prop: "customerTIN",
    align: "text-left",
    width: 200,
  },
  {
    name: "Địa chỉ",
    prop: "address",
    align: "text-left",
    width: 180,
  },
  {
    name: "Điện thoại",
    prop: "phoneNumber",
    align: "text-left",
    width: 180,
  },
];

async function fetchNewEntity(skip, take, keySearch, reload) {
  const response = await $axios.get($api.customer.filter, {
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
      await fetchNewEntity(
        entityList.value.length,
        10,
        props.selectedEntityCode,
        false
      );
    }
  }
}

async function btnDropDownOnClick() {
  if (!isTableOpen.value && entityList.value.length == 0) {
    await fetchNewEntity(
      entityList.value.length,
      10,
      props.selectedEntityCode,
      false
    );
  }
  isTableOpen.value = !isTableOpen.value;
  if (isTableOpen.value && props.selectedEntityId != "") {
    await nextTick();
    talbeContentRef.value.scrollTo({
      top: 38 * selectedEntityIndex.value,
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
      emits("update:selectedEntityId", "");
      fetchNewEntity(0, 20, props.selectedEntityCode, true);
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
    "update:selectedEntityCode",
    entityList.value[index][entityStructure.code]
  );
  emits(
    "update:selectedEntityName",
    entityList.value[index][entityStructure.name]
  );
  emits(
    "update:selectedEntityAddress",
    entityList.value[index][entityStructure.address]
  );
  // console.log(entityList.value[index][entityStructure.address]);
  emits(
    "update:selectedEntityContact",
    entityList.value[index][entityStructure.contact]
  );
  emits("update:selectedEntityId", entityList.value[index][entityStructure.id]);
  selectedEntityIndex.value = index;
  isTableOpen.value = false;
}
</script>

<style
  scoped
  lang="css"
  src="../../../../css/components/views/cash/receipt/customer-combobox.css"
></style>

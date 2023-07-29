<template>
  <div class="tablebox">
    <table class="m-table">
      <thead>
        <tr>
          <th
            class="th1--sticky"
            :style="{
              minWidth: tableStructure.checkboxWidth + 'px',
              maxWidth: tableStructure.checkboxWidth + 'px',
            }"
          >
            <div class="align-center">
              <div
                class="t__checkbox mi-24"
                :class="[selectedAmountInPage > 0 ? 'selected' : '']"
                @click="thCheckboxOnClick"
              >
                <i
                  :class="[
                    selectedAmountInPage == rowList.length
                      ? 'fas fa-check'
                      : 'fas fa-minus',
                  ]"
                ></i>
              </div>
            </div>
          </th>
          <th
            v-for="header in tableStructure.headerList"
            :key="header.name"
            :style="{
              minWidth: header.width != 0 ? header.width + 'px' : 'auto',
              maxWidth: header.width != 0 ? header.width + 'px' : 'auto',
            }"
          >
            <div :class="header.align">
              {{ header.name }}
            </div>
          </th>
          <th class="thn--sticky w-120">
            <div class="align-center">{{ lang.tableHeader.tool }}</div>
          </th>
        </tr>
      </thead>
      <tbody>
        <template v-if="isLoadingData">
          <tr v-for="i in 12" :key="i">
            <td
              class="td1--sticky"
              :style="{
                minWidth: tableStructure.checkboxWidth + 'px',
                maxWidth: tableStructure.checkboxWidth + 'px',
              }"
            >
              <div class="loading-item"></div>
            </td>
            <td
              v-for="header in tableStructure.headerList"
              :key="header.prop"
              :style="{
                minWidth: header.width != 0 ? header.width + 'px' : 'auto',
                maxWidth: header.width != 0 ? header.width + 'px' : 'auto',
              }"
            >
              <div class="loading-item"></div>
            </td>
            <td class="tdn--sticky w-120">
              <div class="loading-item"></div>
            </td>
          </tr>
        </template>
        <template v-else>
          <tr
            v-for="({ active, selected, entity } = row, index) in rowList"
            :key="entity.receiptId"
            :class="{
              active: active,
            }"
            :style="{ color: entity.ledgerStatus ? 'black' : '#14a200' }"
            @click="trOnClick(index)"
            @dblclick="trOnDblclick(entity.receiptId)"
          >
            <td
              class="td1--sticky"
              @dblclick.stop
              :style="{
                minWidth: tableStructure.checkboxWidth + 'px',
                maxWidth: tableStructure.checkboxWidth + 'px',
              }"
            >
              <div class="align-center">
                <div
                  class="t__checkbox mi-24"
                  :class="{
                    selected: selected,
                  }"
                  @click.stop="checkBoxOnClick(index)"
                >
                  <i class="fas fa-check"></i>
                </div>
              </div>
            </td>
            <td
              v-for="header in tableStructure.headerList"
              :key="header.name + entity.receiptId"
              :style="{
                minWidth: header.width != 0 ? header.width + 'px' : 'auto',
                maxWidth: header.width != 0 ? header.width + 'px' : 'auto',
              }"
            >
              <div :class="header.align" v-tooltip="entity[header.prop]">
                {{ header.formatFunction(entity[header.prop]) }}
              </div>
            </td>
            <td
              :class="[table.expandEntityId == entity.receiptId ? 'above' : '']"
              class="tdn--sticky w-120"
              @dblclick.stop
            >
              <div class="t__optionbox align-center">
                <button
                  class="option__edit"
                  @click="btnViewOnClick(entity.receiptId)"
                >
                  {{ lang.table_items.view }}
                </button>
                <button
                  class="btn__expand mi mi-16 mi-expand-down"
                  @click="btnExpandOnClick(entity.receiptId)"
                ></button>
                <ul
                  class="actions-list btn__expand"
                  :class="
                    (entity.receiptId ==
                      rowList[rowList.length - 1].entity.receiptId ||
                      entity.receiptId ==
                        rowList[rowList.length - 2].entity.receiptId) &&
                    rowList.length > 6
                      ? 'action-list--top'
                      : ''
                  "
                  v-show="table.expandEntityId == entity.receiptId"
                  @mouseleave="table.expandEntityId = ''"
                >
                  <li>
                    <div class="li-data">
                      {{ lang.table_items.dupplicate }}
                    </div>
                  </li>
                  <li v-show="!entity.ledgerStatus">
                    <div
                      class="li-data"
                      @click="deleteEntityFunction(entity.receiptId)"
                    >
                      {{ lang.table_items.delete }}
                    </div>
                  </li>
                </ul>
              </div>
            </td>
          </tr>
        </template>
      </tbody>
    </table>
    <div
      class="table__notification"
      v-show="!haveDataAfterCallApi && !isLoadingData"
    >
      <div class="noti__img"></div>
      <div class="noti__text">{{ lang.tableNoti.dataNotFound }}</div>
    </div>
  </div>
  <div class="table__info">
    <div class="ti__left">Tổng</div>
    <div class="ti__right">{{ numberFormater.format(totalReceive) }}</div>
  </div>
  <div class="table__pag">
    <div class="pag__leftside">
      <span
        >{{ lang.tablePag.total.left }}
        <strong>{{ pagingData.totalRecord }}</strong>
        {{ lang.tablePag.total.right }}</span
      >
    </div>
    <div class="pag__rightside">
      <div class="pag__recordcount">
        <div class="record__amount__select" v-show="table.recordAmountOpen">
          <ul>
            <li
              v-for="recordAmount in table.recordAmountList"
              :key="recordAmount"
            >
              <div
                class="record__amount__option"
                :class="[
                  recordAmount == pagingData.pageSize ? 'amount--selected' : '',
                ]"
                @click="recordAmountOptionOnClick(recordAmount)"
              >
                {{ recordAmount }} {{ lang.tablePag.recordAmount }}
              </div>
            </li>
          </ul>
        </div>
        <span>{{ lang.tablePag.recordPerPage }} {{ pagingData.pageSize }}</span>
        <div
          class="pag__arrowdown mi mi-24 mi-arrowdown-small"
          :class="[
            table.recordAmountOpen ? 'mi-arrowup-small' : 'mi-arrowdown-small',
          ]"
          @click="pagArrowdownOnClick"
        ></div>
      </div>
      <div class="pag__info">
        <div class="info__left">
          {{
            pagingData.pageSize * (pagingData.pageNumber - 1) +
            (rowList.length > 0 ? 1 : 0)
          }}
        </div>
        <div class="info__minus">-</div>
        <div class="info__right">
          <div class="right__number" v-show="!isLoadingData">
            <strong>{{
              pagingData.pageSize * (pagingData.pageNumber - 1) +
              pagingData.curAmount
            }}</strong>
          </div>
          <div class="right__loading" v-show="isLoadingData">
            <div class="loading-item"></div>
          </div>
        </div>
        <div class="info__text">{{ lang.tablePag.record }}</div>
      </div>
      <div
        class="pag__prev minc mi-24 mi-arrowleft"
        :class="[
          !isLoadingData && pagingData.pageNumber <= 1 ? 'disabled' : '',
        ]"
        @click="prevPageOnClick"
      ></div>
      <div
        class="pag__next minc mi-24 mi-arrowright"
        :class="[!isLoadingData && isLastPage ? 'disabled' : '']"
        @click="nextPageOnClick"
      ></div>
    </div>
  </div>
</template>

<script setup>
// #region import
import numberFormater from "@/js/common/number-formater";
import { ref, computed, inject } from "vue";
import { useRouter } from "vue-router";
const lang = inject("$lang");
// #endregion

// #region init
const router = useRouter();

const props = defineProps({
  pagingData: Object,
  isLoadingData: Boolean,
  rowList: Array,
  deleteEntityFunction: Function,
  pagingNextPage: Function,
  pagingPrevPage: Function,
  selectedEntityIds: Array,
  selectedAmountInPage: Number,
  haveDataAfterCallApi: Boolean,
  totalReceive: Number,
});

const emits = defineEmits([
  "updatePagingData",
  "updateRowStatus",
  "updateDupplicateEntity",
]);

const table = ref({
  recordAmountOpen: false,
  recordAmountList: [10, 20, 30, 50, 100],
  expandEntityId: "",
});

const tableStructure = {
  allowMultipeOperation: true,
  checkboxWidth: 50,
  headerList: [
    {
      name: lang.cash_receipt.table_header.postedDate,
      formatFunction: (s) => s,
      prop: "postedDate",
      align: "text-center",
      width: 200,
    },
    {
      name: lang.cash_receipt.table_header.receiptDate,
      formatFunction: (s) => s,
      prop: "receiptDate",
      align: "text-center",
      width: 200,
    },
    {
      name: lang.cash_receipt.table_header.receiptNo,
      formatFunction: (s) => s,
      prop: "receiptNo",
      align: "text-left",
      width: 180,
    },
    {
      name: lang.cash_receipt.table_header.reason,
      formatFunction: (s) => s,
      prop: "reason",
      align: "text-left",
      width: 400,
    },
    {
      name: lang.cash_receipt.table_header.totalAmount,
      formatFunction: (s) => numberFormater.format(s),
      prop: "totalAmount",
      align: "text-right",
      width: 200,
    },
    {
      name: lang.cash_receipt.table_header.customerName,
      formatFunction: (s) => s,
      prop: "customerName",
      align: "text-left",
      width: 400,
    },
    {
      name: lang.cash_receipt.table_header.customerCode,
      formatFunction: (s) => s,
      prop: "customerCode",
      align: "text-left",
      width: 200,
    },
    {
      name: lang.cash_receipt.table_header.customerAddress,
      formatFunction: (s) => s,
      prop: "customerAddress",
      align: "text-left",
      width: 240,
    },
  ],
};

// #endregion

// #region computed
/**
 * Kiểm tra xem trang hiện tại có phải là trang cuối không
 * Author: Dũng (28/05/2023)
 */
const isLastPage = computed(() => {
  return (
    (props.pagingData.pageNumber - 1) * props.pagingData.pageSize +
      props.pagingData.curAmount >=
    props.pagingData.totalRecord
  );
});
// #endregion

// #region handle event

/**
 * Sự kiện click vào ô check all
 * Author: Dũng (08/05/2023)
 */
function thCheckboxOnClick() {
  emits("updateRowStatus", {
    type: "toggleAllPage",
    rowIndex: "",
  });
}

/**
 * Click next chuyển trang
 * Author: Dũng (08/05/2023)
 */
async function nextPageOnClick() {
  if (isLastPage.value || props.isLoadingData) return;
  await props.pagingNextPage();
}

/**
 * Click prev chuyển trang
 * Author: Dũng (08/05/2023)
 */
async function prevPageOnClick() {
  if (props.pagingData.pageNumber <= 1 || props.isLoadingData) return;
  await props.pagingPrevPage();
}

/**
 * Click vào btn view receipt
 * @param {String} entityId
 * Author: Dũng (08/05/2023)
 */
function btnViewOnClick(entityId) {
  router.push(`/CA/CAReceipt/${entityId}`);
}

/**
 * Click vào nút mở rộng của một KH
 * @param {String} entityId Id KH
 * Author: Dũng (08/05/2023)
 */
function btnExpandOnClick(entityId) {
  if (table.value.expandEntityId == entityId) {
    table.value.expandEntityId = "";
  } else {
    table.value.expandEntityId = entityId;
  }
}

/**
 * Click chọn số lượng bản ghi/trang
 * @param {Number} recordAmount số lượng bản ghi/trang
 * Author: Dũng (08/05/2023)
 */
function recordAmountOptionOnClick(recordAmount) {
  emits("updatePagingData", {
    totalRecord: props.pagingData.totalRecord,
    curAmount: props.pagingData.curAmount,
    pageSize: recordAmount,
    pageNumber: 1,
  });
  table.value.recordAmountOpen = false;
}

/**
 * Click mở menu chọn số lượng bản ghi/trang
 * Author: Dũng (08/05/2023)
 */
function pagArrowdownOnClick() {
  table.value.recordAmountOpen = !table.value.recordAmountOpen;
}

/**
 * Click vào checkbox
 * Author: Dũng (08/05/2023)
 */
function checkBoxOnClick(rowIndex) {
  emits("updateRowStatus", {
    type: "selected",
    rowIndex: rowIndex,
  });
}

/**
 * Click vào tr
 * Author: Dũng (08/05/2023)
 */
function trOnClick(rowIndex) {
  emits("updateRowStatus", {
    type: "active",
    rowIndex: rowIndex,
  });
}

/**
 * DblClick vào checkbox
 * Author: Dũng (08/05/2023)
 */
function trOnDblclick(entityId) {
  router.push(`/CA/CAReceipt/${entityId}`);
}

// #endregion
</script>

<style
  scoped
  lang="css"
  src="../../../../css/components/views/cash/receipt/receipt-table.css"
></style>

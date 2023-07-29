<template>
  <div class="tablebox">
    <table class="m-table">
      <thead>
        <tr>
          <th v-for="header in tableStructure.headerList" :key="header.name">
            <div :class="header.align" :style="{ width: header.width + 'px' }">
              {{ header.name }}
            </div>
          </th>
          <th class="thn--sticky">
            <div class="align-center mw-90">{{ lang.tableHeader.tool }}</div>
          </th>
        </tr>
      </thead>
      <tbody>
        <template v-if="isLoadingData">
          <tr v-for="i in Math.min(12, pagingData.pageSize)" :key="i">
            <td class="td1--sticky"><div class="loading-item"></div></td>
            <td v-for="j in 5" :key="j">
              <div class="loading-item"></div>
            </td>
            <td class="tdn--sticky">
              <div class="loading-item"></div>
            </td>
          </tr>
        </template>
        <template v-else>
          <tr
            v-for="{
              active,
              acc,
              isExpand,
              isTemporary,
            } = row in rowListDisplay"
            :key="acc.accountId"
            :class="{
              active: active,
              'text--bold': acc.isParent,
            }"
            @click="trOnClick(acc.accountId)"
            @dblclick="trOnDblclick(acc.accountId)"
          >
            <td>
              <div
                class="text-left account__number"
                :style="{
                  paddingLeft:
                    24 * (acc.grade % 9) +
                    (!acc.isParent && acc.parentId == '' ? 24 : 0) +
                    (acc.parentId != '' && isTemporary ? 24 : 0) +
                    'px',
                }"
              >
                <div
                  class="an__expand mi mi-16"
                  :class="[
                    !isExpand
                      ? 'mi-tree-expand--small'
                      : 'mi-tree-collapse--small',
                  ]"
                  v-show="acc.isParent"
                  @click="accNumberExpandOnClick(acc.accountId)"
                  @dblclick.stop
                ></div>
                <div class="an__text" v-tooltip="acc['accountNumber']">
                  {{ acc["accountNumber"] }}
                </div>
              </div>
            </td>
            <td>
              <div class="text-left" v-tooltip="acc.accountNameVi">
                {{ acc.accountNameVi }}
              </div>
            </td>
            <td>
              <div class="text-left" v-tooltip="acc.categoryKindName">
                {{ acc.categoryKindName }}
              </div>
            </td>
            <td>
              <div class="text-left" v-tooltip="acc.accountNameEn">
                {{ acc.accountNameEn }}
              </div>
            </td>
            <td>
              <div class="text-left" v-tooltip="acc.description">
                {{ acc.description }}
              </div>
            </td>
            <td>
              <div class="text-left" v-tooltip="'Đang sử dụng'">
                {{ acc.usingStatusName }}
              </div>
            </td>
            <td
              :class="[table.expandAccId == acc.accountId ? 'above' : '']"
              class="tdn--sticky"
              @dblclick.stop
            >
              <div class="t__optionbox align-center">
                <button
                  class="option__edit"
                  @click="btnEditOnClick(acc.accountId)"
                >
                  {{ lang.table_items.edit }}
                </button>
                <button
                  class="btn__expand mi mi-16 mi-expand-down"
                  @click="btnExpandOnClick(acc.accountId)"
                ></button>
                <ul
                  class="actions-list btn__expand"
                  :class="
                    (acc.accountId ==
                      rowListDisplay[rowListDisplay.length - 1].acc.accountId ||
                      acc.accountId ==
                        rowListDisplay[rowListDisplay.length - 2].acc
                          .accountId) &&
                    rowListDisplay.length > 6
                      ? 'action-list--top'
                      : ''
                  "
                  v-show="table.expandAccId == acc.accountId"
                  @mouseleave="table.expandAccId = ''"
                >
                  <li>
                    <div class="li-data" @click="deleteFunction(acc.accountId)">
                      {{ lang.table_items.delete }}
                    </div>
                  </li>
                  <li v-if="acc.usingStatus">
                    <div
                      class="li-data"
                      @click="usingStatusOnClick(acc.mCodeId, false)"
                    >
                      {{ lang.table_items.stop }}
                    </div>
                  </li>
                  <li v-if="!acc.usingStatus">
                    <div
                      class="li-data"
                      @click="usingStatusOnClick(acc.mCodeId, true)"
                    >
                      {{ lang.table_items.startUsing }}
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
  <div class="table__pag">
    <div class="pag__leftside">
      <span
        >{{ lang.tablePag.total.left }}
        <strong>{{ pagingData.totalRecord }}</strong>
        {{ lang.tablePag.total.right }}</span
      >
    </div>
    <div class="pag__rightside">
      <div class="pag__info">
        <div class="info__left">
          {{
            pagingData.pageSize * (pagingData.pageNumber - 1) +
            (rowListDisplay.length > 0 ? 1 : 0)
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
    </div>
  </div>
</template>

<script setup>
// #region import
import { ref, computed, inject } from "vue";
import { useRouter } from "vue-router";
const lang = inject("$lang");
// #endregion

// #region init
const tableStructure = {
  allowMultipeOperation: true,
  headerList: [
    {
      name: lang.cat_account.tableStructure.accNumber,
      align: "text-left",
      width: 200,
    },
    {
      name: lang.cat_account.tableStructure.accNameVi,
      align: "text-left",
      width: 220,
    },
    {
      name: lang.cat_account.tableStructure.categoryKind,
      align: "text-left",
      width: 150,
    },
    {
      name: lang.cat_account.tableStructure.accNameEn,
      align: "text-left",
      width: 220,
    },
    {
      name: lang.cat_account.tableStructure.description,
      align: "text-left",
      width: 220,
    },
    {
      name: lang.cat_account.tableStructure.usingStatus,
      align: "text-left",
      width: 150,
    },
  ],
};
const router = useRouter();

const props = defineProps({
  pagingData: Object,
  isLoadingData: Boolean,
  rowList: Array,
  deleteFunction: Function,
  pagingNextPage: Function,
  pagingPrevPage: Function,
  haveDataAfterCallApi: Boolean,
});

const emits = defineEmits([
  "updatePagingData",
  "updateRowStatus",
  "updateUsingStatus",
]);

const table = ref({
  recordAmountOpen: false,
  recordAmountList: [10, 20, 30, 50, 100],
  expandAccId: "",
});

// #endregion

// #region computed
const rowListDisplay = computed(() => {
  return props.rowList.filter((row) => row.display);
});
// #endregion

// #region handle event

function accNumberExpandOnClick(accountId) {
  emits("updateRowStatus", {
    type: "ExpandCollapse",
    itemId: accountId,
  });
}

function btnEditOnClick(accId) {
  router.push(`/DI/DIAccount/${accId}`);
}

/**
 * Click vào nút mở rộng của một nhân viên
 * @param {String} empId Id nhân viên
 * Author: Dũng (08/05/2023)
 */
function btnExpandOnClick(empId) {
  if (table.value.expandAccId == empId) {
    table.value.expandAccId = "";
  } else {
    table.value.expandAccId = empId;
  }
}

/**
 * Click vào tr
 * @param {String} rowIndex index của dòng
 * Author: Dũng (08/05/2023)
 */
function trOnClick(accountId) {
  emits("updateRowStatus", {
    type: "active",
    itemId: accountId,
  });
}

/**
 * DblClick vào checkbox
 * @param {String} accId Id account
 * Author: Dũng (08/05/2023)
 */
function trOnDblclick(accId) {
  router.push(`/DI/DIAccount/${accId}`);
}

function usingStatusOnClick(mCodeId, usingStatus) {
  const data = { mCodeId, usingStatus };
  emits("updateUsingStatus", data);
}

// #endregion
</script>

<style
  scoped
  lang="css"
  src="../../../../css/components/views/category/account/account-table.css"
></style>

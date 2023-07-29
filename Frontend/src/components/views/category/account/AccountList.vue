<template>
  <div class="page__wrapper" v-show="isLoadingPage">
    <BaseLoader />
  </div>
  <div class="page__wrapper" v-show="dialog.isDisplay">
    <BaseDialog
      :title="lang.message.confirmDeleteTitle"
      :message="dialog.message"
      :close-on-click="dialogCloseOnClick"
      :no-on-click="dialogCloseOnClick"
      :yes-on-click="dialog.action"
    />
  </div>
  <BaseToastbox
    class="toastbox__position"
    :toast-list="toastList"
    :remove-toast="removeToast"
  />
  <router-view
    name="CustomerForm"
    @update-acc-list="accountOnUpdate"
  ></router-view>
  <div class="pcontent">
    <div class="pcontent__heading">
      <div class="pcontent__title">{{ lang.pageTitle }}</div>
      <BaseButton
        :bname="lang.button.addAccount"
        class="btn--primary"
        @click="btnAddOnClick"
      />
    </div>
    <div class="pcontent__goback" @click="goToCategoryOnClick">
      <div class="goback__icon mi mi-16 mi-chevron-left--primary"></div>
      <div class="goback__text">{{ lang.button.goBack }}</div>
    </div>
    <div class="pcontent__container">
      <div class="pcontent__searchbar">
        <div class="searchbar__right">
          <BaseTextfield
            :pholder="lang.text.searchAccount"
            :hideLabel="true"
            class="txtfield--search mw-300"
            noti=""
            v-model:text="cache.accSearchPattern"
            :realTimeSearch="true"
            :doSearch="doSearchAccount"
          />
          <div class="button__expanall">
            <div class="expandall__text" @click="expandAllBtnOnClick">
              {{ expandBtnStatus == 0 ? lang.text.expand : lang.text.collapse }}
            </div>
          </div>
          <BaseButton class="mi mi-36 mi-refresh" @click="loadAccountData" />
          <div class="button__hoverbox--refresh">
            <div class="hover__arrow"></div>
            <div class="hover__text">{{ lang.button.reload }}</div>
          </div>
          <div
            :class="[isLoadingExport ? 'disabled' : '']"
            class="minc mi-36 mi-excel"
            @click="exportExcelOnClick"
          ></div>
          <div class="button__hoverbox--export">
            <div class="hover__arrow"></div>
            <div class="hover__text">{{ lang.button.export }}</div>
          </div>
          <div class="export__loader" v-show="isLoadingExport">
            <BaseLoader></BaseLoader>
          </div>
        </div>
      </div>
      <AccountTable
        :is-loading-data="isLoadingData"
        :row-list="rowList"
        :key="tableKey"
        :delete-function="deleteAccountOnClick"
        :have-data-after-call-api="haveDataAfterCallApi"
        v-model:pagingData="pagingData"
        :paging-next-page="pagingNextPage"
        :paging-prev-page="pagingPrevPage"
        @update-paging-data="pagingDataOnUpdate"
        @update-row-status="rowStatusOnUpdate"
        @update-using-status="usingStatusOnUpdate"
      />
    </div>
  </div>
</template>

<script setup>
// #region import
import { ref, onMounted, onBeforeUnmount, inject } from "vue";
import { useRouter } from "vue-router";
import $api from "@/js/api";
import { Account } from "@/js/model/account";
import $error from "../../../../js/resources/error";
import AccountTable from "./AccountTable.vue";
import exportFormat from "@/js/resources/export-format";
const $lang = inject("$lang");
const lang = $lang.cat_account;
// #endregion

// #region init
const router = useRouter();
const $emitter = inject("$emitter");
const rowList = ref([]);
const isLoadingData = ref(false);
const isLoadingPage = ref(false);
const isLoadingExport = ref(false);
const $axios = inject("$axios");
const tableKey = ref(0);
const haveDataAfterCallApi = ref(true);
const pagingData = ref({
  totalRecord: 0,
  curAmount: 0,
  pageSize: 20,
  pageNumber: 1,
});
const dialog = ref({
  isDisplay: false,
  message: "",
  action: null,
});
const cache = ref({
  accDeleteId: "",
  accDeleteIndex: null,
  accDeleteNameVi: "",
  accSearchPattern: "",
});

const expandBtnStatus = ref(0);

const toastList = ref([]);
var toastId = 0;
// #endregion

// #region hook
onMounted(async () => {
  // Gọi API lấy danh sách account
  await loadAccountData();
  // Lắng nghe sự kiện sau khi thay đổi kích cỡ sidebar thì vẽ lại table
  $emitter.on("rerenderTable", () => {
    tableKey.value += 1;
    if (tableKey.value > 100) {
      tableKey.value = 0;
    }
  });
});

onBeforeUnmount(() => {
  $emitter.off("rerenderTable");
});
// #endregion

// #region function
/**
 * Tìm kiếm nhân viên khi nhập vào ô tìm kiếm
 * Author: Dũng (26/05/2023)
 */
async function doSearchAccount() {
  pagingData.value.pageNumber = 1;
  await loadAccountData();
}
/**
 * Tạo toast message mới và đẩy vào toastList
 * @param {Object} toast object thông báo
 *
 * Author: Dũng (10/05/2023)
 */
function pushToast(toast) {
  if (toastList.value.length > 5) toastList.value.splice(0, 1);
  toastList.value.push({
    id: toastId,
    type: toast.type,
    message: toast.message,
  });
  if (toast.timeToLive) {
    setToastTimeToLive(toastId, toast.timeToLive);
  }
  ++toastId;
  if (toastId > 99999) toastId = 0;
}

/**
 * Xóa một toast
 * @param {Number} id id của toast
 *
 * Author: Dũng (10/05/2023)
 */
function removeToast(id) {
  let i = 0;
  while (i < toastList.value.length) {
    if (toastList.value[i].id == id) {
      toastList.value.splice(i, 1);
      break;
    }
    ++i;
  }
}

/**
 * Set thời gian hiển thị của toast message
 * @param {Number} id id của toast
 * @param {Number} timeToLive thời gian hiển thị theo ms
 *
 * Author: Dũng (10/05/2023)
 */
function setToastTimeToLive(id, timeToLive) {
  setTimeout(() => {
    removeToast(id);
  }, timeToLive);
}

function showDeleteOneConfirmDialog(accNameVi) {
  dialog.value.message = lang.message.confirmDeleteMessage(accNameVi);
  dialog.value.action = async () => {
    dialog.value.isDisplay = false;
    await deleteAccount();
  };
  dialog.value.isDisplay = true;
}

/**
 * Quản lý lỗi trả về từ api
 * Author: Dũng (08/05/2023)
 */
function handleApiErrorResponse(error) {
  if (error.code == "ERR_NETWORK") {
    pushToast({
      type: "fail",
      message: $error.serverDisconnected,
    });
  } else {
    if (error.response && error.response.data) {
      pushToast({
        type: "fail",
        message: error.response.data.UserMessage,
      });
    } else {
      pushToast({
        type: "fail",
        message: $error.unexpectedError,
      });
    }
  }
}

async function deleteAccount() {
  try {
    isLoadingPage.value = true;

    await $axios.delete($api.account.one(cache.value.accDeleteId));
    await loadAccountData();
    isLoadingPage.value = false;

    // // Xóa KH đó trên table
    // rowList.value.splice(cache.value.accDeleteIndex, 1);

    // isLoadingPage.value = false;

    // // Update pagingData
    // // Cập nhật lại thông tin số bản ghi khách hàng
    // pagingData.value.curAmount -= 1;
    // pagingData.value.totalRecord -= 1;
    // if (pagingData.value.curAmount == 0) {
    //   // Nếu trang hiện tại bị xóa hết thì load lại trang trước đó
    //   if (pagingData.value.pageNumber > 1) --pagingData.value.pageNumber;
    //   await loadAccountData();
    // }

    // Đẩy toast xóa thành công
    pushToast({
      type: "success",
      message: lang.message.deleteSuccess,
      timeToLive: 1500,
    });
  } catch (error) {
    isLoadingPage.value = false;
    handleApiErrorResponse(error);
  }
}

// #endregion

// #region handle event
/**
 * Sự kiện click Tất cả danh mục
 *
 * Author: Dũng (21/06/2023)
 */
function goToCategoryOnClick() {
  router.push("/DI");
}

/**
 * Sự kiện click Export Excel
 *
 * Author: Dũng (04/06/2023)
 */
async function exportExcelOnClick() {
  try {
    if (isLoadingExport.value) return;
    isLoadingExport.value = true;

    const format = exportFormat.account;
    format.keySearch = cache.value.accSearchPattern;

    // Gọi api xuất file excel
    const response = await $axios.post($api.account.exportExcel, format, {
      responseType: "blob",
    });

    // Tạo URL cho blob data
    const url = window.URL.createObjectURL(new Blob([response.data]));

    // Tạo thẻ a và gắn url blob data vào
    const link = document.createElement("a");
    link.href = url;
    link.setAttribute("download", format.fileName);

    // Append link element vào DOM và tự click để download
    document.body.appendChild(link);
    link.click();

    // Remove các element vừa mới tạo khỏi trang
    window.URL.revokeObjectURL(url);
    document.body.removeChild(link);
    isLoadingExport.value = false;
  } catch (error) {
    isLoadingExport.value = false;
    console.log(error);
    pushToast({
      type: "fail",
      message: $error.exportFailed,
    });
  }
}

async function expandRow(rowIndex, itemId) {
  rowList.value[rowIndex].isExpand = true;
  let expandedAmount = 0;
  // Nếu có con từ dữ liệu cũ rồi thì không gọi api lấy con nữa
  if (
    rowIndex + 1 < rowList.value.length &&
    rowList.value[rowIndex + 1].acc.parentId ==
      rowList.value[rowIndex].acc.accountId
  ) {
    // Expand những thằng con cháu của nó
    for (let i = rowIndex + 1; i < rowList.value.length; ++i) {
      if (rowList.value[i].acc.parentId == itemId) {
        rowList.value[i].active = false;
        rowList.value[i].display = true;
        rowList.value[i].isExpand = false;
        ++expandedAmount;
      }
    }
  } else {
    // Nếu chưa có dữ liệu
    let pAccount = rowList.value[rowIndex].acc;
    const filterOption = {
      grade: pAccount.grade + 1,
      parentIdList: pAccount.accountId,
    };
    const filterData = await filterAccount(filterOption);
    const childList = filterData.filteredList;
    expandedAmount = childList.length;
    let insertIndex = rowIndex;
    for (const acc of childList) {
      ++insertIndex;
      const accConverted = new Account(acc);
      rowList.value.splice(insertIndex, 0, {
        active: false,
        display: true,
        acc: accConverted,
        isExpand: false,
        isTemporary: false,
      });
    }
  }
  pagingData.value.curAmount += expandedAmount;

  // Xóa những thằng temporary
  // let i = 0;
  // while (i < rowList.value.length) {
  //   if (rowList.value[i].isTemporary) {
  //     rowList.value.splice(i, 1);
  //     pagingData.value.curAmount -= 1;
  //   }
  //   ++i;
  // }
}

function collapseRow(rowIndex) {
  rowList.value[rowIndex].isExpand = false;
  let collapsedAmount = 0;
  // Collapse những thằng con cháu của nó
  for (let i = rowIndex + 1; i < rowList.value.length; ++i) {
    if (
      rowList.value[i].display &&
      rowList.value[i].acc.mCodeId.startsWith(
        rowList.value[rowIndex].acc.mCodeId
      )
    ) {
      rowList.value[i].active = false;
      rowList.value[i].display = false;
      rowList.value[i].isExpand = false;
      ++collapsedAmount;
    }
  }
  pagingData.value.curAmount -= collapsedAmount;
}

function collapseAllRow() {
  let rowIndex = 0;
  while (rowIndex < rowList.value.length) {
    if (rowList.value[rowIndex].isExpand) {
      collapseRow(rowIndex);
    }
    ++rowIndex;
  }
}

/**
 * Sự kiện khi cập nhật trạng thái của nhân viên (select, active, toggleAll)
 * @param {Object} data object thông báo
 *
 * Author: Dũng (10/05/2023)
 */
async function rowStatusOnUpdate(data) {
  const { type, itemId } = data;
  let rowIndex = 0;
  for (let i = 0; i < rowList.value.length; ++i) {
    if (rowList.value[i].acc.accountId == itemId) {
      rowIndex = i;
      break;
    }
  }
  if (type == "active") {
    // Nếu row này đang không được select thì cập nhật trạng thái active
    if (!rowList.value[rowIndex].selected) {
      rowList.value[rowIndex].active = !rowList.value[rowIndex].active;
    }
    // Nếu row này được bật active
    if (rowList.value[rowIndex].active) {
      // Tắt những row khác đang active mà không selected
      for (const row of rowList.value) {
        if (
          !row.selected &&
          row.acc.accountId != rowList.value[rowIndex].acc.accountId
        )
          row.active = false;
      }
    }
  }
  if (type == "ExpandCollapse") {
    if (!rowList.value[rowIndex].isExpand) {
      await expandRow(rowIndex, itemId);
    } else {
      collapseRow(rowIndex);
    }
  }
}

async function filterAccount(filterOption) {
  try {
    const response = await $axios.get($api.account.filter, {
      params: filterOption,
    });
    return response.data;
  } catch (error) {
    handleApiErrorResponse(error);
  }
}

/**
 * Sự kiện click vào nút đóng dialog
 *
 * Author: Dũng (08/05/2023)
 */
function dialogCloseOnClick() {
  dialog.value.isDisplay = false;
}

function deleteAccountOnClick(accId) {
  let accNameVi = "";
  cache.value.accDeleteId = accId;
  for (let index in rowList.value) {
    if (
      rowList.value[index].acc.accountId == accId &&
      rowList.value[index].acc.isParent
    ) {
      pushToast({
        type: "fail",
        message: lang.message.deleteParentWarnning,
      });
      return;
    }
    if (rowList.value[index].acc.accountId == accId) {
      cache.value.accDeleteIndex = index;
      accNameVi = rowList.value[index].acc.accountNameVi;
      break;
    }
  }
  showDeleteOneConfirmDialog(accNameVi);
}

/**
 * Sự kiện cập nhật trạng thái của object pagingData
 * @param {Object} newData giá trị mới
 *
 * Author: Dũng (08/05/2023)
 */
async function pagingDataOnUpdate(newData) {
  pagingData.value = newData;
  await loadAccountData();
}

/**
 * Chuyển sang trang paging kế tiếp
 *
 * Author: Dũng (08/05/2023)
 */
async function pagingNextPage() {
  pagingData.value.pageNumber += 1;
  await loadAccountData();
  // console.log("n next");
}

/**
 * Chuyển sang trang paging trước đó
 *
 * Author: Dũng (08/05/2023)
 */
async function pagingPrevPage() {
  pagingData.value.pageNumber -= 1;
  await loadAccountData();
  // console.log("p prev");
}

async function expandAllRow() {
  let layerHaveChild;
  let grade = 0;
  do {
    layerHaveChild = false;
    for (let i = 0; i < rowList.value.length; ++i) {
      if (
        rowList.value[i].acc.isParent &&
        rowList.value[i].acc.grade == grade
      ) {
        layerHaveChild = true;
      }
    }
    if (!layerHaveChild) break;
    let i = 0;
    while (i < rowList.value.length) {
      if (
        rowList.value[i].acc.isParent &&
        rowList.value[i].acc.grade == grade
      ) {
        await expandRow(i, rowList.value[i].acc.accountId);
      }
      ++i;
    }
    ++grade;
  } while (layerHaveChild);
}

/**
 * Gọi API lấy dữ liệu nhân viên theo filter
 * Author: Dũng (08/05/2023)
 */
async function loadAccountData() {
  try {
    // Nếu dữ liệu đang được gọi thì return
    if (isLoadingData.value == true) return;
    isLoadingData.value = true;
    await new Promise((resolve) => setTimeout(resolve, 800));
    rowList.value = [];
    let filterOption = {
      grade: cache.value.accSearchPattern.length > 0 ? null : 0,
      parentIdList: "",
      keySearch: cache.value.accSearchPattern,
    };
    const filterData = await filterAccount(filterOption);
    if (filterData.filteredList) {
      for (const acc of filterData.filteredList) {
        const accConverted = new Account(acc);
        rowList.value.push({
          active: false,
          display: true,
          acc: accConverted,
          isExpand: false,
          isTemporary: false,
        });
      }
    }
    // Nếu tìm kiếm tài khoản cập nhật lại expand của các dòng tìm được
    if (cache.value.accSearchPattern.length > 0) {
      let rowIsParent;
      for (const row of rowList.value) {
        rowIsParent = false;
        for (const r of rowList.value) {
          if (r.acc.accountNumber.startsWith(row.acc.accountNumber)) {
            rowIsParent = true;
            break;
          }
        }
        if (rowIsParent) {
          row.isExpand = true;
        }
      }
    }
    // Số bản ghi ở trang hiện tại
    pagingData.value.curAmount = filterData.filteredList.length ?? 0;
    // Tổng số bản ghi
    pagingData.value.totalRecord = filterData.totalRecord ?? 0;
    // Số bản ghi ở trang hiện tại có trống hay không
    haveDataAfterCallApi.value = pagingData.value.totalRecord != 0;

    isLoadingData.value = false;
  } catch (error) {
    isLoadingData.value = false;
    handleApiErrorResponse(error);
  }
}

async function accountOnUpdate(type, data) {
  // console.log("Customer list updated");
  // console.log(type);
  // console.log(data);
  const { account, reload } = data;
  switch (type) {
    case "create":
      pagingData.value.totalRecord += 1;
      pagingData.value.curAmount += 1;
      rowList.value.unshift({
        active: false,
        display: true,
        acc: account,
        isExpand: false,
        isTemporary: true,
      });
      if (pagingData.value.curAmount > 2 * pagingData.value.pageSize) {
        await loadAccountData();
      }
      haveDataAfterCallApi.value = true;
      pushToast({
        type: "success",
        message: lang.message.createSuccess,
        timeToLive: 1500,
      });
      break;
    case "edit":
      if (reload) {
        await loadAccountData();
      } else {
        for (const row of rowList.value) {
          if (row.acc.accountId == account.accountId) {
            row.acc = account;
            break;
          }
        }
      }
      pushToast({
        type: "success",
        message: lang.message.updateSuccess,
        timeToLive: 1500,
      });
      break;
    default:
      break;
  }
  // await loadAccountData();
}

/**
 * Sự kiện click vào nút thêm
 * Author: Dũng (08/05/2023)
 */
function btnAddOnClick() {
  router.replace("/DI/DIAccount/create");
}

async function expandAllBtnOnClick() {
  if (expandBtnStatus.value == 0) {
    await expandAllRow();
    expandBtnStatus.value = 1;
  } else {
    collapseAllRow();
    expandBtnStatus.value = 0;
  }
}

async function usingStatusOnUpdate(data) {
  try {
    const params = {
      mCodeId: data.mCodeId,
      usingStatus: data.usingStatus,
    };
    isLoadingPage.value = true;
    await $axios.put($api.account.changeUsingStatus, params);
    await loadAccountData();
    isLoadingPage.value = false;
  } catch (error) {
    isLoadingPage.value = false;
    handleApiErrorResponse(error);
  }
}

// #endregion
</script>

<style
  scoped
  lang="css"
  src="../../../../css/components/views/category/account/account-list.css"
></style>

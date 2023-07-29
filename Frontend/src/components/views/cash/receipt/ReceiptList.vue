<template>
  <div class="page__wrapper" v-show="isLoadingPage">
    <BaseLoader />
  </div>
  <div class="page__wrapper" v-show="dialog.isDisplay">
    <BaseDialog
      :title="lang.cash_receipt.message.deleteConfirmTitle"
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
    name="ReceiptForm"
    v-model:metadata="formMetadata"
    @reload-entity-list="reloadList"
  ></router-view>
  <div class="rlist__pcontent">
    <div class="rlist__pcontent__overview">
      <div class="overview__container">
        <div class="o_item item--total-receive">
          <div class="o_item__left">
            {{ lang.cash_receipt.text.totalReceived }}
          </div>
          <div
            class="o_item__right"
            v-tooltip="lang.cash_receipt.tooltip.clickToViewDetail"
          >
            12.582.764.110
          </div>
        </div>
        <div class="o_item item--total-paid">
          <div class="o_item__left">
            {{ lang.cash_receipt.text.totalPaid }}
          </div>
          <div
            class="o_item__right"
            v-tooltip="lang.cash_receipt.tooltip.clickToViewDetail"
          >
            502.260.510
          </div>
        </div>
        <div class="o_item item--total-left">
          <div class="o_item__left">
            {{ lang.cash_receipt.text.totalMoney }}
          </div>
          <div
            class="o_item__right"
            v-tooltip="lang.cash_receipt.tooltip.clickToViewDetail"
          >
            12.080.503.600
          </div>
        </div>
      </div>
    </div>
    <div class="rlist__pcontent__container">
      <div class="rlist__pcontent__searchbar">
        <div class="searchbar__right">
          <div class="filter">
            <div class="filter__btn">
              <div class="btn__text">{{ lang.cash_receipt.text.filter }}</div>
              <div class="btn__icon mi mi-16 mi-arrowup--black"></div>
            </div>
            <div class="filter__panel">
              <div class="panel__top"></div>
              <div class="panel__mid"></div>
              <div class="panel__bot"></div>
            </div>
          </div>
          <BaseTextfield
            pholder="Tìm kiếm"
            :hideLabel="true"
            class="txtfield--search mw-300"
            noti=""
            v-model:text="cache.searchPattern"
            :realTimeSearch="true"
            :doSearch="doSearchEntity"
          />
          <BaseButton class="mi mi-36 mi-refresh" @click="loadDataFromApi" />
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
          <div class="btn__add-receipt">
            <div class="add__text" @click="btnAddOnClick">
              {{ lang.cash_receipt.text.receive }}
            </div>
            <div class="add__select">
              <div class="select__icon mi mi-16 mi-arrow-up--white"></div>
            </div>
          </div>
        </div>
        <div class="searchbar__left" v-show="selectedEntityIds.length > 1">
          <div class="left__info">
            {{ lang.cash_receipt.text.selected
            }}<strong>{{ selectedEntityIds.length }}</strong>
          </div>
          <div class="left__cancel" @click="cancelSelectOnClick">
            {{ lang.button.cancelSelect }}
          </div>
          <BaseButton
            :bname="lang.button.batchDelete"
            class="btn--secondary"
            @click="batchDeleteBtnOnClick"
          />
        </div>
      </div>
      <ReceiptTable
        :is-loading-data="isLoadingData"
        :row-list="rowList"
        :key="tableKey"
        :delete-entity-function="deleteEntityOnClick"
        :selected-entity-ids="selectedEntityIds"
        :selected-amount-in-page="selectedAmountInPage"
        :have-data-after-call-api="haveDataAfterCallApi"
        v-model:pagingData="pagingData"
        :paging-next-page="pagingNextPage"
        :paging-prev-page="pagingPrevPage"
        :total-receive="totalReceived"
        @update-paging-data="pagingDataOnUpdate"
        @update-row-status="rowStatusOnUpdate"
        @update-dupplicate-entity="dupplicateEntityOnUpdate"
      />
    </div>
  </div>
</template>

<script setup>
// #region import
import ReceiptTable from "./ReceiptTable.vue";
import { ref, onMounted, onBeforeUnmount, inject } from "vue";
import { useRouter } from "vue-router";
import $api from "@/js/api";
import { Receipt } from "@/js/model/receipt";
import $error from "../../../../js/resources/error";
import exportFormat from "@/js/resources/export-format";
// import $message from "../../../../js/resources/message";
const lang = inject("$lang");
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
const totalReceived = ref(0);
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
  entityDeleteId: "",
  entityDeleteIndex: "",
  searchPattern: "",
});
const selectedEntityIds = ref([]);
const selectedRecordedIds = ref([]);
const selectedUnRecordedIds = ref([]);
const selectedAmountInPage = ref(0);
const toastList = ref([]);
var toastId = 0;
const formMetadata = ref({
  isDupplicate: false,
  entityDupplicate: null,
});
// #endregion

// #region hook
onMounted(async () => {
  // Gọi API lấy danh sách nhân viên
  await loadDataFromApi();
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
async function doSearchEntity() {
  pagingData.value.pageNumber = 1;
  await loadDataFromApi();
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

/**
 * Hiển thị cảnh báo xóa nhân viên
 * @param {String} entityCode mã nhân viên
 *
 * Author: Dũng (08/05/2023)
 */
function showDeleteOneConfirmDialog(entityCode) {
  dialog.value.message = lang.cash_receipt.message.deleteOneMessage;
  console.log(entityCode);
  dialog.value.action = async () => {
    dialog.value.isDisplay = false;
    await deleteEntity();
  };
  dialog.value.isDisplay = true;
}

function batchDeleteBtnOnClick() {
  if (selectedRecordedIds.value.length > 0) {
    pushToast({
      type: "fail",
      message: lang.cash_receipt.toast.cannotDeleteRecordedReceipt,
      timeToLive: 2000,
    });
    return;
  }
  showBatchDeleteConfirmDialog();
}

/**
 * Hiển thị cảnh báo xóa hàng loạt
 *
 * Author: Dũng (08/05/2023)
 */
function showBatchDeleteConfirmDialog() {
  dialog.value.message = lang.cash_receipt.message.deleteBatchMessage;
  dialog.value.isDisplay = true;
  dialog.value.action = async () => {
    dialog.value.isDisplay = false;
    await deleteBatchEntity();
  };
}

/**
 * Quản lý lỗi trả về từ api
 * Author: Dũng (08/05/2023)
 */
function handleApiErrorResponse(error) {
  console.log(error);
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

/**
 * Gọi API xóa KH
 * Author: Dũng (08/05/2023)
 */
async function deleteEntity() {
  try {
    isLoadingPage.value = true;
    // Gọi API xóa KH theo ID
    await $axios.delete($api.receipt.one(cache.value.entityDeleteId));
    // Xóa KH đó trên table
    rowList.value.splice(cache.value.entityDeleteIndex, 1);

    const index = selectedEntityIds.value.indexOf(cache.value.entityDeleteId);
    if (index > -1) {
      // Nếu Entity đó đang được select thì xóa khỏi danh sách select
      selectedEntityIds.value.splice(index, 1);
      selectedAmountInPage.value -= 1;
    }

    isLoadingPage.value = false;

    // Update pagingData
    // Cập nhật lại thông tin số bản ghi khách hàng
    pagingData.value.curAmount -= 1;
    pagingData.value.totalRecord -= 1;
    if (pagingData.value.curAmount == 0) {
      // Nếu trang hiện tại bị xóa hết thì load lại trang trước đó
      if (pagingData.value.pageNumber > 1) --pagingData.value.pageNumber;
      await loadDataFromApi();
    }

    // Đẩy toast xóa thành công
    pushToast({
      type: "success",
      message: lang.cash_receipt.toast.deletedSuccess,
      timeToLive: 1500,
    });
  } catch (error) {
    isLoadingPage.value = false;
    handleApiErrorResponse(error);
  }
}

/**
 * Gọi API xóa hàng loạt nhân viên
 * Author: Dũng (08/05/2023)
 */
async function deleteBatchEntity() {
  try {
    // Số lượng KH đã bị xóa thành công
    let deletedSucess = 0;
    let idList = [];
    let batchAmount = 0;
    isLoadingPage.value = true;

    while (selectedEntityIds.value.length) {
      // Số lượng KH bị xóa tối đa trong một lượt
      batchAmount = Math.min(20, selectedEntityIds.value.length);
      idList = [];
      for (let i = 0; i < batchAmount; ++i)
        idList.push(selectedEntityIds.value[i]);
      await $axios.post($api.receipt.deleteMultiple, idList);
      deletedSucess += batchAmount;
      selectedEntityIds.value.splice(0, batchAmount);
    }
    // Load lại dữ liệu cho table từ trang 1
    pagingData.value.pageNumber = 1;
    isLoadingPage.value = false;
    await loadDataFromApi();

    pushToast({
      type: "success",
      message: lang.cash_receipt.toast.deleteSuccessAmount(deletedSucess),
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
 * Sự kiện Entity Table emit dupplicate lên Entity List
 * @param {Object} entity object entity được dupplicate
 *
 * Author: Dũng (2/07/2023)
 */
function dupplicateEntityOnUpdate(entity) {
  formMetadata.value.isDupplicate = true;
  formMetadata.value.entityDupplicate = entity;
  router.push("/CA/CAReceipt/create");
}

/**
 * Sự kiện khi cập nhật trạng thái của nhân viên (select, active, toggleAll)
 * @param {Object} data object thông báo
 *
 * Author: Dũng (10/05/2023)
 */
function rowStatusOnUpdate(data) {
  const { type, rowIndex } = data;
  if (type == "toggleAllPage") {
    // Nếu không có entity nào đang được chọn
    if (selectedAmountInPage.value == 0) {
      // Chọn tất cả
      selectedAmountInPage.value = rowList.value.length;
      for (const row of rowList.value) {
        row.selected = true;
        row.active = true;
        selectedEntityIds.value.push(row.entity.receiptId);
        if (row.entity.ledgerStatus) {
          selectedRecordedIds.value.push(row.entity.receiptId);
        } else {
          selectedUnRecordedIds.value.push(row.entity.receiptId);
        }
      }
    } else {
      // Nếu có ít nhất một entity đang được chọn
      // Hủy chọn tất cả
      selectedAmountInPage.value = 0;
      for (const row of rowList.value) {
        row.selected = false;
        row.active = false;

        const indexInSelectedEntityIds = selectedEntityIds.value.indexOf(
          row.entity.receiptId
        );
        if (indexInSelectedEntityIds > -1)
          selectedEntityIds.value.splice(indexInSelectedEntityIds, 1);

        const indexInSelectedRecoredIds = selectedRecordedIds.value.indexOf(
          row.entity.receiptId
        );
        if (indexInSelectedRecoredIds > -1)
          selectedRecordedIds.value.splice(indexInSelectedRecoredIds, 1);

        const indexInSelectedUnRecordedIds =
          selectedUnRecordedIds.value.indexOf(row.entity.receiptId);
        if (indexInSelectedUnRecordedIds > -1)
          selectedUnRecordedIds.value.splice(indexInSelectedUnRecordedIds, 1);
      }
    }
    return;
  }

  if (type == "selected") {
    // Đổi trạng thái selected của row
    rowList.value[rowIndex].selected = !rowList.value[rowIndex].selected;
    let selectReceiptId = rowList.value[rowIndex].entity.receiptId;
    // Nếu selected true thì thêm vào selectedEntityIds và bật active
    if (rowList.value[rowIndex].selected) {
      ++selectedAmountInPage.value;
      selectedEntityIds.value.push(selectReceiptId);
      if (rowList.value[rowIndex].entity.ledgerStatus) {
        selectedRecordedIds.value.push(selectReceiptId);
      } else {
        selectedUnRecordedIds.value.push(selectReceiptId);
      }
      rowList.value[rowIndex].active = true;
      // Tắt active của những ô khác mà không được selected
      for (const row of rowList.value) {
        if (row.entity.receiptId != selectReceiptId && !row.selected)
          row.active = false;
      }
    } else {
      --selectedAmountInPage.value;
      // Nếu seleted của entity này false
      // Xóa khỏi selectedEntityIds và tắt active
      selectedEntityIds.value.splice(
        selectedEntityIds.value.indexOf(selectReceiptId),
        1
      );
      selectedRecordedIds.value.splice(
        selectedRecordedIds.value.indexOf(selectReceiptId),
        1
      );
      selectedUnRecordedIds.value.splice(
        selectedUnRecordedIds.value.indexOf(selectReceiptId),
        1
      );
      rowList.value[rowIndex].active = false;
    }
    return;
  }

  if (data.type == "active") {
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
          row.entity.receiptId != rowList.value[rowIndex].entity.receiptId
        )
          row.active = false;
      }
    }
  }
}

/**
 * Sự kiện click vào nút bỏ chọn
 *
 * Author: Dũng (25/05/2023)
 */
function cancelSelectOnClick() {
  for (const row of rowList.value) {
    selectedAmountInPage.value = 0;
    row.selected = false;
    row.active = false;
    selectedEntityIds.value = [];
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

/**
 * Sự kiện click vào nút xóa nhân viên
 * @param {String} entityId Id nhân viên
 *
 * Author: Dũng (08/05/2023)
 */
function deleteEntityOnClick(entityId) {
  let entityCode = "";
  cache.value.entityDeleteId = entityId;
  for (let index in rowList.value) {
    if (rowList.value[index].entity.receiptId == entityId) {
      cache.value.entityDeleteIndex = index;
      entityCode = rowList.value[index].entity.receiptNo;
      break;
    }
  }
  showDeleteOneConfirmDialog(entityCode);
}

/**
 * Sự kiện cập nhật trạng thái của object pagingData
 * @param {Object} newData giá trị mới
 *
 * Author: Dũng (08/05/2023)
 */
async function pagingDataOnUpdate(newData) {
  pagingData.value = newData;
  await loadDataFromApi();
}

/**
 * Chuyển sang trang paging kế tiếp
 *
 * Author: Dũng (08/05/2023)
 */
async function pagingNextPage() {
  pagingData.value.pageNumber += 1;
  await loadDataFromApi();
  // console.log("n next");
}

/**
 * Chuyển sang trang paging trước đó
 *
 * Author: Dũng (08/05/2023)
 */
async function pagingPrevPage() {
  pagingData.value.pageNumber -= 1;
  await loadDataFromApi();
  // console.log("p prev");
}

/**
 * Gọi API lấy dữ liệu nhân viên theo filter
 * Author: Dũng (08/05/2023)
 */
async function loadDataFromApi() {
  try {
    // Nếu dữ liệu đang được gọi thì return
    if (isLoadingData.value == true) return;
    selectedAmountInPage.value = 0;
    isLoadingData.value = true;
    await new Promise((resolve) => setTimeout(resolve, 800));

    // Gọi API filter
    const response = await $axios.get($api.receipt.filter, {
      params: {
        skip: pagingData.value.pageSize * (pagingData.value.pageNumber - 1),
        take: pagingData.value.pageSize,
        keySearch: cache.value.searchPattern,
      },
    });
    rowList.value = [];
    // console.log(response.data);
    if (response.data.filteredList) {
      for (const entity of response.data.filteredList) {
        const entityConverted = new Receipt(entity);
        const isSelected = selectedEntityIds.value.includes(
          entityConverted.receiptId
        );
        if (isSelected) ++selectedAmountInPage.value;
        rowList.value.push({
          active: isSelected,
          selected: isSelected,
          entity: entityConverted,
        });
      }
    }
    console.log(rowList.value);
    pagingData.value.curAmount = response.data.filteredList.length ?? 0;
    // Tổng số bản ghi
    pagingData.value.totalRecord = response.data.totalRecord ?? 0;
    // Số bản ghi ở trang hiện tại có trống hay không
    haveDataAfterCallApi.value = pagingData.value.totalRecord != 0;

    // Gọi api lấy tổng thu
    let totalReceiveResponse = await $axios.get($api.receipt.getTotalReceive, {
      params: {
        keySearch: cache.value.searchPattern ?? "",
      },
    });
    totalReceived.value = totalReceiveResponse.data;
    isLoadingData.value = false;
  } catch (error) {
    isLoadingData.value = false;
    handleApiErrorResponse(error);
  }
}

/**
 * Sự kiện cập nhật mảng rowList
 * @param {String} type kiểu update (thêm hay sửa)
 * @param {Object} data dữ liệu mới
 * Author: Dũng (08/05/2023)
 */
async function reloadList() {
  await loadDataFromApi();
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

    const format = exportFormat.receipt;
    format.keySearch = cache.value.searchPattern;

    // Gọi api xuất file excel
    const response = await $axios.post($api.receipt.exportExcel, format, {
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

function btnAddOnClick() {
  router.push("/CA/CAReceipt/create");
}

// #endregion
</script>

<style
  scoped
  lang="css"
  src="../../../../css/components/views/cash/receipt/receipt-list.css"
></style>

<template>
  <div class="page__wrapper" v-show="isLoadingPage">
    <BaseLoader />
  </div>
  <div class="page__wrapper" v-show="dialog.isDisplay">
    <BaseDialog
      :title="lang.common.dialog.deleteConfirmation.title"
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
    name="EmployeeForm"
    v-model:metadata="formMetadata"
    @update-emplist="employeeOnUpdate"
  ></router-view>
  <div class="pcontent">
    <div class="pcontent__heading">
      <div class="pcontent__title">{{ lang.employeeList.title }}</div>
      <BaseButton
        :bname="lang.button.addEmployee"
        class="btn--primary"
        @click="btnAddOnClick"
      />
    </div>
    <div class="pcontent__goback" @click="goToCategoryOnClick">
      <div class="goback__icon mi mi-16 mi-chevron-left--primary"></div>
      <div class="goback__text">Tất cả danh mục</div>
    </div>
    <div class="pcontent__container">
      <div class="pcontent__searchbar">
        <div class="searchbar__right">
          <BaseTextfield
            :pholder="lang.textfield.searchBar.pholder"
            class="txtfield--search mw-300"
            noti=""
            :hideLabel="true"
            v-model:text="cache.empSearchPattern"
            :realTimeSearch="true"
            :doSearch="doSearchEmployee"
          />
          <BaseButton class="mi mi-36 mi-refresh" @click="loadEmployeeData" />
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
        <div class="searchbar__left" v-show="selectedEmpIds.length > 1">
          <div class="left__info">
            Đã chọn: <strong>{{ selectedEmpIds.length }}</strong>
          </div>
          <div class="left__cancel" @click="cancelSelectOnClick">
            {{ lang.button.cancelSelect }}
          </div>
          <BaseButton
            bname="Xóa hàng loạt"
            class="btn--secondary"
            @click="showBatchDeleteConfirmDialog"
          />
        </div>
      </div>
      <EmployeeTable
        :is-loading-data="isLoadingData"
        :row-list="rowList"
        :key="tableKey"
        :delete-employee-function="deleteEmployeeOnClick"
        :selected-emp-ids="selectedEmpIds"
        :selected-amount-in-page="selectedAmountInPage"
        :have-data-after-call-api="haveDataAfterCallApi"
        v-model:pagingData="pagingData"
        :paging-next-page="pagingNextPage"
        :paging-prev-page="pagingPrevPage"
        @update-paging-data="pagingDataOnUpdate"
        @update-row-status="rowStatusOnUpdate"
        @update-dupplicate-emp="dupplicateEmpOnUpdate"
      />
    </div>
  </div>
</template>

<script setup>
// #region import
import EmployeeTable from "@/components/views/category/employee/EmployeeTable.vue";
import { ref, onMounted, onBeforeUnmount, inject, nextTick } from "vue";
import { useRouter } from "vue-router";
import $api from "@/js/api";
import { Employee } from "@/js/model/employee";
import $error from "../../../../js/resources/error";
import $message from "../../../../js/resources/message";
import $enum from "@/js/common/enum";
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
  empDeleteId: "",
  empDeleteIndex: "",
  empSearchPattern: "",
});
const selectedEmpIds = ref([]);
const selectedAmountInPage = ref(0);
const toastList = ref([]);
var toastId = 0;
const formMetadata = ref({
  isDupplicate: false,
  employeeDupplicate: null,
});
// #endregion

// #region hook
onMounted(async () => {
  // Gọi API lấy danh sách nhân viên
  await loadEmployeeData();
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
async function doSearchEmployee() {
  pagingData.value.pageNumber = 1;
  await loadEmployeeData();
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
 * @param {String} empCode mã nhân viên
 *
 * Author: Dũng (08/05/2023)
 */
function showDeleteOneConfirmDialog(empCode) {
  dialog.value.message = $message.employeeDeleteConfirm(empCode);
  dialog.value.action = async () => {
    dialog.value.isDisplay = false;
    await deleteEmployee();
  };
  dialog.value.isDisplay = true;
}

/**
 * Hiển thị cảnh báo xóa hàng loạt
 *
 * Author: Dũng (08/05/2023)
 */
function showBatchDeleteConfirmDialog() {
  dialog.value.message = $message.employeeMultipleDeleteConfirm(
    selectedEmpIds.value.length
  );
  dialog.value.isDisplay = true;
  dialog.value.action = async () => {
    dialog.value.isDisplay = false;
    await deleteBatchEmployee();
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
 * Gọi API xóa nhân viên
 * Author: Dũng (08/05/2023)
 */
async function deleteEmployee() {
  try {
    isLoadingPage.value = true;
    // Gọi API xóa nhân viên theo ID
    await $axios.delete($api.employee.one(cache.value.empDeleteId));
    // Xóa nhân viên đó trên table
    rowList.value.splice(cache.value.empDeleteIndex, 1);

    const index = selectedEmpIds.value.indexOf(cache.value.empDeleteId);
    if (index > -1) {
      // Nếu employee đó đang được select thì xóa khỏi danh sách select
      selectedEmpIds.value.splice(index, 1);
      selectedAmountInPage.value -= 1;
    }

    isLoadingPage.value = false;

    // Update pagingData
    // Cập nhật lại thông tin số bản ghi nhân viên
    pagingData.value.curAmount -= 1;
    pagingData.value.totalRecord -= 1;
    if (pagingData.value.curAmount == 0) {
      // Nếu trang hiện tại bị xóa hết thì load lại trang trước đó
      if (pagingData.value.pageNumber > 1) --pagingData.value.pageNumber;
      await loadEmployeeData();
    }

    // Đẩy toast xóa thành công
    pushToast({
      type: "success",
      message: $message.employeeDeleted,
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
async function deleteBatchEmployee() {
  try {
    // Số lượng nhân viên đã bị xóa thành công
    let deletedSucess = 0;
    let idList = [];
    let batchAmount = 0;
    isLoadingPage.value = true;

    while (selectedEmpIds.value.length) {
      // Số lượng nhân viên bị xóa tối đa trong một lượt
      batchAmount = Math.min(20, selectedEmpIds.value.length);
      idList = [];
      for (let i = 0; i < batchAmount; ++i)
        idList.push(selectedEmpIds.value[i]);
      await $axios.post($api.employee.deleteMultiple, idList);
      deletedSucess += batchAmount;
      selectedEmpIds.value.splice(0, batchAmount);
    }
    // Load lại dữ liệu cho table từ trang 1
    pagingData.value.pageNumber = 1;
    isLoadingPage.value = false;
    await loadEmployeeData();

    pushToast({
      type: "success",
      message: $message.employeeMultipeDeleted(deletedSucess),
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
    await new Promise((resolve) => setTimeout(resolve, 1000));

    // Gọi api xuất file excel
    const response = await $axios.get($api.employee.exportExcel, {
      responseType: "blob",
    });

    // Tạo URL cho blob data
    const url = window.URL.createObjectURL(new Blob([response.data]));

    // Tạo thẻ a và gắn url blob data vào
    const link = document.createElement("a");
    link.href = url;
    link.setAttribute("download", $enum.exportedFileName);

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

/**
 * Sự kiện Employee Table emit dupplicate lên Employee List
 * @param {Object} emp object employee được dupplicate
 *
 * Author: Dũng (10/05/2023)
 */
function dupplicateEmpOnUpdate(emp) {
  formMetadata.value.isDupplicate = true;
  formMetadata.value.employeeDupplicate = emp;
  router.push("/DI/DIEmployee/create");
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
    // Nếu không có employee nào đang được chọn
    if (selectedAmountInPage.value == 0) {
      // Chọn tất cả
      selectedAmountInPage.value = rowList.value.length;
      for (const row of rowList.value) {
        row.selected = true;
        row.active = true;
        selectedEmpIds.value.push(row.emp.employeeId);
      }
    } else {
      // Nếu có ít nhất một employee đang được chọn
      // Hủy chọn tất cả
      selectedAmountInPage.value = 0;
      for (const row of rowList.value) {
        row.selected = false;
        row.active = false;
        const index = selectedEmpIds.value.indexOf(row.emp.employeeId);
        if (index > -1) selectedEmpIds.value.splice(index, 1);
      }
    }
    return;
  }

  if (type == "selected") {
    // Đổi trạng thái selected của row
    rowList.value[rowIndex].selected = !rowList.value[rowIndex].selected;

    // Nếu selected true thì thêm vào selectedEmpIds và bật active
    if (rowList.value[rowIndex].selected) {
      ++selectedAmountInPage.value;
      selectedEmpIds.value.push(rowList.value[rowIndex].emp.employeeId);
      rowList.value[rowIndex].active = true;
      // Tắt active của những ô khác mà không được selected
      for (const row of rowList.value) {
        if (
          row.emp.employeeId != rowList.value[rowIndex].emp.employeeId &&
          !row.selected
        )
          row.active = false;
      }
    } else {
      --selectedAmountInPage.value;
      // Nếu seleted của employee này false
      // Xóa khỏi selectedEmpIds và tắt active
      selectedEmpIds.value.splice(
        selectedEmpIds.value.indexOf(rowList.value[rowIndex].emp.employeeId),
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
          row.emp.employeeId != rowList.value[rowIndex].emp.employeeId
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
    selectedEmpIds.value = [];
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
 * @param {String} empId Id nhân viên
 *
 * Author: Dũng (08/05/2023)
 */
function deleteEmployeeOnClick(empId) {
  let empCode = "";
  cache.value.empDeleteId = empId;
  for (let index in rowList.value) {
    if (rowList.value[index].emp.employeeId == empId) {
      cache.value.empDeleteIndex = index;
      empCode = rowList.value[index].emp.employeeCode;
      break;
    }
  }
  showDeleteOneConfirmDialog(empCode);
}

/**
 * Sự kiện cập nhật trạng thái của object pagingData
 * @param {Object} newData giá trị mới
 *
 * Author: Dũng (08/05/2023)
 */
async function pagingDataOnUpdate(newData) {
  pagingData.value = newData;
  await loadEmployeeData();
}

/**
 * Chuyển sang trang paging kế tiếp
 *
 * Author: Dũng (08/05/2023)
 */
async function pagingNextPage() {
  pagingData.value.pageNumber += 1;
  await loadEmployeeData();
  // console.log("n next");
}

/**
 * Chuyển sang trang paging trước đó
 *
 * Author: Dũng (08/05/2023)
 */
async function pagingPrevPage() {
  pagingData.value.pageNumber -= 1;
  await loadEmployeeData();
  // console.log("p prev");
}

/**
 * Gọi API lấy dữ liệu nhân viên theo filter
 * Author: Dũng (08/05/2023)
 */
async function loadEmployeeData() {
  try {
    // Nếu dữ liệu đang được gọi thì return
    if (isLoadingData.value == true) return;
    selectedAmountInPage.value = 0;
    isLoadingData.value = true;
    await new Promise((resolve) => setTimeout(resolve, 800));

    // Gọi API filter nhân viên
    const response = await $axios.get($api.employee.filter, {
      params: {
        skip: pagingData.value.pageSize * (pagingData.value.pageNumber - 1),
        take: pagingData.value.pageSize,
        keySearch: cache.value.empSearchPattern,
      },
    });
    rowList.value = [];
    // console.log(response.data);
    if (response.data.filteredList) {
      for (const emp of response.data.filteredList) {
        // Chuyển đổi từ employee nhận từ server sang Class employee của frontend
        const empConverted = new Employee(emp);
        const isSelected = selectedEmpIds.value.includes(
          empConverted.employeeId
        );
        if (isSelected) ++selectedAmountInPage.value;
        rowList.value.push({
          active: isSelected,
          selected: isSelected,
          emp: empConverted,
        });
      }
    }
    // console.log(1);
    // console.log(rowList.value);
    // Số bản ghi ở trang hiện tại
    pagingData.value.curAmount = response.data.filteredList.length ?? 0;
    // Tổng số bản ghi
    pagingData.value.totalRecord = response.data.totalRecord ?? 0;
    // Số bản ghi ở trang hiện tại có trống hay không
    haveDataAfterCallApi.value = pagingData.value.totalRecord != 0;
    isLoadingData.value = false;
  } catch (error) {
    isLoadingData.value = false;
    handleApiErrorResponse(error);
  }
}

/**
 * Sự kiện cập nhật mảng rowList
 * @param {String} type kiểu update (thêm hay sửa)
 * @param {Object} data dữ liệu của employee mới
 * Author: Dũng (08/05/2023)
 */
async function employeeOnUpdate(type, data) {
  // console.log("Employee list updated");
  // console.log(type);
  // console.log(data);
  switch (type) {
    case "create":
      pagingData.value.totalRecord += 1;
      pagingData.value.curAmount += 1;
      rowList.value.unshift({
        active: false,
        selected: false,
        emp: data,
      });
      if (pagingData.value.curAmount > 2 * pagingData.value.pageSize) {
        await loadEmployeeData();
      }
      pushToast({
        type: "success",
        message: $message.employeeCreated,
        timeToLive: 1500,
      });
      break;
    case "edit":
      {
        let tempRow = null;
        let i = 0;
        while (i < rowList.value.length) {
          if (rowList.value[i].emp.employeeId == data.employeeId) {
            tempRow = rowList.value[i];
            tempRow.emp = data;
            break;
          }
          ++i;
        }
        rowList.value.splice(i, 1);
        await nextTick();
        rowList.value.splice(i, 0, tempRow);
        pushToast({
          type: "success",
          message: $message.employeeUpdated,
          timeToLive: 1500,
        });
      }
      break;
    default:
      break;
  }
  // await loadEmployeeData();
}

/**
 * Sự kiện click vào nút thêm
 * Author: Dũng (08/05/2023)
 */
function btnAddOnClick() {
  router.replace("/DI/DIEmployee/create");
}

// #endregion
</script>

<style
  scoped
  lang="css"
  src="../../../../css/components/views/category/employee/employee-list.css"
></style>

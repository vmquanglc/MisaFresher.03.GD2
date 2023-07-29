<template>
  <div class="tablebox">
    <table class="m-table">
      <thead>
        <tr>
          <th class="th1--sticky">
            <div class="align-center mw-40">
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
          <th>
            <div class="text-left mw-120">{{ lang.tableHeader.empCode }}</div>
          </th>
          <th>
            <div class="text-left mw-200">{{ lang.tableHeader.empName }}</div>
          </th>
          <th>
            <div class="text-left mw-90">{{ lang.tableHeader.empGender }}</div>
          </th>
          <th>
            <div class="text-center mw-120">{{ lang.tableHeader.empDob }}</div>
          </th>
          <th class="zindex--8">
            <div class="text-left mw-150">
              <span
                class="text"
                @mouseenter="thOnMouseEnter('isShowIdentityNumber')"
                @mouseleave="thOnMouseLeave('isShowIdentityNumber')"
              >
                {{ lang.tableHeader.empIdentity }}
              </span>
              <div class="th__tooltip" v-show="tooltip.isShowIdentityNumber">
                {{ lang.tableHeader.empIdentityTooltip }}
              </div>
            </div>
          </th>
          <th>
            <div class="text-left mw-180">{{ lang.tableHeader.empPos }}</div>
          </th>
          <th>
            <div class="text-left mw-240">{{ lang.tableHeader.empDepart }}</div>
          </th>
          <th>
            <div class="text-left mw-150">
              {{ lang.tableHeader.empBankAcc }}
            </div>
          </th>
          <th>
            <div class="text-left mw-150">
              {{ lang.tableHeader.empBankName }}
            </div>
          </th>
          <th class="zindex--8">
            <div class="text-left mw-200">
              <span
                class="text"
                @mouseenter="thOnMouseEnter('isShowBankPlace')"
                @mouseleave="thOnMouseLeave('isShowBankPlace')"
              >
                {{ lang.tableHeader.empBankBranch }}
              </span>
              <div class="th__tooltip" v-show="tooltip.isShowBankPlace">
                {{ lang.tableHeader.empBankBranchTooltip }}
              </div>
            </div>
          </th>
          <th class="thn--sticky">
            <div class="align-center mw-90">{{ lang.tableHeader.tool }}</div>
          </th>
        </tr>
      </thead>
      <tbody>
        <template v-if="isLoadingData">
          <tr v-for="i in Math.min(15, pagingData.pageSize)" :key="i">
            <td class="td1--sticky"><div class="loading-item"></div></td>
            <td v-for="j in 10" :key="j">
              <div class="loading-item"></div>
            </td>
            <td class="tdn--sticky">
              <div class="loading-item"></div>
            </td>
          </tr>
        </template>
        <template v-else>
          <tr
            v-for="({ active, selected, emp } = row, index) in rowList"
            :key="emp.employeeId"
            :class="{
              active: active,
            }"
            @click="trOnClick(index)"
            @dblclick="trOnDblclick(emp.employeeId)"
          >
            <td class="td1--sticky" @dblclick.stop>
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
            <td>
              <div class="text-left" v-tooltip="emp.employeeCode">
                {{ emp.employeeCode }}
              </div>
            </td>
            <td>
              <div class="text-left" v-tooltip="emp.employeeFullName">
                {{ emp.employeeFullName }}
              </div>
            </td>
            <td>
              <div class="text-left" v-tooltip="getGenderText(emp.gender)">
                {{ getGenderText(emp.gender) }}
              </div>
            </td>
            <td>
              <div class="text-center" v-tooltip="emp.dateOfBirth">
                {{ emp.dateOfBirth }}
              </div>
            </td>
            <td>
              <div class="text-left" v-tooltip="emp.identityNumber">
                {{ emp.identityNumber }}
              </div>
            </td>
            <td>
              <div class="text-left" v-tooltip="emp.positionName">
                {{ emp.positionName }}
              </div>
            </td>
            <td>
              <div class="text-left" v-tooltip="emp.departmentName">
                {{ emp.departmentName }}
              </div>
            </td>
            <td>
              <div class="text-left" v-tooltip="emp.bankAccount">
                {{ emp.bankAccount }}
              </div>
            </td>
            <td>
              <div class="text-left" v-tooltip="emp.bankName">
                {{ emp.bankName }}
              </div>
            </td>
            <td>
              <div class="text-left" v-tooltip="emp.bankBranch">
                {{ emp.bankBranch }}
              </div>
            </td>
            <td
              :class="[table.expandEmpId == emp.employeeId ? 'above' : '']"
              class="tdn--sticky"
              @dblclick.stop
            >
              <div class="t__optionbox align-center">
                <button
                  class="option__edit"
                  @click="btnEditOnClick(emp.employeeId)"
                >
                  {{ lang.table_items.edit }}
                </button>
                <button
                  class="btn__expand mi mi-16 mi-expand-down"
                  @click="btnExpandOnClick(emp.employeeId)"
                ></button>
                <ul
                  class="actions-list btn__expand"
                  :class="
                    (emp.employeeId ==
                      rowList[rowList.length - 1].emp.employeeId ||
                      emp.employeeId ==
                        rowList[rowList.length - 2].emp.employeeId) &&
                    rowList.length > 6
                      ? 'action-list--top'
                      : ''
                  "
                  v-show="table.expandEmpId == emp.employeeId"
                  @mouseleave="table.expandEmpId = ''"
                >
                  <li>
                    <div
                      class="li-data"
                      @click="dupplicateEmployeeOnClick(emp)"
                    >
                      {{ lang.table_items.dupplicate }}
                    </div>
                  </li>
                  <li>
                    <div
                      class="li-data"
                      @click="deleteEmployeeFunction(emp.employeeId)"
                    >
                      {{ lang.table_items.delete }}
                    </div>
                  </li>
                  <li>
                    <div class="li-data">{{ lang.table_items.stop }}</div>
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
  deleteEmployeeFunction: Function,
  pagingNextPage: Function,
  pagingPrevPage: Function,
  selectedEmpIds: Array,
  selectedAmountInPage: Number,
  haveDataAfterCallApi: Boolean,
});

const emits = defineEmits([
  "updatePagingData",
  "updateRowStatus",
  "updateDupplicateEmp",
]);

const table = ref({
  recordAmountOpen: false,
  recordAmountList: [10, 20, 30, 50, 100],
  expandEmpId: "",
});

const tooltip = ref({
  isShowIdentityNumber: false,
  isShowBankPlace: false,
});

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

function getGenderText(value) {
  if (value == 0) return lang.gender.male;
  else if (value == 1) return lang.gender.female;
  return lang.gender.other;
}
// #endregion

// #region handle event
/**
 * Sự kiện click vào nhân bản
 * Author: Dũng (03/06/2023)
 */
function dupplicateEmployeeOnClick(emp) {
  emits("updateDupplicateEmp", emp);
  // router.push(`/employee/dupplicate/${empId}`);
}

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
 * Mouse leave khỏi ô th có tooltip
 * @param {String} name tên tooltip của ô th
 * Author: Dũng (08/05/2023)
 */
function thOnMouseLeave(name) {
  tooltip.value[name] = false;
}

/**
 * Mouse enter vào ô th có tooltip
 * @param {String} name tên tooltip của ô th
 * Author: Dũng (08/05/2023)
 */
function thOnMouseEnter(name) {
  tooltip.value[name] = true;
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
  console.log("Prev");
}

/**
 * Click vào btn sửa nhân viên
 * @param {String} empId Id nhân viên
 * Author: Dũng (08/05/2023)
 */
function btnEditOnClick(empId) {
  router.push(`/DI/DIEmployee/${empId}`);
}

/**
 * Click vào nút mở rộng của một nhân viên
 * @param {String} empId Id nhân viên
 * Author: Dũng (08/05/2023)
 */
function btnExpandOnClick(empId) {
  if (table.value.expandEmpId == empId) {
    table.value.expandEmpId = "";
  } else {
    table.value.expandEmpId = empId;
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
 * @param {String} empId Id nhân viên
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
 * @param {String} empId Id nhân viên
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
 * @param {String} empId Id nhân viên
 * Author: Dũng (08/05/2023)
 */
function trOnDblclick(empId) {
  router.push(`/DI/DIEmployee/${empId}`);
}

// #endregion
</script>

<style
  lang="css"
  scoped
  src="../../../../css/components/views/category/employee/employee-table.css"
></style>

<template>
  <!-- Bắt đầu table danh sách nhân viên -->
  <div class="sticky-table">
    <div v-if="selectedEmployeeIdList.length != 0" class="mass-action">
      <div class="contain-selection">
        <span>
          {{ this.$t("Selection.Selected") }}
          <b>{{ selectedEmployeeIdList.length }}</b>
        </span>

        <div class="cancel-selected" @click="selectedList = {}">
          {{ this.$t("Selection.UnSelect") }}
        </div>
      </div>
      <div class="delete-selected">
        <button class="link-button" @click="deleteAllEmployeeOnCurrentPage">
          {{ this.$t("Button.Delete") }}
        </button>
      </div>
    </div>

    <div class="icon-empty" v-if="employeeList && employeeList.length == 0">
      <img
        src="../../assets/img/bg_report_nodat.svg"
        :alt="this.$t('NoData')"
      />
      {{ this.$t("NoData") }}
    </div>

    <table class="table">
      <thead>
        <tr>
          <th>
            <MSCheckBox
              :isTableHead="!isAllRowsSelected"
              :value="
                isAllRowsSelected || this.selectedEmployeeIdList.length != 0
              "
              @onChangeValue="(isChecked) => toggleAllEmployee(isChecked)"
            ></MSCheckBox>
          </th>
          <th
            v-for="tableHeader in tableHeaders"
            :key="tableHeader.text"
            @mousemove="tableHeaderOnHover($event, tableHeader)"
            @mouseout="tableHeaderOnUnhover(tableHeader)"
          >
            {{ tableHeader.text }}
          </th>
        </tr>
      </thead>

      <tbody v-if="employeeList == null">
        <tr v-for="row in 20" :key="row">
          <td>
            <div
              :style="{ width: '16px', height: '16px' }"
              class="td-loading"
            ></div>
          </td>
          <td v-for="i in 11" :key="i">
            <div class="td-loading"></div>
          </td>
        </tr>
      </tbody>

      <tbody v-else>
        <tr v-for="employee in employeeList" :key="employee.EmployeeId">
          <td>
            <MSCheckBox
              :value="!!selectedList[employee.EmployeeId]"
              @onChangeValue="
                (isChecked) => {
                  setIdIntoSelectedList(employee.EmployeeId, isChecked);
                }
              "
            ></MSCheckBox>
          </td>
          <td
            v-for="x in [
              employee.EmployeeCode,
              employee.FullName,
              employee.Gender == this.$msEnum.Gender.Male
                ? this.$t('Gender.Male')
                : employee.Gender == this.$msEnum.Gender.Female
                ? this.$t('Gender.Female')
                : this.$t('Gender.OtherGender'),
              this.$msDateFormater.convertIsoToDDMMYYY(employee.DateOfBirth),
              employee.IdentityNumber,
              employee.PositionName,
              employee.DepartmentName,
              employee.BankAccountNumber,
              employee.NameOfBank,
              employee.BankAccountBranch,
            ]"
            :key="x"
            @dblclick="employeeListOnDoubleClick(employee)"
          >
            <span
              v-if="x?.length > 38"
              class="table-data"
              @mousemove="(event) => $msEmitter.emit('showTooltip', event, x)"
              @mouseout="$msEmitter.emit('hideTooltip')"
            >
              {{ x.substring(0, 38) + "..." }}
            </span>
            <span v-else class="table-data">
              {{ x }}
            </span>
          </td>

          <td>
            <button
              class="link-button"
              @click="employeeListOnDoubleClick(employee)"
            >
              {{ this.$t("Button.Edit") }}
            </button>
            <div
              class="icon-container"
              @click="(event) => toggleMenuFeature(event, employee)"
            >
              <div class="icon-triangle-down-black"></div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
  <div class="table__footer">
    <div class="table__footer--left">
      <div class="table__numrows">
        {{
          $t("EmployeeList.TotalNumberOfRecordsIs", [
            TotalRecord ? numeralFormat(TotalRecord, "0,0") : 0,
          ])
        }}
      </div>
    </div>
    <div class="table__footer--right">
      <div class="table__choose-rowcount">
        <p>
          {{ $t("EmployeeList.NumberOfRecordOutOfPageIs") }}
        </p>

        <div class="table__maxrow">
          <MSCombobox
            :items="[10, 20, 30, 50, 100]"
            v-model:value="pageSizeSelected"
            :isSmall="true"
            inputType="number"
            :max="$msEnum.NumberOfEmployeesPerPage.Max"
            :min="$msEnum.NumberOfEmployeesPerPage.Min"
          ></MSCombobox>
        </div>
      </div>
      <div class="table__recordnumber">
        {{
          $t("ReceiptList.FromXToYRecords", [
            pageNumberSelected
              ? pageNumberSelected * pageSizeSelected > TotalRecord
                ? TotalRecord
                : pageNumberSelected * pageSizeSelected
              : 0,
            TotalRecord ? numeralFormat(TotalRecord, "0,0") : 0,
          ])
        }}
      </div>

      <div
        class="icon-container"
        @click="if (1 < pageNumberSelected && employeeList) previousPage();"
      >
        <div
          v-if="1 == pageNumberSelected || !employeeList"
          class="icon-arrow-left-gray"
          :class="{ 'cursor-not-allowed': employeeList }"
        ></div>
        <div v-else class="icon-arrow-left-black"></div>
      </div>
      <div
        class="icon-container"
        @click="if (TotalPage > pageNumberSelected && employeeList) nextPage();"
      >
        <div
          v-if="
            TotalPage == pageNumberSelected ||
            !employeeList ||
            (pageNumberSelected
              ? pageNumberSelected * pageSizeSelected > TotalRecord
                ? TotalRecord
                : pageNumberSelected * pageSizeSelected
              : 0) == TotalRecord
          "
          class="icon-arrow-right-gray"
          :class="{ 'cursor-not-allowed': employeeList }"
        ></div>
        <div v-else class="icon-arrow-right-black"></div>
      </div>
    </div>
  </div>
  <!-- Kết thúc table -->
  <EmployeeMenuFeature></EmployeeMenuFeature>

  <MSDialog
    v-if="dialog.isShowDialog"
    :title="dialog.title"
    :items="dialog.items"
    :contentNo="dialog.contentNo"
    :contentYes="dialog.contentYes"
    :contentCancel="dialog.contentCancel"
    :dialogType="dialog.dialogType"
    :isWarning="dialog.isWarning"
  ></MSDialog>
</template>

<script>
import mixin from "../../js/views/employee/EmployeeList.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/views/employee/EmployeeList.scss" lang="scss">
</style>
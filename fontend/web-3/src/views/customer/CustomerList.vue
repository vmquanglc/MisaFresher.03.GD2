<template>
  <div class="head">
    <div class="left">
      <p>
        {{ $t("Selection.Selected") }}
        <span>{{ selectedCustomerIdList.length }}</span>
      </p>
      <button
        class="cancel-select link-button"
        @click="selectedList = {}"
        :disabled="selectedCustomerIdList.length == 0"
      >
        {{ $t("Selection.UnSelect") }}
      </button>
      <button
        class="link-button"
        :disabled="selectedCustomerIdList.length == 0"
        @click="deleteSelected()"
      >
        {{ $t("Button.Delete") }}
      </button>
    </div>
    <div class="right">
      <MSInputSearch
        v-model:value="inputSearchValue"
        :placeholder="this.$t('Placeholder.InputSearch')"
        @inputSearchOnSubmit="loadCustomerList()"
      ></MSInputSearch>
      <div
        class="icon-container"
        @click="loadCustomerList()"
        @mousemove="
          (event) =>
            $msEmitter.emit('showTooltip', event, this.$t('Button.ReloadList'))
        "
        @mouseout="$msEmitter.emit('hideTooltip')"
      >
        <div class="icon-reload"></div>
      </div>
      <div
        class="icon-container"
        @mousemove="
          (event) =>
            $msEmitter.emit('showTooltip', event, this.$t('Button.ExportList'))
        "
        @mouseout="$msEmitter.emit('hideTooltip')"
        @click="exportOnclick"
      >
        <div class="icon-excel"></div>
      </div>

      <div
        class="icon-container"
        @mousemove="
          (event) =>
            $msEmitter.emit('showTooltip', event, this.$t('Button.Setting'))
        "
        @mouseout="$msEmitter.emit('hideTooltip')"
        @click="notSupported()"
      >
        <div class="icon-setting-gray"></div>
      </div>

      <button class="extra-button" @click="notSupported()">
        {{ this.$t("Button.Utilities") }}
      </button>
      <button class="main-button" @click="addCustomer()">
        {{ this.$t("Button.Add") }}
      </button>
    </div>
  </div>
  <div class="body">
    <div class="sticky-table">
      <table class="table">
        <thead>
          <tr>
            <th>
              <MSCheckBox
                :isTableHead="!isAllRowsSelected"
                :value="selectedCustomerIdList.length != 0"
                @onChangeValue="(isChecked) => toggleAllRows(isChecked)"
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

        <tbody v-if="customerList == null">
          <tr v-for="row in 20" :key="row">
            <td>
              <div
                :style="{ width: '16px', height: '16px' }"
                class="td-loading"
              ></div>
            </td>
            <td v-for="i in 7" :key="i">
              <div class="td-loading"></div>
            </td>
          </tr>
        </tbody>

        <tbody v-else>
          <tr v-for="customer in customerList" :key="customer.CustomerId">
            <td>
              <MSCheckBox
                :value="!!selectedList[customer.CustomerId]"
                @onChangeValue="
                  (isChecked) => {
                    setIdIntoSelectedList(customer.CustomerId, isChecked);
                  }
                "
              ></MSCheckBox>
            </td>
            <td
              v-for="x in getDataRow(customer)"
              :key="x"
              @dblclick="customerListOnDoubleClick(customer)"
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
              <div class="flex">
                <button
                  class="link-button"
                  @click="customerListOnDoubleClick(customer)"
                >
                  {{ this.$t("Button.Edit") }}
                </button>
                <div
                  class="icon-container"
                  @click="(event) => toggleMenuFeature(event, customer)"
                >
                  <div class="icon-triangle-down-black"></div>
                </div>
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
            $t("CustomerList.TotalNumberOfRecordsIs", [
              TotalRecord ? numeralFormat(TotalRecord, "0,0") : 0,
            ])
          }}
        </div>
      </div>
      <div class="table__footer--right">
        <div class="table__choose-rowcount">
          <p>
            {{ $t("CustomerList.NumberOfRecordOutOfPageIs") }}
          </p>

          <div class="table__maxrow">
            <MSCombobox
              :items="[10, 20, 30, 50, 100]"
              v-model:value="pageSizeSelected"
              :isSmall="true"
              inputType="number"
              :max="999"
              :min="10"
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
          @click="if (1 < pageNumberSelected && customerList) previousPage();"
        >
          <div
            v-if="1 == pageNumberSelected || !customerList"
            class="icon-arrow-left-gray"
            :class="{ 'cursor-not-allowed': customerList }"
          ></div>
          <div v-else class="icon-arrow-left-black"></div>
        </div>
        <div
          class="icon-container"
          @click="
            if (
              !(
                TotalPage == pageNumberSelected ||
                !customerList ||
                (pageNumberSelected
                  ? pageNumberSelected * pageSizeSelected > TotalRecord
                    ? TotalRecord
                    : pageNumberSelected * pageSizeSelected
                  : 0) == TotalRecord
              )
            )
              nextPage();
          "
        >
          <div
            v-if="
              TotalPage == pageNumberSelected ||
              !customerList ||
              (pageNumberSelected
                ? pageNumberSelected * pageSizeSelected > TotalRecord
                  ? TotalRecord
                  : pageNumberSelected * pageSizeSelected
                : 0) == TotalRecord
            "
            class="icon-arrow-right-gray"
            :class="{ 'cursor-not-allowed': customerList }"
          ></div>
          <div v-else class="icon-arrow-right-black"></div>
        </div>
      </div>
    </div>
  </div>

  <router-view :key="customerFormKey" name="CustomerRouterView"></router-view>

  <CustomerMenuFeature></CustomerMenuFeature>

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
import CustomerMenuFeature from "./CustomerMenuFeature.vue";
import mixin from "../../js/views/customer/CustomerList.js";
export default {
  mixins: [mixin],
  components: {
    CustomerMenuFeature,
  },
};
</script>


<style src="../../style/views/customer/CustomerList.scss" lang="scss" scoped>
</style>

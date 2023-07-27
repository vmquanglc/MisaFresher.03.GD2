<template>
  <div class="total__head">
    <div class="block">
      <div class="name">
        {{ $t("Cash.TotalRevenueFromTheBeginningOfTheYearToThePresent") }}
      </div>
      <div class="value">300</div>
    </div>
    <div class="block">
      <div class="name">
        {{ $t("Cash.TotalExpenditureAtTheBeginningOfTheYearToPresent") }}
      </div>
      <div class="value">2.840.300</div>
    </div>
    <div class="block">
      <div class="name">{{ $t("Cash.CurrentFundBalance") }}</div>
      <div class="value">(21.340.000)</div>
    </div>
  </div>
  <div class="head">
    <div class="left">
      <p>
        {{ $t("Cash.Selected") }}
        <span>{{ selectedReceiptIdList.length }}</span>
      </p>
      <button
        class="cancel-select link-button"
        @click="selectedList = {}"
        :disabled="selectedReceiptIdList.length == 0"
      >
        {{ $t("Cash.UnSelect") }}
      </button>
      <button
        class="link-button"
        :disabled="selectedReceiptIdList.length == 0"
        @click="deleteSelected()"
      >
        {{ $t("Button.Delete") }}
      </button>
    </div>
    <div class="right">
      <MSInputSearch
        v-model:value="inputSearchValue"
        :placeholder="this.$t('Placeholder.InputSearch')"
        @inputSearchOnSubmit="loadReceiptList()"
      ></MSInputSearch>
      <div
        class="icon-container"
        @mousemove="
          (event) =>
            $msEmitter.emit('showTooltip', event, this.$t('Button.ReloadList'))
        "
        @mouseout="$msEmitter.emit('hideTooltip')"
      >
        <div class="icon-reload" @click="loadReceiptList()"></div>
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
        {{ $t("Button.Utilities") }}
      </button>
      <button class="main-button" @click="addReceipt()">
        {{ $t("Button.Add") }}
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
                :value="selectedReceiptIdList.length != 0"
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

        <tbody v-if="receiptList == null">
          <tr v-for="row in 20" :key="row">
            <td>
              <div
                :style="{ width: '16px', height: '16px' }"
                class="td-loading"
              ></div>
            </td>
            <td v-for="i in this.columnList.length + 1" :key="i">
              <div class="td-loading"></div>
            </td>
          </tr>
        </tbody>

        <tbody v-else>
          <tr
            v-for="receipt in receiptList"
            :key="receipt.ReceiptId"
            :style="{ color: !receipt.IsRecorded ? '#14A200' : 'black' }"
          >
            <td>
              <MSCheckBox
                :value="!!selectedList[receipt.ReceiptId]"
                @onChangeValue="
                  (isChecked) => {
                    setIdIntoSelectedList(receipt.ReceiptId, isChecked);
                  }
                "
              ></MSCheckBox>
            </td>
            <td
              v-for="x in getDataRow(receipt)"
              :key="x"
              @dblclick="receiptListOnDoubleClick(receipt)"
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
                  @click="receiptListOnDoubleClick(receipt)"
                >
                  {{ this.$t("Button.View") }}
                </button>
                <div
                  class="icon-container"
                  @click="(event) => toggleMenuFeature(event, receipt)"
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
            $t("ReceiptList.TotalNumberOfRecordsIs", [
              TotalRecord ? numeralFormat(TotalRecord, "0,0") : 0,
            ])
          }}
        </div>
      </div>
      <div class="table__footer--right">
        <div class="table__choose-rowcount">
          <p>
            {{ $t("ReceiptList.NumberOfRecordOutOfPageIs") }}
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
          @click="if (1 < pageNumberSelected && receiptList) previousPage();"
        >
          <div
            v-if="1 == pageNumberSelected || !receiptList"
            class="icon-arrow-left-gray"
            :class="{ 'cursor-not-allowed': receiptList }"
          ></div>
          <div v-else class="icon-arrow-left-black"></div>
        </div>
        <div
          class="icon-container"
          @click="
            if (
              !(
                TotalPage == pageNumberSelected ||
                !receiptList ||
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
              !receiptList ||
              (pageNumberSelected
                ? pageNumberSelected * pageSizeSelected > TotalRecord
                  ? TotalRecord
                  : pageNumberSelected * pageSizeSelected
                : 0) == TotalRecord
            "
            class="icon-arrow-right-gray"
            :class="{ 'cursor-not-allowed': receiptList }"
          ></div>
          <div v-else class="icon-arrow-right-black"></div>
        </div>
      </div>
    </div>
  </div>

  <router-view :key="receiptFormKey" name="ReceiptRouterView"></router-view>

  <ReceiptMenuFeature></ReceiptMenuFeature>
</template>
  
  <script>
import ReceiptMenuFeature from "./ReceiptMenuFeature.vue";
import mixin from "../../js/views/cash/ReceiptList.js";
export default {
  mixins: [mixin],
  components: {
    ReceiptMenuFeature,
  },
};
</script>
  
  
  <style src="../../style/views/cash/ReceiptList.scss" lang="scss" scoped>
</style>
  
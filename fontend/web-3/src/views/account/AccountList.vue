<template>
  <div class="head">
    <div class="left">
      <MSInputSearch
        v-model:value="inputSearchValue"
        :placeholder="this.$t('Placeholder.InputSearch')"
        @inputSearchOnSubmit="loadAccountList()"
      ></MSInputSearch>
    </div>
    <div class="right">
      <button
        v-if="!IsExpandedAll"
        class="link-button"
        @click="expandAllRows()"
      >
        {{ this.$t("AccountList.Expand") }}
      </button>

      <button v-else class="link-button" @click="collapseAllRows()">
        {{ this.$t("AccountList.Collapse") }}
      </button>

      <div
        class="icon-container"
        @click="loadAccountList()"
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

      <button class="extra-button" @click="notSupported()">
        {{ this.$t("Button.Utilities") }}
      </button>
      <button class="main-button" @click="addAccount()">
        {{ this.$t("Button.Add") }}
      </button>
    </div>
  </div>
  <div class="body">
    <div class="sticky-table">
      <table class="table">
        <thead>
          <tr>
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

        <tbody v-if="accountList == null">
          <tr v-for="row in 20" :key="row">
            <td v-for="i in columnList.length + 1" :key="i">
              <div class="td-loading"></div>
            </td>
          </tr>
        </tbody>

        <tbody v-else>
          <tr
            v-for="account in accountList"
            :key="account.AccountId"
            :class="`${
              selectedId == account.AccountId ? '--active-row' : ''
            } rank-${account.Rank} ${account.IsParent ? '--parent' : ''}`"
          >
            <td
              v-for="x in getDataRow(account)"
              :key="x"
              @dblclick="accountListOnDoubleClick(account)"
              @click="selectedId = account.AccountId"
            >
              <div
                v-if="account.IsParent && account[columnList[0]] == x"
                class="collapse-icon"
              >
                <div
                  :class="`${
                    account.IsExpanded
                      ? 'collapse-icon--minus'
                      : 'collapse-icon--plus'
                  }`"
                  @click="
                    () => {
                      if (account.IsExpanded) {
                        collapseRow(account);
                        account.IsExpanded = false;
                      } else {
                        expandRow(account);
                        account.IsExpanded = true;
                      }
                    }
                  "
                ></div>
              </div>
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
                  @click="accountListOnDoubleClick(account)"
                >
                  {{ this.$t("Button.Edit") }}
                </button>
                <div
                  class="icon-container"
                  @click="(event) => toggleMenuFeature(event, account)"
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
            $t("AccountList.TotalNumberOfRecordsIs", [
              TotalRecord ? numeralFormat(TotalRecord, "0,0") : 0,
            ])
          }}
        </div>
      </div>
      <div class="table__footer--right">
        <div class="table__choose-rowcount">
          <p>
            {{ $t("AccountList.NumberOfRecordOutOfPageIs") }}
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
          @click="if (1 < pageNumberSelected && accountList) previousPage();"
        >
          <div
            v-if="1 == pageNumberSelected || !accountList"
            class="icon-arrow-left-gray"
            :class="{ 'cursor-not-allowed': accountList }"
          ></div>
          <div v-else class="icon-arrow-left-black"></div>
        </div>
        <div
          class="icon-container"
          @click="
            if (
              !(
                TotalPage == pageNumberSelected ||
                !accountList ||
                (pageNumberSelected
                  ? pageNumberSelected * pageSizeSelected > TotalRecord
                    ? TotalRecord
                    : pageNumberSelected * pageSizeSelected
                  : 0) == TotalRecord
              )
            ) {
              nextPage();
            }
          "
        >
          <div
            v-if="
              TotalPage == pageNumberSelected ||
              !accountList ||
              (pageNumberSelected
                ? pageNumberSelected * pageSizeSelected > TotalRecord
                  ? TotalRecord
                  : pageNumberSelected * pageSizeSelected
                : 0) == TotalRecord
            "
            class="icon-arrow-right-gray"
            :class="{ 'cursor-not-allowed': accountList }"
          ></div>
          <div v-else class="icon-arrow-right-black"></div>
        </div>
      </div>
    </div>
  </div>

  <router-view :key="accountFormKey" name="AccountRouterView"></router-view>

  <AccountMenuFeature></AccountMenuFeature>

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
import AccountMenuFeature from "./AccountMenuFeature.vue";
import mixin from "../../js/views/account/AccountList.js";
export default {
  mixins: [mixin],
  components: {
    AccountMenuFeature,
  },
};
</script>
  
  
  <style src="../../style/views/account/AccountList.scss" lang="scss" scoped>
</style>
  
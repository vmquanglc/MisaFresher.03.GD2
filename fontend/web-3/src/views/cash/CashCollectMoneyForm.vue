<template>
  <form class="collect-money">
    <div class="collect-money__header">
      <div class="left">
        <div class="icon-container">
          <div class="icon-clock-black"></div>
        </div>

        <div
          v-if="receipt.ReceiptCode?.length > 10"
          class="title"
          @mousemove="
            (event) =>
              $msEmitter.emit('showTooltip', event, receipt.ReceiptCode)
          "
          @mouseout="$msEmitter.emit('hideTooltip')"
        >
          {{ $t("Receipt.Receipts") }}
          {{ receipt.ReceiptCode.substr(0, 10) + "..." }}
        </div>
        <div v-else class="title">
          {{ $t("Cash.Receipts") }} {{ receipt.ReceiptCode }}
        </div>

        <div class="form-type">
          <MSInputCombobox
            :listItems="ReceiptTypeList"
            propText="value"
            propValue="id"
            v-model:id="receipt.ReceiptType"
            :readonly="readonlyHard || readonlySoft"
          ></MSInputCombobox>
        </div>
      </div>
      <div class="right">
        <div
          class="icon-container"
          @mousemove="
            (event) =>
              $msEmitter.emit(
                'showTooltip',
                event,
                this.$t('Button.Close') + ' (ESC)'
              )
          "
          @mouseout="$msEmitter.emit('hideTooltip')"
          @click="closeForm()"
        >
          <div class="icon-x-black"></div>
        </div>
      </div>
    </div>

    <div class="collect-money__body">
      <div class="left">
        <MSInputCombobox
          :title="$t('Cash.CustomerCode')"
          propText="CustomerCode"
          ref="firstInput"
          propValue="CustomerId"
          :listCols="[
            'CustomerCode',
            'FullName',
            'TaxCode',
            'Address',
            'PhoneNumber',
          ]"
          :listTitle="[
            $t('Cash.Object'),
            $t('Cash.ObjectName'),
            $t('Cash.TaxCode'),
            $t('Cash.Address'),
            $t('Cash.PhoneNumber'),
          ]"
          v-model:id="receipt.CustomerId"
          :apiPaging="$msApi.CustomerApi.Paging"
          :apiGetById="$msApi.CustomerApi.Get"
          @valueOnChange="customerOnSelect"
          :readonly="readonlyHard || readonlySoft"
        ></MSInputCombobox>

        <div class="icon-container">
          <div class="icon-money-black"></div>
        </div>

        <MSInputText
          :title="$t('Cash.CustomerName')"
          v-model:value="receipt.CustomerName"
          inputType="text"
          :isValidating="isValidating"
          v-model:isValid="inputValidation.CustomerName"
          ref="CustomerName"
          :readonly="readonlyHard"
        ></MSInputText>

        <MSInputText
          :title="$t('Cash.Payer')"
          v-model:value="receipt.Payer"
          inputType="text"
          :isValidating="isValidating"
          v-model:isValid="inputValidation.Payer"
          ref="Payer"
          :readonly="readonlyHard"
        ></MSInputText>

        <MSInputText
          :title="$t('ReceiptList.Address')"
          v-model:value="receipt.Address"
          inputType="text"
          :isValidating="isValidating"
          v-model:isValid="inputValidation.Address"
          ref="Address"
          :readonly="readonlyHard"
        ></MSInputText>

        <MSInputCombobox
          :title="$t('Cash.Employee')"
          propText="EmployeeCode"
          propValue="EmployeeId"
          :listCols="[
            'EmployeeCode',
            'FullName',
            'DepartmentName',
            'PhoneNumber',
          ]"
          :listTitle="[
            $t('EmployeeList.EmployeeCode'),
            $t('EmployeeList.FullName'),
            $t('EmployeeList.DepartmentName'),
            $t('EmployeeList.PhoneNumber'),
          ]"
          v-model:id="receipt.EmployeeId"
          :apiPaging="$msApi.EmployeeApi.Paging"
          :apiGetById="$msApi.EmployeeApi.Get"
          :readonly="readonlyHard"
        ></MSInputCombobox>

        <MSInputText
          :title="$t('Cash.Reason')"
          v-model:value="receipt.Reason"
          inputType="text"
          :isValidating="isValidating"
          v-model:isValid="inputValidation.Reason"
          ref="Reason"
          :readonly="readonlyHard"
        ></MSInputText>

        <MSInputText
          :title="$t('Cash.Rejoin')"
          v-model:value="receipt.Amount"
          inputType="money"
          textAlign="right"
          :placeholder="$t('Cash.Amount')"
          :readonly="readonlyHard || readonlySoft"
        ></MSInputText>

        <div class="root-invoice">{{ $t("Cash.OriginalDocuments") }}</div>
      </div>

      <div class="middle">
        <MSInputDate
          :title="$t('ReceiptForm.AccountingDate')"
          v-model:value="receipt.AccountingDate"
          :readonly="readonlyHard || readonlySoft"
          ref="AccountingDate"
        ></MSInputDate>

        <MSInputDate
          :title="$t('ReceiptForm.ReceiptDate')"
          v-model:value="receipt.ReceiptDate"
          :readonly="readonlyHard || readonlySoft"
          ref="ReceiptDate"
        ></MSInputDate>

        <MSInputText
          :title="$t('ReceiptForm.ReceiptCode')"
          v-model:value="receipt.ReceiptCode"
          inputType="text"
          :isRequired="true"
          :isValidating="isValidating"
          v-model:isValid="inputValidation.ReceiptCode"
          ref="ReceiptCode"
          :readonly="readonlyHard || readonlySoft"
        ></MSInputText>
      </div>

      <div class="right">
        <div class="total-money">
          {{ $t("Cash.TotalMoney") }}
        </div>
        <h2
          :class="`${
            totalMoney?.indexOf('(') == -1 ? 'credit-money' : 'debit-money'
          }`"
        >
          {{ totalMoney }}
        </h2>
      </div>
    </div>

    <div class="accounting__body">
      <div class="title">
        {{ $t("Cash.Accounting") }}
      </div>

      <div class="sticky-table">
        <table>
          <tr>
            <th>#</th>
            <th>{{ $t("Cash.Explain") }}</th>
            <th>{{ $t("Cash.DebtAccount") }}</th>
            <th>{{ $t("Cash.CreditAccount") }}</th>
            <th>{{ $t("Cash.Object") }}</th>
            <th>{{ $t("Cash.ObjectName") }}</th>
            <th>{{ $t("Cash.AmountOfMoney") }}</th>
            <th></th>
          </tr>
          <tr
            v-for="(bookKeeping, index) in receipt.BookKeepings"
            :key="bookKeeping"
          >
            <td>{{ index + 1 }}</td>
            <td>
              <div class="inline-block">
                <MSInputText
                  v-model:value="bookKeeping.Note"
                  :readonly="readonlyHard"
                ></MSInputText>
              </div>
            </td>
            <td>
              <div class="inline-block">
                <MSInputCombobox
                  propText="AccountCode"
                  propValue="AccountId"
                  :listCols="['AccountCode', 'NameVi']"
                  :listTitle="[
                    $t('AccountList.AccountCode'),
                    $t('AccountList.NameVi'),
                  ]"
                  v-model:id="bookKeeping.DebitAccountId"
                  :apiPaging="$msApi.AccountApi.PagingCombobox"
                  :apiGetById="$msApi.AccountApi.Get"
                  :readonly="readonlyHard || readonlySoft"
                ></MSInputCombobox>
              </div>
            </td>

            <td>
              <div class="inline-block">
                <MSInputCombobox
                  propText="AccountCode"
                  propValue="AccountId"
                  :listCols="['AccountCode', 'NameVi']"
                  :listTitle="[
                    $t('AccountList.AccountCode'),
                    $t('AccountList.NameVi'),
                  ]"
                  v-model:id="bookKeeping.CollectAccountId"
                  :apiPaging="$msApi.AccountApi.PagingCombobox"
                  :apiGetById="$msApi.AccountApi.Get"
                  :readonly="readonlyHard || readonlySoft"
                ></MSInputCombobox>
              </div>
            </td>

            <td>
              <div class="inline-block">
                <MSInputText
                  v-model:value="bookKeeping.ObjectCode"
                  :readonly="readonlyHard || readonlySoft"
                ></MSInputText>
              </div>
            </td>

            <td>
              <div class="inline-block">
                <MSInputText
                  v-model:value="bookKeeping.ObjectName"
                  :readonly="readonlyHard || readonlySoft"
                ></MSInputText>
              </div>
            </td>

            <td>
              <div class="inline-block">
                <MSInputText
                  v-model:value="bookKeeping.AmountOfMoney"
                  inputType="money"
                  textAlign="right"
                  :readonly="readonlyHard || readonlySoft"
                ></MSInputText>
              </div>
            </td>

            <td>
              <div class="icon-container" @click="removeCurrentRow(index)">
                <div class="icon-trash-black"></div>
              </div>
            </td>
          </tr>
          <tr>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td></td>
            <td
              :class="`${
                totalMoney?.indexOf('(') == -1 ? 'credit-money' : 'debit-money'
              }`"
            >
              {{ totalMoney }}
            </td>
            <td></td>
          </tr>
        </table>
      </div>

      <div class="button-container">
        <button
          type="button"
          class="extra-button"
          @click="addRow()"
          :disabled="readonlyHard || readonlySoft"
        >
          {{ $t("Cash.AddLine") }}
        </button>
        <button
          type="button"
          class="extra-button"
          @click="deleteAllRows()"
          :disabled="readonlyHard || readonlySoft"
        >
          {{ $t("Cash.DeleteAllLines") }}
        </button>
      </div>

      <div class="upload-file">
        <div class="top">
          <div class="icon-container">
            <div class="icon-attach-file"></div>
          </div>

          <div class="title">{{ $t("Cash.Attach") }}</div>

          <p>{{ $t("Cash.MaximumCapacity", ["5MB"]) }}</p>
        </div>

        <div class="input-upload">
          <input type="file" accept="" multiple="multiple" />

          <span class="text-input">{{
            $t("Cash.DragDropFilesHereOrClickHere")
          }}</span>
        </div>
      </div>
    </div>

    <div class="collect-money__footer">
      <div class="left">
        <button class="extra-button" type="button">
          {{ $t("Button.Cancel") }}
        </button>
      </div>
      <div v-if="!isRecorded && readonlyHard" class="right">
        <button class="extra-button" type="button" @click="edit()">
          {{ $t("Button.Edit") }}
        </button>
        <button class="main-button" type="button" @click="setRecorded()">
          {{ $t("Cash.SetRecorded") }}
        </button>
      </div>

      <div
        v-else-if="!isRecorded && !readonlyHard && !readonlySoft"
        class="right"
      >
        <button class="extra-button" type="button" @click="save()">
          {{ $t("Button.Save") }}
        </button>
        <button class="main-button" type="button" @click="saveAndAdd()">
          {{ $t("Button.SaveAndAdd") }}
        </button>
      </div>

      <div v-else-if="isRecorded && readonlyHard" class="right">
        <button class="extra-button" type="button" @click="fastEdit()">
          {{ $t("Cash.FastEdit") }}
        </button>
        <button class="main-button" type="button" @click="unRecorded()">
          {{ $t("Cash.UnRecorded") }}
        </button>
      </div>

      <div v-else class="right">
        <button class="extra-button" type="button" @click="save()">
          {{ $t("Button.Save") }}
        </button>
        <button class="main-button" type="button" @click="saveAndAdd()">
          {{ $t("Button.SaveAndAdd") }}
        </button>
      </div>
    </div>
  </form>
</template>


<script>
import mixin from "../../js/views/cash/CashCollectMoneyForm.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/views/cash/CashCollectMoneyForm.scss" lang="scss" scoped>
</style>
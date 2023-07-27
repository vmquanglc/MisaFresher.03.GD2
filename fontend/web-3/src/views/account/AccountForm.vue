<template>
  <div class="form__account-form">
    <form class="form">
      <div
        class="resize"
        @click="
          () => {
            if (formWidth == '50vw') {
              formWidth = '99.2vw';
            } else {
              formWidth = '50vw';
            }
          }
        "
        @mousemove="
          (event) =>
            $msEmitter.emit(
              'showTooltip',
              event,
              formWidth == '50vw' ? 'Mở rộng' : 'Thu hẹp'
            )
        "
        @mouseout="$msEmitter.emit('hideTooltip')"
      >
        <div class="icon-container">
          <div
            :class="`icon-arrow-${
              formWidth == '50vw' ? 'left' : 'right'
            }-black`"
          ></div>
        </div>
      </div>

      <div class="form__header">
        <p class="title">{{ this.title }}</p>

        <div
          class="icon-container"
          @click="closeForm()"
          @mousemove="
            (event) => $msEmitter.emit('showTooltip', event, 'Thoát (ESC)')
          "
          @mouseout="$msEmitter.emit('hideTooltip')"
        >
          <div class="icon-x-black"></div>
        </div>
      </div>

      <div class="form__body">
        <MSInputText
          :title="this.$t('AccountForm.AccountCode')"
          :isFirstInput="true"
          :isRequired="true"
          :isValidating="isValidating"
          v-model:isValid="inputValidation.AccountCode"
          v-model:value="account.AccountCode"
          :onDoneTyping="checkDuplicateCode"
          :readonly="isReadonly"
          ref="AccountCode"
        ></MSInputText>
        <div class="flex">
          <MSInputText
            :title="this.$t('AccountForm.NameVi')"
            :isRequired="true"
            :isValidating="isValidating"
            v-model:isValid="inputValidation.NameVi"
            v-model:value="account.NameVi"
            :readonly="isReadonly"
            ref="NameVi"
          ></MSInputText>

          <MSInputText
            :title="this.$t('AccountForm.NameEn')"
            v-model:value="account.NameEn"
            :readonly="isReadonly"
            ref="NameEn"
          ></MSInputText>
        </div>

        <div class="flex">
          <MSInputCombobox
            :title="this.$t('AccountForm.AccountSynthetic')"
            propText="AccountCode"
            propValue="AccountId"
            v-model:id="account.AccountSyntheticId"
            :apiPaging="$msApi.AccountApi.PagingCombobox"
            :apiGetById="$msApi.AccountApi.Get"
            @valueOnChange="AccountSyntheticOnSelect"
            :readonly="isReadonly"
          ></MSInputCombobox>
          <MSInputCombobox
            :title="this.$t('AccountForm.AccountProperty')"
            :isRequired="true"
            :isValidating="isValidating"
            v-model:isValid="inputValidation.AccountPropertyId"
            propText="Name"
            propValue="AccountPropertyId"
            v-model:id="account.AccountPropertyId"
            v-model:value="account.AccountPropertyName"
            :readonly="isReadonly"
            :listItems="accountPropertyList"
          ></MSInputCombobox>
        </div>

        <MSInputText
          :title="this.$t('AccountForm.Note')"
          :rows="3"
          v-model:value="account.Note"
          :readonly="isReadonly"
          ref="Note"
        ></MSInputText>

        <MSCheckBox
          :title="this.$t('AccountForm.HasForeignCurrencyAccounting')"
          v-model:value="account.HasForeignCurrencyAccounting"
          :readonly="isReadonly"
        ></MSCheckBox>

        <div class="collapse">
          <div class="icon-container">
            <div class="icon-arrow-right-black"></div>
          </div>
          {{ this.$t("AccountForm.FollowBy") }}
        </div>
        <div class="sub-menu">
          <div class="block">
            <div class="row">
              <MSCheckBox
                :title="this.$t('AccountForm.ObjectType')"
                v-model:value="ObjectType"
                :readonly="isReadonly"
              ></MSCheckBox>
              <MSInputCombobox
                propText="Name"
                propValue="Id"
                v-model:id="account.ObjectType"
                :listItems="objectTypeList"
                :disabled="!ObjectType"
                :readonly="isReadonly"
              ></MSInputCombobox>
            </div>
            <div class="row">
              <MSCheckBox
                :title="this.$t('AccountForm.CostAggregationObject')"
                v-model:value="CostAggregationObject"
                :readonly="isReadonly"
              ></MSCheckBox>
              <MSInputCombobox
                propText="Name"
                propValue="Id"
                v-model:id="account.CostAggregationObject"
                :listItems="followTypeList"
                :disabled="!CostAggregationObject"
                :readonly="isReadonly"
              ></MSInputCombobox>
            </div>
            <div class="row">
              <MSCheckBox
                :title="this.$t('AccountForm.TheOrder')"
                v-model:value="TheOrder"
                :readonly="isReadonly"
              ></MSCheckBox>
              <MSInputCombobox
                propText="Name"
                propValue="Id"
                v-model:id="account.TheOrder"
                :listItems="followTypeList"
                :disabled="!TheOrder"
                :readonly="isReadonly"
              ></MSInputCombobox>
            </div>
            <div class="row">
              <MSCheckBox
                :title="this.$t('AccountForm.PurchaseContract')"
                v-model:value="PurchaseContract"
                :readonly="isReadonly"
              ></MSCheckBox>
              <MSInputCombobox
                propText="Name"
                propValue="Id"
                v-model:id="account.PurchaseContract"
                :listItems="followTypeList"
                :disabled="!PurchaseContract"
                :readonly="isReadonly"
              ></MSInputCombobox>
            </div>
            <div class="row">
              <MSCheckBox
                :title="this.$t('AccountForm.Unit')"
                v-model:value="Unit"
                :readonly="isReadonly"
              ></MSCheckBox>
              <MSInputCombobox
                propText="Name"
                propValue="Id"
                v-model:id="account.Unit"
                :listItems="followTypeList"
                :disabled="!Unit"
                :readonly="isReadonly"
              ></MSInputCombobox>
            </div>
          </div>
          <div class="block">
            <div class="row">
              <MSCheckBox
                :title="this.$t('AccountForm.BankAccount')"
                v-model:value="account.BankAccount"
                :readonly="isReadonly"
              ></MSCheckBox>
            </div>
            <div class="row">
              <MSCheckBox
                :title="this.$t('AccountForm.Construction')"
                v-model:value="Construction"
                :readonly="isReadonly"
              ></MSCheckBox>
              <MSInputCombobox
                propText="Name"
                propValue="Id"
                v-model:id="account.Construction"
                :listItems="followTypeList"
                :disabled="!Construction"
                :readonly="isReadonly"
              ></MSInputCombobox>
            </div>
            <div class="row">
              <MSCheckBox
                :title="this.$t('AccountForm.ContractOfSale')"
                v-model:value="ContractOfSale"
                :readonly="isReadonly"
              ></MSCheckBox>
              <MSInputCombobox
                propText="Name"
                propValue="Id"
                v-model:id="account.ContractOfSale"
                :listItems="followTypeList"
                :disabled="!ContractOfSale"
                :readonly="isReadonly"
              ></MSInputCombobox>
            </div>
            <div class="row">
              <MSCheckBox
                :title="this.$t('AccountForm.ExpenseItem')"
                v-model:value="ExpenseItem"
                :readonly="isReadonly"
              ></MSCheckBox>
              <MSInputCombobox
                propText="Name"
                propValue="Id"
                v-model:id="account.ExpenseItem"
                :listItems="followTypeList"
                :disabled="!ExpenseItem"
                :readonly="isReadonly"
              ></MSInputCombobox>
            </div>
            <div class="row">
              <MSCheckBox
                :title="this.$t('AccountForm.StatisticalCode')"
                v-model:value="StatisticalCode"
                :readonly="isReadonly"
              ></MSCheckBox>
              <MSInputCombobox
                propText="Name"
                propValue="Id"
                v-model:id="account.StatisticalCode"
                :listItems="followTypeList"
                :disabled="!StatisticalCode"
                :readonly="isReadonly"
              ></MSInputCombobox>
            </div>
          </div>
        </div>
      </div>

      <div class="form__footer">
        <div class="absolute">
          <div class="left">
            <button
              :disabled="isProcessing"
              type="button"
              class="extra-button"
              @mousemove="
                (event) =>
                  $msEmitter.emit(
                    'showTooltip',
                    event,
                    this.$t('Button.Cancel') + ' (ESC)'
                  )
              "
              @mouseout="$msEmitter.emit('hideTooltip')"
              @click="closeForm()"
            >
              {{ this.$t("Button.Cancel") }}
            </button>
          </div>
          <div class="right">
            <button
              :disabled="isProcessing || isReadonly"
              id="save"
              type="button"
              class="extra-button"
              @click="save()"
              @mousemove="
                (event) =>
                  $msEmitter.emit(
                    'showTooltip',
                    event,
                    this.$t('Button.Save') + ' (Ctrl + S)'
                  )
              "
              @mouseout="$msEmitter.emit('hideTooltip')"
              :disable="isReadonly"
            >
              {{ this.$t("Button.Save") }}
            </button>

            <button
              :disabled="isProcessing || isReadonly"
              type="button"
              class="main-button"
              @click="saveAndAdd()"
              @mousemove="
                (event) =>
                  $msEmitter.emit(
                    'showTooltip',
                    event,
                    this.$t('Button.SaveAndAdd') + ' (Ctrl + Shift + S)'
                  )
              "
              @mouseout="$msEmitter.emit('hideTooltip')"
            >
              {{ this.$t("Button.SaveAndAdd") }}
            </button>
          </div>
        </div>
      </div>
    </form>
  </div>
</template>
    
<script>
import accountEnum from "../../js/enum/accountEnum";
import mixin from "../../js/views/account/AccountForm.js";
export default {
  mixins: [mixin],
  data() {
    return {
      enumFormTab: accountEnum.FormTab,
    };
  },
};
</script>
    
<style src="../../style/views/account/AccountForm.scss" lang="scss" scoped>
</style>

<style lang="scss" scoped>
.form__account-form {
  .form {
    --form-width: v-bind(formWidth);
  }
}
</style>
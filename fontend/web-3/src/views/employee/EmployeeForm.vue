<template>
  <div class="popup__employee-form">
    <form class="popup">
      <div class="popup__header">
        <h1>{{ this.title }}</h1>
        <div
          @click="hideEmployeeForm"
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
        >
          <div class="icon-x-black"></div>
        </div>
      </div>

      <button
        type="button"
        style="position: absolute; border: none"
        @focus="focusLastInput()"
      ></button>

      <div class="popup__body">
        <div class="popup__sub-body">
          <MSInputText
            :title="this.$t('EmployeeForm.EmployeeCode')"
            :isFirstInput="true"
            v-model:value="employee.EmployeeCode"
            :isRequired="true"
            :isValidating="isValidating"
            v-model:isValid="inputValidation.EmployeeCode"
            :onDoneTyping="checkDuplicateEmployeeCode"
            ref="EmployeeCode"
          ></MSInputText>

          <MSInputText
            :title="this.$t('EmployeeForm.FullName')"
            :errorMessage="this.$t('Error.FullNameRequired')"
            v-model:value="employee.FullName"
            :isRequired="true"
            :isValidating="isValidating"
            v-model:isValid="inputValidation.FullName"
            ref="FullName"
          ></MSInputText>

          <MSInputCombobox
            :title="this.$t('EmployeeForm.Department')"
            :isRequired="true"
            :listItems="this.departments"
            propText="DepartmentName"
            propValue="DepartmentId"
            v-model:id="employee.DepartmentId"
            v-model:value="employee.DepartmentName"
            :isValidating="isValidating"
            v-model:isValid="inputValidation.DepartmentId"
          ></MSInputCombobox>

          <MSInputCombobox
            :title="this.$t('EmployeeForm.Position')"
            :isRequired="false"
            :listItems="this.positions"
            propText="PositionName"
            propValue="PositionId"
            v-model:id="employee.PositionId"
            v-model:value="employee.PositionName"
            :isValidating="isValidating"
            v-model:isValid="inputValidation.PositionId"
          ></MSInputCombobox>
        </div>

        <div class="popup__sub-body">
          <MSInputDate
            :title="this.$t('EmployeeForm.DayOfBirth')"
            v-model:value="employee.DateOfBirth"
            :isValidating="isValidating"
            v-model:isValid="inputValidation.DateOfBirth"
            :errorMessage="dateOfBirthErrorMessage"
            :conditional="dateOfBirthConditional"
          ></MSInputDate>

          <MSInputRadio
            :title="this.$t('EmployeeForm.Gender')"
            :items="inputRadioItems"
            v-model:id="employee.Gender"
          ></MSInputRadio>

          <MSInputText
            :title="this.$t('EmployeeForm.IdentityNumberShort')"
            :tooltip="this.$t('EmployeeForm.IdentityNumber')"
            v-model:value="employee.IdentityNumber"
            ref="IdentityNumber"
            inputType="number"
          ></MSInputText>

          <MSInputDate
            :title="this.$t('EmployeeForm.IdentityDate')"
            v-model:value="employee.IdentityDate"
            :isValidating="isValidating"
            v-model:isValid="inputValidation.IdentityDate"
            :errorMessage="identityDateErrorMessage"
            :conditional="identityDateConditional"
          ></MSInputDate>

          <MSInputText
            :title="this.$t('EmployeeForm.IdentityPlace')"
            v-model:value="employee.IdentityPlace"
            ref="IdentityPlace"
          ></MSInputText>
        </div>

        <div class="popup__sub-body">
          <MSInputText
            :title="this.$t('EmployeeForm.Address')"
            v-model:value="employee.Address"
            ref="Address"
          ></MSInputText>

          <MSInputText
            :title="this.$t('EmployeeForm.PhoneNumberShort')"
            :tooltip="this.$t('EmployeeForm.PhoneNumber')"
            v-model:value="employee.PhoneNumber"
            ref="PhoneNumber"
          ></MSInputText>

          <MSInputText
            :title="this.$t('EmployeeForm.LandlinePhoneShort')"
            :tooltip="this.$t('EmployeeForm.LandlinePhone')"
          ></MSInputText>

          <MSInputText
            :title="this.$t('EmployeeForm.Email')"
            v-model:value="employee.Email"
            ref="Email"
            :isValidating="isValidating"
            v-model:isValid="inputValidation.Email"
            inputType="email"
            :errorMessage="this.$t('Error.IncorrectFormat')"
          ></MSInputText>

          <MSInputText
            :title="this.$t('EmployeeForm.BankAccountNumber')"
            v-model:value="employee.BankAccountNumber"
            ref="BankAccountNumber"
          ></MSInputText>

          <MSInputText
            :title="this.$t('EmployeeForm.NameOfBank')"
            v-model:value="employee.NameOfBank"
            ref="NameOfBank"
          ></MSInputText>

          <MSInputText
            :title="this.$t('EmployeeForm.BankAccountBranch')"
            v-model:value="employee.BankAccountBranch"
            ref="BankAccountBranch"
          ></MSInputText>
        </div>
      </div>

      <div class="popup__footer">
        <button
          :disabled="isProcessing"
          id="save-and-add"
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
        <button
          :disabled="isProcessing"
          id="save"
          type="button"
          class="extra-button"
          @click="save"
          @mousemove="
            (event) =>
              $msEmitter.emit(
                'showTooltip',
                event,
                this.$t('Button.Save') + ' (Ctrl + S)'
              )
          "
          @mouseout="$msEmitter.emit('hideTooltip')"
        >
          {{ this.$t("Button.Save") }}
        </button>

        <button
          :disabled="isProcessing"
          ref="lastButton"
          id="cancel"
          type="button"
          class="extra-button"
          @click="hideEmployeeForm()"
          @mousemove="
            (event) =>
              $msEmitter.emit(
                'showTooltip',
                event,
                this.$t('Button.Cancel') + ' (ESC)'
              )
          "
          @mouseout="$msEmitter.emit('hideTooltip')"
        >
          {{ this.$t("Button.Cancel") }}
        </button>

        <button
          type="button"
          invisible="true"
          @focus="focusFirstInput()"
        ></button>
      </div>
    </form>
  </div>
</template>

<script>
import mixin from "../../js/views/employee/EmployeeForm.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/views/employee/EmployeeForm.scss" lang="scss">
</style>
<template>
  <div class="form__customer-form">
    <form class="form">
      <div class="form__header">
        <p class="title">{{ this.title }}</p>
        <MSInputRadio
          :items="inputRadioItems"
          v-model:id="customer.CustomerType"
          :readonly="isHardReadonly"
        ></MSInputRadio>
        <div
          class="icon-container"
          @click="closeForm()"
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
          <div class="icon-x-black"></div>
        </div>
      </div>

      <div class="form__body">
        <div
          v-show="
            customer.CustomerType == $customerEnum.CustomerType.Organization
          "
          class="organization"
        >
          <div class="form__sub-body">
            <MSInputText
              :title="$t('CustomerForm.TaxCode')"
              :isFirstInput="true"
              v-model:value="customer.TaxCode"
              :isValidating="isValidating"
              v-model:isValid="inputValidation.TaxCode"
              :onDoneTyping="checkCheckTaxCode"
              inputType="number"
              ref="TaxCode"
              :readonly="isHardReadonly"
            ></MSInputText>

            <MSInputText
              :isRequired="true"
              :title="$t('CustomerForm.CustomerCode')"
              v-model:value="customer.CustomerCode"
              :isValidating="isValidating"
              v-model:isValid="inputValidation.CustomerCode"
              :onDoneTyping="checkDuplicateCode"
              ref="CustomerCode"
              :readonly="isHardReadonly"
            ></MSInputText>
            <MSInputText
              :isRequired="true"
              :title="$t('CustomerForm.CustomerName')"
              v-model:value="customer.FullName"
              :isValidating="isValidating"
              v-model:isValid="inputValidation.FullName"
              ref="FullName"
              :readonly="isHardReadonly"
            ></MSInputText>
            <MSInputText
              :title="$t('CustomerForm.Address')"
              v-model:value="customer.Address"
              :isValidating="isValidating"
              v-model:isValid="inputValidation.Address"
              :rows="2"
              ref="Address"
              :readonly="isHardReadonly"
            ></MSInputText>
          </div>

          <div class="form__sub-body">
            <MSInputText
              :title="$t('CustomerForm.Phone')"
              v-model:value="customer.PhoneNumber"
              :isValidating="isValidating"
              v-model:isValid="inputValidation.PhoneNumber"
              ref="PhoneNumber"
              :readonly="isHardReadonly"
            ></MSInputText>
            <MSInputText
              :title="$t('CustomerForm.Website')"
              v-model:value="customer.Website"
              :isValidating="isValidating"
              v-model:isValid="inputValidation.Website"
              ref="Website"
              :readonly="isHardReadonly"
            ></MSInputText>
            <MSInputComboboxMultiSelect
              :title="$t('CustomerForm.CustomerGroup')"
              propText="Name"
              propValue="CustomerGroupId"
              :listCols="['CustomerGroupCode', 'Name']"
              :listTitle="[
                $t('CustomerForm.CustomerGroupCode'),
                $t('CustomerForm.CustomerGroupName'),
              ]"
              v-model:ids="customer.CustomerGroupIds"
              :listItems="customerGroupList"
              :readonly="isHardReadonly"
            >
            </MSInputComboboxMultiSelect>
            <MSInputCombobox
              :title="$t('CustomerForm.SalesAgent')"
              propText="FullName"
              propValue="EmployeeId"
              v-model:id="customer.EmployeeId"
              :apiPaging="$msApi.EmployeeApi.Paging"
              :apiGetById="$msApi.EmployeeApi.Get"
              :readonly="isHardReadonly"
            ></MSInputCombobox>
          </div>

          <div class="form__sub-menu">
            <div class="menu">
              <div
                v-for="formTab in formTabs"
                :key="formTab.id"
                class="tab"
                :class="{ 'tab--active': formTabId == formTab.id }"
                @click="formTabId = formTab.id"
              >
                {{ formTab.value }}
              </div>
            </div>
            <div class="body">
              <div v-if="formTabId == enumFormTab.Infor" class="body__info">
                <div class="block">
                  <MSInputCombobox
                    :title="$t('CustomerForm.HumanContact')"
                    :placeholder="$t('CustomerForm.Vocative')"
                    :listItems="vocativeList"
                    propText="name"
                    propValue="id"
                    v-model:id="Vocative"
                    :readonly="isHardReadonly"
                  ></MSInputCombobox>
                  <MSInputText
                    :placeholder="$t('CustomerForm.FirstAndLastName')"
                    v-model:value="FullName"
                    :readonly="isHardReadonly"
                  ></MSInputText>
                  <MSInputText
                    placeholder="Email"
                    v-model:value="Email"
                    :readonly="isHardReadonly"
                  ></MSInputText>
                  <MSInputText
                    :placeholder="$t('CustomerForm.PhoneNumber')"
                    v-model:value="PhoneNumber"
                    :readonly="isHardReadonly"
                  ></MSInputText>
                </div>

                <div class="block">
                  <MSInputText
                    :title="$t('CustomerForm.ElectronicInvoiceRecipients')"
                    :placeholder="$t('CustomerForm.FirstAndLastName')"
                    v-model:value="RecipientFullName"
                    :readonly="isHardReadonly"
                  ></MSInputText>
                  <MSInputText
                    :placeholder="$t('CustomerForm.EmailPlaceHolder')"
                    v-model:value="RecipientEmail"
                    :readonly="isHardReadonly"
                  ></MSInputText>
                  <MSInputText
                    :placeholder="$t('CustomerForm.PhoneNumber')"
                    v-model:value="RecipientPhoneNumber"
                    :readonly="isHardReadonly"
                  ></MSInputText>
                </div>

                <div class="block">
                  <MSInputText
                    :title="$t('CustomerForm.LegalRepresentative')"
                    :placeholder="$t('CustomerForm.LegalRepresentative')"
                    v-model:value="LegalRepresentative"
                    :readonly="isHardReadonly"
                  ></MSInputText>
                </div>
              </div>

              <div
                v-if="formTabId == enumFormTab.TermPayment"
                class="body__term-payment"
              >
                <MSInputCombobox
                  :title="$t('CustomerForm.TermsOfPayment')"
                  propText="Name"
                  propValue="TermsOfPaymentId"
                  :listTitle="[
                    $t('CustomerForm.TermsOfPaymentCode'),
                    $t('CustomerForm.TermsOfPaymentName'),
                  ]"
                  :listCols="['TermsOfPaymentCode', 'Name']"
                  v-model:id="customer.TermsOfPaymentId"
                  :apiPaging="$msApi.TermsOfPaymentApi.Paging"
                  :apiGetById="$msApi.TermsOfPaymentApi.Get"
                  :readonly="isHardReadonly"
                >
                </MSInputCombobox>
                <MSInputText
                  :title="$t('CustomerForm.MaxDaysOwed')"
                  v-model:value="customer.MaxDaysOwed"
                  inputType="money"
                  textAlign="right"
                  :readonly="isHardReadonly"
                ></MSInputText>
                <MSInputText
                  :title="$t('CustomerForm.MaxAmountOfDebt')"
                  v-model:value="customer.MaxAmountOfDebt"
                  :readonly="isHardReadonly"
                  inputType="money"
                  textAlign="right"
                ></MSInputText>
                <MSInputCombobox
                  :title="$t('CustomerForm.AccountCollect')"
                  propText="AccountCode"
                  propValue="AccountId"
                  :listTitle="[
                    $t('CustomerForm.AccountCode'),
                    $t('CustomerForm.AccountName'),
                  ]"
                  :listCols="['AccountCode', 'NameVi']"
                  v-model:id="customer.AccountCollectId"
                  :apiPaging="$msApi.AccountApi.Paging"
                  :apiGetById="$msApi.AccountApi.Get"
                  :readonly="isHardReadonly"
                ></MSInputCombobox>
              </div>

              <div
                v-if="formTabId == enumFormTab.BankAccount"
                class="body__bank-account"
              >
                <div class="table">
                  <table>
                    <tr>
                      <th>{{ $t("CustomerForm.AccountCode") }}</th>
                      <th>{{ $t("CustomerForm.BankName") }}</th>
                      <th>{{ $t("CustomerForm.Brach") }}</th>
                      <th>{{ $t("CustomerForm.BanksCity") }}</th>
                      <th></th>
                    </tr>
                    <tr
                      v-for="(BankAccount, index) in customer.BankAccounts"
                      :key="index"
                    >
                      <td>
                        <MSInputText
                          v-model:value="BankAccount.BankAccountNumber"
                          inputType="number"
                          :readonly="isHardReadonly"
                        ></MSInputText>
                      </td>
                      <td>
                        <MSInputText
                          v-model:value="BankAccount.Name"
                          :readonly="isHardReadonly"
                        ></MSInputText>
                      </td>
                      <td>
                        <MSInputText
                          v-model:value="BankAccount.Branch"
                          :readonly="isHardReadonly"
                        ></MSInputText>
                      </td>
                      <td>
                        <MSInputText
                          v-model:value="BankAccount.BankCity"
                          :readonly="isHardReadonly"
                        ></MSInputText>
                      </td>
                      <td>
                        <div
                          class="icon-container"
                          @click="removeBankAccount(index)"
                        >
                          <div class="icon-trash-black"></div>
                        </div>
                      </td>
                    </tr>
                  </table>
                </div>

                <div class="button-container">
                  <button
                    class="extra-button"
                    type="button"
                    @click="addBankAccount()"
                  >
                    {{ $t("Cash.AddLine") }}
                  </button>
                  <button
                    class="extra-button"
                    type="button"
                    @click="removeAllBankAccount()"
                  >
                    {{ $t("Cash.DeleteAllLines") }}
                  </button>
                </div>
              </div>

              <div
                v-if="formTabId == enumFormTab.OrtherAddress"
                class="body__orther-address"
              >
                <div class="block">
                  <MSInputCombobox
                    :title="$t('CustomerForm.Location')"
                    :placeholder="$t('CustomerForm.Nation')"
                    propText="Name"
                    propValue="NationId"
                    v-model:id="NationId"
                    :apiPaging="$msApi.NationApi.Paging"
                    :apiGetById="$msApi.NationApi.Get"
                    :readonly="isHardReadonly"
                    @valueOnChange="nationOnChange"
                  ></MSInputCombobox>

                  <MSInputCombobox
                    :placeholder="$t('CustomerForm.City')"
                    propText="Name"
                    propValue="CityId"
                    v-model:id="CityId"
                    :apiPaging="CityPagingApi"
                    :apiGetById="CityGetApi"
                    :readonly="isHardReadonly"
                    :disabled="!NationId"
                    @valueOnChange="cityOnChange"
                  ></MSInputCombobox>

                  <MSInputCombobox
                    :placeholder="$t('CustomerForm.District')"
                    propText="Name"
                    propValue="DistrictId"
                    v-model:id="DistrictId"
                    :apiPaging="DistrictPagingApi"
                    :apiGetById="DistrictGetApi"
                    :readonly="isHardReadonly"
                    :disabled="!CityId"
                    @valueOnChange="districtOnChange"
                  ></MSInputCombobox>

                  <MSInputCombobox
                    :placeholder="$t('CustomerForm.Commune')"
                    propText="Name"
                    propValue="CommuneId"
                    v-model:id="CommuneId"
                    :apiPaging="CommunePagingApi"
                    :apiGetById="CommuneGetApi"
                    :readonly="isHardReadonly"
                    :disabled="!DistrictId"
                  ></MSInputCombobox>
                </div>
                <div class="block">
                  <div class="flex">
                    <div class="address">
                      {{ $t("CustomerForm.DeliveryAddress") }}
                    </div>
                    <MSCheckBox
                      :title="$t('CustomerForm.SameAsCustomerAddress')"
                      v-model:value="IsSameCustomerAddress"
                      @onChangeValue="takeSameCustomerAddress()"
                      :readonly="isHardReadonly"
                    ></MSCheckBox>
                  </div>
                  <div class="table">
                    <table>
                      <tr
                        v-for="(SpecificAddress, index) in SpecificAddresses"
                        :key="index"
                      >
                        <td>
                          <MSInputText
                            v-model:value="SpecificAddress.Address"
                            :readonly="isHardReadonly"
                          ></MSInputText>
                        </td>
                        <td>
                          <div
                            class="icon-container"
                            @click="removeSpecificAddress(index)"
                          >
                            <div class="icon-trash-black"></div>
                          </div>
                        </td>
                      </tr>
                    </table>
                  </div>

                  <div class="button-container">
                    <button
                      :disabled="isHardReadonly"
                      class="extra-button"
                      type="button"
                      @click="addSpecificAddress()"
                    >
                      {{ $t("Cash.AddLine") }}
                    </button>
                    <button
                      :disabled="isHardReadonly"
                      class="extra-button"
                      type="button"
                      @click="removeAllSpecificAddress()"
                    >
                      {{ $t("Cash.DeleteAllLines") }}
                    </button>
                  </div>
                </div>
              </div>

              <div v-if="formTabId == enumFormTab.Note" class="body__note">
                <MSInputText
                  :title="$t('CustomerForm.Note')"
                  v-model:value="customer.Note"
                  :rows="10"
                  :readonly="isHardReadonly"
                ></MSInputText>
              </div>
            </div>
          </div>
        </div>
        <div
          v-show="
            customer.CustomerType != $customerEnum.CustomerType.Organization
          "
          class="individual"
        >
          <div class="form__sub-body">
            <MSInputText
              :isRequired="true"
              :title="$t('CustomerForm.CustomerCode')"
              v-model:value="customer.CustomerCode"
              :isValidating="isValidating"
              v-model:isValid="inputValidation.CustomerCode"
              :onDoneTyping="checkDuplicateCode"
              ref="CustomerCode"
              :readonly="isHardReadonly"
            ></MSInputText>
            <MSInputText
              :title="$t('CustomerForm.TaxCode')"
              :isFirstInput="true"
              v-model:value="customer.TaxCode"
              :isValidating="isValidating"
              v-model:isValid="inputValidation.TaxCode"
              :onDoneTyping="checkCheckTaxCode"
              inputType="number"
              ref="TaxCode"
              :readonly="isHardReadonly"
            ></MSInputText>
            <MSInputCombobox
              :isRequired="true"
              :title="$t('CustomerForm.CustomerName')"
              :placeholder="$t('CustomerForm.Vocative')"
              :listItems="vocativeList"
              propText="name"
              propValue="id"
              v-model:id="Vocative"
              :readonly="isHardReadonly"
            ></MSInputCombobox>
            <MSInputText
              :placeholder="$t('CustomerForm.FirstAndLastName')"
              v-model:value="customer.FullName"
              :isValidating="isValidating"
              v-model:isValid="inputValidation.FullName"
              ref="FullName"
              :readonly="isHardReadonly"
            ></MSInputText>
            <MSInputText
              :title="$t('CustomerForm.Address')"
              v-model:value="customer.Address"
              :isValidating="isValidating"
              v-model:isValid="inputValidation.Address"
              :rows="2"
              ref="Address"
              :readonly="isHardReadonly"
            ></MSInputText>
          </div>

          <div class="form__sub-body">
            <MSInputComboboxMultiSelect
              :title="$t('CustomerForm.CustomerGroup')"
              propText="Name"
              propValue="CustomerGroupId"
              :listCols="['CustomerGroupCode', 'Name']"
              :listTitle="[
                $t('CustomerForm.CustomerGroupCode'),
                $t('CustomerForm.CustomerGroupName'),
              ]"
              v-model:ids="customer.CustomerGroupIds"
              :listItems="customerGroupList"
              :readonly="isHardReadonly"
            >
            </MSInputComboboxMultiSelect>
            <MSInputCombobox
              :title="$t('CustomerForm.SalesAgent')"
              propText="FullName"
              propValue="EmployeeId"
              v-model:id="customer.EmployeeId"
              :apiPaging="$msApi.EmployeeApi.Paging"
              :apiGetById="$msApi.EmployeeApi.Get"
              :readonly="isHardReadonly"
            ></MSInputCombobox>
          </div>

          <div class="form__sub-menu">
            <div class="menu">
              <div
                v-for="formTab in formTabs"
                :key="formTab.id"
                class="tab"
                :class="{ 'tab--active': formTabId == formTab.id }"
                @click="formTabId = formTab.id"
              >
                {{ formTab.value }}
              </div>
            </div>
            <div class="body">
              <div v-if="formTabId == enumFormTab.Infor" class="body__info">
                <div class="block">
                  <MSInputText
                    :title="$t('CustomerForm.ContactInfor')"
                    placeholder="Email"
                    v-model:value="Email"
                    :readonly="isHardReadonly"
                  ></MSInputText>
                  <MSInputText
                    :placeholder="$t('EmployeeForm.PhoneNumber')"
                    v-model:value="PhoneNumber"
                    :isValidating="isValidating"
                    v-model:isValid="inputValidation.PhoneNumber"
                    ref="PhoneNumber"
                    :readonly="isHardReadonly"
                  ></MSInputText>
                  <MSInputText
                    :placeholder="$t('EmployeeForm.LandlinePhone')"
                    v-model:value="LandlineNumber"
                    :readonly="isHardReadonly"
                  ></MSInputText>
                  <MSInputText
                    :title="$t('CustomerForm.LegalRepresentative')"
                    :placeholder="$t('CustomerForm.LegalRepresentative')"
                    v-model:value="LegalRepresentative"
                    :readonly="isHardReadonly"
                  ></MSInputText>
                </div>

                <div class="block">
                  <MSInputText
                    :title="$t('CustomerForm.IdentityTitle')"
                    :placeholder="$t('CustomerForm.IdentityTitle')"
                    v-model:value="customer.IdentityNumber"
                    :isValidating="isValidating"
                    v-model:isValid="inputValidation.IdentityNumber"
                    ref="IdentityNumber"
                    :readonly="isHardReadonly"
                  ></MSInputText>

                  <MSInputDate
                    :title="$t('EmployeeForm.IdentityDate')"
                    v-model:value="customer.IdentityDate"
                    :readonly="isHardReadonly"
                  ></MSInputDate>

                  <MSInputText
                    :title="$t('EmployeeForm.IdentityPlace')"
                    v-model:value="customer.IdentityPlace"
                    :isValidating="isValidating"
                    v-model:isValid="inputValidation.IdentityPlace"
                    ref="IdentityPlace"
                    :readonly="isHardReadonly"
                  ></MSInputText>
                </div>
              </div>

              <div
                v-if="formTabId == enumFormTab.TermPayment"
                class="body__term-payment"
              >
                <MSInputCombobox
                  :title="$t('CustomerForm.TermsOfPayment')"
                  propText="Name"
                  propValue="TermsOfPaymentId"
                  :listTitle="[
                    $t('CustomerForm.TermsOfPaymentCode'),
                    $t('CustomerForm.TermsOfPaymentName'),
                  ]"
                  :listCols="['TermsOfPaymentCode', 'Name']"
                  v-model:id="customer.TermsOfPaymentId"
                  :apiPaging="$msApi.TermsOfPaymentApi.Paging"
                  :apiGetById="$msApi.TermsOfPaymentApi.Get"
                  :readonly="isHardReadonly"
                >
                </MSInputCombobox>
                <MSInputText
                  :title="$t('CustomerForm.MaxDaysOwed')"
                  v-model:value="customer.MaxDaysOwed"
                  :readonly="isHardReadonly"
                  inputType="money"
                  textAlign="right"
                ></MSInputText>
                <MSInputText
                  :title="$t('CustomerForm.MaxAmountOfDebt')"
                  v-model:value="customer.MaxAmountOfDebt"
                  :readonly="isHardReadonly"
                  inputType="money"
                  textAlign="right"
                ></MSInputText>
                <MSInputCombobox
                  :title="$t('CustomerForm.AccountCollect')"
                  propText="AccountCode"
                  propValue="AccountId"
                  :listTitle="[
                    $t('CustomerForm.AccountCode'),
                    $t('CustomerForm.AccountName'),
                  ]"
                  :listCols="['AccountCode', 'NameVi']"
                  v-model:id="customer.AccountCollectId"
                  :apiPaging="$msApi.AccountApi.Paging"
                  :apiGetById="$msApi.AccountApi.Get"
                  :readonly="isHardReadonly"
                ></MSInputCombobox>
              </div>

              <div
                v-if="formTabId == enumFormTab.BankAccount"
                class="body__bank-account"
              >
                <div class="table">
                  <table>
                    <tr>
                      <th>{{ $t("CustomerForm.AccountCode") }}</th>
                      <th>{{ $t("CustomerForm.BankName") }}</th>
                      <th>{{ $t("CustomerForm.Brach") }}</th>
                      <th>{{ $t("CustomerForm.BanksCity") }}</th>
                      <th></th>
                    </tr>
                    <tr
                      v-for="(BankAccount, index) in customer.BankAccounts"
                      :key="index"
                    >
                      <td>
                        <MSInputText
                          v-model:value="BankAccount.BankAccountNumber"
                          :readonly="isHardReadonly"
                          inputType="number"
                        ></MSInputText>
                      </td>
                      <td>
                        <MSInputText
                          v-model:value="BankAccount.Name"
                          :readonly="isHardReadonly"
                        ></MSInputText>
                      </td>
                      <td>
                        <MSInputText
                          v-model:value="BankAccount.Branch"
                          :readonly="isHardReadonly"
                        ></MSInputText>
                      </td>
                      <td>
                        <MSInputText
                          v-model:value="BankAccount.BankCity"
                          :readonly="isHardReadonly"
                        ></MSInputText>
                      </td>
                      <td>
                        <div
                          class="icon-container"
                          @click="removeBankAccount(index)"
                        >
                          <div class="icon-trash-black"></div>
                        </div>
                      </td>
                    </tr>
                  </table>
                </div>

                <div class="button-container">
                  <button
                    class="extra-button"
                    type="button"
                    @click="addBankAccount()"
                  >
                    {{ $t("Cash.AddLine") }}
                  </button>
                  <button
                    class="extra-button"
                    type="button"
                    @click="removeAllBankAccount()"
                  >
                    {{ $t("Cash.DeleteAllLines") }}
                  </button>
                </div>
              </div>

              <div
                v-if="formTabId == enumFormTab.OrtherAddress"
                class="body__orther-address"
              >
                <div class="block">
                  <MSInputCombobox
                    :title="$t('CustomerForm.Location')"
                    :placeholder="$t('CustomerForm.Nation')"
                    propText="Name"
                    propValue="NationId"
                    v-model:id="NationId"
                    :apiPaging="$msApi.NationApi.Paging"
                    :apiGetById="$msApi.NationApi.Get"
                    :readonly="isHardReadonly"
                    @valueOnChange="nationOnChange"
                  ></MSInputCombobox>

                  <MSInputCombobox
                    :placeholder="$t('CustomerForm.City')"
                    propText="Name"
                    propValue="CityId"
                    v-model:id="CityId"
                    :apiPaging="CityPagingApi"
                    :apiGetById="CityGetApi"
                    :readonly="isHardReadonly"
                    :disabled="!NationId"
                    @valueOnChange="cityOnChange"
                  ></MSInputCombobox>

                  <MSInputCombobox
                    :placeholder="$t('CustomerForm.District')"
                    propText="Name"
                    propValue="DistrictId"
                    v-model:id="DistrictId"
                    :apiPaging="DistrictPagingApi"
                    :apiGetById="DistrictGetApi"
                    :readonly="isHardReadonly"
                    :disabled="!CityId"
                    @valueOnChange="districtOnChange"
                  ></MSInputCombobox>

                  <MSInputCombobox
                    :placeholder="$t('CustomerForm.Commune')"
                    propText="Name"
                    propValue="CommuneId"
                    v-model:id="CommuneId"
                    :apiPaging="CommunePagingApi"
                    :apiGetById="CommuneGetApi"
                    :readonly="isHardReadonly"
                    :disabled="!DistrictId"
                  ></MSInputCombobox>
                </div>
                <div class="block">
                  <div class="flex">
                    <div class="address">
                      {{ $t("CustomerForm.DeliveryAddress") }}
                    </div>
                    <MSCheckBox
                      :title="$t('CustomerForm.SameAsCustomerAddress')"
                      v-model:value="IsSameCustomerAddress"
                      @onChangeValue="takeSameCustomerAddress()"
                      :readonly="isHardReadonly"
                    ></MSCheckBox>
                  </div>
                  <div class="table">
                    <table>
                      <tr
                        v-for="(SpecificAddress, index) in SpecificAddresses"
                        :key="index"
                      >
                        <td>
                          <MSInputText
                            v-model:value="SpecificAddress.Address"
                            :readonly="isHardReadonly"
                          ></MSInputText>
                        </td>
                        <td>
                          <div
                            class="icon-container"
                            @click="removeSpecificAddress(index)"
                          >
                            <div class="icon-trash-black"></div>
                          </div>
                        </td>
                      </tr>
                    </table>
                  </div>

                  <div class="button-container">
                    <button
                      :disabled="isHardReadonly"
                      class="extra-button"
                      type="button"
                      @click="addSpecificAddress()"
                    >
                      {{ $t("Cash.AddLine") }}
                    </button>
                    <button
                      :disabled="isHardReadonly"
                      class="extra-button"
                      type="button"
                      @click="removeAllSpecificAddress()"
                    >
                      {{ $t("Cash.DeleteAllLines") }}
                    </button>
                  </div>
                </div>
              </div>

              <div v-if="formTabId == enumFormTab.Note" class="body__note">
                <MSInputText
                  :title="$t('CustomerForm.Note')"
                  v-model:value="customer.Note"
                  :rows="10"
                  :readonly="isHardReadonly"
                ></MSInputText>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="form__footer">
        <div class="absolute">
          <div class="left">
            <button
              :disabled="isProcessing"
              @click="closeForm()"
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
            >
              {{ this.$t("Button.Cancel") }}
            </button>
          </div>
          <div class="right">
            <button
              :disabled="isProcessing || isHardReadonly"
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
            >
              {{ this.$t("Button.Save") }}
            </button>

            <button
              :disabled="isProcessing || isHardReadonly"
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
import customerEnum from "../../js/enum/customerEnum";
import mixin from "../../js/views/customer/CustomerForm.js";
export default {
  mixins: [mixin],
  data() {
    return {
      enumFormTab: customerEnum.FormTab,
    };
  },
};
</script>
  
  <style src="../../style/views/customer/CustomerForm.scss" lang="scss" scoped>
</style>
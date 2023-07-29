<template>
  <div class="wrapper wrapper--dark1 wrapper--form">
    <div class="form__wrapper" v-show="formNoti.showNotibox">
      <BaseNotibox
        :type="formNoti.notiboxType"
        :message="formNoti.notiboxMessage"
        :yes-on-click="formNotiboxYesBtnOnClick"
        ref="notiRef"
      />
    </div>
    <div class="form__wrapper" v-show="formDialog.isShow">
      <BaseDialog
        :title="lang.common.dialog.savingChanges.title"
        :message="lang.common.dialog.savingChanges.message"
        :close-on-click="formDialogCloseBtnOnClick"
        :no-on-click="formDialogNoBtnOnClick"
        :yes-on-click="formDialogYesBtnOnClick"
        ref="dialogRef"
      />
    </div>
    <div class="form__loader" v-show="form.isLoading">
      <BaseLoader />
    </div>
    <div class="form" :class="[form.size == 0 ? 'form--small' : 'form--big']">
      <div class="form__expand">
        <div
          class="expand__icon mi mi-16 mi-chevron-left"
          @click="formExpandOnClick"
        ></div>
      </div>
      <div class="form__header">
        <div class="header__left">
          {{ form.type == "create" ? "Thêm Tài khoản" : "Sửa Tài khoản" }}
        </div>
        <div class="header__right">
          <BaseButton bname="" class="mi-36 btn--help" v-tooltip="'Giúp'" />
          <BaseButton
            bname=""
            class="mi-36 btn--close"
            v-tooltip="lang.cat_account.tooltip.closeBtn"
            @click="closeBtnOnClick"
          />
        </div>
      </div>
      <div class="form__body">
        <div class="form__upper">
          <div class="upper__line w-1_2">
            <div class="line__left flex-1">
              <BaseTextfield
                pholder=""
                :label="lang.cat_account.label.accNumber"
                :isrequired="true"
                v-model:text="account.accountNumber"
                v-model:noti="formNoti.accountNumber"
                ref="accountNumberRef"
              />
            </div>
          </div>
          <div class="upper__line">
            <div class="line__left flex-1">
              <BaseTextfield
                pholder=""
                :label="lang.cat_account.label.accName"
                :isrequired="true"
                v-model:text="account.accountNameVi"
                v-model:noti="formNoti.accountNameVi"
                ref="accountNameViRef"
              />
            </div>
            <div class="line__right flex-1">
              <BaseTextfield
                pholder=""
                :label="lang.cat_account.label.accNameVi"
                :isrequired="false"
                v-model:text="account.accountNameEn"
                noti=""
              />
            </div>
          </div>
          <div class="upper__line">
            <div class="line__left flex-1">
              <AccountCombobox
                :label="lang.cat_account.label.parentNumber"
                v-model:selectedItemId="account.parentId"
                v-model:selectedItemName="account.parentNumber"
                :ranking="true"
                :have-add-btn="true"
              />
            </div>
            <div class="line__right flex-1">
              <BaseCombobox
                :label="lang.cat_account.label.categoryKind"
                pholder=""
                :isrequired="true"
                :option-list="categoryKindList"
                v-model:text="account.categoryKindName"
                v-model:noti="formNoti.categoryKind"
                v-model:selectedItemId="account.categoryKind"
                :schema="categoryKindSchema"
                ref="accountKindRef"
              />
            </div>
          </div>
          <div class="upper__line">
            <div class="flex-1">
              <div class="text__area">
                <div class="text__area__label">
                  {{ lang.cat_account.label.description }}
                </div>
                <textarea
                  name=""
                  id=""
                  cols="100"
                  rows="4"
                  v-model="account.description"
                ></textarea>
              </div>
            </div>
          </div>
          <div class="upper__line">
            <BaseCheckbox
              :label="lang.cat_account.label.foreignCurrency"
              v-model:checked="account.foreignCurrencyAccounting"
            />
          </div>
        </div>
        <div class="form__lower">
          <div class="lower__collapse">
            <div class="collapse__title">
              <div class="title__icon mi mi-16 mi-arrow-right-black"></div>
              <div class="title__text" @click="collapseOnClick">
                {{ lang.cat_account.text.detailBy }}
              </div>
            </div>
            <div class="collapse__option" v-show="!form.collapse">
              <div class="option__line">
                <div class="line__box">
                  <BaseCheckbox
                    :label="lang.cat_account.label.object"
                    :checked="account.detailByAccountObject"
                    class="flex-1"
                    @click="detailByAccountObjectOnClick"
                  />
                  <BaseSelectbox
                    class="flex-1"
                    :isActive="account.detailByAccountObject"
                    label=""
                    pholder=""
                    :isrequired="false"
                    :option-list="accountObjectOptionList"
                    noti=""
                    v-model:selectedItemId="account.detailByAccountObjectKind"
                  />
                </div>
                <div class="line__box">
                  <BaseCheckbox
                    :label="lang.cat_account.label.bankAcc"
                    :checked="account.detailByBankAccount"
                    @click="
                      account.detailByBankAccount = !account.detailByBankAccount
                    "
                  />
                </div>
              </div>
              <div class="option__line">
                <div class="line__box">
                  <BaseCheckbox
                    class="flex-1"
                    :label="lang.cat_account.label.objectCollectionCost"
                    v-tooltip="lang.cat_account.tooltip.objectCollectionCost"
                    :checked="false"
                  />
                  <BaseSelectbox
                    class="flex-1"
                    :isActive="false"
                    label=""
                    pholder=""
                    :isrequired="false"
                    :option-list="[]"
                    noti=""
                    :selectedItemId="-1"
                  />
                </div>
                <div class="line__box">
                  <BaseCheckbox
                    class="flex-1"
                    :label="lang.cat_account.label.construction"
                    :checked="false"
                  />
                  <BaseSelectbox
                    class="flex-1"
                    :isActive="false"
                    label=""
                    pholder=""
                    :isrequired="false"
                    :option-list="[]"
                    noti=""
                    :selectedItemId="-1"
                  />
                </div>
              </div>
              <div class="option__line">
                <div class="line__box">
                  <BaseCheckbox
                    class="flex-1"
                    :label="lang.cat_account.label.order"
                    :checked="false"
                  />
                  <BaseSelectbox
                    class="flex-1"
                    :isActive="false"
                    label=""
                    pholder=""
                    :isrequired="false"
                    :option-list="[]"
                    noti=""
                    :selectedItemId="-1"
                  />
                </div>
                <div class="line__box">
                  <BaseCheckbox
                    class="flex-1"
                    :label="lang.cat_account.label.sellContract"
                    :checked="false"
                  />
                  <BaseSelectbox
                    class="flex-1"
                    :isActive="false"
                    label=""
                    pholder=""
                    :isrequired="false"
                    :option-list="[]"
                    noti=""
                    :selectedItemId="-1"
                  />
                </div>
              </div>
              <div class="option__line">
                <div class="line__box">
                  <BaseCheckbox
                    class="flex-1"
                    :label="lang.cat_account.label.buyContract"
                    :checked="false"
                  />
                  <BaseSelectbox
                    class="flex-1"
                    :isActive="false"
                    label=""
                    pholder=""
                    :isrequired="false"
                    :option-list="[]"
                    noti=""
                    :selectedItemId="-1"
                  />
                </div>
                <div class="line__box">
                  <BaseCheckbox
                    class="flex-1"
                    :label="lang.cat_account.label.expenseItem"
                    v-tooltip="lang.cat_account.tooltip.expenseItem"
                    :checked="false"
                  />
                  <BaseSelectbox
                    class="flex-1"
                    :isActive="false"
                    label=""
                    pholder=""
                    :isrequired="false"
                    :option-list="[]"
                    noti=""
                    :selectedItemId="-1"
                  />
                </div>
              </div>
              <div class="option__line">
                <div class="line__box">
                  <BaseCheckbox
                    class="flex-1"
                    :label="lang.cat_account.label.unit"
                    :checked="false"
                  />
                  <BaseSelectbox
                    class="flex-1"
                    :isActive="false"
                    label=""
                    pholder=""
                    :isrequired="false"
                    :option-list="[]"
                    noti=""
                    :selectedItemId="-1"
                  />
                </div>
                <div class="line__box">
                  <BaseCheckbox
                    class="flex-1"
                    :label="lang.cat_account.label.statisticCode"
                    :checked="false"
                  />
                  <BaseSelectbox
                    class="flex-1"
                    :isActive="false"
                    label=""
                    pholder=""
                    :isrequired="false"
                    :option-list="[]"
                    noti=""
                    :selectedItemId="-1"
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <hr class="m-top-24" />
      <div class="form__footer m-top-24">
        <div class="footer__left">
          <BaseButton
            bname="Hủy"
            class="btn--secondary"
            @click="btnCancelOnClick"
          />
        </div>
        <div class="footer__right">
          <BaseButton
            :bname="lang.button.save"
            class="btn--secondary"
            @click="btnSaveOnClick"
            v-tooltip:top="lang.tooltip.save"
          />
          <BaseButton
            :bname="lang.button.saveAndAdd"
            class="btn--primary"
            v-tooltip:top="lang.tooltip.saveAndAdd"
            @click="btnSaveAndAddOnClick"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
//#region import
import { nextTick, ref, onMounted, inject, watch } from "vue";
import { useRouter, useRoute } from "vue-router";
import $enum from "@/js/common/enum";
import $error from "@/js/resources/error";
import $api from "@/js/api";
import { Account } from "@/js/model/account";
import AccountCombobox from "../account/AccountCombobox.vue";
//#endregion

//#region init
const $axios = inject("$axios");
const router = useRouter();
const route = useRoute();
const account = ref({});
const lang = inject("$lang");
// const _ = require("lodash");

var oldAccount = null;
const formDialog = ref({
  isShow: false,
});

const formNoti = ref({
  showNotibox: false,
  notiboxType: "",
  notiboxMessage: "",

  accountNumber: "",
  accountNameVi: "",
  accountKind: "",
  categoryKind: "",
});

let firstErrorRef = null;
const notiRef = ref(null);
const accountNumberRef = ref(null);
const accountNameViRef = ref(null);
const accountKindRef = ref(null);

const form = ref({
  size: 0,
  collapse: true,
  type: "",
  accId: "",
  isLoading: false,
});

const accountObjectOptionList = [
  lang.cat_account.accountObjectOption.provider,
  lang.cat_account.accountObjectOption.customer,
  lang.cat_account.accountObjectOption.employee,
];

const categoryKindSchema = {
  id: "optionId",
  name: "optionName",
};
const categoryKindList = [
  {
    optionId: "0",
    optionName: lang.cat_account.categoryKind.debitBalance,
  },
  {
    optionId: "1",
    optionName: lang.cat_account.categoryKind.creditBalance,
  },
  {
    optionId: "2",
    optionName: lang.cat_account.categoryKind.biAccount,
  },
  {
    optionId: "3",
    optionName: lang.cat_account.categoryKind.noBalance,
  },
];

const emits = defineEmits(["updateAccList"]);
resetFormState();
//#endregion

//#region hook
onMounted(async () => {
  try {
    // Nếu form là kiểu thông tin account mà id của router không hợp lệ thì quay lại
    if (form.value.type == $enum.form.editType && !isUUID(form.value.accId)) {
      await router.replace("/DI/DIAccount");
      return;
    }
    form.value.isLoading = true;
    // Lấy dữ liệu từ Server
    await getDataFromApi();
    form.value.isLoading = false;
    // Focus vào ô nhập liệu đầu tiên
    focusOnFirstInput();
  } catch (error) {
    form.value.isLoading = false;
    console.log(error);
    await handleResponseStatusCode(error);
  }
});

watch(
  () => account.value.parentNumber,
  () => {
    if (
      formNoti.value.accountNumber ==
      "Số tài khoản có độ dài > 3 ký tự phải điền thông tin <tài khoản tổng hợp>"
    ) {
      formNoti.value.accountNumber = "";
    }
  }
);
//#endregion

//#region function

function focusOnFirstInput() {
  // need
}

/**
 * Reset giá trị và trạng thái form
 *
 * Author: Dũng (08/05/2023)
 */
function resetFormState() {
  form.value = {
    size: 0,
    collapse: true,
    type: route.params.id ? $enum.form.editType : $enum.form.createType,
    accId: route.params.id ?? "",
    isLoading: false,
  };
  formNoti.value = {
    showNotibox: false,
    notiboxType: "",
    notiboxMessage: "",

    accountNumber: "",
    accountNameVi: "",
    accountKind: "",
    categoryKind: "",
  };
  account.value = new Account({});
}

/**
 * Quản lý các mã HTTP Code trả về sau khi gọi API
 * @param {code}
 * Author: Dũng (08/05/2023)
 */
async function handleResponseStatusCode(error) {
  if (error == null || error.response == null) return;
  let code = error.response.status;
  formNoti.value.notiboxType = "alert";
  if (code == 400) {
    // Trường hợp backend trả về BadRequest
    formNoti.value.notiboxMessage = $error.invalidInput;
    // await displayNotiBox();
  } else if (code == 404) {
    // Trường hợp không tìm thấy ID của nhân viên trên URL
    await router.replace("/DI/DICustomer");
  } else {
    // Các trường hợp còn lại
    formNoti.value.notiboxMessage = error.response.data.UserMessage;
    // await displayNotiBox();
  }
}

/**
 * Gọi API lấy thông tin account và gán vào account object
 * @param {String} accId Id account
 *
 * Author: Dũng (28/06/2023)
 */
async function fecthAccountToAccountObject(accId) {
  const response = await $axios.get($api.account.one(accId));
  const accFromApi = response.data;
  account.value = new Account(accFromApi);
}

/**
 * Gọi API khởi tạo dữ liệu cho form
 *
 * Author: Dũng (08/05/2023)
 */
async function getDataFromApi() {
  if (form.value.type == $enum.form.editType) {
    await fecthAccountToAccountObject(form.value.accId, form.value.type);
    const oldAcc = new Account({});
    oldAcc.cloneFromOtherAccount(account.value);
    oldAccount = oldAcc;
    return;
  }
}

/**
 * Kiểm tra một string có phải UUID không
 *
 * Author: Dũng (11/06/2023)
 */
function isUUID(str) {
  const uuidRegex =
    /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/;
  return uuidRegex.test(str);
}
//#endregion

//#region handle event

function collapseOnClick() {
  form.value.collapse = !form.value.collapse;
}

function formExpandOnClick() {
  console.log(form.value.size);
  form.value.size = 1 - form.value.size;
}

function closeBtnOnClick() {
  router.replace("/DI/DIAccount");
}

/**
 * Validate các thông tin có trong form
 *
 * Author: Dũng (08/05/2023)
 */
async function validateData() {
  firstErrorRef = null;

  // Số tài khoản
  // Trống
  if (account.value.accountNumber.trim() == "") {
    account.value.accountNumber = "";
    formNoti.value.accountNumber = $error.fieldCannotEmpty("Số tài khoản");
    firstErrorRef = firstErrorRef ?? accountNumberRef;
  } else {
    // Số TK quá ngắn
    if (account.value.accountNumber.length < 3) {
      formNoti.value.accountNumber = "Số tài khoản phải có độ dài >= 3 ký tự";
      firstErrorRef = firstErrorRef ?? accountNumberRef;
    }

    // Số TK quá dài
    if (account.value.accountNumber.length > 50) {
      formNoti.value.accountNumber = $error.fieldTooLong("Số tài khoản", 50);
      firstErrorRef = firstErrorRef ?? accountNumberRef;
    }

    // Tài khoản không bắt đầu bằng tài khoản tổng hợp
    if (account.value.accountNumber.length > 3) {
      if (account.value.parentNumber == "") {
        formNoti.value.accountNumber =
          "Số tài khoản có độ dài > 3 ký tự phải điền thông tin <tài khoản tổng hợp>";
        firstErrorRef = firstErrorRef ?? accountNumberRef;
      }
    }

    // Tài khoản không bắt đầu bằng tài khoản tổng hợp
    if (
      account.value.parentNumber != "" &&
      !account.value.accountNumber.startsWith(account.value.parentNumber)
    ) {
      formNoti.value.accountNumber =
        "Số tài khoản chi tiết phải bắt đầu bằng số tài khoản tổng hợp";
      firstErrorRef = firstErrorRef ?? accountNumberRef;
    }

    // Trùng số TK
    const isAccountNumberExist = await isAccNumberExist(
      account.value.accountNumber,
      form.value.accId
    );
    if (isAccountNumberExist) {
      formNoti.value.accountNumber = "Số tài khoản đã tồn tại";
      firstErrorRef = firstErrorRef ?? accountNumberRef;
    }
  }

  // Tên tài khoản
  // Tên bị trống
  if (account.value.accountNameVi.trim() == "") {
    formNoti.value.accountNameVi = $error.fieldCannotEmpty("Tên tài khoản");
    firstErrorRef = firstErrorRef ?? accountNameViRef;
  } else {
    // Tên quá dài
    if (account.value.accountNameVi.length > 100) {
      formNoti.value.accountNameVi = $error.fieldTooLong("Tên tài khoản", 100);
      firstErrorRef = firstErrorRef ?? accountNameViRef;
    }
  }

  // Tính chất
  if (account.value.categoryKindName.trim() == "") {
    formNoti.value.categoryKind = $error.fieldCannotEmpty("Tính chất");
    firstErrorRef = firstErrorRef ?? accountKindRef;
  }

  if (firstErrorRef != null) {
    // Update notibox value
    formNoti.value.notiboxType = "alert";
    formNoti.value.notiboxMessage = $error.invalidInput;
  } else {
    formNoti.value.notiboxMessage = "";
  }
}

/**
 * Hiển thị notibox thông báo
 * Author: Duxng(03/06/2023)
 */
async function displayNotiBox() {
  formNoti.value.showNotibox = true;
  await nextTick();
  notiRef.value.yesBtn.refBtn.focus();
}

/**
 * Gọi API tạo mới khách hàng
 *
 * Author: Dũng (08/05/2023)
 */
async function callCreateAccountApi() {
  const requestBody = account.value.convertToApiFormat();
  console.log(requestBody);
  const response = await $axios.post($api.account.index, requestBody);
  return response.data;
}

/**
 * Sự kiện click vào nút cất
 *
 *
 * Author: Dũng (27/06/2023)
 */
async function btnSaveOnClick() {
  try {
    form.value.isLoading = true;

    // Validate dữ liệu của các trường thông tin
    await validateData();

    // Nếu có một lỗi nào đó sau khi validate
    if (formNoti.value.notiboxMessage.length) {
      form.value.isLoading = false;
      // show notibox
      await displayNotiBox();
    } else {
      // Nếu Validate thành công

      // Nếu form là form cập nhật thông tin
      if (form.value.type == $enum.form.editType) {
        // Gọi API sửa nhân viên
        await callEditAccountApi();
        // Emit sự kiện cập nhật lên List để cập nhật trên table
        if (oldAccount.parentId != account.value.parentId) {
          // reload lại cây account
          emits("updateAccList", "edit", {
            account: account.value,
            reload: true,
          });
        } else {
          // không cần reload cây account
          emits("updateAccList", "edit", {
            account: account.value,
            reload: false,
          });
        }
      } else {
        // Nếu form là form thêm mới hoặc nhân bản
        // Gọi API thêm mới
        const newAccountId = await callCreateAccountApi();
        account.value.accountId = newAccountId;
        // Emit sự kiện thêm mới lênList để cập nhật trên table
        emits("updateAccList", "create", {
          account: account.value,
        });
      }

      form.value.isLoading = false;
      router.replace("/DI/DIAccount");
    }
  } catch (error) {
    form.value.isLoading = false;
    console.log(error);
    await handleResponseStatusCode(error);
  }
}

async function btnSaveAndAddOnClick() {
  try {
    form.value.isLoading = true;
    // Validate dữ liệu
    await validateData();

    // Nếu có một lỗi nào đó sau khi validate
    if (formNoti.value.notiboxMessage != "") {
      form.value.isLoading = false;
      // show notibox
      await displayNotiBox();
    } else {
      // Nếu validate thành công

      // Nếu form là form cập nhật thông tin
      if (form.value.type == $enum.form.editType) {
        // edit
        await callEditAccountApi();
        // Emit sự kiện cập nhật lên List để cập nhật trên table
        if (oldAccount.parentId != account.value.parentId) {
          // reload lại cây account
          emits("updateAccList", "edit", {
            account: account.value,
            reload: true,
          });
        } else {
          // không cần reload cây account
          emits("updateAccList", "edit", {
            account: account.value,
            reload: false,
          });
        }
      } else {
        // Nếu form là form thêm mới hoặc nhân bản
        // Gọi API thêm mới
        const newAccountId = await callCreateAccountApi();
        account.value.accountId = newAccountId;
        // Emit sự kiện thêm mới lênList để cập nhật trên table
        emits("updateAccList", "create", {
          account: account.value,
        });
      }

      form.value.isLoading = false;
      // Quay lại trang /DI/DIEmployee/create
      await router.replace("/DI/DIAccount/create");

      // Reset lại các trường thông tin trên form
      resetFormState();

      // Focus vào ô mã nhân viên
      accountNumberRef.value.refInput.focus();
    }
  } catch (error) {
    form.value.isLoading = false;
    await handleResponseStatusCode(error);
  }
}

async function isAccNumberExist(accNumber, accId) {
  const response = await $axios.get($api.account.checkNumberExist, {
    params: {
      id: accId,
      code: accNumber,
    },
  });
  return response.data;
}

/**
 * Gọi API sửa account
 *
 * Author: Dũng (29/06/2023)
 */
async function callEditAccountApi() {
  const requestBody = account.value.convertToApiFormat();
  await $axios.put($api.account.one(form.value.accId), requestBody);
}

/**
 * Focus vào ô lỗi đầu tiên
 *
 * Author: Dũng (08/05/2023)
 */
function focusOnFirstErrorInput() {
  firstErrorRef.value.refInput.focus();
}

/**
 * Sự kiện click vào nút có trong notibox
 * Author: Dũng (08/05/2023)
 */
function formNotiboxYesBtnOnClick() {
  formNoti.value.showNotibox = false;
  focusOnFirstErrorInput();
}

/**
 * Sự kiện click vào nút đóng dialog
 * Author: Dũng (08/05/2023)
 */
async function formDialogCloseBtnOnClick() {
  formDialog.value.isShow = false;
  // Next tick để đợi đến khi formDialog được ẩn đi
  await nextTick();
  if (firstErrorRef != null) {
    focusOnFirstErrorInput();
  }
}

/**
 * Sự kiện click vào nút không trong notibox
 * Author: Dũng (08/05/2023)
 */
function formDialogNoBtnOnClick() {
  formDialog.value.isShow = false;
  router.replace("/DI/DICustomer");
}

/**
 * Sự kiện click vào btn yes khi đóng dialog
 *
 * Author: Dũng (27/05/2023)
 */
async function formDialogYesBtnOnClick() {
  formDialog.value.isShow = false;
  await btnSaveOnClick();
}

function detailByAccountObjectOnClick() {
  account.value.detailByAccountObject = !account.value.detailByAccountObject;
}

function btnCancelOnClick() {
  router.replace("/DI/DIAccount");
}

//#endregion
</script>

<style
  scoped
  lang="css"
  src="../../../../css/components/views/category/account/account-form.css"
></style>

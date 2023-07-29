export class Account {
  accountId;
  accountNumber;
  accountNameVi;
  accountNameEn;
  parentId;
  parentNumber;
  categoryKind;
  categoryKindName;
  description;
  isParent;
  grade;
  mCodeId;
  detailByBankAccount;
  detailByAccountObject;
  detailByAccountObjectKind;
  foreignCurrencyAccounting;
  usingStatus;
  usingStatusName;
  constructor(a) {
    this.accountId = a.accountId ?? "";
    this.accountNumber = a.accountNumber ?? "";
    this.accountNameVi = a.accountNameVi ?? "";
    this.accountNameEn = a.accountNameEn ?? "";
    this.parentId = a.parentId ?? "";
    this.parentNumber = a.parentNumber ?? "";
    this.categoryKind = a.categoryKind != null ? a.categoryKind.toString() : "";
    this.categoryKindName = a.categoryKindName ?? "";
    this.description = a.description ?? "";
    this.isParent = a.isParent ?? false;
    this.grade = a.grade ?? 0;
    this.mCodeId = a.mCodeId ?? "";
    this.detailByBankAccount = a.detailByBankAccount;
    this.detailByAccountObject = a.detailByAccountObject;
    this.detailByAccountObjectKind = a.detailByAccountObjectKind;
    this.foreignCurrencyAccounting = a.foreignCurrencyAccounting ?? false;
    this.usingStatus = a.usingStatus ?? true;
    this.usingStatusName = a.usingStatusName ?? "Đang sử dụng";
  }

  cloneFromOtherAccount(a) {
    this.accountId = a.accountId;
    this.accountNumber = a.accountNumber;
    this.accountNameVi = a.accountNameVi;
    this.accountNameEn = a.accountNameEn;
    this.parentId = a.parentId;
    this.parentNumber = a.parentNumber;
    this.categoryKind = a.categoryKind;
    this.categoryKindName = a.categoryKindName;
    this.description = a.description;
    this.isParent = a.isParent;
    this.grade = a.grade;
    this.mCodeId = a.mCodeId;
    this.detailByBankAccount = a.detailByBankAccount;
    this.detailByAccountObject = a.detailByAccountObject;
    this.detailByAccountObjectKind = a.detailByAccountObjectKind;
    this.foreignCurrencyAccounting = a.foreignCurrencyAccounting;
    this.usingStatus = a.usingStatus;
    this.usingStatusName = a.usingStatusName;
  }

  convertToApiFormat() {
    const obj = {
      accountNumber: this.accountNumber,
      accountNameVi: this.accountNameVi,
      accountNameEn: this.accountNameEn,
      parentId: this.parentId != "" ? this.parentId : null,
      parentNumber: this.parentNumber,
      categoryKind: this.categoryKind,
      categoryKindName: this.categoryKindName,
      description: this.description,
      detailByBankAccount: this.detailByBankAccount,
      detailByAccountObject: this.detailByAccountObject,
      detailByAccountObjectKind: this.detailByAccountObjectKind,
      foreignCurrencyAccounting: this.foreignCurrencyAccounting,
      usingStatus: this.usingStatus,
      usingStatusName: this.usingStatusName,
    };
    return obj;
  }
}

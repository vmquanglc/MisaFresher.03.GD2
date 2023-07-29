import $formatter from "../common/formater";
import numberFormater from "../common/number-formater";
import { v4 as uuidv4 } from "uuid";
export class Customer {
  customerId;
  customerType;
  customerTIN;
  customerCode;
  customerFullName;
  employeeId;
  employeeFullName;
  address;
  phoneNumber;
  website;
  contactNamePrefix;
  contactName;
  contactMobile;
  contactEmail;
  landLineNumber;
  legalRepresentative;
  envoiceContactName;
  envoiceContactEmail;
  envoiceContactMobile;
  paymentTermName;
  dueTime;
  maximizeDebtAmount;
  receiveAccount;
  payAccount;
  payAccountId;
  receiveAccountId;
  bankAccountList;
  country;
  provinceOrCity;
  district;
  wardOrCommune;
  countryId;
  provinceOrCityId;
  districtId;
  wardOrCommuneId;
  shippingAddressList;
  description;
  identityNumber;
  identityDate;
  identityPlace;
  groupCodeList;

  constructor(e) {
    this.customerId = e.customerId ?? "";
    this.customerType = e.customerType ?? 0;
    this.isProvider = e.isProvider ?? false;
    this.customerTIN = e.customerTIN ?? "";
    this.customerCode = e.customerCode ?? "";
    this.customerFullName = e.customerFullName ?? "";
    this.employeeId = e.employeeId ?? null;
    this.employeeFullName = e.employeeFullName ?? "";
    this.address = e.address ?? "";
    this.phoneNumber = e.phoneNumber ?? "";
    this.website = e.website ?? "";
    this.contactNamePrefix = e.contactNamePrefix ?? "";
    this.contactName = e.contactName ?? "";
    this.contactMobile = e.contactMobile ?? "";
    this.contactEmail = e.contactEmail ?? "";
    this.landLineNumber = e.landLineNumber ?? "";
    this.legalRepresentative = e.legalRepresentative ?? "";
    this.envoiceContactName = e.envoiceContactName ?? "";
    this.envoiceContactEmail = e.envoiceContactEmail ?? "";
    this.envoiceContactMobile = e.envoiceContactMobile ?? "";
    this.paymentTermName = e.paymentTermName ?? "";
    this.dueTime = "";
    if (e.dueTime != null && e.dueTime > 0) {
      this.dueTime = e.dueTime.toString();
    }
    this.maximizeDebtAmount = "";
    if (e.maximizeDebtAmount != null && e.maximizeDebtAmount > 0) {
      this.maximizeDebtAmount = numberFormater.format(e.maximizeDebtAmount);
    }
    this.receiveAccount = e.receiveAccount ?? "";
    this.payAccount = e.payAccount ?? "";
    this.payAccountId = e.payAccountId ?? "";
    this.receiveAccountId = e.receiveAccountId ?? "";
    this.bankAccountList = [];
    if (e.bankAccountList != null) {
      this.bankAccountList = JSON.parse(e.bankAccountList);
    }
    if (this.bankAccountList.length == 0) {
      this.bankAccountList.push({
        id: uuidv4(),
        bankAccount: "",
        bankName: "",
        bankBranch: "",
        bankProvince: "",
      });
    }
    this.country = e.country ?? "";
    this.provinceOrCity = e.provinceOrCity ?? "";
    this.district = e.district ?? "";
    this.wardOrCommune = e.wardOrCommune ?? "";
    this.countryId = String(e.countryId);
    this.provinceOrCityId = String(e.provinceOrCityId);
    this.districtId = String(e.districtId);
    this.wardOrCommuneId = String(e.wardOrCommuneId);
    this.shippingAddressList = {
      sameOfAddress: false,
      list: [
        {
          id: uuidv4(),
          address: "",
        },
      ],
    };
    if (e.shippingAddressList != null) {
      let eAddressList = JSON.parse(e.shippingAddressList);
      if (eAddressList.list.length > 0) {
        this.shippingAddressList.sameOfAddress = eAddressList.sameOfAddress;
        this.shippingAddressList.list = eAddressList.list;
      }
    }
    this.description = e.description ?? "";
    this.identityNumber = e.identityNumber ?? "";
    this.identityDate = $formatter.changeFormat(e.identityDate);
    this.identityPlace = e.identityPlace ?? "";
    this.groupCodeList = e.groupCodeList ? e.groupCodeList.split(",") : [];
  }

  /**
   * Trả về Object với định dạng là Body của request
   *
   * Author: Dũng (12/05/2023)
   */
  convertToApiFormat() {
    let obj = {
      customerType: this.customerType,
      isProvider: this.isProvider,
      customerTIN: this.customerTIN,
      customerCode: this.customerCode,
      customerFullName: this.customerFullName,
      employeeId: this.employeeId?.length > 0 ? this.employeeId : null,
      address: this.address,
      phoneNumber: this.phoneNumber,
      website: this.website,
      contactNamePrefix: this.contactNamePrefix,
      contactName: this.contactName,
      contactMobile: this.contactMobile,
      contactEmail: this.contactEmail,
      landLineNumber: this.landLineNumber,
      legalRepresentative: this.legalRepresentative,
      envoiceContactName: this.envoiceContactName,
      envoiceContactEmail: this.envoiceContactEmail,
      envoiceContactMobile: this.envoiceContactMobile,
      paymentTermName: this.paymentTermName,
      dueTime: this.dueTime != "" ? Number(this.dueTime) : null,
      maximizeDebtAmount: numberFormater.getNumber(this.maximizeDebtAmount),
      receiveAccount: this.receiveAccount,
      payAccount: this.payAccount,
      payAccountId: this.payAccountId?.length > 0 ? this.payAccountId : null,
      receiveAccountId:
        this.receiveAccountId?.length > 0 ? this.receiveAccountId : null,
      bankAccountList: JSON.stringify(this.bankAccountList),
      country: this.country,
      provinceOrCity: this.provinceOrCity,
      district: this.district,
      wardOrCommune: this.wardOrCommune,
      countryId: Number(this.countryId),
      provinceOrCityId: Number(this.provinceOrCityId),
      districtId: Number(this.districtId),
      wardOrCommuneId: Number(this.wardOrCommuneId),
      shippingAddressList: JSON.stringify(this.shippingAddressList),
      description: this.description,
      identityNumber: this.identityNumber,
      identityDate: $formatter.formatDateToApiDate(this.identityDate),
      identityPlace: this.identityPlace,
      groupCodeList: this.groupCodeList.join(","),
    };
    return obj;
  }

  cloneFromOtherCustomer(e) {
    this.customerId = e.customerId;
    this.customerType = e.customerType;
    this.isProvider = e.isProvider;
    this.customerTIN = e.customerTIN;
    this.customerCode = e.customerCode;
    this.customerFullName = e.customerFullName;
    this.employeeId = e.employeeId;
    this.employeeFullName = e.employeeFullName;
    this.address = e.address;
    this.phoneNumber = e.phoneNumber;
    this.website = e.website;
    this.contactNamePrefix = e.contactNamePrefix;
    this.contactName = e.contactName;
    this.contactMobile = e.contactMobile;
    this.contactEmail = e.contactEmail;
    this.landLineNumber = e.landLineNumber;
    this.legalRepresentative = e.legalRepresentative;
    this.envoiceContactName = e.envoiceContactName;
    this.envoiceContactEmail = e.envoiceContactEmail;
    this.envoiceContactMobile = e.envoiceContactMobile;
    this.paymentTermName = e.paymentTermName;
    this.dueTime = e.dueTime;
    this.maximizeDebtAmount = e.maximizeDebtAmount;
    this.receiveAccount = e.receiveAccount;
    this.payAccount = e.payAccount;
    this.payAccountId = e.payAccountId;
    this.receiveAccountId = e.receiveAccountId;
    this.bankAccountList = e.bankAccountList;
    this.country = e.country;
    this.provinceOrCity = e.provinceOrCity;
    this.district = e.district;
    this.wardOrCommune = e.wardOrCommune;
    this.countryId = e.countryId;
    this.provinceOrCityId = e.provinceOrCityId;
    this.districtId = e.districtId;
    this.wardOrCommuneId = e.wardOrCommuneId;
    this.shippingAddressList = e.shippingAddressList;
    this.description = e.description;
    this.identityNumber = e.identityNumber;
    this.identityDate = e.identityDate;
    this.identityPlace = e.identityPlace;
    this.groupCodeList = e.groupCodeList;
  }
}

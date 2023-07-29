import { v4 as uuidv4 } from "uuid";
import numberFormater from "../common/number-formater";
export class ReceiptDetail {
  status;
  receiptDetailId;
  receiptId;
  description;
  debitAccountId;
  debitAccountNumber;
  creditAccountId;
  creditAccountNumber;
  customerCode;
  customerName;
  amount;

  constructor(r) {
    this.receiptDetailId = r.receiptDetailId ?? "";
    // "create" "view" "update" "delete"
    this.status = this.receiptDetailId == "" ? "create" : "view";
    if (this.status == "create") this.receiptDetailId = uuidv4();
    this.receiptId = r.receiptId ?? "";
    this.description = r.description ?? "";
    this.debitAccountId = r.debitAccountId ?? "";
    this.debitAccountNumber = r.debitAccountNumber ?? "";
    this.creditAccountId = r.creditAccountId ?? "";
    this.creditAccountNumber = r.creditAccountNumber ?? "";
    this.customerCode = r.customerCode ?? "";
    this.customerName = r.customerName ?? "";
    this.amount = numberFormater.format(r.amount) ?? "";
  }

  cloneFromOther(r) {
    this.status = r.status;
    this.receiptDetailId = r.receiptDetailId;
    this.receiptId = r.receiptId;
    this.description = r.description;
    this.debitAccountId = r.debitAccountId;
    this.debitAccountNumber = r.debitAccountNumber;
    this.creditAccountId = r.creditAccountId;
    this.creditAccountNumber = r.creditAccountNumber;
    this.customerCode = r.customerCode;
    this.customerName = r.customerName;
    this.amount = r.amount;
  }

  copyValueOf(r) {
    this.receiptId = r.receiptId;
    this.description = r.description;
    this.debitAccountId = r.debitAccountId;
    this.debitAccountNumber = r.debitAccountNumber;
    this.creditAccountId = r.creditAccountId;
    this.creditAccountNumber = r.creditAccountNumber;
    this.customerCode = r.customerCode;
    this.customerName = r.customerName;
    this.amount = r.amount;
  }

  convertToApiFormat() {
    const obj = {
      status: this.status,
      receiptDetailId: this.receiptDetailId,
      receiptId: this.receiptId.length > 0 ? this.receiptId : null,
      description: this.description,
      debitAccountId:
        this.debitAccountId.length > 0 ? this.debitAccountId : null,
      debitAccountNumber: this.debitAccountNumber,
      creditAccountId:
        this.creditAccountId.length > 0 ? this.creditAccountId : null,
      creditAccountNumber: this.creditAccountNumber,
      customerCode: this.customerCode,
      customerName: this.customerName,
      amount: numberFormater.getNumber(this.amount),
    };
    return obj;
  }
}

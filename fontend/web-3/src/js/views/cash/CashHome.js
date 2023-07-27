const mixin = {
  name: "CashHome",
  data() {
    return {
      cashHomeKey: 0,
      dialog: {},
    };
  },
  watch: {
    ids() {
      console.log(this.ids.length);
    },
  },
  computed: {
    navTab() {
      if (this.$route.name == "business-process") {
        return 0;
      }
      if (this.$route.name == "receipt-list") {
        return 1;
      }
    },
  },
  created() {
    this.$msEmitter.on("showDialog", this.showDialog);
    this.$msEmitter.on("hideDialog", this.hideDialog);
    this.$msEmitter.on("reRenderCashHome", this.reRenderCashHome);
  },
  beforeUnmount() {
    this.$msEmitter.off("showDialog");
    this.$msEmitter.off("hideDialog");
    this.$msEmitter.off("reRenderCashHome");
  },
  methods: {
    reRenderCashHome() {
      this.cashHomeKey++;
    },
    /**
     * Hàm ẩn dialog
     * @param {Enum} answer
     * Author: LeDucTiep (12/07/2023)
     */
    async hideDialog(answer) {
      switch (this.dialog.dialogType) {
        case this.$msEnum.DialogType.Delete:
          // Nếu user đã trả lời và đã xác định được receipt cần xóa
          if (this.ReceiptOnMenu && answer == this.$msEnum.DialogAnswer.Yes) {
            // Thì xóa thông tin nhân viên đó
            this.$msEmitter.emit("deleteReceipt", this.ReceiptOnMenu);
            // this.deleteReceipt(this.ReceiptOnMenu);
          }
          break;

        case this.$msEnum.DialogType.DeleteSelected:
          // Nếu user đã trả lời và đã xác định được receipt cần xóa
          if (answer == this.$msEnum.DialogAnswer.Yes) {
            console.log("ahihi");
            // Xóa các phần tử đã chọn
            // this.deleteListReceipts(this.selectedReceiptIdList);
            this.$msEmitter.emit("deleteListReceipts");
          }
          break;

        case this.$msEnum.DialogType.AlertFormChanged:
          // Nếu user đã trả lời
          this.$msEmitter.emit("closeReceiptFormChanged", answer);
          break;

        default:
          break;
      }
      this.dialog.isShowDialog = false;
    },

    /**
     * Hàm hiển thị dialog
     * @param {*} type Loại dialog
     * @param {*} messageList Danh sách thông báo
     * @param {*} receipt Phiếu thu
     * Author: LeDucTiep (12/07/2023)
     */
    showDialog(type, messageList, receipt) {
      // Định kiểu cho dialog
      this.dialog = {
        dialogType: type,
        isShowDialog: true,
        items: messageList,
      };

      // Truyền vào receipt nếu là dialog delete
      switch (type) {
        case this.$msEnum.DialogType.Delete:
          this.dialog = {
            ...this.dialog,
            title: this.$t("Dialog.DeleteRecord"),
            isWarning: true,
            contentNo: this.$t("Button.No"),
            contentYes: this.$t("Button.Delete"),
          };
          this.ReceiptOnMenu = receipt;
          break;

        case this.$msEnum.DialogType.Error:
          this.dialog = {
            ...this.dialog,
            title: this.$t("Dialog.ErrorTitle"),
            isWarning: false,
            contentYes: this.$t("Button.Accept"),
          };
          break;
        case this.$msEnum.DialogType.DeleteSelected:
          this.dialog = {
            ...this.dialog,
            title: this.$t("Dialog.DeleteRecord"),
            isWarning: true,
            contentNo: this.$t("Button.No"),
            contentYes: this.$t("Button.Delete"),
          };
          break;

        case this.$msEnum.DialogType.AlertFormChanged:
          this.dialog = {
            ...this.dialog,
            title: this.$t("Dialog.SaveChanges"),
            isWarning: false,
            contentCancel: this.$t("Button.Cancel"),
            contentNo: this.$t("Button.No"),
            contentYes: this.$t("Button.Save"),
          };
      }
    },
  },
};

export default mixin;

const mixin = {
  name: "receiptMenuFeature",
  data() {
    return {
      // receipt đã hiện menu
      receipt: null,
      // Là menu đang mở
      isOpen: false,
      // Vị trí hiển thị của menu
      left: 0,
      top: 0,
    };
  },
  methods: {
    async setRecorded() {
      if (this.receipt.ReceiptId) {
        this.receipt.IsRecorded = true;
        const response = await this.$msAxios(
          "put",
          this.$msApi.ReceiptApi.SettingRecorded + "/" + this.receipt.ReceiptId,
          {
            data: true,
          }
        );
        if (response?.data) {
          this.$msEmitter.emit("addNotice", {
            type: this.$msEnum.NoticeType.Success,
            message: this.$t("Notice.BookKeeping.Success"),
          });
        } else {
          this.$msEmitter.emit("addNotice", {
            type: this.$msEnum.NoticeType.Error,
            message: this.$t("Notice.BookKeeping.Error"),
          });
        }
      }
      this.isOpen = false;
    },

    async unSetRecorded() {
      if (this.receipt.ReceiptId) {
        this.receipt.IsRecorded = false;
        const response = await this.$msAxios(
          "put",
          this.$msApi.ReceiptApi.SettingRecorded + "/" + this.receipt.ReceiptId,
          {
            data: false,
          }
        );
        if (response?.data) {
          this.$msEmitter.emit("addNotice", {
            type: this.$msEnum.NoticeType.Success,
            message: this.$t("Notice.UnBookKeeping.Success"),
          });
        } else {
          this.$msEmitter.emit("addNotice", {
            type: this.$msEnum.NoticeType.Error,
            message: this.$t("Notice.UnBookKeeping.Error"),
          });
        }
      }
      this.isOpen = false;
    },

    /**
     * Xem receipt đã kích hoạt menu khi nhấn nút xem
     * Author: LeDucTiep (04/05/2023)
     */
    receiptViewOnClick() {
      // Chuyển hướng đến trang nhân bản
      this.$router.push(`/cash/view/${this.receipt.ReceiptId}`);
      // Đóng menu
      this.isOpen = false;
    },

    /**
     * Xóa receipt đã kích hoạt menu khi nhấn nút xóa
     * Author: LeDucTiep (04/05/2023)
     */
    receiptDeleteOnClick() {
      // Hiển thị dialog comfirm
      // Bạn có chắc chắn xóa
      this.$msEmitter.emit(
        "showDialog",
        this.$msEnum.DialogType.Delete,
        [
          this.$t("DeleteAlert", [
            this.receipt.ReceiptCode,
            this.receipt.FullName,
          ]).replace(" ?", "?"),
        ],
        this.receipt
      );
      // Đóng menu
      this.isOpen = false;
    },
  },
  created() {
    // Toggle menu
    this.$msEmitter.on("toggleMenuFeature", (event, receipt) => {
      // Chủ của menu được kích hoạt
      this.receipt = receipt;
      this.isOpen = !this.isOpen;
      if (!this.isOpen || !this.$refs.tiepMenu) return;

      let menuWidth = 100;
      let menuHeight = 100;

      // Tính toán tọa độ mở ra menu
      let right = event.clientX + menuWidth;
      let left = event.clientX - menuWidth;

      if (right > window.innerWidth) {
        this.left = left;
      } else if (left < 0) {
        this.left = event.clientX;
      } else {
        this.left = event.clientX - menuWidth / 2;
      }

      if (event.clientY + menuHeight > window.innerHeight)
        this.top = event.clientY - menuHeight;
      else this.top = event.clientY + 20;
    });
  },
  beforeUnmount() {
    // Hủy sự kiện
    this.$msEmitter.off("toggleMenuFeature");
  },
};

export default mixin;

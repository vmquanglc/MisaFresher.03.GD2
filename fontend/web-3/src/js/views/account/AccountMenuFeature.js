const mixin = {
  name: "AccountMenuFeature",
  data() {
    return {
      // Account đã hiện menu
      account: null,
      // Là menu đang mở
      isOpen: false,
      // Vị trí hiển thị của menu
      left: 0,
      top: 0,
    };
  },
  methods: {
    /**
     * Xem account đã kích hoạt menu khi nhấn nút xem
     * Author: LeDucTiep (04/05/2023)
     */
    accountViewOnClick() {
      // Chuyển hướng đến trang nhân bản
      this.$router.push(`/account/view/${this.account.AccountId}`);
      // Đóng menu
      this.isOpen = false;
    },

    /**
     * Xóa account đã kích hoạt menu khi nhấn nút xóa
     * Author: LeDucTiep (04/05/2023)
     */
    accountDeleteOnClick() {
      // Hiển thị dialog comfirm
      // Bạn có chắc chắn xóa
      this.$msEmitter.emit(
        "showDialog",
        this.$msEnum.DialogType.Delete,
        [
          this.$t("DeleteAlert", [
            this.account.AccountCode,
            this.account.FullName,
          ]).replace(" ?", "?"),
        ],
        this.account
      );
      // Đóng menu
      this.isOpen = false;
    },
  },
  created() {
    // Toggle menu
    this.$msEmitter.on("toggleMenuFeature", (event, account) => {
      // Chủ của menu được kích hoạt
      this.account = account;
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

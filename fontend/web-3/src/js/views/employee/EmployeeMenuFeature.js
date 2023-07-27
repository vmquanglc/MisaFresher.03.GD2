const mixin = {
  name: "EmployeeMenuFeature",
  data() {
    return {
      // Employee đã hiện menu
      employee: null,
      // Là menu đang mở
      isOpen: false,
      // Vị trí hiển thị của menu
      left: 0,
      top: 0,
    };
  },
  methods: {
    /**
     * Xóa employee đã kích hoạt menu khi nhấn nút xóa
     * Author: LeDucTiep (04/05/2023)
     */
    employeeDuplicateOnClick() {
      // Chuyển hướng đến trang nhân bản
      this.$router.push(`/employee/duplicate/${this.employee.EmployeeId}`);
      // Đóng menu
      this.isOpen = false;
    },

    /**
     * Xóa employee đã kích hoạt menu khi nhấn nút xóa
     * Author: LeDucTiep (04/05/2023)
     */
    employeeDeleteOnClick() {
      // Hiển thị dialog comfirm
      // Bạn có chắc chắn xóa
      this.$msEmitter.emit(
        "showDialog",
        this.$msEnum.DialogType.Delete,
        [
          this.$t("DeleteAlert", [
            this.employee.EmployeeCode,
            this.employee.FullName,
          ]).replace(" ?", "?"),
        ],
        this.employee
      );
      // Đóng menu
      this.isOpen = false;
    },
  },
  created() {
    // Toggle menu
    this.$msEmitter.on("toggleMenuFeature", (event, employee) => {
      // Chủ của menu được kích hoạt
      this.employee = employee;
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

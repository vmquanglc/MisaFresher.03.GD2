const mixin = {
  name: "EmployeeHome",
  data() {
    return {
      // Dùng để rerender lại form
      employeeFormKey: 0,
      // Dùng để tạo hiệu ứng loading
      isLoading: false,
      // Lưu từ khóa tìm kiếm
      inputSearchValue: "",

      columnList: [
        "EmployeeCode",
        "FullName",
        "Gender",
        "DateOfBirth",
        "IdentityNumber",
        "PositionName",
        "DepartmentName",
        "BankAccountNumber",
        "NameOfBank",
        "BankAccountBranch",
      ],
    };
  },

  created() {
    // reset form sau khi submit form
    this.$msEmitter.on("resetAddEmployeeForm", this.resetAddEmployeeForm);

    // set loading
    this.$msEmitter.on(
      "setLoading",
      (isLoading) => (this.isLoading = isLoading)
    );
  },
  mounted() {
    // Thêm sự kiện phím tắt
    document.addEventListener("keydown", this.keyEvent);
  },
  beforeUnmount() {
    // Hủy bỏ event
    // Phím tắt
    document.removeEventListener("keydown", this.keyEvent);
    // Bỏ lắng nghe sự kiện
    this.$msEmitter.off("resetAddEmployeeForm");
    this.$msEmitter.off("setLoading");
  },

  methods: {
    /**
     * Hàm xuất excel
     * Author: LeDucTiep (01/06/2023)
     */
    async exportEmployeeOnclick(event) {
      event.target.style.cursor = "wait";
      const start = Date.now();
      await this.exportInBackEnd();
      const end1 = Date.now();

      console.log(`Export from backend: ${end1 - start} ms`);
      event.target.style.cursor = "pointer";
    },

    /**
     * Hàm gọi api để kết xuất sữ liệu
     * Author: LeDucTiep (07/06/2023)
     */
    async exportInBackEnd() {
      // Lấy file về
      const response = await this.$msAxios(
        "get",
        this.$msApi.EmployeeApi.ExportExcel,
        {
          responseType: "blob",
          params: {
            // Kích thước của trang
            Fields: this.columnList,
          },
          paramsSerializer: function (params) {
            return require("qs").stringify(params, { arrayFormat: "repeat" });
          },
        }
      );

      if (!response) {
        this.$msEmitter.emit("addNotice", {
          type: this.$msEnum.NoticeType.Error,
          message: this.$t("Error.UnknownError"),
        });
        return false;
      }

      var FILE = window.URL.createObjectURL(new Blob([response.data]));

      // Lưu file
      var docUrl = document.createElement("a");
      docUrl.href = FILE;
      docUrl.setAttribute("download", this.$t("EmployeeExport.FileName"));
      document.body.appendChild(docUrl);
      docUrl.click();
    },

    /**
     * Hàm hiển thị thông báo tính năng chưa hỗ trọ
     * Author: LeDucTiep (12/07/2023)
     */
    notSupported() {
      this.$msEmitter.emit(
        "showDialog",
        this.$msEnum.DialogType.FeatureNotSupported,
        [this.$t("Dialog.FeatureNotSupported")]
      );
    },

    /**
     * Hàm sử lý sự kiện phím tắt
     * Author: LeDucTiep (28/05/2023)
     */
    keyEvent(event) {
      if (document.onkeydown) return;
      // Phím tắt thêm thông tin nhân viên
      if ((event.metaKey || event.ctrlKey) && event.key === "1") {
        event.preventDefault();
        if (
          ["EmployeePagingRouter", "EmployeeHomeRouter", "Home"].includes(
            this.$route.name
          )
        ) {
          this.addEmployee();
        }
      }
    },

    /**
     * Rerender employee form
     * Author: LeDucTiep (04/05/2023)
     */
    resetAddEmployeeForm() {
      // rerender form
      this.employeeFormKey += 1;
    },

    /**
     * Mở form thêm thông tin nhân viên
     * Author: LeDucTiep (04/05/2023)
     */
    addEmployee() {
      // Chuyển đến form thêm
      this.$router.push("/employee/create");
    },

    /**
     * Reload danh sách employee khi nhấn nút reload
     * Author: LeDucTiep (04/05/2023)
     */
    reloadEmployeeOnclick() {
      // Tải lại danh sách nhân viên
      this.$msEmitter.emit("reloadEmployeeOnclick");
    },
  },
};

export default mixin;

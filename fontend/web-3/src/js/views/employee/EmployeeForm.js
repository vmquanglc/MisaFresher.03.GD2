import { EmployeeFieldLength } from "../../enum/inputLength";

const mixin = {
  name: "EmployeeForm",
  props: ["id"],
  data() {
    return {
      // Tiêu đề của form
      title: "",
      // Thông tin nhập vào
      employee: {
        Gender: this.$msEnum.Gender.Male,
      },
      // chứa {field: isValid}
      inputValidation: {},
      // Thông tin nhân viên trước khi thay đổi
      employeeBeforeChange: {},
      // input giới tính
      inputRadioItems: [
        {
          id: 0,
          value: this.$t("Gender.Male"),
        },
        {
          id: 1,
          value: this.$t("Gender.Female"),
        },
        {
          id: 2,
          value: this.$t("Gender.Other"),
        },
      ],
      // Các trường bắt buộc
      requiredFields: [],
      // Các trường là ngày tháng
      dateFields: [],
      // Dùng cho combobox department
      departments: [],
      // Dùng cho combobox position
      positions: [],
      // Khi nhất submit thì kích hoạt validate: true
      isValidating: false,
      // Thông báo lỗi lỗi của ngày sinh
      dateOfBirthErrorMessage: this.$t("Error.IncorrectFormat"),
      // Thông báo lỗi lỗi của ngày cấp
      identityDateErrorMessage: this.$t("Error.IncorrectFormat"),
      // Đang xử lý
      isProcessing: false,
    };
  },
  async created() {
    this.$msEmitter.on(
      "closeEmployeeFormChanged",
      this.closeEmployeeFormChanged
    );
    // Load thông tin cho form
    // Nếu là form thêm
    if (this.$route.name == "EmployeeCreateRouter") {
      // Sửa title thành form thêm
      this.title = this.$t("FormTitle.Add");

      // Lấy mã employeeCode mới về
      const response = await this.$msAxios(
        "get",
        this.$msApi.EmployeeApi.GetNewCode
      );

      // Lỗi
      if (!response) {
        this.$msEmitter.emit("addNotice", {
          type: this.$msEnum.NoticeType.Error,
          message: this.$t("Notice.Put.Error"),
        });
        return false;
      }

      // Nhận mã code mới
      this.employee.EmployeeCode = response.data;
    } else if (this.$route.name == "EmployeeDuplicateRouter") {
      // Sửa title thành form thêm
      this.title = this.$t("FormTitle.Add");

      // Lấy thông tin nhân viên theo id
      let response = await this.$msAxios(
        "get",
        this.$msApi.EmployeeApi.Get(this.id)
      );

      if (!response || response.status != this.$msEnum.HttpStatusCode.Success) {
        this.$msEmitter.emit("showDialog", this.$msEnum.DialogType.Error, [
          this.$t("Error.UnknownError"),
        ]);
        return false;
      }

      // Nhận thông tin nhân viên cần sửa
      let employeeForEdit = response.data;

      if (employeeForEdit.DateOfBirth)
        employeeForEdit.DateOfBirth = this.$msDateFormater.convertIsoToDDMMYYY(
          employeeForEdit.DateOfBirth
        );

      if (employeeForEdit.IdentityDate)
        employeeForEdit.IdentityDate = this.$msDateFormater.convertIsoToDDMMYYY(
          employeeForEdit.IdentityDate
        );

      this.employee = { ...employeeForEdit };
      this.employeeBeforeChange = { ...employeeForEdit };

      // Lấy mã employeeCode mới về
      response = await this.$msAxios("get", this.$msApi.EmployeeApi.GetNewCode);

      if (!response) {
        this.$msEmitter.emit("showDialog", this.$msEnum.DialogType.Error, [
          this.$t("Error.UnknownError"),
        ]);
        return false;
      }

      // Nhận mã code mới
      this.employee.EmployeeCode = response.data;
    } else {
      // Nếu là form sửa
      // Sửa title thành form sửa
      this.title = this.$t("FormTitle.Edit");

      // Lấy thông tin nhân viên theo id
      const response = await this.$msAxios(
        "get",
        this.$msApi.EmployeeApi.Get(this.id)
      );

      if (!response || response.status != this.$msEnum.HttpStatusCode.Success) {
        this.$msEmitter.emit("showDialog", this.$msEnum.DialogType.Error, [
          this.$t("Error.UnknownError"),
        ]);
        return false;
      }

      // Nhận thông tin nhân viên cần sửa
      let employeeForEdit = response.data;

      if (employeeForEdit.DateOfBirth)
        employeeForEdit.DateOfBirth = this.$msDateFormater.convertIsoToDDMMYYY(
          employeeForEdit.DateOfBirth
        );

      if (employeeForEdit.IdentityDate)
        employeeForEdit.IdentityDate = this.$msDateFormater.convertIsoToDDMMYYY(
          employeeForEdit.IdentityDate
        );

      this.employee = { ...employeeForEdit };
      this.employeeBeforeChange = { ...employeeForEdit };
    }

    // Load thông tin cho combobox
    // lấy danh sách các đơn vị
    // Nếu là lần đầu mở form
    if (this.$msGlobalDepartments.length == 0) {
      const response = await this.$msAxios(
        "get",
        this.$msApi.DepartmentApi.GetAll
      );

      if (!response || response.status != this.$msEnum.HttpStatusCode.Success) {
        this.$msEmitter.emit("showDialog", this.$msEnum.DialogType.Error, [
          this.$t("Error.UnknownError"),
        ]);
        return false;
      }

      // Lưu thông tin vào biến toàn cục, lần mở form tiếp theo sẽ không phải load lại
      this.$msGlobalDepartments.push(...response.data);
    }

    this.$nextTick(() => {
      // lấy thông tin department từ biến toàn cục
      this.departments = this.$msGlobalDepartments;
    });

    // Lấy danh sách các chức danh
    // Nếu là lần đầu mở form
    if (this.$msGlobalPositions.length == 0) {
      const response = await this.$msAxios(
        "get",
        this.$msApi.PositionApi.GetAll
      );

      if (!response || response.status != this.$msEnum.HttpStatusCode.Success) {
        this.$msEmitter.emit("showDialog", this.$msEnum.DialogType.Error, [
          this.$t("Error.UnknownError"),
        ]);
        return false;
      }

      // Lưu thông tin vào biến toàn cục, lần mở form tiếp theo sẽ không phải load lại
      this.$msGlobalPositions.push(...response.data);
    }
    this.$nextTick(() => {
      // Lấy thông tin position từ biến toàn cục
      this.positions = this.$msGlobalPositions;
    });
  },
  mounted() {
    // Thêm sự kiện phím tắt
    document.addEventListener("keydown", this.keyEvent);
    this.focusFirstInput();
  },
  beforeUnmount() {
    // Xóa sự kiện
    document.removeEventListener("keydown", this.keyEvent);
    this.$msEmitter.off("closeEmployeeFormChanged");
  },
  methods: {
    /**
     * Hàm thực kiểm tra ngày sinh hợp lệ
     * @returns true: ngày sinh hợp lệ
     * Author: LeDucTiep (13/06/2023)
     */
    dateOfBirthConditional() {
      // Ngày sinh không được phép nhập lớn hơn ngày hiện tại
      const timeNow = new Date();
      const date = this.$msDateFormater.stringToDate(this.employee.DateOfBirth);

      if (date && date.getTime() > timeNow.getTime()) {
        this.dateOfBirthErrorMessage = this.$t("Error.DateOfBirthTime");
        return false;
      }

      return true;
    },

    /**
     * Hàm kiểm tra ngày cấp
     * @returns true: ngày cấp là hợp lệ
     * Author: LeDucTiep (13/06/2023)
     */
    identityDateConditional() {
      // Ngày cấp không được phép nhập lớn hơn ngày hiện tại
      const timeNow = new Date();
      const date = this.$msDateFormater.stringToDate(
        this.employee.IdentityDate
      );

      if (date && date.getTime() > timeNow.getTime()) {
        this.identityDateErrorMessage = this.$t("Error.IdentityDateTime");
        return false;
      }

      return true;
    },

    /**
     * Hàm kiểm tra nội dung form đã thay đổi chưa
     * Author: LeDucTiep (04/06/2023)
     */
    isChanged() {
      // form sửa
      if (
        ["EmployeeEditRouter", "EmployeeDuplicateRouter"].includes(
          this.$route.name
        )
      ) {
        for (const i in this.employee) {
          if (["PositionName", "DepartmentName"].includes(i)) continue;
          if (this.employee[i] !== this.employeeBeforeChange[i]) {
            return true;
          }
        }
      } else {
        if (Object.keys(this.employee).length > 4) {
          return true;
        } else if (
          this.employee.Gender !== this.$msEnum.Gender.Male ||
          this.employee.DepartmentName !== "" ||
          this.employee.PositionName !== ""
        )
          return true;
      }
      return false;
    },

    /**
     * Hàm xử lý đóng form
     * @param {*} answer Phản hồi của người dùng với thông báo
     * Author: LeDucTiep (04/06/2023)
     */
    closeEmployeeFormChanged(answer) {
      // Lưu thông tin đã thay đổi
      if (answer == this.$msEnum.DialogAnswer.Yes) {
        // Lưu thông tin đã được thay đổi
        this.save();
      } else if (answer == this.$msEnum.DialogAnswer.No) {
        // Chuyển về trang chủ
        this.$router.push("/employee");
      }
    },

    /**
     * Hàm thông báo form đã bị thay đổi
     * Author: LeDucTiep (04/06/2023)
     */
    alertFormChanged() {
      this.$msEmitter.emit(
        "showDialog",
        this.$msEnum.DialogType.AlertFormChanged,
        [this.$t("FormChangedAlert")]
      );
    },

    /**
     * Hàm sử lý sự kiện phím tắt
     * Author: LeDucTiep (28/05/2023)
     */
    keyEvent(event) {
      if (document.onkeydown) return;
      const isESC = event.keyCode == 27;
      const isCtrlS = (event.metaKey || event.ctrlKey) && event.key === "s";
      const isCtrlShiftS =
        (event.metaKey || event.ctrlKey) && event.key === "S";

      if (isESC) {
        event.preventDefault();
        // Ẩn form
        this.hideEmployeeForm();
      } else if (isCtrlShiftS) {
        // Hủy sự kiện mặc định
        event.preventDefault();
        // Lưu và thêm
        this.saveAndAdd();
      } else if (isCtrlS) {
        // Hủy sự kiện mặc định
        event.preventDefault();
        // Lưu
        this.save();
      }
    },

    /**
     * Hàm focus vào nút cuối cùng
     * Author: LeDucTiep (08/06/2023)
     */
    focusLastInput() {
      // Focus vào nút hủy
      this.$refs.lastButton.focus();
    },

    /**
     * Hàm focus vào ô input đầu tiên
     * Author: LeDucTiep (17/05/2023)
     */
    focusFirstInput() {
      this.$msEmitter.emit("focusFirstInput");
    },

    /**
     * Hàm check xem employeeCode nhập vào đã tồn tại trong hệ thống chưa
     * Author: LeDucTiep (17/05/2023)
     */
    async checkDuplicateEmployeeCode() {
      try {
        if (this.employee.EmployeeCode) {
          // Kiểm tra trùng mã

          let response = null;
          // Nếu là form edit thì mã code phải ko bị trùng với người khác
          if (this.$route.name == "EmployeeEditRouter") {
            response = await this.$msAxios(
              "get",
              this.$msApi.EmployeeApi.CheckEditCodeDuplicated,
              {
                params: {
                  // employeeCode cần kiểm chứng
                  employeeCode: this.employee.EmployeeCode,
                  itsCode: this.employeeBeforeChange.EmployeeCode,
                },
              }
            );
          }
          // Nếu là form thêm thì mã code phải chưa tồn tại
          else {
            response = await this.$msAxios(
              "get",
              this.$msApi.EmployeeApi.CheckCodeDuplicated,
              {
                params: {
                  // employeeCode cần kiểm chứng
                  code: this.employee.EmployeeCode,
                },
              }
            );
          }

          //Nếu requests lỗi
          if (!response) {
            this.$msEmitter.emit("addNotice", {
              type: this.$msEnum.NoticeType.Error,
              message: this.$t("Error.UnknownError"),
            });

            // Thêm border đỏ
            this.$refs["EmployeeCode"].$el.classList.add("input-errored");

            // Xóa code lỗi
            this.employee.EmployeeCode = "";

            return {
              isErrored: true,
              message: "",
            };
          }
          // Nếu requests xịn
          else if (response.status == this.$msEnum.HttpStatusCode.Success) {
            if (
              !response.data &&
              this.$refs["EmployeeCode"].$el.classList.contains("input-errored")
            )
              this.$refs["EmployeeCode"].$el.classList.remove("input-errored");

            if (response.data)
              this.$refs["EmployeeCode"].$el.classList.add("input-errored");

            // Có trùng mã?
            return {
              isErrored: response.data,
              message: this.$t("Error.CodeExisted"),
            };
          }
        }
      } catch {
        return false;
      }
    },

    /**
     * Hàm ẩn employee form
     * Author: LeDucTiep (01/05/2023)
     */
    hideEmployeeForm() {
      // Nếu dữ liệu đã thay đổi thì hỏi người dùng
      if (this.isChanged()) {
        this.alertFormChanged();
      } else {
        this.$router.push("/employee");
      }
    },

    /**
     * Hàm kích hoạt khi nhấn nút 'Cất'
     * Author: LeDucTiep (01/05/2023)
     */
    async save() {
      try {
        this.isProcessing = true;
        // kiểm tra tất cả input
        const isValid = this.validation();
        // Submit form nếu tất cả input đều hợp lệ
        if (isValid) {
          // Đẩy thông tin lên api
          const isApiOk = await this.postOrPutEmployee();
          if (!isApiOk) {
            return;
          }
          // Trở về màn hình chính
          this.$router.push("/employee");
        }
      } finally {
        this.isProcessing = false;
      }
    },

    /**
     * Hàm kích hoạt khi nhấn nút 'Cất và thêm'
     * Author: LeDucTiep (01/05/2023)
     */
    async saveAndAdd() {
      try {
        this.isProcessing = true;
        // Kiểm tra tất cả input
        const isValid = this.validation();
        // Submit form nếu tất cả input đều hợp lệ
        if (isValid) {
          // Đẩy thông tin lên API
          const isApiOk = await this.postOrPutEmployee();
          if (!isApiOk) {
            return;
          }
          if (
            ["EmployeeEditRouter", "EmployeeDuplicateRouter"].includes(
              this.$route.name
            )
          ) {
            // Nếu là form sửa thì chuyển về form thêm
            await this.$router.replace("/employee/create");
          }

          // Reset lại form
          this.$msEmitter.emit("resetAddEmployeeForm");
        }
      } finally {
        this.isProcessing = false;
      }
    },

    /**
     * Post hoặc put tùy theo form thêm hay sửa
     * Author: LeDucTiep (01/05/2023)
     */
    async postOrPutEmployee() {
      // Tạo hiệu ứng loading
      this.$msEmitter.emit("setLoading", true);

      // Chuẩn bị dữ liệu gửi lên
      const dataSend = {
        ...this.employee,
      };

      // Định dạng ngày tháng
      if (this.employee.DateOfBirth)
        dataSend.DateOfBirth = this.$msDateFormater
          .stringToDate(this.employee.DateOfBirth)
          .toISOString();
      if (this.employee.IdentityDate)
        dataSend.IdentityDate = this.$msDateFormater
          .stringToDate(this.employee.IdentityDate)
          .toISOString();

      // Gửi dữ liệu
      try {
        // Nếu là form thêm
        if (
          ["EmployeeCreateRouter", "EmployeeDuplicateRouter"].includes(
            this.$route.name
          )
        ) {
          // Gửi thông tin để tạo nhân viên mới
          const response = await this.$msAxios(
            "post",
            this.$msApi.EmployeeApi.Post,
            dataSend
          );

          if (!response) {
            this.$msEmitter.emit("addNotice", {
              type: this.$msEnum.NoticeType.Error,
              message: this.$t("Notice.Post.Error"),
            });
            return false;
          }

          if (response.status == this.$msEnum.HttpStatusCode.CreatedSuccess) {
            this.$msEmitter.emit("addNotice", {
              type: this.$msEnum.NoticeType.Success,
              message: this.$t("Notice.Post.Success"),
            });
          }

          this.employee.EmployeeId = response.data;
          // reload lại giao diện
          this.$msEmitter.emit("afterPostEmployee", this.employee);
        } else {
          // Nếu là form sửa
          // Gửi thông tin để sửa đổi thông tin
          const response = await this.$msAxios(
            "put",
            this.$msApi.EmployeeApi.Put(this.id),
            dataSend
          );

          // Lỗi
          if (!response) {
            this.$msEmitter.emit("addNotice", {
              type: this.$msEnum.NoticeType.Error,
              message: this.$t("Notice.Put.Error"),
            });
            return false;
          }

          this.$msEmitter.emit("addNotice", {
            type: this.$msEnum.NoticeType.Success,
            message: this.$t("Notice.Put.Success"),
          });

          // reload lại giao diện
          this.$msEmitter.emit("afterPutEmployee", this.employee);
        }
      } finally {
        // Kết thúc trạng thái loading
        this.$msEmitter.emit("setLoading", false);
      }
      return true;
    },

    /**
     * Hàm validation các trường dữ liệu trong form
     * Author: LeDucTiep (01/05/2023)
     */
    validation() {
      // kích hoạt trạng thái đang validate
      this.isValidating = true;

      let _isValid = true;

      for (const field in this.inputValidation) {
        const isValid = this.inputValidation[field];
        if (!isValid) {
          _isValid = false;
          break;
        }
      }

      // Kiểm tra giá trị của ngày tháng <= hiện tại
      if (!this.dateOfBirthConditional() || !this.identityDateConditional())
        _isValid = false;

      // Kiểm tra độ dài của text

      for (const key in this.$refs) {
        try {
          if (key !== "EmployeeCode")
            this.$refs[key].$el.classList.remove("input-errored");
        } catch {
          continue;
        }
      }

      // Danh sách lỗi
      const errorList = [];

      // Duyệt qua các input
      for (const field in this.$refs) {
        if (!EmployeeFieldLength[field] || !this.employee[field]) continue;

        // Nếu độ dài không đúng
        if (this.employee[field].length > EmployeeFieldLength[field]) {
          // Tên trường dữ liệu
          const name = this.$t(`EmployeeForm.${field}`);

          // Nội dung lỗi
          const error = this.$t("Error.NoLongerThan", [
            name,
            EmployeeFieldLength[field],
          ]);

          // Thêm lỗi
          errorList.push(error);

          // Tô đỏ trường bị lỗi
          this.$refs[field].$el.classList.add("input-errored");
        }
      }

      // Nếu có lỗi
      if (errorList.length > 0) {
        this.$msEmitter.emit(
          "showDialog",
          this.$msEnum.DialogType.Error,
          errorList
        );
        _isValid = false;
      }

      // passed tất cả bài kiểm tra thì cho qua
      return _isValid;
    },
  },
};

export default mixin;

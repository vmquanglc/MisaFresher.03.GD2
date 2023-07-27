import accountEnum from "../../enum/accountEnum";
import { AccountFieldLength } from "../../enum/inputLength";

const mixin = {
  name: "EmployeeForm",
  props: ["id"],
  data() {
    return {
      // Tiêu đề của form
      title: "Thêm tài khoản",
      formWidth: "50vw",
      formTabs: [
        { id: accountEnum.FormTab.Infor, value: "Thông tin liên hệ" },
        {
          id: accountEnum.FormTab.TermPayment,
          value: "Điều khoản thanh toán",
        },
        { id: accountEnum.FormTab.BankAccount, value: "Tài khoản ngân hàng" },
        { id: accountEnum.FormTab.OrtherAddress, value: "Địa chỉ khác" },
        { id: accountEnum.FormTab.Note, value: "Ghi chú" },
      ],
      // Index của tab đã chọn
      formTabId: accountEnum.FormTab.Infor,
      account: {
        AccountType: 0,
        ObjectType: 0,
        CostAggregationObject: 0,
        TheOrder: 0,
        ContractOfSale: 0,
        PurchaseContract: 0,
        Unit: 0,
        Construction: 0,
        ExpenseItem: 0,
        StatisticalCode: 0,
      },
      isProcessing: false,
      accountBeforeChange: {},
      accountPropertyList: [],
      accountGroupList: [],
      objectTypeList: [
        { Id: 1, Name: "Nhà cung cấp" },
        { Id: 2, Name: "Khách hàng" },
        { Id: 3, Name: "Nhân viên" },
      ],
      followTypeList: [
        { Id: 1, Name: "Chỉ cảnh báo" },
        { Id: 2, Name: "Bắt buộc nhập" },
      ],
      isValidating: false,
      inputValidation: {},
      isReadonly: false,
    };
  },

  computed: {
    ObjectType: {
      get() {
        return !!this.account?.ObjectType;
      },
      set(value) {
        if (!this.account) {
          this.account = {};
        }

        this.account.ObjectType = value ? 1 : 0;
      },
    },
    CostAggregationObject: {
      get() {
        return !!this.account?.CostAggregationObject;
      },
      set(value) {
        if (!this.account) {
          this.account = {};
        }

        this.account.CostAggregationObject = value ? 1 : 0;
      },
    },

    TheOrder: {
      get() {
        return !!this.account?.TheOrder;
      },
      set(value) {
        if (!this.account) {
          this.account = {};
        }

        this.account.TheOrder = value ? 1 : 0;
      },
    },

    ContractOfSale: {
      get() {
        return !!this.account?.ContractOfSale;
      },
      set(value) {
        if (!this.account) {
          this.account = {};
        }

        this.account.ContractOfSale = value ? 1 : 0;
      },
    },

    PurchaseContract: {
      get() {
        return !!this.account?.PurchaseContract;
      },
      set(value) {
        if (!this.account) {
          this.account = {};
        }

        this.account.PurchaseContract = value ? 1 : 0;
      },
    },

    Unit: {
      get() {
        return !!this.account?.Unit;
      },
      set(value) {
        if (!this.account) {
          this.account = {};
        }
        this.account.Unit = value ? 1 : 0;
      },
    },

    Construction: {
      get() {
        return !!this.account?.Construction;
      },
      set(value) {
        if (!this.account) {
          this.account = {};
        }
        this.account.Construction = value ? 1 : 0;
      },
    },

    ExpenseItem: {
      get() {
        return !!this.account?.ExpenseItem;
      },
      set(value) {
        if (!this.account) {
          this.account = {};
        }
        this.account.ExpenseItem = value ? 1 : 0;
      },
    },

    StatisticalCode: {
      get() {
        return !!this.account?.StatisticalCode;
      },
      set(value) {
        if (!this.account) {
          this.account = {};
        }
        this.account.StatisticalCode = value ? 1 : 0;
      },
    },
  },

  async created() {
    await this.loadForm();
  },

  mounted() {
    this.$msEmitter.emit("focusFirstInput");
  },

  beforeUnmount() {},

  methods: {
    /**
     * Hàm thực mở rộng form
     * Author: LeDucTiep (12/07/2023)
     */
    setBigForm() {
      this.formWidth = "100vw";
    },

    /**
     * Hàm thu hẹp form
     * Author: LeDucTiep (12/07/2023)
     */
    setSmallForm() {
      this.formWidth = "50vw";
    },

    /**
     * Hàm kiểm tra mã code có bị trùng không
     * @returns bool
     * Author: LeDucTiep (12/07/2023)
     */
    async checkDuplicateCode() {
      try {
        if (
          this.account.AccountCode &&
          this.account.AccountCode.length <= AccountFieldLength["AccountCode"]
        ) {
          // Kiểm tra trùng mã
          let response = null;
          // Nếu là form edit thì mã code phải ko bị trùng với người khác

          if (
            ["AccountEditRouter", "AccountViewRouter"].includes(
              this.$route.name
            )
          ) {
            response = await this.$msAxios(
              "get",
              this.$msApi.AccountApi.CheckEditCodeDuplicated,
              {
                params: {
                  // employeeCode cần kiểm chứng
                  employeeCode: this.account.AccountCode,
                  itsCode: this.accountBeforeChange.AccountCode,
                },
              }
            );
          }
          // Nếu là form thêm thì mã code phải chưa tồn tại
          else {
            response = await this.$msAxios(
              "get",
              this.$msApi.AccountApi.CheckCodeDuplicated,
              {
                params: {
                  // employeeCode cần kiểm chứng
                  code: this.account.AccountCode,
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
            this.$refs["AccountCode"].$el.classList.add("input-errored");

            // Xóa code lỗi
            this.account.AccountCode = "";

            return {
              isErrored: true,
              message: "",
            };
          }
          // Nếu requests xịn
          else if (response.status == this.$msEnum.HttpStatusCode.Success) {
            if (
              !response.data &&
              this.$refs["AccountCode"].$el.classList.contains("input-errored")
            )
              this.$refs["AccountCode"].$el.classList.remove("input-errored");

            if (response.data)
              this.$refs["AccountCode"].$el.classList.add("input-errored");

            // Có trùng mã?
            return {
              isErrored: response.data,
              message: this.$t("Error.CodeExisted"),
            };
          }
        } else {
          // Thêm border đỏ
          this.$refs["AccountCode"].$el.classList.add("input-errored");

          return {
            isErrored: true,
            message: "",
          };
        }
      } catch {
        return false;
      }
    },

    /**
     * Hàm đòng form
     * Author: LeDucTiep (12/07/2023)
     */
    closeForm() {
      this.$router.push(`/account`);
    },

    /**
     * Hàm khởi tạo form
     * Author: LeDucTiep (12/07/2023)
     */
    async loadForm() {
      // console.log("route: ", this.$route.name);

      const response = await this.$msAxios(
        "get",
        this.$msApi.AccountPropertyApi.GetAll
      );

      if (!response || response.status != this.$msEnum.HttpStatusCode.Success) {
        this.$msEmitter.emit("showDialog", this.$msEnum.DialogType.Error, [
          this.$t("Error.UnknownError"),
        ]);
        return false;
      }

      this.accountPropertyList = response.data;
      if (
        this.id &&
        ["AccountEditRouter", "AccountViewRouter"].includes(this.$route.name)
      ) {
        if ("AccountViewRouter" == this.$route.name) {
          this.isReadonly = true;
          this.title = "Thông tin tài khoản";
        } else {
          this.title = "Sửa tài khoản";
        }
        // Lấy thông tin theo id
        const response = await this.$msAxios(
          "get",
          this.$msApi.AccountApi.Get(this.id)
        );

        if (
          !response ||
          response.status != this.$msEnum.HttpStatusCode.Success
        ) {
          this.$msEmitter.emit("showDialog", this.$msEnum.DialogType.Error, [
            this.$t("Error.UnknownError"),
          ]);
          return false;
        }

        // Nhận thông tin nhân viên cần sửa
        let accountForEdit = response.data;

        if (accountForEdit.DateOfBirth)
          accountForEdit.DateOfBirth = this.$msDateFormater.convertIsoToDDMMYYY(
            accountForEdit.DateOfBirth
          );

        if (accountForEdit.IdentityDate)
          accountForEdit.IdentityDate =
            this.$msDateFormater.convertIsoToDDMMYYY(
              accountForEdit.IdentityDate
            );

        this.account = { ...accountForEdit };

        this.accountBeforeChange = { ...accountForEdit };
      } else if (this.$route.name == "AccountCreateRouter") {
        // Lấy account property
        const response = await this.$msAxios(
          "get",
          this.$msApi.AccountPropertyApi.Paging,
          {
            params: {
              // Kích thước của trang
              pageSize: 1,
              // vị trí trang
              pageNumber: 1,
            },
          }
        );
        if (response.data.Data && response.data.Data.length == 1) {
          this.account.AccountPropertyId =
            response.data.Data[0].AccountPropertyId;
        }
      } else if (this.$route.name == "AccountCreateChildRouter") {
        this.account = {};
        this.account.AccountSyntheticId = this.id;
        this.accountBeforeChange = { ...this.account };

        const response = await this.$msAxios(
          "get",
          this.$msApi.AccountApi.Get(this.id)
        );

        if (response.data?.AccountPropertyId) {
          this.account.AccountPropertyId = response.data.AccountPropertyId;
        }
      }

      // Lấy mã của tài khoản tổng hợp

      if (
        this.account.AccountSyntheticId &&
        this.account.AccountSyntheticId !=
          "00000000-0000-0000-0000-000000000000"
      ) {
        const res = await this.$msAxios(
          "get",
          this.$msApi.AccountApi.Get(this.account.AccountSyntheticId)
        );

        if (res?.data?.AccountCode)
          this.account.ParentCode = res.data.AccountCode;
      }
    },

    /**
     * Hàm thực hiện khi thay đổi tải khoản tổng hợp
     * @param {Object} account Tài khoản tổng hợp
     * Author: LeDucTiep (12/07/2023)
     */
    AccountSyntheticOnSelect(account) {
      this.account.ParentCode = account.AccountCode;
    },

    /**
     * Hàm validate
     * @returns bool
     * Author: LeDucTiep (12/07/2023)
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
      // if (!this.dateOfBirthConditional() || !this.identityDateConditional())
      //   _isValid = false;

      // Kiểm tra độ dài của text

      for (const key in this.$refs) {
        try {
          if (key !== "AccountCode")
            this.$refs[key].$el.classList.remove("input-errored");
        } catch {
          continue;
        }
      }

      // Danh sách lỗi
      const errorList = [];

      // Duyệt qua các input
      for (const field in this.$refs) {
        if (!AccountFieldLength[field] || !this.account[field]) continue;

        // Nếu độ dài không đúng
        if (this.account[field].length > AccountFieldLength[field]) {
          // Tên trường dữ liệu
          const name = this.$t(`AccountForm.${field}`);

          // Nội dung lỗi
          const error = this.$t("Error.NoLongerThan", [
            name,
            AccountFieldLength[field],
          ]);

          // Thêm lỗi
          errorList.push(error);

          // Tô đỏ trường bị lỗi
          this.$refs[field].$el.classList.add("input-errored");
        }
      }

      if (
        this.account.ParentCode &&
        this.account.AccountCode &&
        !this.account.AccountCode.startsWith(this.account.ParentCode)
      ) {
        errorList.push(this.$t("Error.AccountCodeError"));
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
          this.$router.push("/account");
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
            [
              "AccountEditRouter",
              "AccountDuplicateRouter",
              "AccountCreateChildRouter",
            ].includes(this.$route.name)
          ) {
            // Nếu là form sửa thì chuyển về form thêm
            if (this.id) {
              await this.$router.replace("/account/create-child/" + this.id);
            } else {
              await this.$router.replace("/account/create");
            }
          }

          // Reset lại form
          this.$msEmitter.emit("resetAddAccountForm");
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
        ...this.account,
      };

      // Định dạng ngày tháng
      if (this.account.DateOfBirth)
        dataSend.DateOfBirth = this.$msDateFormater
          .stringToDate(this.account.DateOfBirth)
          .toISOString();
      if (this.account.IdentityDate)
        dataSend.IdentityDate = this.$msDateFormater
          .stringToDate(this.account.IdentityDate)
          .toISOString();

      // console.log("datasend: ", dataSend);

      // Gửi dữ liệu
      try {
        // Nếu là form thêm
        if (
          [
            "AccountCreateRouter",
            "AccountDuplicateRouter",
            "AccountCreateChildRouter",
          ].includes(this.$route.name)
        ) {
          // Gửi thông tin để tạo nhân viên mới
          const response = await this.$msAxios(
            "post",
            this.$msApi.AccountApi.Post,
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

          this.account.AccountId = response.data;
          // reload lại giao diện
          this.$msEmitter.emit("afterPostAccount", this.account);
        } else {
          // Nếu là form sửa
          // Gửi thông tin để sửa đổi thông tin
          const response = await this.$msAxios(
            "put",
            this.$msApi.AccountApi.Put(this.id),
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
          this.$msEmitter.emit("afterPutAccount", this.account);
        }
      } finally {
        // Kết thúc trạng thái loading
        this.$msEmitter.emit("setLoading", false);
      }
      return true;
    },
  },
};

export default mixin;

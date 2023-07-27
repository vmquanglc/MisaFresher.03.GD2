import { ReceiptFieldLength } from "../../enum/inputLength";

const mixin = {
  name: "CashCollectMoneyForm",
  props: {
    id: {
      type: String,
    },
  },
  data() {
    return {
      receipt: {
        ReceiptType: 1,
      },
      receiptBeforeChange: {},
      isProcessing: false,
      isValidating: false,
      inputValidation: {},
      initDebitAccountId: null,
      initCreditAccountId: null,
      readonlyHard: false,
      readonlySoft: false,
      ReceiptTypeList: [
        { id: 1, value: "1. " + this.$t("Cash.CollectMoneyFromCustomers") },
        { id: 2, value: "2. " + this.$t("Cash.EmployeeApplicationRefund") },
        { id: 3, value: "3. " + this.$t("Cash.WithdrawDepositToFundEntry") },
        { id: 4, value: "4. " + this.$t("Cash.OtherAutumn") },
      ],
    };
  },
  computed: {
    isRecorded() {
      return this.receipt?.IsRecorded;
      // return false;
    },
    totalMoney() {
      if (!this.receipt.BookKeepings) {
        return "0";
      }

      let money = this.receipt.BookKeepings.reduce((sum, obj) => {
        if (!isNaN(obj.AmountOfMoney)) {
          let num = isNaN(parseFloat(obj.AmountOfMoney))
            ? 0
            : parseFloat(obj.AmountOfMoney);

          return parseFloat(sum) + num;
        }
        return sum;
      }, 0);

      return this.formatMoney(isNaN(money) ? 0 : money);
    },

    initReason() {
      return `Thu tiền của ${
        this.receipt.CustomerName ? this.receipt.CustomerName : ""
      }`;
    },
  },
  async created() {
    await this.initForm();
  },
  mounted() {
    this.focusFirstInput();
  },
  methods: {
    focusFirstInput() {
      this.$refs.firstInput.focusInput();
    },
    /**
     * Hàm format tiền
     * @param {*} value Giá trị cần format
     * @returns Giá trị sau khi format
     * Author: LeDucTiep (12/07/2023)
     */
    formatMoney(value) {
      value = value?.toString();
      let money = value.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");

      if (money?.indexOf("-") !== -1) {
        money = money?.replace(/-/g, "");
        money = `(${money})`;
      }

      return money;
    },

    /**
     * Hàm thực hiện khi select khách hàng
     * @param {*} item
     * Author: LeDucTiep (12/07/2023)
     */
    async customerOnSelect(item) {
      if (this.readonlyHard || this.readonlySoft) return;
      this.receipt = {
        ...this.receipt,
        CustomerName: item.FullName,
        Address: item.Address,
        ObjectCode: item.CustomerCode,
      };

      this.receipt.Reason = this.initReason;

      let payer = null;
      if (item.CustomerType == this.$customerEnum.CustomerType.Organization) {
        const response = await this.$msAxios(
          "get",
          this.$msApi.CustomerApi.Get(item.CustomerId)
        );
        payer = response.data?.ContactInfor?.RecipientFullName;
      }

      this.receipt.Payer = payer;

      if (this.receipt?.BookKeepings) {
        this.receipt.BookKeepings = this.receipt.BookKeepings.map(
          (bookKeeping) => {
            bookKeeping.Note = this.initReason;
            bookKeeping.ObjectCode = item.CustomerCode;
            bookKeeping.ObjectName = item.FullName;

            return bookKeeping;
          }
        );
      }
    },

    /**
     * Hàm xóa một dòng
     * @param {index} index
     * Author: LeDucTiep (12/07/2023)
     */
    removeCurrentRow(index) {
      if (this.receipt.BookKeepings && !this.readonlyHard) {
        this.receipt.BookKeepings.splice(index, 1);
      }
    },

    /**
     * Hàm sửa
     * Author: LeDucTiep (12/07/2023)
     */
    edit() {
      this.readonlyHard = false;
      this.readonlySoft = false;
    },

    /**
     * Hàm sửa nhanh
     * Author: LeDucTiep (12/07/2023)
     */
    fastEdit() {
      this.readonlyHard = false;
      this.readonlySoft = true;
    },

    /**
     * Hàm ghi sổ
     * Author: LeDucTiep (12/07/2023)
     */
    async setRecorded() {
      this.receipt.IsRecorded = true;

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
        }
      } finally {
        this.isProcessing = false;
      }
    },

    /**
     * Hàm bỏ ghi sổ
     * Author: LeDucTiep (12/07/2023)
     */
    async unRecorded() {
      this.receipt.IsRecorded = false;

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
        }
      } finally {
        this.isProcessing = false;
      }
    },

    /**
     * Hàm thêm một bản ghi
     * Author: LeDucTiep (12/07/2023)
     */
    async save() {
      try {
        this.receipt.IsRecorded = true;

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
          await this.$router.push(`/cash/edit/${this.receipt.ReceiptId}`);
          this.$msEmitter.emit("reRenderCashHome");
        }
      } finally {
        this.isProcessing = false;
      }
    },

    /**
     * Hàm Lưu và thêm bản ghi
     * Author: LeDucTiep (12/07/2023)
     */
    async saveAndAdd() {
      try {
        this.receipt.IsRecorded = true;

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
          console.log(this.$route.name);
          if (
            ["receipt-edit", "ReceiptDuplicateRouter"].includes(
              this.$route.name
            )
          ) {
            // Nếu là form sửa thì chuyển về form thêm
            await this.$router.replace("/cash/create");
          }

          // Reset lại form
          this.$msEmitter.emit("reRenderCashHome");
        }
      } finally {
        this.isProcessing = false;
      }
    },

    /**
     * Hàm post hoặc put
     * Author: LeDucTiep (12/07/2023)
     */
    async postOrPutEmployee() {
      // Tạo hiệu ứng loading
      this.$msEmitter.emit("setLoading", true);

      // Chuẩn bị dữ liệu gửi lên
      const dataSend = {
        ...this.receipt,
      };

      // Định dạng ngày tháng
      if (this.receipt.AccountingDate)
        dataSend.AccountingDate = this.$msDateFormater
          .stringToDate(this.receipt.AccountingDate)
          .toISOString();

      if (this.receipt.ReceiptDate)
        dataSend.ReceiptDate = this.$msDateFormater
          .stringToDate(this.receipt.ReceiptDate)
          .toISOString();

      // console.log("datasend: ", dataSend);

      // Gửi dữ liệu
      try {
        // Nếu là form thêm
        if (
          ["receipt-add", "ReceiptDuplicateRouter"].includes(this.$route.name)
        ) {
          // Gửi thông tin để tạo nhân viên mới
          const response = await this.$msAxios(
            "post",
            this.$msApi.ReceiptApi.Post,
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

          this.receipt.ReceiptId = response.data;
          // reload lại giao diện
          this.$msEmitter.emit("afterPostReceipt", this.receipt);
        } else {
          // Nếu là form sửa
          // Gửi thông tin để sửa đổi thông tin
          const response = await this.$msAxios(
            "put",
            this.$msApi.ReceiptApi.Put(this.id),
            dataSend
          );

          // Lỗi
          if (!response) {
            if (this.readonlyHard && this.receipt.IsRecorded) {
              this.$msEmitter.emit("addNotice", {
                type: this.$msEnum.NoticeType.Error,
                message: this.$t("Notice.BookKeeping.Error"),
              });
            } else if (this.readonlyHard && !this.receipt.IsRecorded) {
              this.$msEmitter.emit("addNotice", {
                type: this.$msEnum.NoticeType.Error,
                message: this.$t("Notice.UnBookKeeping.Error"),
              });
            } else {
              this.$msEmitter.emit("addNotice", {
                type: this.$msEnum.NoticeType.Error,
                message: this.$t("Notice.Put.Error"),
              });
            }
            return false;
          }

          if (this.readonlyHard && this.receipt.IsRecorded) {
            this.$msEmitter.emit("addNotice", {
              type: this.$msEnum.NoticeType.Success,
              message: this.$t("Notice.BookKeeping.Success"),
            });
          } else if (this.readonlyHard && !this.receipt.IsRecorded) {
            this.$msEmitter.emit("addNotice", {
              type: this.$msEnum.NoticeType.Success,
              message: this.$t("Notice.UnBookKeeping.Success"),
            });
          } else {
            this.$msEmitter.emit("addNotice", {
              type: this.$msEnum.NoticeType.Success,
              message: this.$t("Notice.Put.Success"),
            });
          }

          // reload lại giao diện
          this.$msEmitter.emit("afterPutReceipt", this.receipt);
        }
      } finally {
        // Kết thúc trạng thái loading
        this.$msEmitter.emit("setLoading", false);
      }
      return true;
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
          if (key !== "ReceiptCode")
            this.$refs[key].$el.classList.remove("input-errored");
        } catch {
          continue;
        }
      }

      // Danh sách lỗi
      const errorList = [];

      // Duyệt qua các input
      for (const field in this.$refs) {
        if (!ReceiptFieldLength[field] || !this.receipt[field]) continue;

        // Nếu độ dài không đúng
        if (this.receipt[field].length > ReceiptFieldLength[field]) {
          // Tên trường dữ liệu
          const name = this.$t(`ReceiptForm.${field}`);

          // Nội dung lỗi
          const error = this.$t("Error.NoLongerThan", [
            name,
            ReceiptFieldLength[field],
          ]);

          // Thêm lỗi
          errorList.push(error);

          // Tô đỏ trường bị lỗi
          this.$refs[field].$el.classList.add("input-errored");
        }
      }

      let isEmptyDebitAccount = false;
      let isEmptyCreditAccount = false;

      if (this.receipt?.BookKeepings) {
        for (const index in this.receipt.BookKeepings) {
          const bookKeeping = this.receipt.BookKeepings[index];

          if (!bookKeeping.DebitAccountId) {
            isEmptyDebitAccount = true;
          }

          if (!bookKeeping.CollectAccountId) {
            isEmptyCreditAccount = true;
          }
        }
      }

      if (isEmptyDebitAccount) {
        errorList.push("Tài khoản nợ không được để trống");
      }

      if (isEmptyCreditAccount) {
        errorList.push("Tài khoản có không được để trống");
      }

      // Kiểm tra ngày hạch toán và ngày phiếu thu
      if (this.receipt?.AccountingDate && this.receipt?.ReceiptDate) {
        const accountingDate = this.$msDateFormater.stringToDate(
          this.receipt.AccountingDate
        );
        const receiptDate = this.$msDateFormater.stringToDate(
          this.receipt.ReceiptDate
        );

        if (accountingDate.getTime() < receiptDate.getTime()) {
          _isValid = false;
          errorList.push("Ngày phiếu thu không được lớn hơn ngày hạch toán");

          // Tô đỏ trường bị lỗi
          this.$refs["AccountingDate"].$el.classList.add("input-errored");
          this.$refs["ReceiptDate"].$el.classList.add("input-errored");
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

    /**
     * Hàm khởi tạo form
     * Author: LeDucTiep (12/07/2023)
     */
    async initForm() {
      // console.log("route: ", this.$route.name);

      if (this.$route.name == "receipt-add") {
        const response = await this.$msAxios(
          "get",
          this.$msApi.ReceiptApi.GetNewCode
        );

        // Lỗi
        if (!response) {
          this.$msEmitter.emit("addNotice", {
            type: this.$msEnum.NoticeType.Error,
            message: this.$t("Notice.Put.Error"),
          });
          return false;
        }

        const today = this.$msDateFormater.convertIsoToDDMMYYY(new Date());

        this.receipt = {
          ReceiptType: 1,
          BookKeepings: null,
          Reason: this.initReason,
          ReceiptCode: response.data,
          AccountingDate: today,
          ReceiptDate: today,
        };

        // Lấy tài khoản nợ và tài khoản có mặc định
        const resDebit = await this.$msAxios(
          "get",
          this.$msApi.AccountApi.PagingCombobox,
          {
            params: {
              // Kích thước của trang
              pageSize: 1,
              // vị trí trang
              pageNumber: 1,
              // Từ khóa tìm kiếm
              searchTerm: "Dư nợ",
            },
          }
        );

        const resCredit = await this.$msAxios(
          "get",
          this.$msApi.AccountApi.PagingCombobox,
          {
            params: {
              // Kích thước của trang
              pageSize: 1,
              // vị trí trang
              pageNumber: 1,
              // Từ khóa tìm kiếm
              searchTerm: "Dư có",
            },
          }
        );

        const res = await this.$msAxios(
          "get",
          this.$msApi.AccountApi.PagingCombobox,
          {
            params: {
              // Kích thước của trang
              pageSize: 1,
              // vị trí trang
              pageNumber: 1,
              // Từ khóa tìm kiếm
              searchTerm: "Lưỡng tính",
            },
          }
        );

        if (resDebit?.data?.Data?.length) {
          this.initDebitAccountId = resDebit?.data?.Data[0].AccountId;
        } else if (res?.data?.Data?.length) {
          this.initDebitAccountId = res.data?.Data[0].AccountId;
        }

        if (resCredit?.data?.Data?.length) {
          this.initCreditAccountId = resCredit?.data?.Data[0].AccountId;
        } else if (res?.data?.Data?.length) {
          this.initCreditAccountId = res.data?.Data[0].AccountId;
        }

        this.addRow();
      } else if (
        this.id &&
        ["receipt-edit", "receipt-view"].includes(this.$route.name)
      ) {
        this.readonlyHard = true;
        // Lấy thông tin theo id
        const response = await this.$msAxios(
          "get",
          this.$msApi.ReceiptApi.Get(this.id)
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
        let receiptForEdit = response.data;

        if (receiptForEdit.AccountingDate)
          receiptForEdit.AccountingDate =
            this.$msDateFormater.convertIsoToDDMMYYY(
              receiptForEdit.AccountingDate
            );

        if (receiptForEdit.ReceiptDate)
          receiptForEdit.ReceiptDate = this.$msDateFormater.convertIsoToDDMMYYY(
            receiptForEdit.ReceiptDate
          );

        this.receipt = { ...receiptForEdit };
        this.receiptBeforeChange = { ...receiptForEdit };

        if (this.receipt.BookKeepings?.length) {
          const lastBookkeepings =
            this.receipt.BookKeepings[this.receipt.BookKeepings.length - 1];

          this.initDebitAccountId = lastBookkeepings.DebitAccountId;
          this.initCreditAccountId = lastBookkeepings.CollectAccountId;
        }
      }
    },

    /**
     * Hàm đóng form
     * Author: LeDucTiep (12/07/2023)
     */
    closeForm() {
      this.$router.push(`/cash/receipt`);
    },

    /**
     * Hàm thêm một dòng
     * Author: LeDucTiep (12/07/2023)
     */
    addRow() {
      const newObject = {
        Note: this.initReason,
        DebitAccountId: this.initDebitAccountId,
        CollectAccountId: this.initCreditAccountId,
        ObjectCode: this.receipt.ObjectCode,
        ObjectName: this.receipt.CustomerName,
        AmountOfMoney: 0,
      };
      if (!this.receipt.BookKeepings) {
        this.receipt.BookKeepings = [newObject];
      } else {
        this.receipt.BookKeepings.push(newObject);
      }
    },

    /**
     * Hàm xóa một dòng
     * Author: LeDucTiep (12/07/2023)
     */
    deleteAllRows() {
      this.receipt.BookKeepings = [];
    },
  },
};

export default mixin;

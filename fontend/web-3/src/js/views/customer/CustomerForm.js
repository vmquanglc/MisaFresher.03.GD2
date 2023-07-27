import customerEnum from "../../enum/customerEnum";
import { CustomerFieldLength } from "../../enum/inputLength";

const mixin = {
  name: "CustomerForm",
  props: ["id"],
  data() {
    return {
      // Tiêu đề của form
      title: "Thông tin khách hàng",
      inputRadioItems: [
        {
          id: 0,
          value: "Tổ chức",
        },
        {
          id: 1,
          value: "Cá nhân",
        },
      ],
      formTabs: [
        { id: customerEnum.FormTab.Infor, value: "Thông tin liên hệ" },
        {
          id: customerEnum.FormTab.TermPayment,
          value: "Điều khoản thanh toán",
        },
        { id: customerEnum.FormTab.BankAccount, value: "Tài khoản ngân hàng" },
        { id: customerEnum.FormTab.OrtherAddress, value: "Địa chỉ khác" },
        { id: customerEnum.FormTab.Note, value: "Ghi chú" },
      ],
      // Index của tab đã chọn
      formTabId: customerEnum.FormTab.Infor,
      customer: {
        CustomerType: 0,
      },
      isProcessing: false,
      customerBeforeChange: {},
      vocativeList: [
        { id: 0, name: "Anh" },
        { id: 1, name: "Bà" },
        { id: 2, name: "Bạn" },
        { id: 3, name: "Chị" },
        { id: 4, name: "Miss" },
        { id: 5, name: "Mr" },
        { id: 6, name: "Mrs" },
        { id: 7, name: "Ông" },
      ],
      customerGroupList: [],

      // chứa {field: isValid}
      inputValidation: {},
      isValidating: false,
      isHardReadonly: false,
    };
  },

  computed: {
    Vocative: {
      get() {
        return this.customer?.ContactInfor?.Vocative;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.ContactInfor) {
          this.customer.ContactInfor = {};
        }
        this.customer.ContactInfor.Vocative = value;
      },
    },
    Email: {
      get() {
        return this.customer?.ContactInfor?.Email;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.ContactInfor) {
          this.customer.ContactInfor = {};
        }
        this.customer.ContactInfor.Email = value;
      },
    },

    FullName: {
      get() {
        return this.customer?.ContactInfor?.FullName;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.ContactInfor) {
          this.customer.ContactInfor = {};
        }
        this.customer.ContactInfor.FullName = value;
      },
    },

    PhoneNumber: {
      get() {
        return this.customer?.ContactInfor?.PhoneNumber;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.ContactInfor) {
          this.customer.ContactInfor = {};
        }
        this.customer.ContactInfor.PhoneNumber = value;
      },
    },
    LandlineNumber: {
      get() {
        return this.customer?.ContactInfor?.LandlineNumber;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.ContactInfor) {
          this.customer.ContactInfor = {};
        }
        this.customer.ContactInfor.LandlineNumber = value;
      },
    },
    LegalRepresentative: {
      get() {
        return this.customer?.ContactInfor?.LegalRepresentative;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.ContactInfor) {
          this.customer.ContactInfor = {};
        }
        this.customer.ContactInfor.LegalRepresentative = value;
      },
    },

    RecipientFullName: {
      get() {
        return this.customer?.ContactInfor?.RecipientFullName;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.ContactInfor) {
          this.customer.ContactInfor = {};
        }
        this.customer.ContactInfor.RecipientFullName = value;
      },
    },

    RecipientEmail: {
      get() {
        return this.customer?.ContactInfor?.RecipientEmail;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.ContactInfor) {
          this.customer.ContactInfor = {};
        }
        this.customer.ContactInfor.RecipientEmail = value;
      },
    },
    RecipientPhoneNumber: {
      get() {
        return this.customer?.ContactInfor?.RecipientPhoneNumber;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.ContactInfor) {
          this.customer.ContactInfor = {};
        }
        this.customer.ContactInfor.RecipientPhoneNumber = value;
      },
    },

    MaxDaysOwed: {
      get() {
        return this.customer?.TermsOfPayment?.MaxDaysOwed;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.TermsOfPayment) {
          this.customer.TermsOfPayment = {};
        }
        this.customer.TermsOfPayment.MaxDaysOwed = value;
      },
    },
    NationId: {
      get() {
        return this.customer?.OtherLocation?.NationId;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.OtherLocation) {
          this.customer.OtherLocation = {};
        }

        this.customer.OtherLocation.NationId = value;
      },
    },
    CityId: {
      get() {
        return this.customer?.OtherLocation?.CityId;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.OtherLocation) {
          this.customer.OtherLocation = {};
        }

        this.customer.OtherLocation.CityId = value;
      },
    },
    CommuneId: {
      get() {
        return this.customer?.OtherLocation?.CommuneId;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.OtherLocation) {
          this.customer.OtherLocation = {};
        }

        this.customer.OtherLocation.CommuneId = value;
      },
    },
    DistrictId: {
      get() {
        return this.customer?.OtherLocation?.DistrictId;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.OtherLocation) {
          this.customer.OtherLocation = {};
        }

        this.customer.OtherLocation.DistrictId = value;
      },
    },

    IsSameCustomerAddress: {
      get() {
        return this.customer?.OtherLocation?.IsSameCustomerAddress;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.OtherLocation) {
          this.customer.OtherLocation = {};
        }

        this.customer.OtherLocation.IsSameCustomerAddress = value;
      },
    },

    SpecificAddresses: {
      get() {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.OtherLocation) {
          this.customer.OtherLocation = {
            SpecificAddresses: [{}],
          };
        }
        return this.customer.OtherLocation.SpecificAddresses;
      },
      set(value) {
        if (!this.customer) {
          this.customer = {};
        } else if (!this.customer.OtherLocation) {
          this.customer.OtherLocation = {};
        }
        this.customer.OtherLocation.SpecificAddresses = value;
      },
    },

    CityPagingApi: {
      get() {
        if (this.customer?.OtherLocation?.NationId) {
          return this.$msApi.CityApi.Paging + "/" + this.NationId;
        }
        return null;
      },
    },

    DistrictPagingApi: {
      get() {
        if (this.customer?.OtherLocation?.CityId) {
          return this.$msApi.DistrictApi.Paging + "/" + this.CityId;
        }
        return null;
      },
    },

    CommunePagingApi: {
      get() {
        if (this.customer?.OtherLocation?.DistrictId) {
          return this.$msApi.CommuneApi.Paging + "/" + this.DistrictId;
        }
        return null;
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
     * Hàm kiểm tra mã số thuế
     * Author: LeDucTiep (12/07/2023)
     */
    async checkCheckTaxCode() {
      if (this.customer.TaxCode) {
        const response = await this.$msAxios(
          "get",
          this.$msApi.TaxCodeApi.CheckExist(this.customer.TaxCode)
        );

        if (response.data.code == "00") {
          this.customer.FullName = response.data?.data?.name;
          this.customer.Address = response.data?.data?.address;
          return {
            isErrored: false,
            message: "",
          };
        } else {
          return {
            isErrored: true,
            message: "Mã số thuế không đúng",
          };
        }
      } else
        return {
          isErrored: true,
          message: "",
        };
    },
    /**
     * Hàm thực hiện khi quốc gia thay đổi
     * @param {Object} nation
     * Author: LeDucTiep (12/07/2023)
     */
    nationOnChange(nation) {
      console.log(this.NationId, nation.NationId);
      this.CityId = null;
      this.CommuneId = null;
      this.DistrictId = null;
    },

    /**
     * Hàm thực hiện khi Huyện thay đổi
     * @param {Object} district
     * Author: LeDucTiep (12/07/2023)
     */
    districtOnChange(district) {
      console.log("district", district);
      this.CommuneId = null;
    },

    /**
     * Hàm thực hiện khi thành phố thay đổi
     * @param {Object} city
     * Author: LeDucTiep (12/07/2023)
     */
    cityOnChange(city) {
      console.log("city: ", city);
      this.CommuneId = null;
      this.DistrictId = null;
    },

    /**
     * Hàm gán giống địa chỉ khách hàng
     * Author: LeDucTiep (12/07/2023)
     */
    takeSameCustomerAddress() {
      if (this.IsSameCustomerAddress && this.customer.Address) {
        this.SpecificAddresses = [{ Address: this.customer.Address }];
      }
    },

    /**
     * Hàm kiểm tra mã khách hàng có bị trùng không
     * @returns Object
     * Author: LeDucTiep (14/07/2023)
     */
    async checkDuplicateCode() {
      try {
        if (
          this.customer.CustomerCode &&
          this.customer.CustomerCode.length <=
            CustomerFieldLength["CustomerCode"]
        ) {
          // Kiểm tra trùng mã

          let response = null;
          // Nếu là form edit thì mã code phải ko bị trùng với người khác
          if (
            ["CustomerEditRouter", "CustomerViewRouter"].includes(
              this.$route.name
            )
          ) {
            response = await this.$msAxios(
              "get",
              this.$msApi.CustomerApi.CheckEditCodeDuplicated,
              {
                params: {
                  // employeeCode cần kiểm chứng
                  employeeCode: this.customer.CustomerCode,
                  itsCode: this.customerBeforeChange.CustomerCode,
                },
              }
            );
          }
          // Nếu là form thêm thì mã code phải chưa tồn tại
          else {
            response = await this.$msAxios(
              "get",
              this.$msApi.CustomerApi.CheckCodeDuplicated,
              {
                params: {
                  // employeeCode cần kiểm chứng
                  code: this.customer.CustomerCode,
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
            this.$refs["CustomerCode"].$el.classList.add("input-errored");

            // Xóa code lỗi
            this.customer.CustomerCode = "";

            return {
              isErrored: true,
              message: "",
            };
          }
          // Nếu requests xịn
          else if (response.status == this.$msEnum.HttpStatusCode.Success) {
            if (
              !response.data &&
              this.$refs["CustomerCode"].$el.classList.contains("input-errored")
            )
              this.$refs["CustomerCode"].$el.classList.remove("input-errored");

            if (response.data)
              this.$refs["CustomerCode"].$el.classList.add("input-errored");

            // Có trùng mã?
            return {
              isErrored: response.data,
              message: this.$t("Error.CodeExisted"),
            };
          }
        } else {
          // Thêm border đỏ
          this.$refs["CustomerCode"].$el.classList.add("input-errored");

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
     * Hàm lấy API lấy thành phố theo Id quốc gia và id thành phố
     * @param {Guid} cityId Id của thành phố
     * @returns API lấy thành phố theo quốc gia và id thành phố
     * Author: LeDucTiep (14/07/2023)
     */
    CityGetApi(cityId) {
      return this.$msApi.CityApi.Get(this.NationId) + "/" + cityId;
    },

    /**
     * Hàm lấy API lấy thông tin huyện theo Id thành phố và id huyện
     * @param {Guid} districtId Id của huyện
     * @returns API lấy thông tin huyện theo Id thành phố và id huyện
     * Author: LeDucTiep (14/07/2023)
     */
    DistrictGetApi(districtId) {
      return this.$msApi.DistrictApi.Get(this.CityId) + "/" + districtId;
    },

    /**
     * Hàm lấy API lấy thông tin xã theo id xã và id huyện
     * @param {Guid} communeId Id của xã
     * @returns API lấy thông tin xã theo id xã và id huyện
     * Author: LeDucTiep (14/07/2023)
     */
    CommuneGetApi(communeId) {
      return this.$msApi.CommuneApi.Get(this.DistrictId) + "/" + communeId;
    },

    /**
     * Hàm thêm một dòng địa chỉ
     * Author: LeDucTiep (14/07/2023)
     */
    addSpecificAddress() {
      this.SpecificAddresses.push({
        newId: this.SpecificAddresses.length + 1,
      });
    },

    /**
     * Hàm xóa tất cả các dòng địa chỉ
     * Author: LeDucTiep (14/07/2023)
     */
    removeAllSpecificAddress() {
      this.SpecificAddresses = [{}];
    },

    /**
     * Hàm xóa một dòng địa chỉ
     * @param {Objecgt} SpecificAddress
     * Author: LeDucTiep (14/07/2023)
     */
    removeSpecificAddress(index) {
      this.SpecificAddresses.splice(index, 1);
    },

    /**
     * Hàm xóa tất cả các dòng địa chỉ
     * Author: LeDucTiep (14/07/2023)
     */
    removeAllBankAccount() {
      this.customer.BankAccounts = [];
    },

    /**
     * Hàm xóa một dòng tài khoản ngân hàng
     * @param {int} index
     * Author: LeDucTiep (14/07/2023)
     */
    removeBankAccount(index) {
      console.log(index);
      this.customer.BankAccounts.splice(index, 1);
    },

    /**
     * Hàm thêm một dòng tài khoản ngân hàng
     * Author: LeDucTiep (14/07/2023)
     */
    addBankAccount() {
      if (!this.customer.BankAccounts) {
        this.customer.BankAccounts = [];
      }
      this.customer.BankAccounts.push({
        newKey: this.customer.BankAccounts.length + 1,
      });
    },

    /**
     * Hàm đòng form
     * Author: LeDucTiep (14/07/2023)
     */
    closeForm() {
      this.$router.push(`/customer`);
    },
    async loadForm() {
      // console.log("route: ", this.$route.name);

      const response = await this.$msAxios(
        "get",
        this.$msApi.CustomerGroupApi.GetAll
      );

      if (!response || response.status != this.$msEnum.HttpStatusCode.Success) {
        this.$msEmitter.emit("showDialog", this.$msEnum.DialogType.Error, [
          this.$t("Error.UnknownError"),
        ]);
        return false;
      }

      this.customerGroupList = response.data;

      if (
        this.id &&
        ["CustomerEditRouter", "CustomerViewRouter"].includes(this.$route.name)
      ) {
        if ("CustomerViewRouter" == this.$route.name) {
          this.isHardReadonly = true;
        }
        // Lấy thông tin theo id
        const response = await this.$msAxios(
          "get",
          this.$msApi.CustomerApi.Get(this.id)
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
        let customerForEdit = response.data;

        if (customerForEdit.DateOfBirth)
          customerForEdit.DateOfBirth =
            this.$msDateFormater.convertIsoToDDMMYYY(
              customerForEdit.DateOfBirth
            );

        if (customerForEdit.IdentityDate)
          customerForEdit.IdentityDate =
            this.$msDateFormater.convertIsoToDDMMYYY(
              customerForEdit.IdentityDate
            );

        this.customer = { ...customerForEdit };
        this.customerBeforeChange = { ...customerForEdit };
      } else if (this.$route.name == "CustomerCreateRouter") {
        // Lấy mã  mới về
        const response = await this.$msAxios(
          "get",
          this.$msApi.CustomerApi.GetNewCode
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
        this.customer.CustomerCode = response.data;
      }

      if (
        !this.customer.BankAccounts ||
        this.customer.BankAccounts?.length == 0
      ) {
        this.customer.BankAccounts = [{}];
      }
    },

    /**
     * Hàm validate
     * @returns bool
     * Author: LeDucTiep (14/07/2023)
     */
    validation() {
      // kích hoạt trạng thái đang validate
      this.isValidating = true;

      let _isValid = true;

      for (const field in this.inputValidation) {
        const isValid = this.inputValidation[field];
        if (!isValid) {
          _isValid = false;

          this.$refs[field]?.forceFocus();

          break;
        }
      }

      // Kiểm tra giá trị của ngày tháng <= hiện tại

      // Kiểm tra độ dài của text

      // Hủy bỏ input đỏ
      for (const key in this.$refs) {
        try {
          if (key !== "CustomerCode")
            this.$refs[key].$el.classList.remove("input-errored");
        } catch {
          continue;
        }
      }

      // Danh sách lỗi
      const errorList = [];

      // Kiểm tra số tài khoản ngân hàng có lặp lại không
      const bankAccountNumbers = this.customer.BankAccounts.map(
        (item) => item.BankAccountNumber
      );

      let set = new Set(bankAccountNumbers);

      const isDuplicatedBankAccount = set.size !== bankAccountNumbers.length;

      if (isDuplicatedBankAccount) {
        errorList.push(this.$t("Error.DuplicatedBankAccount"));
      }

      // Duyệt qua các input
      for (const field in this.$refs) {
        if (!CustomerFieldLength[field] || !this.customer[field]) continue;

        // Nếu độ dài không đúng
        if (this.customer[field].length > CustomerFieldLength[field]) {
          // Tên trường dữ liệu
          const name = this.$t(`CustomerForm.${field}`);

          // Nội dung lỗi
          const error = this.$t("Error.NoLongerThan", [
            name,
            CustomerFieldLength[field],
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
          const isApiOk = await this.postOrPutCustomer();
          if (!isApiOk) {
            return;
          }
          // Trở về màn hình chính
          this.$router.push("/customer");
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
          const isApiOk = await this.postOrPutCustomer();
          if (!isApiOk) {
            return;
          }
          if (
            ["CustomerEditRouter", "CustomerDuplicateRouter"].includes(
              this.$route.name
            )
          ) {
            // Nếu là form sửa thì chuyển về form thêm
            await this.$router.replace("/customer/create");
          }

          // Reset lại form
          this.$msEmitter.emit("resetAddCustomerForm");
        }
      } finally {
        this.isProcessing = false;
      }
    },

    /**
     * Post hoặc put tùy theo form thêm hay sửa
     * Author: LeDucTiep (01/05/2023)
     */
    async postOrPutCustomer() {
      // Tạo hiệu ứng loading
      this.$msEmitter.emit("setLoading", true);

      // Chuẩn bị dữ liệu gửi lên
      const dataSend = {
        ...this.customer,
      };

      // Định dạng ngày tháng
      if (this.customer.DateOfBirth)
        dataSend.DateOfBirth = this.$msDateFormater
          .stringToDate(this.customer.DateOfBirth)
          .toISOString();
      if (this.customer.IdentityDate)
        dataSend.IdentityDate = this.$msDateFormater
          .stringToDate(this.customer.IdentityDate)
          .toISOString();

      // console.log("datasend: ", dataSend);

      // Gửi dữ liệu
      try {
        // Nếu là form thêm
        if (
          ["CustomerCreateRouter", "CustomerDuplicateRouter"].includes(
            this.$route.name
          )
        ) {
          // Gửi thông tin để tạo nhân viên mới
          const response = await this.$msAxios(
            "post",
            this.$msApi.CustomerApi.Post,
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

          this.customer.CustomerId = response.data;
          // reload lại giao diện
          this.$msEmitter.emit("afterPostCustomer", this.customer);
        } else {
          // Nếu là form sửa
          // Gửi thông tin để sửa đổi thông tin
          const response = await this.$msAxios(
            "put",
            this.$msApi.CustomerApi.Put(this.id),
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
          this.$msEmitter.emit("afterPutCustomer", this.customer);
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

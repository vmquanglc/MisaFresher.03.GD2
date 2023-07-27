const mixin = {
  name: "CustomerList",
  data() {
    return {
      customerFormKey: 0,
      inputSearchValue: "",
      tableHeaders: [
        {
          text: this.$t("CustomerList.CustomerCode"),
        },
        {
          text: this.$t("CustomerList.FullName"),
        },
        {
          text: this.$t("CustomerList.Address"),
        },
        {
          text: this.$t("CustomerList.PhoneNumberShort"),
          hint: this.$t("CustomerList.PhoneNumber"),
        },
        {
          text: this.$t("CustomerList.IdentityNumberShort"),
          hint: this.$t("CustomerList.IdentityNumber"),
        },
        {
          text: this.$t("CustomerList.Feature"),
        },
      ],
      customerList: null,
      columnList: [
        "CustomerCode",
        "FullName",
        "Address",
        "PhoneNumber",
        "IdentityNumber",
      ],
      // Danh sách phần tử đã chọn
      selectedList: {},
      // Thông tin của dialog hiện lên
      dialog: {},
      TotalRecord: 0,

      TotalPage: 0,

      pageSizeSelected: 20,

      pageNumberSelected: 1,

      CustomerOnMenu: null,
    };
  },
  watch: {
    pageSizeSelected() {
      this.pageNumberSelected = 1;
      this.loadCustomerList();
    },
  },
  computed: {
    selectedCustomerIdList() {
      let list = [];
      for (let id in this.selectedList) {
        if (this.selectedList[id]) {
          list.push(id);
        }
      }
      return list;
    },
    isAllRowsSelected() {
      if (!this.customerList) return false;

      // Không có phần tử nào được chọn
      if (Object.keys(this.selectedList).length == 0) return false;

      for (const index in this.customerList) {
        let customer = this.customerList[index];
        if (!this.selectedList[customer.CustomerId]) {
          return false;
        }
      }
      return true;
    },
  },
  created() {
    this.loadCustomerList();
    this.$msEmitter.on("resetAddCustomerForm", this.resetAddCustomerForm);
    this.$msEmitter.on("afterPostCustomer", this.afterPostCustomer);
    this.$msEmitter.on("afterPutCustomer", this.afterPutCustomer);
    this.$msEmitter.on("showDialog", this.showDialog);
    this.$msEmitter.on("hideDialog", this.hideDialog);
  },
  beforeUnmount() {
    this.$msEmitter.off("resetAddCustomerForm");
    this.$msEmitter.off("afterPostCustomer");
    this.$msEmitter.off("afterPutCustomer");
    this.$msEmitter.off("showDialog");
    this.$msEmitter.off("hideDialog");
  },
  methods: {
    /**
     * Hàm xuất excel
     * @param {Event} event
     * Author: LeDucTiep (07/06/2023)
     */
    async exportOnclick(event) {
      event.target.style.cursor = "wait";
      await this.exportInBackEnd();
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
        this.$msApi.CustomerApi.ExportExcel,
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
      docUrl.setAttribute("download", this.$t("CustomerExport.FileName"));
      document.body.appendChild(docUrl);
      docUrl.click();
    },

    /**
     * Hàm thực hiện sau khi sửa thông tin khách hàng
     * @param {*} customer Khách hàng
     * Author: LeDucTiep (12/07/2023)
     */
    afterPutCustomer(customer) {
      // Nếu có khách hàng
      if (this.customerList.length > 0)
        // Duyệt qua từng khách hàng
        for (let index = 0; index < this.customerList.length; index++)
          // Nếu khách hàng có mã khớp với khách hàng đã sửa
          if (this.customerList[index].CustomerId == customer.CustomerId) {
            // Cập nhật lại thông tin trên giao diện
            this.customerList[index] = customer;
            return;
          }
    },

    /**
     * Hàm thực hiện sau khi thêm khách hàng
     * @param {Object} customer Khách hàng
     * Author: LeDucTiep (12/07/2023)
     */
    afterPostCustomer(customer) {
      // Thêm customer mới vào đầu danh sách
      for (const index in this.columnList) {
        const field = this.columnList[index];
        console.log("Field: " + field, ",value: ", customer[field]);
        if (customer[field] == undefined) {
          customer[field] = "";
        }
      }
      this.customerList.unshift(customer);
      this.TotalRecord++;
    },

    /**
     * Hàm reset lại form
     * Author: LeDucTiep (12/07/2023)
     */
    resetAddCustomerForm() {
      this.customerFormKey++;
    },

    /**
     * Hàm xóa danh sách khách hàng
     * @param {Object} dataSend Thông tin gửi lên api
     * @returns bool
     * Author: LeDucTiep (12/07/2023)
     */
    async deleteListCustomers(dataSend) {
      // Gọi api xóa
      const response = await this.$msAxios(
        "delete",
        this.$msApi.CustomerApi.DeleteMany,
        {
          data: dataSend,
        }
      );

      // Request lỗi
      if (!response) {
        this.$msEmitter.emit("addNotice", {
          type: this.$msEnum.NoticeType.Error,
          message: this.$t("Error.UnknownError"),
        });
        return false;
      }

      // Số lượng bị xóa
      const deleteCount = response.data;

      // Lỗi
      if (deleteCount == 0) {
        this.$msEmitter.emit("addNotice", {
          type: this.$msEnum.NoticeType.Error,
          message: this.$t("Notice.Delete.Error"),
        });
        return false;
      }

      // Thành công
      this.$msEmitter.emit("addNotice", {
        type: this.$msEnum.NoticeType.Success,
        message: this.$t("Notice.Delete.ManySuccess", [deleteCount]),
      });

      // Xóa trên giao diện
      for (const i in this.selectedCustomerIdList) {
        let customerId = this.selectedCustomerIdList[i];

        const index = this.getIndexOfCustomerId(customerId);
        // Nếu tìm thấy thì mới xóa
        if (index > -1) {
          // Xóa đi 1 bản ghi từ vị trí index
          this.customerList.splice(index, 1);
          // giản số lượng bản ghi của trang hiện tại
          this.TotalRecord--;
        }
      }

      // Xóa các id đã chọn
      this.selectedList = {};

      // Nếu xóa hết toàn trang thì load lại trang
      if (this.customerList.length == 0) {
        this.loadCustomerList();
      }
    },

    /**
     * Hàm lấy index của khách hàng trong danh sách
     * @param {Guid} id id của khách hàng
     * @returns index
     * Author: LeDucTiep (12/07/2023)
     */
    getIndexOfCustomerId(id) {
      // Nếu danh sách nhân viên không tồn tại thì trở về
      if (!this.customerList) return -1;

      // Nếu danh sách không có nhân viên thì trở về
      if (this.customerList.length == 0) return -1;

      // Tìm và trả về index nếu thấy
      for (let i = 0; i < this.customerList.length; i++) {
        if (this.customerList[i].CustomerId == id) {
          return i;
        }
      }

      // Không tìm thấy
      return -1;
    },

    /**
     * Hàm ẩn dialog
     * @param {Enum} answer Câu trả lời ủa người dùng
     * Author: LeDucTiep (12/07/2023)
     */
    async hideDialog(answer) {
      switch (this.dialog.dialogType) {
        case this.$msEnum.DialogType.Delete:
          // Nếu user đã trả lời và đã xác định được customer cần xóa
          if (this.CustomerOnMenu && answer == this.$msEnum.DialogAnswer.Yes) {
            // Thì xóa thông tin nhân viên đó
            this.deleteCustomer(this.CustomerOnMenu);
          }
          break;

        case this.$msEnum.DialogType.DeleteSelected:
          // Nếu user đã trả lời và đã xác định được customer cần xóa
          if (
            this.selectedCustomerIdList.length > 0 &&
            answer == this.$msEnum.DialogAnswer.Yes
          ) {
            // Xóa các phần tử đã chọn
            this.deleteListCustomers(this.selectedCustomerIdList);
          }
          break;

        case this.$msEnum.DialogType.AlertFormChanged:
          // Nếu user đã trả lời
          this.$msEmitter.emit("closeCustomerFormChanged", answer);
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
     * @param {*} customer Khách hàng
     * Author: LeDucTiep (12/07/2023)
     */
    showDialog(type, messageList, customer) {
      // Định kiểu cho dialog
      this.dialog = {
        dialogType: type,
        isShowDialog: true,
        items: messageList,
      };

      // Truyền vào customer nếu là dialog delete
      switch (type) {
        case this.$msEnum.DialogType.Delete:
          this.dialog = {
            ...this.dialog,
            title: this.$t("Dialog.DeleteRecord"),
            isWarning: true,
            contentNo: this.$t("Button.No"),
            contentYes: this.$t("Button.Delete"),
          };
          this.CustomerOnMenu = customer;
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

    /**
     * Hàm xóa khách hàng
     * @param {Object} customer Khách hàng
     * @returns bool
     * Author: LeDucTiep (12/07/2023)
     */
    async deleteCustomer(customer) {
      // Gọi api xóa
      const response = await this.$msAxios(
        "delete",
        this.$msApi.CustomerApi.Delete(customer.CustomerId)
      );

      if (!response) {
        this.$msEmitter.emit("addNotice", {
          type: this.$msEnum.NoticeType.Error,
          message: this.$t("Error.UnknownError"),
        });
        return false;
      }

      const deleteCount = response.data;

      // Lỗi
      if (deleteCount == 0) {
        this.$msEmitter.emit("addNotice", {
          type: this.$msEnum.NoticeType.Error,
          message: this.$t("Notice.Delete.Error"),
        });
        return false;
      }

      // Thành công
      this.$msEmitter.emit("addNotice", {
        type: this.$msEnum.NoticeType.Success,
        message: this.$t("Notice.Delete.Success"),
      });

      // Xóa trên giao diện
      const index = this.customerList?.indexOf(customer);
      // Nếu tìm thấy thì mới xóa
      if (index > -1) {
        // Xóa đi 1 bản ghi từ vị trí index
        this.customerList.splice(index, 1);
        // giản số lượng bản ghi của trang hiện tại
        this.TotalRecord--;
      }
    },

    /**
     * Hàm lấy thông tin của một dòng
     * @param {Object} customer Khách hàng
     * @returns Object
     * Author: LeDucTiep (12/07/2023)
     */
    getDataRow(customer) {
      const data = {};
      for (const field in customer) {
        for (const colIndex in this.columnList) {
          const col = this.columnList[colIndex];
          if (col == field) {
            data[col] = customer[col];
          }
        }
      }
      return data;
    },

    /**
     * Hàm tải danh sách khách hàng
     * Author: LeDucTiep (12/07/2023)
     */
    async loadCustomerList() {
      this.customerList = null;
      const response = await this.$msAxios(
        "get",
        this.$msApi.CustomerApi.Paging,
        {
          params: {
            // Kích thước của trang
            pageSize: this.pageSizeSelected,
            // vị trí trang
            pageNumber: this.pageNumberSelected,
            // Từ khóa tìm kiếm
            searchTerm: this.inputSearchValue,
          },
        }
      );
      if (response.data) {
        this.TotalRecord = response.data.TotalRecord;
        this.customerList = response.data.Data;
      }
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
     * Hàm toggle tất cả các dòng
     * @param {bool} isChecked Đã check
     * Author: LeDucTiep (12/07/2023)
     */
    toggleAllRows(isChecked) {
      for (const index in this.customerList) {
        let customer = this.customerList[index];
        this.selectedList[customer.CustomerId] = isChecked;
      }
    },

    /**
     * Hàm hiện tooltip khi hover lên table header
     * Author: LeDucTiep (29/05/2023)
     */
    tableHeaderOnHover(event, tableHeader) {
      if (tableHeader.hint)
        this.$msEmitter.emit("showTooltip", event, tableHeader.hint);
    },

    /**
     * Hàm ẩn tooltip khi hover lên table header
     * Author: LeDucTiep (29/05/2023)
     */
    tableHeaderOnUnhover(tableHeader) {
      if (tableHeader.hint) {
        this.$msEmitter.emit("hideTooltip");
      }
    },

    /**
     * Hàm tích chọn một dòng
     * Author: LeDucTiep (06/05/2023)
     */
    setIdIntoSelectedList(id, isChecked) {
      if (!isChecked && this.selectedList[id]) {
        this.selectedList[id] = false;
      } else if (isChecked && !this.selectedList[id]) {
        this.selectedList[id] = true;
      }
    },

    /**
     * Hàm thực hiện khi nhấn đúp vào 1 dòng
     * @param {Object} customer Khách hàng
     * Author: LeDucTiep (12/07/2023)
     */
    customerListOnDoubleClick(customer) {
      // Chuyển hướng về màn hình sửa
      this.$router.push(`/customer/edit/${customer.CustomerId}`);
    },

    /**
     * Hàm mở form thêm khách hàng
     * Author: LeDucTiep (12/07/2023)
     */
    addCustomer() {
      this.$router.push(`/customer/create`);
    },

    /**
     * Hàm xóa bản ghi đã select
     * Author: LeDucTiep (12/07/2023)
     */
    deleteSelected() {
      // Hiện thông báo confirm
      this.showDialog(this.$msEnum.DialogType.DeleteSelected, [
        this.$t("DeleteSelectedAlert", [this.selectedCustomerIdList.length]),
      ]);
    },

    /**
     * Hiện menu các chức năng. vd: nhân bản, xóa, ngừng sử dụng
     * @param {event} event Event click
     * @param {Object} customer là customer đã hiện menu
     * Author: LeDucTiep (01/05/2023)
     */
    toggleMenuFeature(event, customer) {
      // Hiển thị menu danh sách các feature
      this.$msEmitter.emit("toggleMenuFeature", event, customer);
    },

    /**
     * Chuyển đến trang trước
     * Author: LeDucTiep (06/05/2023)
     */
    previousPage() {
      // Giảm vị trí trang
      this.pageNumberSelected--;
      // danh sách sẽ tự động thay đổi khi page number selected thay đổi

      this.loadCustomerList();
    },

    /**
     * Chuyển trang tiếp theo
     * Author: LeDucTiep (06/05/2023)
     */
    nextPage() {
      // tăng vị trí trang
      this.pageNumberSelected++;
      // danh sách sẽ tự động thay đổi khi page number selected thay đổi
      this.loadCustomerList();
    },
  },
};

export default mixin;

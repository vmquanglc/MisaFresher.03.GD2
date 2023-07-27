const mixin = {
  name: "ReceiptList",
  data() {
    return {
      receiptFormKey: 0,
      inputSearchValue: "",
      tableHeaders: [
        {
          text: "Ngày hạch toán",
        },
        {
          text: "Ngày chứng từ",
        },
        {
          text: "Số chứng từ",
        },
        {
          text: "Diễn giải",
        },
        {
          text: "Số tiền",
        },
        {
          text: "Đối tượng",
        },
        {
          text: "Mã đối tượng",
        },
        {
          text: "Địa chỉ",
        },
        {
          text: "Chức năng",
        },
      ],
      receiptList: null,
      columnList: [
        "AccountingDate",
        "ReceiptDate",
        "ReceiptCode",
        "Reason",
        "TotalMoney",
        "CustomerName",
        "CustomerCode",
        "Address",
      ],
      // Danh sách phần tử đã chọn
      selectedList: {},
      // Thông tin của dialog hiện lên
      dialog: {},
      TotalRecord: 0,

      TotalPage: 0,

      pageSizeSelected: 20,

      pageNumberSelected: 1,

      ReceiptOnMenu: null,
    };
  },
  watch: {
    async pageSizeSelected() {
      this.pageNumberSelected = 1;
      await this.loadReceiptList();
    },
  },
  computed: {
    selectedReceiptIdList() {
      let list = [];
      for (let id in this.selectedList) {
        if (this.selectedList[id]) {
          list.push(id);
        }
      }
      return list;
    },
    isAllRowsSelected() {
      if (!this.receiptList) return false;

      // Không có phần tử nào được chọn
      if (Object.keys(this.selectedList).length == 0) return false;

      for (const index in this.receiptList) {
        let receipt = this.receiptList[index];
        if (!this.selectedList[receipt.ReceiptId]) {
          return false;
        }
      }
      return true;
    },
  },
  created() {
    this.loadReceiptList();
    this.$msEmitter.on("resetAddReceiptForm", this.resetAddReceiptForm);
    this.$msEmitter.on("afterPostReceipt", this.afterPostReceipt);
    this.$msEmitter.on("afterPutReceipt", this.afterPutReceipt);
    this.$msEmitter.on("deleteReceipt", this.deleteReceipt);
    this.$msEmitter.on("deleteListReceipts", this.deleteListReceipts);
  },
  beforeUnmount() {
    this.$msEmitter.off("resetAddReceiptForm");
    this.$msEmitter.off("afterPostReceipt");
    this.$msEmitter.off("afterPutReceipt");
    this.$msEmitter.off("deleteReceipt");
    this.$msEmitter.off("deleteListReceipts");
  },
  methods: {
    /**
     * Hàm xuất khẩu
     * @param {Object} event
     * Author: LeDucTiep (12/07/2023)
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
        this.$msApi.ReceiptApi.ExportExcel,
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
      docUrl.setAttribute("download", this.$t("ReceiptExport.FileName"));
      document.body.appendChild(docUrl);
      docUrl.click();
    },

    /**
     * Hàm thực hiện sau khi sửa phiếu thu
     * @param {Object} receipt Phiếu thu
     * Author: LeDucTiep (12/07/2023)
     */
    afterPutReceipt(receipt) {
      // Nếu có khách hàng
      if (this.receiptList.length > 0)
        // Duyệt qua từng khách hàng
        for (let index = 0; index < this.receiptList.length; index++)
          // Nếu khách hàng có mã khớp với khách hàng đã sửa
          if (this.receiptList[index].EmployeeId == receipt.EmployeeId) {
            // Cập nhật lại thông tin trên giao diện
            this.receiptList[index] = receipt;
            return;
          }
    },

    /**
     * Hàm thực hiện sau khi thêm phiếu thu
     * @param {Object} receipt Phiếu thu
     * Author: LeDucTiep (12/07/2023)
     */
    afterPostReceipt(receipt) {
      // Thêm receipt mới vào đầu danh sách
      this.receiptList.unshift(receipt);
      this.TotalRecord++;
    },

    /**
     * Hàm reset form
     * Author: LeDucTiep (12/07/2023)
     */
    resetAddReceiptForm() {
      this.receiptFormKey++;
    },

    /**
     * Hàm xóa danh sách phiếu thu
     * @param {Object} dataSend
     * Author: LeDucTiep (12/07/2023)
     */
    async deleteListReceipts() {
      // Gọi api xóa
      const response = await this.$msAxios(
        "delete",
        this.$msApi.ReceiptApi.DeleteMany,
        {
          data: this.selectedReceiptIdList,
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
      for (const i in this.selectedReceiptIdList) {
        let receiptId = this.selectedReceiptIdList[i];

        const index = this.getIndexOfReceiptId(receiptId);
        // Nếu tìm thấy thì mới xóa
        if (index > -1) {
          // Xóa đi 1 bản ghi từ vị trí index
          this.receiptList.splice(index, 1);
          // giản số lượng bản ghi của trang hiện tại
          this.TotalRecord--;
        }
      }

      // Xóa các id đã chọn
      this.selectedList = {};

      // Nếu xóa hết toàn trang thì load lại trang
      if (this.receiptList.length == 0) {
        this.loadReceiptList();
      }
    },

    /**
     * Hàm lấy index của phiếu thu trong danh sách
     * @param {Guid} id Id của phiếu thu
     * @returns index của phiếu thu trong danh sách
     * Author: LeDucTiep (12/07/2023)
     */
    getIndexOfReceiptId(id) {
      // Nếu danh sách nhân viên không tồn tại thì trở về
      if (!this.receiptList) return -1;

      // Nếu danh sách không có nhân viên thì trở về
      if (this.receiptList.length == 0) return -1;

      // Tìm và trả về index nếu thấy
      for (let i = 0; i < this.receiptList.length; i++) {
        if (this.receiptList[i].ReceiptId == id) {
          return i;
        }
      }

      // Không tìm thấy
      return -1;
    },

    /**
     * Hàm xóa phiếu thu
     * @param {Object} receipt Phiếu thu
     * @returns bool
     * Author: LeDucTiep (12/07/2023)
     */
    async deleteReceipt(receipt) {
      // Gọi api xóa
      const response = await this.$msAxios(
        "delete",
        this.$msApi.ReceiptApi.Delete(receipt.ReceiptId)
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
      const index = this.receiptList?.indexOf(receipt);
      // Nếu tìm thấy thì mới xóa
      if (index > -1) {
        // Xóa đi 1 bản ghi từ vị trí index
        this.receiptList.splice(index, 1);
        // giản số lượng bản ghi của trang hiện tại
        this.TotalRecord--;
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
     * Hàm lấy thông tin của một dòng
     * @param {Object} receipt phiếu thu
     * @returns Thông tin của 1 dòng
     * Author: LeDucTiep (12/07/2023)
     */
    getDataRow(receipt) {
      const data = {};
      for (const colIndex in this.columnList) {
        for (const field in receipt) {
          const col = this.columnList[colIndex];
          if (col == field) {
            data[col] = receipt[col];
          }
        }
      }
      return data;
    },

    /**
     * Hàm tải danh sách phiếu thu
     * Author: LeDucTiep (12/07/2023)
     */
    async loadReceiptList() {
      this.receiptList = null;
      const response = await this.$msAxios(
        "get",
        this.$msApi.ReceiptApi.Paging,
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

      if (response?.data) {
        this.TotalRecord = response.data.TotalRecord;
        this.receiptList = response?.data?.Data.map((obj) => {
          if (obj.AccountingDate)
            obj.AccountingDate = this.$msDateFormater.convertIsoToDDMMYYY(
              obj.AccountingDate
            );
          if (obj.ReceiptDate)
            obj.ReceiptDate = this.$msDateFormater.convertIsoToDDMMYYY(
              obj.ReceiptDate
            );
          if (obj.TotalMoney)
            obj.TotalMoney = obj.TotalMoney
              ? this.numeralFormat(obj.TotalMoney, "0,0")
              : 0;

          return obj;
        });
      }
    },

    /**
     * Hàm toogle tất cả các dòng
     * @param {bool} isChecked
     * Author: LeDucTiep (12/07/2023)
     */
    toggleAllRows(isChecked) {
      for (const index in this.receiptList) {
        let receipt = this.receiptList[index];
        this.selectedList[receipt.ReceiptId] = isChecked;
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
     * Hàm thực hiện khi nhân đúp vào một dòng
     * @param {Object} receipt Phiếu thu
     * Author: LeDucTiep (12/07/2023)
     */
    receiptListOnDoubleClick(receipt) {
      // Chuyển hướng về màn hình sửa
      this.$router.push(`/cash/edit/${receipt.ReceiptId}`);
    },

    /**
     * Hàm thêm một phiếu thu
     * Author: LeDucTiep (12/07/2023)
     */
    addReceipt() {
      this.$router.push(`/cash/create`);
    },

    /**
     * Hàm xóa bản ghi đã select
     * Author: LeDucTiep (12/07/2023)
     */
    deleteSelected() {
      // Hiện thông báo confirm
      this.$msEmitter.emit(
        "showDialog",
        this.$msEnum.DialogType.DeleteSelected,
        [
          this.$t("DeleteSelectedAlert", [
            this.selectedReceiptIdList ? this.selectedReceiptIdList.length : 0,
          ]),
        ]
      );
    },

    /**
     * Hiện menu các chức năng. vd: nhân bản, xóa, ngừng sử dụng
     * @param {event} event Event click
     * @param {Object} receipt là receipt đã hiện menu
     * Author: LeDucTiep (01/05/2023)
     */
    toggleMenuFeature(event, receipt) {
      // Hiển thị menu danh sách các feature
      this.$msEmitter.emit("toggleMenuFeature", event, receipt);
    },

    /**
     * Chuyển đến trang trước
     * Author: LeDucTiep (06/05/2023)
     */
    async previousPage() {
      // Giảm vị trí trang
      this.pageNumberSelected--;
      // danh sách sẽ tự động thay đổi khi page number selected thay đổi
      await this.loadReceiptList();
    },

    /**
     * Chuyển trang tiếp theo
     * Author: LeDucTiep (06/05/2023)
     */
    async nextPage() {
      // tăng vị trí trang
      this.pageNumberSelected++;
      // danh sách sẽ tự động thay đổi khi page number selected thay đổi
      await this.loadReceiptList();
    },
  },
};

export default mixin;

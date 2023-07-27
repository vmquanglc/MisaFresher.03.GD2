const mixin = {
  name: "AccountList",
  data() {
    return {
      accountFormKey: 0,
      inputSearchValue: "",
      tableHeaders: [
        {
          text: this.$t("AccountList.AccountCode"),
        },
        {
          text: this.$t("AccountList.NameVi"),
        },
        {
          text: this.$t("AccountList.AccountProperty"),
        },
        {
          text: this.$t("AccountList.NameEn"),
        },
        {
          text: this.$t("AccountList.Note"),
        },
        {
          text: this.$t("AccountList.Feature"),
        },
      ],
      accountList: null,
      columnList: [
        "AccountCode",
        "NameVi",
        "AccountPropertyName",
        "NameEn",
        "Note",
      ],
      selectedId: null,
      // Danh sách phần tử đã chọn
      selectedList: {},
      // Thông tin của dialog hiện lên
      dialog: {},
      TotalRecord: 0,

      TotalPage: 0,

      pageSizeSelected: 20,

      pageNumberSelected: 1,

      AccountOnMenu: null,

      IsExpandedAll: true,
    };
  },
  watch: {
    async pageSizeSelected() {
      this.pageNumberSelected = 1;
      await this.loadAccountList();
    },
  },
  computed: {
    selectedAccountIdList() {
      let list = [];
      for (let id in this.selectedList) {
        if (this.selectedList[id]) {
          list.push(id);
        }
      }
      return list;
    },
    isAllRowsSelected() {
      if (!this.accountList) return false;

      // Không có phần tử nào được chọn
      if (Object.keys(this.selectedList).length == 0) return false;

      for (const index in this.accountList) {
        let account = this.accountList[index];
        if (!this.selectedList[account.AccountId]) {
          return false;
        }
      }
      return true;
    },
  },
  created() {
    this.loadAccountList();
    this.$msEmitter.on("resetAddAccountForm", this.resetAddAccountForm);
    this.$msEmitter.on("afterPostAccount", this.afterPostAccount);
    this.$msEmitter.on("afterPutAccount", this.afterPutAccount);
    this.$msEmitter.on("showDialog", this.showDialog);
    this.$msEmitter.on("hideDialog", this.hideDialog);
  },
  beforeUnmount() {
    this.$msEmitter.off("resetAddAccountForm");
    this.$msEmitter.off("afterPostAccount");
    this.$msEmitter.off("afterPutAccount");
    this.$msEmitter.off("showDialog");
    this.$msEmitter.off("hideDialog");
  },
  methods: {
    /**
     * Hàm collapse tất cả các dòng
     * Author: LeDucTiep (12/07/2023)
     */
    collapseAllRows() {
      for (const index in this.accountList) {
        const account = this.accountList[index];

        if (account && account.Rank == 1) {
          account.IsExpanded = false;
          this.collapseRow(account);
        }
      }

      this.IsExpandedAll = false;
    },

    /**
     * Hàm mở rộng tất cả các dòng
     * Author: LeDucTiep (12/07/2023)
     */
    async expandAllRows() {
      let checkContainsParent = () => {
        const myList = this.accountList.filter(
          (acc) => acc.IsParent && !acc.IsExpanded
        );
        return myList.length > 0;
      };
      while (checkContainsParent()) {
        for (let index = 0; index < this.accountList.length; index++) {
          const account = this.accountList[index];
          if (account.IsParent && !account.IsExpanded) {
            await this.expandRow(account);
            account.IsExpanded = true;
          }
        }
      }
      this.IsExpandedAll = true;
    },

    /**
     * Hàm mở rộng một dòng
     * @param {Object} account Tài khoản
     * Author: LeDucTiep (12/07/2023)
     */
    async expandRow(account) {
      const response = await this.$msAxios(
        "get",
        this.$msApi.AccountApi.Paging,
        {
          params: {
            // Kích thước của trang
            pageSize: this.pageSizeSelected,
            // vị trí trang
            pageNumber: this.pageNumberSelected,
            // Từ khóa tìm kiếm
            searchTerm: this.inputSearchValue,

            misaCode: account.MisaCode,
          },
        }
      );
      if (response.data) {
        const isExisted = (item) => {
          return (
            this.accountList.filter((x) => x.AccountId == item.AccountId)
              .length > 0
          );
        };

        const index = this.accountList.indexOf(account);
        if (index != -1) {
          for (const item of response.data.Data) {
            if (!isExisted(item)) {
              this.accountList.splice(index + 1, 0, item);
            }
          }
        }
      }
      this.TotalRecord = this.accountList.length;
    },

    /**
     * Hàm thu hẹp một dòng
     * @param {Object} account Tài khoản
     * Author: LeDucTiep (12/07/2023)
     */
    collapseRow(account) {
      this.accountList = this.accountList.filter(
        (acc) =>
          !acc.MisaCode.startsWith(account.MisaCode) ||
          acc.MisaCode == account.MisaCode
      );

      this.TotalRecord = this.accountList.length;
    },

    /**
     * Hàm thực hiện sau khi sửa tài khoản
     * @param {Object} account Tài khoản
     * Author: LeDucTiep (12/07/2023)
     */
    afterPutAccount(account) {
      // Duyệt qua từng khách hàng
      for (let index = 0; index < this.accountList.length; index++)
        // Nếu khách hàng có mã khớp với khách hàng đã sửa
        if (this.accountList[index].AccountId == account.AccountId) {
          // Cập nhật lại thông tin trên giao diện
          this.accountList[index] = account;
          return;
        }
    },

    /**
     * Hàm thực hiện sau khi thêm tài khoản
     * @param {Object} account Tài khoản
     * Author: LeDucTiep (12/07/2023)
     */
    afterPostAccount(account) {
      if (!account.NameEn) {
        account.NameEn = "";
      }
      if (!account.Note) {
        account.Note = "";
      }

      let isNotFound = true;
      // Tìm vị trí phần tử tra
      for (let index = 0; index < this.accountList.length; index++)
        // Nếu khách hàng có mã khớp với khách hàng đã sửa
        if (this.accountList[index].AccountId == account.AccountSyntheticId) {
          // Cập nhật lại thông tin trên giao diện

          this.accountList[index].IsExpanded =
            this.accountList[index].IsExpanded == undefined
              ? true
              : this.accountList[index].IsExpanded;
          this.accountList[index].IsParent = true;

          account.Rank = this.accountList[index].Rank + 1;
          account.MisaCode =
            this.accountList[index].MisaCode + account.AccountCode + "/";

          // Thêm vào
          this.accountList.splice(index + 1, 0, account);
          isNotFound = false;
          break;
        }

      if (isNotFound) {
        account.Rank = 1;
        account.MisaCode = account.AccountCode + "/";

        // Thêm vào
        this.accountList.splice(0, 0, account);
      }

      this.TotalRecord++;
    },

    /**
     * Hàm reset tài khoản
     * Author: LeDucTiep (12/07/2023)
     */
    resetAddAccountForm() {
      this.accountFormKey++;
    },

    /**
     * Hàm xóa danh sách tài khoản
     * @param {Object} dataSend
     * Author: LeDucTiep (12/07/2023)
     */
    async deleteListAccounts(dataSend) {
      // Gọi api xóa
      const response = await this.$msAxios(
        "delete",
        this.$msApi.AccountApi.DeleteMany,
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
      for (const i in this.selectedAccountIdList) {
        let accountId = this.selectedAccountIdList[i];

        const index = this.getIndexOfAccountId(accountId);
        // Nếu tìm thấy thì mới xóa
        if (index > -1) {
          // Xóa đi 1 bản ghi từ vị trí index
          this.accountList.splice(index, 1);
          // giản số lượng bản ghi của trang hiện tại
          this.TotalRecord--;
        }
      }

      // Xóa các id đã chọn
      this.selectedList = {};

      // Nếu xóa hết toàn trang thì load lại trang
      if (this.accountList.length == 0) {
        this.loadAccountList();
      }
    },

    /**
     * Hàm lấy index của tài khoản
     * @param {Guid} id Id của tài khoản
     * Author: LeDucTiep (12/07/2023)
     */
    getIndexOfAccountId(id) {
      // Nếu danh sách nhân viên không tồn tại thì trở về
      if (!this.accountList) return -1;

      // Nếu danh sách không có nhân viên thì trở về
      if (this.accountList.length == 0) return -1;

      // Tìm và trả về index nếu thấy
      for (let i = 0; i < this.accountList.length; i++) {
        if (this.accountList[i].AccountId == id) {
          return i;
        }
      }

      // Không tìm thấy
      return -1;
    },

    /**
     * Hàm ẩn dialog
     * @param {Enum} answer
     * Author: LeDucTiep (12/07/2023)
     */
    async hideDialog(answer) {
      switch (this.dialog.dialogType) {
        case this.$msEnum.DialogType.Delete:
          // Nếu user đã trả lời và đã xác định được account cần xóa
          if (this.AccountOnMenu && answer == this.$msEnum.DialogAnswer.Yes) {
            // Thì xóa thông tin nhân viên đó
            this.deleteAccount(this.AccountOnMenu);
          }
          break;

        case this.$msEnum.DialogType.DeleteSelected:
          // Nếu user đã trả lời và đã xác định được account cần xóa
          if (
            this.selectedAccountIdList.length > 0 &&
            answer == this.$msEnum.DialogAnswer.Yes
          ) {
            // Xóa các phần tử đã chọn
            this.deleteListAccounts(this.selectedAccountIdList);
          }
          break;

        case this.$msEnum.DialogType.AlertFormChanged:
          // Nếu user đã trả lời
          this.$msEmitter.emit("closeAccountFormChanged", answer);
          break;

        default:
          break;
      }
      this.dialog.isShowDialog = false;
    },

    /**
     * Hàm hiện dialog
     * @param {*} type Loại dialog
     * @param {*} messageList Danh sách thông báo
     * @param {*} account Tài khoản
     * Author: LeDucTiep (12/07/2023)
     */
    showDialog(type, messageList, account) {
      // Định kiểu cho dialog
      this.dialog = {
        dialogType: type,
        isShowDialog: true,
        items: messageList,
      };

      // Truyền vào account nếu là dialog delete
      switch (type) {
        case this.$msEnum.DialogType.FeatureNotSupported:
          this.dialog = {
            ...this.dialog,
            title: this.$t("Dialog.DefaultTitle"),
            isWarning: false,
            contentYes: this.$t("Button.Accept"),
          };
          this.AccountOnMenu = account;
          break;

        case this.$msEnum.DialogType.Delete:
          this.dialog = {
            ...this.dialog,
            title: this.$t("Dialog.DeleteRecord"),
            isWarning: true,
            contentNo: this.$t("Button.No"),
            contentYes: this.$t("Button.Delete"),
          };
          this.AccountOnMenu = account;
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
     * Hàm xóa tài khoản
     * @param {Object} account
     * Author: LeDucTiep (12/07/2023)
     */
    async deleteAccount(account) {
      // Gọi api xóa
      const response = await this.$msAxios(
        "delete",
        this.$msApi.AccountApi.Delete(account.AccountId)
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
      const index = this.accountList.indexOf(account);
      // Nếu tìm thấy thì mới xóa
      if (index > -1) {
        // Xóa đi 1 bản ghi từ vị trí index
        this.accountList.splice(index, 1);
        // giản số lượng bản ghi của trang hiện tại
        this.TotalRecord--;
      }
    },

    /**
     * Hàm lấy thông tin của một dòng
     * @param {Object} account
     * Author: LeDucTiep (12/07/2023)
     */
    getDataRow(account) {
      const data = {};
      for (const colIndex in this.columnList) {
        for (const field in account) {
          const col = this.columnList[colIndex];
          if (col == field) {
            data[col] = account[col];
          }
        }
      }
      return data;
    },

    /**
     * Hàm xuất khẩu dữ liệu
     * @param {*} event
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
        this.$msApi.AccountApi.ExportExcel,
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
      docUrl.setAttribute("download", this.$t("AccountExport.FileName"));
      document.body.appendChild(docUrl);
      docUrl.click();
    },

    /**
     * Hàm tải danh sách tài khoản
     * Author: LeDucTiep (12/07/2023)
     */
    async loadAccountList() {
      this.accountList = null;
      const response = await this.$msAxios(
        "get",
        this.$msApi.AccountApi.Paging,
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
        this.accountList = response.data.Data;

        for (const index in this.accountList) {
          const acc = this.accountList[index];
          if (this.hasChildInList(acc)) {
            acc.IsExpanded = true;
          }
        }

        this.TotalRecord = response.data.TotalRecord;
      }
    },

    hasChildInList(account) {
      for (const index in this.accountList) {
        const acc = this.accountList[index];
        if (
          acc.MisaCode.startsWith(account.MisaCode) &&
          acc.MisaCode != account.MisaCode
        )
          return true;
      }
      return false;
    },

    /**
     * Hàm toggle tất cả các dòng
     * @param {Bool} isChecked
     * Author: LeDucTiep (12/07/2023)
     */
    toggleAllRows(isChecked) {
      for (const index in this.accountList) {
        let account = this.accountList[index];
        this.selectedList[account.AccountId] = isChecked;
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
     * Hàm thực hiện khi kích đúp vào một dòng
     * @param {Object} account
     * Author: LeDucTiep (12/07/2023)
     */
    accountListOnDoubleClick(account) {
      // Chuyển hướng về màn hình sửa
      this.$router.push(`/account/edit/${account.AccountId}`);
    },

    /**
     * Hàm thêm tài khoản
     * Author: LeDucTiep (12/07/2023)
     */
    addAccount() {
      if (this.selectedId)
        this.$router.push(`/account/create-child/${this.selectedId}`);
      else this.$router.push(`/account/create`);
    },

    /**
     * Hàm các xóa tài khoản đã chọn
     * Author: LeDucTiep (12/07/2023)
     */
    deleteSelected() {
      // Hiện thông báo confirm
      this.showDialog(this.$msEnum.DialogType.DeleteSelected, [
        this.$t("DeleteSelectedAlert", [this.selectedAccountIdList.length]),
      ]);
    },

    /**
     * Hàm hiển thị thông báo tính năng chưa hỗ trọ
     * Author: LeDucTiep (12/07/2023)
     */
    notSupported() {
      this.showDialog(this.$msEnum.DialogType.FeatureNotSupported, [
        this.$t("Dialog.FeatureNotSupported"),
      ]);
    },

    /**
     * Hiện menu các chức năng. vd: nhân bản, xóa, ngừng sử dụng
     * @param {event} event Event click
     * @param {Object} account là account đã hiện menu
     * Author: LeDucTiep (01/05/2023)
     */
    toggleMenuFeature(event, account) {
      // Hiển thị menu danh sách các feature
      this.$msEmitter.emit("toggleMenuFeature", event, account);
    },

    /**
     * Chuyển đến trang trước
     * Author: LeDucTiep (06/05/2023)
     */
    async previousPage() {
      // Giảm vị trí trang
      this.pageNumberSelected--;
      // danh sách sẽ tự động thay đổi khi page number selected thay đổi

      await this.loadAccountList();
    },

    /**
     * Chuyển trang tiếp theo
     * Author: LeDucTiep (06/05/2023)
     */
    async nextPage() {
      // tăng vị trí trang
      this.pageNumberSelected++;
      // danh sách sẽ tự động thay đổi khi page number selected thay đổi
      await this.loadAccountList();
    },
  },
};

export default mixin;

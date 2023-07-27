const mixin = {
  name: "EmployeeList",
  props: {
    // Từ khóa tìm kiếm
    inputSearchValue: String,
  },
  data: function () {
    return {
      // Danh sách nhân viên
      employeeList: null,
      // Tiêu đề các cột
      tableHeaders: [
        { text: this.$t("EmployeeList.EmployeeCode") },
        { text: this.$t("EmployeeList.FullName") },
        { text: this.$t("EmployeeList.Gender") },
        { text: this.$t("EmployeeList.DateOfBirth") },
        {
          text: this.$t("EmployeeList.IdentityNumberShort"),
          hint: this.$t("EmployeeList.IdentityNumber"),
        },
        { text: this.$t("EmployeeList.PositionName") },
        { text: this.$t("EmployeeList.DepartmentName") },
        {
          text: this.$t("EmployeeList.BankAccountNumberShort"),
          hint: this.$t("EmployeeList.BankAccountNumber"),
        },
        { text: this.$t("EmployeeList.NameOfBank") },
        {
          text: this.$t("EmployeeList.BankAccountBranchShort"),
          hint: this.$t("EmployeeList.BankAccountBranch"),
        },
        { text: this.$t("EmployeeList.Function") },
      ],
      // Thông tin của dialog hiện lên
      dialog: {},
      // employee đã mở ra dialog
      EmployeeOnMenu: null,
      // vị trí trang hiện tại, người dùng sẽ thay đổi trường này
      pageNumberSelected: 1,
      // kích thước trang, người dùng sẽ thay đổi kích thước trang ở đây
      pageSizeSelected: 20,
      // tổng số lượng bản ghi có trong db phù hợp với request
      TotalRecord: 0,
      // tổng số lượng trang
      TotalPage: 0,
      // Danh sách phần tử đã chọn
      selectedList: {},
    };
  },
  watch: {
    /**
     * Hàm theo dõi số thứ tự trang
     * @param {Number} newQuestion Số trang mới
     * @param {Number} oldQuestion Số trang cũ
     * Author: LeDucTiep (15/06/2023)
     */
    pageNumberSelected(newQuestion, oldQuestion) {
      // Trường hợp 1 == '1' thì không cần thay đổi làm gì
      if (newQuestion != oldQuestion) {
        // Tải lại danh sách khi thay đổi page number
        this.loadListByPage();
      }
    },
    /**
     * Hàm theo dõi sự thay đổi của kích thước trang đã chọn
     * @param {Number} newQuestion Giá trị mới
     * @param {Number} oldQuestion Giá trị cũ
     * Author: LeDucTiep (15/06/2023)
     */
    pageSizeSelected(newQuestion, oldQuestion) {
      // Trường hợp 1 == '1' thì không cần thay đổi làm gì
      if (newQuestion != oldQuestion) {
        // Khi thay đổi kích thước trang thì chuyển về trang đầu
        this.pageNumberSelected = 1;
        // Tải lại danh sách khi thay đổi kích thước danh sách
        this.loadListByPage();
      }
    },
  },
  computed: {
    /**
     * Lấy tất cả các employeeId đã chọn
     * Author: LeDucTiep (29/05/2023)
     */
    selectedEmployeeIdList() {
      let list = [];
      for (let id in this.selectedList) {
        if (this.selectedList[id]) {
          list.push(id);
        }
      }
      return list;
    },
    /**
     * Kiểm tra tất cả các employeeId của trang đã chọn hết chưa
     * Author: LeDucTiep (29/05/2023)
     */
    isAllRowsSelected() {
      if (!this.employeeList) return false;

      // Không có phần tử nào được chọn
      if (Object.keys(this.selectedList).length == 0) return false;

      for (const index in this.employeeList) {
        let employee = this.employeeList[index];
        if (!this.selectedList[employee.EmployeeId]) {
          return false;
        }
      }
      return true;
    },
  },
  methods: {
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
     * Hàm xóa tất cả employee đã chọn
     * Author: LeDucTiep (29/05/2023)
     */
    deleteAllEmployeeOnCurrentPage() {
      // Hiện thông báo confirm
      this.showDialog(this.$msEnum.DialogType.DeleteSelected, [
        this.$t("DeleteSelectedAlert", [this.selectedEmployeeIdList.length]),
      ]);
    },

    /**
     * Hàm tích hoặc bỏ tất cả các dòng
     * Author: LeDucTiep (06/05/2023)
     */
    toggleAllEmployee(isChecked) {
      for (const index in this.employeeList) {
        let employee = this.employeeList[index];
        this.selectedList[employee.EmployeeId] = isChecked;
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
     * Chuyển đến trang trước
     * Author: LeDucTiep (06/05/2023)
     */
    previousPage() {
      // Giảm vị trí trang
      this.pageNumberSelected--;
      // danh sách sẽ tự động thay đổi khi page number selected thay đổi
    },

    /**
     * Chuyển trang tiếp theo
     * Author: LeDucTiep (06/05/2023)
     */
    nextPage() {
      // tăng vị trí trang
      this.pageNumberSelected++;
      // danh sách sẽ tự động thay đổi khi page number selected thay đổi
    },

    /**
     * Tải danh sách theo trang
     * Author: LeDucTiep (05/05/2023)
     */
    loadListByPage() {
      // Chỉnh sửa router
      this.$router.replace(
        `/employee/pageSize=${this.pageSizeSelected}/pageNumber=${this.pageNumberSelected}`
      );
      // Load lại danh sách employee
      this.loadEmployeeList();
    },

    /**
     * Hiện menu các chức năng. vd: nhân bản, xóa, ngừng sử dụng
     * @param {event} event Event click
     * @param {employee} employee là employee đã hiện menu
     * Author: LeDucTiep (01/05/2023)
     */
    toggleMenuFeature(event, employee) {
      // Hiển thị menu danh sách các feature
      this.$msEmitter.emit("toggleMenuFeature", event, employee);
    },

    /**
     * Hàm lấy danh sách employee
     * Author: LeDucTiep (01/05/2023)
     */
    async loadEmployeeList() {
      // Xóa danh sách cũ
      this.employeeList = null;

      // Nếu ở chức năng tìm kiếm thì
      if (this.inputSearchValue) {
        // reset vị trí trang về 1
        this.pageNumberSelected = 1;
      }

      // Lấy danh sách employee về theo kích thước trang, vị trí trang và từ khóa tìm kiếm
      const response = await this.$msAxios(
        "get",
        this.$msApi.EmployeeApi.Paging,
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

      if (!response) {
        this.$msEmitter.emit("addNotice", {
          type: this.$msEnum.NoticeType.Error,
          message: this.$t("Error.UnknownError"),
        });
        return false;
      }

      // Nạp danh sách
      this.employeeList = response.data.Data;

      // Hiển thị tổng bản ghi
      this.TotalRecord = response.data.TotalRecord;

      // Hiển thị tổng trang
      this.TotalPage = Math.ceil(this.TotalRecord / this.pageSizeSelected);

      // Hiển thị danh sách rỗng khi không tìm thấy bản ghi nào
      if (!this.employeeList) {
        this.employeeList = [];
      }
    },

    /**
     * Hàm sự kiện nhấn đúp vào 1 dòng trong danh sách nhân viên
     * @param {employee} employee Employee bị double click
     * Author: LeDucTiep (01/05/2023)
     */
    employeeListOnDoubleClick(employee) {
      // Chuyển hướng về màn hình sửa
      this.$router.push(`/employee/edit/${employee.EmployeeId}`);
    },

    /**
     * Ẩn dialog và thực hiện lệnh của dialog mà người dùng đã xác nhận
     * @param {answer} answer Câu trả lời của người dùng
     * Author: LeDucTiep (01/05/2023)
     */
    async hideDialog(answer) {
      switch (this.dialog.dialogType) {
        case this.$msEnum.DialogType.Delete:
          // Nếu user đã trả lời và đã xác định được employee cần xóa
          if (this.EmployeeOnMenu && answer == this.$msEnum.DialogAnswer.Yes) {
            // Thì xóa thông tin nhân viên đó
            this.deleteEmployee(this.EmployeeOnMenu);
          }
          break;

        case this.$msEnum.DialogType.DeleteSelected:
          // Nếu user đã trả lời và đã xác định được employee cần xóa
          if (
            this.selectedEmployeeIdList.length > 0 &&
            answer == this.$msEnum.DialogAnswer.Yes
          ) {
            // Xóa các phần tử đã chọn
            this.deleteListEmployees(this.selectedEmployeeIdList);
          }
          break;

        case this.$msEnum.DialogType.AlertFormChanged:
          // Nếu user đã trả lời
          this.$msEmitter.emit("closeEmployeeFormChanged", answer);
          break;

        default:
          break;
      }
      this.dialog.isShowDialog = false;
    },

    /**
     * Hàm xóa một nhân viên
     * @param {*} employee Nhân viên cần xóa
     * Author: LeDucTiep (09/05/2023)
     */
    async deleteEmployee(employee) {
      // Gọi api xóa
      const response = await this.$msAxios(
        "delete",
        this.$msApi.EmployeeApi.Delete(employee.EmployeeId)
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
      const index = this.employeeList?.indexOf(employee);
      // Nếu tìm thấy thì mới xóa
      if (index > -1) {
        // Xóa đi 1 bản ghi từ vị trí index
        this.employeeList.splice(index, 1);
        // giản số lượng bản ghi của trang hiện tại
        this.TotalRecord--;
      }
    },

    /**
     * Hàm xóa nhiều
     * Author: LeDucTiep (29/05/2023)
     */
    async deleteListEmployees(dataSend) {
      // Gọi api xóa
      const response = await this.$msAxios(
        "delete",
        this.$msApi.EmployeeApi.DeleteMany,
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
      for (const i in this.selectedEmployeeIdList) {
        let employeeId = this.selectedEmployeeIdList[i];

        const index = this.getIndexOfEmployeeId(employeeId);
        // Nếu tìm thấy thì mới xóa
        if (index > -1) {
          // Xóa đi 1 bản ghi từ vị trí index
          this.employeeList.splice(index, 1);
          // giản số lượng bản ghi của trang hiện tại
          this.TotalRecord--;
        }
      }

      // Xóa các id đã chọn
      this.selectedList = {};

      // Nếu xóa hết toàn trang thì load lại trang
      if (this.employeeList.length == 0) {
        this.loadEmployeeList();
      }
    },

    /**
     * Hàm lấy index của employee trong danh sách thông qua id
     * @param {*} id Id của phần tử cần lấy id
     * Author: LeDucTiep (01/05/2023)
     */
    getIndexOfEmployeeId(id) {
      // Nếu danh sách nhân viên không tồn tại thì trở về
      if (!this.employeeList) return -1;

      // Nếu danh sách không có nhân viên thì trở về
      if (this.employeeList.length == 0) return -1;

      // Tìm và trả về index nếu thấy
      for (let i = 0; i < this.employeeList.length; i++) {
        if (this.employeeList[i].EmployeeId == id) {
          return i;
        }
      }

      // Không tìm thấy
      return -1;
    },

    /**
     * Hiện dialog
     * @param {dialogType} dialogType Loại của dialog đã hiện lên
     * @param {*} messageList Là Array các thông tin cần thông báo
     * Author: LeDucTiep (01/05/2023)
     */
    showDialog(type, messageList, employee) {
      // Định kiểu cho dialog
      this.dialog = {
        dialogType: type,
        isShowDialog: true,
        items: messageList,
      };

      // Truyền vào employee nếu là dialog delete
      switch (type) {
        case this.$msEnum.DialogType.Delete:
          this.dialog = {
            ...this.dialog,
            title: this.$t("Dialog.DeleteRecord"),
            isWarning: true,
            contentNo: this.$t("Button.No"),
            contentYes: this.$t("Button.Delete"),
          };
          this.EmployeeOnMenu = employee;
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
     * Hiển thị phần tử vừ thêm lên giao diện, không load lại trang
     * @param {Employee} employee nhân viên đã post, thêm
     * Author: LeDucTiep (07/05/2023)
     */
    afterPostEmployee(employee) {
      // Thêm employee mới vào đầu danh sách
      this.employeeList.unshift(employee);
      this.TotalRecord++;
    },

    /**
     * Hiển thị lại phần tử bị sửa trên giao diện, không load lại trang
     * @param {Employee} employee nhân viên đã put, sửa
     * Author: LeDucTiep (07/05/2023)
     */
    afterPutEmployee(employee) {
      // Nếu có nhân viên
      if (this.employeeList.length > 0)
        // Duyệt qua từng nhân viên
        for (let index = 0; index < this.employeeList.length; index++)
          // Nếu nhân viên có mã khớp với nhân viên đã sửa
          if (this.employeeList[index].EmployeeId == employee.EmployeeId) {
            // Cập nhật lại thông tin trên giao diện
            this.employeeList[index] = employee;
            return;
          }
    },
  },

  created() {
    // Lấy page size từ router
    if (this.$route.params.pageSize)
      this.pageSizeSelected = this.$route.params.pageSize;

    // Lấy page number từ router
    if (this.$route.params.pageNumber)
      this.pageNumberSelected = this.$route.params.pageNumber;

    // Nếu không lấy được thông tin từ router thì lấy giá trị mặc định và load danh sách nhân viên
    if (!this.$route.params.pageNumber || !this.$route.params.pageSize) {
      // Kích thước và vị trí trang mặc định
      this.pageSizeSelected = 20;
      this.pageNumberSelected = 1;
    }
    this.loadEmployeeList();

    // hiện dialog
    this.$msEmitter.on("showDialog", this.showDialog);

    // ẩn dialog
    this.$msEmitter.on("hideDialog", this.hideDialog);

    // khi nhấn nút click thì reload lại danh sách
    this.$msEmitter.on("reloadEmployeeOnclick", this.loadEmployeeList);

    // hàm thay đổi trên giao diện sau khi thay đổi employee trên api xong
    this.$msEmitter.on("afterPutEmployee", this.afterPutEmployee);

    // thêm mới employee trên giao diện sau khi thêm bằng api
    this.$msEmitter.on("afterPostEmployee", this.afterPostEmployee);

    // sựa kiện từ EmployeeHome khi ô search nhập xong, nhấn nút search hoặc nhấn enter
    this.$msEmitter.on("searchEmployee", this.loadListByPage);
  },

  beforeUnmount() {
    // Xóa các sự kiên emit
    this.$msEmitter.off("showDialog");
    this.$msEmitter.off("hideDialog");
    this.$msEmitter.off("reloadEmployeeOnclick");
    this.$msEmitter.off("afterPutEmployee");
    this.$msEmitter.off("afterPostEmployee");
    this.$msEmitter.off("searchEmployee");
  },
};

export default mixin;

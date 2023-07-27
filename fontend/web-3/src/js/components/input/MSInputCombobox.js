import MSResource from "../../resource/resource.js";

const mixin = {
  name: "MSInputCombobox",
  props: {
    disabled: {
      type: Boolean,
      default: false,
    },
    align: {
      type: String,
      default: "left",
    },
    // apiPaging để lấy dữ liệu của combobox
    apiPaging: String,
    apiGetById: [String, Function],
    // Tên thuộc tính chứa text
    propText: String,
    // Tên thuộc tính chứa value
    propValue: String,
    // chứa id mặc định chọn khi khởi tạo
    id: [String, Number],
    // chứa value mặc định khi khởi tạo
    value: [String, Number],
    // tooltip của combobox
    tooltip: {
      type: String,
      default: "",
    },
    // true: Nếu input để trống sẽ hiện trạng thái lỗi
    // false: Không có trạng thái nhập lỗi
    isValidating: {
      type: Boolean,
      default: false,
    },
    // Là ô input lỗi
    isValid: {
      type: Boolean,
      default: true,
    },
    // label của combobox
    title: String,
    // là ô bắt buộc
    isRequired: {
      type: Boolean,
      default: false,
    },
    // Thông báo lỗi
    errorMessage: {
      type: String,
      default: MSResource.VN.Error.Required,
    },
    placeholder: String,
    // Danh sách các phần tử của combobox
    listItems: Array,
    listCols: {
      type: Array,
    },
    listTitle: {
      type: Array,
    },
    readonly: {
      type: Boolean,
      default: false,
    },
  },
  emits: ["update:id", "update:value", "update:isValid", "valueOnChange"],
  computed: {
    inputValueComputed: {
      get() {
        return this.disabled ? "" : this.inputText;
      },
      set(value) {
        this.inputText = value;
      },
    },
  },
  data() {
    return {
      // Thông báo lỗi
      message: this.errorMessage,
      // Danh sách các phần tử và bị thay đổi khi input thay đổi
      items: null,
      rootItems: this.listItems,
      // Ẩn hiện danh sách lựa chọn
      isShowList: false,
      // Text hiển thị trong ô input
      inputText: this.value,
      // Value của combobox
      inputValue: this.id,
      // True: kích hoạt trạng thái input lỗi
      // False: trạng thái input không lỗi
      isInvalidInput: false,
      // Đang dùng key để chọn danh sách
      isUsingKey: false,
      // Phần tử chọn bằng key
      indexKeyChoosing: 0,
      // kích thước của trang
      pageSize: 20,
      // Số trang
      pageNumber: 1,
      // Độ rộng của cột
      columnWidthList: {},
    };
  },
  async created() {
    await this.initCombobox();
  },
  mounted() {
    this.isShowList = false;
  },
  beforeUnmount() {
    // Đóng tooltip
    this.$msEmitter.emit("hideTooltip");
  },
  watch: {
    /**
     * Khi danh sách các item truyền vào thay đổi thì cập nhật lại item đã chọn
     * Author: LeDucTiep (04/05/2023)
     */
    listItems() {
      if (this.listItems) {
        // truyền sang biến tạm để sử dụng
        this.items = [...this.listItems];
        this.rootItems = [...this.listItems];
        // Chọn lại item đã chọn
        this.idOnChange();
      }
    },

    /**
     * Khi id đã chọn truyền vào thay đổi thì cập nhật lại inputText và inputValue
     * Author: LeDucTiep (04/05/2023)
     */
    id() {
      this.idOnChange();
    },

    /**
     * Hàm phát hiện ô input bị lỗi và bắn lỗi ra ngoài
     * Author: LeDucTiep (10/06/2023)
     */
    isInvalidInput() {
      this.$emit("update:isValid", !this.isInvalidInput);
    },

    /**
     * Khi prop isValidating cập nhật thì xét xem inputText có hợp lệ không
     * Author: LeDucTiep (04/05/2023)
     */
    isValidating() {
      this.inputTextOnChange();
      this.isShowList = false;
    },

    async apiPaging() {
      await this.initCombobox();
    },
  },
  methods: {
    /**
     * Hàm focus vào ô input
     * Author: LeDucTiep (27/05/2023)
     */
    focusInput() {
      this.$refs.myFirstInput.focus();
    },
    /**
     * Hàm tính toán lại độ rộng của cột
     * Author: LeDucTiep (12/07/2023)
     */
    reCalculateColumnWidth() {
      if (this.listTitle) {
        for (const index in this.listTitle) {
          const title = this.listTitle[index];

          this.columnWidthList[index] = title.length;
        }
      }
      if (this.itemsRoot) {
        for (const indexItem in this.itemsRoot) {
          const item = this.itemsRoot[indexItem];

          for (const index in this.listCols) {
            const field = this.listCols[index];
            if (item[field]) {
              this.columnWidthList[index] = Math.max(
                item[field].length,
                this.columnWidthList[index]
              );
            }
          }
        }
      }
    },
    /**
     * Hàm khởi tạo combobox
     * Author: LeDucTiep (30/05/2023)
     */
    async initCombobox() {
      this.inputText = "";
      this.inputValue = "";
      if (this.apiPaging) {
        // dùng apiPaging để lấy danh sách items
        const response = await this.$msAxios("get", this.apiPaging);

        if (
          !response ||
          response.status == this.$msEnum.HttpStatusCode.Success
        ) {
          this.items = [...response.data.Data];
          this.rootItems = [...response.data.Data];
        }
      }

      this.inputTextOnChange();

      this.idOnChange();
      this.reCalculateColumnWidth();
    },
    /**
     * Hàm ẩn khi nhấn ra bên ngoài
     * Author: LeDucTiep (30/05/2023)
     */
    comboboxClickOutSide() {
      this.isShowList = false;
    },

    /**
     * Hàm focus ô input tiếp theo
     * @param {*} elem Phần từ phát ra sự kiện enter
     * Author: LeDucTiep (28/05/2023)
     */
    focusNext(elem) {
      if (!elem.form) return;
      const currentIndex = Array.from(elem.form.elements).indexOf(elem);
      elem.form.elements
        .item(
          currentIndex < elem.form.elements.length - 1 ? currentIndex + 1 : 0
        )
        .focus();
      this.isShowList = false;
    },

    /**
     * Hàm thực hiện khi nhấn nút mũi tên đi xuống
     * Author: LeDucTiep (28/05/2023)
     */
    onPressKeyArrowDown() {
      try {
        if (this.readonly) return;

        if ((!this.items || this.items.length == 1) && this.rootItems) {
          this.items = [...this.rootItems];
        }
        // Đánh dấu đang sử dụng phím tắt
        this.isUsingKey = true;

        // Thay đổi phần tử đã chọn
        if (this.isShowList) {
          // Tăng index
          if (this.indexKeyChoosing == this.items.length - 1)
            this.indexKeyChoosing = 0;
          else this.indexKeyChoosing++;
        } else {
          // Nếu là lần nhấn đầu thì mở list lên
          this.isShowList = true;
        }

        try {
          // Cập nhật cho ô input
          this.inputText = this.items[this.indexKeyChoosing][this.propText];
          this.inputValue = this.items[this.indexKeyChoosing][this.propValue];

          // Update lên component cha
          this.emitUpdateValue(this.items[this.indexKeyChoosing]);
        } finally {
          this.$emit("update:isValid", true);

          // Hủy bỏ mọi lỗi
          this.isInvalidInput = false;
        }
      } catch {
        //
      }
    },

    /**
     * Hàm thực hiện khi nhấn nút mũi tên lên
     * Author: LeDucTiep (28/05/2023)
     */
    onPressKeyArrowUp() {
      try {
        if (this.readonly) return;

        if ((!this.items || this.items.length == 1) && this.rootItems) {
          this.items = [...this.rootItems];
        }
        // Đánh dấu đang sử dụng phím tắt
        this.isUsingKey = true;

        // Thay đổi phần tử đã chọn
        if (this.isShowList) {
          // Giảm index
          if (this.indexKeyChoosing == 0)
            this.indexKeyChoosing = this.items.length - 1;
          else this.indexKeyChoosing--;
        } else {
          // Nếu là lần nhấn đầu thì mở list lên
          this.isShowList = true;
        }

        // Cập nhật cho ô input

        this.inputText = this.items[this.indexKeyChoosing][this.propText];
        this.inputValue = this.items[this.indexKeyChoosing][this.propValue];

        // Update lên component cha
        this.emitUpdateValue(this.items[this.indexKeyChoosing]);
        this.$emit("update:isValid", true);

        // Hủy bỏ mọi lỗi
        this.isInvalidInput = false;
      } catch {
        //
      }
    },

    /**
     * Hàm thực hiện khi người dùng nhập xong được 1 giây
     * Author: LeDucTiep (28/05/2023)
     */
    async finishedTyping() {
      if (this.readonly) return;

      // Nếu chưa nhập hoặc danh sách lựa chọn là rỗng
      if (!this.inputText || !this.rootItems) return;

      if (this.apiPaging) {
        const response = await this.$msAxios("get", this.apiPaging, {
          params: {
            // Kích thước của trang
            pageSize: this.pageSize,
            // vị trí trang
            pageNumber: this.pageNumber,
            // Từ khóa tìm kiếm
            searchTerm: this.inputText,
          },
        });

        if (response) {
          this.rootItems = response.data.Data;
          this.items = [...this.rootItems];
        }
      }

      let flagNotFound = true;

      // Tìm kiếm phần tử phù hợp

      for (const item of this.rootItems) {
        if (
          item[this.propText]
            .toLowerCase()
            .replaceAll(/\s/g, "")
            .search(this.inputText.toLowerCase().replaceAll(/\s/g, "")) == 0 &&
          this.rootItems.filter((item) =>
            item[this.propText]
              .toLowerCase()
              .replaceAll(/\s/g, "")
              .includes(this.inputText.toLowerCase().replaceAll(/\s/g, ""))
          ).length == 1
        ) {
          // Nếu khi gõ xong mà chỉ còn 1 phần tử khớp
          // Thì gán luôn cho ô input
          this.inputValue = item[this.propValue];
          this.inputText = item[this.propText];
          this.$emit("update:isValid", true);
          this.isShowList = false;
          flagNotFound = false;
          this.emitUpdateValue(item);
          break;
        }
      }

      // Nếu không nhập đúng thì
      if (flagNotFound) {
        this.isInvalidInput = true;
        this.message = "Không hợp lệ";
      }
    },

    /**
     * Hàm thực hiện khi scroll
     * Author: LeDucTiep (30/05/2023)
     */
    onScroll({ target: { scrollTop, clientHeight, scrollHeight } }) {
      if (scrollTop + clientHeight + 2 >= scrollHeight) {
        this.loadMorePosts();
      }
    },

    /**
     * Hàm tải thêm dữ liệu
     * Author: LeDucTiep (30/05/2023)
     */
    async loadMorePosts() {
      if (!this.apiPaging) return;

      this.pageNumber++;

      const response = await this.$msAxios("get", this.apiPaging, {
        params: {
          // Kích thước của trang
          pageSize: this.pageSize,
          // vị trí trang
          pageNumber: this.pageNumber,
        },
      });

      if (response) {
        const resData = response.data.Data;

        for (const index in resData) {
          const item = resData[index];
          if (!this.isContains(this.rootItems, item[this.propValue])) {
            this.rootItems.push(item);
          }
        }

        this.items = [...this.rootItems];
      }
    },

    /**
     * Hàm kiểm tra phần tử có tồn tại không
     * @param {*} array Mảng phần tử
     * @param {*} id Id của phần tử
     * @returns bool
     * Author: LeDucTiep (30/05/2023)
     */
    isContains(array, id) {
      for (const item of array) {
        if (item[this.propValue] == id) {
          return true;
        }
      }
      return false;
    },

    /**
     * Khi thay đổi props idItem đã chọn thì cập nhật lại inputText và inputValue
     * Author: LeDucTiep (04/05/2023)
     */
    async idOnChange() {
      let flagNotFound = true;

      // Khi prop "id đã chọn" chuyền vào thay đổi thì chọn lại item đó trong danh sách
      if (
        this.rootItems &&
        this.id != null &&
        this.id != "00000000-0000-0000-0000-000000000000"
      ) {
        for (const item of this.rootItems) {
          if (item[this.propValue] == this.id) {
            this.inputText = item[this.propText];
            this.inputValue = item[this.propValue];
            this.emitUpdateValue();
            this.$emit("update:isValid", true);
            flagNotFound = false;
            break;
          }
        }
      }
      // gọi api lấy item mới
      if (
        flagNotFound &&
        this.apiGetById &&
        this.id != null &&
        this.id != "00000000-0000-0000-0000-000000000000"
      ) {
        const response = await this.$msAxios("get", this.apiGetById(this.id));

        if (
          response &&
          response.data &&
          response.data[this.propValue] != this.id
        ) {
          this.rootItems.push(response.data);
          this.items = [...this.rootItems];

          for (const item of this.rootItems) {
            if (item[this.propValue] == this.id) {
              this.inputText = item[this.propText];
              this.inputValue = item[this.propValue];
              this.emitUpdateValue();
              this.$emit("update:isValid", true);
              break;
            }
          }
        }
      }
    },

    /**
     * Khi thay đổi text của input thì lọc lại list item để phù hợp
     * Author: LeDucTiep (04/05/2023)
     */
    inputTextOnChange() {
      if (this.readonly) return;

      this.$emit("update:value", this.inputText);
      // Nếu ô input đã nhập và danh sách không rỗng
      if (this.inputText && this.rootItems) {
        // Lọc các phần tử trong danh sách giống với input đã nhập
        this.items = this.rootItems.filter((item) =>
          item[this.propText]
            .toLowerCase()
            .includes(this.inputText.toLowerCase())
        );
        if (this.items.length == 0) {
          this.items = [...this.rootItems];
        }

        this.isShowList = true;
      } else {
        this.$emit("update:id", null);
      }
      // Check input có hợp lệ không
      this.isInvalidInput = this.isRequired && !this.inputText;
    },

    /**
     * Hàm complete khi un focus
     * Author: LeDucTiep (12/06/2023)
     */
    onBlur() {
      if (this.inputText && this.rootItems) {
        // Lọc các phần tử trong danh sách giống với input đã nhập
        const items = this.rootItems.filter((item) =>
          item[this.propText]
            .toLowerCase()
            .includes(this.inputText.toLowerCase())
        );

        if (items.length == 1) {
          this.selectItemOnClick(items[0]);
        }

        this.isShowList = false;
      }
      // Check input có hợp lệ không
      this.isInvalidInput = this.isRequired && !this.inputText;
    },

    /**
     * Hàm bật tắt danh sách các lựa chọn
     * Author: LeDucTiep (04/05/2023)
     */
    toggleItemsList() {
      if (this.readonly) return;

      this.isShowList = !this.isShowList;
      if (this.rootItems) {
        this.items = [...this.rootItems];
      }
    },

    /**
     * Hàm hoạt động khi nhấn vào lựa chọn
     * @param {*} item Item bị click
     * Author: LeDucTiep (04/05/2023)
     */
    selectItemOnClick(item) {
      // Hiển thị item đã chọn lên ô input
      this.inputText = item[this.propText];
      this.inputValue = item[this.propValue];

      // Tắt danh sách lựa chọn khi đã lựa chọn xong
      this.isShowList = false;
      this.isInvalidInput = false;

      this.emitUpdateValue(item);
    },

    /**
     * Hàm update value lên component cha
     * Author: LeDucTiep (28/05/2023)
     */
    emitUpdateValue(item = null) {
      this.$emit("update:id", this.inputValue);
      this.$emit("update:value", this.inputText);
      if (item) {
        this.$emit("valueOnChange", item);
      }
    },
  },
};

export default mixin;

import MSResource from "../../resource/resource.js";

const mixin = {
  name: "MSInputComboboxMultiSelect",
  props: {
    // Tên thuộc tính chứa text
    propText: String,
    // Tên thuộc tính chứa value
    propValue: String,
    // chứa id mặc định chọn khi khởi tạo
    ids: {
      type: Array,
      default: [],
    },
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
    disabled: {
      type: Boolean,
      default: false,
    },
  },
  emits: ["update:ids", "update:isValid"],
  data() {
    return {
      // Thông báo lỗi
      message: this.errorMessage,
      // Danh sách các phần tử gốc
      itemsRoot: null,
      // Danh sách các phần tử và bị thay đổi khi input thay đổi
      items: null,
      // Ẩn hiện danh sách lựa chọn
      isShowList: false,
      // Text hiển thị trong ô input
      inputText: "",
      // Value của combobox
      inputValue: "",
      // True: kích hoạt trạng thái input lỗi
      // False: trạng thái input không lỗi
      isInvalidInput: false,
      // Đang dùng key để chọn danh sách
      isUsingKey: false,
      // Phần tử chọn bằng key
      indexKeyChoosing: 0,
      // Danh sách đã chọn
      selectedItems: [],
      columnWidthList: {},
    };
  },
  async created() {
    await this.initCombobox();
  },
  beforeUnmount() {
    // Đóng tooltip
    this.$msEmitter.emit("hideTooltip");
  },
  watch: {
    listItems() {
      this.initCombobox();
    },
    ids() {
      this.initCombobox();
    },
    async apiPaging() {
      await this.initCombobox();
    },
  },
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
  methods: {
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
    initCombobox() {
      if (this.listItems) {
        this.itemsRoot = [...this.listItems];
        this.items = [...this.itemsRoot];
      }
      if (this.listItems && this.ids) {
        this.selectedItems = this.listItems.filter((item) =>
          this.ids.includes(item[this.propValue])
        );
      }
      this.reCalculateColumnWidth();
    },
    /**
     * Hàm xóa phần tử đã chọn
     * @param {Object} selectedItem Phần tử đã chọn
     * Author: LeDucTiep (20/06/2023)
     */
    removeSelectedItem(selectedItem) {
      if (this.readonly) return;

      this.selectedItems = this.selectedItems.filter(
        (item) => item[this.propValue] != selectedItem[this.propValue]
      );
      this.removeItem(selectedItem);
    },

    /**
     * Hàm thêm một item
     * @param {Object} selectedItem
     * Author: LeDucTiep (30/05/2023)
     */
    addItem(selectedItem) {
      if (this.readonly) return;

      if (!selectedItem || !selectedItem[this.propValue]) return;

      // Đã có phần tử này
      const selectedId = selectedItem[this.propValue];
      if (this.ids.includes(selectedId)) return;

      // Phần tử này phải tồn tại
      let myList = this.listItems.find(
        (item) => item[this.propValue] == selectedId
      );

      if (myList) {
        this.selectedItems.push(selectedItem);

        this.emitUpdateValue();
      }
    },

    /**
     * Hàm xóa một item trong danh sách
     * @param {*} selectedItem
     * Author: LeDucTiep (30/05/2023)
     */
    removeItem(selectedItem) {
      if (this.readonly) return;

      if (!selectedItem || !selectedItem[this.propValue]) return;

      // Xóa phần tử đã chọn
      this.selectedItems = this.selectedItems.filter(
        (item) => item[this.propValue] != selectedItem[this.propValue]
      );

      this.emitUpdateValue();
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

        // Đánh dấu đang sử dụng phím tắt
        this.isUsingKey = true;

        // Thay đổi phần tử đã chọn
        if (this.isShowList && this.items) {
          // Tăng index
          if (this.indexKeyChoosing == this.items.length - 1)
            this.indexKeyChoosing = 0;
          else this.indexKeyChoosing++;
        } else {
          // Nếu là lần nhấn đầu thì mở list lên
          this.isShowList = true;
        }

        // Cập nhật cho ô input
        this.inputText = this.items[this.indexKeyChoosing][this.propText];
        this.inputValue = this.items[this.indexKeyChoosing][this.propValue];

        this.$emit("update:isValid", true);

        // Hủy bỏ mọi lỗi
        this.isInvalidInput = false;
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

        // Đánh dấu đang sử dụng phím tắt
        this.isUsingKey = true;

        // Thay đổi phần tử đã chọn
        if (this.isShowList && this.items) {
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
    finishedTyping() {
      if (this.readonly) return;

      try {
        // Nếu chưa nhập hoặc danh sách lựa chọn là rỗng
        if (!this.inputText || !this.itemsRoot) return;

        let flagNotFound = true;

        // Tìm kiếm phần tử phù hợp

        for (const item of this.itemsRoot) {
          if (
            item[this.propText]
              .toLowerCase()
              .replaceAll(/\s/g, "")
              .search(this.inputText.toLowerCase().replaceAll(/\s/g, "")) ==
              0 &&
            this.itemsRoot.filter((item) =>
              item[this.propText]
                .toLowerCase()
                .replaceAll(/\s/g, "")
                .includes(this.inputText.toLowerCase().replaceAll(/\s/g, ""))
            ).length == 1
          ) {
            // Nếu khi gõ xong mà chỉ còn 1 phần tử khớp
            // Thì gán luôn cho ô input

            this.addItem(item);
            this.inputText = "";
            this.$emit("update:isValid", true);
            this.isShowList = false;
            flagNotFound = false;
            break;
          }
        }

        // Nếu không nhập đúng thì
        if (flagNotFound) {
          this.isInvalidInput = true;
          this.message = "Không hợp lệ";
        }
      } catch {
        //
      }
    },

    /**
     * Khi thay đổi text của input thì lọc lại list item để phù hợp
     * Author: LeDucTiep (04/05/2023)
     */
    inputTextOnChange() {
      if (this.readonly) return;

      // Nếu ô input đã nhập và danh sách không rỗng
      if (this.inputText && this.itemsRoot) {
        // Lọc các phần tử trong danh sách giống với input đã nhập
        this.items = this.itemsRoot.filter((item) =>
          item[this.propText]
            .toLowerCase()
            .includes(this.inputText.toLowerCase())
        );
        if (this.items.length == 0) {
          this.items = [...this.itemsRoot];
        }

        this.isShowList = true;
      }
      // Check input có hợp lệ không
      this.isInvalidInput = this.isRequired && !this.inputText;
    },

    /**
     * Hàm complete khi un focus
     * Author: LeDucTiep (12/06/2023)
     */
    onBlur() {
      if (this.inputText && this.itemsRoot) {
        // Lọc các phần tử trong danh sách giống với input đã nhập
        const items = this.itemsRoot.filter((item) =>
          item[this.propText]
            .toLowerCase()
            .includes(this.inputText.toLowerCase())
        );

        if (items.length == 1) {
          this.addItem(items[0]);
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
      if (this.itemsRoot) this.items = [...this.itemsRoot];
    },

    /**
     * Hàm hoạt động khi nhấn vào lựa chọn
     * @param {*} item Item bị click
     * Author: LeDucTiep (04/05/2023)
     */
    selectItemOnClick(item) {
      const selectedId = item[this.propValue];
      if (this.ids.includes(selectedId)) {
        this.removeItem(item);
      } else {
        this.addItem(item);
      }

      // Tắt danh sách lựa chọn khi đã lựa chọn xong

      this.isInvalidInput = false;
    },

    /**
     * Hàm update value lên component cha
     * Author: LeDucTiep (28/05/2023)
     */
    emitUpdateValue() {
      if (this.readonly) return;

      const selectedIds = this.selectedItems.map(
        (item) => item[this.propValue]
      );

      this.$emit("update:ids", selectedIds);
    },
  },
};

export default mixin;

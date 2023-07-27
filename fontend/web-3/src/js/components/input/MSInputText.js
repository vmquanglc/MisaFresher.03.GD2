import MSResource from "../../resource/resource.js";

const mixin = {
  name: "MSInputText",
  props: {
    // Label của input
    title: {
      type: String,
      default: "",
    },
    textAlign: {
      type: String,
      default: "left",
    },
    // value của input
    value: {
      type: [String, Number],
      default: "",
    },

    // Kiểu của input
    inputType: {
      type: String,
      default: "text",
    },

    // placehoder của input
    placeholder: String,

    // Tooltip khi hover label
    tooltip: {
      type: String,
      default: "",
    },

    // là trường bắt buộc
    isRequired: {
      type: Boolean,
      default: false,
    },
    rows: Number,
    // Thông tin lỗi
    errorMessage: {
      type: String,
      default: MSResource.VN.Error.Required,
    },

    // Đang submiting -> đang trong trạng thái validate các trường dữ liệu
    isValidating: {
      type: Boolean,
      default: false,
    },

    // Là ô input lỗi
    isValid: {
      type: Boolean,
      default: true,
    },

    // Là ô input đầu tiên trong form
    isFirstInput: {
      type: Boolean,
      default: false,
    },

    // Hàm thực hiện khi người dùng nhập xong
    onDoneTyping: {
      type: Function,
      default: () => true,
    },

    readonly: {
      type: Boolean,
      default: false,
    },
  },

  emits: ["update:value", "update:isValid"],

  data() {
    return {
      // Giá trị người dùng nhập vào
      inputValue: this.value,
      // Bản thân component nhận thấy nó bị lỗi
      isInvalidInput: false,
      // Lỗi đặc thù từ bên ngoài
      isErroredFromOutSide: false,
      // Thông báo lỗi từ bên ngoài truyền vào
      messageFromOutSide: "",
    };
  },

  watch: {
    /**
     * prop value của input thay đổi thì cập nhật vào data inputValue
     * Author: LeDucTiep (04/05/2023)
     */
    value() {
      this.inputValue = this.value;
    },

    /**
     * Khi prop isValidating cập nhật thì xét xem inputText có hợp lệ không
     * Author: LeDucTiep (04/05/2023)
     */
    isValidating() {
      this.inputValueOnUpdate();
    },

    /**
     * inputValue thay đổi thì xét xem input nhập vào có hợp lệ ko
     * Author: LeDucTiep (04/05/2023)
     */
    inputValue() {
      this.inputValueOnUpdate();
    },

    /**
     * Hàm phát hiện ô input bị lỗi và bắn lỗi ra ngoài
     * Author: LeDucTiep (10/06/2023)
     */
    isInvalidInput() {
      this.$emit("update:isValid", !this.isInvalidInput);
    },
  },

  created() {
    // Nhận sự kiện focus ô input đầu tiên trong form
    this.$msEmitter.on("focusFirstInput", this.focusInput);
    // Kiểm tra input có đúng không
    // Phòng trường hợp chưa nhập đã submit form
    this.inputValueOnUpdate();
  },

  async mounted() {
    await this.doneTyping();
  },

  beforeUnmount() {
    // Hủy lắng nghe các sự kiện
    this.$msEmitter.off("focusFirstInput");
    // Ẩn tooltip
    this.$msEmitter.emit("hideTooltip");
  },

  methods: {
    /**
     * Hàm format tiền
     * @param {String} value Giá trị cần format
     * @returns Giá trị sau khi format
     * Author: LeDucTiep (28/05/2023)
     */
    formatMoney(value) {
      value = value.toString().replace(/\./g, "");
      return value.replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.");
    },

    /**
     * Hàm kiểm tra xem có phải kí tự số hay không
     * @param {*} evt Sự kiện
     * Author: LeDucTiep (28/05/2023)
     */
    checkValidInput(evt) {
      evt = evt ? evt : window.event;
      var charCode = evt.which ? evt.which : evt.keyCode;

      // Nếu là kiểu số thì chỉ cho phép nhập số
      if (
        ["number", "money"].includes(this.inputType) &&
        charCode > 31 &&
        charCode != 45 &&
        (charCode < 48 || charCode > 57)
      ) {
        evt.preventDefault();
      }
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
    },

    /**
     * Hàm focus vào ô input
     * Author: LeDucTiep (27/05/2023)
     */
    focusInput() {
      if (this.isFirstInput) this.$refs.myFirstInput.focus();
    },

    /**
     * Hàm focus vào input
     * Author: LeDucTiep (27/05/2023
     */
    forceFocus() {
      this.$refs.myFirstInput.focus();
    },

    /**
     * Xác nhận input có đúng ko
     * Author: LeDucTiep (04/05/2023)
     */
    inputValueOnUpdate() {
      this.isInvalidInput = false;

      // check để trống
      if (!this.inputValue && this.isRequired) {
        this.isInvalidInput = true;
        return;
      }

      if (this.inputValue && ["money"].includes(this.inputType)) {
        while (this.inputValue && this.inputValue.toString()[0] == "0") {
          this.inputValue = this.inputValue.toString().substring(1);
        }
      }

      if (this.inputValue && this.inputType == "money") {
        this.inputValue = this.formatMoney(this.inputValue);
      }

      // Check email
      if (this.inputValue && this.inputType == "email") {
        const regex = /[^\s@]+@[^\s@]+\.[^\s@]+/;
        const isEmail = this.inputValue.match(regex);
        if (!isEmail) {
          this.isInvalidInput = true;
          return;
        }
      }
      this.doneTyping();
    },

    /**
     * Hàm thực thi khi người dùng nhập xong dữ liệu
     * Author: LeDucTiep (17/05/2023)
     */
    async doneTyping() {
      if (this.inputValue) {
        const res = await this.onDoneTyping();
        // Nếu đã xảy ra lỗi thì hiển thị
        this.isErroredFromOutSide = res?.isErrored;
        this.messageFromOutSide = res?.message;
        this.$emit("update:isValid", !this.isErroredFromOutSide);
      }
    },
  },
};

export default mixin;

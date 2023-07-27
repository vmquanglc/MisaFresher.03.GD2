const mixin = {
  name: "MSCombobox",
  props: {
    value: [String, Number],
    items: Array,
    inputType: {
      type: String,
      default: "text",
    },
    isSmall: {
      type: Boolean,
      default: false,
    },
    max: {
      type: Number,
      default: 900,
    },
    min: {
      typeof: Number,
      default: 10,
    },
  },
  emits: ["update:value"],
  data() {
    return {
      isShowList: false,
      inputText: "",
    };
  },
  created() {
    this.inputText = this.value;
  },
  methods: {
    /**
     * Hàm kiểm tra xem có phải kí tự số hay không
     * @param {*} evt Sự kiện
     * Author: LeDucTiep (28/05/2023)
     */
    checkValidInput: function (evt) {
      evt = evt ? evt : window.event;
      var charCode = evt.which ? evt.which : evt.keyCode;
      // Nếu là kiểu số thì chỉ cho phép nhập số
      if (
        this.inputType == "number" &&
        charCode > 31 &&
        (charCode < 48 || charCode > 57)
      ) {
        evt.preventDefault();
      }
    },

    /**
     * Hàm thực hiện khi người dùng nhập xong
     * Author: LeDucTiep (28/05/2023)
     */
    inputOnChange() {
      if (this.inputType == "number") {
        if (this.inputText > this.max) this.inputText = this.max;
        if (this.inputText < this.min) this.inputText = this.min;
      }

      this.isShowList = false;
      this.$emit("update:value", this.inputText);
    },

    /**
     * Hàm bật tắt danh sách các lựa chọn
     * Author: LeDucTiep (05/05/2023)
     */
    toggleItemsList() {
      this.isShowList = !this.isShowList;
    },

    /**
     * Hàm hoạt động khi nhấn vào lựa chọn
     * @param {*} item Item bị click
     * Author: LeDucTiep (04/05/2023)
     */
    selectItemOnClick(item) {
      this.inputText = item;
      this.isShowList = false;
      this.$emit("update:value", item);
    },
  },
};

export default mixin;

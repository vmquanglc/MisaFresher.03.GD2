const mixin = {
  name: "MSInputSearch",
  props: {
    // Label của input
    title: String,
    // Giá trị của input
    value: {
      type: String,
      default: "",
    },
    // Placeholder của input
    placeholder: String,
    // Nội dung của tooltip
    tooltip: String,
  },
  data() {
    return {
      // Biến để hứng giá trị người dùng nhập vào
      inputValue: this.value,
    };
  },
  beforeUnmount() {
    // Ẩn tooltip
    this.$msEmitter.emit("hideTooltip");
  },
  watch: {
    value() {
      // Thay đổi giá trị nội bộ khi giá trị truyền vào thay đổi
      this.inputValue = this.value;
    },
  },
  methods: {
    /**
     * Hàm submit input
     * Author: LeDucTiep (07/06/2023)
     */
    inputSearchOnSubmit() {
      // Kích hoạt tìm kiếm lên component cha
      this.$emit("inputSearchOnSubmit");
    },
  },
};

export default mixin;

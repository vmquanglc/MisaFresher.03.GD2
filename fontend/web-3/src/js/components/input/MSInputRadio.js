const mixin = {
  name: "MSInputRadio",
  props: {
    // Nội dung label
    title: String,
    // Nội dung tooltip
    tooltip: {
      type: String,
      default: "",
    },
    // Danh sách item của combobox
    items: {
      type: Object,
    },
    // Id của item đã chọn
    id: {
      type: [String, Number],
      default: 0,
    },
    readonly: {
      type: Boolean,
      default: false,
    },
  },
  emits: ["update:id"],
  beforeUnmount() {
    // Ẩn tooltip
    this.$msEmitter.emit("hideTooltip");
  },
  methods: {
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
     * Hàm gán value khi nhấn vào input radio
     * @param {*} item Item bị click
     * Author: LeDucTiep (04/05/2023)
     */
    itemOnSelect(item) {
      if (this.readonly) return;
      // Cập nhật để component cha biết rằng id và value đã được chọn là gì
      this.$emit("update:id", item.id);
    },
  },
};

export default mixin;

const mixin = {
  name: "MSCheckBox",
  props: {
    isTableHead: {
      type: Boolean,
      default: false,
    },
    value: {
      type: Boolean,
      default: false,
    },
    title: {
      type: String,
    },
    readonly: {
      type: Boolean,
      default: false,
    },
  },
  emits: ["onChangeValue", "update:value"],
  data() {
    return {
      isChecked: this.value,
    };
  },
  watch: {
    value() {
      this.isChecked = this.value;
    },
  },
  methods: {
    /**
     * Hàm thực hiện khi input thay đổi
     * Author: LeDucTiep (30/05/2023)
     */
    onInput() {
      if (this.readonly) return;

      this.isChecked = !this.isChecked;

      this.$emit("update:value", this.isChecked);
      this.$emit("onChangeValue", this.isChecked);
    },
  },
};

export default mixin;

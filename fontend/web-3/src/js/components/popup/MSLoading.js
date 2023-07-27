const mixin = {
  name: "MSLoading",
  props: {
    isSmall: {
      type: Boolean,
      default: false,
    },
  },
  computed: {
    isBig() {
      return !this.isSmall;
    },
  },
};

export default mixin;

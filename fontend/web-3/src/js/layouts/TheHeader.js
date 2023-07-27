const mixin = {
  name: "TheHeader",
  methods: {
    /**
     * Hàm ẩn bớt
     * Author: LeDucTiep (07/06/2023)
     */
    showLessOnClick() {
      // Bắn sự kiện
      this.$msEmitter.emit("showLessOnClick");
    },
  },
};

export default mixin;

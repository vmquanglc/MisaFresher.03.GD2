const mixin = {
  name: "MSTooltip",
  data() {
    return {
      // Tọa độ của tooltip
      left: 0,
      top: 0,
      text: "",
      isOpen: false,
      // Tọa độ đuôi thò ra của tooltip
      tailTooltipXLocation: null,
      tailTooltipYLocation: null,
    };
  },
  created() {
    // Lắng nghe sự kiện
    this.$msEmitter.on("showTooltip", this.showTooltip);
    this.$msEmitter.on("hideTooltip", this.closeTooltip);
  },
  beforeUnmount() {
    // Hủy lắng nghe sự kiện
    this.$msEmitter.off("showTooltip");
    this.$msEmitter.off("hideTooltip");
  },
  methods: {
    /**
     * Hàm hiện tooltip
     * Author: LeDucTiep (13/06/2023)
     */
    showTooltip(event, message) {
      // Chủ của menu được kích hoạt
      this.isOpen = true;
      this.text = message;
      if (this.$refs.tiepTooltip) {
        // Tính toán tọa độ hiển thị tooltip
        let toolTipWidth = this.$refs.tiepTooltip.offsetWidth;
        let toolTipHeight = this.$refs.tiepTooltip.offsetHeight;

        let right = event.clientX + toolTipWidth;
        let left = event.clientX - toolTipWidth;

        this.tailTooltipXLocation = null;
        if (right > window.innerWidth) {
          this.left = left;
          this.tailTooltipXLocation = toolTipWidth - 14 + "px";
        }
        if (!this.tailTooltipXLocation && left < 0) {
          this.left = event.clientX;
          this.tailTooltipXLocation = 14 + "px";
        }
        if (!this.tailTooltipXLocation) {
          this.left = event.clientX - toolTipWidth / 2;
          this.tailTooltipXLocation = toolTipWidth / 2 + "px";
        }

        let bottom = event.clientY + 20;
        let top = event.clientY - toolTipHeight;

        this.tailTooltipYLocation = null;
        if (bottom + toolTipHeight + 20 > window.innerHeight) {
          this.top = top - 20;
          this.tailTooltipYLocation = toolTipHeight - 3.5 + "px";
        } else {
          this.top = bottom;
          this.tailTooltipYLocation = -3.5 + "px";
        }
      }
    },

    /**
     * Hàm ẩn tooltip
     * Author: LeDucTiep (13/06/2023)
     */
    closeTooltip() {
      this.isOpen = false;
    },
  },
};

export default mixin;

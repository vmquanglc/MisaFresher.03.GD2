const mixin = {
  name: "MSNoticeMessage",
  props: {
    notice: Object,
  },
  data() {
    return {
      timeOut: null,
    };
  },
  computed: {
    /**
     * Computed tính ra icon của thông báo
     * Author: LeDucTiep (11/06/2023)
     */
    iconClass() {
      switch (this.notice.type) {
        case this.$msEnum.NoticeType.Success:
          return "icon-notice-success";

        case this.$msEnum.NoticeType.Error:
          return "icon-notice-error";

        case this.$msEnum.NoticeType.Warning:
          return "icon-notice-warning";

        default:
          return "icon-notice-information";
      }
    },

    /**
     * Computed tính ra loại của thông báo
     * Author: LeDucTiep (11/06/2023)
     */
    noticeType() {
      switch (this.notice.type) {
        case this.$msEnum.NoticeType.Success:
          return this.$t("NoticeType.Success");

        case this.$msEnum.NoticeType.Error:
          return this.$t("NoticeType.Error");

        case this.$msEnum.NoticeType.Warning:
          return this.$t("NoticeType.Warning");

        default:
          return this.$t("NoticeType.Information");
      }
    },

    /**
     * Computed tính ra màu sắc của thông báo
     * Author: LeDucTiep (11/06/2023)
     */
    colorClass() {
      switch (this.notice.type) {
        case this.$msEnum.NoticeType.Success:
          return "color-green";

        case this.$msEnum.NoticeType.Error:
          return "color-red";

        case this.$msEnum.NoticeType.Warning:
          return "color-orange";

        default:
          return "color-blue";
      }
    },
  },
  created() {
    // Ẩn thông báo sau 5 giây
    this.timeOut = setTimeout(() => {
      this.$msEmitter.emit("removeNotice", this.notice);
    }, 5000);
  },
  methods: {
    /**
     * Hàm đóng thông báo
     * Author: LeDucTiep (11/06/2023)
     */
    closeOnClick() {
      this.$msEmitter.emit("removeNotice", this.notice);
    },
  },
  beforeUnmount() {
    clearTimeout(this.timeOut);
  },
};

export default mixin;

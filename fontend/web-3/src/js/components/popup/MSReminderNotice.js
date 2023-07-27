const mixin = {
  name: "MSReminderNotice",
  data() {
    return {
      messageList: [],
    };
  },
  created() {
    this.$msEmitter.on("addNotice", this.addNotice);
    this.$msEmitter.on("removeNotice", this.removeNotice);
  },
  beforeUnmount() {
    this.$msEmitter.off("addNotice");
    this.$msEmitter.off("removeNotice");
  },
  computed: {
    /**
     * Tọa độ hiển thị của message
     * Author: LeDucTiep (11/06/2023)
     */
    positionLeft() {
      return window.innerWidth / 2;
    },
  },
  methods: {
    /**
     * Hàm thêm thông báo
     * @param {Object} message Thông báo cần thêm
     * Author: LeDucTiep (11/06/2023)
     */
    addNotice(message) {
      this.messageList.push({
        ...message,
        id: (Math.random().toString(36) + Date.now().toString(36)).substring(2),
      });
    },

    /**
     * Hàm xóa một thông báo
     * @param {Object} messageToRemove Thông báo muốn xóa
     * Author: LeDucTiep (11/06/2023)
     */
    removeNotice(messageToRemove) {
      this.messageList = this.messageList.filter((message) => {
        return message.id != messageToRemove.id;
      });
    },
  },
};

export default mixin;

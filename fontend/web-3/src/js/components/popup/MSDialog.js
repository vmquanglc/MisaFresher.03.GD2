import MSResource from "../../resource/resource.js";

const mixin = {
  name: "MSDialog",
  props: {
    // Danh sách thông báo
    items: Array,
    // Tiêu đề của dialog
    title: {
      type: String,
      default: MSResource.VN.Dialog.DefaultTitle,
    },
    // Là dialog cảnh báo
    isWarning: {
      type: Boolean,
      default: false,
    },
    // Nội dung nút Yes
    contentYes: {
      type: String,
      default: MSResource.VN.Button.Accept,
    },
    // Nội dung nút No
    contentNo: {
      type: String,
    },
    // Nội dung nút Cancel
    contentCancel: {
      type: String,
    },

    // Kiểu
    dialogType: {
      type: Number,
    },
  },
  data() {
    return {
      // Sự kiện của document trước khi Mounted
      documentEventBeforeMounted: null,
    };
  },
  mounted() {
    // Thêm sự kiện phím tắt
    document.onkeydown = this.keyEvent;
  },
  beforeUnmount() {
    // Xóa sự kiện
    document.onkeydown = null;
    this.$msEmitter.emit("hideTooltip");
  },
  beforeMount() {
    this.$nextTick(() => {
      this.focusButtonYes();
    });
  },
  methods: {
    /**
     * Hàm focus button tiếp theo
     * @param {*} elem Phần từ phát ra sự kiện
     * Author: LeDucTiep (28/05/2023)
     */
    nextButton(elem) {
      if (!elem.form) return;
      const currentIndex = Array.from(elem.form.elements).indexOf(elem);
      elem.form.elements
        .item(
          currentIndex < elem.form.elements.length - 1 ? currentIndex + 1 : 0
        )
        .focus();
    },

    /**
     * Hàm focus button trước đó
     * @param {*} elem Phần từ phát ra sự kiện
     * Author: LeDucTiep (28/05/2023)
     */
    previousButton(elem) {
      if (!elem.form) return;
      const currentIndex = Array.from(elem.form.elements).indexOf(elem);
      elem.form.elements
        .item(
          currentIndex > 0 ? currentIndex - 1 : elem.form.elements.length - 1
        )
        .focus();
    },

    /**
     * Xoay vòng tabindex
     * Author: LeDucTiep (06/06/2023)
     */
    focusFirstButton() {
      this.focusButtonYes();
    },

    /**
     * Hàm focus vào button cuối cùng
     * Author: LeDucTiep (08/06/2023)
     */
    focusLastButton() {
      if (this.$refs.buttonCancel) this.$refs.buttonCancel.focus();
      else if (this.$refs.buttonNo) this.$refs.buttonNo.focus();
      else this.$refs.buttonYes.focus();
    },

    /**
     * Hàm sử lý sự kiện phím tắt
     * Author: LeDucTiep (28/05/2023)
     */
    keyEvent(event) {
      const isESC = event.keyCode == 27;
      const isEnter = event.key === "Enter";

      if (isESC) {
        event.preventDefault();
        // Chọn cancel
        this.hide(this.$msEnum.DialogAnswer.Cancel);
      } else if (isEnter) {
        event.preventDefault();
        // Chọn yes
        this.hide(this.$msEnum.DialogAnswer.Yes);
      }
    },

    /**
     * Focus nút đồng ý khi hiện dialog
     * Author: LeDucTiep (27/05/2023)
     */
    focusButtonYes() {
      if (this.$refs.buttonYes) this.$refs.buttonYes.focus();
    },

    /**
     * Ẩn dialog
     * @param {*} answer Trả lời của người dùng
     * Author: LeDucTiep (04/05/2023)
     */
    hide(answer) {
      this.$msEmitter.emit("hideDialog", answer);
    },

    /**
     * Ẩn dialog và thực hiện yêu cầu
     * Author: LeDucTiep (04/05/2023)
     */
    yesOnClick() {
      this.hide(this.$msEnum.DialogAnswer.Yes);
    },

    /**
     * Ẩn dialog và từ chối yêu cầu
     * Author: LeDucTiep (04/05/2023)
     */
    noOnClick() {
      this.hide(this.$msEnum.DialogAnswer.No);
    },

    /**
     * Ẩn dialog
     * Author: LeDucTiep (06/06/2023)
     */
    cancelOnClick() {
      this.hide(this.$msEnum.DialogAnswer.Cancel);
    },
  },
};

export default mixin;

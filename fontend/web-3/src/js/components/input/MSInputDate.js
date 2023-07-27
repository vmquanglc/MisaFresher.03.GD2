import MSResource from "../../resource/resource.js";

const mixin = {
  name: "MSInputDate",
  props: {
    // Label của input
    title: {
      type: String,
      default: "",
    },
    // Tooltip khi hover lên label
    tooltip: {
      type: String,
      default: "",
    },
    // Value mặc định của input
    value: {
      type: String,
      default: "",
    },
    // là bắt buộc
    isRequired: {
      type: Boolean,
      default: false,
    },
    // Thông báo lỗi
    errorMessage: {
      type: String,
      default: MSResource.VN.Error.IncorrectFormat,
    },
    // true: Hiện thông báo lỗi nếu input sai định dạng
    isValidating: {
      type: Boolean,
      default: false,
    },
    // Là ô input lỗi
    isValid: {
      type: Boolean,
      default: true,
    },
    // Hàm điều kiện hợp lệ của ngày tháng
    conditional: {
      type: Function,
      default: () => true,
    },
    readonly: {
      type: Boolean,
      default: false,
    },
  },
  emits: ["update:value", "update:isValid"],
  beforeUnmount() {
    this.$msEmitter.emit("hideTooltip");
  },
  data() {
    return {
      inputValue: "",
      isInvalidInput: false,
      day: 0,
      month: 0,
      year: 0,
      numberDay: 0,
      currentDay: null,
      startColumn: 0,
      isShowingYearPicker: false,
      isShowingMonthPicker: false,
      isShowingDatePicker: false,
      // Tọa độ của popup datepicker theo viewport
      left: 0,
      top: 0,
    };
  },
  created() {
    this.inputValueOnUpdate();
  },
  watch: {
    /**
     * Khi prop value truyền vào bị thay đổi thì cập nhật lại ngày tháng năm theo value truyền vào
     * Author: LeDucTiep (04/05/2023)
     */
    value() {
      // lần đầu khởi tạo
      if (this.value && !this.inputValue) {
        this.inputValue = this.value;
        this.currentDay = this.$msDateFormater.stringToDate(this.value);
      }
    },

    /**
     * Khi prop isValidating cập nhật thì xét xem inputText có hợp lệ không
     * Author: LeDucTiep (04/05/2023)
     */
    isValidating() {
      this.inputValueOnUpdate();
    },

    /**
     * Khi inputValue thay đổi thì cập nhật lại isInvalidInput, currentDay
     * Author: LeDucTiep (04/05/2023)
     */
    inputValue() {
      this.inputValueOnUpdate();
    },

    /**
     * Khi currentDay thay đổi thì cập nhật lại Day, Month, Year, số ngày trong tháng, inputValue
     * Author: LeDucTiep (04/05/2023)
     */
    currentDay() {
      if (this.currentDay) {
        this.getDay();
        this.getMonth();
        this.getYear();
        this.getDaysInCurrentMonth();
        this.getStartColumn();
        this.setInputValue();
      }
    },

    /**
     * Hàm phát hiện ô input bị lỗi và bắn lỗi ra ngoài
     * Author: LeDucTiep (10/06/2023)
     */
    isInvalidInput() {
      this.$emit("update:isValid", !this.isInvalidInput);
    },
  },
  methods: {
    /**
     * Hàm kiểm tra xem có phải kí tự số hoặc /
     * @param {*} evt Sự kiện
     * Author: LeDucTiep (28/05/2023)
     */
    checkValidInput(evt) {
      evt = evt ? evt : window.event;

      var charCode = evt.which ? evt.which : evt.keyCode;
      // Cho phép nhập kí tự /
      if (charCode == 47) return;

      // Nếu không phải số thì loại bỏ event
      if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        evt.preventDefault();
      }
    },

    /**
     * Hàm ẩn khi nhấn ra bên ngoài
     * Author: LeDucTiep (30/05/2023)
     */
    pickerClickOutSide() {
      this.isShowingDatePicker = false;
    },

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
     * Cập nhật lại isInvalidInput, currentDay khi inputValue thay đổi
     * Author: LeDucTiep (05/02/2023)
     */
    inputValueOnUpdate() {
      if (this.readonly) return;

      const isCorect = this.conditional();
      if (!isCorect) {
        this.isInvalidInput = true;
        return;
      }
      if (this.inputValue && this.inputValue.length <= 10) {
        const pattern = /\d{1,2}\/\d{1,2}\/\d{4}/;
        if (pattern.test(this.inputValue)) {
          const dt = this.inputValue.split("/");

          const dateString = dt.slice(0, 3).reverse().join("-");

          const datetime = new Date(dateString);

          if (datetime == "Invalid Date") {
            this.isInvalidInput = true;
            return false;
          }

          this.currentDay = new Date(dateString);

          this.isInvalidInput = false;
        } else {
          this.isInvalidInput = true;
        }
      } else {
        this.isInvalidInput = this.isRequired || this.inputValue.length > 10;
        this.$emit("update:value", null);
      }
    },

    /**
     * Lấy vị trí nhấn chuột và hiển thị datePicker
     * @param {*} event Event click
     * Author: LeDucTiep (04/05/2023)
     */
    toggleDatePicker(event) {
      if (this.readonly) {
        this.isShowingDatePicker = false;
        return;
      }

      if (!this.currentDay) {
        this.getToDay();
      }
      const datePickerWidth = 320;
      const datePickerHeight = 350;
      this.isShowingDatePicker = !this.isShowingDatePicker;

      if (window.innerWidth - event.clientX < datePickerWidth)
        this.left = event.clientX - datePickerWidth;
      else if (event.clientX < datePickerWidth) this.left = event.clientX;
      else this.left = event.clientX - datePickerWidth / 2;

      if (window.innerHeight - event.clientY < datePickerHeight)
        this.top = event.clientY - 24 - datePickerHeight;
      else this.top = event.clientY + 24;
    },

    /**
     * lấy ngày hiện tại
     * Author: LeDucTiep (29/04/2023)
     */
    getDay() {
      this.day = this.currentDay.getDate();
    },

    /**
     * Lấy tháng hiện tại
     * Author: LeDucTiep (29/04/2023)
     */
    getMonth() {
      this.month = this.currentDay.getMonth() + 1;
    },

    /**
     * Lấy năm hiện tại
     * Author: LeDucTiep (29/04/2023)
     */
    getYear() {
      this.year = this.currentDay.getFullYear();
    },

    /**
     * Đật lại ngày
     * @param {*} numberOfDay Ngày muốn đặt lại
     * Author: LeDucTiep (29/04/2023)
     */
    setDay(numberOfDay) {
      this.currentDay = new Date(this.year, this.month - 1, numberOfDay);
      this.isShowingDatePicker = false;
    },

    /**
     * Đặt lại tháng
     * @param {*} numberOfMonth Tháng muốn đặt lại
     * Author: LeDucTiep (29/04/2023)
     */
    setMonth(numberOfMonth) {
      this.currentDay = new Date(this.year, numberOfMonth - 1, this.day);
    },

    /**
     * Đặt lại năm
     * @param {*} numberOfYear Năm muốn đặt lại
     * Author: LeDucTiep (29/04/2023)
     */
    setYear(numberOfYear) {
      this.currentDay = new Date(numberOfYear, this.month - 1, this.day);
    },

    /**
     * Lấy số ngày trong tráng hiện tại
     * Author: LeDucTiep (29/04/2023)
     */
    getDaysInCurrentMonth() {
      this.numberDay = new Date(
        this.currentDay.getFullYear(),
        this.currentDay.getMonth() + 1,
        0
      ).getDate();
    },

    /**
     * Tính toán ra vị trí hiển thị đầu tiên của ngày trong danh sách ngày
     * Author: LeDucTiep (29/04/2023)
     */
    getStartColumn() {
      let firstWeekDay = new Date(this.year, this.month - 1, 1).getDay();
      this.startColumn = [6, 0, 1, 2, 3, 4, 5][firstWeekDay] + 1;
    },

    /**
     * Lấy ngày tháng hiện tại
     * Author: LeDucTiep (29/04/2023)
     */
    getToDay() {
      if (this.readonly) return;

      this.currentDay = new Date();
      this.isShowingDatePicker = false;
    },

    /**
     * Gán ngày tháng được lưu trong componet cho input
     * Author: LeDucTiep (29/04/2023)
     */
    setInputValue() {
      if (this.readonly) return;

      // Format dd/mm/yy
      this.inputValue = `${this.day.toString().padStart(2, "0")}/${this.month
        .toString()
        .padStart(2, "0")}/${this.year.toString().padStart(4, "0")}`;
      this.$emit("update:value", this.inputValue);
    },

    /**
     * Load lịch cho tháng tiếp theo
     * Author: LeDucTiep (29/04/2023)
     */
    getLastMonthOnClick() {
      if (this.month == 1) {
        this.month = 12;
      } else {
        this.month -= 1;
      }
      this.currentDay = new Date(this.year, this.month - 1, this.day);
    },

    /**
     * Load lịch cho tháng tới
     * Author: LeDucTiep (29/04/2023)
     */
    getNextMonthOnLick() {
      if (this.month == 12) {
        this.month = 1;
      } else {
        this.month += 1;
      }
      this.currentDay = new Date(this.year, this.month - 1, this.day);
    },
  },
};

export default mixin;

const mixin = {
  name: "TheNav",
  props: {
    isShowLessNav: {
      type: Boolean,
      default: false,
    },
  },
  watch: {
    isShowLessNav() {
      this.isSmall = this.isShowLessNav;
    },
  },
  data() {
    return {
      // align các phần tử trong nav
      alignment: "left",
      // Độ rộng của nav
      navWidth: 200,
      // Là nav nhỏ
      isSmall: false,

      navigators: [
        {
          path: "/Overview",
          text: this.$t("Nav.Overview"),
        },
        {
          path: "/cash/business-process",
          text: this.$t("Nav.Cash"),
        },
        {
          path: "/deposits",
          text: this.$t("Nav.Deposits"),
        },
        {
          path: "/purchase",
          text: this.$t("Nav.Purchase"),
        },
        {
          path: "/sell",
          text: this.$t("Nav.Sell"),
        },
        {
          path: "/bill-management",
          text: this.$t("Nav.BillManagement"),
        },
        {
          path: "/warehouse",
          text: this.$t("Nav.Warehouse"),
        },
        {
          path: "/service-tools",
          text: this.$t("Nav.ServiceTools"),
        },
        {
          path: "/fixed-assets",
          text: this.$t("Nav.FixedAssets"),
        },
        {
          path: "/price",
          text: this.$t("Nav.Price"),
        },
        {
          path: "/synthetic",
          text: this.$t("Nav.Synthetic"),
        },
        {
          path: "/report",
          text: this.$t("Nav.Report"),
        },
        {
          path: "/financial-analysis",
          text: this.$t("Nav.FinancialAnalysis"),
        },
        {
          path: "/account",
          text: this.$t("Nav.Account"),
        },
        {
          path: "/category",
          text: this.$t("Nav.Category"),
        },
      ],
    };
  },
  computed: {},
  methods: {
    isActive(item) {
      let isActive = false;

      if (item.path == "/category") {
        isActive = ["/customer", "/employee"].includes(this.$route.path);
      }

      if (item.path == "/cash/business-process") {
        isActive = ["/cash/business-process", "/cash/receipt"].includes(
          this.$route.path
        );
      }

      return item.path == this.$route.path || isActive;
    },
    /**
     * Làm lớn nav
     * Author: LeDucTiep (03/02/2023)
     */
    setBigger() {
      this.isSmall = false;
      this.alignment = "left";
      this.navWidth = 200;
      this.$emit("update:isShowLessNav", false);
    },
    /**
     * Làm nhỏ nav
     * Author: LeDucTiep (03/02/2023)
     */
    setSmaller() {
      this.isSmall = true;
      this.alignment = "center";
      this.navWidth = 56;
      this.$emit("update:isShowLessNav", true);
    },
  },
  created() {
    // Hành đông khi nhấn nút thu nhỏ
    this.$msEmitter.on("showLessOnClick", () => {
      if (this.isSmall == true) {
        this.setBigger();
      } else {
        this.setSmaller();
      }
    });
  },
  beforeUnmount() {
    this.$msEmitter.off("showLessOnClick");
  },
};

export default mixin;

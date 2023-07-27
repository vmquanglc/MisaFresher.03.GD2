<template>
  <div class="container" :style="{ width: windowWidth }">
    <TheNav v-model:isShowLessNav="isShowLessNav"></TheNav>
    <div
      class="head-and-content"
      :style="{ width: headAndContentWidth + 'px' }"
    >
      <TheHeader></TheHeader>
      <TheContent></TheContent>
    </div>
    <MSTooltip></MSTooltip>
    <MSReminderNotice></MSReminderNotice>
  </div>
</template>

<script>
import TheNav from "./layouts/TheNav.vue";
import TheContent from "./layouts/TheContent.vue";
import TheHeader from "./layouts/TheHeader.vue";

export default {
  name: "App",
  components: {
    TheNav,
    TheContent,
    TheHeader,
  },
  data() {
    return {
      // Dùng để thay đổi trạng thái của nav
      isShowLessNav: false,
      // Dùng để gán độ rộng màn hình cho CSS
      windowWidth: window.innerWidth + "px",
      // Khoảng trống bên phải của nav
      headAndContentWidth: 0,
    };
  },
  watch: {
    /**
     * Tính toán lại kích trước của content khi kích thước nav thay đổi
     * Author: LeDucTiep (04/05/2023)
     */
    isShowLessNav() {
      this.reCalculateContentWidth();
    },
  },

  methods: {
    /**
     * Tính toán lại kích thước content khi thay đổi kích thước màn hình
     * Author: LeDucTiep (04/05/2023)
     */
    onResize() {
      this.windowWidth = window.innerWidth + "px";
      this.reCalculateContentWidth();
    },

    /**
     * Tính toán lại kích thước của content
     * Author: LeDucTiep (04/05/2023)
     */
    reCalculateContentWidth() {
      this.headAndContentWidth =
        this.isShowLessNav || window.innerWidth < 870
          ? window.innerWidth - 56
          : window.innerWidth - 200;
    },
  },

  created() {
    // Responsive
    this.reCalculateContentWidth();
  },

  mounted() {
    this.$nextTick(() => {
      window.addEventListener("resize", this.onResize);
    });
  },

  beforeUnmount() {
    window.removeEventListener("resize", this.onResize);
  },
};
</script>

<style src="./style/base/main.scss" lang="scss">
</style>

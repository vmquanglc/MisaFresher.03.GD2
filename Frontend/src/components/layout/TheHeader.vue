<template>
  <div class="header">
    <div
      class="header__sidearea"
      :style="{ 'flex-basis': isHeaderBig ? '200px' : '78px' }"
    >
      <div class="header__navicon mi mi-24 mi-header-nav"></div>
      <div class="header__applogo" v-show="isHeaderBig"></div>
    </div>
    <div class="header__mainarea">
      <div class="header__leftarea">
        <div class="header__sidebaric mi mi-24 mi-header-three-line"></div>
        <div class="header__title">
          <div class="header__brandname">
            CÔNG TY TNHH SẢN XUẤT - THƯƠNG MẠI - DỊCH VỤ QUI PHÚC
          </div>
          <div class="header__title__arrow mi mi-14 mi-arrowdown"></div>
        </div>
      </div>
      <div class="header__rightarea">
        <div
          class="header__noti mi mi-24 mi-header-bell"
          v-tooltip="'Thông báo'"
        ></div>
        <div class="header__user">
          <div class="header__avatar"></div>
          <div class="header__username">Dũng Nguyễn</div>
          <div class="header__user__arrow mi mi-24 mi-arrowdown-small"></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
// #region import
import { ref, inject, onBeforeUnmount } from "vue";
const $emitter = inject("$emitter");
// #endregion

// #region init
const isHeaderBig = ref(true);
// #endregion

// #region hook
$emitter.on("resizeSidebar", (isSidebarBig) => {
  isHeaderBig.value = isSidebarBig;
  // console.log("hi");
});

onBeforeUnmount(() => {
  $emitter.off("resizeSidebar");
});
// #endregion
</script>

<style
  scoped
  lang="css"
  src="../../css/components/layout/the-header.css"
></style>

<template>
  <div class="nav" :style="{ width: navWidth + 'px' }">
    <div class="nav-header">
      <div
        class="icon"
        :style="{ 'margin-left': (!isSmall ? 3 : 0) + 'px' }"
      ></div>

      <img
        v-if="!isSmall"
        width="26"
        height="26"
        src="../../public/favicon.png"
      />

      <div v-if="!isSmall" class="app-name">{{ $t("Nav.Accountant") }}</div>
    </div>

    <div class="nav__body">
      <router-link
        v-for="item in navigators"
        :key="item"
        :to="item.path"
        class="nav-body__category"
        :class="{ '--active': isActive(item) }"
      >
        <div class="icon"></div>
        <div v-if="!isSmall" class="title">{{ item.text }}</div>
      </router-link>
    </div>
    <div
      class="nav__footer"
      @click="
        () => {
          if (isSmall) setBigger();
          else setSmaller();
        }
      "
    >
      <div
        class="icon-container"
        :class="{ 'icon-container-smaller': isSmall }"
      >
        <div v-if="isSmall" class="icon-arrow-right-gray"></div>
        <div v-else class="icon-arrow-left-gray"></div>
      </div>
      <div v-if="!isSmall" class="title">{{ $t("Nav.Collapse") }}</div>
    </div>
  </div>
</template>

<script>
import mixin from "../js/layouts/TheNav";
export default {
  mixins: [mixin],
};
</script>

<style lang="scss" src="../style/layouts/TheNav.scss" scoped>
</style>

<template>
  <div class="header">
    <div class="left-header">
      <div class="icon" @click="showLessOnClick"></div>
      <div class="header__menu">
        {{ $t("CompanyName") }}
      </div>
      <div class="header__menu--list">
        <div class="header__menu--item">
          {{ $t("CompanyName") }}
        </div>
        <div class="header__menu--item">
          {{ $t("CompanyName") }}
        </div>
        <div class="header__menu--item">
          {{ $t("CompanyName") }}
        </div>
        <div class="header__menu--item">
          {{ $t("CompanyName") }}
        </div>
        <div class="header__menu--item">
          {{ $t("CompanyName") }}
        </div>
      </div>
      <div class="icon"></div>
    </div>
    <div class="right-header">
      <div class="icon"></div>
      <div class="icon" @click="$router.push('/account')"></div>
      <div class="header__username" @click="$router.push('/account')">
        {{ $t("UserName") }}
      </div>
      <div class="icon-container">
        <div class="icon"></div>
      </div>
    </div>
  </div>
</template>

<script>
import mixin from "../js/layouts/TheHeader";
export default {
  mixins: [mixin],
};
</script>

<style lang="scss" src="../style/layouts/TheHeader.scss">
</style>
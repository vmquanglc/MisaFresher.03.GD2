<template>
  <div class="account__home">
    <div class="account__home-head">
      <div class="home__title">{{ this.$t("AccountHome.AccountList") }}</div>
      <div class="home__nav">
        <div class="icon-container">
          <div class="icon-arrow-left-blue"></div>
        </div>
        <a href="/category" class="navigate">{{
          this.$t("AccountHome.AllCategory")
        }}</a>
      </div>
    </div>
    <div class="account__home-body">
      <AccountList></AccountList>
    </div>
  </div>
</template>

<script>
import AccountList from "./AccountList.vue";
import mixin from "../../js/views/account/AccountHome.js";
export default {
  mixins: [mixin],
  components: {
    AccountList,
  },
};
</script>

<style src="../../style/views/account/AccountHome.scss" lang="scss" scoped>
</style>
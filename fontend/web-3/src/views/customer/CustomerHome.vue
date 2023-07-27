<template>
  <div class="customer__home">
    <div class="customer__home-head">
      <div class="home__title">{{ $t("CustomerHome.CustomerList") }}</div>
      <div class="home__nav">
        <div class="icon-container">
          <div class="icon-arrow-left-blue"></div>
        </div>
        <a href="/category" class="navigate">
          {{ $t("CustomerHome.AllCategory") }}</a
        >
      </div>
    </div>
    <div class="customer__home-body">
      <CustomerList></CustomerList>
    </div>
  </div>
</template>

<script>
import CustomerList from "./CustomerList.vue";
import mixin from "../../js/views/customer/CustomerHome.js";
export default {
  mixins: [mixin],
  components: {
    CustomerList,
  },
};
</script>

<style src="../../style/views/customer/CustomerHome.scss" lang="scss" scoped>
</style>
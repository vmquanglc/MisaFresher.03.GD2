<template>
  <div
    v-show="isOpen"
    :style="{ left: left + 'px', top: top + 'px' }"
    class="menu__list"
    ref="tiepMenu"
  >
    <div @click="accountViewOnClick">
      {{ this.$t("MenuFeatures.View") }}
    </div>
    <div @click="accountDeleteOnClick">
      {{ this.$t("MenuFeatures.Delete") }}
    </div>
  </div>
</template>
    
  <script>
import mixin from "../../js/views/account/AccountMenuFeature.js";
export default {
  mixins: [mixin],
};
</script>
    
    <style src="../../style/views/account/AccountMenuFeature.scss" lang="scss">
</style>
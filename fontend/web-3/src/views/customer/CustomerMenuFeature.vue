<template>
  <div
    v-show="isOpen"
    :style="{ left: left + 'px', top: top + 'px' }"
    class="menu__list"
    ref="tiepMenu"
  >
    <div @click="customerViewOnClick">
      {{ this.$t("MenuFeatures.View") }}
    </div>
    <div @click="customerDeleteOnClick">
      {{ this.$t("MenuFeatures.Delete") }}
    </div>
  </div>
</template>
  
<script>
import mixin from "../../js/views/customer/CustomerMenuFeature.js";
export default {
  mixins: [mixin],
};
</script>
  
  <style src="../../style/views/customer/CustomerMenuFeature.scss" lang="scss">
</style>
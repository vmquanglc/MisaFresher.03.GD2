<template>
  <div
    v-show="isOpen"
    :style="{ left: left + 'px', top: top + 'px' }"
    class="menu__list"
    ref="tiepMenu"
  >
    <div v-show="receipt?.IsRecorded" @click="unSetRecorded()">Bỏ ghi</div>
    <div v-show="!receipt?.IsRecorded" @click="setRecorded()">Ghi sổ</div>
    <div @click="receiptViewOnClick">
      {{ this.$t("Button.Edit") }}
    </div>
    <div v-show="!receipt?.IsRecorded" @click="receiptDeleteOnClick">
      {{ this.$t("MenuFeatures.Delete") }}
    </div>
  </div>
</template>
    
  <script>
import mixin from "../../js/views/cash/ReceiptMenuFeature.js";
export default {
  mixins: [mixin],
};
</script>
    <style src="../../style/views/cash/ReceiptMenuFeature.scss" lang="scss">
</style>
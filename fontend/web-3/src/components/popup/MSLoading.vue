<template>
  <div class="popup__loading">
    <div class="icon-container rotate">
      <!-- <div class="icon-loading-dotted"></div> -->
      <div
        class="icon-loading-solid-green"
        :class="{ 'small-loading': isSmall, 'big-loading': isBig }"
      ></div>
    </div>
  </div>
</template>

<script>
import mixin from "../../js/components/popup/MSLoading";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/components/popup/MSLoading.scss" lang="scss">
</style>
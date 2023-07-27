<template>
  <div
    v-if="isOpen"
    ref="tiepTooltip"
    class="tooltip"
    :style="{ top: top + 'px', left: left + 'px' }"
  >
    {{ text }}
  </div>
</template>

<script>
import mixin from "../../js/components/popup/MSTooltip.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/components/popup/MSTooltip.scss" lang="scss" scoped>
</style>

<style lang="css" >
.tooltip::before {
  top: v-bind(tailTooltipYLocation);
  left: v-bind(tailTooltipXLocation);
}
</style>
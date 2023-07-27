<template>
  <div
    v-show="isOpen"
    :style="{ left: left + 'px', top: top + 'px' }"
    class="menu__list"
    ref="tiepMenu"
  >
    <div @click="employeeDuplicateOnClick">
      {{ this.$t("MenuFeatures.Replication") }}
    </div>
    <div @click="employeeDeleteOnClick">
      {{ this.$t("MenuFeatures.Delete") }}
    </div>
    <div>
      {{ this.$t("MenuFeatures.StopUsing") }}
    </div>
  </div>
</template>

<script>
import mixin from "../../js/views/employee/EmployeeMenuFeature.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/views/employee/EmployeeMenuFeature.scss" lang="scss">
</style>
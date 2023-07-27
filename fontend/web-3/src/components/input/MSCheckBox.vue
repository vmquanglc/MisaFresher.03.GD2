<template>
  <div class="input-checkbox">
    <label>
      <div class="box">
        <input
          :readonly="readonly"
          type="checkbox"
          :value="isChecked"
          @input="onInput"
        />
        <div v-if="isTableHead" :class="{ 'checked-th': isChecked }"></div>
        <div v-else :class="{ 'checked-td': isChecked }"></div>
      </div>
      {{ title }}
    </label>
  </div>
</template>

<script>
import mixin from "../../js/components/input/MSCheckBox.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/components/input/MSCheckBox.scss" lang="scss" scoped>
</style>
<template>
  <div class="input-search">
    <div
      v-if="title"
      class="input__label"
      @mousemove="
        (event) => {
          if (tooltip) {
            $msEmitter.emit('showTooltip', event, tooltip);
          }
        }
      "
      @mouseout="$msEmitter.emit('hideTooltip')"
    >
      {{ title }}
    </div>
    <div class="input__text">
      <input
        class="input__text-field"
        type="text"
        :placeholder="placeholder"
        v-model="inputValue"
        @input="$emit('update:value', inputValue)"
        v-debounce:500ms="inputSearchOnSubmit"
      />
      <!-- @keyup.enter="inputSearchOnSubmit" -->

      <div class="icon-container" @click="inputSearchOnSubmit">
        <div class="icon-search-small-black"></div>
      </div>
    </div>
  </div>
</template>

<script>
import mixin from "../../js/components/input/MSInputSearch.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/components/input/MSInputSearch.scss" lang="scss" scoped>
</style>
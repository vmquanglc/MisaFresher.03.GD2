<template>
  <div class="input">
    <div
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

    <label v-for="item in items" :key="item.id">
      <input
        type="radio"
        :value="item.id"
        :checked="item.id == id"
        @click="itemOnSelect(item)"
        @keypress.enter="focusNext($event.target)"
        :disabled="readonly"
      />
      {{ item.value }}
    </label>
  </div>
</template>

<script>
import mixin from "../../js/components/input/MSInputRadio.js";
export default {
  mixins: [mixin],
};
</script>
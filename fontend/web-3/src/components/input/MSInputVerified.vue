<template>
  <div class="input-verified">
    <label class="input__label">Field label</label>

    <div class="input__text">
      <input class="input__text-field" type="text" placeholder="misa.amis.vn" />

      <div class="icon-container">
        <div class="icon-check-green"></div>
      </div>
    </div>
  </div>
</template>

<script>
import mixin from "../../js/components/input/MSInputVerified.js";
export default {
  mixins: [mixin],
};
</script>
<template>
  <div class="input-verifying">
    <label class="input__label">Field label</label>

    <div class="input__text">
      <input
        class="input__text-field"
        type="text"
        placeholder="Some things..."
      />

      <div class="icon-container">
        <div class="icon-refresh-green rotate"></div>
      </div>
    </div>
  </div>
</template>

<script>
import mixin from "../../js/components/input/MSInputVerifying.js";
export default {
  mixins: [mixin],
};
</script>
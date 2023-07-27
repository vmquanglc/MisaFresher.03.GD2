<template>
  <div class="input">
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
      <span v-if="isRequired" class="required">*</span>
    </div>
    <div class="input__text">
      <textarea
        v-if="rows"
        :rows="rows"
        ref="myFirstInput"
        class="input__text-field"
        :class="{
          'border-red': isValidating && isInvalidInput,
        }"
        :placeholder="placeholder"
        v-model="inputValue"
        @input="$emit('update:value', inputValue)"
        @keypress.enter="focusNext($event.target)"
        @keypress="checkValidInput($event)"
        v-debounce:500ms="doneTyping"
        :readonly="readonly"
      ></textarea>
      <input
        v-else
        ref="myFirstInput"
        class="input__text-field"
        :class="{
          'border-red':
            (isValidating && isInvalidInput) || isErroredFromOutSide,
          'align-right': textAlign == 'right',
          'align-left': textAlign == 'left',
          'color-red':
            inputType == 'money' &&
            inputValue &&
            inputValue?.indexOf('-') != -1,
        }"
        :placeholder="placeholder"
        v-model="inputValue"
        @input="
          $emit(
            'update:value',
            inputValue && inputType == 'money'
              ? inputValue?.replace(/\./g, '')
              : inputValue
          )
        "
        @keypress.enter="focusNext($event.target)"
        @keypress="checkValidInput($event)"
        v-debounce:500ms="doneTyping"
        :readonly="readonly"
      />

      <p v-if="isValidating && isInvalidInput" class="errormessage">
        {{ errorMessage }}
      </p>
      <p v-if="isErroredFromOutSide" class="errormessage">
        {{ messageFromOutSide }}
      </p>
    </div>
  </div>
</template>

<script>
import mixin from "../../js/components/input/MSInputText.js";
export default {
  mixins: [mixin],
};
</script>

<style scoped>
.input__label {
  font-weight: bold;
}
textarea {
  padding: 12px !important;
  height: 100% !important;
  width: 99% !important;
  resize: none !important;
  font-family: Roboto;
}

.input__text-field:read-only {
  background-color: var(--input-disabled-color);
}

.align-right {
  text-align: right !important;
  padding-right: 12px !important;
}

.color-red {
  color: red !important;
}
</style>
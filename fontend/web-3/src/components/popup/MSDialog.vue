<template>
  <div class="dialog" :class="{ 'dialog-warning': isWarning }">
    <div class="dialog__container">
      <div class="dialog__head">
        <div class="title">
          {{ title }}
        </div>
        <div
          class="icon-container"
          @click="cancelOnClick()"
          @mousemove="
            (event) =>
              $msEmitter.emit(
                'showTooltip',
                event,
                this.$t('Button.Close') + ' (ESC)'
              )
          "
          @mouseout="$msEmitter.emit('hideTooltip')"
        >
          <div class="icon-x-black"></div>
        </div>
      </div>
      <div class="dialog__body">
        <div>
          <ul>
            <li v-for="item in items" :key="item">
              {{ items.length == 1 ? "" : "-" }}
              {{ item }}
            </li>
          </ul>
        </div>
      </div>

      <form class="dialog__foot">
        <button
          ref="firstTabindexRepeater"
          @focus="focusFirstButton"
          tabindex="5"
          style="background-color: white; border: 0px solid white"
        ></button>

        <button
          v-if="contentCancel"
          ref="buttonCancel"
          type="button"
          class="extra-button"
          @keydown.enter="cancelOnClick"
          @keydown.right="nextButton($event.target)"
          @keydown.left="previousButton($event.target)"
          @click="cancelOnClick"
          tabindex="4"
        >
          {{ contentCancel }}
        </button>

        <button
          v-if="contentNo"
          ref="buttonNo"
          type="button"
          class="extra-button"
          @keydown.enter="noOnClick"
          @keydown.right="nextButton($event.target)"
          @keydown.left="previousButton($event.target)"
          @click="noOnClick"
          tabindex="3"
        >
          {{ contentNo }}
        </button>

        <button
          v-if="contentYes"
          ref="buttonYes"
          type="button"
          class="main-button"
          @keydown.enter="yesOnClick"
          @keydown.right="nextButton($event.target)"
          @keydown.left="previousButton($event.target)"
          @click="yesOnClick"
          tabindex="2"
          @mousemove="
            (event) =>
              $msEmitter.emit('showTooltip', event, contentYes + ' (Enter)')
          "
          @mouseout="$msEmitter.emit('hideTooltip')"
        >
          {{ contentYes }}
        </button>

        <button
          ref="lastTabindexRepeater"
          @focus="focusLastButton"
          tabindex="1"
          style="
            background-color: white;
            border: 0px solid white;
            position: absolute;
          "
        ></button>
      </form>
    </div>
  </div>
</template>

<script>
import mixin from "../../js/components/popup/MSDialog.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/components/popup/MSDialog.scss" lang="scss" scoped>
</style>
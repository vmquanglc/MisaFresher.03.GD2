<template>
  <div v-click-outside="comboboxClickOutSide" class="input">
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
      {{ title }} <span v-if="isRequired" class="required">*</span>
    </div>

    <div
      class="mcombobox"
      :class="{
        'outline-red': isValidating && isInvalidInput,
      }"
      :style="{
        outline:
          isShowList && !disabled && items && items.length > 0
            ? '1px solid #50b83c'
            : '',
      }"
    >
      <input
        type="text"
        class="mcombobox__input"
        v-model="inputValueComputed"
        v-debounce:1000ms="finishedTyping"
        :placeholder="placeholder"
        @input="inputTextOnChange()"
        @keypress.enter="focusNext($event.target)"
        @keyup.up="onPressKeyArrowUp"
        @keyup.down="onPressKeyArrowDown"
        @blur="onBlur"
        :disabled="disabled"
        :readonly="readonly"
        ref="myFirstInput"
      />
      <div class="mcombobox__button" @click="toggleItemsList()">
        <div class="icon-container">
          <div class="icon-arrow-down-black"></div>
        </div>
      </div>
      <div
        class="mcombobox__data"
        v-show="isShowList && !disabled && items && items.length > 0"
        @scroll="onScroll"
      >
        <div v-if="!listCols">
          <div
            v-for="item in items"
            :key="item[propValue]"
            class="mcombobox-item"
            :class="{ '--selected': item[propText] == inputText && isUsingKey }"
            :value="item[propValue]"
            @click="selectItemOnClick(item)"
            :style="{ 'min-width': '230px' }"
          >
            {{ item[propText] }}
            <div class="icon-container" v-if="item[propText] == inputText">
              <div class="icon-check-green"></div>
            </div>
          </div>
        </div>
        <div v-else>
          <table>
            <tr>
              <th
                v-for="(head, index) in listTitle"
                :key="head"
                :style="{ 'min-width': columnWidthList[index] * 10 + 'px' }"
              >
                {{ head }}
              </th>
              <th></th>
            </tr>
            <div class="sticky-table" ref="stickyTable">
              <tr
                v-for="item in items"
                :key="item[propValue]"
                @click="selectItemOnClick(item)"
                :class="{ '--selected': item[propText] == inputText }"
              >
                <td
                  v-for="(col, index) in listCols"
                  :key="col"
                  :style="{ 'min-width': columnWidthList[index] * 10 + 'px' }"
                >
                  {{ item[col] }}
                </td>
                <td>
                  <div
                    class="icon-container"
                    v-if="item[propText] == inputText"
                  >
                    <div class="icon-check-green" ref="checkedItem"></div>
                  </div>
                </td>
              </tr>
            </div>
          </table>
        </div>
      </div>
      <p v-if="isValidating && isInvalidInput" class="errormessage">
        {{ message }}
      </p>
    </div>
  </div>
</template>

<script>
import mixin from "../../js/components/input/MSInputCombobox.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/components/input/MSInputCombobox.scss" lang="scss" scoped>
</style>
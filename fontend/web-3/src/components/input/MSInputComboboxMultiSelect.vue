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
      :class="{ 'outline-red': isValidating && isInvalidInput }"
      :style="{
        outline:
          isShowList && !disabled && items && items.length > 0
            ? '1px solid #50b83c'
            : '',
      }"
    >
      <div class="mcombobox__body">
        <div
          v-for="selectedItem in selectedItems"
          :key="selectedItem[this.propValue]"
          class="selected-item"
        >
          <div class="item-text">{{ selectedItem[this.propText] }}</div>
          <div class="icon-container" @click="removeSelectedItem(selectedItem)">
            <div class="icon-x-black--small"></div>
          </div>
        </div>

        <input
          type="text"
          class="mcombobox__input"
          v-model="inputValueComputed"
          v-debounce:1000ms="finishedTyping"
          @input="inputTextOnChange()"
          @keypress.enter="
            finishedTyping();
            focusNext($event.target);
          "
          @keyup.up="onPressKeyArrowUp"
          @keyup.down="onPressKeyArrowDown"
          @blur="onBlur"
          :readonly="readonly"
          :disabled="disabled"
        />
      </div>
      <div class="mcombobox__button" @click="toggleItemsList()">
        <div class="icon-container">
          <div class="icon-arrow-down-black"></div>
        </div>
      </div>
      <div
        class="mcombobox__data"
        v-show="isShowList && items && items.length > 0"
      >
        <div v-if="!listCols">
          <div
            v-for="item in items"
            :key="item[propValue]"
            class="mcombobox-item"
            :class="{ '--selected': item[propText] == inputText && isUsingKey }"
            :value="item[propValue]"
            @click="selectItemOnClick(item)"
          >
            {{ item[propText] }}
            <div class="icon-container" v-if="ids.includes(item[propValue])">
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
            <div class="sticky-table">
              <tr
                v-for="item in items"
                :key="item[propValue]"
                @click="selectItemOnClick(item)"
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
                    v-if="ids.includes(item[propValue])"
                  >
                    <div class="icon-check-green"></div>
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
import mixin from "../../js/components/input/MSInputComboboxMultiSelect.js";
export default {
  mixins: [mixin],
};
</script>
  
  <style src="../../style/components/input/MSInputComboboxMultiSelect.scss" lang="scss" scoped>
</style>

<style lang="scss" scoped>
</style>
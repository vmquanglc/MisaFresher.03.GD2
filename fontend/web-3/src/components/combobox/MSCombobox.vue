<template>
  <div class="mcombobox" :class="{ 'mcombobox--small': isSmall }">
    <input
      type="text"
      class="mcombobox__input"
      :class="{ 'mcombobox__input--small': isSmall }"
      v-model="inputText"
      v-debounce:500ms="inputOnChange"
      @keypress="checkValidInput($event)"
    />
    <div
      class="mcombobox__button"
      :class="{ 'mcombobox__button--small': isSmall }"
      @click="toggleItemsList"
    >
      <div class="icon-container">
        <div v-if="isShowList" class="icon-arrow-up-black"></div>
        <div v-else class="icon-arrow-down-black"></div>
      </div>
    </div>
    <div
      class="mcombobox__data"
      :class="{ 'mcombobox__data--up': isSmall }"
      v-show="isShowList && items.length > 0"
    >
      <a
        v-for="item in items"
        :key="item"
        class="mcombobox-item"
        :value="item"
        @click="selectItemOnClick(item)"
        >{{ item }}
        <div class="icon-container" v-if="item == value">
          <div class="icon-check-green"></div>
        </div>
      </a>
    </div>
  </div>
</template>

<script>
import mixin from "../../js/components/combobox/MSCombobox.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/components/combobox/MSCombobox.scss" lang="scss" scoped>
</style>
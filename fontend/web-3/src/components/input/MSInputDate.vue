<template>
  <div v-click-outside="pickerClickOutSide" class="input-date">
    <label
      class="input__label"
      @mousemove="
        (event) => {
          if (tooltip) {
            $msEmitter.emit('showTooltip', event, tooltip);
          }
        }
      "
      @mouseout="$msEmitter.emit('hideTooltip')"
      >{{ title }} <span v-if="isRequired" class="required">*</span>
    </label>

    <div class="input__text">
      <input
        class="input__text-field"
        :class="{
          'border-red':
            isValidating && (isInvalidInput || (isRequired && !inputValue)),
        }"
        type="text"
        placeholder="dd/mm/yyyy"
        v-model="inputValue"
        @keypress.enter="focusNext($event.target)"
        @keypress="checkValidInput($event)"
        :readonly="readonly"
      />
      <p
        v-if="isValidating && (isInvalidInput || (isRequired && !inputValue))"
        class="errormessage"
      >
        {{ errorMessage }}
      </p>
      <div ref="iconDate" class="icon-container" @click="toggleDatePicker">
        <div class="icon-date"></div>
      </div>
    </div>

    <div
      :style="{ left: left + 'px', top: top + 'px' }"
      class="date-picker__popup"
      v-show="isShowingDatePicker"
    >
      <!-- Bắt đầu year picker -->
      <div v-show="isShowingYearPicker" class="year-picker">
        <div class="year-picker__head">
          <div class="icon-container" @click="setYear(year - 12)">
            <div class="icon-arrow-left-black"></div>
          </div>
          <div
            class="year-picker__year-range"
            @click="isShowingYearPicker = false"
          >
            {{ year - 5 }} - {{ year + 6 }}
          </div>
          <div class="icon-container" @click="setYear(year + 12)">
            <div class="icon-arrow-right-black"></div>
          </div>
        </div>
        <div class="year-picker__body">
          <div
            v-for="num in 12"
            :key="num"
            @click="
              setYear(num + year - 6);
              this.isShowingYearPicker = false;
            "
            class="year-picker__item"
            :class="{ selected: year == num + year - 6 }"
          >
            {{ num + year - 6 }}
          </div>
        </div>
      </div>
      <!-- Kết thúc year picker -->
      <!-- Bắt đầu month picker -->
      <div v-show="isShowingMonthPicker" class="month-picker">
        <div class="month-picker__head">
          <div class="icon-container" @click="setMonth(month - 1)">
            <div class="icon-arrow-left-black"></div>
          </div>
          <div
            class="month-picker__month"
            @click="
              this.isShowingMonthPicker = false;
              this.isShowingYearPicker = true;
            "
          >
            {{ this.$t("DateTime.Month") }} {{ month }}
          </div>
          <div class="icon-container" @click="setMonth(month + 1)">
            <div class="icon-arrow-right-black"></div>
          </div>
        </div>
        <div class="month-picker__body">
          <div
            v-for="num in 12"
            :key="num"
            @click="
              setMonth(num);
              this.isShowingMonthPicker = false;
              this.isShowingYearPicker = true;
            "
            class="month-picker__item"
            :class="{ selected: month == num }"
          >
            {{ num }}
          </div>
        </div>
      </div>
      <!-- Kết thúc month picker -->
      <!-- Bắt đầu date picker head -->
      <div class="date-picker__head">
        <div
          class="date-head-left"
          @click="
            isShowingMonthPicker = !isShowingMonthPicker;
            this.isShowingYearPicker = false;
          "
        >
          <div class="month-and-year">
            {{ this.$t("DateTime.Month") }} {{ month }}, {{ year }}
          </div>
          <div class="icon-container">
            <div class="icon-arrow-down-black"></div>
          </div>
        </div>
        <div class="week-changer">
          <div class="icon-container" @click="getLastMonthOnClick">
            <div class="icon-arrow-left-black"></div>
          </div>
          <div class="icon-container" @click="getNextMonthOnLick">
            <div class="icon-arrow-right-black"></div>
          </div>
        </div>
      </div>
      <!-- Kết thúc date picker head -->
      <!-- Bắt đầu date picker body -->
      <div class="date-picker__body">
        <div class="date-picker-weekname">
          <div class="week-name">{{ this.$t("DateTime.Monday") }}</div>
          <div class="week-name">{{ this.$t("DateTime.Tuesday") }}</div>
          <div class="week-name">{{ this.$t("DateTime.Wednesday") }}</div>
          <div class="week-name">{{ this.$t("DateTime.Thursday") }}</div>
          <div class="week-name">{{ this.$t("DateTime.Friday") }}</div>
          <div class="week-name">{{ this.$t("DateTime.Saturday") }}</div>
          <div class="week-name">{{ this.$t("DateTime.Sunday") }}</div>
        </div>
        <div class="date-picker-day">
          <div
            v-for="number in numberDay"
            :key="number"
            class="date-item"
            :class="{ selected: number == day }"
            @click="setDay(number)"
          >
            {{ number }}
          </div>
        </div>
      </div>
      <!-- Kết thúc date picker body -->
      <!-- bắt đầu date picker footer -->
      <div class="date-picker__footer">
        <div class="link-button" @click="getToDay">
          {{ this.$t("DateTime.Today") }}
        </div>
      </div>
      <!-- Kết thúc date picker footer -->
    </div>
  </div>
</template>

<script>
import mixin from "../../js/components/input/MSInputDate.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/components/input/MSInputDate.scss" lang="scss" scoped>
</style>

<style lang="scss" scoped>
.input__text-field:read-only {
  background-color: var(--input-disabled-color);
}
.input-date {
  .date-picker__popup {
    .date-picker__body {
      .date-item:nth-child(1) {
        grid-column-start: v-bind(startColumn);
      }
    }
  }
}
</style>
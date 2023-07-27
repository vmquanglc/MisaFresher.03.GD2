<template>
  <div class="notice">
    <div class="notice__left">
      <div class="icon-container">
        <div :class="iconClass"></div>
      </div>
      <div class="notice__type" :class="colorClass">
        {{ noticeType }}
      </div>
      <div class="notice__content">{{ notice.message }}</div>
    </div>
    <div class="notice__right">
      <div class="icon-container" @click="closeOnClick">
        <div class="icon-x-black"></div>
      </div>
    </div>
  </div>
</template>

<script>
import mixin from "../../js/components/popup/MSNoticeMessage.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/components/popup/MSNoticeMessage.scss" lang="scss">
</style>
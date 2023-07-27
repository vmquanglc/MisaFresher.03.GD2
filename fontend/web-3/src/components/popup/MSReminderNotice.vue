<template>
  <div
    ref="noticeContainer"
    :style="{ left: positionLeft + 'px' }"
    class="notice-container"
  >
    <MSNoticeMessage
      v-for="notice in messageList"
      :key="notice.id"
      :notice="notice"
    ></MSNoticeMessage>
  </div>
</template>

<script>
import mixin from "../../js/components/popup/MSReminderNotice.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/components/popup/MSReminderNotice.scss" lang="scss">
</style>
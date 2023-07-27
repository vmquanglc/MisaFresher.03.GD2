<template>
  <div class="cash__home">
    <div class="cash__nav">
      <div
        class="nav-button"
        :class="{ 'button--active': navTab == 0 }"
        @click="$router.push(`/cash/business-process`)"
      >
        {{ $t("Cash.Procedure") }}
      </div>
      <div
        class="nav-button"
        :class="{ 'button--active': navTab == 1 }"
        @click="$router.push(`/cash/receipt`)"
      >
        {{ $t("Cash.CollectAndSpendMoney") }}
      </div>
    </div>
    <div class="cash__content">
      <router-view :key="cashHomeKey" name="CashRouterView"></router-view>
    </div>
  </div>
  <MSDialog
    v-if="dialog.isShowDialog"
    :title="dialog.title"
    :items="dialog.items"
    :contentNo="dialog.contentNo"
    :contentYes="dialog.contentYes"
    :contentCancel="dialog.contentCancel"
    :dialogType="dialog.dialogType"
    :isWarning="dialog.isWarning"
  ></MSDialog>
</template>

<script>
import mixin from "../../js/views/cash/CashHome.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/views/cash/CashHome.scss" lang="scss" scoped>
</style>
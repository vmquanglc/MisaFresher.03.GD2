<template>
  <div class="business-process">
    <div class="business-process__content">
      <div class="cash__process">
        <div class="cp__header">
          {{ $t("Cash.CashBusiness") }}
        </div>
        <div class="cp__body">
          <div class="body__button" @click="$router.push('/cash/create')">
            <div class="body__button-content">
              {{ $t("Cash.CollectMoney") }}
            </div>
          </div>
          <div class="body__button">
            <div class="body__button-content">
              {{ $t("Cash.FundInventory") }}
            </div>
          </div>
          <div class="body__button">
            <div class="body__button-content">
              {{ $t("Cash.Pay") }}
            </div>
          </div>
        </div>
        <div class="cp__footer">
          <div
            v-for="(footerButton, index) in footerButtons"
            :key="index"
            class="footer__button"
            @click="
              if (footerButton.route) {
                $router.push(footerButton.route);
              }
            "
          >
            <div class="button__icon"></div>
            <div class="button__content">{{ footerButton.name }}</div>
          </div>
        </div>
      </div>
      <div class="cash__report">
        <div class="cr__header">
          {{ $t("Cash.Report") }}
        </div>
        <div class="cr__body">
          <ul>
            <li>{{ $t("Cash.DailyCashBalanceStatisticsTable") }}</li>
            <li>{{ $t("Cash.CashFlow") }}</li>
            <li>{{ $t("Cash.CashCollectionLogNumber") }}</li>
            <li>{{ $t("Cash.DetailedAccountingNumberOfCashFund") }}</li>
            <li>{{ $t("Cash.PaymentDiaryNumber") }}</li>
          </ul>
        </div>
        <div class="cr__footer">{{ $t("Cash.AllReports") }}</div>
      </div>
    </div>
  </div>
  <router-view name="CashRouterView"></router-view>
</template>

<script>
import mixin from "../../js/views/cash/CashBusinessProcess.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/views/cash/CashBusinessProcess.scss" lang="scss" scoped>
</style>
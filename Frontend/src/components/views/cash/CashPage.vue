<template>
  <div class="pcontent">
    <div class="pcontent__tab">
      <div
        class="tab"
        :class="[route.path.includes('CAProcess') ? 'tab--selected' : '']"
        @click="processTabOnClick"
      >
        {{ lang.cash_page.tab.process }}
      </div>
      <div
        class="tab"
        @click="receiptTabOnClick"
        :class="[route.path.includes('CAReceipt') ? 'tab--selected' : '']"
      >
        {{ lang.cash_page.tab.receipt }}
      </div>
    </div>
    <div class="pcontent__main">
      <router-view></router-view>
    </div>
  </div>
</template>

<script setup>
import { useRouter, useRoute } from "vue-router";
import { inject } from "vue";

const router = useRouter();
const route = useRoute();
const lang = inject("$lang");

function processTabOnClick() {
  router.replace("/CA/CAProcess");
}

function receiptTabOnClick() {
  router.replace("/CA/CAReceipt");
}
</script>

<style
  scoped
  lang="css"
  src="../../../css/components/views/cash/cash-page.css"
></style>

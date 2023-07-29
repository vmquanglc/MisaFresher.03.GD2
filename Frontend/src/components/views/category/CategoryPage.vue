<template>
  <div class="pcontent" v-if="route.path == '/DI'">
    <div class="pcontent__title">{{ lang.category.pageTitle }}</div>
    <div class="pcontent__container">
      <div class="container__row" v-for="(row, index) in rows" :key="index">
        <div
          class="container__colgroup"
          v-for="(colGroup, index) in row"
          :key="index"
          :style="{ width: 100 / row.length + '%' }"
        >
          <div class="col__item" v-for="item in colGroup" :key="item.title">
            <div class="col__title">{{ item.title }}</div>
            <router-link
              class="col__link"
              v-for="link in item.links"
              :key="link.name"
              :to="link.to"
            >
              {{ link.name }}
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
  <router-view></router-view>
</template>

<script setup>
import { ref, inject } from "vue";
import { useRoute } from "vue-router";
const route = useRoute();
const lang = inject("$lang");

const rows = ref([
  [
    [
      {
        title: lang.category.menu.subject,
        links: [
          {
            name: lang.category.link.customer,
            to: "/DI/DICustomer",
          },
          {
            name: lang.category.link.provider,
            to: "",
          },
          {
            name: lang.category.link.employee,
            to: "/DI/DIEmployee",
          },
          {
            name: lang.category.link.group,
            to: "",
          },
        ],
      },
    ],
    [
      {
        title: lang.category.menu.goods,
        links: [
          {
            name: lang.category.link.supply,
            to: "",
          },
          {
            name: lang.category.link.warehouse,
            to: "",
          },
          {
            name: lang.category.link.groupOfGoods,
            to: "",
          },
          {
            name: lang.category.link.unit,
            to: "",
          },
        ],
      },
    ],
    [
      {
        title: lang.category.menu.account,
        links: [
          {
            name: lang.category.link.accountSystem,
            to: "/DI/DIAccount",
          },
          {
            name: lang.category.link.transferAccount,
            to: "",
          },
          {
            name: lang.category.link.defaultAccount,
            to: "",
          },
        ],
      },
    ],
  ],
  [
    [
      {
        title: lang.category.menu.price,
        links: [
          {
            name: lang.category.link.costAggregationObj,
            to: "",
          },
          {
            name: lang.category.link.expenseItem,
            to: "",
          },
          {
            name: lang.category.link.construction,
            to: "",
          },
          {
            name: lang.category.link.constructionType,
            to: "",
          },
        ],
      },
    ],
    [
      {
        title: lang.category.menu.bank,
        links: [
          {
            name: lang.category.link.bank,
            to: "",
          },
          {
            name: lang.category.link.bankAcc,
            to: "",
          },
        ],
      },
    ],
    [
      {
        title: lang.category.menu.branch,
        links: [
          {
            name: lang.category.link.organizationStruct,
            to: "",
          },
        ],
      },
    ],
  ],
  [
    [
      {
        title: lang.category.menu.asset,
        links: [
          {
            name: lang.category.link.toolType,
            to: "",
          },
          {
            name: lang.category.link.assetType,
            to: "",
          },
        ],
      },
      {
        title: lang.category.menu.income,
        links: [
          {
            name: lang.category.link.typeKeepingSymbol,
            to: "",
          },
          {
            name: lang.category.link.incomeTaxSchedule,
            to: "",
          },
        ],
      },
    ],
    [
      {
        title: lang.category.menu.tax,
        links: [
          {
            name: lang.category.link.specialConsumptionTariff,
            to: "",
          },
          {
            name: lang.category.link.resourceTaxSchedule,
            to: "",
          },
        ],
      },
    ],
    [
      {
        title: lang.category.menu.other,
        links: [
          {
            name: lang.category.link.termsOfPayments,
            to: "",
          },
          {
            name: lang.category.link.revenueAndPayment,
            to: "",
          },
          {
            name: lang.category.link.statisticalCode,
            to: "",
          },
          {
            name: lang.category.link.currency,
            to: "",
          },
          {
            name: lang.category.link.documentType,
            to: "",
          },
        ],
      },
    ],
  ],
]);
</script>

<style
  scoped
  lang="css"
  src="../../../css/components/views/category/category-page.css"
></style>

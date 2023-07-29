<template>
  <div class="cash__process__container">
    <div class="cash__process__main">
      <div class="main__left">
        <div class="left__top">
          <div class="top__title">
            <div class="title__text">
              {{ lang.cash_process.text.cashmajor }}
            </div>
          </div>
          <div class="top__content">
            <div class="box__item receive__box" @click="receiveBoxOnClick">
              <div class="box__text">{{ lang.cash_process.text.receive }}</div>
            </div>
            <div class="box__item payment__box">
              <div class="box__text">{{ lang.cash_process.text.paid }}</div>
            </div>
            <div class="box__item audit__box">
              <div class="box__text">
                {{ lang.cash_process.text.fundInventory }}
              </div>
            </div>
          </div>
        </div>
        <div class="left__bottom">
          <div class="bottom__item" @click="customerItemOnClick">
            <div class="item__content">
              <div class="item__icon minc mi-24 mi-process-customer"></div>
              <div class="item__text">
                {{ lang.cash_process.text.customer }}
              </div>
            </div>
          </div>
          <div class="bottom__item">
            <div class="item__content">
              <div class="item__icon minc mi-24 mi-process-provider"></div>
              <div class="item__text">
                {{ lang.cash_process.text.provider }}
              </div>
            </div>
          </div>
          <div class="bottom__item" @click="employeeItemOnClick">
            <div class="item__content">
              <div class="item__icon minc mi-24 mi-process-employee"></div>
              <div class="item__text">
                {{ lang.cash_process.text.employee }}
              </div>
            </div>
          </div>
          <div class="bottom__item">
            <div class="item__content">
              <div class="item__icon minc mi-24 mi-setting--active"></div>
              <div class="item__text">{{ lang.cash_process.text.option }}</div>
            </div>
          </div>
        </div>
      </div>
      <div class="main__right">
        <div class="right__top">
          <div class="top__title">
            <div class="title__text">{{ lang.cash_process.text.report }}</div>
          </div>
        </div>
        <div class="right__mid">
          <ul class="rm__ul">
            <li>{{ lang.cash_process.text.reportOption.opt1 }}</li>
            <li>{{ lang.cash_process.text.reportOption.opt2 }}</li>
            <li>{{ lang.cash_process.text.reportOption.opt3 }}</li>
            <li>{{ lang.cash_process.text.reportOption.opt4 }}</li>
            <li>{{ lang.cash_process.text.reportOption.opt5 }}</li>
          </ul>
        </div>
        <div class="right__bottom">
          <div class="bottom__text">{{ lang.cash_process.text.allReport }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useRouter } from "vue-router";
import { inject } from "vue";

const lang = inject("$lang");

const router = useRouter();

function customerItemOnClick() {
  router.push("/DI/DICustomer");
}

function employeeItemOnClick() {
  router.push("/DI/DIEmployee");
}

function receiveBoxOnClick() {
  router.push("/CA/CAReceipt/create");
}
</script>

<style
  scoped
  lang="css"
  src="../../../../css/components/views/cash/process/cash-process.css"
></style>

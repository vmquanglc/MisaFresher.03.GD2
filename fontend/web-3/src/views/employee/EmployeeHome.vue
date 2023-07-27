<template>
  <div class="content">
    <div class="content__head">
      <div class="content__title">
        <h2>{{ this.$t("Header.Employee") }}</h2>
        <div class="home__nav">
          <div class="icon-container">
            <div class="icon-arrow-left-blue"></div>
          </div>
          <a href="/category" class="navigate">
            {{ $t("CustomerHome.AllCategory") }}</a
          >
        </div>
      </div>
    </div>

    <div class="content__body">
      <div class="content__body-head">
        <MSInputSearch
          v-model:value="inputSearchValue"
          :placeholder="this.$t('Placeholder.InputSearch')"
          @inputSearchOnSubmit="this.$msEmitter.emit('searchEmployee')"
        ></MSInputSearch>
        <div
          class="icon-container"
          @click="reloadEmployeeOnclick"
          @mousemove="
            (event) =>
              $msEmitter.emit(
                'showTooltip',
                event,
                this.$t('Button.ReloadList')
              )
          "
          @mouseout="$msEmitter.emit('hideTooltip')"
        >
          <div class="icon-reload"></div>
        </div>
        <div
          class="icon-container"
          @click="exportEmployeeOnclick"
          @mousemove="
            (event) =>
              $msEmitter.emit(
                'showTooltip',
                event,
                this.$t('Button.ExportList')
              )
          "
          @mouseout="$msEmitter.emit('hideTooltip')"
        >
          <div class="icon-excel"></div>
        </div>
        <div
          class="icon-container"
          @mousemove="
            (event) =>
              $msEmitter.emit('showTooltip', event, this.$t('Button.Setting'))
          "
          @mouseout="$msEmitter.emit('hideTooltip')"
          @click="notSupported()"
        >
          <div class="icon-setting-gray"></div>
        </div>

        <button class="extra-button" @click="notSupported()">
          {{ this.$t("Button.Utilities") }}
        </button>
        <button
          class="main-button"
          @click="addEmployee"
          @mousemove="
            (event) =>
              $msEmitter.emit(
                'showTooltip',
                event,
                this.$t('Button.AddEmployee') + ' (Ctrl + 1)'
              )
          "
          @mouseout="$msEmitter.emit('hideTooltip')"
        >
          {{ this.$t("Button.AddEmployee") }}
        </button>
      </div>
      <div class="content__body-body">
        <EmployeeList :inputSearchValue="inputSearchValue"></EmployeeList>
      </div>
    </div>

    <router-view :key="employeeFormKey" name="EmployeeRouterView"></router-view>
  </div>
  <div v-show="isLoading">
    <MSLoading></MSLoading>
  </div>
</template>

<script>
import mixin from "../../js/views/employee/EmployeeHome.js";
export default {
  mixins: [mixin],
};
</script>


<style src="../../style/views/employee/EmployeeHome.scss" lang="scss" scoped>
</style>

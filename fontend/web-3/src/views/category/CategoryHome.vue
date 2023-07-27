<template>
  <div class="category__content">
    <div class="category__header">Danh mục</div>
    <div class="category__body">
      <div class="category__container">
        <div
          v-for="(category, index) in categorys"
          :key="index"
          class="category__block"
        >
          <div class="head">{{ category.title }}</div>
          <div
            v-for="(item, i) in category.items"
            :key="i"
            class="item"
            @click="
              if (item.route) {
                $router.push(item.route);
              }
            "
          >
            {{ item.content }}
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import mixin from "../../js/views/category/CategoryHome.js";
export default {
  mixins: [mixin],
};
</script>

<style src="../../style/views/category/CategoryHome.scss" lang="scss" scoped>
</style>
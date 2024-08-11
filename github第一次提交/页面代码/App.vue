<template>
  <div id="app">
    <Sidebar @changeView="handleChangeView" />
    <div class="MainContent">
      <component :is="currentView"></component>
    </div>
  </div>
</template>

<script>
import Sidebar from './views/BusinessSidebar.vue';
import BusinessHomePage from './views/BusinessHomePage.vue';
import BusinessMarket from './views/BusinessMarket.vue';
import BusinessOrder from './views/BusinessOrderManage.vue';
import BusinessCommodity from './views/BusinessCommodity.vue';

export default {
  name: 'App',
  components: {
    Sidebar,
    BusinessHomePage,
    BusinessMarket,
    BusinessOrder,
    BusinessCommodity,
  },
  data() {
    return {
      currentView: 'BusinessHomePage'
    }
  },
  methods: {
    handleChangeView(view) {
      this.currentView = view;
    }
  }
}
</script>

<style>
#app {
  display: flex;
}

.MainContent {
  margin-left: 23vh;
  margin-top: 6vh;
}
</style>
————————————————

<!-- 商家页面 -->
<template>
    <div style="display: flex;">
      <Sidebar @changeView="handleChangeView" />
      <div class="MainContent" style="border: 7px solid white;">
        <component :is="currentView"></component>
      </div>
    </div>
  </template>
  
  <script>
  import Sidebar from './BusinessSidebar.vue';
  import BusinessHomePage from './BusinessHomePage.vue';
  import BusinessMarket from './BusinessMarket.vue';
  import BusinessOrder from './BusinessOrderManage.vue';
  import BusinessCommodity from './BusinessCommodity.vue';
  
  export default {
    // name: 'App',
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
  
  <style scoped>
  #app {
    display: flex;
  }
  
  .MainContent {
    margin-left: 23vh;
    margin-top: 6vh;
  }
  </style>
  ————————————————
  
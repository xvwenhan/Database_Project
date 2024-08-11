<template>
  <div class="BusinessMarket">
       <BusinessMarketTopbar @changeView="handleChangeView" />
       <div class="MarketContent">
       <component :is="currentView"></component>
       </div> 
  </div>
</template>

<script>
import BusinessMarketShow from './BusinessMarketShow.vue'
import BusinessMarketTopbar from './BusinessMarketTopbar.vue'

export default {
   name:'BusinessOrder',
   components: {
   BusinessMarketShow,
   BusinessMarketTopbar,
 },
 data() {
   return {
     currentView: 'BusinessMarketShow'
   }
 },
 methods: {
   handleChangeView(view) {
     this.currentView = view;
   }
 }
};
</script>
  
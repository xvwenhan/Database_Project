<!-- 商家页面组件 -->
<template>
   <div class="BusinessCommodity">
        <BusinessCommodityTopbar @changeView="handleChangeView" />
        <div class="CommodityContent">
        <component :is="currentView"></component>
        </div> 
   </div>
</template>

<script>
import BusinessCommodityShow from './BusinessCommodityShow.vue'
import BusinessCommodityTopbar from './BusinessCommodityTopbar.vue'

export default {
    name:'BusinessCommodity',
    components: {
    BusinessCommodityShow,
    BusinessCommodityTopbar,
  },
  data() {
    return {
      currentView: 'BusinessCommodityShow'
    }
  },
  methods: {
    handleChangeView(view) {
      this.currentView = view;
    }
  }
};
</script>



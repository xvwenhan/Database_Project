
<template>
  <div class="BusinessCommodity">
       <BusinessCommodityTopbar @changeView="handleChangeView" />
       <div class="CommodityContent">
       <component :is="currentView" ref="currentView"></component>
       </div> 
  </div>
</template>

<script> 
import BusinessCommodityShow from './BusinessCommodityShow.vue'
import BusinessCommodityTopbar from './BusinessCommodityTopbar.vue'

export default {
   name: 'BusinessCommodity',
   components: {  
       BusinessCommodityShow,
       BusinessCommodityTopbar,
   },
   data() {
       return {
           viewType: 1,
           currentView: 'BusinessCommodityShow'
       }
   },
   methods: {
       handleChangeView(viewType) {
           this.viewType = viewType;
           this.$nextTick(() => {
               if (this.$refs.currentView && this.$refs.currentView.handleChange) {
                   this.$refs.currentView.handleChange(viewType);
               } else {
                   console.error('handleChange method not found in currentView component');
               }
           });
       },
   }
};
</script>




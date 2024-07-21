  <!-- 商家页面组件 -->
  <template>
    <div class="BusinessOrder">
         <BusinessOrderTopbar @changeView="handleChangeView" />
         <div class="CommodityContent">
         <component :is="currentView"></component>
         </div> 
    </div>
 </template>
 
 <script>
 import BusinessOrderShow from './BusinessOrderShow.vue'
 import BusinessOrderTopbar from './BusinessOrderTopbar.vue'
 
 export default {
     name:'BusinessOrder',
     components: {
     BusinessOrderShow,
     BusinessOrderTopbar,
   },
   data() {
     return {
       currentView: 'BusinessOrderShow'
     }
   },
   methods: {
     handleChangeView(view) {
       this.currentView = view;
     }
   }
 };
 </script>
 
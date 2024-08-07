<template>
    <div class="CommodityTopbar">
      <button @click="changeView(0)" class="ButtonA">
        <span class="Buttondes">全部订单</span>
      </button> 
      <button @click="changeView(2)" class="ButtonA">
        <span class="Buttondes">待发货订单</span>
      </button>
      <button @click="changeView(3)" class="ButtonA">
        <span class="Buttondes">运输中订单</span>
      </button>
      <button @click="changeView(4)" class="ButtonA">
        <span class="Buttondes">已送达订单</span>
      </button>
      <button @click="changeView(5)" class="ButtonA">
        <span class="Buttondes">待退货订单</span>
      </button>
      <button @click="changeView(6)" class="ButtonA">
        <span class="Buttondes">已退货订单</span>
      </button>
    </div>
  </template>
  
  <script>
  export default {
    methods: {
      changeView(viewType) {
        this.$emit('changeView', viewType);
      }
    }
  };
  </script>

<style scoped>
.CommodityTopbar {
    /* color: white; */
    width:86%;
    background-color: #51a88e;
    height: 4vh;
    display: flex;
    position: fixed;
}

.ButtonA {
    width: 10%;
    height: 4vh; 
}

.ButtonA:hover {
    background-color: #51a88e;
}

.Buttondes {
    padding: 1vh;
}

</style>
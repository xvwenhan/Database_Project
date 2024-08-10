<template>
    <div class="CommodityTopbar">
      <button @click="changeView(1)" class="ButtonA">
        <span class="Buttondes">全部商品</span>
      </button>
      <button @click="changeView(2)" class="ButtonA">
        <span class="Buttondes">已售商品</span>
      </button>
      <button @click="changeView(3)" class="ButtonA">
        <span class="Buttondes">未售商品</span>
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
    width:86%;
    background-color: #51a88e;
    color: white;
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
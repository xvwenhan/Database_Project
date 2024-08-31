<!-- 管理员页面的头部 -->
<template>
  <header class="header">
    <div class="logo">
      <img src="@/assets_dxy/logo.png" alt="Logo" class="icon"/>
      <span style="font-size: 17px;">瑕宝阁</span>
    </div>
    <div class="user-info">
      <img src="@/assets_dxy/person_icon.svg" alt="Icon" class="icon">
      <span style="font-size: 17px;">管理员ID: {{ userId }}</span>
    </div>
  </header>
</template>

<script setup>

const userId =localStorage.getItem('userId');

</script>
  
<style scoped>
.header {
  background-color: #ffffff;
  padding: 10px;
  display: flex;
  height: 70px;
}

.logo {
  display: flex;
  align-items: center;
}

.icon {
  height: 30px;
  margin-right: 10px;
}

.user-info {
  margin-left: auto;
  display: flex;
  align-items: center;
}
</style>

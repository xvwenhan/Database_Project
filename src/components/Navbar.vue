<!-- 买家用户页面的头部 -->
<template>
    <nav class="navbar">
      <div class="line top-line"></div>
      <div class="navbar-top">
        <div class="navbar-brand">
          <img src="@/assets/logo.svg" alt="Logo" class="logo" />
          <span style="font-size: 17px;">旧时王谢堂前燕，飞入寻常百姓家</span>
        </div>
        <div class="navbar-right">
          <span style="font-size: 17px;">2024-07-08 星期一 农历六月初三 小暑 </span>
        </div>
      </div>
      <div class="line"></div>
      <div class="navbar-bottom">
        <ul class="navbar-menu">
          <li class="navbar-item" v-for="item in menuItems" :key="item.text" style="font-size: 22px;">
            <router-link :to="item.link" class="nav-link" active-class="active">{{ item.text }}</router-link>
          </li>
        </ul>
      </div>
      <div class="line bottom-line"></div>
    </nav>
  </template>
  
  <script setup>
  ///////////////////////////////////////////////////////////////学姐版本
  // import { reactive } from 'vue';
  // import { ElIcon } from 'element-plus'
  // import { Plus } from '@element-plus/icons-vue'  // 确保正确导入 Plus 图标
  

  ////////////////////////////////////////////////////////////广告页测试
  import { reactive, toRefs } from 'vue';
  import { ElIcon } from 'element-plus'
  import { Plus } from '@element-plus/icons-vue'  // 确保正确导入 Plus 图标
  import { defineProps } from 'vue';
  const props = defineProps({
  textColor: {
    type: String,
    default: '#333'
  }
});


  const menuItems = reactive([
    { text: "首页", link: "/home" },
    { text: "商品", link: "/merchandise/1" },
    { text: "市集", link: "/loginandregister" },//仅链接测试下
    // { text: "市集", link: "/bazaar" },
    { text: "论坛", link: "/forum" },
    { text: "购物车", link: "/cart" },
    { text: "订单中心", link: "/ordercentre" },
    { text: "个人中心", link: "/personalcentre" },
  ]);
  </script>
  
  <style scoped>
  * {
    box-sizing: border-box;
    margin: 0;
    padding: 0;
  }
  
  html, body {
    overflow-x: hidden; 
  }
  
  .navbar {
    width: 100%;
    background-color: #fff;
    position: relative;
  }
  
  .line {
    position: absolute;
    left: 0;
    width: 100vw;
    height: 1px;
    background-color: #e7e7e7;
  }
  
  .top-line {
    top: 0;
  }
  
  .bottom-line {
    bottom: 0;
  }
  
  .navbar-top,
  .navbar-bottom {
    max-width: 1200px;
    margin: 0 auto;
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 10px 20px;
  }
  
  .navbar-top {
    height: 60px;
    padding-left: 0px;
  }
  
  .navbar-bottom {
    height: 50px;
    justify-content: center;
  }
  
  .navbar-brand {
    display: flex;
    align-items: center;
  }
  
  .logo {
    height: 40px;
    margin-right: 10px;
  }
  
  .navbar-right {
    display: flex;
    align-items: center;
  }
  
  .icons i {
    margin-left: 10px;
    cursor: pointer;
  }
  
  .navbar-menu {
    list-style: none;
    display: flex;
    gap: 50px;
    margin: 0;
    padding: 0;
  }
  /* ////////////////////////////学姐之前版本 */
  /* .navbar-item .nav-link {
    text-decoration: none;
    color: #333;
    padding: 5px 10px;
  }
   */
   /* ///////////////////////////广告页测试 */
   .navbar-item .nav-link {
    text-decoration: none;
    color: inherit; /* 使用 inherit 以继承父级颜色 */
    padding: 5px 10px;
  }
  .navbar-item .nav-link.active {
    border-bottom: 2px solid red;
  }
  </style>
  
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
        <div class="navbar-search">
          <select class="search-type">
            <option value="product">商品</option>
            <option value="vendor">商家</option>
          </select>
          <input type="text" placeholder="搜索..." class="search-input"/>
          <button class="search-button" @click="handleSearch">搜索</button>
        </div>
      </div>
      <div v-if="loading" class="loading-overlay">
        <div class="loading-spinner"></div>
        <p>搜索中，请稍候...</p>
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
  import { reactive, ref } from 'vue';
  import { ElIcon } from 'element-plus'
  import { Plus } from '@element-plus/icons-vue'  // 确保正确导入 Plus 图标
  import { defineProps } from 'vue';

  import { useRouter } from 'vue-router';

  const router = useRouter();
  const loading = ref(false);
  const props = defineProps({
  textColor: {
    type: String,
    default: '#333'
  }
});


  const menuItems = reactive([
    { text: "首页", link: "/home" },
    { text: "商品", link: "/merchandise/1" },
    { text: "市集", link: "/bazaar" },
    { text: "论坛", link: "/forum" },
    { text: "收藏夹", link: "/cart" },
    { text: "订单中心", link: "/ordercentre" },
    { text: "个人中心", link: "/personalcentre" },
  ]);

  const handleSearch = async () => {
  loading.value = true;
  const searchType = document.querySelector('.search-type').value;
  const searchText = document.querySelector('.search-input').value;
  console.log('搜索类型:', searchType);
  console.log('搜索内容:', searchText);
  
  // 模拟一个异步请求，这里可以替换为真实的 API 调用
  setTimeout(() => {
    loading.value = false;
    // 根据搜索类型跳转到相应的页面
    if (searchType === 'product') {
      router.push(`/searchproductshowcase?query=${searchText}`);
    } else if (searchType === 'vendor') {
      router.push(`/merchantshowcase?query=${searchText}`);
    }
  }, 2000);
};
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
  .navbar-search {
  margin-left: 20px;
  height: 42px;
  display: flex;
  align-items: center;
  border: 2px solid orange; /* 添加橙色边框 */
  border-radius: 25px;
  padding: 5px;
}

.search-type {
  height: 35px;
  border: none;
  border-radius: 20px;
  padding: 0 10px;
  margin-right: 10px;
  background-color: white;
  color: #333;
  outline: none;
}

.search-input {
  height: 35px;
  border: none;
  outline: none;
  flex-grow: 1;
  margin-right: 10px;
  width: 100px;
}

.search-button {
  height: 35px;
  padding: 0 20px;
  background-color: orange;
  color: white;
  border: none;
  border-radius: 20px;
  display: flex;
  align-items: center;
  cursor: pointer;
}

.loading-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.loading-spinner {
  border: 5px solid #f3f3f3;
  border-radius: 50%;
  border-top: 5px solid #3498db;
  width: 50px;
  height: 50px;
  animation: spin 2s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}
  </style>
  
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
        <select class="search-type" v-model="searchType">
          <option value="product">商品</option>
          <option value="vendor">商家</option>
        </select>
        <input type="text" v-model="searchText" placeholder="搜索..." class="search-input"/>
        <button class="search-button" @click="handleSearch">搜索</button>
      </div>
    </div>
    <div class="line bottom-line"></div>
  </nav>
</template>

<script setup>
import { reactive, ref, onMounted, watch } from 'vue';
import { useRouter, useRoute } from 'vue-router';

const router = useRouter();
const route = useRoute();

const searchType = ref('product'); // 搜索类型
const searchText = ref(''); // 搜索关键字

const menuItems = reactive([
  { text: "首页", link: "/home" },
  { text: "商品", link: "/merchandise/1" },
  { text: "市集", link: "/bazaar" },
  { text: "论坛", link: "/forum" },
  { text: "收藏夹", link: "/cart" },
  { text: "订单中心", link: "/ordercentre" },
  { text: "个人中心", link: "/personalcentre" },
]);

onMounted(() => {
  // 从本地存储中恢复搜索类型和关键字
  searchType.value = localStorage.getItem('searchType') === '1' ? 'vendor' : 'product';
  searchText.value = localStorage.getItem('keyword') || '';
});

// 监听路由变化
watch(() => route.fullPath, () => {
  // 当路由变化时，从本地存储中恢复搜索类型和关键字
  searchType.value = localStorage.getItem('searchType') === '1' ? 'vendor' : 'product';
  searchText.value = localStorage.getItem('keyword') || '';
});

const handleSearch = async () => {
  console.log('搜索类型:', searchType.value);
  console.log('搜索内容:', searchText.value);

  // 保存搜索类型和关键字到本地存储
  localStorage.setItem('searchType', searchType.value === 'product' ? '0' : '1');
  localStorage.setItem('keyword', searchText.value);


  if (searchType.value === 'product') {
    if (router.currentRoute.value.path !== '/searchproductshowcase') {
      router.push(`/searchproductshowcase`);
    } else {
      router.push('/home').then(() => router.push(`/searchproductshowcase`));
    }
  } else if (searchType.value === 'vendor') {
    if (router.currentRoute.value.path !== '/merchantshowcase') {
      router.push(`/merchantshowcase`);
    } else {
      router.push('/home').then(() => router.push(`/merchantshowcase`));
    }
  }
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
</style>

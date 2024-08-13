<template>
  <nav class="navbar">
    <div class="line top-line"></div>
    <div class="navbar-top">
      <div class="navbar-brand">
        <img src="@/assets/logo.svg" alt="Logo" class="logo" />
        <span style="font-size: 17px;">旧时王谢堂前燕，飞入寻常百姓家</span>
      </div>
      <div class="navbar-right">
        <div class="date-weather">
          <span style="font-size: 17px;">{{ lunarDate }}</span>
          <span style="font-size: 17px; margin-left: 10px;">{{ weather }}</span>
        </div>
        <img src="@/assets/wy/profilephoto.jpg" alt="Profile Photo" class="profile-photo" @click="openModal"/>
      </div>
    </div>
    <div class="line"></div>
    <div class="navbar-bottom">
      <ul class="navbar-menu">
        <li class="navbar-item" v-for="item in menuItems" :key="item.text">
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
    <UserModal ref="userModal" />
  </nav>
</template>

<script setup>
import { reactive, ref, onMounted, watch } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import UserModal from '../views/UserCenter.vue';
import axios from 'axios';
import { getLunar } from 'chinese-lunar-calendar';
import chineseLunar from 'chinese-lunar';
// 引入UserModal组件
const userModal = ref(null);

// 打开模态框的函数
const openModal = () => {
  userModal.value.isUserCenterOpen = true;
};

const router = useRouter();
const route = useRoute();

const searchType = ref('product'); // 搜索类型
const searchText = ref(''); // 搜索关键字
const currentDate = ref('');
const weather = ref('');
const lunarDate = ref('');


const menuItems = reactive([
  { text: "首页", link: "/home" },
  { text: "商品", link: "/merchandise/1" },
  { text: "市集", link: "/bazaar" },
  { text: "论坛", link: "/forum" },
  { text: "收藏夹", link: "/cart" },
  { text: "订单中心", link: "/ordercentre" },
  // { text: "个人中心", link: "/personalcentre" },
]);



// 获取天气信息
const fetchWeather = async () => {
  try {
    const response = await axios.get('https://api.openweathermap.org/data/2.5/weather', {
      params: {
        q: 'Shanghai', // 这里可以替换成您想获取天气的城市
        appid: '5f274af243427c3098128d11ecd97cd9', // 您的OpenWeatherMap API key
        lang: 'zh_cn' // 使用中文语言
      }
    });
    weather.value = `${response.data.weather[0].description}`;
  } catch (error) {
    console.error('获取天气信息失败', error);
  }
};

onMounted(() => {
  // 从本地存储中恢复搜索类型和关键字
  searchType.value = localStorage.getItem('searchType') === '1' ? 'vendor' : 'product';
  searchText.value = localStorage.getItem('keyword') || '';

  const year = new Date().getFullYear();
  const month = new Date().getMonth() + 1;
  const date = new Date().getDate();

  // 获取完整的农历信息
  const lunarInfo = getLunar(year, month, date);
  console.log('Lunar Info:', lunarInfo); // 打印查看lunarInfo对象的内容

  // 使用适当的属性组合显示完整的农历信息
  lunarDate.value = `${lunarInfo.lunarYear}${lunarInfo.dateStr}`;
  console.log('农历:', lunarDate.value);

  fetchWeather();

  // 每小时更新日期和天气
  setInterval(() => {
    lunarDate.value = getLunar(year, month, date).dateStr;
  }, 3600000);
});

watch(() => route.fullPath, () => {
  searchType.value = localStorage.getItem('searchType') === '1' ? 'vendor' : 'product';
  searchText.value = localStorage.getItem('keyword') || '';
});

const handleSearch = async () => {
  console.log('搜索类型:', searchType.value);
  console.log('搜索内容:', searchText.value);
  
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
  width: 100%; /* 设置宽度为100% */
  margin: 0; /* 移除外边距 */
  padding-left: 300px; /* 导航栏文字 */
  background-color: #a61b29; /* 设置背景颜色为苋菜红 */
  display: flex;
  justify-content: flex-start;
  align-items: center;
  height: 60px;
}


.navbar-brand {
  display: flex;
  align-items: center;
  margin-left: 100px;
}

.logo {
  height: 40px;
  margin-right: 10px;
}

.navbar-right {
  display: flex;
  align-items: center;
  margin-right: 50px;
}

.date-weather {
  display: flex;
  align-items: center;
}

.profile-photo {
  width: 50px;
  height: 50px;
  border-radius: 50%; /* 使头像为圆形 */
  margin-left: 50px;
}


.icons i {
  margin-left: 10px;
  cursor: pointer;
}

.navbar-menu {
  margin-left: 20px;
  list-style: none;
  display: flex;
  /* gap: 50px; */
  margin: 0;
  padding: 0;
}

.navbar-item {
  position: relative;
}

.navbar-item .nav-link {
  text-decoration: none;
  color: white; /* 字体颜色白色 */
  font-size: 22px;
  padding: 20px 40px;
  transition: background-color 0.3s ease, color 0.3s ease;
  display: block;
  line-height: 20px; 
}

.navbar-item .nav-link.active,
.navbar-item .nav-link:hover {
  background-color: #f7f4ed; /* 鼠标悬停或点击时背景颜色 */
  color: #a61b29; /* 悬停或点击时字体颜色 */
  /* padding-left: 50px; 
  padding-right: 50px;
  margin-left: -25px; 
  margin-right: -25px; */
  z-index: 1; /* 提高 z-index 以覆盖其他元素 */
}




.navbar-search {
  margin-left: auto;
  height: 42px;
  display: flex;
  align-items: center;
  border: 2px solid orange; /* 确保边框颜色为橙色 */
  border-radius: 25px;
  padding: 5px;
  background-color: #ffffff; /* 确保背景色为白色 */
  box-shadow: none; /* 取消所有阴影效果 */
  margin-right: 30px;
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


<!-- 商家侧边栏顶侧栏页面组件 -->
<template>
    <div class="Sidebar">
      <ul>
        <li @click="$emit('changeView', 'BusinessHomePage')">
          <img src="@/assets/test.svg" alt="BusinessHomePage" class="SidebarIcon">
          <span>首页</span>
        </li>
        <li @click="$emit('changeView', 'BusinessOrder')">
          <img src="@/assets/test.svg" alt="BusinessOrder" class="SidebarIcon">
          <span>订单管理</span>
        </li>
        <li @click="$emit('changeView', 'BusinessCommodity')">
          <img src="@/assets/test.svg" alt="BusinessCommodity" class="SidebarIcon">
          <span>商品管理</span>
        </li>
        <li @click="$emit('changeView', 'BusinessMarket')">
          <img src="@/assets/test.svg" alt="BusinessMarket" class="SidebarIcon">
          <span>参与市集</span>
        </li>
      </ul>
    </div>

    <div class="Topbar">
        <span class="Website">
            <img src="@/assets/logo.png" alt="Website" id="WebsiteIcon">
            <span>旧时王谢堂前燕，飞入寻常百姓家</span>
        </span>
        <ul>
            <li @click="$emit('changeView', 'BusinessHomePage')">
                <img src="@/assets/home.svg" alt="HomePage" class="TopbarIcon">
            </li>
            <!-- <li @click="show = true">
                <img src="@/assets/setting.svg" alt="Setting"  id="Setting">
            </li> -->
            <li @click="showModal = true">
                <img src="@/assets/setting.svg" alt="Setting" id="Setting">
            </li>
        </ul>
        <div class="StoreScore">
      <span v-if="loading">Loading...</span>
      <span v-if="error">{{ error }}</span>
      <span v-if="storeScoreName">
        Store Score: {{ storeScoreName.storeScore }}
        Store Name: {{ storeScoreName.storeName }}
      </span>
    </div>
    </div>

    <div v-if="show" class="SettingPopUp">
      <div class="SettingContent">
        <span class="close" @click="closeModalT">&times;</span>
        <UserCenter />
      </div>
    </div>

    <div v-if="showModal" class="SettingPopUp">
      <div class="SettingContent">
        <span class="close" @click="closeModal">&times;</span>
        <BusinessSetting />
      </div>
    </div>
  </template>
  
  <script>
  import BusinessSetting from './BusinessSetting.vue';
  import axiosInstance from '../components/axios';
  export default {
  
    name: 'Businessnav',
    components:{
      BusinessSetting,
    },
    data() {
    return {
      showModal: false,
      show:false,
      storeScoreName: null,
      loading: false,
      error: null
    };
    
  },
  created() {
    this.fetchStoreScore();  // 在组件创建时调用方法获取店铺评分
  },
  methods: {
    closeModal() {
      this.showModal = false;
    },
    closeModalT() {
      this.show = false;
    },
    async fetchStoreScore() {
      const storeId =localStorage.getItem('userId');
      this.loading = true;
      this.error = null;
      this.storeScoreName = null;

      try {
        const response = await axiosInstance.get('/StoreFront/UpdateStoreScore', {
          params: { storeId }
        });
        this.storeScoreName = response.data;
        console.log('Store score:', response.data); // 控制台输出
      } catch (error) {
        this.error = 'Failed to fetch store score';
        console.error('Error fetching store score:', error);
      } finally {
        this.loading = false;
      }
    }
  }
  }
  </script>
  
  <style scoped>
  .Sidebar {
    width: 23vh;
    background-color: #367151;
    color: white;
    height: 94vh;
    position: fixed;
    bottom: 0;
    left: 0;
  }
  
  .Sidebar ul {
    padding: 0;
  }
  
  .Sidebar li {
    display: flex;
    /* 垂直中心 */
    align-items: center;
    padding: 1.5vh;
    cursor: pointer;
  }
  
  .SidebarIcon {
    margin-right: 1vh;
  }
  
  .Sidebar li:hover {
    background-color: #065f43;
  }

  .Topbar {
    width:100%;
    background-color: #6ca795;
    color: white;
    height: 6vh;
    display: flex;
    align-items: center;
    position: fixed;
    top: 0;
    left: 0;
  }
  
  .Website {
    display: flex;
    margin-left: 1.5vh;
  }

  #WebsiteIcon {
    width: 3vh; 
    height: 3vh; 
    margin-right: 1.5vh;
  }

  .Topbar ul {
    display: flex;
    list-style: none;
  }
  
  .Topbar li {
    cursor: pointer;
  }

  #Setting {
    width: 3vh; 
    height: 3vh; 
    margin: 1vh;
  }
  
  .TopbarIcon {
    width: 3vh; 
    height: 3vh; 
    margin: 1vh;
  }
  
  .Topbar li:hover {
    background-color: #0b8d77;
  }

  .SettingPopUp {
  position: fixed;
  z-index: 1;
  left: 0;
  top: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0,0,0,0.4);
  display: flex;
  justify-content: center;
  align-items: center;
}

.SettingContent {
  color: #065f43;
  background-color: #fefefe;
  display: inline-block;
  padding:2vh;
  position: relative;
}

.close {
  color: #aaa;
  float: right;
  font-size: 28px;
  font-weight: bold;
  z-index: 2;
  position: absolute;
  right: 10px;
  top: 10px;
}
 
.close:hover,
.close:focus {
  color: black;
  text-decoration: none;
  cursor: pointer;
}

  </style>
<!-- 商家侧边栏顶侧栏页面组件 -->
<template>
  <aside class="sidebar">
      <nav>
        <ul>
          <li>
            <router-link to="/BusinessHomePage" class="nav-link" active-class="active-link">
              <div class="button-content">
                <img src="@/assets/wy/homepage.png" alt="Icon2" class="sidebar-icon white-icon">
                <span>首页</span>
              </div>
            </router-link>
          </li>
          <li>
            <router-link to="/BusinessOrder" class="nav-link" active-class="active-link">
              <div class="button-content">
                <img src="@/assets/wy/order.png" alt="Icon3" class="sidebar-icon white-icon">
                <span>订单管理</span>
              </div>
            </router-link>
          </li>
          <li>
            <router-link to="/BusinessCommodity" class="nav-link" active-class="active-link">
              <div class="button-content">
                <img src="@/assets/wy/shop.png" alt="Icon4" class="sidebar-icon white-icon">
                <span>商品管理</span>
              </div>
            </router-link>
          </li>
          <li>
            <router-link to="/BusinessMarket" class="nav-link" active-class="active-link">
              <div class="button-content">
                <img src="@/assets_dxy/market_icon.svg" alt="Icon4" class="sidebar-icon white-icon">
                <span>参与市集</span>
              </div>
            </router-link>
          </li>
        </ul>
      </nav>
    </aside>
    <!-- <div class="Sidebar">
      <ul>
        <li @click="$emit('changeView', 'BusinessHomePage')">
          <div class="button-content">
            <img src="@/assets_dxy/certification_icon.svg" alt="BusinessHomePage" class="SidebarIcon">
            <span>首页</span>
          </div>
        </li>
        <li @click="$emit('changeView', 'BusinessOrder')">
          <img src="@/assets_dxy/certification_icon.svg" alt="BusinessOrder" class="SidebarIcon">
          <span>订单管理</span>
        </li>
        <li @click="$emit('changeView', 'BusinessCommodity')">
          <img src="@/assets_dxy/certification_icon.svg" alt="BusinessCommodity" class="SidebarIcon">
          <span>商品管理</span>
        </li>
        <li @click="$emit('changeView', 'BusinessMarket')">
          <img src="@/assets_dxy/certification_icon.svg" alt="BusinessMarket" class="SidebarIcon">
          <span>参与市集</span>
        </li>
      </ul>
    </div> -->

    <div class="Topbar">
        <span class="Website">
            <img src="@/assets/logo.png" alt="Website" id="WebsiteIcon">
            <span>瑕宝阁</span>
        </span>
        <ul>
            <!-- <li @click="$emit('changeView', 'BusinessHomePage')">
                <img src="@/assets/home.svg" alt="HomePage" class="TopbarIcon">
            </li> -->
            <!-- <li @click="show = true">
                <img src="@/assets/setting.svg" alt="Setting"  id="Setting">
            </li> -->
            <!-- <li @click="showModal = true">
                <img src="@/assets/setting.svg" alt="Setting" id="Setting">
            </li> -->
            <!-- <li @click="$emit('changeView', 'BusinessInformation')">
              <img src="@/assets/setting.svg" alt="Setting" id="Setting">
            </li> -->
            <li>
            <router-link to="/BusinessInformation">
              <img src="@/assets/setting.svg" alt="Setting" id="Setting">
            </router-link>
          </li>
        </ul>
        <div class="StoreScore">
      <span v-if="loading">Loading...</span>
      <span v-if="error">{{ error }}</span>
      <span v-if="storeScoreName">
        <!-- <img src="@/assets_dxy/person_icon.svg" alt="Icon" class="icon">
        Store Score: {{ storeScoreName.storeScore }}
        <img src="@/assets_dxy/person_icon.svg" alt="Icon" class="icon">
        Store Name: {{ storeScoreName.storeName }} -->
        <div class="HeadEnd">
        <img src="@/assets/wy/point.png" alt="Icon" class="icon" >
        <span id="TEnd1"> {{ storeScoreName.storeScore }}</span>
        <img :src="userimades.ima" alt="Icon" class="icon"id="TEnd2">
        <span id="TEnd1">{{ storeScoreName.storeName }}</span>
      </div>
      </span>

      <!-- <div class="HeadEnd">
        <img src="@/assets/wy/point.png" alt="Icon" class="icon" >
        <span id="TEnd1"> {{ storeScoreName.storeScore }}</span>
        <img :src="userimades.ima" alt="Icon" class="icon"id="TEnd2">
        <span id="TEnd1">{{ storeScoreName.storeName }}</span>
      </div> -->
    </div>
    <hr>
    </div>

    <!-- <div v-if="show" class="SettingPopUp">
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
    </div> -->
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
      storeScoreName: {},
      loading: false,
      error: null,
      //头像简介
      userimades:{
                ima:'',
                descri:'',
            },
    };
    
  },
  // created() {
  //   this.fetchStoreScore();  // 在组件创建时调用方法获取店铺评分
  // },
  methods: {
    closeModal() {
      this.showModal = false;
    },
    closeModalT() {
      this.show = false;
    },
     //获取头像简介
     async fetchImageAndText(id) {
            try {
                console.log(id,'!');
                const response = await axiosInstance.post('/UserInfo/GetPhotoAndDescribtion', {
                id
                });

                const { describtion, photo } = response.data;
                console.log('1:',this.userimades.ima);
                console.log('2:',this.userimades.descri);
                this.userimades.ima = `data:image/jpeg;base64,${photo}`;
                this.userimades.descri = describtion;

                console.log('获取到的头像和文字描述:', this.userimades);
            } catch (error) {
                console.error('获取头像和简介失败:', error);
                this.$message.error('获取头像和简介描述失败，请稍后再试');
            }
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
  },
  mounted() {          
        const userId = localStorage.getItem('userId');
        this.fetchStoreScore();
        this.fetchImageAndText(userId);
    }
  }
  </script>
  
  <style scoped>
  /* .Sidebar {
    width: 150px;
    background-color:#82111f;
    color: white;
    height: 100vh; */
    /* position: fixed; */
    /* bottom: 0;
    left: 0; */
    /* padding: 60px 0;
    display: flex;
  } */
  
  /* .Sidebar ul {
    padding: 0;
  }
  .Sidebar li {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 1.5vh;
    cursor: pointer;
  }
  
  .SidebarIcon {
    margin-left: 3vh;
    margin-right:3vh;
  }
  
  .Sidebar li:hover {
    background-color:#6e0e1a;
  } */
  

  .Topbar {
    background-color: rgb(237,227,228);
    color: rgb(11, 2, 2);
    height: 6vh;
    width: calc(100% - 150px);
    display: flex;
    align-items: center;
    position: fixed;
    top: 0;
    right: 0;
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

.sidebar {
    width: 150px;
    /* background-color: rgb(241,164,164); */
    /* background-color: rgb(171,79,79); */
    background-color:#82111f;
    color: #fff;
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 60px 0;
    height: 100vh;
  }
  
  .white-icon {
    filter: brightness(0) invert(1);
  }
  
  .button-content {
    display: flex;
    flex-direction: column;
    align-items: center;
  }
  
  nav ul {
    list-style: none;
    padding: 0;
  }
  
  nav li {
    width: 100%;
    text-align: center;
    margin: 0 0;
  }
  
  .nav-link {
    color: #ffffff;
    /* background-color: rgb(241,164,164); */
    /* background-color: rgb(171,79,79); */
    background-color:#82111f;
    border: none;
    text-decoration: none;
    display: flex;
    align-items: center;
    justify-content: center;
    width: 150px;
    height: 130px;
    padding: 10px 0;
    cursor: pointer;
  }
  
  .nav-link:hover {
    /* background: rgb(238,142,142); */
    /* background-color: rgb(180,69,69); */
    background-color:#6e0e1a;
  }
  
  .active-link {
    border-top: 1px solid #fff;
    border-bottom: 1px solid #fff;
  }
  #TEnd1 {
    margin-right: 80px;
  }
  #TEnd2 {
    width: 30px;
    height: 30px;
    border-radius: 20px;
    margin-right: 10px;
  }
  .HeadEnd {
    margin-left: 800px;
    display: flex;
    align-items: center;
  }

  </style>
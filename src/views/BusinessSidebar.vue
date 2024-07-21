<!-- 商家页面组件 侧边栏以及头部-->
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
            <li @click="showModal = true">
                <img src="@/assets/setting.svg" alt="Setting" id="Setting">
            </li>
        </ul>
    </div>

    <div v-if="showModal" class="SettingPopUp">
      <div class="SettingContent">
        <span class="close" @click="showModal = false">&times;</span>
        <p>这里是弹窗内容</p>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'Businessnav',
    data() {
    return {
      showModal: false
    };
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
    list-style: none;
    padding: 0;
  }
  
  .Sidebar li {
    display: flex;
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
    object-fit: cover;  
    margin-right: 1.5vh;
  }

  .Topbar ul {
    display: flex;
    list-style: none;
    margin:0;
  }
  
  .Topbar li {
    cursor: pointer;
  }

  #Setting {
    width: 3vh; 
    height: 3vh; 
    object-fit: cover; 
    margin: 1vh;
  }
  
  .TopbarIcon {
    width: 3vh; 
    height: 3vh; 
    object-fit: cover; 
    margin: 1vh;
  }
  
  .Topbar li:hover {
    background-color: #0b8d77;
  }

  .SettingPopUp {
  display: block;
  position: fixed;
  z-index: 1;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  overflow: auto;
  background-color: rgba(0,0,0,0.4);
}

.SettingContent {
  background-color: #fefefe;
  margin: 15% auto;
  padding: 20px;
  border: 1px solid #888;
  width: 80%;
}

.close {
  color: #aaa;
  float: right;
  font-size: 28px;
  font-weight: bold;
}

.close:hover,
.close:focus {
  color: black;
  text-decoration: none;
  cursor: pointer;
}
  </style>
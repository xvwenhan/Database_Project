<template>
  <Navbar />
    <div class="main-container">
      <div class="main-content">
        <div class="shop-title">          
          <div class="shop-info">
              <div class="shop-name">
                  <span>{{shopinfo.storeName}}</span>
              </div>
              <div class="other-info">
                  <span>本店评分：{{shopinfo.storeScore}}</span>
                  <span>本店地址：{{shopinfo.Address}}</span>
              </div>
          </div>
          <div class="favoriteButton">
            <el-button @click="clickFavorite" >{{ isFavorite ? '已收藏' : '收藏' }}</el-button>
          </div>
        </div>
  
        <div class="shop-content">
          <div class="sidebar-category">
            <div role="tree" class="sidebar">
              <div
                v-for="category in categories"
                :key="category.id"
                :id="'category-' + category.id"
                role="treeitem"
                tabindex="0"
                :class="{ selected: selectedCategory === category.id }"
                class="category"
                @click="filterProducts(category,'Sider')"
              >
                {{ category.name }}
              </div>
            </div>
          </div>
          <div class="product-display">
            <div class="search-box">
              <el-input
                placeholder="搜索本店商品"
                v-model="searchQuery"
              >
              </el-input>
              <el-button @click="filterProducts(null,'Button')">搜索</el-button>
            </div>
            <div class="product">
              <!-- 根据后端数据展示商品 -->
            </div>
          </div>
        </div>
      </div>
    </div>
</template>
  
<script setup>
  import { ref } from 'vue';
  import Navbar from '../components/Navbar.vue';
  import { ElButton, ElMessage, ElInput } from 'element-plus';
  import 'element-plus/dist/index.css';
  
  //测试数据（待测试接口：根据店铺id查询店铺信息、全部商品信息、店铺自定义分类信息；店铺内搜索商品、店铺内根据分类筛选商品）
  const shopinfo = ref({storeId:"123",storeName:"测试店名",storeScore:4.9,Address:"上海"});
  const isFavorite = ref(0);
  const categories = ref([
    { id: 1, name: '全部商品' },
    { id: 2, name: '分类二' },
    { id: 3, name: '分类三' },
    { id: 4, name: '分类四' }
  ]);
  
  const clickFavorite = () => {
    // 此处发送请求到服务器更新收藏状态
    isFavorite.value = !isFavorite.value;
    if(isFavorite.value==1){
      ElMessage.success("收藏成功")
    }
    else{
      ElMessage.success("取消收藏成功")
    }
  };
  
  const searchQuery = ref('');
  const selectedCategory = ref(1);
  const filterProducts = (category, source) => {
    if(source=='Button'){
      console.log('搜索内容:', searchQuery.value);
      //搜索商品
    }
    else if(source=='Sider'){
      selectedCategory.value = category.id;
      console.log(`分类 ${category.name} 被点击`);
      //检测侧边栏内容进行筛选商品
    }
  }
  
  </script>
  
  <style scoped>
  
  .main-container{
    align-items: center;
    background-color: #f4f6f8;
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    min-height: 100vh;
    padding-bottom: 48px;
    position: relative;
  }
  
  .main-content{
    margin-top: 20px;
    max-width: 1200px;
    width:80%;
    justify-content: center;
    box-sizing: border-box;
  }
  
  .shop-title{
    background-color: #fff;
    border-radius: 16px;
    box-sizing: border-box;
    display: flex;
    justify-content: space-between;
    padding: 16px 20px 20px;
    width: 100%;
  }
  
  .shop-info{
    flex-direction: column;
  }
  
  .shop-name{
    font-size: 24px;
    font-weight: bold;
    padding: 5px 0;
  }
  
  .other-info{
    font-size: 15px;
    padding: 5px 0;
    display: flex;
    flex-direction: column;
  }
  
  .favoriteButton{
    justify-self: end;
    align-self: end;
  }
  
  .shop-content{
    align-items: flex-start;
    display: flex;
    justify-content: space-between;
    width: 100%;
  }
  
  .sidebar-category{
    background-color: #fff;
    border-radius: 16px;
    box-sizing: border-box;
    min-height: 70vh;
    margin-top: 16px;
    /* overflow-x: hidden;
    overflow-y: scroll; */
    padding: 8px;
    position: sticky;
    top: 10px;
    width: 20%;
  }
  
  .product-display{
    background-color: #fff;
    border-radius: 16px;
    box-sizing: border-box;
    margin-top: 16px;
    margin-left: 16px;
    padding: 24px 0 0;
    width: 100%;
    min-height: 70vh;
  }
  
  .search-box{
    width: 45%;
    margin-left:50%;  
    display: flex;
    gap: 10px;
  }
  
  .sidebar {
    background-color: #fff;
    padding: 8px;
    border-radius: 8px;
  }
  
  .category {
    font-size: 16px;
    padding: 8px 12px;
    margin-bottom: 8px;
    cursor: pointer;
    border-radius: 4px;
    transition: background-color 0.2s;
  }
  
  .category.selected {
    background-color: #bbd0ed;
    font-weight: bold;
    color:#7495c3;
  }
  
  </style>
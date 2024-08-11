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
              <div class="display-items">
                <div 
                  v-for="product in products" 
                  :key="product.productId" 
                  class="product-item"
                  @click="handleProductClick(product.productId)"
                >
                  <img :src="product.productPic" :alt="product.productId" class="product-image" />
                  <div class="product-text">
                    <h2>{{ product.productName }}</h2>
                    <p>价格: ¥{{ product.productPrice }}</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
</template>
  
<script setup>
import { reactive, ref, onMounted  } from 'vue';
import { useRouter } from 'vue-router';
import Navbar from '../components/Navbar.vue';
import { ElButton, ElMessage, ElInput } from 'element-plus';
import 'element-plus/dist/index.css';
import axiosInstance from '../components/axios';

const router = useRouter();

//测试数据（待测试接口：全部商品信息、店铺内搜索商品、店铺内根据分类筛选商品）
//S1234567测试账号
const shopinfo = reactive({storeId:"",storeName:"",storeScore:0,Address:"默认地址"});
const isFavorite = ref(0);
const categories = ref([
  { id: 1, name: '全部商品' },
]);
const products = reactive([]);
  
const searchQuery = ref('');
const selectedCategory = ref(1);
const filterProducts = (category, source) => {
  if(source=='Button'){
    console.log('搜索内容:', searchQuery.value);
    //搜索商品
    fetchProductsBySearch(searchQuery.value)
  }
  else if(source=='Sider'){
    selectedCategory.value = category.id;
    console.log(`分类 ${category.name} 被点击`);
    //检测侧边栏内容进行筛选商品
    if(selectedCategory.value==1){
      fetchAllProducts();
    }
    else{
      fetchProductsByTag(category.name);
    }
  }
}

const tags = ref([]);
const message = ref('');
const fetchTags = async () => {
  try {
    const response = await axiosInstance.get('/Shopping/GetStoreTags', {
      params: {
        storeId: shopinfo.storeId
      }
    });
    tags.value = response.data;
    message.value = '已获取自定义分类';
    addCategory(); 
  } catch (error) {
    if (error.response) {
      message.value = error.response.data;
    } else {
      message.value = '获取分类数据失败';
    }
  }
  console.log(message.value);
};

const addCategory = () => {
  tags.value.forEach((categoryName) => {
    categories.value.push({
      id: categories.value.length + 1,
      name: categoryName,
    });
  });
  console.log(categories.value);

};

const handleProductClick = (productId) => {
  localStorage.setItem('productIdOfDetail',productId);
  router.push('/productdetail');
};


const message1 = ref('');
const fetchStoreInfo = async () => {
  try {
    const response = await axiosInstance.get('/Shopping/GetStoreInfo', {
      params: {
        storeId: shopinfo.storeId
      }
    });
    shopinfo.storeName = response.data.name;
    shopinfo.storeScore = response.data.score;
    message1.value = '已获取店铺信息';
  } catch (error) {
    if (error.response) {
      message1.value = error.response.data;
    } else {
      message1.value = '获取店铺信息失败';
    }
  }
  console.log(message1.value);
};

const message2 = ref('');
const fetchIsBookmarked = async () => {
  try {
    const response = await axiosInstance.get('/Favourite/IsStoreBookmarked', {
      params: {
        userId:'U6210129',
        storeId: shopinfo.storeId
      }
    });
    isFavorite.value = response.data;
    message2.value = '已获取收藏信息';
  } catch (error) {
    if (error.response) {
      message2.value = error.response.data;
    } else {
      message2.value = '获取收藏信息失败';
    }
  }
  console.log(message2.value);
};


const message3= ref('');
const fetchAllProducts = async () => {
  try {
    const response = await axiosInstance.get('/StoreViewProduct/GetProductsByStoreIdAndViewType', {
      params: {
        storeId: shopinfo.storeId,
        ViewType: 1
      }
    });
    if (products.length > 0) {
      products.splice(0, products.length);
    }
    response.data.forEach(product => {
      product.productPic = `data:image/png;base64,${product.productPic}`;
      products.push(product);
    });
    message3.value = '已获取全部商品信息';
  } catch (error) {
    if (error.response) {
      message3.value = error.response.data;
    } else {
      message3.value = '获取全部信息失败';
    }
  }
  console.log(message3.value);
};

const clickFavorite = () => {
  if(isFavorite.value==false){
    bookmarkStore();
  }
  else{
    unbookmarkStore();
  }
};

const message4 = ref('');
const bookmarkStore = async () => {
  try {
    const response = await axiosInstance.post('/Favourite/BookmarkStore',  {
      params: {
        userId: 'U6210129',
        storeId: shopinfo.storeId
      }
    });
    message4.value = response.data;
  } catch (error) {
    if (error.response) {
      message4.value = error.response.data;
    } else {
      message4.value = '操作失败';
    }
  }
  if(message4.value){
    ElMessage.info(message4.value);
  }
  fetchIsBookmarked();
};

const message5 = ref('');
const unbookmarkStore = async () => {
  try {
    const response = await axiosInstance.delete('/Favourite/UnbookmarkStore',  {
      params: {
        userId: 'U6210129',
        storeId: shopinfo.storeId
      }
    });
    message5.value = response.data;
  } catch (error) {
    if (error.response) {
      message5.value = error.response.data;
    } else {
      message5.value = '操作失败';
    }
  }
  if(message5.value){
    ElMessage.info(message5.value);
  }
  fetchIsBookmarked();
};


const message6= ref('');
const fetchProductsByTag = async (tag) => {
  try {
    const response = await axiosInstance.get('/StoreViewProduct/searchByStoreTag', {
      params: {
        storeId: shopinfo.storeId,
        storeTag: tag
      }
    });
    if (products.length > 0) {
      products.splice(0, products.length);
    }
    response.data.forEach(product => {
      product.productPic = `data:image/png;base64,${product.productPic}`;
      products.push(product);
    });
    message6.value = '已获取'+tag+'分类商品信息';
  } catch (error) {
    if (error.response) {
      message6.value = error.response.data;
    } else {
      message6.value = '获取分类商品信息失败';
    }
  }
  console.log(message6.value);
};


const message7= ref('');
const fetchProductsBySearch = async (word) => {
  try {
    const response = await axiosInstance.get('/StoreViewProduct/search', {
      params: {
        storeId: shopinfo.storeId,
        keyword: word
      }
    });
    if (products.length > 0) {
      products.splice(0, products.length);
    }
    response.data.forEach(product => {
      product.productPic = `data:image/png;base64,${product.productPic}`;
      products.push(product);
    });
    message7.value = '已获取搜索商品信息';
  } catch (error) {
    if (error.response) {
      message7.value = error.response.data;
    } else {
      message7.value = '获取搜索商品信息失败';
    }
  }
  console.log(message7.value);
};

onMounted(() => {
  shopinfo.storeId = localStorage.getItem('storeIdOfDetail');
  console.log(shopinfo.storeId);
  fetchIsBookmarked();
  fetchStoreInfo();
  fetchTags();
  fetchAllProducts();
});
  
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
  overflow: auto;
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
  align-items: flex-start;
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

.product {
  margin-top: 20px;
  margin-left: 20px;
  margin-bottom: 20px;
  display: flex;
  width: 100%;
}


.display-items {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
}

.product-item {
  background-color: #fff;
  padding: 20px;
  border-radius: 5px;
  box-shadow: 0 2px 5px rgba(0,0,0,0.1);
  transition: transform 0.3s, box-shadow 0.3s;
  display: grid;
  width: 221px;
  height: 274px;
}


.product-item:hover {
  transform: translateY(-2px);
  /* box-shadow: 0 4px 10px rgba(0,0,0,0.15); */
}

.product-image {
  width: 100%;
  height: auto;
  max-height: 150px;
  object-fit: cover;
  border-radius: 5px;
  margin-bottom: 10px;
}

.product-item h2 {
  font-size: 18px;
  margin: 0 0 10px;
}

.product-item p {
  font-size: 16px;
  margin: 0 0 10px;
}

.product-text{
  align-self: end;
}
  
  </style>
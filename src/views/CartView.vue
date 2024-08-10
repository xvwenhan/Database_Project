<!-- 买家用户的收藏夹页面 -->
<template>
    <Navbar />
    <div class="main-container">
        <aside class="sidebar-category">
            <div role="tree" class="sidebar">
                <div
                v-for="category in categories"
                :key="category.id"
                :id="'category-' + category.id"
                role="treeitem"
                tabindex="0"
                :class="{ selected: selectedCategory === category.id }"
                class="category"
                @click="filter(category)"
                >
                {{ category.name }}
                </div>
            </div>
        </aside>
        <div class="display">
            <div v-if="selectedCategory==1" >
                <div class="display-items">
                    <div v-for="product in paginatedProducts" :key="product.productId" class="product-item">
                        <!-- 展示格式待替换 -->
                        <p>{{ product.productId }}</p>
                        <p>价格: ¥{{ product.productPrice }}</p>
                    </div>
                </div>
                <div class="pagination">
                    <button @click="productPageChange(currentPage1 - 1)" :disabled="currentPage1 === 1">上一页</button>
                    <span>第 {{ currentPage1 }} 页 / 共 {{ productsPages }} 页</span>
                    <button @click="productPageChange(currentPage1 + 1)" :disabled="currentPage1 === productsPages">下一页</button>
                </div>
            </div>
            <div v-else>
                <div class="display-items">
                    <div v-for="store in paginatedStores" :key="store.storeId" class="store-item">
                        <!-- 展示格式待替换 -->
                        <p>{{ store.storeId }}</p>
                        <p>{{ store.storeName }}</p>
                        <p>店铺评分{{ store.storeScore }}</p>
                    </div>
                </div>
                <div class="pagination">
                    <button @click="storePageChange(currentPage2 - 1)" :disabled="currentPage2 === 1">上一页</button>
                    <span>第 {{ currentPage2 }} 页 / 共 {{ storesPages }} 页</span>
                    <button @click="storePageChange(currentPage2 + 1)" :disabled="currentPage2 === storesPages">下一页</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, reactive, computed } from 'vue';
import Navbar from '../components/Navbar.vue';
import 'element-plus/dist/index.css';
import axiosInstance from '../components/axios';

const categories = ref([
    { id: 1, name: '收藏商品' },
    { id: 2, name: '收藏店铺' },
]);

const selectedCategory = ref(1);
const filter = (category) => {
    selectedCategory.value = category.id;
    console.log(` ${category.name} 被点击`);
    if(selectedCategory==1){
        fetchProducts();
    }
    else{
        fetchStores();
    }
}

//////商品相关
const Products = reactive([]);
const message01 = ref('');
const fetchProducts = async () => {
  try {
    const response = await axiosInstance.post('/Favourite/GetFavoriteProducts', {
      "userId": "U6210129" //测试账号
    });
    Products.splice(0, Products.length, ...response.data);
    message01.value = '已获取收藏商品数据';
    console.log('收藏商品数据：'+Products.values);
  } catch (error) {
    if (error.response) {
      message01.value = error.response.data;
    } else {
      message01.value = '获取数据失败';
    }
  }
  console.log(message01.value);
};

onMounted(() => {
    fetchProducts();
});

// 设置表格页面大小及当前页数
const pageSize1 = ref(1);
const currentPage1 = ref(1);

const totalProducts = computed(() => Products.length);
const productsPages = computed(() => Math.ceil(totalProducts.value / pageSize1.value));

// 处理并分页后的商品数据
const paginatedProducts = computed(() => {
  const start = (currentPage1.value - 1) * pageSize1.value;
  const end = start + pageSize1.value;
  return Products.slice(start, end);
});

// 切换页面
const productPageChange = (page) => {
    if (page >= 1 && page <= productsPages.value) {
        currentPage1.value = page;
    }
};


///// 店铺相关
const Stores = reactive([]);
const message02 = ref('');
const fetchStores = async () => {
  try {
    const response = await axiosInstance.post('/Favourite/GetFavoriteStores', {
      "userId": "U6210129" //测试账号
    });
    Stores.splice(0, Stores.length, ...response.data);
    message02.value = '已获取收藏店铺数据';
    console.log('收藏店铺数据：'+Stores.values);
  } catch (error) {
    if (error.response) {
      message02.value = error.response.data;
    } else {
      message02.value = '获取数据失败';
    }
  }
  console.log(message02.value);
};

// 设置表格页面大小及当前页数
const pageSize2 = ref(1);
const currentPage2 = ref(1);

const totalStores = computed(() => Stores.length);
const storesPages = computed(() => Math.ceil(totalStores.value / pageSize2.value));

// 处理并分页后的商品数据
const paginatedStores = computed(() => {
  const start = (currentPage2.value - 1) * pageSize2.value;
  const end = start + pageSize2.value;
  return Stores.slice(start, end);
});

// 切换页面
const storePageChange = (page) => {
    if (page >= 1 && page <= storesPages.value) {
        currentPage2.value = page;
    }
};

</script>

<style scoped>
  
.main-container{
  /* align-items: center;
  background-color: #f4f6f8;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  min-height: 100vh;
  padding-bottom: 48px;
  position: relative; */
  
  display: flex;
  height: 100vh;
}

.main-content{
  margin-top: 20px;
  width:100%;
  justify-content: center;
  box-sizing: border-box;
  align-items: flex-start;
  display: flex;
  justify-content: space-between;
}

.sidebar-category{
  /* background-color: #fff;
  border-radius: 16px;
  box-sizing: border-box;
  min-height: 70vh;
  margin-top: 16px;
  padding: 8px;
  position: sticky;
  top: 10px;
  width: 20%; */
  flex: 0 0 200px;
  background-color: #f8f8f8;
  padding: 20px;
  box-shadow: 2px 0 5px rgba(0,0,0,0.1);
}

.display{
  background-color: #fff;
  border-radius: 16px;
  box-sizing: border-box;
  margin-top: 16px;
  margin-left: 16px;
  padding: 24px 0 0;
  width: 100%;
  min-height: 70vh;
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
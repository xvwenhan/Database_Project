<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axiosInstance from '../components/axios';
import Navbar from '../components/Navbar.vue';
import { useRouter} from 'vue-router';

const router = useRouter();

const products = ref([]);
const loading = ref(true);  // 用于控制缓冲页面的显示
const errorMessage = ref(''); // 错误信息

const fetchStores = async (keyword: string, type: string) => {
  try {
    const response = await axiosInstance.get('/NaviSearch/search', {
      params: {
        keyword: keyword,
        type: type
      }
    });
    console.log('返回数据', response.data);

    if (response.data && response.data.length > 0) {
      // products.value = response.data;
      products.value = Array(100).fill(response.data).flat(); // 将数据重复100次并平展成一个数组
      errorMessage.value = ''; // 清除错误信息
    } else {
      products.value = [];
      errorMessage.value = '你搜索的宝贝不存在...'; // 设置错误信息
    }
  } catch (error) {
    console.error('Error fetching stores:', error);
    errorMessage.value = '你搜索的宝贝不存在...'; // 设置错误信息
  } finally {
    loading.value = false;  // 数据获取完毕后关闭缓冲页面
  }
};

const goToProductDetail = (productId: string) => {
  // console.log('Selected Store ID:', productId);
  localStorage.setItem('productIdOfDetail', productId);  // 存储 productId
  console.log('跳转至 /productdetail');
  router.push('/productdetail');  // 跳转到商品详情页
};

onMounted(() => {
  const keyword = localStorage.getItem('keyword') || '';
  const type = localStorage.getItem('searchType') || '0';
  fetchStores(keyword, type);
});
</script>

<template>
  <Navbar />
  <!-- <div v-if="loading" class="loading-overlay">
    <div class="loading-spinner"></div>
    <p>搜索中，请稍候...</p>
  </div> -->
  <div 
  style="background-color: #f7f4ed;height: 100%;overflow-x: hidden;"
  >
    <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
    <div v-else class="product-display">
      <div v-for="product in products" :key="product.productId" class="product-item" @click="goToProductDetail(product.productId)">
        <img :src="'data:image/png;base64,' + product.productPic" :alt="product.productName" class="product-image" />
        <div class="product-info">
          <p class="product-price">
            <span class="special-price">价格</span> ¥{{ product.productPrice }}
          </p>
          <h2 class="product-name">【{{ product.productName }}】{{ product.description }}</h2>
        </div>
      </div>
      
    </div>
  </div>
</template>

<style scoped>
.product-display {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
  padding-top: 20px;
  padding-left: 100px;
  padding-right: 100px;
  
}

.product-item {
  width: calc(25% - 20px); /* 每行显示四个商品，减去间隙 */
  padding: 20px;
  border: 1px solid #e7e7e7;
  border-radius: 10px;
  background-color: #fff;
  display: flex;
  flex-direction: column;
  align-items: center;
  cursor: pointer; /* 使鼠标在悬停时变成手型指针 */
  transition: border-color 0.3s ease; /* 添加平滑的边框颜色变化效果 */
}

.product-item:hover {
  border-color: #a61b29; /* 悬停时的边框颜色 */
}

.product-image {
  width: 100%;
  height: 200px;
  object-fit: cover;
  border-radius: 10px;
  margin-bottom: 10px;
}

.product-info {
  text-align: center;
}

.product-name {
  font-size: 16px;
  color: #333;
  margin-bottom: 10px;
}

.product-price {
  font-size: 18px;
  color: #a61b29;
  margin-bottom: 10px;
}

.special-price {
  background-color: #a61b29;
  color: #fff;
  padding: 2px 5px;
  border-radius: 5px;
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
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.error-message {
  text-align: center;
  font-size: 24px;
  color: #e60012;
  margin: 20px;
}
</style>

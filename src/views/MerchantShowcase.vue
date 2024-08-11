<template>
  <Navbar />
  <div v-if="loading" class="loading-overlay">
    <div class="loading-spinner"></div>
    <p>搜索中，请稍候...</p>
  </div>
  <div v-else>
    <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
    <div v-else class="store-page">
      <div v-for="store in stores" :key="store.storeId" class="store-container">
        <div class="store-content">
          <div class="store-header">
            <img :src="'data:image/png;base64,' + store.storePhoto.result" alt="Store Avatar" class="store-avatar" />
            <div class="store-info">
              <h2 class="store-name">{{ store.storeName }}</h2>
              <p class="store-rating">好评率: {{ (store.storeScore * 100).toFixed(2) }}%</p>
            </div>
          </div>
          <div class="store-products">
            <div v-for="product in store.homeProducts" :key="product.productId" class="product-item">
              <img :src="'data:image/png;base64,' + product.productPic" alt="Product Image" class="product-image" />
              <p class="product-price">¥{{ product.productPrice }}</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import axiosInstance from '../components/axios';
import Navbar from '../components/Navbar.vue';

const stores = ref([]);
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
      stores.value = response.data;
      errorMessage.value = ''; // 清除错误信息
    } else {
      stores.value = [];
      errorMessage.value = '你搜索的商家不存在...'; // 设置错误信息
    }
  } catch (error) {
    console.error('Error fetching stores:', error);
    errorMessage.value = '你搜索的商家不存在...'; // 设置错误信息
  } finally {
    loading.value = false;  // 数据获取完毕后关闭缓冲页面
  }
};

onMounted(() => {
  const keyword = localStorage.getItem('keyword') || '';
  const type = localStorage.getItem('searchType') || '1';
  fetchStores(keyword, type);
});
</script>

<style scoped>
.store-page {
  display: flex;
  flex-direction: column;
  gap: 20px;
  padding: 20px;
  align-items: center; /* 居中对齐 */
}

.store-container {
  border: 1px solid #e7e7e7;
  border-radius: 10px;
  padding: 20px;
  width: 80%; /* 调整宽度 */
  max-width: 1200px; /* 限制最大宽度 */
}

.store-content {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  width: 100%; /* 使用全宽 */
}

.store-header {
  display: flex;
  align-items: center; /* 头像和名称居中对齐 */
  gap: 10px; /* 调整头像和店铺名称之间的间距 */
  margin-bottom: 20px;
}

.store-avatar {
  width: 60px; /* 头像大小 */
  height: 60px; /* 头像大小 */
  border-radius: 50%; /* 将头像裁剪为圆形 */
  object-fit: cover;
}

.store-info {
  display: flex;
  flex-direction: column;
}

.store-name {
  font-size: 24px; /* 调整为更大的字体 */
  font-weight: bold;
  margin: 0;
}

.store-rating {
  font-size: 18px; /* 调整为更大的字体 */
  color: #888;
}

.store-products {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
}

.product-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  width: 150px;
}

.product-image {
  width: 150px; /* 固定图片宽度 */
  height: 150px; /* 固定图片高度 */
  object-fit: cover;
  border-radius: 5px;
}

.product-price {
  font-size: 16px;
  color: #333;
  margin-top: 5px;
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

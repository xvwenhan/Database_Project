<template>
  <Navbar />
  <div class="carousel-container">
    <button class="carousel-button left" @click="prevSlide">
      <el-icon :size="20"><ArrowLeftBold /></el-icon>
    </button>
    <div class="carousel-slide" :style="slideStyle">
      <div v-for="(market, index) in markets" :key="index" class="carousel-item">
        <h1>{{ market.theme }}</h1>
        <div class="image-container">
          <img :src="'data:image/png;base64,' + market.posterImg" class="carousel-image" @click="goToMerchantShowcase(market.marketId)" />
          <div class="date-overlay">截至：{{ formatDate(market.endTime) }}</div>
        </div>
      </div>
    </div>
    <button class="carousel-button right" @click="nextSlide">
      <el-icon :size="20"><ArrowRightBold /></el-icon>
    </button>
  </div>
</template>

<script setup lang="ts">
import Navbar from '../components/Navbar.vue';
import { ref, onMounted, computed } from 'vue';
import router from '@/router';
import { ArrowLeftBold, ArrowRightBold } from '@element-plus/icons-vue';
import axiosInstance from '../components/axios';

const markets = ref([]);
const currentIndex = ref(0);

const fetchMarkets = async () => {
  try {
    const response = await axiosInstance.get('/Administrator/GetAllMarket');
    console.log('返回数据', response.data);
    markets.value = response.data;
  } catch (error) {
    console.error('Error fetching markets:', error);
  }
};

onMounted(() => {
  fetchMarkets();
});

const prevSlide = () => {
  currentIndex.value = (currentIndex.value - 1 + markets.value.length) % markets.value.length;
};

const nextSlide = () => {
  currentIndex.value = (currentIndex.value + 1) % markets.value.length;
};

const goToMerchantShowcase = (marketId) => {
  // 将marketId存入localStorage
  localStorage.setItem('selectedMarketId', marketId);
  // 跳转到市集商品页面
  router.push('/bazaarmerchandise');
};

const slideStyle = computed(() => ({
  transform: `translateX(-${currentIndex.value * 100}%)`
}));

const formatDate = (dateString) => {
  const options = { year: 'numeric', month: 'numeric', day: 'numeric' };
  return new Date(dateString).toLocaleDateString(undefined, options);
};
</script>

<style scoped>
.carousel-container {
  position: relative;
  width: 100%;
  overflow: hidden;
  max-width: 1420px;
  margin: 0 auto;
}

.carousel-slide {
  display: flex;
  transition: transform 0.5s ease;
}

.carousel-item {
  width: 100%;
  flex-shrink: 0;
  text-align: center;
  padding: 20px;
  box-sizing: border-box;
}

.image-container {
  position: relative;
  width: 100%;
  height: 650px; /* 固定高度 */
}

.carousel-image {
  width: 100%;
  height: 100%;
  object-fit: cover; /* 使图片适应容器 */
  cursor: pointer;
}

.date-overlay {
  position: absolute;
  bottom: 10px;
  right: 10px;
  background-color: rgba(0, 0, 0, 0.5);
  color: white;
  padding: 5px;
  font-size: 12px;
}

.carousel-button {
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  background-color: rgba(0, 0, 0, 0.5);
  color: white;
  border: none;
  padding: 20px;
  cursor: pointer;
  z-index: 10;
  border-radius: 50%;
}

.carousel-button.left {
  left: 10px;
}

.carousel-button.right {
  right: 10px;
}

.carousel-button:hover {
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
}
</style>
